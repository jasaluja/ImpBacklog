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
using Backlog_Segregation_Tool.BusinessLogic;

namespace Backlog_Segregation_Tool.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _config;
		public DataSet backlog = new DataSet();
		public BacklogDataModel backlogData = new BacklogDataModel();

		public String[] ColumnsOrder;
		public String[] diplometsClients;
		public String ChallangerTag;
		public int ChallangersMaxDays;
		public HomeController(IConfiguration configuration)
		{
			_config = configuration;
			diplometsClients = _config.GetSection("DiplometsClients").Get<List<string>>().ToArray();
			ChallangerTag = _config.GetSection("ChallangerTag").Value.ToString();
			ChallangersMaxDays =Convert.ToInt32(_config.GetSection("ChallangersMaxDays").Value.ToString());
			ColumnsOrder = _config.GetSection("ColumnsOrder").Get<List<string>>().ToArray();
		}

		public IActionResult Index(String gname = "DCHA.3.Architect.AMS.Imp")
		{
			var path = _config.GetSection("Excel_path").Value;
			ExcelReader excelReader = new ExcelReader(path,gname);
			DataTable fdata = excelReader.FilteredData;
			fdata = Utility.ReOrderTable(fdata,ColumnsOrder);
			fdata = Utility.AddPreTriageStatusColumn(fdata);
			BacklogSperator bs = new BacklogSperator(fdata);
			bs.getDiplomatsCases(diplometsClients);
			bs.getChallangersCases(ChallangersMaxDays,ChallangerTag);
			backlogData = bs.getSepratedBacklog();
			if (gname == "DCHA.3.Architect.AMS.Imp")
			{
				backlogData.IsImpSelected = true;
				backlogData.IsBaseSelected = false;
			}
			if(gname == "DCHA.3.Architect.AMS.Base")
			{
				backlogData.IsImpSelected = false;
				backlogData.IsBaseSelected = true;
			}
			return View(backlogData);
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
