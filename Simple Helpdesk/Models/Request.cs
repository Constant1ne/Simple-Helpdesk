using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple_Helpdesk.Models
{
    /// <summary>
    /// Класс представляющий заявку
    /// </summary>
    public class Request
    {
        public Request() {
            this.Descriptions = new List<RequestDescription>();
        }
        public int ID { get; set; }

        [Required(ErrorMessage = "Введите название заявки")]
        [Display(Name = "Название заявки")]
        [MaxLength(30, ErrorMessage = "Превышена максимальная длина записи")]
        public string Name { get; set; }

        /// <summary>
        /// Ссылка на описания заявки, отношение one-to-many
        /// </summary>
        public virtual List<RequestDescription> Descriptions { get; set; }
    }

    /// <summary>
    /// Класс представляющий состояние заявки
    /// </summary>
    public class RequestDescription
    {
        public int ID { get; set; }

        /// <summary>
        /// Внешний ключ, заявка к которой относится данное описание
        /// </summary>
        public int RequestID { get; set; }

        /// <summary>
        /// Статус заявки 
        /// </summary>
        [Display(Name = "Статус")]
        public RequestStatus Status { get; set; }

        /// <summary>
        /// Дата текущего изменения 
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime ModificationTime { get; set; }

        /// <summary>
        /// Описание заявки или его текущего изменения
        /// </summary>
        [Required(ErrorMessage = "Введите причину создания/текущего изменения статуса заявки")]
        [Display(Name = "Описание изменения")]
        [MaxLength(200, ErrorMessage = "Превышена максимальная длина записи")]
        public string Description { get; set; }
    }

    public enum RequestStatus
    {
        Undefined = 0,
        Opened = 1, // Открыта
        Solved = 2, // Решена
        Returned = 3, // Возвращена
        Closed = 4 // Закрыта
    }

    /// <summary>
    /// картеж для возврата данных post запросом, полученных в представлении CreateRequest
    /// </summary>
    public class RequestTuple
    {
        public Request request { get; set; }
        public RequestDescription description { get; set; }
    }
}