using Geocoding.Google;
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
    public partial class MapPage : ContentPage
    {
        public Position FStartingCoordinate { get; set; }

        public MapPage(string AEnderecoInicial)
        {
            InitializeComponent();

            Task Teste;
            List<Position> FCoordinates;
            FCoordinates = FindCoordinates(AEnderecoInicial);
            Position LPosition = FCoordinates.First();
            FStartingCoordinate = LPosition;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(LPosition, Distance.FromMiles(0.2)));

            User NewUser = new User();
            NewUser.IdUser = 1;
            NewUser.Name = "Vinicius";
            NewUser.City = "Sorocaba";

            IncidentType NewIncidentType = new IncidentType();
            NewIncidentType.IdType = 1;
            NewIncidentType.Category = IncidentCategory.Traffic;
            NewIncidentType.Name = "Transito";

            Incident NewIncident = new Incident();
            NewIncident.IdIncident = 1;
            NewIncident.Type = NewIncidentType;
            NewIncident.UserAuthor = NewUser;

            IncidentRating NewIncidentRating = new IncidentRating();
            NewIncidentRating.IdIncident = 1;
            NewIncidentRating.Incident = NewIncident;
            NewIncidentRating.RatingUser = NewUser;
            NewIncidentRating.VotePositive = true;

            //NewIncident.Rating.Add(NewIncidentRating);
            NewIncident.Description = "Aconteceu algo por aqui que eu nao sei explicar!";

            //Evento chamado toda vez que o mapa for clicado
            map.MapClicked += (sender, e) =>
            {
                var LLat = e.Point.Latitude;
                var LLong = e.Point.Longitude;
                Position LPos = new Position(e.Point.Longitude, e.Point.Longitude);


                AddIncident(LPos);

                var lat = e.Point.Latitude.ToString("0.000");
                var lng = e.Point.Longitude.ToString("0.000");

                this.DisplayAlert("MapClicked", $"{lat}/{lng}", "CLOSE");
            };

            map.PinClicked += (sender, e) =>
            {
                if (e.Pin != null)
                {
                    var LIncidentModal = new IncidentModal(e.Pin.Tag);

                    Teste = Navigation.PushModalAsync(LIncidentModal);
                }
            };

            
        }

        public void AddIncident(Position LPosXY)
        {
            var LIncidentCreation = new IncidentCreation(LPosXY);

            Task LProcess = Navigation.PushModalAsync(LIncidentCreation);

            Incident LNewIncident = LIncidentCreation.CreateIncident();

            Pin newPin = new Pin()
            {
                Type = PinType.Place,
                Label = LNewIncident.Type.Name,
                Address = LNewIncident.City,
                Position = new Position(LPosXY.Latitude, LPosXY.Longitude)
            };

            newPin.Tag = LNewIncident;

            map.Pins.Add(newPin);
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