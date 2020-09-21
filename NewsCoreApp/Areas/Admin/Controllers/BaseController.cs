using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsCoreApp.Helpers;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorization]
    public class BaseController : Controller
    {
    }
}