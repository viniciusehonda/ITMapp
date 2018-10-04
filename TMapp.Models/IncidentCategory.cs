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

        public string GetIncidentTypeName()
        {
            if(IncidentType == IncidentCategoryEnum.Violence)
            {
                return "Violência";
            }
            else if(IncidentType == IncidentCategoryEnum.Disaster)
            {
                return "Desastre";
            }
            else if(IncidentType == IncidentCategoryEnum.Traffic)
            {
                return "Trânsito";
            }
            else if(IncidentType == IncidentCategoryEnum.Environment)
            {
                return "Ambiente";
            }
            else if(IncidentType == IncidentCategoryEnum.Event)
            {
                return "Evento";
            }
            else
            {
                return "";
            }
        }
    }
}
