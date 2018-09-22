using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{
    public class IncidentType
    {
        public int IdType { get; set; }
        public string TypeName { get; set; }
        public IncidentCategory Category { get; set; }

    }
}
