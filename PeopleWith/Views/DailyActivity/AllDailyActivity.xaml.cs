//using System.Collections.ObjectModel;
//using Mopups.Services;
//using Newtonsoft.Json;
//using Syncfusion.Maui.ListView;

//namespace PeopleWith;

//public partial class AllDailyActivity : ContentPage
//{
//    public ObservableCollection<userdailyactivity> AllUserActivity = new ObservableCollection<userdailyactivity>();
//    public ObservableCollection<activefrequency> AllUserfrequency = new ObservableCollection<activefrequency>();
//    public ObservableCollection<activefrequency> SelectedDayFrequency = new ObservableCollection<activefrequency>();
//    public ObservableCollection<ActivityPlanner> ScheduleItems { get; set; } = new ObservableCollection<ActivityPlanner>();
//    public bool Selecteddatechange = false; 
//    List<DateTime> dateList = new List<DateTime>();
//    public DateTime dateforschedule = new DateTime();
//    public APICalls database = new APICalls();
//    public List<Schedule> changeddatesforlistview = new List<Schedule>();
//    public event EventHandler<bool> ConnectivityChanged;
//    //Crash Handler
//    CrashDetected crashHandler = new CrashDetected();

//    async public void NotasyncMethod(Exception Ex)
//    {
//        try
//        {
//            await crashHandler.SentryCrashDetected(Ex);
//            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
//        }
//        catch (Exception ex)
//        {
//            //Dunno 
//        }
//    }
//    public AllDailyActivity()
//    {
//        try
//        {

//            InitializeComponent();
//            ActivityLoading.IsVisible = true;
//            GetActivityInfo();

//            //get 7 days before and 7 days after todays date

//            // Get today's date
//            DateTime today = DateTime.Today;

//            // Initialize a list to hold the dates
//            // Add 7 days before today's date
//            for (int i = -30; i <= 3; i++)
//            {
//                dateList.Add(today.AddDays(i));
//            }

//            foreach (var item in dateList)
//            {
//                var newitem = new Schedule();

//                newitem.Day = item.Day.ToString();
//                newitem.Date = item.Date.ToString("ddd");

//                if (item.Date > DateTime.Now.Date)
//                {
//                    newitem.Bgcolour = Colors.Transparent;
//                    newitem.Bordercolour = Color.FromArgb("#fce9d9");
//                    newitem.Op = 0.5;
//                }
//                else
//                {
//                    newitem.Bgcolour = Color.FromArgb("#fce9d9");
//                    newitem.Bordercolour = Colors.Transparent;
//                    newitem.Op = 1;
//                }

//                changeddatesforlistview.Add(newitem);
//            }

//            ActivityDates.ItemsSource = changeddatesforlistview;

//            // Find today's date in the list

//            var indexForToday = dateList.IndexOf(today);

//            // Check if today's date is in the list
//            if (indexForToday >= 0)
//            {
//                Selecteddatechange = true; 
//                // Set the selected item to today's date
//                ActivityDates.SelectedItem = changeddatesforlistview[indexForToday];

//                var dateforlabel = dateList[indexForToday];

//                datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");
//                dateforschedule = dateList[indexForToday];

//                // Scroll to today's date and try to center it
//                ActivityDates.ScrollTo(changeddatesforlistview[indexForToday], ScrollToPosition.Center, true);
//            }

//            ActivityLoading.IsVisible = false;
//        }
//        catch (Exception Ex)
//        {
//            ActivityLoading.IsVisible = false;
//            NotasyncMethod(Ex);
//        }
//    }


//    private async void GetActivityInfo()
//    {
//        try
//        {
//            //Get User Activity Here
//            APICalls database = new APICalls();

//            var GetActivityTask = database.GetUserActivityAsync();
//            AllUserActivity = await GetActivityTask;

//            WorkoutItemsDue();

//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private async void WorkoutItemsDue()
//    {
//        try
//        {

//            // Clear the existing schedule items
//            ScheduleItems.Clear();

//            // Use dateforschedule as the selected date for populating the schedule
//            DateTime selectedDate = dateforschedule;

//            foreach (var item in AllUserActivity)
//            {
//                var startDate = DateTime.Parse(item.startdate);
//                DateTime? endDate = string.IsNullOrEmpty(item.enddate) ? (DateTime?)null : DateTime.Parse(item.enddate);

//                item.name = item.activitytitle;
//                if (item.noteslist != null && item.noteslist.Count > 0)
//                {
//                    item.displayname = item.noteslist[0].displayname;
//                    item.displaynameAdded = !string.IsNullOrEmpty(item.displayname);
//                }
//                else
//                {
//                    item.displayname = string.Empty; 
//                    item.displaynameAdded = false;
//                }


//                foreach (var freq in item.activityfrequencylist)
//                {
//                    //One off 
//                    var Newitem = new ActivityPlanner();
//                    if (freq.frequency == "One Off")
//                    {
//                        if (selectedDate.Date == startDate.Date)
//                        {
//                            //Only Add the One Instance (I.E Start Date)  
//                            AddScheduleItem(item, freq);
//                        }
//                    }

//                    //Daily 
//                    else if (freq.frequency == "Daily")
//                    {
//                        if (endDate.HasValue && selectedDate.Date <= endDate.Value.Date)
//                        {
//                            // Has endDate, Selected Date before or equals it, add it
//                            AddScheduleItem(item, freq);
//                        }
//                        // If no endDate (ongoing activity), add it
//                        else if (!endDate.HasValue)
//                        {
//                            AddScheduleItem(item, freq);
//                        }
//                    }

//                    // Weekly
//                    else if (freq.frequency == "Weekly")
//                    {
//                        if (endDate.HasValue && selectedDate.Date <= endDate.Value.Date || !endDate.HasValue) // Ongoing activity
//                        {
//                            // Check if selectedDate matches the correct day of the week
//                            if (selectedDate.Date >= startDate.Date && selectedDate.DayOfWeek == ParseDay(freq.day))
//                            {
//                                AddScheduleItem(item, freq);
//                            }
//                        }
//                    }

//                    // Monthly
//                    else if (freq.frequency == "Monthly")
//                    {
//                        if (endDate.HasValue && selectedDate.Date <= endDate.Value.Date ||
//                            !endDate.HasValue) // Ongoing activity
//                        {
//                            if (selectedDate.Day == startDate.Day) // Same day of the month
//                            {
//                                AddScheduleItem(item, freq);
//                            }
//                        }
//                    }

//                    // Quarterly (Every 3 Months)
//                    else if (freq.frequency == "Quarterly")
//                    {
//                        if (endDate.HasValue && selectedDate.Date <= endDate.Value.Date ||
//                            !endDate.HasValue) // Ongoing activity
//                        {
//                            int monthsDiff = ((selectedDate.Year - startDate.Year) * 12) + (selectedDate.Month - startDate.Month);
//                            if (monthsDiff % 3 == 0 && selectedDate.Day == startDate.Day) // Every 3 months on the same day
//                            {
//                                AddScheduleItem(item, freq);
//                            }
//                        }
//                    }

//                    // Yearly
//                    else if (freq.frequency == "Yearly")
//                    {
//                        if (endDate.HasValue && selectedDate.Date <= endDate.Value.Date ||
//                            !endDate.HasValue) // Ongoing activity
//                        {
//                            if (selectedDate.Month == startDate.Month && selectedDate.Day == startDate.Day) // Same month & day
//                            {
//                                AddScheduleItem(item, freq);
//                            }
//                        }
//                    }
//                }
//            }

//            //Set Schedule items 
//            if (ScheduleItems.Count > 0)
//            {
//                ActivityPlannerStack.IsVisible = true;
//                EmptyStack.IsVisible = false;
//                ActivityPlanner.ItemsSource = ScheduleItems;
//                AddTaskStack.IsVisible = true; 
//            }
//            else
//            {
//                ActivityPlannerStack.IsVisible = false;
//                EmptyStack.IsVisible = true;
//                AddTaskStack.IsVisible = false;
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private void AddScheduleItem(userdailyactivity item, activefrequency freq)
//    {
//        try
//        {
//            var newItem = new ActivityPlanner
//            {
//                activityid = item.id,
//                frequencyid = freq.id,
//                frequency = freq.frequency,
//                startdate = item.startdate,
//                enddate = item?.enddate,
//                time = freq.time,
//                Name = item.activitytitle,
//                //here for now 
//                Check = false,
//                Strike = TextDecorations.None,
//                Showcompleted = false,
//                Showfrequency = true,
//                FeedbackAdded = false
//            };

//            //Check if feedback item exists 
//            if (item.activityfeedbacklist != null)
//            {
//                // Attempt to find the feedback item
//                var getfeedbackitem = item.activityfeedbacklist
//                    .Where(x => x.id == freq.id)
//                    .Where(p =>
//                        DateTime.Parse(p.Scheduledatetime).Date == dateforschedule.Date &&
//                        DateTime.Parse(p.Scheduledatetime).TimeOfDay == TimeSpan.Parse(newItem.time))
//                    .FirstOrDefault();
//                if (getfeedbackitem != null)
//                {
//                    newItem.Strike = TextDecorations.Strikethrough;
//                    newItem.Check = true;
//                    newItem.Showcompleted = true;
//                    newItem.Showfrequency = false;
//                    newItem.Datetimerecorded = getfeedbackitem.datetimerecorded;
//                    newItem.FeedbackAdded = true;
//                }
//            }

//            if (freq.frequency == "One Off")
//            {
//                newItem.frequencystring = freq.frequency;
//            }
//            else
//            {
//                if (freq.frequency == "Daily")
//                {
//                    newItem.frequencystring = "Every Day starting from " + item.startdate;
//                }
//                else if (freq.frequency == "Weekly")
//                {
//                    newItem.frequencystring = "Every Week starting from " + item.startdate;
//                }
//                else if (freq.frequency == "Monthly")
//                {
//                    newItem.frequencystring = "Every Month starting from " + item.startdate;
//                }
//                else if (freq.frequency == "Quarterly")
//                {
//                    newItem.frequencystring = "Every 3 Month starting from " + item.startdate;
//                }
//                else if (freq.frequency == "Yearly")
//                {
//                    newItem.frequencystring = "Every Year starting from " + item.startdate;
//                }
//            }

//            if (string.IsNullOrEmpty(freq.duration))
//            {
//                newItem.duration = freq.time;
//            }
//            else
//            {
//                newItem.duration = freq.duration;
//            }
//            //Set day for weekly 
//            newItem.day = !string.IsNullOrEmpty(freq.day) ? freq.day : null;


//            //Set Checkbox Visibiliy based on time 
//            newItem.IsCheckBoxVisible = dateforschedule.Date <= DateTime.Now.Date;

//            // Ensure noteslist exists and has at least one item
//            if (item.noteslist != null && item.noteslist.Count > 0)
//            {
//                var firstNote = item.noteslist[0];

//                newItem.displayname = !string.IsNullOrEmpty(firstNote.displayname) ? firstNote.displayname : null;
//                newItem.notes = !string.IsNullOrEmpty(firstNote.notes) ? firstNote.notes : null;
//            }
//            else
//            {
//                //Leave Empty
//            }

//            // Add to schedule
//            ScheduleItems.Add(newItem);
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }


//    DayOfWeek ParseDay(string day)
//    {
//        return day switch
//        {
//            "Mon" => DayOfWeek.Monday,
//            "Tue" => DayOfWeek.Tuesday,
//            "Wed" => DayOfWeek.Wednesday,
//            "Thu" => DayOfWeek.Thursday,
//            "Fri" => DayOfWeek.Friday,
//            "Sat" => DayOfWeek.Saturday,
//            "Sun" => DayOfWeek.Sunday,
//            _ => throw new ArgumentException("Invalid day format")
//        };
//    }

//    private void UpdateDates(string DateSelected)
//    {
//        try
//        {
//            // Ensure input date is valid
//            if (!DateTime.TryParse(DateSelected, out DateTime selectedDate))
//            {
//                Console.WriteLine("Invalid date format");
//                return;
//            }

//            dateList.Clear();
//            changeddatesforlistview.Clear();

//            // Populate the date list from -30 to +30 days relative to selectedDate
//            for (int i = -30; i <= 30; i++)
//            {
//                dateList.Add(selectedDate.AddDays(i));
//            }

//            foreach (var item in dateList)
//            {
//                var newitem = new Schedule
//                {
//                    Day = item.Day.ToString(),
//                    Date = item.ToString("ddd"),
//                    Bgcolour = (item.Date > DateTime.Now.Date) ? Colors.Transparent : Color.FromArgb("#fce9d9"),
//                    Bordercolour = (item.Date > DateTime.Now.Date) ? Color.FromArgb("#fce9d9") : Colors.Transparent,
//                    Op = (item.Date > DateTime.Now.Date) ? 0.5 : 1
//                };

//                changeddatesforlistview.Add(newitem);
//            }

//            // Update UI
//            ActivityDates.ItemsSource = null; // Reset to force refresh
//            ActivityDates.ItemsSource = changeddatesforlistview;

//            // Find index of selected date
//            var indexForSelected = dateList.IndexOf(selectedDate);

//            if (indexForSelected >= 0)
//            {
//                Selecteddatechange = true;
//                ActivityDates.SelectedItem = changeddatesforlistview[indexForSelected];

//                datelbl.Text = selectedDate.ToString("dddd, dd MMMM");
//                dateforschedule = selectedDate;

//                // Scroll to selected date
//                ActivityDates.ScrollTo(changeddatesforlistview[indexForSelected], ScrollToPosition.Center, true);
//            }

//            WorkoutItemsDue();
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }


//    private async void AddBtn_Clicked(object sender, EventArgs e)
//    {
//        try
//        {
//            await MopupService.Instance.PushAsync(new AddActivity());
//            //AddBtn.IsEnabled = false;
//            //await Navigation.PushAsync(new AddDailyActivity(), false);
//            //AddBtn.IsEnabled = true;
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
//    {
//        try
//        {
//            await MopupService.Instance.PushAsync(new Infopopup("activity") { });
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private void ActivityDates_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
//    {
//        try
//        {

//            var item = e.DataItem as Schedule;

//            var itemsSource = (sender as SfListView).ItemsSource as IList<Schedule>;
//            int index = itemsSource.IndexOf(item);


//            var dateforlabel = dateList[index];

//            dateforschedule = dateList[index];

//            datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");

//            WorkoutItemsDue();
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
//    {
//        try
//        {
//            // Create TaskCompletionSource to get the result
//            var tcs = new TaskCompletionSource<string>();

//            // Open the popup and pass TaskCompletionSource
//            await MopupService.Instance.PushAsync(new ActivityCalendar(tcs));

//            // Wait for the result
//            string selectedDate = await tcs.Task;

//            if (!string.IsNullOrEmpty(selectedDate))
//            {
//                //Update Dates
//                UpdateDates(selectedDate);
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
//    {
//        try
//        {
//            //Connectivity Changed 
//            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
//            if (accessType == NetworkAccess.Internet)
//            {

//                if (sender is CheckBox checkBox && checkBox.BindingContext is ActivityPlanner selectedItem)
//                {

//                    var SelectedActivity = AllUserActivity.FirstOrDefault(item => item.activityfrequencylist.Any(x => x.id == selectedItem.frequencyid));

//                    //var listviewItem = ScheduleItems.Where(x => x.frequencyid == selectedItem.frequencyid).FirstOrDefault();
//                    if (checkBox.IsChecked == true)
//                    {

//                        //Add New Feedback When Checked = True 
//                        selectedItem.Strike = TextDecorations.Strikethrough;
//                        selectedItem.Check = true;
//                        selectedItem.Showcompleted = true;
//                        selectedItem.Showfrequency = false;
//                        if (selectedItem.FeedbackAdded == false)
//                        {
//                            selectedItem.Datetimerecorded = DateTime.Now.ToString("HH:mm, dd/MM/yyyy");
//                        }

//                        //Update Item in DB as recorded 

//                        var newfeedback = new ActivityFeedback();
//                        newfeedback.id = selectedItem.frequencyid;
//                        newfeedback.Recorded = "Taken";
//                        newfeedback.frequency = selectedItem.frequency;
//                        newfeedback.Scheduledatetime = selectedItem.time + ", " + dateforschedule.ToString("dd/MM/yyyy");                      
//                        newfeedback.day = selectedItem.day;
//                        newfeedback.name = selectedItem.Name;
//                        newfeedback.displayname = selectedItem?.displayname;
//                        newfeedback.datetimerecorded = DateTime.Now.ToString("HH:mm, dd/MM/yyyy");


//                        //Feedback 
//                        if (SelectedActivity.activityfeedbacklist == null)
//                        {
//                            SelectedActivity.activityfeedbacklist = new ObservableCollection<ActivityFeedback>();
//                        }

//                        //Used to Stop repeat Data When Setting Checkbox to checked 
//                        if(selectedItem.FeedbackAdded == false)
//                        {
//                            selectedItem.FeedbackAdded = true;
//                            SelectedActivity.activityfeedbacklist.Add(newfeedback);
//                            SelectedActivity.feedback = JsonConvert.SerializeObject(SelectedActivity.activityfeedbacklist);
//                            await database.UpdateActivityFeedbackAsync(SelectedActivity);

//                        }

//                    }
//                    else
//                    {

//                        //Remove Feedback When Checked = False 
//                        selectedItem.Strike = TextDecorations.None;
//                        selectedItem.Check = false;
//                        selectedItem.Showcompleted = false;
//                        selectedItem.Showfrequency = true;
//                        selectedItem.FeedbackAdded = false; 

//                        //Feedback just a precaution 
//                        if (SelectedActivity.activityfeedbacklist == null)
//                        {
//                            SelectedActivity.activityfeedbacklist = new ObservableCollection<ActivityFeedback>();
//                        }

//                        var activityfeedback = new ActivityFeedback();

//                        foreach (var item in SelectedActivity.activityfeedbacklist)
//                        {
//                            if (item.id == selectedItem.frequencyid)
//                            {
//                                if (item.datetimerecorded == selectedItem.Datetimerecorded)
//                                {
//                                    activityfeedback = item;
//                                }
//                            }
//                        }

//                        if(activityfeedback.id != null)
//                        {
//                            SelectedActivity.activityfeedbacklist.Remove(activityfeedback);

//                            if (SelectedActivity.activityfeedbacklist.Count == 0)
//                            {
//                                SelectedActivity.feedback = null;
//                            }
//                            else
//                            {
//                                SelectedActivity.feedback = JsonConvert.SerializeObject(SelectedActivity.activityfeedbacklist);
//                            }

//                            await database.UpdateActivityFeedbackAsync(SelectedActivity);
//                        }   
//                    }
//                }

//            }
//            else
//            {
//                var isConnected = accessType == NetworkAccess.Internet;
//                ConnectivityChanged?.Invoke(this, isConnected);
//            }
          
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//   }

//    //private async void ActivityPlanner_SelectionChanged(object sender, SelectionChangedEventArgs e)
//    //{
//    //    try
//    //    {
//    //        var selectedItem = e.CurrentSelection.FirstOrDefault() as ActivityPlanner;
//    //        if (selectedItem == null)  return;

//    //        var selectedActivity = AllUserActivity.FirstOrDefault(item =>
//    //                   item.activityfrequencylist.Any(x => x.id == selectedItem.frequencyid));


//    //        await Navigation.PushAsync(new SingleDailyActivity(selectedItem, selectedActivity), false);

//    //        if (sender is CollectionView collectionView)
//    //        {
//    //            collectionView.SelectedItem = null;
//    //        }

//    //        //ActivityPlanner.SelectedItem = null;
//    //    }
//    //    catch (Exception Ex)
//    //    {
//    //        NotasyncMethod(Ex);
//    //    }
//    //}

//    private void Listviewbtn_Clicked(object sender, EventArgs e)
//    {
//        try
//        {
//            PlannerStack.IsVisible = false;
//            ListviewStack.IsVisible = true;

//            ActivityListview.ItemsSource = AllUserActivity;

//            if(AllUserActivity.Count == 0)
//            {
//                ActivityListview.IsVisible = false;
//                EmptyStack.IsVisible = true; 
//            }
//            else
//            {
//                ActivityListview.IsVisible = true;
//                EmptyStack.IsVisible = false;
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private void todaybtn_Clicked(object sender, EventArgs e)
//    {
//        try
//        {
//            //Stops Repeat Clicking when Already Showing Today's date
//            if(dateforschedule.Date != DateTime.Now.Date)
//            {
//                string selectedDate = DateTime.Now.ToString("dd/MM/yyyy");
//                UpdateDates(selectedDate);
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private void timelinebtn_Clicked(object sender, EventArgs e)
//    {
//        try
//        {
//            PlannerStack.IsVisible = true;
//            ListviewStack.IsVisible = false;

//            //Set Schedule items 
//            if (ScheduleItems.Count > 0)
//            {
//                ActivityPlannerStack.IsVisible = true;
//                EmptyStack.IsVisible = false;
//                ActivityPlanner.ItemsSource = ScheduleItems;
//                AddTaskStack.IsVisible = true;
//            }
//            else
//            {
//                ActivityPlannerStack.IsVisible = false;
//                EmptyStack.IsVisible = true;
//                AddTaskStack.IsVisible = false;
//            }
//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    private async void ActivityListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
//    {
//        try
//        {
//            var selectedActivity = e.DataItem as userdailyactivity;
//            if (selectedActivity == null)  return;

//            //var selectplanner = ScheduleItems.FirstOrDefault(x => x.activityid == selectedActivity.id);

//            await Navigation.PushAsync(new SingleDailyActivity(selectedActivity), false);

//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }

//    public async void ActivityPlanner_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
//    {
//        try
//        {
//            var selectedItem = e.DataItem as ActivityPlanner;
//            if (selectedItem == null) return;

//            var selectedActivity = AllUserActivity.FirstOrDefault(item => item.activityfrequencylist.Any(x => x.id == selectedItem.frequencyid));

//            await Navigation.PushAsync(new SingleDailyActivity(selectedActivity), false);

//        }
//        catch (Exception Ex)
//        {
//            NotasyncMethod(Ex);
//        }
//    }
//}