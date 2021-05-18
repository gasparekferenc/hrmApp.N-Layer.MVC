using Microsoft.AspNetCore.Mvc;


namespace hrmApp.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        #region GET: Index
        // GET: Home/Index --------------------------------------------------------
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region GET: Index2
        // GET: Home/Index2 --------------------------------------------------------
        public IActionResult Blank()
        {
            return View("Blank");
        }
        #endregion

        #region Contact()            
        public IActionResult Contact()
        {
            return View();
        }
        #endregion
    }
}
