using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsCoreApp.Application;
using NewsCoreApp.Application.Interfaces;
using NewsCoreApp.Data;
using NewsCoreApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    public class ImageController : BaseController
    {
        private IImageService _imageService;

        public ImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpPost]
        public IActionResult SaveEntity(Image image)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (image.Id == 0)
                {
                    _imageService.Add(image);
                }
                else
                {
                    _imageService.Update(image);
                }
                _imageService.Save();
                return new OkObjectResult(image);
            }
        }

        [HttpPost]
        public IActionResult SaveMulti(List<Image> imageList)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                foreach (var image in imageList)
                {
                    _imageService.Add(image);
                }
                _imageService.Save();
            }
            return new OkObjectResult(imageList);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _imageService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _imageService.GetById(id);

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
                _imageService.Delete(id);
                _imageService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion AJAX API
    }
}