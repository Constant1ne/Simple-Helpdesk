using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simple_Helpdesk.Models
{
    /// <summary>
    /// Класс представляющий заявку
    /// </summary>
    public class Request
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<RequestDescription> Descriptions { get; set; }
    }

    /// <summary>
    /// Класс представляющий состояние заявки
    /// </summary>
    public class RequestDescription
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime ModificationTime { get; set; }
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