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
    public class ImageController : Controller
    {
        private IImageService _imageService;
        private IImageAlbumService _imageAlbumService;

        public ImageController(IImageService imageService, IImageAlbumService imageAlbumService)
        {
            _imageService = imageService;
            _imageAlbumService = imageAlbumService;
        }

        public IActionResult Index()
        {
            ViewBag.ImageAlbums = _imageAlbumService.GetAll();
            return View();
        }

        [HttpGet]
        public IActionResult GetImagesByAlbum(int albumId)
        {
            //var imageAlbum = _imageAlbumService.GetByIdWithIncludeObject(albumId);
            //var images = imageAlbum.Images.ToList();
            var images = _imageService.GetImagesByAlbumId(albumId);

            return new OkObjectResult(images);
        }
    }
}