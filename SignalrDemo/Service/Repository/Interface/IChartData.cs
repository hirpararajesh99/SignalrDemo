using SignalrDemo.Models.ResponseModel;

namespace SignalrDemo.Service.Repository.Interface
{
    public interface IChartData
    {
        Task<List<ChartDataResponse>> GetChartData(string IndexName, string Date);
    }
}
