using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Data.IRepositories
{
    public interface IImageAlbumRepository : IRepository<ImageAlbum, int>
    {
    }
}
