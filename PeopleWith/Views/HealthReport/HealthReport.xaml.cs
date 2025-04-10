using PeopleWith;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;

namespace PeopleWith;
public partial class HealthReport : ContentPage
{
    public const string HealthReportStart = "https://portal.peoplewith.com/migration/health-report.php?";
    public string HealthReportEnd = "id=" + Helpers.Settings.UserKey;
    public string HealthReportUrl;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public HealthReport()
	{
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                InitializeComponent();
                HealthReportUrl = HealthReportStart + HealthReportEnd;
                HealthReportViewer.Source = HealthReportUrl;

                if(DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    HealthReportViewer.HeightRequest = 650;
                }
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }         
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }   
    }

    private async void NovoConsentData()
    {
        try
        {
            if (!String.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                var signup = Helpers.Settings.SignUp;
                if (signup.Contains("SAX"))
                { //All Novo SignupCodes 
                    NovoConsent.IsVisible = true;
                    NovoContentlbl.Text = Preferences.Default.Get("NovoContent", String.Empty);
                    NovoExitidlbl.Text = Preferences.Default.Get("NovoExitid", String.Empty);

                    // Hide NovoConsent after 5 Seconds 
                    Task.Run(async () =>
                    {
                        await Task.Delay(5000);
                        NovoConsent.IsVisible = false;
                    });

                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
#if ANDROID
            PrintHelperAndroid.Print(HealthReportViewer);
#endif
#if IOS
           PrintHelperIOS.Print(HealthReportViewer);
#endif
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    private void HealthReportViewer_Navigated(object sender, WebNavigatedEventArgs e)
    {
        try
        {
            LoadingStack.IsVisible = false;
            HealthReportStack.IsVisible = true;
            NovoConsentData();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}