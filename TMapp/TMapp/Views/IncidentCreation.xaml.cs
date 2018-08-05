using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMapp.Models;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TMapp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IncidentCreation : ContentPage
	{
        //Trazer do banco para um select.
        public IncidentType LType { get; set; }
        public User LAuthor { get; set; }
        public string LDescription { get; set; }
        public string LCountry { get; set; }
        public string LState { get; set; }
        public string LCity { get; set; }
        public double LPosX { get; set; }
        public double LPosY { get; set; }


		public IncidentCreation (Position APosXY)
		{
			InitializeComponent ();
		}

        public Incident CreateIncident()
        {
            Incident LIncident = new Incident();

            LIncident.Type = LType;
            LIncident.UserAuthor = LAuthor;
            LIncident.Description = LDescription;
            LIncident.Country = LCountry;
            LIncident.State = LState;
            LIncident.City = LCity;
            LIncident.PosX = LPosX;
            LIncident.PosY = LPosY;

            return LIncident;
        }
	}
}