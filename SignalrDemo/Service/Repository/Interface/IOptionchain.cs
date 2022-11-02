using SignalrDemo.Models.ResponseModel;
using System.Threading.Tasks;

namespace SignalrDemo.Service.Repository.Interface
{
    public interface IOptionchain
    {
        Task<string> GetOpationchain(string symbolName);
        Task<string> GetHTMLWithCookies(string baseurl);
        Task<CallPutDifferenceResponse> Calculate(string opationChainResponse, string name);

        Task<int> InsertData(CallPutDifferenceResponse modal);
        Task<int> CronJob();
    }
}
