using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

namespace SignalrDemo.Hubs
{
   
    public class MyData : Hub
    {
        private readonly StockTicker _stockTicker;
        public MyData(StockTicker stockTicker)
        {
            _stockTicker = stockTicker;
        }
        public async Task SendNotificationToAll()
        {
            await _stockTicker.Send();
        }
        public override async Task OnConnectedAsync()
        {
            var id = Context?.GetHttpContext()?.Request.Query["emp"].ToString();
            await Groups.AddToGroupAsync(Context?.ConnectionId, id);
            await base.OnConnectedAsync();
        }

    }
}
