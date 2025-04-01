


namespace PeopleWith;


public partial class SymptomGallery : ContentPage
{

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
    public SymptomGallery()
	{
		InitializeComponent();
	}

    private void Gallery_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{
            //Push to Single View 

		}
		catch (Exception Ex)
		{
            NotasyncMethod(Ex);
        }
    }
}