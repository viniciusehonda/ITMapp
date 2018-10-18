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
using Xamarin.Forms.Xaml;

namespace TMapp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IncidentEdit : ContentPage
	{
        public Incident FIncident { get; set; }
        public IncidentCategory FCurrentCategory { get; set; }
        public ICollection<IncidentCategory> FCategories { get; set; }
        public DateTime FDatMax { get; set; }
        public string FDescription { get; set; }
        public TimeSpan FHour { get; set; }
        public DateTime FDataHora { get; set; }
        public string FSelectedCategory { get; set; }
        public string FSelectedType { get; set; }
        public IncidentCategoryEnum LEnumSelectedCategory { get; set; }

        public IncidentEdit (Incident AIncident)
		{
			InitializeComponent ();
            FCategories = GetCategories();
            FDatMax = DateTime.Now;
            FIncident = AIncident;
            FDescription = AIncident.Description;
            xDescription.Text = FDescription;
            FDataHora = AIncident.DataHora.Value;
            dtpDateHour.Date = FDataHora;
            FHour = AIncident.DataHora.Value.TimeOfDay;
            tpHour.Time = FHour;
            
            if(AIncident.Category.IncidentType == IncidentCategoryEnum.Violence)
            {
                pckCategory.SelectedIndex = 0;
            }
            else if(AIncident.Category.IncidentType == IncidentCategoryEnum.Disaster)
            {
                pckCategory.SelectedIndex = 1;
            }
            else if (AIncident.Category.IncidentType == IncidentCategoryEnum.Event)
            {
                pckCategory.SelectedIndex = 2;
            }
            else if (AIncident.Category.IncidentType == IncidentCategoryEnum.Traffic)
            {
                pckCategory.SelectedIndex = 3;
            }
            else if (AIncident.Category.IncidentType == IncidentCategoryEnum.Environment)
            {
                pckCategory.SelectedIndex = 4;
            }

            FSelectedCategory = AIncident.Category.CategoryName;
            List<string> LPickerOptions = GetPickerOptions();

            foreach(var LItem in LPickerOptions)
            {
                pckType.Items.Add(LItem);
            }

            if (AIncident.Category.CategoryName.Equals("Assalto"))
            {
                pckType.SelectedIndex = 0;
            }
            else if(AIncident.Category.CategoryName.Equals("Roubo"))
            {
                pckType.SelectedIndex = 1;
            }
            else if (AIncident.Category.CategoryName.Equals("Furto"))
            {
                pckType.SelectedIndex = 2;
            }
            else if (AIncident.Category.CategoryName.Equals("Enchente"))
            {
                pckType.SelectedIndex = 0;
            }
            else if (AIncident.Category.CategoryName.Equals("Festa"))
            {
                pckType.SelectedIndex = 0;
            }
            else if (AIncident.Category.CategoryName.Equals("Feira"))
            {
                pckType.SelectedIndex = 1;
            }
            else if (AIncident.Category.CategoryName.Equals("Transito"))
            {
                pckType.SelectedIndex = 0;
            }
            else if(AIncident.Category.CategoryName.Equals("Acidente"))
            {
                pckType.SelectedIndex = 1;
            }
            else if (AIncident.Category.CategoryName.Equals("AcumuloLixo"))
            {
                pckType.SelectedIndex = 0;
            }
            else if (AIncident.Category.CategoryName.Equals("MalCheiro"))
            {
                pckType.SelectedIndex = 1;
            }

        }


        public void ButtonForm_Clicked(object sender, EventArgs e)
        {
            //Substitui os valores da classe pelos valores dos campos
            FDescription = xDescription.Text;
            FIncident.Description = FDescription;
            FDataHora = FDataHora.Date;
            FDataHora.Add(FHour);
            FIncident.DataHora = FDataHora;
            FIncident.IdCategory = FCurrentCategory.IdCategory;
            FIncident.Category = FCurrentCategory;

            try
            {
                HttpClient FCliente = new HttpClient();
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/IncidentEdit";
                var json = JsonConvert.SerializeObject(FIncident);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = FCliente.PostAsync(url, content);

                result.Wait();

                DisplayAlert("Aviso", "Ocorrência alterada com sucesso!", "OK");
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

        ICollection<IncidentCategory> GetCategories()
        {
            HttpClient FCliente = new HttpClient();
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

        private void pckCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            FSelectedCategory = pckCategory.Items[pckCategory.SelectedIndex];

            LEnumSelectedCategory = SetCategory();

            //if(pckCategory.SelectedIndex != -1)
            //{
            //    pckCategory.SelectedIndex = -1;
            //}

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

    }
}