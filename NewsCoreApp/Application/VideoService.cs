using NewsCoreApp.Data;
using NewsCoreApp.Data.EF;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Enums;
using NewsCoreApp.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Application
{
    public class VideoService
    {
        private EFRepository<Video, int> _videoRepository;
        private EFUnitOfWork _unitOfWork;

        public VideoService()
        {
            DbFactory dbFactory = new DbFactory();
            _videoRepository = new EFRepository<Video, int>(dbFactory);
            _unitOfWork = new EFUnitOfWork(dbFactory);
        }

        public Video Add(Video video)
        {
            _videoRepository.Add(video);
            return video;
        }

        public void Delete(int id)
        {
            _videoRepository.Remove(id);
        }

        public List<Video> GetAll()
        {
            return _videoRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public List<Video> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _videoRepository.FindAll(x => x.Title.Contains(keyword))
                    .OrderBy(x => x.Id).ToList();
            else
                return _videoRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public PagedResult<Video> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _videoRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Title.Contains(keyword));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ToList();

            var paginationSet = new PagedResult<Video>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public Video GetById(int id)
        {
            return _videoRepository.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Video video)
        {
            _videoRepository.Update(video);
        }
    }
}