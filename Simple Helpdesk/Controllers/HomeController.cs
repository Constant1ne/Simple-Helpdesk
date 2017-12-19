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
            description.RequestID = request.ID;
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

        [HttpGet]
        public ActionResult ChangeLog(int Id) {
            Request request = db.Requests.Where(r => r.ID == Id).FirstOrDefault();
            if (request == null) {
                return RedirectToAction("Error", "Home", new Error("Запись с таким идентификатором не найдена, Id = " + Id));
            }
            return View(request);
        }

        [HttpGet]
        public ActionResult UpdateRequest(int Id) {
            Request found_Request = db.Requests.Where(r => r.ID == Id).FirstOrDefault();
            if (found_Request == null) {
                return RedirectToAction("Error", "Home", new Error("Запись с таким идентификатором не найдена, Id = " + Id));
            }
            if (found_Request.Descriptions.Last().Status == RequestStatus.Closed) {
                return RedirectToAction("Error", "Home", new Error("Заявка закрыта, дальнейшее редактирование её невозможно"));
            }
            RequestTuple tuple = new RequestTuple() {
                request = found_Request,
                description = found_Request.Descriptions.Last()
            };
            return View(tuple);
        }

        [HttpPost]
        public ActionResult UpdateRequest(RequestTuple tuple) {
            Request found_Request = db.Requests.Where(r => r.ID == tuple.description.RequestID).FirstOrDefault();
            if (found_Request == null) {
                return RedirectToAction("Error", "Home", new Error("Запись с таким идентификатором не найдена, Id = " + tuple.description.RequestID));
            }

            tuple.description.ModificationTime = DateTime.Now;
            found_Request.Descriptions.Add(tuple.description);

            db.Descriptions.Add(tuple.description);
            //db.Requests.Add(request);
            db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult RemoveRequest(int Id) {
            Request request = db.Requests.Where(r => r.ID == Id).FirstOrDefault();
            if (request == null) {
                return RedirectToAction("Error", "Home", new Error("Запись с таким идентификатором не найдена, Id = " + Id));
            }
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
