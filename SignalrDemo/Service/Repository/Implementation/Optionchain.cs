using Newtonsoft.Json;
using SignalrDemo.Service.Repository.Interface;
using System.Net;
using SignalrDemo.Helper;
using UrlHelper = SignalrDemo.Helper.UrlHelper;
using SignalrDemo.Models.ResponseModel;
using SignalrDemo.Service.UnitofWork;
using SignalrDemo.DbEntity;

namespace SignalrDemo.Service.Repository.Implementation
{
    public class Optionchain : IOptionchain
    {
        IUnitofWorkRepository _unitofWorkRepository;
        private readonly StockTicker _stockTicker;
        private static bool IsCalledFirstTime = false;
        private static CookieContainer cookieContainer = new CookieContainer();
        private static int timeout = 10;

        public Optionchain(IUnitofWorkRepository unitofWorkRepository, StockTicker stockTicker)
        {
            _unitofWorkRepository = unitofWorkRepository;
            _stockTicker = stockTicker;
        }

        public async Task<CallPutDifferenceResponse> Calculate(string opationChainResponse, string name)
        {
            CallPutDifferenceResponse data = new CallPutDifferenceResponse();
            try
            {
                OptionChainResponse response = JsonConvert.DeserializeObject<OptionChainResponse>(opationChainResponse);
                int record = DateTime.Now.DayOfWeek.ToString() switch
                {
                    "Monday" => 5,
                    "Tuesday" => 4,
                    "Wednesday" => 3,
                    "Thursday" => 2,
                    "Friday" => 5,
                    _ => 0
                };
                var records = response.records;
                double currentPrice = 0;
                Datum price = new Datum();
                if (name == "BANKNIFTY")
                {
                    currentPrice = Math.Round(records.underlyingValue / 100, 0) * 100;
                    price = response.filtered.data.FirstOrDefault(x => x.strikePrice == currentPrice);
                }
                else if (name == "NIFTY")
                {
                    currentPrice = Math.Round(records.underlyingValue / 50, 0) * 50;
                    price = response.filtered.data.FirstOrDefault(x => x.strikePrice == currentPrice);
                }
                else
                {
                    currentPrice = Math.Round(records.underlyingValue);
                    price = response.filtered.data.FirstOrDefault(x => x.strikePrice >= currentPrice);
                }

                var priceAbove = response.filtered.data.Where(x => x.strikePrice > currentPrice).Take(record).ToList();
                var priceBelow = response.filtered.data.Where(x => x.strikePrice < currentPrice).OrderByDescending(x => x.strikePrice).Take(record).ToList();
                double SumOfPutIO = 0;
                double SumOfCALLIO = 0;
                SumOfPutIO = SumOfPutIO + price.PE.changeinOpenInterest;
                SumOfCALLIO = SumOfCALLIO + price.CE.changeinOpenInterest;
                for (int i = 0; i < record; i++)
                {
                    var pa = priceAbove[i];
                    var pb = priceBelow[i];
                    SumOfPutIO = SumOfPutIO + (pa.PE.changeinOpenInterest + pb.PE.changeinOpenInterest);
                    SumOfCALLIO = SumOfCALLIO + (pa.CE.changeinOpenInterest + pb.CE.changeinOpenInterest);
                }
                double diff = (SumOfPutIO - SumOfCALLIO);
                decimal pcr = 0;
                pcr = (Convert.ToDecimal(SumOfPutIO) / Convert.ToDecimal(SumOfCALLIO));

                data.StrikePrice = (int)currentPrice;
                data.ResponseDateTime = Convert.ToDateTime(records.timestamp);
                data.SumOfCall = (int)SumOfCALLIO;
                data.SumOfPut = (int)SumOfPutIO;
                data.Difference = (int)diff;
                data.Name = name;
                data.CreatedDateTime = DateTime.UtcNow.AddHours(5.5);

            }
            catch
            {
            }
            return data;
        }

        public async Task<string> GetHTMLWithCookies(string baseurl)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(baseurl)))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "*/*");
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Safari/537.36");

                    var httpclient = new HttpClient(handler);
                    httpclient.Timeout = TimeSpan.FromSeconds(timeout);
                    using (var response = await httpclient.SendAsync(request).ConfigureAwait(false))
                    {
                        response.EnsureSuccessStatusCode();
                        using (HttpContent content = response.Content)
                        {
                            return await content.ReadAsStringAsync();
                        }
                    }
                }
            }

        }

        public async Task<string> GetOpationchain(string symbolName)
        {
            string url, res = String.Empty;
            if (symbolName == "NIFTY" || symbolName == "BANKNIFTY")
            {
                url = $"https://www.nseindia.com/api/option-chain-indices?symbol={symbolName}";
            }
            else
            {
                url = $"https://www.nseindia.com/api/option-chain-equities?symbol={symbolName}";
            }

            try
            {
                if (!IsCalledFirstTime)
                {
                    var generateCookie = UrlHelper.CookiesUrlForNSE;
                    await GetHTMLWithCookies(generateCookie);
                    IsCalledFirstTime = true;
                }
                res = await GetHTMLWithCookies(url);

            }
            catch (HttpRequestException ex)
            {
                if (string.Compare(ex.Message, "Response status code does not indicate success: 401 (Unauthorized).", true) == 0)
                {
                    cookieContainer = new CookieContainer();
                    var generateCookie = UrlHelper.CookiesUrlForNSE;
                    await GetHTMLWithCookies(generateCookie);
                    res = await GetHTMLWithCookies(url);

                }
            }
            return res;
        }

        public async Task<int> InsertData(CallPutDifferenceResponse modal)
        {
            var data = CommonHelper.ToDocumentData<CallPutDifferenceResponse, OptionChainDatum>(modal);
            await _unitofWorkRepository.RepositoryAsync<OptionChainDatum>().InsertAsync(data);
            var result = await _unitofWorkRepository.CommitAsync();
            return result;
        }
        public async Task<int> CronJob()
        {
            int res = 0;
            DateTime dateTime = DateTime.UtcNow.AddHours(5.5);
            var t = dateTime.TimeOfDay;
            int dayNumber = (int)dateTime.DayOfWeek;

            if (dayNumber >= 1 && dayNumber <= 5)
            {
                if (t.TotalMinutes >= 540 && t.TotalMinutes <= 935)
                {
                    var chainData = await GetOpationchain("BANKNIFTY");
                    var diff = await Calculate(chainData, "BANKNIFTY");
                    res = await InsertData(diff);

                    var chainNData = await GetOpationchain("NIFTY");
                    var Ndiff = await Calculate(chainNData, "NIFTY");
                    await InsertData(Ndiff);
                    await _stockTicker.Send();
                }
            }

            return res;
        }
    }
}
