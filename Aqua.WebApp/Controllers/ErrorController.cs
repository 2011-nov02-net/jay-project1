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
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
