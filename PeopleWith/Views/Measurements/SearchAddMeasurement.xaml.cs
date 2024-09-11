using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace PeopleWith;

public partial class SearchAddMeasurement : ContentPage
{
    APICalls aPICalls = new APICalls();
    public ObservableCollection<measurement> Measurements = new ObservableCollection<measurement>();
    public ObservableCollection<usermeasurement> UserMeasurements = new ObservableCollection<usermeasurement>();
    public ObservableCollection<measurement> SortedMeasurements = new ObservableCollection<measurement>();
    public SearchAddMeasurement()
	{
		InitializeComponent();
	}

    public SearchAddMeasurement(ObservableCollection<usermeasurement> usermeasurementlist)
    {
        InitializeComponent();

        UserMeasurements = usermeasurementlist;

        getmeasurementlist();

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
        catch (Exception ex)
        {

        }

    }

    private async void measurementlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as measurement;

            await Navigation.PushAsync(new AddMeasurement(item, UserMeasurements, Measurements), false);
        }
        catch (Exception ex)
        {

        }
    }
}