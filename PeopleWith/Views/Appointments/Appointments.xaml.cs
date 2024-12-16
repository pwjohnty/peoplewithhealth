using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using Syncfusion.Maui.DataSource.Extensions;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class Appointments : ContentPage
{
    public ObservableCollection<hcp> AllHCPs = new ObservableCollection<hcp>();
    public ObservableCollection<appointment> AllAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<appointmentcalendar> CalendarData = new ObservableCollection<appointmentcalendar>();
    public appointment AppointmenttoPass = new appointment();
    public appointment UpdatedAppointment = new appointment();
    public ObservableCollection<appointment> itemstoremove = new ObservableCollection<appointment>();
    APICalls database = new APICalls();
    bool AddedAppointment;
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
    public Appointments()
    {
        try
        {
            InitializeComponent();
            GetAllHCPData();

            WeakReferenceMessenger.Default.Register<UpdateAppFeedback>(this, (recipient, message) =>
            {

                AllAppointments = (ObservableCollection<appointment>)message.AllAppointments;

                foreach(var item in AllAppointments)
                {
                    if(item.id == message.AppointmentId)
                    {
                        AppointmenttoPass = item;
                        UpdatedAppointment = item; 
                    }
                }

                //Update FeedBack Page 
                Feedbacktitle.IsVisible = false;
                Feedbackbtn.IsVisible = false;
                //Attendedlbl.Text = SelectedAppointment.attended; 
                if (UpdatedAppointment.attended == "No")
                {
                    FeedbackNotRecorded.IsVisible = true;
                }
                else
                {
                    FeedbackRecorded.IsVisible = true;
                    ActualDurationlbl.Text = UpdatedAppointment.feedback[0].ActualDuration;
                    if (string.IsNullOrEmpty(UpdatedAppointment.feedback[0].DoctorsNotes))
                    {
                        DoctorsNotesbl.Text = "No Doctors Notes Recorded";
                    }
                    else
                    {
                        DoctorsNotesbl.Text = UpdatedAppointment.feedback[0].DoctorsNotes;
                    }
                    if (string.IsNullOrEmpty(UpdatedAppointment.feedback[0].AdditionalNotes))
                    {
                        AddNoteslbl.Text = "No Additional Notes Recorded";
                    }
                    else
                    {
                        AddNoteslbl.Text = UpdatedAppointment.feedback[0].AdditionalNotes; 
                    }

                }


            });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public Appointments(ObservableCollection<appointment> GetAllAppointments, ObservableCollection<hcp> GetAllHCPS)
    {
        try
        {
            InitializeComponent();
            AddedAppointment = true;
            AllAppointments = GetAllAppointments;
            AllHCPs = GetAllHCPS;
            GetAllHCPData();

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
      
    }
    public async void GetAllHCPData()
    {
        try
        {
            if (!AddedAppointment)
            {
                AllHCPs = await database.GetUserHCP();
            foreach (var item in AllHCPs)
            {
                item.fullname = item.firstname + " " + item.surname;
            }

                AllAppointments = await database.GetUserAppointment();
            }

            PopulateCalendar();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async public void PopulateCalendar()
    {
        try
        {
            CalendarData.Clear(); 

            foreach(var item in AllAppointments)
            {
                if(item.deleted == true)
                {
                    itemstoremove.Add(item); 
                }
            }

            foreach(var item in itemstoremove)
            {
                AllAppointments.Remove(item);
            }

            foreach (var item in AllAppointments)
            {
                var StartDateTime = DateTime.Parse(item.datetime);
                string Eventtitle;
                DateTime EndDateTime; 

                if (string.IsNullOrEmpty(item.expectedduration))
                {
                    EndDateTime = StartDateTime.AddHours(1); 
                }
                else
                {
                    var split = item.expectedduration.Split(' ');
                    var GetTimeSpan = new TimeSpan(Int32.Parse(split[0]), Int32.Parse(split[2]), 0);
                    EndDateTime = StartDateTime + GetTimeSpan; 
                }

                if(item.hcpname == "No HCP")
                {
                    Eventtitle = "Appointment"; 
                }
                else
                {
                    Eventtitle = "Appointment with " + item.hcpname;
                }

                var appoint = new appointmentcalendar
                {
                    Appointid = item.id,
                    From = StartDateTime,
                    To = EndDateTime,
                    EventName = Eventtitle,
                    Background = new SolidColorBrush(Color.FromArgb("#ffcccb")),
                    TextColor = Colors.White
                };

                CalendarData.Add(appoint); 
            }
            // Define AppointmentMapping
            Calendar.AppointmentMapping = new Syncfusion.Maui.Scheduler.SchedulerAppointmentMapping
            {
                StartTime = nameof(appointmentcalendar.From),
                EndTime = nameof(appointmentcalendar.To),
                Subject = nameof(appointmentcalendar.EventName),
                Background = nameof(appointmentcalendar.Background),
                TextColorMapping = nameof(appointmentcalendar.TextColor),
                IsAllDay = nameof(appointmentcalendar.IsAllDay)
            };

            // Set the AppointmentsSource
            Calendar.AppointmentsSource = CalendarData;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

   

    async private void Calendar_Tapped(object sender, Syncfusion.Maui.Scheduler.SchedulerTappedEventArgs e)
    {
        try
        {
            
            if (Calendar.View == Syncfusion.Maui.Scheduler.SchedulerView.Day)
            {
                AddAppoint.IsEnabled = false;
                var TappedAppointments = e.Appointments;

                if (TappedAppointments != null && TappedAppointments.Count > 0)
                {
                    var tappedAppointment = TappedAppointments[0] as appointmentcalendar;

                    if (tappedAppointment != null)
                    {
                        var appointmentId = tappedAppointment.Appointid;

                        AppointmentCalendar.IsVisible = false;
                        AppointmentDetails.IsVisible = true;

                        //Populate Appointment Details Page
                        var SelectedAppointment = AllAppointments.FirstOrDefault(s => s.id == appointmentId);
                        AppointmenttoPass = SelectedAppointment; 
                        Titlelbl.Text = "Appointment with " + SelectedAppointment.hcpname;
                        var DateTimes = DateTime.Parse(SelectedAppointment.datetime);
                        var StartDate = DateTimes.ToString("dd.MMM.yy . HH:mm");
                        if (string.IsNullOrEmpty(SelectedAppointment.expectedduration))
                        {
                            var EndDate = DateTimes.AddHours(1);
                            var EndDateString = EndDate.ToString("HH:mm");
                            DateTimelbl.Text = StartDate + " - " + EndDateString;
                        }
                        else
                        {
                            var split = SelectedAppointment.expectedduration.Split(' ');
                            var GetTimeSpan = new TimeSpan(Int32.Parse(split[0]), Int32.Parse(split[2]), 0);
                            var EndDate = DateTimes + GetTimeSpan;
                            var EndDateString = EndDate.ToString("HH:mm");
                            DateTimelbl.Text = StartDate + " - " + EndDateString;
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
                                if(SelectedAppointment.attended == "No")
                                {
                                    FeedbackNotRecorded.IsVisible = true;
                                }
                                else
                                {
                                    Feedbacktitle.IsVisible = true;
                                    Feedbackbtn.IsVisible = true;
                                }

                            }
                            else
                            {
                                //Show Feedback 
                                Feedbacktitle.IsVisible = false;
                                Feedbackbtn.IsVisible = false;
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
                                        AddNoteslbl.Text = SelectedAppointment.feedback[0].AdditionalNotes; ;
                                    }
                                   
                                }

                            }
                              
                        }

                    }
                }
                else
                {

                    //AppointmentCalendar.IsVisible = false;
                    //AppointmentDetails.IsVisible = true;

                }
            }
            else
            {
                NavigationPage.SetHasNavigationBar(this, false);
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    AndroidBtn.IsVisible = true;
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    IOSBtn.IsVisible = true;
                }
                AddAppoint.IsEnabled = true;
                var tappedDate = e.Date;
                if (tappedDate != null)
                {
                    if (tappedDate == DateTime.Now.Date)
                    {
                        tappedDate = tappedDate + DateTime.Now.TimeOfDay;
                    }
                    Calendar.SelectedDate = tappedDate;
                    Calendar.View = Syncfusion.Maui.Scheduler.SchedulerView.Day;
                    Calendar.SelectedDate = null;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddAppoint.IsEnabled = false;
                await Navigation.PushAsync(new AddAppointment(AllHCPs, AllAppointments), false);
                AddAppoint.IsEnabled = true;
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

    async private void Feedbackbtn_Clicked(object sender, EventArgs e)
    {
        try 
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                Feedbackbtn.IsEnabled = false;
                await Navigation.PushAsync(new AppointmentFeedback(AppointmenttoPass, AllAppointments), false);
                Feedbackbtn.IsEnabled = true;
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

    async private void BackButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (AppointmentDetails.IsVisible == true)
            {
                AppointmentDetails.IsVisible = false;
                AppointmentCalendar.IsVisible = true;
                AddAppoint.IsEnabled = true;
                FeedbackRecorded.IsVisible = false;
                FeedbackNotRecorded.IsVisible = false;
                Feedbacktitle.IsVisible = false;
                Feedbackbtn.IsVisible = false;
            }
            else if (Calendar.View == Syncfusion.Maui.Scheduler.SchedulerView.Day)
            {
                Calendar.View = Syncfusion.Maui.Scheduler.SchedulerView.Month;
                AddAppoint.IsEnabled = true;
                NavigationPage.SetHasNavigationBar(this, true);
                AndroidBtn.IsVisible = false;
                IOSBtn.IsVisible = false;
            }
            else
            {
                AddAppoint.IsEnabled = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Delete Appointment
                Deletebtn.IsEnabled = false;

            APICalls database = new APICalls();
            AppointmenttoPass.deleted = true;
            await database.DeleteUserAppointment(AppointmenttoPass);

            await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Deleted") { });
            await Task.Delay(1500);
            AppointmentCalendar.IsVisible = true;
            AppointmentDetails.IsVisible = false;
            Calendar.View = Syncfusion.Maui.Scheduler.SchedulerView.Month;
            

            AllAppointments.Remove(AppointmenttoPass);

            PopulateCalendar();

            await MopupService.Instance.PopAllAsync(false);

                AddAppoint.IsEnabled = true;
                Deletebtn.IsEnabled = true;

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