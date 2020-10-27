using Microsoft.EntityFrameworkCore;
using NewsCoreApp.Data;
using NewsCoreApp.Data.EF;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Enums;
using NewsCoreApp.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsCoreApp.Application
{
    public class FunctionService
    {
        private EFRepository<Function, string> _functionRepository;
        private EFUnitOfWork _unitOfWork;

        public FunctionService()
        {
            DbFactory dbFactory = new DbFactory();
            _functionRepository = new EFRepository<Function, string>(dbFactory);
            _unitOfWork = new EFUnitOfWork(dbFactory);
        }

        public Function Add(Function function)
        {
            _functionRepository.Add(function);
            return function;
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public Task<List<Function>> GetAll()
        {
            return _functionRepository.FindAll().OrderBy(x => x.Id).ToListAsync();
        }

        public List<Function> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _functionRepository.FindAll(x => x.Name.Contains(keyword))
                    .OrderBy(x => x.Id).ToList();
            else
                return _functionRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public PagedResult<Function> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.SortOrder)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ToList();

            var paginationSet = new PagedResult<Function>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public List<Function> GetAllParent()
        {
            return _functionRepository.FindAll(x => x.ParentId == null).OrderBy(y => y.SortOrder).ToList();
        }

        public IEnumerable<Function> GetAllWithParentId(string parentId)
        {
            return _functionRepository.FindAll(x => x.ParentId == parentId).ToList();
        }

        public void ReOrder(string sourceId, string targetId)
        {

            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);

        }

        public Function GetById(string id)
        {
            return _functionRepository.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Function function)
        {
            _functionRepository.Update(function);
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }

        public bool CheckItemExist(string id)
        {
            return _functionRepository.CheckExist(id);
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }
    }
}