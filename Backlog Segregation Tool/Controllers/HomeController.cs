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
using Microsoft.Extensions.Caching.Memory;

namespace Backlog_Segregation_Tool.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMemoryCache _memoryCache;
		private readonly ILogger<HomeController> _logger;
		private readonly IConfiguration _config;
		public DataSet backlog = new DataSet();
		public DataTable fdata = new DataTable();
		public DataTable tasks_fdata = new DataTable();
		public BacklogDataModel backlogData = new BacklogDataModel();
		public String Excel_path;
		public String Tasks_path;
		public TasksBacklogDataModel taskBacklog = new TasksBacklogDataModel();
		public HomeController(IConfiguration configuration,IMemoryCache _cache)
		{
			_config = configuration;
			_memoryCache = _cache;
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
				if (!_memoryCache.TryGetValue(gname, out DataTable fdata))
				{
					fdata = ExcelReader.FilterByGroupName(ExcelReader.ReadExcel(Excel_path), gname);
					_memoryCache.Set(gname,fdata);
				
				}

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

		public IActionResult Tasks(String gname= "DCHA.3.Architect.AMS.Imp")
		{
			try
			{
				if (!String.IsNullOrEmpty(_config.GetSection("Tasks_path").Value))
				{
					Tasks_path = _config.GetSection("Tasks_path").Value;
				}
				else
				{
					return Error("Tasks File path is not available");
				}

				//Utility.ValidateConfig(_config);
				if (!_memoryCache.TryGetValue("tasks."+gname, out DataTable tasks_fdata))
				{
					tasks_fdata = ExcelReader.ReadExcel(Tasks_path);
					_memoryCache.Set("tasks."+gname, tasks_fdata);

				}
				if (!_memoryCache.TryGetValue(gname, out DataTable fdata))
				{
					fdata = ExcelReader.FilterByGroupName(ExcelReader.ReadExcel(Excel_path), gname);
				}
				tasks_fdata = BacklogSperator.MapTasksData(fdata, tasks_fdata);
				DataSet seperatedData = BacklogSperator.SeprateTaskCases(tasks_fdata);
				BacklogSperator.filtered_data = seperatedData.Tables[0];
				BacklogSperator.getDiplomatsCases(Utility.diplometsClients, Utility.ColumnsOrder);
				BacklogSperator.getChallangersCases(Utility.ChallangersMaxDays, Utility.ChallangerTag, Utility.ColumnsOrder);
				backlogData = BacklogSperator.getSepratedBacklog();
				taskBacklog.FTDataModel = backlogData;
				BacklogSperator.filtered_data = seperatedData.Tables[0];
				BacklogSperator.getDiplomatsCases(Utility.diplometsClients, Utility.ColumnsOrder);
				BacklogSperator.getChallangersCases(Utility.ChallangersMaxDays, Utility.ChallangerTag, Utility.ColumnsOrder);
				backlogData = BacklogSperator.getSepratedBacklog();
				taskBacklog.SwDataModel = backlogData;
				if (gname == "DCHA.3.Architect.AMS.Imp")
				{
					backlogData.IsImpSelected = true;
					backlogData.IsBaseSelected = false;
				}
				if (gname == "DCHA.3.Architect.AMS.Base")
				{
					backlogData.IsImpSelected = false;
					backlogData.IsBaseSelected = true;
				}
			}
			catch (Exception e)
			{
				return Error(e.Message);
			}
			return View(taskBacklog);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(String error)
		{
			return View("~/Views/Shared/Error.cshtml",new ErrorViewModel { ErrorMessage=error});
		}
	}
}
