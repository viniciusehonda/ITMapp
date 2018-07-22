using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{
    //public enum IncidentType
    //{
    //    #region Violence
    //    Assalto,
    //    Roubo,
    //    Furto,
    //    #endregion
    //    #region Disaster
    //    Enchente,
    //    #endregion
    //    #region Event
    //    Festa,
    //    Feira,
    //    #endregion
    //    #region Traffic
    //    Transito,
    //    Acidente,
    //    #endregion
    //    #region Environment
    //    GarbageAccu
    //    #endregion
    //}

    public class Incident
    {
        public int IdIncident { get; set; }
        public int IdType { get; set; }
        public IncidentType Type { get; set;}
        public int IdUser { get; set; }
        public User UserAuthor { get; set; }
        public IList<IncidentRating> Rating { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }

    }
}
