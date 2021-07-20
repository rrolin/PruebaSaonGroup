using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PruebaTecnica_Saon.Catalogs;
using PruebaTecnica_Saon.Libraries;
using PruebaTecnica_Saon.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace PruebaTecnica_Saon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        // Endpoints
        private readonly string _endpointReports;
        private readonly string _endpointTotalReport;
        private readonly string _endpointProvinces;
        private readonly string _endpointRegions;

        // Data
        private readonly DataLibrary _data;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Instance for using data access library.
            _data = new DataLibrary(configuration);

            // Initialize endpoints
            _endpointReports = Endpoints.Reports;
            _endpointTotalReport = Endpoints.TotalReport;
            _endpointProvinces = Endpoints.Provinces;
            _endpointRegions = Endpoints.Regions;
            
        }

        #region Main actions
        [HttpGet]
        public IActionResult Index()
        {
            var regions = _data.GetAsync<RegionsModel>(endpoint: _endpointRegions).Result;

            return View();
        }

        #endregion

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
