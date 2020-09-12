using NewsCoreApp.Data;
using NewsCoreApp.Data.EF;
using NewsCoreApp.Data.Entities;
using NewsCoreApp.Data.Enums;
using NewsCoreApp.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Application
{
    public class ContactService
    {
        private EFRepository<Contact, string> _contactRepository;
        private EFUnitOfWork _unitOfWork;

        public ContactService()
        {
            DbFactory dbFactory = new DbFactory();
            _contactRepository = new EFRepository<Contact, string>(dbFactory);
            _unitOfWork = new EFUnitOfWork(dbFactory);
        }

        public Contact Add(Contact contact)
        {
            _contactRepository.Add(contact);
            return contact;
        }

        public void Delete(string id)
        {
            _contactRepository.Remove(id);
        }

        public List<Contact> GetAll()
        {
            return _contactRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public List<Contact> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _contactRepository.FindAll(x => x.Name.Contains(keyword))
                    .OrderBy(x => x.Id).ToList();
            else
                return _contactRepository.FindAll().OrderBy(x => x.Id).ToList();
        }

        public PagedResult<Contact> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _contactRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword));

            int totalRow = query.Count();

            query = query.OrderByDescending(x => x.Id)
                .Skip((page - 1) * pageSize).Take(pageSize);

            var data = query.ToList();

            var paginationSet = new PagedResult<Contact>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public Contact GetById(string id)
        {
            return _contactRepository.FindById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(Contact contact)
        {
            _contactRepository.Update(contact);
        }

        public bool CheckItemExist(string id)
        {
            return _contactRepository.CheckExist(id);
        }
    }
}