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
		public DataSet backlog = new DataSet();
		

		public String[] diplometsClients;
		public HomeController(IConfiguration configuration)
		{
			_config = configuration;
			diplometsClients = _config.GetSection("DiplometsClients").Get<List<string>>().ToArray();
		}

		public IActionResult Index(String groupName)
		{
			Console.WriteLine("excel readerjhgkhk");
			var path = _config.GetSection("Excel_path").Value;
			ExcelReader excelReader = new ExcelReader(path,groupName);
			DataTable fdata = excelReader.FilteredData;
			BacklogSperator bs = new BacklogSperator(fdata);
			backlog.Tables.Add(bs.getDiplomatsCases(diplometsClients));
			bs.getChallangersCases(36);
			backlog.Tables.Add(bs.challangers);
			backlog.Tables.Add(bs.optimizers);
			return View(backlog);
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
