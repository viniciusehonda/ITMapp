using Geocoding.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

            LoadIncidents();

            //Evento chamado toda vez que o mapa for clicado
            map.MapClicked += (sender, e) =>
            {
                var LLat = e.Point.Latitude;
                var LLong = e.Point.Longitude;
                Position LPos = new Position(e.Point.Latitude, e.Point.Longitude);

                AddIncident(LPos);

                //var lat = e.Point.Latitude.ToString("0.000");
                //var lng = e.Point.Longitude.ToString("0.000");

                //this.DisplayAlert("MapClicked", $"{lat}/{lng}", "CLOSE");
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

        public BitmapDescriptor PinImageDispatcher(int LIncidentType)
        {
            if(LIncidentType >= 0 && LIncidentType <= 2)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    return BitmapDescriptorFactory.FromBundle("ViolencePin");
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    return BitmapDescriptorFactory.FromBundle("ViolencePin.png");
                }
            }
            else if(LIncidentType == 3)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    return BitmapDescriptorFactory.FromBundle("DisasterPin");
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    return BitmapDescriptorFactory.FromBundle("DisasterPinBlue.png");
                }
            }
            else if(LIncidentType >= 4 && LIncidentType <= 5)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    return BitmapDescriptorFactory.FromBundle("EventPin");
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    return BitmapDescriptorFactory.FromBundle("EventPin.png");
                }
            }
            else if(LIncidentType >= 6 && LIncidentType <= 7)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    return BitmapDescriptorFactory.FromBundle("TrafficPin");
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    return BitmapDescriptorFactory.FromBundle("TrafficPin.png");
                }
            }
            else if(LIncidentType >= 8 && LIncidentType <= 9)
            {
                if (Device.RuntimePlatform.Equals(Device.iOS))
                {
                    return BitmapDescriptorFactory.FromBundle("EnvironmentPin");
                }
                else if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    return BitmapDescriptorFactory.FromBundle("EnvironmentPin.png");
                }
            }
                return BitmapDescriptorFactory.DefaultMarker(Color.White);
            
        }

        public void LoadIncidents()
        {
            var LReqIncidents = GetIncidentsAsync();
            LReqIncidents.Wait();
            var LIncidents = LReqIncidents.Result;

            foreach(var Incident in LIncidents)
            {
                //BitmapDescriptor LPin = BitmapDescriptorFactory.DefaultMarker(Color.White);

                //LPin = PinImageDispatcher((int)Incident.Category.IncidentType);

                var LCategory = GetCategoryById(Incident.IdCategory);
                LCategory.Wait();
                Incident.Category = LCategory.Result;

                Pin newPin = new Pin()
                {
                    Type = PinType.Place,
                    Label = Incident.Description,
                    Address = Incident.City,
                    Icon = PinImageDispatcher((int)Incident.Category.IncidentType),
                    Position = new Position(Incident.PosX, Incident.PosY)
                };

                newPin.Tag = Incident;

                map.Pins.Add(newPin);
            }
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
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Incident/";
                var response = FCliente.GetStringAsync(url);
                response.Wait();
                var res = response.Result;
                var Incidents = JsonConvert.DeserializeObject<ICollection<Incident>>(res);
                return Incidents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IncidentCategory> GetCategoryById(int LId)
        {
            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Category/" + LId.ToString();
                var response = FCliente.GetStringAsync(url);
                response.Wait();
                var res = response.Result;
                var Categories = JsonConvert.DeserializeObject<IncidentCategory>(res);
                return Categories;
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