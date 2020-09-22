using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NewsCoreApp.Application;
using NewsCoreApp.Data.Entities;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    public class FunctionController : BaseController
    {
        private FunctionService _functionService;

        public FunctionController()
        {
            _functionService = new FunctionService();
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _functionService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _functionService.GetAllPaging(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllParent()
        {
            var model = _functionService.GetAllParent();
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(Function function)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (!_functionService.CheckItemExist(function.Id))
                {
                    _functionService.Add(function);
                }
                else
                {
                    _functionService.Update(function);
                }
                _functionService.Save();
                return new OkObjectResult(function);
            }
        }

        [HttpGet]
        public IActionResult GetById(string id)
        {
            var model = _functionService.GetById(id);

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
                _functionService.Delete(id);
                _functionService.Save();

                return new OkObjectResult(id);
            }
        }

        #endregion AJAX API
    }
}