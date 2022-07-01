using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Backlog_Segregation_Tool.Controllers
{
	public class UploadController : Controller
	{
        IConfiguration _config;
        String path;
		public UploadController(IConfiguration configuration)
		{
            _config = configuration;
		}
		public IActionResult Index()
		{
			return View();
		}
       

           [HttpPost]
            public async Task<IActionResult> Index(IFormFile file)
            {
                long size = file.Length;

            
                
                    if (size > 0)
                    {
                // full path to file in temp location
                var filePath = _config.GetSection("Upload_path").Value; //we are using Temp file name just for the example. Add your own file path.
                        
                
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
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

