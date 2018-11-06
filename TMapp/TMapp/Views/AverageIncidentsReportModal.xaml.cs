using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMapp.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TMapp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AverageIncidentsReportModal : ContentPage
	{
		public AverageIncidentsReportModal ()
		{
			InitializeComponent ();

            ICollection<AverageDataHelper> LAverages = GetCategories();
            //var Total = LAverages.Sum(a => a.Count);
            //xTotal.Text = Total.ToString();
            //xTotalAssalto.Text = (LAverages.Where(a => a.IdCategory == 1).FirstOrDefault().Count / Total).ToString() + "%";
            //xTotalRoubo.Text = (LAverages.Where(a => a.IdCategory == 2).FirstOrDefault().Count / Total).ToString() + "%";
            //xTotalFurto.Text = (LAverages.Where(a => a.IdCategory == 3).FirstOrDefault().Count / Total).ToString() + "%";

            //xTotalEnchente.Text = (LAverages.Where(a => a.IdCategory == 4).FirstOrDefault().Count / Total).ToString() + "%";

            //xTotalFesta.Text = (LAverages.Where(a => a.IdCategory == 5).FirstOrDefault().Count / Total).ToString() + "%";
            //xTotalFeira.Text = (LAverages.Where(a => a.IdCategory == 6).FirstOrDefault().Count / Total).ToString() + "%";

            //xTotalTransito.Text = (LAverages.Where(a => a.IdCategory == 7).FirstOrDefault().Count / Total).ToString() + "%";
            //xTotalAcidente.Text = (LAverages.Where(a => a.IdCategory == 8).FirstOrDefault().Count / Total).ToString() + "%";

            //xTotalAcumulo.Text = (LAverages.Where(a => a.IdCategory == 9).FirstOrDefault().Count / Total).ToString() + "%";
            //xTotalCheiro.Text = (LAverages.Where(a => a.IdCategory == 10).FirstOrDefault().Count / Total).ToString() + "%";

        }

        public ICollection<AverageDataHelper> GetCategories()
        {
            HttpClient FCliente = new HttpClient();
            try
            {
                string url = "https://tmappwebapi20180922043720.azurewebsites.net/api/AverageIncidentsReport";
                var response = FCliente.GetStringAsync(url);
                response.Wait();
                var res = response.Result;
                var Averages = JsonConvert.DeserializeObject<ICollection<AverageDataHelper>>(res);

                return Averages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}