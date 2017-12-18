using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace Simple_Helpdesk.Models
{
    public class RequestContext : DbContext
    {
        public RequestContext() : base("RequestsDataBase") { }

        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestDescription> Descriptions { get; set; }
    }
}