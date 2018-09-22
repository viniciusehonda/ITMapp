using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{
    public enum IncidentTypeEnum
    {
        #region Violence
        Assalto,
        Roubo,
        Furto,
        #endregion
        #region Disaster
        Enchente,
        #endregion
        #region Event
        Festa,
        Feira,
        #endregion
        #region Traffic
        Transito,
        Acidente,
        #endregion
        #region Environment
        AcumuloLixo,
        MalCheiro
        #endregion
    }

    public enum IncidentCategoryEnum
    {
        Violence, //Robbery ... etc
        Disaster, //Flood ... etc
        Event, //Party, street market ...
        Traffic, //Accident ... etc
        Environment, //Trash, bad smell ... etc
        None
    }

    public class Enums
    {
    }
}
