using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PeopleWith;

public partial class DietInfo : ContentPage
{
    Information infoSource = new Information();
    userdiet DietPassed = new userdiet();
    diet Dietinformation = new diet();

    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    public HttpClient Client = new HttpClient();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public DietInfo(userdiet DietinfoPassed)
	{
		InitializeComponent();
        DietPassed = DietinfoPassed;
        titlelbl.Text = DietPassed.diettitle;
        GetInfo();
           
    }

    private void ConfigureClient()
    {
        try
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
            Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
        }
        catch (Exception Ex)
        {
            //Empty
        }
    }

    async private void GetInfo()
    {
        try
        {

            Dietloading.IsVisible = true;
            MainStack.IsVisible = false;
            //Get Diet Info 
            ConfigureClient();
            //Chnage back to this 
            var id = DietPassed.dietid;
            var url = $"https://pwapi.peoplewith.com/api/diet/dietid/{id}";
            HttpResponseMessage responseconsent = await Client.GetAsync(url);

            if (responseconsent.IsSuccessStatusCode)
            {
                string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                var userResponseconsent = JsonConvert.DeserializeObject<APIDietResponse>(contentconsent);
                var consent = userResponseconsent.Value;

                Dietinformation = consent[0]; 

            }
            else
            {
                //Error
            }

            if (!String.IsNullOrEmpty(Dietinformation.dietid))
            {
                var Split = Dietinformation.dietinformation.Split('|');
                Bodylbl.Text = Split[0];
                infoSource.link = Split[1];
                infoSource.img = "link.png";
                infoSource.type = "Website";
                string extracted = ExtractDomain(Split[1]);
                infoSource.title = extracted;

                typelbl.Text = infoSource.type;
                titelbl.Text = infoSource.title;
            }

            MainStack.IsVisible = true;
            Dietloading.IsVisible = false; 
            //infolist.ItemsSource = infoSource;
        }
        catch( Exception Ex)
        {
            MainStack.IsVisible = true;
            Dietloading.IsVisible = false;
        }
    }

    static string ExtractDomain(string url)
    {
        try
        {
            Match match = Regex.Match(url, @"//([^/]+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
        catch (Exception ex)
        {
            return null; 
            //Dunno 
        }
    }

    //async private void infolist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    //{
    //    var item = e.DataItem as Information;
    
    //     await Browser.OpenAsync(item.link, BrowserLaunchMode.SystemPreferred);
    //}

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Browser.OpenAsync(infoSource.link, BrowserLaunchMode.SystemPreferred);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
      
    }
}