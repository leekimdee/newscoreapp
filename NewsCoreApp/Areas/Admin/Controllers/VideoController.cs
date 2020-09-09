using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsCoreApp.Application;
using NewsCoreApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    public class VideoController : BaseController
    {
        private VideoService _videoService;

        public VideoController()
        {
            _videoService = new VideoService();
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _videoService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _videoService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(Video video)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (video.Id == 0)
                {
                    _videoService.Add(video);
                }
                else
                {
                    _videoService.Update(video);
                }
                _videoService.Save();
                return new OkObjectResult(video);
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _videoService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _videoService.Delete(id);
                _videoService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion AJAX API
    }
}