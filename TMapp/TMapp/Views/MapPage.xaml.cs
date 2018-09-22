using Geocoding.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

           //var LReqIncidents = GetIncidentsAsync();
           // ICollection<Incident> LIncidents = LReqIncidents.Result;

           // foreach(var item in LIncidents)
           // {
           // }
           // User NewUser = new User();
           // NewUser.IdUser = 1;
           // NewUser.Name = "Vinicius";
           // NewUser.City = "Sorocaba";

           // IncidentType NewIncidentType = new IncidentType();
           // NewIncidentType.IdType = 1;
           // NewIncidentType.TypeName = "Transito";

           // Incident NewIncident = new Incident();
           // NewIncident.IdIncident = 1;
           // NewIncident.UserAuthor = NewUser;

           // IncidentRating NewIncidentRating = new IncidentRating();
           // NewIncidentRating.IdIncident = 1;
           // NewIncidentRating.Incident = NewIncident;
           // NewIncidentRating.RatingUser = NewUser;
           // NewIncidentRating.PositiveVote = true;

            //NewIncident.Rating.Add(NewIncidentRating);
            //NewIncident.Description = "Aconteceu algo por aqui que eu nao sei explicar!";

            //Evento chamado toda vez que o mapa for clicado
            map.MapClicked += (sender, e) =>
            {
                var LLat = e.Point.Latitude;
                var LLong = e.Point.Longitude;
                Position LPos = new Position(e.Point.Longitude, e.Point.Longitude);

                //AddIncident(LPos);

                var LIncidentCreation = new IncidentCreation(LPos);

                //var form = new NavigationPage(new IncidentCreation(LPos));
                Task LProcess = Navigation.PushAsync(LIncidentCreation);
                LProcess.Wait();

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
            //Incident LNewIncident = LIncidentCreation.CreateIncident();

            //Pin newPin = new Pin()
            //{
            //    Type = PinType.Place,
            //    //Label = LNewIncident.Type.TypeName,
            //    Address = LNewIncident.City,
            //    Position = new Position(LPosXY.Latitude, LPosXY.Longitude)
            //};

            //newPin.Tag = LNewIncident;

            //map.Pins.Add(newPin);
        }

        HttpClient FCliente = new HttpClient();
        public async Task<ICollection<Incident>> GetIncidentsAsync()
        {
            try
            {
                string url = "http://localhost:65029/api/Incident/";
                var response = await FCliente.GetStringAsync(url);
                var Incidents = JsonConvert.DeserializeObject<ICollection<Incident>>(response);
                return Incidents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Metodo devolve as posicoes encontradas,
        //utilizando como argumento uma string com  o endereco
        public List<Position> FindCoordinates(string AEndereco)
        {
            AEndereco = "Sorocaba, State of São Paulo, Brazil";
            List<Position> LResults = new List<Position>();
            Position LPosition = new Position(-23.5015299, -47.45256029999996);
            //GoogleGeocoder LGeocoder = new GoogleGeocoder();
            //LGeocoder.ApiKey = "AIzaSyDt42XuK3ltzmuy01R0jnj3H3br4e2ieZQ";

            //Task<IEnumerable<GoogleAddress>> LPesquisa = LGeocoder.GeocodeAsync(AEndereco);
            //LPesquisa.Wait();
            //var teste = LPesquisa.Result;
            //foreach (GoogleAddress Endereco in LPesquisa.Result)
            //{
            //  LPosition = new Position(Endereco.Coordinates.Latitude, Endereco.Coordinates.Longitude);
            //LResults.Add(LPosition);
            //}
            LResults.Add(LPosition);

            return LResults;
        }
    }
}