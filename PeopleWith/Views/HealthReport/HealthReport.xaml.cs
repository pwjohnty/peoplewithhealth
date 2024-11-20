using PeopleWith;

namespace PeopleWith;
public partial class HealthReport : ContentPage
{
    public const string HealthReportStart = "https://portal.peoplewith.com/migration/health-report.php?";
    public string HealthReportEnd = "id=" + Helpers.Settings.UserKey + "&pid=" + Helpers.Settings.ValidationCode;
    public string HealthReportUrl;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
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
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}