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
    public class ImageAlbumController : BaseController
    {
        private IImageAlbumService _imageAlbumService;

        public ImageAlbumController(IImageAlbumService imageAlbumService)
        {
            _imageAlbumService = imageAlbumService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _imageAlbumService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _imageAlbumService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ImageAlbum imageAlbum)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (imageAlbum.Id == 0)
                {
                    _imageAlbumService.Add(imageAlbum);
                }
                else
                {
                    _imageAlbumService.Update(imageAlbum);
                }
                _imageAlbumService.Save();
                return new OkObjectResult(imageAlbum);
            }
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _imageAlbumService.GetById(id);

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
                _imageAlbumService.Delete(id);
                _imageAlbumService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion AJAX API
    }
}