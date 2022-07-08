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
                String filePath = _config.GetSection("Upload_path").Value; 
                        
                     if(!ExcelReader.ValidateExcelCoulmns(filePath,new string[] {"jhghh"}))
				{
                    return Json("Mandatory columns are missing");
                    
				}
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                             file.CopyTo(stream);
                        }
                _cache.Remove("DCHA.3.Architect.AMS.Imp");
                _cache.Remove("DCHA.3.Architect.AMS.Base");
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            
            return Json("File Uploaded Successfully.");
            }
        public IActionResult Success()
		{
            return View();
		}
        }
    }

