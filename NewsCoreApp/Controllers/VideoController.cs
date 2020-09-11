using Microsoft.AspNetCore.Mvc;
using NewsCoreApp.Application;

namespace NewsCoreApp.Controllers
{
    public class VideoController : Controller
    {
        private VideoService _videoService;

        public VideoController()
        {
            _videoService = new VideoService();
        }

        public IActionResult Index()
        {
            var video = _videoService.GetAll();
            return View(video);
        }
    }
}