using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMapp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMapp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingUp : ContentPage
	{
        public string FName { get; set; }
        public string FEmail { get; set; }
        public string FPassword { get; set; }
        public string FCountry { get; set; }
        public string FState { get; set; }
        public string FCity { get; set; }

        public SingUp ()
		{
			InitializeComponent ();
            pb_ProgressBar.IsVisible = false;
            xPassword.IsPassword = true;
		}

        async void ButtonForm_Clicked(object sender, EventArgs e)
        {
            pb_ProgressBar.IsVisible = true;
            pb_ProgressBar.Progress = 0.1;
            User LUser = new User();

            LUser.CreateUser(xName.Text, xEmail.Text, xPassword.Text, xCountry.Text, xState.Text, xCity.Text);

            if(LUser.Name.Length > 0 && LUser.EMail.Length > 0 && LUser.Password.Length > 0 && LUser.Country.Length > 0 && LUser.State.Length > 0 && LUser.City.Length > 0)
            {
                //LUser.CreateUser(FName, FEmail, FPassword, FCountry, FState, FCity);

                try
                {
                    HttpClient FCliente = new HttpClient();
                    pb_ProgressBar.Progress = 0.3;
                    string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/User";
                    var json = JsonConvert.SerializeObject(LUser);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = FCliente.PostAsync(url, content);

                    result.Wait();
                    await pb_ProgressBar.ProgressTo(1, 250, Easing.SinIn);
                    //FCliente.Dispose();
                    await DisplayAlert("Aviso", "Cadastrado com sucesso!", "OK");
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
                catch(Exception ex)
                {

                    throw ex;
                }
            }
            else
            {
                pb_ProgressBar.Progress = 0;
                pb_ProgressBar.IsVisible = false;
                await DisplayAlert("Aviso", "Todos os campos devem ser preenchidos.", "OK");
            }

        }
    }
}