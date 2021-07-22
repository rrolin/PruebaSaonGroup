using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PruebaTecnica_Saon.Catalogs;
using PruebaTecnica_Saon.Libraries;
using PruebaTecnica_Saon.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PruebaTecnica_Saon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        // Endpoints
        private readonly string _endpointReports;
        //private readonly string _endpointTotalReport;
        //private readonly string _endpointProvinces;
        private readonly string _endpointRegions;

        // Data
        private readonly DataLibrary _data;

        // Other variables
        private readonly string _latestDate;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;

            // Instance for using data access library.
            _data = new DataLibrary(configuration);

            // Initialize endpoints
            _endpointReports = Endpoints.Reports;
            //_endpointTotalReport = Endpoints.TotalReport;
            //_endpointProvinces = Endpoints.Provinces;
            _endpointRegions = Endpoints.Regions;

            // Initialize variables
            // Date is set to a day before due information may not be available anytime on current day.
            _latestDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        }

        #region Main actions
        [HttpGet]
        public IActionResult Index()
        {
            // Get top ten regions with confirmed cases
            var reportTopRegions = _data
                .GetAsync<ReportByRegionModel>(
                    endpoint: $"{_endpointReports}?date={_latestDate}"
                )
                .Result
                .data
                .OrderByDescending(x => x.confirmed)
                .Take(10)
                .ToList();

            // Convert to ModelView type model
            List<ReportModel> reportModel = reportTopRegions
                .Select(x => new ReportModel() 
                { 
                    locationHeader = "REGION", 
                    location = x.region.name,
                    cases = x.confirmed,
                    deaths = x.deaths
                })
                .ToList();

            // Data for fill out select controls
            ViewData["Regions"] = new SelectList(GetRegions(), "iso", "name");

            return View(reportModel);
        }

        [HttpGet]
        public IActionResult ReportProvinces(string region)
        {
            // Get top ten provinces with confirmed cases
            var reportTopProvinces = _data
                .GetAsync<ReportByRegionModel>(
                    endpoint: $"{_endpointReports}?date={_latestDate}&iso={region}"
                )
                .Result
                .data
                .OrderByDescending(x => x.confirmed)
                .Take(10)
                .ToList();

            // Display alert in case of empty
            if (reportTopProvinces is null || reportTopProvinces.Count is 0)
            {
                return Json(StatusCode(statusCode: 204, value: "No data was found with current search criteria."));
            }

            // Convert to ModelView type model
            List<ReportModel> reportModel = reportTopProvinces
                .Select(x => new ReportModel()
                {
                    locationHeader = "PROVINCE",
                    location = x.region.province,
                    cases = x.confirmed,
                    deaths = x.deaths
                })
                .ToList();

            return PartialView("~/Views/Home/_ReportTable.cshtml", reportModel);
        }

        #endregion

        #region UTILITIES
        [NonAction]
        public List<Region> GetRegions()
        {
            // Regions list
            var regions = _data.GetAsync<RegionsModel>(endpoint: _endpointRegions).Result.data.OrderBy(x => x.name).ToList();

            // Add default first option
            regions.Insert(
                index: 0,
                item: new()
                {
                    iso = "",
                    name = "Select..."
                }
            );

            return regions;
        }

        #endregion

        #region OPTIONS
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
