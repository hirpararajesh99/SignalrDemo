using Hangfire;
using Microsoft.AspNetCore.Mvc;
using SignalrDemo.Models;
using SignalrDemo.Service.Repository.Implementation;
using SignalrDemo.Service.Repository.Interface;
using System.Diagnostics;

namespace SignalrDemo.Controllers
{
    public class HomeController : Controller
    {
        

        
        private readonly IRecurringJobManager _recurringJobManager;

        public HomeController(IRecurringJobManager recurringJobManager)
        {
        
            _recurringJobManager = recurringJobManager;
        }
        public async Task<IActionResult> Index()
        {           
            _recurringJobManager.AddOrUpdate<IOptionchain>("Cron Job", x => x.CronJob(), "*/5 * * * *");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }            
     
    }
}