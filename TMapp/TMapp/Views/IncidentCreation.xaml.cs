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
    public partial class IncidentCreation : ContentPage
    {
        HttpClient FCliente = new HttpClient();
        //Configurations
        public DateTime FDatMax
        {
            get
            {
                return DateTime.Now;
            }

            set
            {

            }
        }

        public TimeSpan FHour { get; set; }
        public string FDescription { get; set; }
        public DateTime FDataHora { get; set; }
        public string FSelectedCategory { get; set; }
        public string FSelectedType { get; set; }
        public IncidentCategoryEnum LEnumSelectedCategory { get; set; }
        public IncidentCategory FCurrentCategory { get; set; }

        //Trazer do banco para um select.
        public IncidentType LType { get; set; }
        public User LAuthor { get; set; }
        public string LCountry { get; set; }
        public string LState { get; set; }
        public string LCity { get; set; }
        public double LPosX { get; set; }
        public double LPosY { get; set; }

        //
        ICollection<IncidentCategory> FCategories;


        public IncidentCreation(Position APosXY)
        {
            InitializeComponent();
            BindingContext = this;

            LPosX = APosXY.Latitude;
            LPosY = APosXY.Longitude;
            FDataHora = new DateTime();
            FDataHora = DateTime.Now;

            ICollection<IncidentCategory> GetCategories()
            {
                try
                {
                    string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Category/";
                    var response = FCliente.GetStringAsync(url);
                    response.Wait();
                    var res = response.Result;
                    var LResCategories = JsonConvert.DeserializeObject<ICollection<IncidentCategory>>(res);
                    return LResCategories;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            FCategories = GetCategories();

            //var LTask = GetCategoriesAsync();
            //LTask.Wait();

            //FCategories = LTask.Result;
        }

        public Incident CreateIncident()
        {
            Incident LIncident = new Incident();

            LIncident.IdCategory = FCurrentCategory.IdCategory;
            LIncident.Category = FCurrentCategory;

            LIncident.UserAuthor = App.CurrentUser;

            LIncident.Description = FDescription;
            FDataHora.Date.Add(FHour);
            LIncident.DataHora = FDataHora;

            LIncident.Country = LCountry;
            LIncident.State = LState;
            LIncident.City = LCity;
            LIncident.PosX = LPosX;
            LIncident.PosY = LPosY;

            return LIncident;
        }

        public IncidentCategoryEnum SetCategory()
        {

            if (FSelectedCategory.Equals("Violência"))
            {
                return IncidentCategoryEnum.Violence;
            }
            if (FSelectedCategory.Equals("Desastre"))
            {
                return IncidentCategoryEnum.Disaster;
            }
            if (FSelectedCategory.Equals("Evento"))
            {
                return IncidentCategoryEnum.Event;
            }
            if (FSelectedCategory.Equals("Trânsito"))
            {
                return IncidentCategoryEnum.Traffic;
            }
            if (FSelectedCategory.Equals("Ambiente"))
            {
                return IncidentCategoryEnum.Environment;
            }

            return IncidentCategoryEnum.Other;
        }

        public List<string> GetPickerOptions()
        {
            List<string> LTypeOptions = new List<string>();

            if (FSelectedCategory.Equals("Violência"))
            {
                LTypeOptions.Add("Assalto");
                LTypeOptions.Add("Roubo");
                LTypeOptions.Add("Furto");

            }
            if (FSelectedCategory.Equals("Desastre"))
            {
                LTypeOptions.Add("Enchente");

            }
            if (FSelectedCategory.Equals("Evento"))
            {
                LTypeOptions.Add("Festa");
                LTypeOptions.Add("Feira");

            }
            if (FSelectedCategory.Equals("Trânsito"))
            {
                LTypeOptions.Add("Trânsito");
                LTypeOptions.Add("Acidente");

            }
            if (FSelectedCategory.Equals("Ambiente"))
            {
                LTypeOptions.Add("Acumulo de lixo");
                LTypeOptions.Add("Mal cheiro");

            }

            return LTypeOptions;
        }


        private void pckCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FSelectedCategory = pckCategory.Items[pckCategory.SelectedIndex];

            LEnumSelectedCategory = SetCategory();

            if (pckCategory.SelectedIndex != -1)
            {
                pckCategory.SelectedIndex = -1;
            }

            List<string> LPickerOptions = GetPickerOptions();

            if (LPickerOptions.Count > 1)
            {
                foreach (var item in LPickerOptions)
                {
                    pckType.Items.Add(item);
                }
            }
            else
            {
                pckType.Items.Add(LPickerOptions.FirstOrDefault());
            }

        }

        private void pckType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FSelectedType = pckType.Items[pckType.SelectedIndex];

            if (FSelectedType.Equals("Assalto"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Assalto")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Roubo"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Roubo")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Furto"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Furto")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Enchente"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Enchente")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Festa"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Festa")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Feira"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Feira")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Trânsito"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Transito")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Acidente"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("Acidente")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Acumulo de lixo"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("AcumuloLixo")).FirstOrDefault();
            }
            if (FSelectedType.Equals("Mal cheiro"))
            {
                FCurrentCategory = FCategories.Where(c => c.CategoryName.Equals("MalCheiro")).FirstOrDefault();
            }
        }

        private void ButtonForm_Clicked(object sender, EventArgs e)
        {

            Incident LIncident = new Incident();

            FDataHora = dtpDateHour.Date;
            FDataHora = FDataHora.Add(FHour);

            LIncident.CreateIncident(FCurrentCategory, App.CurrentUser, FDescription, FDataHora, LPosX, LPosY);
            // async Task<ICollection<IncidentCategory>> IncidentForm()
            // {
            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Incident";
                var json = JsonConvert.SerializeObject(LIncident);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = FCliente.PostAsync(url, content);

                result.Wait();

                DisplayAlert("Aviso", "Ocorrência adicionada com sucesso!", "OK");
                Application.Current.MainPage.Navigation.PopAsync();
                IncidentFilter LFilter = new IncidentFilter();
                Application.Current.MainPage.Navigation.PushAsync(new MapPage(LFilter));
                Navigation.PopModalAsync();

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return;
            // }
        }
    }
}
