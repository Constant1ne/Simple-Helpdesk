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
            this.ResetFields();
        }
        
        /// <summary>
        /// Вывести заявки у которых текущий статус равен указанному значению
        /// </summary>
        [Display(Name = "Отображать заявки в состоянии:")]
        public RequestStatus Status { get; set; }
        
        /// <summary>
        /// Показывать заявки которые хоть раз были "возвращены" в обработку
        /// </summary>
        [Display(Name = "Включить в выборку только те заявки, которые хоть раз возвращались на доработку")]
        public bool isReturned { get; set; }

        /// <summary>
        /// Вывести заявки созданные после
        /// </summary>
        [Display(Name = "Только завки созданные после")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime? After { get; set; }

        /// <summary>
        /// Вывести заявки созданные до
        /// </summary>
        [Display(Name = "только заявки созданные до")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy H:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime)]
        public DateTime? Before { get; set; }

        /// <summary>
        /// Сортировка по дате создания (новые вверху), если false то по дате редактирования (новые вверху)
        /// </summary>
        [Display(Name = "Сортировка по дате создания или редактирования(от новых к более старым)")]
        public bool SortByCreationTime { get; set; }

        public void ResetFields() {
            this.Status = RequestStatus.Undefined;
            this.isReturned = false;
            this.After = null;
            this.Before = null;
            SortByCreationTime = true;
        }
    }
}