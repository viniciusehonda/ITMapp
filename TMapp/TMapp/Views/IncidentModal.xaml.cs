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
        public string LLabel { get; set; }

		public IncidentModal(Object APin)
		{
            if (APin != null)
            {
                var LPin = (Incident)APin;
                LLabel = LPin.Description;
            }

            InitializeComponent();

            xLabel.Text = LLabel;

		}
	}
}