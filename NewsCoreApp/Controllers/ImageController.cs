using Microsoft.AspNetCore.Mvc;
using NewsCoreApp.Application;

namespace NewsCoreApp.Controllers
{
    public class ImageController : Controller
    {
        private ImageService _imageService;
        private ImageAlbumService _imageAlbumService;

        public ImageController()
        {
            _imageService = new ImageService();
            _imageAlbumService = new ImageAlbumService();
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