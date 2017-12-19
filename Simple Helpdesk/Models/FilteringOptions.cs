using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simple_Helpdesk.Models;
using System.ComponentModel.DataAnnotations;

namespace Simple_Helpdesk.Models
{
    /// <summary>
    /// Хранилище фильтров и из значений
    /// </summary>
    public class FilteringOptions
    {
        public FilteringOptions() {
            this.Reset();
        }
        
        /// <summary>
        /// Вывести заявки у которых текущий статус равен указанному значению
        /// </summary>
        [Display(Name = "Отображать заявки только с этим состоянием:")]
        public RequestStatus? Status { get; set; }
        
        /// <summary>
        /// Показывать заявки которые хоть раз были "возвращены" в обработку
        /// </summary>
        [Display(Name = "Отображаться заявки хоть раз вернувшиеся на доработку")]
        public bool isReturned { get; set; }

        /// <summary>
        /// Вывести заявки созданные после
        /// </summary>
        [Display(Name = "Только завки созданные после")]
        public DateTime? After { get; set; }

        /// <summary>
        /// Вывести заявки созданные до
        /// </summary>
        [Display(Name = "только заявки созданные до")]
        public DateTime? Before { get; set; }

        /// <summary>
        /// Сортировка по дате создания (новые вверху), если false то по дате редактирования (новые вверху)
        /// </summary>
        [Display(Name = "Сортировка по дате создания(в противном случае по дате редактирования)")]
        public bool SortByCreationTime { get; set; }

        public void Reset() {
            this.Status = null;
            this.isReturned = false;
            this.After = null;
            this.Before = null;
            SortByCreationTime = true;
        }
    }    
}