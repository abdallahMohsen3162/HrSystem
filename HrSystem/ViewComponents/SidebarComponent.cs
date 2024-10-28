using Microsoft.AspNetCore.Mvc;

namespace HrSystem.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            bool isLoggedIn = HttpContext.User?.Identity?.IsAuthenticated ?? false;


            ViewData["IsLoggedIn"] = isLoggedIn;
            Console.WriteLine(isLoggedIn);
            return View();
        }
    }
}
