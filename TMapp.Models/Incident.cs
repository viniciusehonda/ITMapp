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

    }
}
