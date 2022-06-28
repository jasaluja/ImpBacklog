﻿using System;
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
		public String Excel_path;
		
		public HomeController(IConfiguration configuration)
		{
			_config = configuration;
			
		}

		public IActionResult Index(String gname = "DCHA.3.Architect.AMS.Imp")
		{
			try
			{
			if (!String.IsNullOrEmpty(_config.GetSection("Excel_path").Value)) { 
				Excel_path = _config.GetSection("Excel_path").Value;
			}
			else
			{
				return Error("Excel path is not available");
			}
			
				Utility.ValidateConfig(_config);
			
			ExcelReader excelReader = new ExcelReader(Excel_path,gname);
			DataTable fdata = excelReader.FilteredData;
			BacklogSperator.filtered_data = fdata;
			BacklogSperator.getDiplomatsCases(Utility.diplometsClients,Utility.ColumnsOrder);
		    BacklogSperator.getChallangersCases(Utility.ChallangersMaxDays, Utility.ChallangerTag,Utility.ColumnsOrder);
			
			backlogData = BacklogSperator.getSepratedBacklog();

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
			}
			catch (Exception e)
			{
				return Error(e.Message);
			}
			return View(backlogData);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(String error)
		{
			return View("~/Views/Shared/Error.cshtml",new ErrorViewModel { ErrorMessage=error});
		}
	}
}
