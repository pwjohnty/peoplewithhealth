using Syncfusion.Maui.Charts;
using Syncfusion.Maui.DataSource;
using Syncfusion.Maui.DataSource.Extensions;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class MainSchedule : ContentPage
{
    List<DateTime> dateList = new List<DateTime>();
    public List<Schedule> changeddatesforlistview = new List<Schedule>();

    APICalls aPICalls = new APICalls();
    public ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    public ObservableCollection<MedtimesDosages> ScheduleList = new ObservableCollection<MedtimesDosages>();
    public ObservableCollection<MedtimesDosages> AsRequiredList = new ObservableCollection<MedtimesDosages>();
    public DateTime dateforschedule = new DateTime();
    public ObservableCollection<IGrouping<string, MedtimesDosages>> GroupedScheduleList { get; set; }

    public MainSchedule()
	{
		InitializeComponent();


        getusermeds();

        //get 7 days before and 7 days after todays date

        // Get today's date
        DateTime today = DateTime.Today;

        // Initialize a list to hold the dates
   

        // Add 7 days before today's date
        for (int i = -30; i <= 7; i++)
        {
            dateList.Add(today.AddDays(i));
        }


        foreach(var item in dateList)
        {
            var newitem = new Schedule();

            newitem.Day = item.Day.ToString();
            newitem.Date = item.Date.ToString("ddd");

            if(item.Date > DateTime.Now.Date)
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

    private void scheduledatelist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as Schedule;

            // Get the index of the tapped item from the ItemsSource
            var itemsSource = (sender as SfListView).ItemsSource as IList<Schedule>;
            int index = itemsSource.IndexOf(item);


            var dateforlabel = dateList[index];

            dateforschedule = dateList[index];

            datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");


            populateschedule();
        }
        catch(Exception ex)
        {

        }
    }

    async void getusermeds()
    {
        try
        {

            AllUserMedications = await aPICalls.GetUserMedicationsAsync();

            populateschedule();

           



        }
        catch(Exception ex)
        {

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
                            //check if today is within the days list
                            if (splitstring[1].Contains(dayname))
                            {
                                //add all the items 
                                foreach (var medtimes in item.schedule)
                                {
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
                            var dayname = dateforschedule.ToString("ddd");
                            //check if today is within the days list
                            if (splitstring[1].Contains(dayname))
                            {
                                //add all the items 
                                foreach (var medtimes in item.schedule)
                                {
                                    medtimes.Usermedid = item.id;
                                    medtimes.Feedbackid = medtimes.id.ToString();
                                    medtimes.Name = item.medicationtitle;
                                    ScheduleList.Add(medtimes);
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
             
                    //Dont Add 
                }

            }

            
                if(dateforschedule.Date > DateTime.Now.Date)
                {
                   foreach(var med in ScheduleList)
                    {
                        med.Buttonenabled = false;
                        med.Buttonop = 0;
                         med.Buttonntop = 0;
                        
                    }
                }
                else
                {
                    foreach (var med in ScheduleList)
                    {
                        med.Buttonenabled = true;
                        med.Buttonop = 0.2;
                        med.Buttonntop = 0.2;

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
                                med.Buttonntop = 0;
                            }
                            else
                            {
                                //Not Taken
                                med.Buttonop = 0;   
                                med.Buttonntop = 1; 
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
                mainschedulelistview.ItemsSource = ScheduleList;
                //   int totalItems = GroupedScheduleList.Sum(g => g.Count()) + GroupedScheduleList.Count; // Items + Group headers
                mainschedulelistview.HeightRequest = ScheduleList.Count * 100;
            }
        }
        catch(Exception ex)
        {

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
                        AsRequiredList.Add(Medtime);
                    }
                }
              
            }
        }
        catch(Exception Ex)
        {

        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //taken (tick) tapped on schedule

            // Get the tapped Image
            ExtendedImage label = (ExtendedImage)sender;

            var getitem = ScheduleList.Where(x => x.Feedbackid == label.FeedbackID).FirstOrDefault();

            var getusermeditem = AllUserMedications.Where(x => x.id == label.UsermedID).FirstOrDefault();



            if (getitem != null)
            {

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

                getusermeditem.feedback.Add(newfeedback);
                await aPICalls.UpdateMedicationFeedbackAsync(getusermeditem);
            }
        }
        catch(Exception ex )
        {

        }
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            //not taken (x) tapped on schedule

            // Get the tapped Image
            ExtendedImage label = (ExtendedImage)sender;

            var getitem = ScheduleList.Where(x => x.Feedbackid == label.FeedbackID).FirstOrDefault();

            var getusermeditem = AllUserMedications.Where(x => x.id == label.UsermedID).FirstOrDefault();



            if (getitem != null)
            {

                getitem.Buttonntop = 1;

                var newfeedback = new MedSuppFeedback();
                newfeedback.id = label.FeedbackID;
                newfeedback.Recorded = "Not Taken";
                TimeSpan timeSpan = TimeSpan.Parse(getitem.time);
                var dt = dateforschedule.Date + timeSpan;
                newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");


                getusermeditem.feedback.Add(newfeedback);


                await aPICalls.UpdateMedicationFeedbackAsync(getusermeditem);


            }


        }
        catch (Exception ex)
        {

        }
    }

    async private void asrequiredbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            populateAsRequired();
            if (asrequiredbtn.Text == "As Required Medications/Supplements")
            {
                asrequiredbtn.Text = "Medication/Supplement Schedule";
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

            }

           
        }
        catch (Exception Ex)
        {

        }
        
    }
}