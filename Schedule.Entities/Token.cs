using System;
using System.Collections.Generic;
using System.Text;

namespace Schedule.Entities
{
    public class Token
    {
        public string Username { get; set; }
        public string AuthenticationToken { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiricyDate { get; set; }
    }
}
