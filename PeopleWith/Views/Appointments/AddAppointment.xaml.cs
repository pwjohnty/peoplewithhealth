using System.Collections.ObjectModel;
using Mopups.Services;
using Syncfusion.Maui.DataSource.Extensions;
using Syncfusion.Maui.Picker;

namespace PeopleWith;

public partial class AddAppointment : ContentPage
{
    string SelectedDate;
    string SelectedTime;
    string SelectedLocation;
    string SelectedType;
    string Duration;
    string SelectedHCP;
    string SelectedHCPName; 
    string AppointmentReminderSelected; 
    public ObservableCollection<appointment> locaitonlist = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> Typelist = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> AppointmenttoAdd = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> AddedAppointment = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> AllAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<hcp> AllHCPs = new ObservableCollection<hcp>();
    public ObservableCollection<hcp> itemstoremove = new ObservableCollection<hcp>();
    public hcp SelectHCP = new hcp();
    public TimeSpan ReminderTimepsan = new TimeSpan();
    string NavFrom; 
    AppointmentNotifications AppointmentNotif = new AppointmentNotifications(); 
    public ObservableCollection<string> ReminderList { get; set; } = new ObservableCollection<string>();
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


    protected override bool OnBackButtonPressed()
    {
        try
        {
            //AllHCPs Remove (No HCP) 
            foreach (var item in AllHCPs)
            {
                if (item.fullname == "No HCP")
                {
                    itemstoremove.Add(item);
                }
            }
            foreach (var items in itemstoremove)
            {
                AllHCPs.Remove(items);
            }

            return false;
            
        }
        catch (Exception Ex)
        {
            //Add Crash log
            NotasyncMethod(Ex);
            return false;
        }
    }

    public AddAppointment(hcp HCPSelected, ObservableCollection<hcp> GetAllHCPs)
    {
        try
        {
            InitializeComponent();
            AllHCPs = GetAllHCPs;
            SelectHCP = HCPSelected;
            //Add No HCP item 
            var item = "No HCP";
            bool Check = AllHCPs.Any(s => s.fullname.Contains(item, StringComparison.OrdinalIgnoreCase));
            if (Check)
            {
                //Do Nothing 
            }
            else
            {
                var EMPTYHCP = new hcp();
                EMPTYHCP.fullname = "No HCP";
                EMPTYHCP.hcpid = "1";
                AllHCPs.Add(EMPTYHCP);
            }

            NavFrom = "HCPs";

            PopulateAllItems();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    public AddAppointment(ObservableCollection<hcp> GetAllHCPs, ObservableCollection<appointment> GetAllAppointments)
	{
        try
        {
            InitializeComponent();
            AllHCPs = GetAllHCPs;
            AllAppointments = GetAllAppointments;
            //Add No HCP item 
            var item = "No HCP";
            bool Check = AllHCPs.Any(s => s.fullname.Contains(item, StringComparison.OrdinalIgnoreCase));
            if (Check)
            {
                //Do Nothing 
            }
            else
            {
                var EMPTYHCP = new hcp();
                EMPTYHCP.fullname = "No HCP";
                EMPTYHCP.hcpid = "1";
                AllHCPs.Add(EMPTYHCP);
            }

            NavFrom = "Appointments";

            PopulateAllItems();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public void PopulateAllItems()
    {
        try
        {
            DurationPicker.SelectedTime = new TimeSpan(0, 0, 0);

            //Location Chips Populate
            var locationone = new appointment();
            locationone.location = "GP Surgery";
            locaitonlist.Add(locationone);

            var locationtwo = new appointment();
            locationtwo.location = "Hospital";
            locaitonlist.Add(locationtwo);

            var locationthree = new appointment();
            locationthree.location = "A&E";
            locaitonlist.Add(locationthree);

            var locationfour = new appointment();
            locationfour.location = "Private Consultation";
            locaitonlist.Add(locationfour);

            locationChips.ItemsSource = locaitonlist;

            //Type Chips Populate
            var typeone = new appointment();
            typeone.type = "In Person";
            Typelist.Add(typeone);

            var typetwo = new appointment();
            typetwo.type = "Virtual";
            Typelist.Add(typetwo);

            var typethree = new appointment();
            typethree.type = "Telephone";
            Typelist.Add(typethree);

            TypeChips.ItemsSource = Typelist;

            PickerColumn pickerColumn = new PickerColumn
            {
                ItemsSource = AllHCPs,
                DisplayMemberPath = "fullname"
            };

            hcpPicker.Columns.Add(pickerColumn);

            addtimepicker.Time = DateTime.Now.TimeOfDay;


            //Navigation From SelectedHCP 

            if(NavFrom == "HCPs")
            {
                var hcpname = SelectHCP.fullname; 
                hcpPicker.Columns[0].SelectedItem = hcpname;
            }
            else
            {
                hcpPicker.Columns[0].SelectedItem = "No HCP";
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async public void PopulateAppointmentReminder()
    {
        try
        {
            SelectedDate = adddatepicker.Date.ToString("dd/MM/yyyy");
            SelectedTime = addtimepicker.Time.ToString(@"hh\:mm");

            var SelectedDateTime = DateTime.Parse(SelectedDate + " " + SelectedTime);
            var CurrentDateTime = DateTime.Now;

            //Calcute TimeSpan Between 
            ReminderList.Clear();
            double DiffinDate = (SelectedDateTime - CurrentDateTime).TotalMinutes;
            if (DiffinDate < 15)
            {
                ReminderList.Add("No Reminder");
               
            }
            else if (DiffinDate > 15 && DiffinDate < 30)
            {
                ReminderList.Add("15 Minutes Before");
            }
            else if (DiffinDate > 30 && DiffinDate < 45)
            {
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
            }
            else if (DiffinDate > 45 && DiffinDate < 60)
            {
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
                ReminderList.Add("45 Minutes Before");
            }
            else if (DiffinDate > 60 && DiffinDate <= 1440)
            {
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
                ReminderList.Add("45 Minutes Before");
                ReminderList.Add("1 Hour Before");
            }
            else if (DiffinDate >= 1441)
            {
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
                ReminderList.Add("45 Minutes Before");
                ReminderList.Add("1 Hour Before");
                ReminderList.Add("1 Day Before");
            }
            AppointmentReminder.ItemsSource = ReminderList;

            if(ReminderList.Count == 1)
            {
                AppointmentReminder.ChipBackground = Color.FromRgba("#ffcccb");
                AppointmentReminder.ChipTextColor = Color.FromRgba("#031926");
                //AppointmentReminder.Items[0];
                //AppointmentReminder.SelectedItem = ReminderList[0];
            }
            else
            {
                AppointmentReminder.ChipBackground = Colors.Transparent;
                AppointmentReminder.ChipTextColor = Colors.Gray;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void DurationPicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.TimePickerSelectionChangedEventArgs e)
    {
        try
        {
            var Time = e.NewValue.ToString();
            var split = Time.Split(':');
            var getTime = split[0] + " Hours " + split[1] + " Minutes";
            Duration = getTime;
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }


    private void locationChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {

        try
        {
            var appointment = e.AddedItem as appointment;

            if (appointment != null)
            {
                SelectedLocation = appointment.location;
            }
        }
        catch (Exception Ex)
        {
           //Leave Empty
        }
    }

    private void TypeChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var appointment = e.AddedItem as appointment;

            if (appointment != null)
            {
                SelectedType = appointment.type;
            }
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }
    private void HCPPicker_SelectionChanged(object sender, PickerSelectionChangedEventArgs e)
    {
        try
        {
            var selectedHCP = e.NewValue;

            int i = 0;
            foreach(var item in AllHCPs)
            {
                if(i == selectedHCP)
                {
                    SelectedHCP = item.hcpid;
                    SelectedHCPName = item.fullname; 
                }
                i = i + 1; 
            }
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    private void adddatepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            PopulateAppointmentReminder(); 
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }

    }

    private void addtimepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            PopulateAppointmentReminder();
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    private void AppointmentReminder_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var chipGroup = e.AddedItem as string; 

            if (chipGroup != null)
            {
                AppointmentReminderSelected = chipGroup;
            }
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    async private void AppointmentAdd_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AppointmentAdd.IsEnabled = false;

                if (string.IsNullOrEmpty(SelectedLocation))
                {
                    Vibration.Vibrate();
                    await DisplayAlert("Select Location", "Please select a location from this list", "Ok");
                    return;
                }
                else if (string.IsNullOrEmpty(SelectedType))
                {
                    Vibration.Vibrate();
                    await DisplayAlert("Select Type", "Please select a type from this list", "Ok");
                    return;
                }
                else
                {
                    var NewAppointment = new appointment();
                    //Get Selected DateTime
                    string dateandtime = SelectedDate + " " + SelectedTime;
                    //var datetime = DateTime.Parse(SelectedDate + " " + SelectedTime);
                    NewAppointment.datetime = dateandtime;

                    //Get Location 
                    NewAppointment.location = SelectedLocation;

                    //GetType
                    NewAppointment.type = SelectedType;

                    //Get HCP (As HCP ID) if 1 Then it's NoHCP
                    if (String.IsNullOrEmpty(SelectedHCP) || String.IsNullOrEmpty(SelectedHCPName))
                    {

                        //Selected index of Sfpicker 0 
                        NewAppointment.hcpname = AllHCPs[0].fullname;
                        NewAppointment.hcpid = AllHCPs[0].hcpid;
                    }
                    else
                    {
                        if (SelectedHCP == "No HCP")
                        {
                            //Dont Add
                        }
                        else
                        {
                            NewAppointment.hcpid = SelectedHCP;
                            NewAppointment.hcpname = SelectedHCPName;
                        }
                    }


                    //Get Duration (Convert for Notifiction)
                    if (Duration == "00 Hours 00 Minutes")
                    {
                        //Dont Add
                    }
                    else
                    {
                        NewAppointment.expectedduration = Duration;

                    }

                    //Get Notes (Can Be Empty) 
                    NewAppointment.reason = notesentry.Text;

                    NewAppointment.userid = Helpers.Settings.UserKey;

                    //Get Appointment Reminder (Can Be Empty) 
                    if (string.IsNullOrEmpty(AppointmentReminderSelected))
                    {

                    }
                    else
                    {
                        if (AppointmentReminderSelected == "No Reminder")
                        {
                            NewAppointment.reminderinterval = AppointmentReminderSelected;
                        }
                        else
                        {
                            if (AppointmentReminderSelected == "15 Minutes Before")
                            {
                                NewAppointment.Reminder = new TimeSpan(0, 15, 0);
                            }
                            else if (AppointmentReminderSelected == "30 Minutes Before")
                            {
                                NewAppointment.Reminder = new TimeSpan(0, 30, 0);
                            }
                            else if (AppointmentReminderSelected == "45 Minutes Before")
                            {
                                NewAppointment.Reminder = new TimeSpan(0, 45, 0);
                            }
                            else if (AppointmentReminderSelected == "1 Hour Before")
                            {
                                NewAppointment.Reminder = new TimeSpan(1, 0, 0);
                            }
                            else if (AppointmentReminderSelected == "1 Day Before")
                            {
                                NewAppointment.Reminder = new TimeSpan(1, 0, 0, 0);
                            }

                            //Add Appontment Notification (Id saved in Reminder Interval)
                            Random random = new Random();
                            int notid = random.Next(100000, 100000001);

                            await AppointmentNotif.AddAppointment(NewAppointment);

                            NewAppointment.reminderinterval = AppointmentReminderSelected;
                        }
                    }

                    AppointmenttoAdd.Add(NewAppointment);

                    APICalls database = new APICalls();
                    AddedAppointment = await database.PostUserAppointmentAsync(AppointmenttoAdd);

                    AppointmentAdd.IsEnabled = true;
                    //AllHCPs Remove (No HCP) 
                    foreach (var item in AllHCPs)
                    {
                        if (item.fullname == "No HCP")
                        {
                            itemstoremove.Add(item);
                        }
                    }
                    foreach (var items in itemstoremove)
                    {
                        AllHCPs.Remove(items);
                    }


                    //Navigation From Appointments 

                    if (NavFrom == "Appointments")
                    {

                        foreach (var item in AddedAppointment)
                        {
                            AllAppointments.Add(item);
                        }
                        await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Added") { });
                        await Task.Delay(1500);

                        await MopupService.Instance.PopAllAsync(false);
                        await Navigation.PushAsync(new Appointments(AllAppointments, AllHCPs));
                        var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is Appointments);
                        if (pageToRemoves != null)
                        {

                            Navigation.RemovePage(pageToRemoves);
                        }
                        Navigation.RemovePage(this);

                    }
                    else
                    {
                        //Navigation From HCP's

                        await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Added") { });
                        await Task.Delay(1500);

                        await MopupService.Instance.PopAllAsync(false);

                        await Navigation.PushAsync(new SingleHCP(SelectHCP, AllHCPs));
                        var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleHCP);
                        if (pageToRemoves != null)
                        {

                            Navigation.RemovePage(pageToRemoves);
                        }
                        Navigation.RemovePage(this);
                    }
                }
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