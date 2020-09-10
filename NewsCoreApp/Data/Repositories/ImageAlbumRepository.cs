using NewsCoreApp.Data.EF;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Data.Repositories
{
    public class ImageAlbumRepository : EFRepository<ImageAlbum, int>, IImageAlbumRepository
    {
        public ImageAlbumRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
