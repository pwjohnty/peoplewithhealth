using PeopleWith;

namespace PeopleWith;



public partial class HealthReport : ContentPage
{
    public const string HealthReportStart = "https://portal.peoplewith.com/migration/health-report.php?";
    public string HealthReportEnd = "id=" + Helpers.Settings.UserKey + "&pid=" + Helpers.Settings.ValidationCode;
    public string HealthReportUrl;
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
            InitializeComponent();
            HealthReportUrl = HealthReportStart + HealthReportEnd;
            //HealthReportViewer.Source = test;
            //string test = "https://portal.peoplewith.com/migration/health-report.php?id=0cd8854d-5073-4725-989b-c190e5a8cb15&pid=12312";
            HealthReportViewer.Source = HealthReportUrl;


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

#if ANDROID
            PrintHelperAndroid.Print(HealthReportViewer);
#endif
#if IOS
           PrintHelperIOS.Print(HealthReportViewer);
#endif
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