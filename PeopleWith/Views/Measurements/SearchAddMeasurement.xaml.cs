using CommunityToolkit.Maui.Core.Extensions;
using Maui.FreakyControls.Extensions;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace PeopleWith;

public partial class SearchAddMeasurement : ContentPage
{
    APICalls aPICalls = new APICalls();
    public ObservableCollection<measurement> Measurements = new ObservableCollection<measurement>();
    public ObservableCollection<usermeasurement> UserMeasurements = new ObservableCollection<usermeasurement>();
    public ObservableCollection<measurement> SortedMeasurements = new ObservableCollection<measurement>();
    public ObservableCollection<measurement> FilterResults = new ObservableCollection<measurement>();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    userfeedback userfeedbacklistpassed = new userfeedback();
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

    public SearchAddMeasurement(ObservableCollection<usermeasurement> usermeasurementlist, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            UserMeasurements = usermeasurementlist;

            userfeedbacklistpassed = userfeedbacklist;
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
            Measloading.IsVisible = true; 
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

            var ordermeasurementlist = new ObservableCollection<measurement>(SortedMeasurements.OrderBy(x => x.measurementname).ToList());

            measurementlist.ItemsSource = ordermeasurementlist;
            //measurementlist.HeightRequest = ordermeasurementlist.Count * 57;

            FilterResults = ordermeasurementlist;

            Results.Text = "Results" + " (" + ordermeasurementlist.Count + ")";

            loadingstack.IsVisible = false;
            datastack.IsVisible = true;
            Measloading.IsVisible = false;

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
            await Navigation.PushAsync(new AddMeasurement(item, UserMeasurements, Measurements, userfeedbacklistpassed), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = searchbar.Text.ToString();
            var filteredMeasures = new ObservableCollection<measurement>(FilterResults.Where(s => s.measurementname.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.measurementname);
            measurementlist.ItemsSource = filteredMeasures;
            var count = filteredMeasures.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
            if(count == "0")
            {
                NoResultslbl.IsVisible = true;
                measurementlist.IsVisible = false; 
            }
            else
            {
                measurementlist.IsVisible = true;
                NoResultslbl.IsVisible = false; 
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}