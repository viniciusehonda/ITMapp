using Geocoding.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace TMapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public Position FStartingCoordinate {get; set;}

		public MapPage (string AEnderecoInicial)
		{
			InitializeComponent();

			List<Position> FCoordinates;
			FCoordinates = FindCoordinates(AEnderecoInicial);
			Position LPosition = FCoordinates.First();
            FStartingCoordinate = LPosition;
			map.MoveToRegion(MapSpan.FromCenterAndRadius(LPosition, Distance.FromMiles(0.2)));
			
			//Evento chamado toda vez que o mapa for clicado
			map.MapClicked += (sender, e) =>
			{
                var LLat = e.Point.Latitude;
                var LLong = e.Point.Longitude;

                var lat = e.Point.Latitude.ToString("0.000");
				var lng = e.Point.Longitude.ToString("0.000");

                Pin newPin = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Um lugar qualquer",
                    Address = AEnderecoInicial,
                    Position = new Position(LLat, LLong)
                };
                map.Pins.Add(newPin);
                this.DisplayAlert("MapClicked", $"{lat}/{lng}", "CLOSE");
            };
		}

		//Metodo devolve as posicoes encontradas,
		//utilizando como argumento uma string com  o endereco
		public List<Position> FindCoordinates(string AEndereco)
		{
			List<Position> LResults = new List<Position>();
			Position LPosition;
			GoogleGeocoder LGeocoder = new GoogleGeocoder();
			Task<IEnumerable<GoogleAddress>> LPesquisa = LGeocoder.GeocodeAsync(AEndereco);
			LPesquisa.Wait();
			foreach (GoogleAddress Endereco in LPesquisa.Result)
			{
				LPosition = new Position(Endereco.Coordinates.Latitude, Endereco.Coordinates.Longitude);
				LResults.Add(LPosition);
			}
			return LResults;
		}
	}
}