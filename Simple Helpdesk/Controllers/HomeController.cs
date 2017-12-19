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
            var requests = db.Requests;
            return View(requests.ToList());
        }

        //
        // GET: /Home/Create
        [HttpGet]
        public ViewResult CreateRequest() {
            return View();
        }
        
        [HttpPost]
        public ActionResult CreateRequest(RequestTuple requestTuple) {
            Request request = requestTuple.request;
            RequestDescription description = requestTuple.description;

            description.ModificationTime = DateTime.Now;
            description.RequestID = request;
            request.Descriptions = new List<RequestDescription>() { description };

            db.Requests.Add(request);
            db.Descriptions.Add(description);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Error(Error error) {
            return View(error);
        }
    }
}
