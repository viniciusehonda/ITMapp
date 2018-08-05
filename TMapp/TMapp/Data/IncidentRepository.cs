using System;
using System.Collections.Generic;
using System.Text;
using TMapp.Data.Data.Interfaces;
using TMapp.Standard;

namespace TMapp.Data
{
    public class IncidentRepository : IIncidentRepository
    {

        private readonly Context _databaseContext;

        public IncidentRepository(string ADbPath)
        {
            _databaseContext = new Context(ADbPath);
        }
    }
}
