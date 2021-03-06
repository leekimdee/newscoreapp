﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public UploadController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Upload image for form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadImage()
        {
            DateTime now = DateTime.Now;
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }
            else
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                return new OkObjectResult(Path.Combine(imageFolder, filename).Replace(@"\", @"/"));
            }
        }

        /// <summary>
        /// Upload multi image for form
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UploadMultiImage()
        {
            DateTime now = DateTime.Now;
            var files = Request.Form.Files;
            List<string> imagePathList = new List<string>();
            if (files.Count == 0)
            {
                return new BadRequestObjectResult(files);
            }
            else
            {
                var imageFolder = $@"\uploaded\images\{now.ToString("yyyyMMdd")}";

                string folder = _hostingEnvironment.WebRootPath + imageFolder;

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                foreach(var file in files)
                {
                    var filename = ContentDispositionHeaderValue
                                    .Parse(file.ContentDisposition)
                                    .FileName
                                    .Trim('"');

                    string filePath = Path.Combine(folder, filename);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    imagePathList.Add(Path.Combine(imageFolder, filename).Replace(@"\", @"/"));
                }

                return new OkObjectResult(imagePathList);
            }
        }
    }
}