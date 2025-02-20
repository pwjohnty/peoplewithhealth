using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PeopleWith;

public partial class InvestInfo : ContentPage
{
    Information infoSource = new Information();
    userinvestigation InvestPassed = new userinvestigation();
    investigation investinformation = new investigation();

    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
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
    public InvestInfo(userinvestigation InvestInfopassed)
    {
		InitializeComponent();
        InvestPassed = InvestInfopassed;
        titlelbl.Text = InvestPassed.investigationname;
        GetInfo();
    }

    async private void GetInfo()
    {
        try
        {

            Investloading.IsVisible = true;
            MainStack.IsVisible = false;
            //Get Diet Info 
            HttpClient client = new HttpClient();

            var id = InvestPassed.investigationid;
            var url = $"https://pwdevapi.peoplewith.com/api/investigation/investigationid/{id}";
            HttpResponseMessage responseconsent = await client.GetAsync(url);

            if (responseconsent.IsSuccessStatusCode)
            {
                string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                var userResponseconsent = JsonConvert.DeserializeObject<APInvestigationResponse>(contentconsent);
                var consent = userResponseconsent.Value;

                investinformation = consent[0];

            }
            else
            {
                //Error
            }

            if (!String.IsNullOrEmpty(investinformation.investigationid))
            {
                var Split = investinformation.investigationinformation.Split('|');
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
            Investloading.IsVisible = false;
            //infolist.ItemsSource = infoSource;
        }
        catch (Exception Ex)
        {
            MainStack.IsVisible = true;
            Investloading.IsVisible = false;
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