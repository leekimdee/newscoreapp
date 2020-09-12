using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsCoreApp.Application;
using NewsCoreApp.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        private ContactService _contactService;

        public ContactController()
        {
            _contactService = new ContactService();
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _contactService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _contactService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                //if (string.IsNullOrEmpty(contact.Id))
                if(!_contactService.CheckItemExist(contact.Id))
                {
                    _contactService.Add(contact);
                }
                else
                {
                    _contactService.Update(contact);
                }
                _contactService.Save();
                return new OkObjectResult(contact);
            }
        }

        [HttpGet]
        public IActionResult GetById(string id)
        {
            var model = _contactService.GetById(id);

            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                _contactService.Delete(id);
                _contactService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion AJAX API
    }
}