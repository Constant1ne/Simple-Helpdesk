using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simple_Helpdesk.Models
{
    public class Error
    {
        public Error() { }
        public Error(string message) {
            this.Message = message;
        }
        public string Message { get; set; }
    }
}