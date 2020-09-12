using NewsCoreApp.Data;
using NewsCoreApp.Data.EF;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Enums;
using NewsCoreApp.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Application
{
    public class FeedbackService
    {
        private EFRepository<Feedback, int> _feedbackRepository;
        private EFUnitOfWork _unitOfWork;

        public FeedbackService()
        {
            DbFactory dbFactory = new DbFactory();
            _feedbackRepository = new EFRepository<Feedback, int>(dbFactory);
            _unitOfWork = new EFUnitOfWork(dbFactory);
        }

        public Feedback Add(Feedback feedback)
        {
            _feedbackRepository.Add(feedback);
            return feedback;
        }

        public void Delete(int id)
        {
            _feedbackRepository.Remove(id);
        }

        public List<Feedback> GetAll()
        {
            return _feedbackRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public List<Feedback> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _feedbackRepository.FindAll(x => x.Title.Contains(keyword))
                    .OrderBy(x => x.Id).ToList();
            else
                return _feedbackRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public PagedResult<Feedback> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _feedbackRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Title.Contains(keyword));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.CreatedDate)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ToList();

            var paginationSet = new PagedResult<Feedback>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public Feedback GetById(int id)
        {
            return _feedbackRepository.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Feedback feedback)
        {
            _feedbackRepository.Update(feedback);
        }
    }
}