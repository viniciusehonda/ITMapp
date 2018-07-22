using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{
    public class User
    {
        public int IdUser { get; set; }
        public string Name { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
    }
}
