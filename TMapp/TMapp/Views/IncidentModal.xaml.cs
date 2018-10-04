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
	public partial class IncidentModal : ContentPage
	{
        public Incident LIncident { get; set; }
        public string LLabel { get; set; }
        public string VlblTipo { get; set; }
        public string VlblDescricao { get; set; }

		public IncidentModal(Object APin)
		{

            InitializeComponent();

            if (APin != null)
            {
                LIncident = (Incident)APin;
                xLblTipo.Text = LIncident.Category.CategoryName;
                xLblDescricao.Text = LIncident.Description;
                xLblData.Text = LIncident.DataHora.Value.Date.ToString("dd/MM/yyyy");
                xLblHora.Text = LIncident.DataHora.Value.TimeOfDay.ToString();
            }

        }
	}
}