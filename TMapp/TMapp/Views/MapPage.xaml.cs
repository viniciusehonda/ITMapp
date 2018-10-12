using Geocoding.Google;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMapp.Helpers;
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

        public MapPage(IncidentFilter AFilter)
        {
            InitializeComponent();

            Task FTask;
            List<Position> FCoordinates;
            FCoordinates = FindCoordinates("");
            Position LPosition = FCoordinates.First();
            FStartingCoordinate = LPosition;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(LPosition, Distance.FromMiles(0.2)));
            map.Pins.Clear();
            LoadIncidents(AFilter);

            #region MapEvents
            //Evento chamado toda vez que o mapa for clicado
            map.MapClicked += (sender, e) =>
            {
                var LLat = e.Point.Latitude;
                var LLong = e.Point.Longitude;
                Position LPos = new Position(e.Point.Latitude, e.Point.Longitude);

                AddIncident(LPos);

            };

            //Evento quando o pin for clicado
            map.PinClicked += (sender, e) =>
            {
                if (e.Pin != null)
                {
                    var LIncidentModal = new IncidentModal(e.Pin.Tag);
                    //new IncidentModal(e.Pin.Tag);
                    Navigation.PushModalAsync(LIncidentModal);
                    //FTask = Navigation.PushModalAsync(LIncidentModal);
                    //FTask.Dispose();
                }
            };

            #endregion
        }

        public void OnFilterButtonClicked(object sender, EventArgs e)
        {
            Task LTask;
            var LFilterModal = new FilterModal();
            LTask = Navigation.PushModalAsync(LFilterModal);
            //LTask.Wait();
            if (LTask.IsCompleted)
                LTask.Dispose();
        }

        public void OnLoginButtonClicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new Login(), this);
            Application.Current.MainPage.Navigation.PopAsync();
        }

        public void LoadIncidents(IncidentFilter AFilters)
        {
            ICollection<Incident> LReqIncidents;
            if (AFilters.IdCategory.HasValue || AFilters.DateStart != null)
            {
                LReqIncidents = GetFilteredIncidents(AFilters);
            }
            else
            {
                LReqIncidents = GetIncidents();
            }

            if (LReqIncidents != null)
            {
                if (LReqIncidents.Count > 0)
                {
                    foreach (var Incident in LReqIncidents)
                    {
                        var LCategory = GetCategoryById(Incident.IdCategory);

                        Incident.Category = LCategory;

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
            }


        }

        public void AddIncident(Position LPosXY)
        {
            var LIncidentCreation = new IncidentCreation(LPosXY);

            Task LProcess = Navigation.PushModalAsync(LIncidentCreation);

            map.Pins.Clear();
            //LProcess.Dispose();
            //IncidentFilter LFilter = new IncidentFilter();
            //LoadIncidents(LFilter);
        }

        public ICollection<Incident> GetIncidents()
        {
            HttpClient FCliente = new HttpClient();
            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Incident";
                var response = FCliente.GetStringAsync(url);
                response.Wait();
                var res = response.Result;
                var Incidents = JsonConvert.DeserializeObject<ICollection<Incident>>(res);

                //response.Dispose();
                //FCliente.Dispose();
                return Incidents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<Incident> GetFilteredIncidents(IncidentFilter AFilters)
        {
            HttpClient FCliente = new HttpClient();
            try
            {
                var teste = JsonConvert.DeserializeObject<DateTime>(AFilters.DateStart);
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/FilteredIncident";
                var json = JsonConvert.SerializeObject(AFilters);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = FCliente.PostAsync(url, content);
                result.Wait();
                var res = result.Result.Content.ReadAsStringAsync();
                if (res.Result != null)
                {
                    res.Wait();
                    var LIncidents = JsonConvert.DeserializeObject<ICollection<Incident>>(res.Result);
                    res.Dispose();
                    FCliente.Dispose();

                    return LIncidents;
                }
                else
                {
                    IEnumerable<Incident> LIncident = Enumerable.Empty<Incident>();
                    return LIncident.ToList();
                }

                //return LIncidents;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICollection<IncidentCategory> GetCategories()
        {
            HttpClient FCliente = new HttpClient();
            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Category";
                var response = FCliente.GetStringAsync(url);
                response.Wait();
                var res = response.Result;
                var categories = JsonConvert.DeserializeObject<ICollection<IncidentCategory>>(res);

                //response.Dispose();
                //FCliente.Dispose();
                return categories;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IncidentCategory GetCategoryById(int LId)
        {
            HttpClient FCliente = new HttpClient();
            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Category/" + LId.ToString();
                var response = FCliente.GetAsync(url);
                response.Wait();
                var res = response.Result.Content.ReadAsStringAsync();
                res.Wait();
                var Categories = JsonConvert.DeserializeObject<IncidentCategory>(res.Result);

                //response.Dispose();
                //FCliente.Dispose();
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

        public BitmapDescriptor PinImageDispatcher(int LIncidentType)
        {
            if (LIncidentType == 0)
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
            else if (LIncidentType == 1)
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
            else if (LIncidentType == 2)
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
            else if (LIncidentType == 3)
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
            else if (LIncidentType == 4)
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
    }
}