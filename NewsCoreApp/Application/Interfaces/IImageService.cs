using NewsCoreApp.Data.Entities;
using NewsCoreApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application.Interfaces
{
    public interface IImageService
    {
        Image Add(Image image);

        void Update(Image image);

        void Delete(int id);

        List<Image> GetAll();

        List<Image> GetAll(string keyword);

        PagedResult<Image> GetAllPaging(string keyword, int page, int pageSize);

        Image GetById(int id);

        List<Image> GetImagesByAlbumId(int albumId);

        void Save();
    }
}
