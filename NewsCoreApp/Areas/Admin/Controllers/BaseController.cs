using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NewsCoreApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class BaseController : Controller
    {
    }
}