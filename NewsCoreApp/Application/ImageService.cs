using NewsCoreApp.Application.Interfaces;
using NewsCoreApp.Data;
using NewsCoreApp.Data.EF;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Enums;
using NewsCoreApp.Data.Interfaces;
using NewsCoreApp.Data.IRepositories;
using NewsCoreApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application
{
    public class ImageService : IImageService
    {
        private IImageRepository _imageRepository;
        private IUnitOfWork _unitOfWork;

        public ImageService(IImageRepository imageRepository, IUnitOfWork unitOfWork)
        {
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
        }

        public Image Add(Image image)
        {
            _imageRepository.Add(image);
            return image;
        }

        public void Delete(int id)
        {
            _imageRepository.Remove(id);
        }

        public List<Image> GetAll()
        {
            return _imageRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public List<Image> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _imageRepository.FindAll(x => x.Title.Contains(keyword))
                    .OrderBy(x => x.Id).ToList();
            else
                return _imageRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public PagedResult<Image> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _imageRepository.FindAll(x => x.Status == Status.Active, i => i.ImageAlbum);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Title.Contains(keyword));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ToList();

            var paginationSet = new PagedResult<Image>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public Image GetById(int id)
        {
            return _imageRepository.FindById(id);
        }

        public List<Image> GetImagesByAlbumId(int albumId)
        {
            return _imageRepository.FindAll(f => f.ImageAlbumId == albumId).ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Image image)
        {
            _imageRepository.Update(image);
        }
    }
}
