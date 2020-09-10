using NewsCoreApp.Data.Entities;
using NewsCoreApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application.Interfaces
{
    public interface IVideoService
    {
        Video Add(Video video);

        void Update(Video video);

        void Delete(int id);

        List<Video> GetAll();

        List<Video> GetAll(string keyword);

        PagedResult<Video> GetAllPaging(string keyword, int page, int pageSize);

        Video GetById(int id);

        void Save();
    }
}
