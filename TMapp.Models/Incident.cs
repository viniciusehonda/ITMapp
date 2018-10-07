using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{

    public class Incident
    {
        public int IdIncident { get; set; }
        public int IdCategory { get; set; }
        public IncidentCategory Category { get; set;}
        public int IdUser { get; set; }
        public User UserAuthor { get; set; }
        public ICollection<IncidentRating> Ratings { get; set; }
        public string Description { get; set; }
        public DateTime? DataHora { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public bool Status { get; set; }

        public void CreateIncident(IncidentCategory ACategory, User AUserAuthor, string ADescription, DateTime? ADataHora, double APosX, double APosY)
        {

            this.Category = ACategory;
            this.IdCategory = ACategory.IdCategory;
            this.UserAuthor = AUserAuthor;
            this.IdUser = AUserAuthor.IdUser;
            this.Country = AUserAuthor.Country;
            this.State = AUserAuthor.State;
            this.City = AUserAuthor.City;
            this.Description = ADescription;
            this.DataHora = ADataHora;
            this.PosX = APosX;
            this.PosY = APosY;
            this.Status = true;
            
        }

        public void EditIncident(IncidentCategory ACategory, string ADescription, DateTime? ADataHora)
        {
            this.Category = ACategory;
            this.IdCategory = ACategory.IdCategory;
            this.Description = ADescription;
            this.DataHora = ADataHora;
        }

        public void DisableIncident()
        {
            this.Status = false;
        }
    }
}
