using Microsoft.AspNetCore.Mvc;

namespace MTBjorn.NetMon.Web.Controllers;

public class HomeController : Controller
{
	public IActionResult Index()
	{
		return View();
	}
}
