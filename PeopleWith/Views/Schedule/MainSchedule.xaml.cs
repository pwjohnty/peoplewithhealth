using Syncfusion.Maui.Charts;
using Syncfusion.Maui.DataSource;
using Syncfusion.Maui.DataSource.Extensions;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using Mopups.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace PeopleWith;

public partial class MainSchedule : ContentPage
{
    List<DateTime> dateList = new List<DateTime>();
    public List<Schedule> changeddatesforlistview = new List<Schedule>();

    APICalls aPICalls = new APICalls();
    public ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<MedSuppFeedback> MedsFeedbackRemove = new ObservableCollection<MedSuppFeedback>();
    public ObservableCollection<usersupplement> AllUserSupplements = new ObservableCollection<usersupplement>();
    public ObservableCollection<MedtimesDosages> ScheduleList = new ObservableCollection<MedtimesDosages>();
    public ObservableCollection<MedtimesDosages> AsRequiredList = new ObservableCollection<MedtimesDosages>();
    public DateTime dateforschedule = new DateTime();
    public ObservableCollection<IGrouping<string, MedtimesDosages>> GroupedScheduleList { get; set; }
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


    public MainSchedule()
	{
        try
        {
            InitializeComponent();
            getusermedsandsupps();

            //get 7 days before and 7 days after todays date

            // Get today's date
            DateTime today = DateTime.Today;

            // Initialize a list to hold the dates
            // Add 7 days before today's date
            for (int i = -30; i <= 7; i++)
            {
                dateList.Add(today.AddDays(i));
            }

            foreach (var item in dateList)
            {
                var newitem = new Schedule();

                newitem.Day = item.Day.ToString();
                newitem.Date = item.Date.ToString("ddd");

                if (item.Date > DateTime.Now.Date)
                {
                    newitem.Bgcolour = Colors.Transparent;
                    newitem.Bordercolour = Color.FromArgb("#e5f0fb");
                    newitem.Op = 0.5;
                }
                else
                {
                    newitem.Bgcolour = Color.FromArgb("#e5f0fb");
                    newitem.Bordercolour = Colors.Transparent;
                    newitem.Op = 1;
                }

                changeddatesforlistview.Add(newitem);
            }

            scheduledatelist.ItemsSource = changeddatesforlistview;

            // Find today's date in the list

            var indexForToday = dateList.IndexOf(today);

            // Check if today's date is in the list
            if (indexForToday >= 0)
            {
                // Set the selected item to today's date
                scheduledatelist.SelectedItem = changeddatesforlistview[indexForToday];

                var dateforlabel = dateList[indexForToday];

                datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");
                dateforschedule = dateList[indexForToday];

                // Scroll to today's date and try to center it
                scheduledatelist.ScrollTo(changeddatesforlistview[indexForToday], ScrollToPosition.Center, true);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void scheduledatelist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //hide As Required list and Change Button back to Inital 

            asrequiredbtn.Text = "As Required Medications/Supplements";
            mainschedulelistview.IsVisible = true;
            AsRequiredlistview.IsVisible = false;
           
            var item = e.DataItem as Schedule;

            // Get the index of the tapped item from the ItemsSource
            var itemsSource = (sender as SfListView).ItemsSource as IList<Schedule>;
            int index = itemsSource.IndexOf(item);


            var dateforlabel = dateList[index];

            dateforschedule = dateList[index];

            datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");


            populateschedule();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getusermedsandsupps()
    {
        try
        {

            AllUserMedications = await aPICalls.GetUserMedicationsAsync();

            AllUserSupplements = await aPICalls.GetUserSupplementsAsync();

            populateschedule();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async void populateschedule()
    {
        try
        {
            ScheduleList.Clear();
            var sd = new DateTime();
            var ed = new DateTime();
            bool hasnoendate = new bool();

            foreach (var item in AllUserMedications)
            {
                 sd = DateTime.Parse(item.startdate);

                if(!string.IsNullOrEmpty(item.enddate))
                {
                     ed = DateTime.Parse(item.enddate);
                }
                else
                {
                    hasnoendate = true;
                }

                //find out if it is daily, weekly or days interval

                var splitstring = item.frequency.Split('|');

                if (splitstring[0] == "Daily")
                {

                   if(hasnoendate)
                    {
                        
                        if(dateforschedule >= sd)
                        {

                            //add all the items 
                            foreach(var medtimes in item.schedule)
                            {
                                medtimes.Type = "Medication";
                                medtimes.Usermedid = item.id;
                                medtimes.Feedbackid = medtimes.id.ToString();
                                medtimes.Name = item.medicationtitle;
                                ScheduleList.Add(medtimes);
                            }


                        }


                    }
                   else
                    {

                        if(dateforschedule >= sd && dateforschedule <= ed)
                        {
                            //add all the items 
                            foreach (var medtimes in item.schedule)
                            {
                                medtimes.Type = "Medication";
                                medtimes.Usermedid = item.id;
                                medtimes.Feedbackid = medtimes.id.ToString();
                                medtimes.Name = item.medicationtitle;
                                ScheduleList.Add(medtimes);
                            }
                        }

                    }





                }
                else if (splitstring[0] == "Weekly")
                {
                    if (hasnoendate)
                    {

                        if (dateforschedule >= sd)
                        {

                            var dayname = dateforschedule.ToString("ddd");
                            var daynames = dateforschedule.ToString("dddd");
                            var dayfour = daynames.Substring(0, 4); 
                            //check if today is within the days list
                            if (splitstring[1].Contains(dayname))
                            {
                                int Index = 0;
                                foreach (var x in item.schedule)
                                {
                                    var GetDay = item.TimeDosage[Index].Split('|');
                                    x.Day = GetDay[2];
                                    Index = Index + 1;
                                }

                                //add all the items 
                                foreach (var medtimes in item.schedule)
                                {
                                    if(medtimes.Day == dayname || medtimes.Day == dayfour)
                                    {
                                        medtimes.Type = "Medication";
                                        medtimes.Usermedid = item.id;
                                        medtimes.Feedbackid = medtimes.id.ToString();
                                        medtimes.Name = item.medicationtitle;
                                        ScheduleList.Add(medtimes);
                                    }
                                }
                            }            
                        }
                    }
                    else
                    {

                        if (dateforschedule >= sd && dateforschedule <= ed)
                        {
                            var dayname = dateforschedule.ToString("ddd");
                            //check if today is within the days list
                            if (splitstring[1].Contains(dayname))
                            {
                                int Index = 0;
                                foreach (var x in item.schedule)
                                {
                                    var GetDay = item.TimeDosage[Index].Split('|');
                                    x.Day = GetDay[2];
                                    Index = Index + 1;
                                }
                                //add all the items 
                                foreach (var medtimes in item.schedule)
                                {
                                    if (medtimes.Day == dayname)
                                    {
                                        medtimes.Type = "Medication";
                                        medtimes.Usermedid = item.id;
                                        medtimes.Feedbackid = medtimes.id.ToString();
                                        medtimes.Name = item.medicationtitle;
                                        ScheduleList.Add(medtimes);
                                    }
                                }
                            }
                        }

                    }
                }
                else if (splitstring[0] == "Days Interval")
                {

                    var daycount = Convert.ToInt32(splitstring[1]);

                    if (hasnoendate)
                    {

                        if (dateforschedule >= sd)
                        {

                            // Calculate the difference in days from the start date
                            var daysDifference = (dateforschedule - sd).Days;

                            // Check if today's date is a multiple of the daycount (e.g., every 3rd day)
                            if (daysDifference % daycount == 0)
                            {
                                // Add all the items for today
                                foreach (var medtimes in item.schedule)
                                {
                                    medtimes.Type = "Medication";
                                    medtimes.Usermedid = item.id;
                                    medtimes.Feedbackid = medtimes.id.ToString();
                                    medtimes.Name = item.medicationtitle;
                                    ScheduleList.Add(medtimes);
                                }
                            }
                        }
                    }
                    else
                    {

                        if (dateforschedule >= sd && dateforschedule <= ed)
                        {
                            // Calculate the difference in days from the start date
                            var daysDifference = (DateTime.Now - sd).TotalDays;

                            // Check if today's date is a multiple of the daycount (e.g., every 3rd day)
                            if (daysDifference % daycount == 0)
                            {
                                // Add all the items for today
                                foreach (var medtimes in item.schedule)
                                {
                                    medtimes.Type = "Medication";
                                    medtimes.Usermedid = item.id;
                                    medtimes.Feedbackid = medtimes.id.ToString();
                                    medtimes.Name = item.medicationtitle;
                                    ScheduleList.Add(medtimes);
                                }
                            }
                        }

                    }


                }
                else if (splitstring[0] == "As Required")
                {
                    if (item.feedback != null)
                    {

                        foreach (var medtimes in item.feedback)
                        {
                            var convertdate = DateTime.Parse(medtimes.datetime);
    
                            if(convertdate.Date == dateforschedule.Date)
                            {
                                var newitem = new MedtimesDosages();
                                newitem.Type = "Medication";
                                newitem.Usermedid = item.id;
                                newitem.Feedbackid = medtimes.id.ToString();
                                newitem.Name = item.medicationtitle;
                                newitem.Dosage = medtimes.Recorded + " " + item.unit;
                                newitem.time = "00:00";
                                newitem.Buttonop = 1;
                                newitem.Buttonntop = 0;
                                newitem.AsReqlblVis = true;
                                ScheduleList.Add(newitem);
                            }
                        }
                    }


                }

            }

            foreach (var item in AllUserSupplements)
            {
                sd = DateTime.Parse(item.startdate);

                if (!string.IsNullOrEmpty(item.enddate))
                {
                    ed = DateTime.Parse(item.enddate);
                }
                else
                {
                    hasnoendate = true;
                }

                //find out if it is daily, weekly or days interval

                var splitstring = item.frequency.Split('|');

                if (splitstring[0] == "Daily")
                {

                    if (hasnoendate)
                    {

                        if (dateforschedule >= sd)
                        {

                            //add all the items 
                            foreach (var medtimes in item.schedule)
                            {
                                medtimes.Type = "Supplement";
                                medtimes.Usermedid = item.id;
                                medtimes.Feedbackid = medtimes.id.ToString();
                                medtimes.Name = item.supplementtitle;
                                ScheduleList.Add(medtimes);
                            }


                        }


                    }
                    else
                    {

                        if (dateforschedule >= sd && dateforschedule <= ed)
                        {
                            //add all the items 
                            foreach (var medtimes in item.schedule)
                            {
                                medtimes.Type = "Supplement";
                                medtimes.Usermedid = item.id;
                                medtimes.Feedbackid = medtimes.id.ToString();
                                medtimes.Name = item.supplementtitle;
                                ScheduleList.Add(medtimes);
                            }
                        }

                    }





                }
                else if (splitstring[0] == "Weekly")
                {
                    if (hasnoendate)
                    {

                        if (dateforschedule >= sd)
                        {

                            var dayname = dateforschedule.ToString("ddd");
                            //check if today is within the days list
                            if (splitstring[1].Contains(dayname))
                            {
                                int Index = 0;
                                foreach (var x in item.schedule)
                                {
                                    var GetDay = item.TimeDosage[Index].Split('|');
                                    x.Day = GetDay[2];
                                    Index = Index + 1;
                                }
                                //add all the items 
                                foreach (var medtimes in item.schedule)
                                {
                                    if (medtimes.Day == dayname)
                                    {
                                        medtimes.Type = "Supplement";
                                        medtimes.Usermedid = item.id;
                                        medtimes.Feedbackid = medtimes.id.ToString();
                                        medtimes.Name = item.supplementtitle;
                                        ScheduleList.Add(medtimes);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        if (dateforschedule >= sd && dateforschedule <= ed)
                        {
                            var dayname = dateforschedule.ToString("ddd");
                            //check if today is within the days list
                            if (splitstring[1].Contains(dayname))
                            {
                                int Index = 0;
                                foreach (var x in item.schedule)
                                {
                                    var GetDay = item.TimeDosage[Index].Split('|');
                                    x.Day = GetDay[2];
                                    Index = Index + 1;
                                }
                                //add all the items 
                                foreach (var medtimes in item.schedule)
                                {
                                    if (medtimes.Day == dayname)
                                    {
                                        medtimes.Type = "Supplement";
                                        medtimes.Usermedid = item.id;
                                        medtimes.Feedbackid = medtimes.id.ToString();
                                        medtimes.Name = item.supplementtitle;
                                        ScheduleList.Add(medtimes);

                                    }
                                }
                            }
                        }
                    }
                }
                else if (splitstring[0] == "Days Interval")
                {

                    var daycount = Convert.ToInt32(splitstring[1]);

                    if (hasnoendate)
                    {

                        if (dateforschedule >= sd)
                        {

                            // Calculate the difference in days from the start date
                            var daysDifference = (DateTime.Now - sd).Days;

                            // Check if today's date is a multiple of the daycount (e.g., every 3rd day)
                            if (daysDifference % daycount == 0)
                            {
                                // Add all the items for today
                                foreach (var medtimes in item.schedule)
                                {
                                    medtimes.Type = "Supplement";
                                    medtimes.Usermedid = item.id;
                                    medtimes.Feedbackid = medtimes.id.ToString();
                                    medtimes.Name = item.supplementtitle;
                                    ScheduleList.Add(medtimes);
                                }
                            }
                        }
                    }
                    else
                    {

                        if (dateforschedule >= sd && dateforschedule <= ed)
                        {
                            // Calculate the difference in days from the start date
                            var daysDifference = (DateTime.Now - sd).TotalDays;

                            // Check if today's date is a multiple of the daycount (e.g., every 3rd day)
                            if (daysDifference % daycount == 0)
                            {
                                // Add all the items for today
                                foreach (var medtimes in item.schedule)
                                {
                                    medtimes.Type = "Supplement";
                                    medtimes.Usermedid = item.id;
                                    medtimes.Feedbackid = medtimes.id.ToString();
                                    medtimes.Name = item.supplementtitle;
                                    ScheduleList.Add(medtimes);
                                }
                            }
                        }

                    }


                }
                else if (splitstring[0] == "As Required")
                {
                    if (item.feedback != null)
                    {

                        foreach (var medtimes in item.feedback)
                        {
                            var convertdate = DateTime.Parse(medtimes.datetime);

                            if (convertdate.Date == dateforschedule.Date)
                            {
                                var newitem = new MedtimesDosages();
                                newitem.Type = "Supplement";
                                newitem.Usermedid = item.id;
                                newitem.Feedbackid = medtimes.id.ToString();
                                newitem.Name = item.supplementtitle;
                                newitem.Dosage = medtimes.Recorded + " " + item.unit;
                                newitem.time = "00:00";
                                newitem.Buttonop = 1;
                                newitem.Buttonntop = 0;
                                newitem.AsReqlblVis = true;
                                ScheduleList.Add(newitem);
                            }
                        }
                    }
                }

            }



            if (dateforschedule.Date > DateTime.Now.Date)
                {
                   foreach(var med in ScheduleList)
                    {
                        med.Buttonenabled = false;
                        med.Buttonop = 0;
                         med.Buttonntop = 0;

                    if(med.Type == "Medication")
                    {
                        med.ListBackgroundColor = Color.FromArgb("#e5f9f4");
                   
                    }
                    else
                    {
                        med.ListBackgroundColor = Color.FromArgb("#f9f4e5");
                    }
                        
                    }
                }
                else
                {
                    foreach (var med in ScheduleList)
                    {
                        med.Buttonenabled = true;
                        med.Buttonop = 0.2;
                        med.Buttonntop = 0.2;

                    if (med.Type == "Medication")
                    {

                        med.ListBackgroundColor = Color.FromArgb("#e5f9f4");

                        //check feedback here
                        var getuseremed = AllUserMedications.Where(x => x.id == med.Usermedid).FirstOrDefault();

                        var dt = dateforschedule.ToString("dd/MM/yyyy");


                        // Check if feedback is null before trying to access it
                        if (getuseremed.feedback != null)
                        {
                            // Attempt to find the feedback item
                            var getfeedbackitem = getuseremed.feedback
                                .Where(x => x.id == med.Feedbackid)
                                .Where(x => x.datetime.Contains(dt))
                                .FirstOrDefault();

                            if (getfeedbackitem != null)
                            {
                              
                                if (getfeedbackitem.Recorded == "Taken")
                                {
                                    //Medication Taken 
                                    med.Buttonop = 1;
                                    med.Buttonntop = 0.2;
                                }
                                else if(getfeedbackitem.Recorded == "Not Taken")
                                {
                                    //Not Taken
                                 
                                    med.Buttonop = 0.2;
                                    med.Buttonntop = 1;
                                }
                                else
                                {
                                    med.Buttonop = 0.2;
                                    med.Buttonntop = 0.2;
                                }
                            }
                            else
                            {
                                //Not Recorded
                                med.Buttonop = 0.2;
                                med.Buttonntop = 0.2;
                            }
                        }
                        else
                        {
                            //Not Recorded
                            med.Buttonop = 0.2;
                            med.Buttonntop = 0.2;
                        }
                    }
                    else
                    {


                        med.ListBackgroundColor = Color.FromArgb("#f9f4e5");

                        //check feedback here
                        var getusersupp = AllUserSupplements.Where(x => x.id == med.Usermedid).FirstOrDefault();

                        var dtsupp = dateforschedule.ToString("dd/MM/yyyy");


                        // Check if feedback is null before trying to access it
                        if (getusersupp.feedback != null)
                        {
                            // Attempt to find the feedback item
                            var getfeedbackitemsupp = getusersupp.feedback
                                .Where(x => x.id == med.Feedbackid)
                                .Where(x => x.datetime.Contains(dtsupp))
                                .FirstOrDefault();

                            if (getfeedbackitemsupp != null)
                            {

                                if (getfeedbackitemsupp.Recorded == "Taken")
                                {
                                    //Medication Taken 
                                    med.Buttonop = 1;
                                    med.Buttonntop = 0.2;
                                }
                                else if(getfeedbackitemsupp.Recorded == "Not Taken")
                                {
                                    //Not Taken
                                    med.Buttonop = 0.2;
                                    med.Buttonntop = 1;
                                }
                                else
                                {
                                    med.Buttonop = 0.2;
                                    med.Buttonntop = 0.2;
                                }
                            }
                            else
                            {
                                //Not Recorded
                                med.Buttonop = 0.2;
                                med.Buttonntop = 0.2;
                            }
                        }
                        else
                        {
                            //Not Recorded
                            med.Buttonop = 0.2;
                            med.Buttonntop = 0.2;
                        }
                    }

                }
                }


            // Group by time and create the grouped collection
            //group the listview so all days are the same
            if (ScheduleList.Count == 0)
            {
                //No items Due for today 
                mainschedulelistview.IsVisible = false;
                nodatastack.IsVisible = true;
            }
            else
            {
                //populate mainschedulelistview 
                mainschedulelistview.IsVisible = true;
                nodatastack.IsVisible = false;

                var orderbytime = ScheduleList.OrderBy(x => TimeSpan.Parse(x.time)).ToList();

                foreach(var item in orderbytime)
                {
                    if(item.AsReqlblVis == true)
                    {
                        item.time = "As Required";
                    }
                }

                mainschedulelistview.ItemsSource = orderbytime;
                //   int totalItems = GroupedScheduleList.Sum(g => g.Count()) + GroupedScheduleList.Count; // Items + Group headers
                mainschedulelistview.HeightRequest = ScheduleList.Count * 110;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void populateAsRequired()
    {
        try
        {
            AsRequiredList.Clear(); 
            foreach (var item in AllUserMedications)
            {
                if (item.frequency.Contains("|"))
                {
                    var splitstring = item.frequency.Split('|');

                    if (splitstring[0] == "As Required")
                    {
                        var Medtime = new MedtimesDosages();
                        Medtime.Usermedid = item.id;
                        Medtime.Name = item.medicationtitle;
                        Medtime.Dosage = splitstring[1];
                        Medtime.dosageunit = item.unit;
                        Medtime.time = "As Required";
                        Medtime.Type = "Medication";
                        Medtime.ListBackgroundColor = Color.FromArgb("#e5f9f4");
                        AsRequiredList.Add(Medtime);
                    }
                }
                else 
                {
                    if (item.frequency == "As Required")
                    {
                        var Medtime = new MedtimesDosages();
                        Medtime.Usermedid = item.id;
                        Medtime.Name = item.medicationtitle;
                        Medtime.Dosage = "N/A";
                        Medtime.dosageunit = item.unit;
                        Medtime.time = "As Required";
                        Medtime.Type = "Medication";
                        Medtime.ListBackgroundColor = Color.FromArgb("#e5f9f4");
                        AsRequiredList.Add(Medtime);
                    }
                }
              
            }

            foreach (var item in AllUserSupplements)
            {
                if (item.frequency.Contains("|"))
                {
                    var splitstring = item.frequency.Split('|');

                    if (splitstring[0] == "As Required")
                    {
                        var Medtime = new MedtimesDosages();
                        Medtime.Usermedid = item.id;
                        Medtime.Name = item.supplementtitle;
                        Medtime.Dosage = splitstring[1];
                        Medtime.dosageunit = item.unit;
                        Medtime.time = "As Required";
                        Medtime.Type = "Supplement";
                        Medtime.ListBackgroundColor = Color.FromArgb("#f9f4e5");
                        AsRequiredList.Add(Medtime);
                    }
                }
                else
                {
                    if (item.frequency == "As Required")
                    {
                        var Medtime = new MedtimesDosages();
                        Medtime.Usermedid = item.id;
                        Medtime.Name = item.supplementtitle;
                        Medtime.Dosage = "N/A";
                        Medtime.dosageunit = item.unit;
                        Medtime.time = "As Required";
                        Medtime.Type = "Supplement";
                        Medtime.ListBackgroundColor = Color.FromArgb("#f9f4e5");
                        AsRequiredList.Add(Medtime);
                    }
                }

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //taken (tick) tapped on schedule

                // Get the tapped Image
                ExtendedImage label = (ExtendedImage)sender;

                var getitem = ScheduleList.Where(x => x.Feedbackid == label.FeedbackID).FirstOrDefault();

                if (getitem != null)
                {
                    MedsFeedbackRemove.Clear();

                    if (getitem.Type == "Medication")
                    {

                        var getusermeditem = AllUserMedications.Where(x => x.id == label.UsermedID).FirstOrDefault();

                        getitem.Buttonop = 1;

                        var newfeedback = new MedSuppFeedback();
                        newfeedback.id = label.FeedbackID;
                        newfeedback.Recorded = "Taken";
                        TimeSpan timeSpan = TimeSpan.Parse(getitem.time);
                        var dt = dateforschedule.Date + timeSpan;
                        newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");

                        if (getusermeditem.feedback == null || !getusermeditem.feedback.Any())
                        {
                            //feedback is null initalize before hand 
                            getusermeditem.feedback = new ObservableCollection<MedSuppFeedback>();
                        }
                        foreach(var item in getusermeditem.feedback)
                        {
                            if(item.id == newfeedback.id)
                            {
                                if (item.datetime == newfeedback.datetime)
                                {
                                    MedsFeedbackRemove.Add(item); 
                                }
                            }
                        }
                       
                        foreach(var item in MedsFeedbackRemove)
                        {
                            getusermeditem.feedback.Remove(item);
                        }

                        getusermeditem.feedback.Add(newfeedback);
                        await aPICalls.UpdateMedicationFeedbackAsync(getusermeditem);


                        //Update Feedback on SingleMedications Page
                        WeakReferenceMessenger.Default.Send(new UpdateShowAllMeds(getusermeditem));
                    }
                    else
                    {
                        //update supplement feedback
                        var getusermeditem = AllUserSupplements.Where(x => x.id == label.UsermedID).FirstOrDefault();


                        getitem.Buttonop = 1;

                        var newfeedback = new MedSuppFeedback();
                        newfeedback.id = label.FeedbackID;
                        newfeedback.Recorded = "Taken";
                        TimeSpan timeSpan = TimeSpan.Parse(getitem.time);
                        var dt = dateforschedule.Date + timeSpan;
                        newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");

                        if (getusermeditem.feedback == null || !getusermeditem.feedback.Any())
                        {
                            //feedback is null initalize before hand 
                            getusermeditem.feedback = new ObservableCollection<MedSuppFeedback>();
                        }

                        foreach (var item in getusermeditem.feedback)
                        {
                            if (item.id == newfeedback.id)
                            {
                                if (item.datetime == newfeedback.datetime)
                                {
                                    MedsFeedbackRemove.Add(item);
                                }
                            }
                        }

                        foreach (var item in MedsFeedbackRemove)
                        {
                            getusermeditem.feedback.Remove(item);
                        }

                        getusermeditem.feedback.Add(newfeedback);
                        await aPICalls.UpdateSupplementFeedbackAsync(getusermeditem);

                        //Update Feedback on SingleSupplements Page (selectedSupp) 
                        WeakReferenceMessenger.Default.Send(new UpdateShowAllSupps(getusermeditem));

                    }

                    //update Button On / Off View 
                    var Updateitem = ScheduleList.Where(x => x.Feedbackid == label.FeedbackID).FirstOrDefault();
                    Updateitem.Buttonntop = 0.2;

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

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //not taken (x) tapped on schedule

                // Get the tapped Image
                ExtendedImage label = (ExtendedImage)sender;

                var getitem = ScheduleList.Where(x => x.Feedbackid == label.FeedbackID).FirstOrDefault();
                if (getitem != null)
                {
                    MedsFeedbackRemove.Clear();

                    if (getitem.Type == "Medication")
                    {

                        var getusermeditem = AllUserMedications.Where(x => x.id == label.UsermedID).FirstOrDefault();

                        getitem.Buttonntop = 1;

                        var newfeedback = new MedSuppFeedback();
                        newfeedback.id = label.FeedbackID;
                        newfeedback.Recorded = "Not Taken";
                        TimeSpan timeSpan = TimeSpan.Parse(getitem.time);
                        var dt = dateforschedule.Date + timeSpan;
                        newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");

                        if (getusermeditem.feedback == null || !getusermeditem.feedback.Any())
                        {
                            //feedback is null initalize before hand 
                            getusermeditem.feedback = new ObservableCollection<MedSuppFeedback>();
                        }

                        foreach (var item in getusermeditem.feedback)
                        {
                            if (item.id == newfeedback.id)
                            {
                                if (item.datetime == newfeedback.datetime)
                                {
                                    MedsFeedbackRemove.Add(item);
                                }
                            }
                        }

                        foreach (var item in MedsFeedbackRemove)
                        {
                            getusermeditem.feedback.Remove(item);
                        }

                        getusermeditem.feedback.Add(newfeedback);
                        await aPICalls.UpdateMedicationFeedbackAsync(getusermeditem);

                        //Update Feedback on SingleMedications Page
                        WeakReferenceMessenger.Default.Send(new UpdateShowAllMeds(getusermeditem));

                    }
                    else
                    {
                        //update supplement feedback
                        var getusermeditem = AllUserSupplements.Where(x => x.id == label.UsermedID).FirstOrDefault();

                        getitem.Buttonntop = 1;

                        var newfeedback = new MedSuppFeedback();
                        newfeedback.id = label.FeedbackID;
                        newfeedback.Recorded = "Not Taken";
                        TimeSpan timeSpan = TimeSpan.Parse(getitem.time);
                        var dt = dateforschedule.Date + timeSpan;
                        newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");

                        if (getusermeditem.feedback == null || !getusermeditem.feedback.Any())
                        {
                            //feedback is null initalize before hand 
                            getusermeditem.feedback = new ObservableCollection<MedSuppFeedback>();
                        }

                        foreach (var item in getusermeditem.feedback)
                        {
                            if (item.id == newfeedback.id)
                            {
                                if (item.datetime == newfeedback.datetime)
                                {
                                    MedsFeedbackRemove.Add(item);
                                }
                            }
                        }

                        foreach (var item in MedsFeedbackRemove)
                        {
                            getusermeditem.feedback.Remove(item);
                        }

                        getusermeditem.feedback.Add(newfeedback);
                        await aPICalls.UpdateSupplementFeedbackAsync(getusermeditem);

                        //Update Feedback on SingleSupplements Page (selectedSupp) 
                        WeakReferenceMessenger.Default.Send(new UpdateShowAllSupps(getusermeditem));

                    }

                    //update Button On / Off View 
                    var Updateitem = ScheduleList.Where(x => x.Feedbackid == label.FeedbackID).FirstOrDefault();
                    Updateitem.Buttonop = 0.2;
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

    async private void asrequiredbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                asrequiredbtn.IsEnabled = false;
                populateAsRequired();
                if (asrequiredbtn.Text == "As Required Medications/Supplements")
                {
                    // asrequiredbtn.Text = "Medication/Supplement Schedule";
                    // Group by time and create the grouped collection
                    //group the listview so all days are the same
                    if (AsRequiredList.Count == 0)
                    {

                        await DisplayAlert("As Required", "No As Required Medications or Supplements are added, Try Adding Some to Access this Feature", "Close");
                        ////No items Due for today 
                        //mainschedulelistview.IsVisible = false;
                        //AsRequiredlistview.IsVisible = false; 
                        //nodatastack.IsVisible = true;
                    }
                    else
                    {

                        //populate mainschedulelistview 
                        asrequiredbtn.Text = "Medication/Supplement Schedule";
                        mainschedulelistview.IsVisible = false;
                        AsRequiredlistview.IsVisible = true;
                        nodatastack.IsVisible = false;
                        AsRequiredlistview.ItemsSource = AsRequiredList;
                        AsRequiredlistview.HeightRequest = AsRequiredList.Count * 100;
                    }
                }
                else
                {
                    asrequiredbtn.Text = "As Required Medications/Supplements";

                    mainschedulelistview.IsVisible = true;
                    AsRequiredlistview.IsVisible = false;
                }
                asrequiredbtn.IsEnabled = true;
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

    private async void AsRequiredlistview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var item = e.DataItem as MedtimesDosages;

                if (item.Type == "Medication")
                {
                    var findmed = AllUserMedications.Where(x => x.id == item.Usermedid).FirstOrDefault();
                    await Navigation.PushAsync(new AddAsRequiredDosage(item, findmed), false);
                }
                else
                {
                    var findsupp = AllUserSupplements.Where(x => x.id == item.Usermedid).FirstOrDefault();
                    await Navigation.PushAsync(new AddAsRequiredDosage(item, findsupp), false);
                }


                // await MopupService.Instance.PushAsync(new AddAsRequiredDosage(item) { });
                // await Task.Delay(1000);
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