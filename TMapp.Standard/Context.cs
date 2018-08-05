using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TMapp.Models;

namespace TMapp.Standard
{
    public class Context : DbContext
    {
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentRating> IncidentRatings { get; set; }
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<User> Users { get; set; }

        public readonly string _databasePath;

        public Context(string AdatabasePath)
        {
            _databasePath = AdatabasePath;

            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder AOptionsBuilder)
        {
            AOptionsBuilder.UseSqlite($"Filename={_databasePath}");
        }
    }
}
