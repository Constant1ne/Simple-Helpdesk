﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Simple_Helpdesk.Models;

namespace Simple_Helpdesk.Controllers
{
    public class HomeController : Controller
    {
        private MyRequestContext db = new MyRequestContext("RequestsDataBase");
        private FilteringOptions filteringOptions = new FilteringOptions();
        
        //
        // GET: /Home/
        /// <summary>
        /// Главная страница. Таблица с записями.
        /// </summary>
        /// <param name="options">Фильтры с параметрами для выборки записей из базы</param>
        [HttpGet]
        public ActionResult Index(FilteringOptions options) {                                    
            if (options == null) {    
                return View(db.Requests.ToList());
            }

            IQueryable<Request> requests = db.Requests;

            // Отображение в зависимости от текущего статуса заявки
            if (options.Status != RequestStatus.Undefined) {
                // Записи в зависимости от текущего статуса
                requests = from req in requests
                           where req.Descriptions.OrderByDescending(des => des.ID).FirstOrDefault().Status == options.Status
                           select req;
            }

            // Отображение тех заявок, которые хоть раз возращались на доработку
            if (options.isReturned) {
                // Для всех заявок, у которых в истории статусов присутствует RequestStatus.Returned
                requests = from req in requests
                           from desc in req.Descriptions
                           where desc.Status == RequestStatus.Returned
                           select req;
            }

            if (options.After != null) {
                // Для всех заявок созданных после
                requests = from req in requests
                           from desc in req.Descriptions
                           where desc.Status == RequestStatus.Opened
                           && desc.ModificationTime > options.After
                           select req;
            }

            if (options.Before != null) {
                // Для всех заявок созданных до
                requests = from req in requests
                           from desc in req.Descriptions
                           where desc.Status == RequestStatus.Opened
                           && desc.ModificationTime < options.Before
                           select req;
            }

            // отсоритовать по возрастанию дат создания или изменения
            if (options.SortByCreationTime) {
                // сортировка по созданию
                requests = from req in requests
                           orderby req.Descriptions.OrderBy(des => des.ID).FirstOrDefault().ModificationTime descending
                           select req;

            } else {
                // сортировка по последнему изменению
                requests = from req in requests
                           orderby req.Descriptions.OrderByDescending(des => des.ID).FirstOrDefault().ModificationTime descending
                           select req;
            }

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
            if (!ModelState.IsValid) { // проверили корректность ввода
                return View();
            }
            Request request = requestTuple.request; // достали из картежа ссылку на заявку
            RequestDescription description = requestTuple.description; // достали из картежа ссылку на её описание

            description.ModificationTime = DateTime.Now;
            //description.RequestID = request.ID; // отношение описание <->  заявка (one-to-one)
            request.Descriptions.Add(description); // отношение заявка <-> описания (one-to-many)

            // записали в базу
            this.db.Requests.Add(request);
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
            if (!ModelState.IsValid) {
                return RedirectToAction("UpdateRequest", "Home", new { Id = tuple.description.RequestID } );
            }
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
        /// Форма с настройками выборки из базы
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
