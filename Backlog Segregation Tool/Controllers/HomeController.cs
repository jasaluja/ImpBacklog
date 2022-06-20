using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Data;
using Backlog_Segregation_Tool.Models;

namespace Backlog_Segregation_Tool.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _config;
		String[] fnames = new string[] {"Coast360 Federal Credit Union",
"FALCON NATIONAL BANK CENTRA",
"MERIWEST CREDIT UNION",
"SIERRA CENTRAL CREDIT UNION",
"WHATCOM EDUCATIONAL CREDIT UNION",
"SEATTLE CREDIT UNION"
};
		public HomeController(IConfiguration configuration)
		{
			_config = configuration;
			
		}

		public IActionResult Index()
		{
			Console.WriteLine("excel readerjhgkhk");
			var path = _config.GetSection("Excel_path").Value;
			ExcelReader excelReader = new ExcelReader(path);
			DataTable fdata = excelReader.FilteredData;
			BacklogSperator bs = new BacklogSperator(fdata);
			bs.getDiplomatsCases(fnames);
			bs.getChallangersCases(36);
			return View(excelReader.FilteredData);
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
