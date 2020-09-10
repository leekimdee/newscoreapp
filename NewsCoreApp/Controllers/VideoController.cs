using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsCoreApp.Application;
using NewsCoreApp.Application.Interfaces;
using NewsCoreApp.Data;

namespace NewsCoreApp.Controllers
{
    public class VideoController : Controller
    {
        private IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public IActionResult Index()
        {
            var video = _videoService.GetAll();
            return View(video);
        }
    }
}