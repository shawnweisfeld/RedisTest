using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace RedisTestWeb.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            await RedisHelper.StringSetAsync("myvalue", DateTime.UtcNow.ToLongTimeString());

            return View();
        }

        public async Task<ActionResult> About()
        {
            ViewBag.Message = await RedisHelper.StringGetAsync("myvalue");

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}