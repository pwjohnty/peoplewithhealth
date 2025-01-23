using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AppointmentFeedback : ContentPage
{
    public appointment AppointmentPassed = new appointment();
    public ObservableCollection<appointment> AllAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> Appointtoremove = new ObservableCollection<appointment>();
    public ObservableCollection<string> ChipItems { get; set; }
    public string Attended;
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

    protected override bool OnBackButtonPressed()
    {
        try
        {
            if (AddFeedbackStack.IsVisible == true)
            {
                //FeedBack Not yet Addded
                return false;
            }
            else
            {
                //Feedback Addeded
                
                return false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return false;
        }

    }

    public AppointmentFeedback(appointment PassedAppoint, ObservableCollection<appointment> GetAllAppointments)
	{
        try
        {
            InitializeComponent();
            AppointmentPassed = PassedAppoint;
            AllAppointments = GetAllAppointments;
            PopulatePage();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
	}

    public AppointmentFeedback(appointment PassedAppoint)
    {
        try
        {
            InitializeComponent();
            AppointmentPassed = PassedAppoint;
            PopulatePage();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public async void PopulatePage()
    {
        try
        {

            ChipItems = new ObservableCollection<string>
                {
                   "Days",
                   "Hours",
                   "Minutes",
                };

            TimeChips.ItemsSource = ChipItems;

            if (AppointmentPassed.feedback == null || string.IsNullOrEmpty(AppointmentPassed.attended))
            {
               //Add New Feedback
            }
            else
            {

                //Update Feedback
                FeedbackAdd.Text = "Update Feedback";

                //Pre-Select Attended
                if (AppointmentPassed.attended == "No")
                {

                    btnno.BorderColor = Color.FromRgba("#ffe4e1");
                    btnno.BackgroundColor = Color.FromRgba("#ffe4e1");
                    btnno.TextColor = Color.FromRgba("#d96783");
                    btnyes.BorderColor = Colors.LightGray;
                    btnyes.BackgroundColor = Colors.Transparent;
                    btnyes.TextColor = Colors.Gray;
                    Attended = "No";

                    ActualDurationtitle.IsVisible = false;
                    ActualDurationEntry.IsVisible = false;
                    DoctorNoteslbl.IsVisible = false;
                    DoctorEntryStack.IsVisible = false;
                    AddNoteslbl.IsVisible = false;
                    AddNotesStack.IsVisible = false;
                    DoctorNotesinfo.IsVisible = false;
                    AddNotesinfo.IsVisible = false;

                }
                else
                {
                    btnyes.BorderColor = Color.FromRgba("#ffe4e1");
                    btnyes.BackgroundColor = Color.FromRgba("#ffe4e1");
                    btnyes.TextColor = Color.FromRgba("#d96783");
                    btnno.BorderColor = Colors.LightGray;
                    btnno.BackgroundColor = Colors.Transparent;
                    btnno.TextColor = Colors.Gray;
                    Attended = "Yes";

                    ActualDurationtitle.IsVisible = true;
                    ActualDurationEntry.IsVisible = true;
                    DoctorNoteslbl.IsVisible = true;
                    DoctorEntryStack.IsVisible = true;
                    AddNoteslbl.IsVisible = true;
                    AddNotesStack.IsVisible = true;
                    DoctorNotesinfo.IsVisible = true;
                    AddNotesinfo.IsVisible = true;

                    //Pre-Select Actual Duration
                    var SplitDuration = AppointmentPassed.feedback[0].ActualDuration;
                    var Split = SplitDuration.Split(' ');
                    var GetLengthTime = Split[1];
                    TimeChips.SelectedItem = GetLengthTime;
                    DurationEntry.Text = Split[0];
                    Durationlbl.Text = GetLengthTime;

                    //Pre-Populate Doctors Notes 

                    if (string.IsNullOrEmpty(AppointmentPassed.feedback[0].DoctorsNotes))
                    {
                        //Do Nothing
                    }
                    else
                    {
                        DoctorNotes.Text = AppointmentPassed.feedback[0].DoctorsNotes;
                    }

                    //Pre-Popilate Add Notes 
                    if (string.IsNullOrEmpty(AppointmentPassed.feedback[0].AdditionalNotes))
                    {
                        //Do Nothing
                    }
                    else
                    {
                        AddNotes.Text = AppointmentPassed.feedback[0].AdditionalNotes;
                    }

                }

             

                //FinalFeedbackStack.IsVisible = true;

                //Titlelbl.Text = "Appointment with " + AppointmentPassed.hcpname;
                //var DateTimes = DateTime.Parse(AppointmentPassed.datetime);
                //var StartDate = DateTimes.ToString("dd.MMM.yy . HH:mm");
                //if (string.IsNullOrEmpty(AppointmentPassed.expectedduration))
                //{
                //    var EndDate = DateTimes.AddHours(1);
                //    var EndDateString = EndDate.ToString("HH:mm");
                //    DateTimelbl.Text = StartDate + " - " + EndDateString;
                //}
                //else
                //{
                //    var split = AppointmentPassed.expectedduration.Split(' ');
                //    var GetTimeSpan = new TimeSpan(Int32.Parse(split[0]), Int32.Parse(split[2]), 0);
                //    var EndDate = DateTimes + GetTimeSpan;
                //    var EndDateString = EndDate.ToString("HH:mm");
                //    DateTimelbl.Text = StartDate + " - " + EndDateString;
                //}

                //locationlbl.Text = AppointmentPassed.location;

                //typelbl.Text = AppointmentPassed.type;

                //if (string.IsNullOrEmpty(AppointmentPassed.reminderinterval))
                //{
                //    reminderlbl.Text = "No Reminder Added";
                //}
                //else
                //{
                //    reminderlbl.Text = AppointmentPassed.reminderinterval;
                //}

                //if (string.IsNullOrEmpty(AppointmentPassed.reason))
                //{
                //    noteslbl.Text = "No Reason Added";
                //}
                //else
                //{
                //    noteslbl.Text = AppointmentPassed.reason;
                //}

                //Attendancelbl.Text = AppointmentPassed.attended;

                //if(AppointmentPassed.attended == "Yes")
                //{
                //    ActualDurationStack.IsVisible = true;
                //    DoctorNotesStack.IsVisible = true;
                //    AdditionalNotesStack.IsVisible = true;

                //    ActualDurationlbl.Text = AppointmentPassed.feedback[0].ActualDuration;
                //    if (String.IsNullOrEmpty(AppointmentPassed.feedback[0].DoctorsNotes))
                //    {
                //        DoctorsNotesbl.Text = "No Doctors Notes Added";
                //    }
                //    else
                //    {
                //        DoctorsNotesbl.Text = AppointmentPassed.feedback[0].DoctorsNotes;
                //    }
                //    if (String.IsNullOrEmpty(AppointmentPassed.feedback[0].AdditionalNotes))
                //    {
                //        AdditionalNoteslbl.Text = "No Additional Notes Added";
                //    }
                //    else
                //    {
                //        AdditionalNoteslbl.Text = AppointmentPassed.feedback[0].AdditionalNotes;
                //    }

                //}
                //All Data
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    private void btnyes_Clicked(object sender, EventArgs e)
    {
        try
        {
            btnyes.BorderColor = Color.FromRgba("#ffe4e1"); 
            btnyes.BackgroundColor = Color.FromRgba("#ffe4e1");
            btnyes.TextColor = Color.FromRgba("#d96783");
            btnno.BorderColor = Colors.LightGray;
            btnno.BackgroundColor = Colors.Transparent;
            btnno.TextColor = Colors.Gray; 
            Attended = "Yes";

            //Show All Feedback 
            ActualDurationtitle.IsVisible = true;
            ActualDurationEntry.IsVisible = true;
            DoctorNoteslbl.IsVisible = true;
            DoctorEntryStack.IsVisible = true;
            AddNoteslbl.IsVisible = true;
            AddNotesStack.IsVisible = true;
            DoctorNotesinfo.IsVisible = true;
            AddNotesinfo.IsVisible = true;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void btnno_Clicked(object sender, EventArgs e)
    {
        try
        {
            btnno.BorderColor = Color.FromRgba("#ffe4e1");
            btnno.BackgroundColor = Color.FromRgba("#ffe4e1");
            btnno.TextColor = Color.FromRgba("#d96783");
            btnyes.BorderColor = Colors.LightGray;
            btnyes.BackgroundColor = Colors.Transparent;
            btnyes.TextColor = Colors.Gray;
            Attended = "No";

            //hide All Feedback 
            ActualDurationtitle.IsVisible = false;
            ActualDurationEntry.IsVisible = false;
            DoctorNoteslbl.IsVisible = false;
            DoctorEntryStack.IsVisible = false;
            AddNoteslbl.IsVisible = false;
            AddNotesStack.IsVisible = false;
            DoctorNotesinfo.IsVisible = false;
            AddNotesinfo.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void FeedbackAdd_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                FeedbackAdd.IsEnabled = false;
                //Validation 
                if (btnno.BackgroundColor == Colors.Transparent && btnyes.BackgroundColor == Colors.Transparent)
                {
                    await DisplayAlert("Did you Attend", "select if you Attend the Appointment", "Close");
                    FeedbackAdd.IsEnabled = true;
                    return;
                }
                else if (Attended == "No")
                {
                    //No Selected only update Attended
                    AppointmentPassed.attended = "No";

                    if (AppointmentPassed.feedback == null || AppointmentPassed.feedback.Count == 0)
                    {
                      //Do Nothings
                    }
                    else
                    {
                        AppointmentPassed.feedback.Clear();
                    }
                }
                else if (Attended == "Yes")
                {
                    //Yes Selected 
                    if (string.IsNullOrEmpty(Durationlbl.Text))
                    {
                        await DisplayAlert("Actual Duration", "select either Days, Hours or Minutes", "Close");
                        FeedbackAdd.IsEnabled = true;
                        return;
                    }
                    else if (string.IsNullOrEmpty(DurationEntry.Text))
                    {
                        await DisplayAlert("Actual Duration", "Duration cannot be Empty, Please enter a Duration", "Close");
                        FeedbackAdd.IsEnabled = true;
                        return;
                    }
                    else
                    {
                        //update Appointment Attended
                        AppointmentPassed.attended = "Yes";

                        if (AppointmentPassed.feedback == null || AppointmentPassed.feedback.Count == 0)
                        {
                            AppointmentPassed.feedback = new ObservableCollection<appointmentfeedback>();
                        }
                        else
                        {
                            AppointmentPassed.feedback.Clear();
                        }


                        //Update Feedback 
                        AppointmentPassed.feedback.Add(new appointmentfeedback
                        {
                            ActualDuration = DurationEntry.Text + " " + Durationlbl.Text,
                            DoctorsNotes = DoctorNotes.Text,
                            AdditionalNotes = AddNotes.Text
                        });
                    }
                }

                //Update Appointment Feedback
                APICalls database = new APICalls();
                await database.UpdateAppointmentItem(AppointmentPassed);


                foreach (var item in AllAppointments)
                {
                    if (item.id == AppointmentPassed.id)
                    {
                        Appointtoremove.Add(item);
                    }
                }

                foreach (var items in Appointtoremove)
                {
                    AllAppointments.Remove(items);
                }
                AllAppointments.Add(AppointmentPassed);

                //Change View to Feedback View 

                //AddFeedbackStack.IsVisible = false;
                //FinalFeedbackStack.IsVisible = true;
                //PopulatePage();

                await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Feedback Added") { });
                await Task.Delay(1500);
                await Navigation.PushAsync(new SingleAppointment(AppointmentPassed, AllAppointments));
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleAppointment);
                if (pageToRemoves != null)
                {

                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);
                FeedbackAdd.IsEnabled = true;
                await MopupService.Instance.PopAllAsync(false);


                //WeakReferenceMessenger.Default.Send(new UpdateAppFeedback(AllAppointments, AppointmentPassed.id));


                //await Navigation.PushAsync(new Appointments(AllAppointments, AllHCPs));
                //var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is Appointments);
                //if (pageToRemoves != null)
                //{

                //    Navigation.RemovePage(pageToRemoves);
                //}
                //Navigation.RemovePage(this);
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

    private void TimeChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var SelectedChip = e.AddedItem;
            Durationlbl.Text = SelectedChip.ToString();
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    private void DurationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (e.NewTextValue.Contains("."))
            {
                var NewValue = e.NewTextValue;
                DurationEntry.Text = NewValue.Replace(".", "");
            }
        }
        catch (Exception Ex)
        {
            //leave Empty
        }
    }
}