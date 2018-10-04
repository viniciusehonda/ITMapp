using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace TMapp
{
	public partial class MainPage : ContentPage
	{
		public string FRua { get; set; }
		public string FCidade { get; set; }
		public string FEndereco { get; set; }

		public MainPage()
		{
			InitializeComponent();
			BindingContext = this;
		}

		private void ButtonBuscar_Clicked(object sender, EventArgs e)
		{
            //FEndereco = String.Format(FRua + " " + FCidade);
            Navigation.PushAsync(new MapPage(""));

		}
	}
}
