using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Backlog_Segregation_Tool.BusinessLogic;
namespace Backlog_Segregation_Tool.Controllers
{
	public class UploadController : Controller
	{
        IConfiguration _config;
        IMemoryCache _cache;
        String path;
		public UploadController(IConfiguration configuration,IMemoryCache memoryCache)
		{
            _config = configuration;
            _cache = memoryCache;
		}
		public IActionResult Index()
		{
			return View();
		}
       

           [HttpPost]
            public IActionResult Index(IFormFile file)
            {
                long size = file.Length;

            
                
                    if (size > 0)
                    {
                // full path to file in temp location
                String filePath = _config.GetSection("Excel_path").Value;
                String[] mustColumns = _config.GetSection("MustCaseColumns").Get<List<string>>().ToArray();

                bool valid = ExcelReader.ValidateExcelCoulmns(file, mustColumns);
                TempData["UploadMsg"] = "Upload Failed! Please validate the File.";

                using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                             file.CopyTo(stream);
                        }
                _cache.Remove("DCHA.3.Architect.AMS.Imp");
                _cache.Remove("DCHA.3.Architect.AMS.Base");
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            TempData["UploadMsg"] = "File Uploaded";
            return Json("File Uploaded Successfully.");
            }
        [HttpPost]
        public String UploadTask(IFormFile file)
        {
            long size = file.Length;



            if (size > 0)
            {
                // full path to file in temp location
                String[] mustColumns = _config.GetSection("MustTasksColumns").Get<List<string>>().ToArray();
                String filePath = _config.GetSection("Tasks_path").Value;
                bool valid = ExcelReader.ValidateExcelCoulmns(file,mustColumns);
                if (!valid) {
                    TempData["UploadMsg"] = "Upload Failed! Please validate the File.";
                    return "File not uploaded";
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                _cache.Remove("tasks.DCHA.3.Architect.AMS.Imp");
                _cache.Remove("tasks.DCHA.3.Architect.AMS.Base");
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            //TempData["UploadMsg"] = "File Uploaded";
            return "File Uploaded Successfully.";
        }

        public IActionResult Success()
		{
            return View();
		}
        }
    }

