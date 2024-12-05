//using Android.Telephony;
//using Java.Time.Temporal;
//using Com.Google.Android.Exoplayer2.Util;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
//using Vision;

namespace PeopleWith;

public partial class MeasurementsPage : ContentPage
{
	public ObservableCollection<measurement> Measurements = new ObservableCollection<measurement>();
    public ObservableCollection<usermeasurement> UserMeasurements = new ObservableCollection<usermeasurement>();
    public ObservableCollection<usermeasurement> newUserMeasurements = new ObservableCollection<usermeasurement>();
    public ObservableCollection<measurement> SortedMeasurements = new ObservableCollection<measurement>();
    APICalls aPICalls = new APICalls();
    bool hasusermeasurements;
    bool editusermeasurements;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    userfeedback userfeedbacklistpassed = new userfeedback();
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

    public MeasurementsPage()
	{
        try
        {
            InitializeComponent();

            //  getmeasurementlist();

            getusermeasurements();

            WeakReferenceMessenger.Default.Register<UpdateListMainPage>(this, (r, m) =>
            {

                newUserMeasurements = (ObservableCollection<usermeasurement>)m.Value;
                // Measurements = message.Value.SecondCollection;

                editusermeasurements = true;

                getusermeasurements();
            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    
        //// Register to receive the ValueChangedMessage with the custom wrapper type
        //WeakReferenceMessenger.Default.Register<ValueChangedMessage<CollectionsMessage<usermeasurement, measurement>>>(this, (recipient, message) =>
        //{
        //    // Handle the received message
        //    UserMeasurements = message.Value.FirstCollection;
        //   // Measurements = message.Value.SecondCollection;

        //    hasusermeasurements = true;

        //    getusermeasurements();
        //    // Do something with the received collections
        //});
    }

    public MeasurementsPage(userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();

            //  getmeasurementlist();

            userfeedbacklistpassed = userfeedbacklist;

            getusermeasurements();

            WeakReferenceMessenger.Default.Register<UpdateListMainPage>(this, (r, m) =>
            {

                newUserMeasurements = (ObservableCollection<usermeasurement>)m.Value;
                // Measurements = message.Value.SecondCollection;

                editusermeasurements = true;

                getusermeasurements();
            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

        //// Register to receive the ValueChangedMessage with the custom wrapper type
        //WeakReferenceMessenger.Default.Register<ValueChangedMessage<CollectionsMessage<usermeasurement, measurement>>>(this, (recipient, message) =>
        //{
        //    // Handle the received message
        //    UserMeasurements = message.Value.FirstCollection;
        //   // Measurements = message.Value.SecondCollection;

        //    hasusermeasurements = true;

        //    getusermeasurements();
        //    // Do something with the received collections
        //});
    }

    public MeasurementsPage(ObservableCollection<usermeasurement> updatedusermeasurements, ObservableCollection<measurement> updatedmeasurements)
    {
        try
        {
            InitializeComponent();

            //   getmeasurementlist();

            UserMeasurements = updatedusermeasurements;
            Measurements = updatedmeasurements;
            hasusermeasurements = true;

            getusermeasurements();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    async void getusermeasurements()
	{
		try 
		{

            if (hasusermeasurements)
            {
                //usermeasurements already updated
            
            }
            else
            {
                //Original 
                //UserMeasurements = await aPICalls.GetUserMeasurements();
                MeasLoading.IsVisible = true; 

                APICalls database = new APICalls();
                var getMeasurementsTask = database.GetUserMeasurements();

                //var delayTask = Task.Delay(1000);

                //if (await Task.WhenAny(getMeasurementsTask, delayTask) == delayTask)
                //{
                    //await MopupService.Instance.PushAsync(new GettingReady("Loading Measurements") { });
                //}

                UserMeasurements = await getMeasurementsTask;

                

            }


            if (editusermeasurements)
            {
                UserMeasurements.Clear();
                usermeasurementlist.ItemsSource = null;
                UserMeasurements = newUserMeasurements;
                var newlist = newUserMeasurements.OrderByDescending(x => DateTime.Parse(x.inputdatetime)).ToList();

                foreach (var item in newlist)
                {
                    var dateconverted = DateTime.Parse(item.inputdatetime);
                    if (dateconverted.Date == DateTime.Now.Date)
                    {
                        item.datechanged = "Today";
                    }
                    else if (dateconverted.Date == DateTime.Now.Date.AddDays(-1))
                    {
                        item.datechanged = "Yesterday";
                    }
                    else
                    {
                        item.datechanged = dateconverted.ToString("dd MMM");
                    }
                }


                usermeasurementlist.ItemsSource = newlist;
                MeasLoading.IsVisible = false;
                await MopupService.Instance.PopAllAsync(false);
                // usermeasurementlist.BackgroundColor = Colors.Red;
                return;
            }

            if (UserMeasurements.Count == 0)
            {
                nodatastack.IsVisible = true;
                datastack.IsVisible = false;
                MeasLoading.IsVisible = false;
                return;
            }

            nodatastack.IsVisible = false;
            datastack.IsVisible = true;


            foreach (var item in UserMeasurements)
			{
                var dateconverted = DateTime.Parse(item.inputdatetime);
                if (dateconverted.Date == DateTime.Now.Date)
                {
                    item.datechanged = "Today";
                }
                else if (dateconverted.Date == DateTime.Now.Date.AddDays(-1))
                {
                    item.datechanged = "Yesterday";
                }
                else
                {
                    item.datechanged = dateconverted.ToString("dd MMM");
                }
            }


            if (UserMeasurements.Count > 0)
            {
                recentlbl.IsVisible = true;
                usermeasurementlist.IsVisible = true;

                // Filter and sort the measurements to only include the most recent one of each type
                var filteredMeasurements = UserMeasurements
                    .GroupBy(m => m.measurementid) 
                    .Select(g => g.OrderByDescending(m => DateTime.Parse(m.inputdatetime)) 
                    .First()) 
                    .ToList();


                filteredMeasurements = filteredMeasurements.OrderByDescending(x => DateTime.Parse(x.inputdatetime)).ToList();

             
           
                usermeasurementlist.ItemsSource = filteredMeasurements;
                usermeasurementlist.HeightRequest = filteredMeasurements.Count * 110;

                

                ////remove any user measurements in measurement list
                //foreach(var item in Measurements)
                //{
                //    if(filteredMeasurements.Any(x => x.measurementid == item.measurementid))
                //    {
                //        //do nothing
                //    }
                //    else
                //    {
                //        SortedMeasurements.Add(item);
                //    }
                //}

                //var ordermeasurementlist = SortedMeasurements.OrderBy(x => x.measurementname).ToList();

                //measurementlist.ItemsSource = ordermeasurementlist;
                //measurementlist.HeightRequest = ordermeasurementlist.Count * 57;
            }
            MeasLoading.IsVisible = false; 
            //await MopupService.Instance.PopAllAsync(false);
        }
		catch(Exception Ex)
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

            getusermeasurements();

           // measurementlist.ItemsSource = Measurements;
           // measurementlist.HeightRequest = Measurements.Count * 57;

        }
		catch(Exception Ex)
		{
            NotasyncMethod(Ex);
        }

	}

    private async void measurementlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{
			var item = e.DataItem as measurement;

			await Navigation.PushAsync(new SingleMeasurement(item, UserMeasurements, SortedMeasurements, userfeedbacklistpassed), false);

		}
		catch(Exception Ex)
		{
            NotasyncMethod(Ex);
        }
    }

    private async void usermeasurementlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as usermeasurement;

            await Navigation.PushAsync(new SingleMeasurement(item, UserMeasurements, SortedMeasurements, userfeedbacklistpassed), false);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                await Navigation.PushAsync(new SearchAddMeasurement(UserMeasurements, userfeedbacklistpassed), false);
                AddBtn.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //no data , add stack clicked
        try
        {
            await Navigation.PushAsync(new SearchAddMeasurement(UserMeasurements, userfeedbacklistpassed), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void MeasureInfoTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("measure") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }
}