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
    public appointment SelectedAppointment = new appointment(); 
    public hcp SelectHCP = new hcp();
    public TimeSpan ReminderTimepsan = new TimeSpan();
    string NavFrom; 
    AppointmentNotifications AppointmentNotif = new AppointmentNotifications();
    APICalls database = new APICalls();
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
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }


    protected override void OnDisappearing()
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
            
        }
        catch (Exception Ex)
        {
            //Add Crash log
            NotasyncMethod(Ex);
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
            NavFrom = "HCPs";

            GetHCPData();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddAppointment(appointment AppointmentSelected, ObservableCollection<appointment> GetAllAppointments)
    {
        try
        {
            InitializeComponent();
            AllAppointments = GetAllAppointments;
            SelectedAppointment = AppointmentSelected;
            NavFrom = "EditAppoint";

            AppointmentAdd.Text = "Update Appointment";

            GetHCPData();

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    public AddAppointment( ObservableCollection<appointment> GetAllAppointments)
	{
        try
        {
            InitializeComponent();
            AllAppointments = GetAllAppointments;
            NavFrom = "Appointments";
            GetHCPData();
           
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void GetHCPData()
    {
        try
        {

            var getHCPsTask = database.GetUserHCP();

            AllHCPs = await getHCPsTask;
            foreach (var items in AllHCPs)
            {
                items.fullname = items.firstname + " " + items.surname;
            }

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
                //Set it to be First Appearing 
                AllHCPs.Insert(0, EMPTYHCP);
                //AllHCPs.Add(EMPTYHCP);
            }

            PopulateAllItems();

            if(AppointmentAdd.Text == "Update Appointment")
            {
                PrepopulateItems(); 
            }
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

            //New Way

            var sortedlist = 
            HcpChips.ItemsSource = AllHCPs; 

            //Old Way of Choosing HCP
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

    public void PrepopulateItems()
    {
        try
        {
            //Pre-Populate Date/Time
            var GetDatetime = DateTime.Parse(SelectedAppointment.datetime);
            adddatepicker.Date = GetDatetime.Date;
            addtimepicker.Time = GetDatetime.TimeOfDay;

            //Pre-Populate Location 
            if (!string.IsNullOrEmpty(SelectedAppointment.location))
            {

                //locationChips.SelectedItem = SelectedAppointment.location;
                locationChips.SelectedItem = locaitonlist.FirstOrDefault(item => item.location == SelectedAppointment.location);
            }

            //Pre-Populate Type 
            if (!string.IsNullOrEmpty(SelectedAppointment.type))
            {
                TypeChips.SelectedItem = Typelist.FirstOrDefault(item => item.type == SelectedAppointment.type);
                //TypeChips.SelectedItem = SelectedAppointment.type;
            }

            //Pre-Populate HCP
            if (!string.IsNullOrEmpty(SelectedAppointment.hcpname))
            {
                //hcpPicker.Columns[0].SelectedItem = "No HCP";
                //HcpChips.SelectedItem = "No HCP";
                HcpChips.SelectedItem = AllHCPs.FirstOrDefault(item => item.fullname == SelectedAppointment.hcpname);
                
            }
            else
            {
                //hcpPicker.Columns[0].SelectedItem = SelectedAppointment.hcpname;
                //HcpChips.SelectedItem = SelectedAppointment.hcpname;
                HcpChips.SelectedItem = AllHCPs.FirstOrDefault(item => item.fullname == "No HCP");
            }

            //Pre-Populate Duration 
            if (!string.IsNullOrEmpty(SelectedAppointment.expectedduration))
            {
                var GetDuration = SelectedAppointment.expectedduration;
                var cleanInput = GetDuration.Replace(" Hours", "").Replace(" Minutes", "").Trim();
                var GetSplit = cleanInput.Split(' ');
                hoursentry.Text = GetSplit[0];
                minsentry.Text = GetSplit[1];
            }


            //Reminder 
            if (!string.IsNullOrEmpty(SelectedAppointment.reminderinterval))
            {
                //Only No Reminder
                if (ReminderList.Count == 1)
                {
                    AppointmentReminder.SelectedItem = ReminderList[0];
                }
                else
                {
                    if (ReminderList.Contains(SelectedAppointment.reminderinterval))
                    {
                        var index = ReminderList.IndexOf(SelectedAppointment.reminderinterval); 
                        AppointmentReminder.SelectedItem = ReminderList[index];
                    }
                    else
                    {
                        //Not Valid Date time so set to Nothing

                    }
                }
            }
            else
            {
                //If it is null 
                AppointmentReminder.SelectedItem = ReminderList[0];
            }

            //Reason
            notesentry.Text = SelectedAppointment.reason;

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
                ReminderList.Add("No Reminder");
                ReminderList.Add("15 Minutes Before");
            }
            else if (DiffinDate > 30 && DiffinDate < 45)
            {
                ReminderList.Add("No Reminder");
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
            }
            else if (DiffinDate > 45 && DiffinDate < 60)
            {
                ReminderList.Add("No Reminder");
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
                ReminderList.Add("45 Minutes Before");
            }
            else if (DiffinDate > 60 && DiffinDate <= 1440)
            {
                ReminderList.Add("No Reminder");
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
                ReminderList.Add("45 Minutes Before");
                ReminderList.Add("1 Hour Before");
            }
            else if (DiffinDate >= 1441)
            {
                ReminderList.Add("No Reminder");
                ReminderList.Add("15 Minutes Before");
                ReminderList.Add("30 Minutes Before");
                ReminderList.Add("45 Minutes Before");
                ReminderList.Add("1 Hour Before");
                ReminderList.Add("1 Day Before");
            }
            AppointmentReminder.ItemsSource = ReminderList;

            if(ReminderList.Count == 1)
            {
                AppointmentReminder.SelectedItem = ReminderList.FirstOrDefault("No Reminder");
                //AppointmentReminder.ChipBackground = Color.FromRgba("#ffe4e1");
                //AppointmentReminder.ChipTextColor = Color.FromRgba("#031926");
                //AppointmentReminder.Items[0];
                //AppointmentReminder.SelectedItem = ReminderList[0];
            }
            //else
            //{
            //    AppointmentReminder.ChipBackground = Colors.Transparent;
            //    AppointmentReminder.ChipTextColor = Colors.Gray;
            //}
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
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


    private void HcpChips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var appointment = e.AddedItem as hcp;

            if (appointment != null)
            {
                SelectedHCP = appointment.hcpid;
                SelectedHCPName = appointment.fullname;
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
                    AppointmentAdd.IsEnabled = true;
                    return;
                }
                else if (string.IsNullOrEmpty(SelectedType))
                {
                    Vibration.Vibrate();
                    await DisplayAlert("Select Type", "Please select a type from this list", "Ok");
                    AppointmentAdd.IsEnabled = true;
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
                        if (SelectedHCP == "No HCP" || SelectedHCP == "1")
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
                    if (string.IsNullOrEmpty(hoursentry.Text) && string.IsNullOrEmpty(minsentry.Text))
                    {
                        NewAppointment.expectedduration = null;
                    }
                    else
                    {
                        var hours = "";
                        var mins = "";

                        if (string.IsNullOrEmpty(hoursentry.Text))
                        {
                            hours = "00";
                        }
                        else
                        {
                            hours = hoursentry.Text;
                        }

                        if (string.IsNullOrEmpty(minsentry.Text))
                        {
                            mins = "00";
                        }
                        else
                        {
                            mins = minsentry.Text;
                        }

                        var timestring = hours + " Hours " + mins + " Minutes";
                        NewAppointment.expectedduration = timestring;
                    }
                    //if (Duration == "00 Hours 00 Minutes")
                    //{
                    //    //Dont Add
                    //}
                    //else
                    //{
                    //    NewAppointment.expectedduration = Duration;

                    //}

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


                    if (AppointmentAdd.Text == "Update Appointment")
                    {
                        NewAppointment.id = SelectedAppointment.id; 
                    }

                    AppointmenttoAdd.Add(NewAppointment);

                    APICalls database = new APICalls();
                    AddedAppointment = await database.PostUserAppointmentAsync(AppointmenttoAdd);
                                  
                    AppointmentAdd.IsEnabled = true;
                    //AllHCPs Remove (No HCP) 
                    foreach (var item in AllHCPs)
                    {
                        if (item.fullname == "No HCP" || item.fullname == "1")
                        {
                            itemstoremove.Add(item);
                        }
                    }
                    foreach (var items in itemstoremove)
                    {
                        AllHCPs.Remove(items);
                    }


                    //Navigation From Appointments 

                    if (NavFrom == "Appointments" || NavFrom == "EditAppoint")
                    {

                        foreach (var item in AddedAppointment)
                        {
                            AllAppointments.Add(item);
                        }

                        if (AppointmentAdd.Text == "Update Appointment")
                        {
                            await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Updated") { });
                        }
                        else
                        {
                            await MopupService.Instance.PushAsync(new PopupPageHelper("Appointment Added") { });
                        }

                        
                        await Task.Delay(1500);

                        await MopupService.Instance.PopAllAsync(false);
                        await Navigation.PushAsync(new AllAppointments());
                        var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllAppointments);
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

    private void hoursentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //hours entry text changed
            var entry = sender as Entry;
            if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;

            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void minsentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var entry = sender as Entry;
            if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;

            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }

                // Validate the value is within the range (0-59)
                if (int.TryParse(newText, out int minutes))
                {
                    if (minutes > 59)
                    {
                        newText = "00"; // Set to max value
                    }
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void fifteenminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 15 minutes
            minutes += 15;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void thirtyminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 15 minutes
            minutes += 30;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void sixtyminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 60 minutes (equivalent to adding 1 hour)
            hours += 1;

            // Handle hour overflow (optional, wrap to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0; // Reset to 0 if over 23 (for 24-hour format)
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void ninetyminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            // Parse the current hour and minute values
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 1 hour and 30 minutes
            minutes += 30;
            hours += 1;

            // Handle minute overflow
            if (minutes >= 60)
            {
                minutes -= 60; // Adjust minutes
                hours += 1;    // Increment hours for overflow
            }

            // Handle hour overflow (optional, wrap to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0; // Reset to 0 if over 23 (for 24-hour format)
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

   
}