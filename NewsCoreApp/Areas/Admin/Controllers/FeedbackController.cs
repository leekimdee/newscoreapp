using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsCoreApp.Application;
using NewsCoreApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    public class FeedbackController : BaseController
    {
        private FeedbackService _feedbackService;

        public FeedbackController()
        {
            _feedbackService = new FeedbackService();
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _feedbackService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _feedbackService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(Feedback video)
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
                    _feedbackService.Add(video);
                }
                else
                {
                    _feedbackService.Update(video);
                }
                _feedbackService.Save();
                return new OkObjectResult(video);
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _feedbackService.GetById(id);

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
                _feedbackService.Delete(id);
                _feedbackService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion AJAX API
    }
}