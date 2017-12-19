using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace Simple_Helpdesk.Models
{
    /// <summary>
    /// Класс контекста данных для Entity Framework
    /// </summary>
    public class RequestContext : DbContext
    {
        // имя базы данных, если не найдена, то создается по умолчанию в /App_Data
        public RequestContext() : base("RequestsDataBase") { }

        // отображения таблиц базы данных на нижеописанные классы
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestDescription> Descriptions { get; set; }
    }
}