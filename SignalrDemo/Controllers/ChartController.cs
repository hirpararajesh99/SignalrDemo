using Microsoft.AspNetCore.Mvc;
using SignalrDemo.Service.Repository.Interface;
using System;

namespace SignalrDemo.Controllers
{
    [Route("Chart")]
    public class ChartController : Controller
    {
        private readonly IChartData _chart;

        public ChartController(IChartData chart)
        {
            _chart = chart;
        }

        [Route("banknifty")]
        [HttpGet]
        public IActionResult Index()
        {            
            return View();
        }
        [Route("nifty")]
        [HttpGet]
        public IActionResult Nifty()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetChartDataAsync(string name)
        {
            var res = await _chart.GetChartData(name, DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd"));
            return Json(res);
        }
    }
}
