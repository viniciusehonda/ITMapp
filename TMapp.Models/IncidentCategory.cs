using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{
    public class IncidentCategory
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public IncidentCategoryEnum IncidentType { get; set; }
        public ICollection<Incident> Incidents { get; set; }
    }
}
