using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
	public partial class IncidentModal : ContentPage
	{
        public Incident LIncident { get; set; }
        public string LLabel { get; set; }
        public string VlblTipo { get; set; }
        public string VlblDescricao { get; set; }
        public int PositiveVote { get; set; }
        public int NegativeVote { get; set; }

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

                ICollection<IncidentRating> LRatings = GetRatings();

                int LPositive = LRatings.Where(o => o.PositiveVote == true).Count();
                int LNegtative = LRatings.Where(o => o.PositiveVote == false).Count();
                xLabelPositive.Text = LPositive.ToString();
                xLabelNegative.Text = LNegtative.ToString();


                if (App.CurrentUser != null)
                {
                    if (App.CurrentUser.IdUser == LIncident.IdUser || (LRatings.Where(u => u.IdUser == App.CurrentUser.IdUser).Count() > 0))
                    {
                        if (LRatings.Where(u => u.IdUser == App.CurrentUser.IdUser).Count() > 0)
                        {
                            if(LRatings.Where(u => u.IdUser == App.CurrentUser.IdUser).First().PositiveVote)
                            {
                                ButtonPositive.BorderColor = Color.Blue;
                            }
                            else
                            {
                                ButtonNegative.BorderColor = Color.Blue;
                            }
                        }

                        if(App.CurrentUser.IdUser == LIncident.IdUser)
                        {
                            ButtonDelete.IsVisible = true;
                        }

                        ButtonPositive.IsVisible = true;
                        ButtonPositive.IsEnabled = false;
                        ButtonNegative.IsVisible = true;
                        ButtonNegative.IsEnabled = false;

                    }
                    else
                    {

                        ButtonPositive.IsVisible = true;
                        ButtonPositive.IsEnabled = true;
                        ButtonNegative.IsVisible = true;
                        ButtonNegative.IsEnabled = true;
                    }
                }

            }


        }

        public void ButtonPositive_Clicked(object sender, EventArgs e)
        {
            HttpClient FCliente = new HttpClient();

            IncidentRating LRating = new IncidentRating();
            LRating.CreateRating(App.CurrentUser, LIncident);
            LRating.VotePositive();

            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/IncidentRating";
                var json = JsonConvert.SerializeObject(LRating);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = FCliente.PostAsync(url, content);
                res.Wait();
            }
            catch(Exception ex)
            {
                throw ex;
            }
             
        }

        public void ButtonNegative_Clicked(object sender, EventArgs e)
        {
            HttpClient FCliente = new HttpClient();

            IncidentRating LRating = new IncidentRating();
            LRating.CreateRating(App.CurrentUser, LIncident);
            LRating.VoteNegative();

            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/IncidentRating";
                var json = JsonConvert.SerializeObject(LRating);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var res = FCliente.PostAsync(url, content);
                res.Wait();
            }
            catch(Exception ex)
            {
                throw ex;
            }
           
        }

        public void ButtonDelete_Clicked(object sender, EventArgs e)
        {
            HttpClient FCliente = new HttpClient();

            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Incident/" + LIncident.IdIncident;
                var res = FCliente.DeleteAsync(url);
                res.Wait();

                Application.Current.MainPage.Navigation.PopAsync();
                IncidentFilter LFilter = new IncidentFilter();
                Application.Current.MainPage.Navigation.PushAsync(new MapPage(LFilter));
                Navigation.PopModalAsync();

            }
            catch(Exception ex)
            {
                throw ex;
            }
            

        }

        public ICollection<IncidentRating> GetRatings()
        {
            HttpClient FCliente = new HttpClient();

            string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/IncidentRating/" + LIncident.IdIncident;

            var Response = FCliente.GetAsync(url);
            Response.Wait();
            var res = Response.Result.Content.ReadAsStringAsync();
            res.Wait();
            return JsonConvert.DeserializeObject<ICollection<IncidentRating>>(res.Result);


        }

    }
}