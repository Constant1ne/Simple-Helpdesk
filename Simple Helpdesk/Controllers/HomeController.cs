using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simple_Helpdesk.Models;

namespace Simple_Helpdesk.Controllers
{
    public class HomeController : Controller
    {
        private RequestContext db = new RequestContext();
        
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        //
        // GET: /Home/Create
        [HttpGet]
        public ViewResult CreateRequest() {
            return View();
        }
    }
}
