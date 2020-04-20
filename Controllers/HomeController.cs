using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore_API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace BookStore_API.Controllers
{
    /// <summary>
    /// This is a test API controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private ILoggerService _logger;

        public HomeController(ILoggerService logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            _logger.LogInfo("Accessed Home Controller via GET without parameters");
            return Ok("Hello World");
        }
    }
}