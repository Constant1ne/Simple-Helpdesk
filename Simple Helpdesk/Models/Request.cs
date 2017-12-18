using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Simple_Helpdesk.Models
{
    /// <summary>
    /// Класс представляющий заявку
    /// </summary>
    public class Request
    {
        public int ID { get; set; }

        [Required]
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
        public Request RequestID { get; set; }

        /// <summary>
        /// Статус заявки 
        /// </summary>
        [Display(Name = "Статус")]
        public RequestStatus Status { get; set; }

        /// <summary>
        /// Дата текущего изменения 
        /// </summary>
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime ModificationTime { get; set; }

        [Required]
        [Display(Name = "Описание")]
        [MaxLength(200, ErrorMessage = "Превышена максимальная длина записи")]
        public string Descripion { get; set; }
    }

    public enum RequestStatus
    {
        Opened = 1,
        Solved = 2,
        Returned = 3,
        Closed = 4
    }
}