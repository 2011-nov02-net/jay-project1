using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Aqua.Data;
using Aqua.Data.Model;
using Aqua.WebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Aqua.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DbContextOptions<AquaContext> _contextOptions;
        public HomeController(ILogger<HomeController> logger, DbContextOptions<AquaContext> contextOptions)
        {
            _logger = logger;
            _contextOptions = contextOptions;
        }

        public IActionResult Index()
        {
            var storeLocations = new LocationRepo(_contextOptions);
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
