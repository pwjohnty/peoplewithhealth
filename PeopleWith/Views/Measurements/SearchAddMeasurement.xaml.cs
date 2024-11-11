using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace PeopleWith;

public partial class SearchAddMeasurement : ContentPage
{
    APICalls aPICalls = new APICalls();
    public ObservableCollection<measurement> Measurements = new ObservableCollection<measurement>();
    public ObservableCollection<usermeasurement> UserMeasurements = new ObservableCollection<usermeasurement>();
    public ObservableCollection<measurement> SortedMeasurements = new ObservableCollection<measurement>();
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

    public SearchAddMeasurement()
	{
        try
        {
            InitializeComponent();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
	}

    public SearchAddMeasurement(ObservableCollection<usermeasurement> usermeasurementlist)
    {
        try
        {
            InitializeComponent();
            UserMeasurements = usermeasurementlist;
            getmeasurementlist();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getmeasurementlist()
    {

        try
        {
            //APICalls aPICalls = new APICalls();
            Measurements = await aPICalls.GetMeasurements();

            //getusermeasurements();

            //remove any user measurements in measurement list
            foreach (var item in Measurements)
            {
                if (UserMeasurements.Any(x => x.measurementid == item.measurementid))
                {
                    //do nothing
                }
                else
                {
                    SortedMeasurements.Add(item);
                }
            }

            var ordermeasurementlist = SortedMeasurements.OrderBy(x => x.measurementname).ToList();

            measurementlist.ItemsSource = ordermeasurementlist;
            measurementlist.HeightRequest = ordermeasurementlist.Count * 57;

            loadingstack.IsVisible = false;
            datastack.IsVisible = true;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    private async void measurementlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as measurement;
            await Navigation.PushAsync(new AddMeasurement(item, UserMeasurements, Measurements), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}