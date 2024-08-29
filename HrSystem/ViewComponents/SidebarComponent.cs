using Microsoft.AspNetCore.Mvc;

namespace HrSystem.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
