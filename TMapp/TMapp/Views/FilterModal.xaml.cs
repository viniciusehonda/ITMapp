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

/*
                     <x:String>Todos</x:String>
                    <x:String>Violência</x:String>
                    <x:String>Desastre</x:String>
                    <x:String>Evento</x:String>
                    <x:String>Trânsito</x:String>
                    <x:String>Ambiente</x:String>
                    */

namespace TMapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterModal : ContentPage
    {
        public int? FIdCategory { get; set; }
        public DateTime? FDateStart { get; set; }
        HttpClient FCliente = new HttpClient();
        ICollection<IncidentCategory> FCategories;
        IncidentCategory SelectedCategory = new IncidentCategory();
        bool DateSelected = false;

        public FilterModal()
        {
            InitializeComponent();

            pckCategory.Items.Add("Todos");
            FCategories = GetCategories();

            foreach (var LCate in FCategories)
            {

                pckCategory.Items.Add(LCate.CategoryName);
            }

        }

        private void dtpDateHour_Selected(object sender, EventArgs e)
        {
            FDateStart = dtpDateHour.Date;
            DateSelected = true;
        }

        private void pckCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FSelectedCategory = pckCategory.Items[pckCategory.SelectedIndex];

            if (FSelectedCategory.Equals("Assalto") || FSelectedCategory.Equals("Roubo") || FSelectedCategory.Equals("Furto"))
            {
                SelectedCategory = FCategories.Where(c => c.IncidentType == IncidentCategoryEnum.Violence).First();
            }
            else if (FSelectedCategory.Equals("Enchente"))
            {
                SelectedCategory = FCategories.Where(c => c.IncidentType == IncidentCategoryEnum.Disaster).First();
            }
            else if (FSelectedCategory.Equals("Festa") || FSelectedCategory.Equals("Feira"))
            {
                SelectedCategory = FCategories.Where(c => c.IncidentType == IncidentCategoryEnum.Event).First();
            }
            else if (FSelectedCategory.Equals("Transito") || FSelectedCategory.Equals("Acidente"))
            {
                SelectedCategory = FCategories.Where(c => c.IncidentType == IncidentCategoryEnum.Traffic).First();
            }
            else if (FSelectedCategory.Equals("AcumuloLixo") || FSelectedCategory.Equals("MalCheiro"))
            {
                SelectedCategory = FCategories.Where(c => c.IncidentType == IncidentCategoryEnum.Event).First();
            }
            else
            {
                SelectedCategory = null;
            }
        }

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

        void FilterButton_Clicked(object sender, EventArgs e)
        {
            IncidentFilter LFilter = new IncidentFilter();

            if(SelectedCategory == null)
            {
                LFilter.IdCategory = null;
            }
            else
            {
                LFilter.IdCategory = SelectedCategory.IdCategory;
            }

            if(DateSelected == true)
            {
                LFilter.DateStart = JsonConvert.SerializeObject(FDateStart.Value);
            }
            else
            {
                LFilter.DateStart = null;
            }

            DisplayAlert("Aviso", "Filtro Aplicado!", "OK");
            Application.Current.MainPage.Navigation.PopAsync();
            Application.Current.MainPage.Navigation.PushAsync(new MapPage(LFilter));
            Navigation.PopModalAsync();
        }
    }
}