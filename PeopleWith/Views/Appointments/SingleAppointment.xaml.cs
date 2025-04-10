using System.Collections.ObjectModel;
using Mopups.Services;
using Microsoft.Maui.Networking;

namespace PeopleWith;

public partial class SingleAppointment : ContentPage
{
    appointment SelectedAppointment = new appointment();
    ObservableCollection<appointment> AllAppointmentsPassed = new ObservableCollection<appointment>();
    public ObservableCollection<hcp> AllHCPs = new ObservableCollection<hcp>();
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
    public SingleAppointment(appointment AppointmentSelected, ObservableCollection<appointment> AllAppointmentData)
    {
        InitializeComponent();
        SelectedAppointment = AppointmentSelected;
        AllAppointmentsPassed = AllAppointmentData;
        if (SelectedAppointment.location == "Private Consultation")
        {
            lbllocation.Text = "Consultation";
        }
        else
        {
            lbllocation.Text = SelectedAppointment.location;
        }
        lbltype.Text = SelectedAppointment.type;
        AppointmentWithlbl.Text = SelectedAppointment.AppointWith;
        var GetStart = DateTime.Parse(SelectedAppointment.datetime);
        lblStart.Text = GetStart.ToString("HH:mm");
        


        //Appointment Details 
        var DateTimes = DateTime.Parse(SelectedAppointment.datetime);
        var StartDate = DateTimes.ToString("dd/MM/yy HH:mm");
        if (string.IsNullOrEmpty(SelectedAppointment.expectedduration))
        {
            var EndDate = DateTimes.AddHours(1);
            var EndDateString = EndDate.ToString("HH:mm");
            DateTimelbl.Text = StartDate;
            lblEnd.Text = "Unknown";
        }
        else
        {
            var split = SelectedAppointment.expectedduration.Split(' ');
            var GetTimeSpan = new TimeSpan(Int32.Parse(split[0]), Int32.Parse(split[2]), 0);
            var EndDate = DateTimes + GetTimeSpan;
            var EndDateString = EndDate.ToString("HH:mm");
            DateTimelbl.Text = StartDate + " - " + EndDateString;
            lblEnd.Text = SelectedAppointment.ExpectedEnd;
        }

        locationlbl.Text = SelectedAppointment.location;

        typelbl.Text = SelectedAppointment.type;

        if (string.IsNullOrEmpty(SelectedAppointment.reminderinterval))
        {
            reminderlbl.Text = "No Reminder Added";
        }
        else
        {
            reminderlbl.Text = SelectedAppointment.reminderinterval;
        }

        if (string.IsNullOrEmpty(SelectedAppointment.reason))
        {
            noteslbl.Text = "No Reason Added";
        }
        else
        {
            noteslbl.Text = SelectedAppointment.reason;
        }

        //Check if Appointment over (Add Feedback if True) 
        if (DateTimes < DateTime.Now)
        {
            if (SelectedAppointment.feedback == null || SelectedAppointment.feedback.Count == 0)
            {
                if (SelectedAppointment.attended == "No")
                {
                    FeedbackNotRecorded.IsVisible = true;
                    FeedNoTitle.IsVisible = true;
                    FeedBackAdded.IsVisible = true;
                }
                else
                {
                    AddFeedStack.IsVisible = true;
                }

            }
            else
            {
                //Show Feedback 
                AddFeedStack.IsVisible = false;
                FeedBackAdded.IsVisible = true;
                //Attendedlbl.Text = SelectedAppointment.attended; 
                if (SelectedAppointment.attended == "No")
                {
                    FeedbackNotRecorded.IsVisible = true;
                }
                else
                {
                    FeedbackRecorded.IsVisible = true;
                    ActualDurationlbl.Text = SelectedAppointment.feedback[0].ActualDuration;
                    if (string.IsNullOrEmpty(SelectedAppointment.feedback[0].DoctorsNotes))
                    {
                        DoctorsNotesbl.Text = "No Doctors Notes Recorded";
                    }
                    else
                    {
                        DoctorsNotesbl.Text = SelectedAppointment.feedback[0].DoctorsNotes;
                    }
                    if (string.IsNullOrEmpty(SelectedAppointment.feedback[0].AdditionalNotes))
                    {
                        AddNoteslbl.Text = "No Additional Notes Recorded";
                    }
                    else
                    {
                        AddNoteslbl.Text = SelectedAppointment.feedback[0].AdditionalNotes;
                    }
                }
            }
        }
    }

    private async void EditAppoint_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Edit Feedback
                EditFeed.IsEnabled = false;
                await Navigation.PushAsync(new AddAppointment(SelectedAppointment, AllAppointmentsPassed), false);

                EditFeed.IsEnabled = true;

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

    //private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    //{

    //}

    private async void AddFeed_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddFeed.IsEnabled = false;
                await Navigation.PushAsync(new AppointmentFeedback(SelectedAppointment, AllAppointmentsPassed), false);
                AddFeed.IsEnabled = true;
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

    private async void EditFeed_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Edit Feedback
                EditFeed.IsEnabled = false;
                await Navigation.PushAsync(new AppointmentFeedback(SelectedAppointment, AllAppointmentsPassed), false); 

                EditFeed.IsEnabled = true;

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

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Delete Appointment
                DeleteBtn.IsEnabled = false;

                APICalls database = new APICalls();
                SelectedAppointment.deleted = true;
                await database.DeleteUserAppointment(SelectedAppointment);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Deleted") { });
                await Task.Delay(1500);
                await MopupService.Instance.PopAllAsync(false);
                await Navigation.PushAsync(new AllAppointments());
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllAppointments);
                if (pageToRemoves != null)
                {

                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);


                await MopupService.Instance.PopAllAsync(false);
                DeleteBtn.IsEnabled = true;

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
}