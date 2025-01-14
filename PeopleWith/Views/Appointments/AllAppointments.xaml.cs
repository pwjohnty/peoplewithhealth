using Mopups.Services;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllAppointments : ContentPage
{
    public ObservableCollection<hcp> AllHCPs = new ObservableCollection<hcp>(); 
    public ObservableCollection<appointment> AppointmentsData = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> UpcomingAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> HistorialAppointments = new ObservableCollection<appointment>();
    APICalls database = new APICalls();
    private bool IsLoading = true;
    //Connectivity Changed 
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
    public AllAppointments()
	{
		InitializeComponent();
        GetAllAppointmentData();
    }

    public async void GetAllAppointmentData()
    {
        try
        {
            //HCP API Call
            SegmentDetails.IsVisible = false;
            AppointLoading.IsVisible = true;
            AllHCPs = await database.GetUserHCP();
            foreach (var item in AllHCPs)
            {
                item.fullname = item.firstname + " " + item.surname;
            }

            //Appointment API Call
            AppointmentsData = await database.GetUserAppointment();

            PopulateData();

            AppointLoading.IsVisible = false;
            SegmentDetails.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public async void PopulateData()
    {
        try
        {
            foreach(var item in AppointmentsData)
            {
                if(item.hcpname == "No HCP")
                {
                    item.AppointWith = "Scheduled Appointment"; 
                }
                else
                {
                    item.AppointWith = "Appointment With " + item.hcpname; 
                }

                item.locationtype = item.location + " - " + item.type;

                var GetDateTime = DateTime.Parse(item.datetime);

                string Day; 
                if(GetDateTime.Date == DateTime.Now.Date)
                {
                    Day = "Today"; 
                }
                else if((GetDateTime.Date == DateTime.Now.Date.AddDays(1)))
                {
                    Day = "Tomorrow"; 
                }
                else if (GetDateTime.Date > DateTime.Now.Date && GetDateTime.Date <= DateTime.Now.Date.AddDays(7))
                {
                    Day = GetDateTime.ToString("dddd"); 
                }
                else
                {
                    Day = GetDateTime.ToString("dd/MM/yy"); 
                }
                string Duration;

                if (string.IsNullOrEmpty(item.expectedduration))
                {
                    Duration = GetDateTime.ToString("HH:mm");
                }
                else
                {
                    var GetDuration = item.expectedduration;
                    var cleanInput = GetDuration.Replace(" Hours", "").Replace(" Minutes", "").Trim();
                    var GetSplit = cleanInput.Split(' '); 
                    string GetTime = GetSplit[0] + ":" + GetSplit[1];
                    TimeSpan ExpectedTs = TimeSpan.Parse(GetTime);
                    DateTime EndDate = GetDateTime + ExpectedTs; 
                    var Start = GetDateTime.ToString("HH:mm");
                    var End = EndDate.ToString("HH:mm");
                    item.ExpectedEnd = End;
                    Duration = Start + " - " + End;

                }

                item.Appointmentlength = Day + " " + Duration;

                if (string.IsNullOrEmpty(item.reminderinterval))
                {
                    item.reminderinterval = "No Reminder";
                }

                if (string.IsNullOrEmpty(item.reason))
                {
                    item.HasReason = false; 
                }
                else
                {
                    item.HasReason = true; 
                }

                if(DateTime.Now <= GetDateTime)
                {
                    //Add To Upcoming 
                    UpcomingAppointments.Add(item);

                }
                else
                {
                    //Add To Historical 
                    HistorialAppointments.Add(item);
                }
            }

            var sortedList = UpcomingAppointments.OrderByDescending(x => DateTime.Parse(x.datetime)).ToObservableCollection();
            var sortedListtwo = HistorialAppointments.OrderByDescending(x => DateTime.Parse(x.datetime)).ToObservableCollection();

            UpcomingAppointments = sortedList;
            HistorialAppointments = sortedListtwo; 

            if(sortedList.Count == 0 && sortedListtwo.Count == 0)
            {
                nodatastack.IsVisible = true;
                datastack.IsVisible = false; 
            }
            else if (sortedList.Count == 0)
            {
                UpcomingList.IsVisible = true;
                noActiveAppointlbl.IsVisible = true;
                SegmentDetails.Text = "Appointments you have currently scheduled";
            }
            else
            {

                noActiveAppointlbl.IsVisible = false;
                nodatastack.IsVisible = false;
                datastack.IsVisible = true;
                //Set Segmentlbl
                SegmentDetails.Text = "Appointments you have currently scheduled";

                UpcomingList.IsVisible = true;
            }

            UpcomingList.ItemsSource = UpcomingAppointments;
            HistoricalList.ItemsSource = HistorialAppointments;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    //Toolbar item (Add New) Clicked 
    private async void AddBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                await Navigation.PushAsync(new AddAppointment(AppointmentsData), false);
                AddBtn.IsEnabled = true;
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

    //Info Icon Clicked 
    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("appoint") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    
    private async void AllAppointments_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var SelectedAppointment = new appointment(); 
            SelectedAppointment = e.DataItem as appointment;
            await Navigation.PushAsync(new SingleAppointment(SelectedAppointment, AppointmentsData), false); 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HistoricalList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var SelectedAppointment = new appointment();
            SelectedAppointment = e.DataItem as appointment;
            await Navigation.PushAsync(new SingleAppointment(SelectedAppointment, AppointmentsData), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {
            var index = e.NewIndex;

            if (index == 0)
            {
                if (IsLoading == true)
                {
                    UpcomingList.IsVisible = true;
                    HistoricalList.IsVisible = false;
                    IsLoading = false;
                }
                else
                {
                    SegmentDetails.Text = "Appointments you have currently scheduled";
                    //Active Appointments
                    UpcomingList.IsVisible = false; 
                    noActiveAppointlbl.IsVisible = false;
                    //Inactive Appointments
                    HistoricalList.IsVisible = false;
                    noHistoryAppointlbl.IsVisible = false;

                    if (UpcomingAppointments.Count == 0)
                    {
                        UpcomingList.IsVisible = false;
                        noActiveAppointlbl.IsVisible = true;
                    }
                    else
                    {
                        
                        UpcomingList.IsVisible = true;
                        noActiveAppointlbl.IsVisible = false;
                    }
                }
            }
            else if (index == 1)
            {
                SegmentDetails.Text = "Appointments you had previously scheduled";
                //Active Appointments
                UpcomingList.IsVisible = false;
                noActiveAppointlbl.IsVisible = false;
                //Inactive Appointments
                HistoricalList.IsVisible = false;
                noHistoryAppointlbl.IsVisible = false;

                if (HistorialAppointments.Count == 0)
                {
                    HistoricalList.IsVisible = false;
                    noHistoryAppointlbl.IsVisible = true;
                }
                else
                {
                    
                    HistoricalList.IsVisible = true;
                    noHistoryAppointlbl.IsVisible = false;

                }
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }
}