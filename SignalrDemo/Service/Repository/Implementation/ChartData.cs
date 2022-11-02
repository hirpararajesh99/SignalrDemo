using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc.Rendering;
using SignalrDemo.DbEntity;
using SignalrDemo.Models.ResponseModel;
using SignalrDemo.Service.Repository.Interface;
using SignalrDemo.Service.UnitofWork;

namespace SignalrDemo.Service.Repository.Implementation
{
    public class ChartData : IChartData
    {
        IUnitofWorkRepository uow;

        public ChartData (IUnitofWorkRepository unitofWorkRepository)
        {
            uow = unitofWorkRepository;
        }
        public async Task<List<ChartDataResponse>> GetChartData(string IndexName, string Date)
            {
            var chartData = await uow.RepositoryAsync<OptionChainDatum>().GetAllListAsync(d => d.Name == IndexName,x=>x.OrderBy(o=>o.Id));
            var data = chartData.Where(d=>d.CreatedDateTime.ToString("yyyy-MM-dd")==Date).Select(x => new ChartDataResponse()
            {
                x = x.CreatedDateTime.ToString("hh:mm"),
                y = (int)x.Difference
            }).ToList();
            return data;
        }
    }
}
