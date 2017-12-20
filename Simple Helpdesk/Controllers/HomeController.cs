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
        private FilteringOptions filteringOptions = new FilteringOptions();
        
        //
        // GET: /Home/
        [HttpGet]
        public ActionResult Index(FilteringOptions options) {
            IEnumerable<Request> requests;
            if (options == null) {
                requests = db.Requests;
                return View(requests.ToList());
            }

            // Отображение в зависимости от текущего статуса заявки
            if (options.Status == RequestStatus.Undefined) {
                requests = db.Requests;
            } else {
                requests = db.Requests.Where(request => request.Descriptions.Last().Status == options.Status);
            }

            // Отображение тех заявок, которые хоть раз возращались на доработку
            if (options.isReturned) {
                // Для всех заявок, у которых в истории статусов присутствует RequestStatus.Returned
            }

            if (options.After != null) {
                // Для всех заявок созданных после
            }

            if (options.Before != null) {
                // Для всех заявок созданных до
            }

            // отсоритовать по возрастанию дат создания или изменения

            return View(requests.ToList());
        }

        //
        // GET: /Home/Create
        /// <summary>
        /// Интерфейс для создания новой заявки
        /// </summary>
        [HttpGet]
        public ViewResult CreateRequest() {
            return View();
        }

        /// <summary>
        /// Создание заявки
        /// </summary>
        /// <param name="requestTuple"> картеж состоящий из заявки и её текущего статуса</param>
        [HttpPost]
        public ActionResult CreateRequest(RequestTuple requestTuple) {
            Request request = requestTuple.request; // достали из картежа ссылку на заявку
            RequestDescription description = requestTuple.description; // достали из картежа ссылку на её описание

            description.ModificationTime = DateTime.Now;
            description.RequestID = request.ID; // отношение описание <->  заявка (one-to-one)
            request.Descriptions.Add(description); // отношение заявка <-> описания (one-to-many)

            // записали в базу
            this.db.Requests.Add(request);
            this.db.Descriptions.Add(description);
            this.db.SaveChanges();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Страница для отображения ошибок
        /// </summary>
        [HttpGet]
        public ActionResult Error(Error error) {
            return View(error);
        }

        /// <summary>
        /// Отображение всех изменений для 1 заявки
        /// </summary>
        /// <param name="Id">ID заявки</param>
        [HttpGet]
        public ActionResult ChangeLog(int Id) {
            Request request = this.db.Requests.Where(r => r.ID == Id).FirstOrDefault();
            // в случае когда заявка не найдена формируем сообщение об ошибке
            if (request == null) {
                return RedirectToAction("Error", "Home", new Error("Заявка с таким идентификатором не найдена, Id = " + Id));
            }
            return View(request);
        }

        /// <summary>
        /// Форма для редактирования состояния заявки
        /// </summary>
        /// <param name="Id">ID заявки</param>
        [HttpGet]
        public ActionResult UpdateRequest(int Id) {
            Request found_Request = this.db.Requests.Where(r => r.ID == Id).FirstOrDefault();
            // ошибка - заявка не найдена
            if (found_Request == null) {
                return RedirectToAction("Error", "Home", new Error("Заявка с таким идентификатором не найдена, Id = " + Id));
            }
            // ошибка при попытке редактировать закрытую заявку
            if (found_Request.Descriptions.Last().Status == RequestStatus.Closed) {
                return RedirectToAction("Error", "Home", new Error("Заявка закрыта, дальнейшее редактирование её невозможно"));
            }
            // собираем заявку и её текущий статус в картеж, передаем форме
            RequestTuple tuple = new RequestTuple() {
                request = found_Request,
                description = found_Request.Descriptions.Last()
            };
            return View(tuple);
        }

        /// <summary>
        /// Содание нового состояния для заявки
        /// </summary>
        /// <param name="tuple">картеж состоящий из заявки и её текущего статуса</param>
        [HttpPost]
        public ActionResult UpdateRequest(RequestTuple tuple) {
            // в картеже приходит только новый объект соответствующий новому состоянию заявки
            // саму заявку, для которой поменяется значение состояния необходимо достать из базы
            Request found_Request = this.db.Requests.Where(r => r.ID == tuple.description.RequestID).FirstOrDefault();
            if (found_Request == null) {
                return RedirectToAction("Error", "Home", new Error("Произошла ошибка генерации нового состояния, идентификатор редактируемой заявки не найден, Id = " + tuple.description.RequestID));
            }

            tuple.description.ModificationTime = DateTime.Now;
            found_Request.Descriptions.Add(tuple.description); // добавили заявке новое состояние

            this.db.Descriptions.Add(tuple.description); // занесли новое состояние в базу
            this.db.SaveChanges();
            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Удаление заявки из базы
        /// </summary>
        /// <param name="Id">ID заявки</param>
        [HttpGet]
        public ActionResult RemoveRequest(int Id) {
            Request request = this.db.Requests.Where(r => r.ID == Id).FirstOrDefault();
            if (request == null) {
                return RedirectToAction("Error", "Home", new Error("Запись с таким идентификатором не найдена, Id = " + Id));
            }
            // удаляем запись из таблицы заявок
            this.db.Requests.Remove(request); // из таблицы состояний соответствующие позиции удалятся автоматически (ON DELETE CASCADE)
            this.db.SaveChanges();
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult FilteringOptions(FilteringOptions options) {
            return View(options);
        }

        [HttpGet]
        public ActionResult FilteringOptionsApply(FilteringOptions options) {
            return RedirectToAction("Index", "Home", options);
        }
    }
}
