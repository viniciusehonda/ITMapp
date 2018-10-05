using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class Login : ContentPage
    {
        public HttpClient FCliente = new HttpClient();

        public Login()
        {
            InitializeComponent();
            pb_ProgressBar.IsVisible = false;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            var LSignUpPage = new SingUp();
            await Navigation.PushAsync(LSignUpPage);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            var user = new User
            {
                EMail = usernameEntry.Text,
                Password = passwordEntry.Text
            };
            pb_ProgressBar.Progress = 0;
            pb_ProgressBar.IsVisible = true;
            await pb_ProgressBar.ProgressTo(0.3, 250, Easing.SinIn);
            var isValid = CheckCredentials(user);
            await pb_ProgressBar.ProgressTo(1, 250, Easing.SinIn);
            if (isValid)
            {
                App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new MapPage(""), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }

        bool CheckCredentials(User user)
        {

            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/Login";
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = FCliente.PostAsync(url, content);
                pb_ProgressBar.ProgressTo(0.5, 250, Easing.SinIn);
                result.Wait();
                var res = result.Result.Content.ReadAsStringAsync();
                res.Wait();
                var LUser = JsonConvert.DeserializeObject<User>(res.Result);

                App.CurrentUser = LUser;

            }
            catch(Exception ex)
            {
                throw ex;
            }

            //Pb_ProgressBar.ProgressTo(.9, 250, Easing.SinIn);
            pb_ProgressBar.ProgressTo(0.8, 250, Easing.SinIn);
            return user.EMail == App.CurrentUser.EMail && user.Password == App.CurrentUser.Password;
        }

    }
}