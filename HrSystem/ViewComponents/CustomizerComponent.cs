using Microsoft.AspNetCore.Mvc;

namespace HrSystem.ViewComponents
{
    public class CustomizerViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
