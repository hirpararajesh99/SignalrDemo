using Microsoft.AspNetCore.SignalR;
using SignalrDemo.Hubs;

namespace SignalrDemo
{
    public class StockTicker
    {
        private IHubContext<MyData> Hub { get; set; }
        public StockTicker(IHubContext<MyData> hub)
        {
            Hub = hub;
        }
        public async Task Send()
        {
            await Hub.Clients.All.SendAsync("GetDataBanknifty");
            await Hub.Clients.All.SendAsync("GetDataNifty");
        }

    }
}
