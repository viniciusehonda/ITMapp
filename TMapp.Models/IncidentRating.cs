﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TMapp.Models
{
    public class IncidentRating
    {
        public int IdRating { get; set; }
        public int IdIncident { get; set; }
        public Incident Incident { get; set; }
        public int IdUser { get; set; }
        public User RatingUser { get; set; }
        public bool PositiveVote { get; set; }

        public void CreateRating(User ARatingUser, Incident AIncident)
        {

            this.IdIncident = AIncident.IdIncident;
            this.IdUser = ARatingUser.IdUser;

        }

        public void VotePositive()
        {

            this.PositiveVote = true;
        }

        public void VoteNegative()
        {

            this.PositiveVote = false;
        }
    }
}
