using NewsCoreApp.Data.Entities;
using NewsCoreApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application.Interfaces
{
    public interface IImageAlbumService : IDisposable
    {
        ImageAlbum Add(ImageAlbum imageAlbum);

        void Update(ImageAlbum imageAlbum);

        void Delete(int id);

        List<ImageAlbum> GetAll();

        List<ImageAlbum> GetAll(string keyword);

        PagedResult<ImageAlbum> GetAllPaging(string keyword, int page, int pageSize);

        ImageAlbum GetById(int id);

        ImageAlbum GetByIdWithIncludeObject(int id);

        void ReOrder(int sourceId, int targetId);

        void Save();
    }
}
