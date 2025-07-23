using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Messaging;
using Maui.FreakyControls.Extensions;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;


namespace PeopleWith;

public partial class ShowAllSupplement : ContentPage
{
    usersupplement MedSelected = new usersupplement();
    ObservableCollection<usersupplement> MedicationList = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> TakenMedicationList = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> NotTakenMedicationList = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> NotRecordedMedicationList = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> MedicationNotRecordedList = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> UpdateFilterDates = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> FilteredMedList = new ObservableCollection<usersupplement>();
    public ObservableCollection<TimelineItem> TimelineItems { get; set; } = new ObservableCollection<TimelineItem>();
    public string SelectedFrame = String.Empty;
    Color SetColour;
    private const int PageSize = 20;
    private int currentPage = 0;
    public ICommand LoadMoreCommand { get; }
    public bool IsLazyLoading { get; set; }
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

    public class TimelineItem
    {
        public string DisplayText { get; set; }
        public string DateRange { get; set; }
        public DateTime Order { get; set; }
        public string type { get; set; }
    }

    public ShowAllSupplement(usersupplement SelectedMed)
    {
        try
        {
            InitializeComponent();
            MedSelected = SelectedMed;
            MedicationName.Text = MedSelected.supplementtitle;

            PopulateListView();

            UpdateFilterDates = MedicationList;
            CalculateTimeLine();

            var Freq = MedSelected.frequency.Split('|');
            //As Required Medication 
            if (Freq[0] == "As Required")
            {

            }
            else
            {
                if (MedicationList.Count == 0)
                {
                    nodatastack.IsVisible = true;
                    datastack.IsVisible = false;
                }

            }

            //Update Page From Schedule 
            WeakReferenceMessenger.Default.Register<UpdateShowAllSupps>(this, (r, m) =>
            {
                var CheckCurrent = (usersupplement)m.Value;
                var Feedback = new ObservableCollection<MedSuppFeedback>();
                Feedback = CheckCurrent.feedback;

                //Checks the page Navigated From is the same thats being updated (Otherwise not needed)
                if (MedSelected.id == CheckCurrent.id)
                {
                    //Check if Schedule for DateTime Exists
                    MedSelected.feedback.Clear();
                    foreach (var item in Feedback)
                    {
                        MedSelected.feedback.Add(item);
                    }
                    PopulateListView();
                    UpdateFilterDates = MedicationList;
                    CalculateTimeLine();

                    var Freq = MedSelected.frequency.Split('|');
                    //As Required Medication 
                    if (Freq[0] == "As Required")
                    {

                    }
                    else
                    {
                        if (MedicationList.Count == 0)
                        {
                            nodatastack.IsVisible = true;
                            datastack.IsVisible = false;
                        }

                    }
                }

            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //Calculate TimelineListview 
    private void CalculateTimeLine()
    {
        try
        {
            var typestring = string.Empty;
            TimelineItems.Clear();
            if (UpdateFilterDates.Count == 0)
            {
                return;
            }
            else
            {
                var Max = UpdateFilterDates.Max(x => DateTime.Parse(x.MedDateTime));
                var Min = UpdateFilterDates.Min(x => DateTime.Parse(x.MedDateTime));
                var Totaldays = (Max.Date - Min.Date).TotalDays;


                if (Totaldays <= 7)
                {
                    typestring = "Day";
                    FilterTimeLine.IsVisible = false;
                    foreach (var date in UpdateFilterDates.OrderBy(d => DateTime.Parse(d.MedDateTime)))
                    {
                        var DateTimes = DateTime.Parse(date.MedDateTime);
                        TimelineItems.Add(new TimelineItem
                        {
                            DisplayText = DateTimes.ToString("dd/MM/yy"),
                            DateRange = DateTimes.ToString("dd/MM/yy"),
                            Order = DateTimes,
                            type = "Day"
                        });
                    }
                }
                else if (Totaldays > 7 && Totaldays < 30)
                {
                    typestring = "Week";
                    FilterTimeLine.IsVisible = true;
                    var groupedByWeek = UpdateFilterDates
    .OrderBy(d => DateTime.Parse(d.MedDateTime))
    .GroupBy(d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Parse(d.MedDateTime), CalendarWeekRule.FirstDay, DayOfWeek.Monday));

                    foreach (var group in groupedByWeek)
                    {
                        var firstDate = group.First();
                        var lastDate = group.Last();
                        var Datefirst = DateTime.Parse(firstDate.MedDateTime);
                        var Datelast = DateTime.Parse(lastDate.MedDateTime);
                        TimelineItems.Add(new TimelineItem
                        {
                            DisplayText = $"Week of " + Datefirst.ToString("dd MMM yy"),
                            DateRange = Datefirst.ToString("dd/MM/yy") + "|" + Datelast.ToString("dd/MM/yy"),
                            Order = Datefirst,
                            type = "Week"
                        });
                    }

                }
                else
                {

                    typestring = "Month";
                    FilterTimeLine.IsVisible = true;

                    // More than 30 days: Show months
                    var groupedByMonth = UpdateFilterDates
      .OrderBy(d => DateTime.Parse(d.MedDateTime))
      .GroupBy(d => new { Year = DateTime.Parse(d.MedDateTime).Year, Month = DateTime.Parse(d.MedDateTime).Month });


                    foreach (var group in groupedByMonth)
                    {
                        var firstDate = group.First();
                        var Datefirst = DateTime.Parse(firstDate.MedDateTime);
                        TimelineItems.Add(new TimelineItem
                        {
                            DisplayText = Datefirst.ToString("MMMM yyyy"),
                            DateRange = $"{Datefirst:MMMM yyyy}",
                            Order = Datefirst,
                            type = "Month"
                        });
                    }
                }



                FilterTimeLine.ItemsSource = TimelineItems.OrderByDescending(d => d.Order);
                var GetTimelineFirst = TimelineItems.OrderByDescending(d => d.Order).First().Order;
                FilteredMedList.Clear();
                //Set items Source on load 
                if (typestring == "Day")
                {
                    FilteredMedList = UpdateFilterDates.OrderByDescending(d => DateTime.Parse(d.MedDateTime)).ToObservable();
                }
                else if (typestring == "Week")
                {
                    var GetinitalDateRange = TimelineItems.OrderByDescending(d => d.Order).First().DateRange;
                    var splititem = GetinitalDateRange.Split('|');
                    var InitalDate = splititem[0];
                    var LastDate = splititem[1];
                    var StartDate = DateTime.Parse(InitalDate);
                    var EndDate = DateTime.Parse(LastDate);
                    var filteredMedList = UpdateFilterDates
          .Where(x =>
          {
              var medDate = DateTime.Parse(x.MedDateTime).Date;
              return medDate >= StartDate.Date && medDate <= EndDate.Date;
          })
          .ToObservable();

                    FilteredMedList = filteredMedList;
                    FilterTimeLine.SelectedItem = TimelineItems.OrderByDescending(d => d.Order).FirstOrDefault();
                }
                else if (typestring == "Month")
                {

                    FilteredMedList = UpdateFilterDates.Where(x => DateTime.Parse(x.MedDateTime).ToString("MMMM yyyy") == GetTimelineFirst.ToString("MMMM yyyy")).ToObservable();
                    FilterTimeLine.SelectedItem = TimelineItems.FirstOrDefault(item => item.DisplayText == GetTimelineFirst.ToString("MMMM yyyy"));
                    FilteredMedList = new ObservableCollection<usersupplement>(FilteredMedList.OrderByDescending(d => DateTime.Parse(d.MedDateTime)));

                }
                UserMedicationSchedule.ItemsSource = FilteredMedList;
                UserMedicationSchedule.HeightRequest = FilteredMedList.Count * 120;


            }


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    //Incrementally Load ListviewData

    //private void LoadInitialData()
    //{
    //    LoadBatch(0, PageSize);
    //}

    //private async Task LoadMoreItems()
    //{
    //    if (IsLazyLoading) return;

    //    IsLazyLoading = true;

    //    try
    //    {
    //        await Task.Delay(300); // Simulate a delay for smoother UI

    //        // Calculate the next batch to load
    //        int startIndex = currentPage * PageSize;
    //        if (startIndex < MedicationList.Count)
    //        {
    //            LoadBatch(startIndex, PageSize);
    //            currentPage++;
    //        }
    //    }
    //    finally
    //    {
    //        IsLazyLoading = false;
    //    }
    //}


    //private void LoadBatch(int startIndex, int batchSize)
    //{
    //    // Add items from MedicationList to PagedMedicationList
    //    int endIndex = Math.Min(startIndex + batchSize, MedicationList.Count);
    //    for (int i = startIndex; i < endIndex; i++)
    //    {
    //        PagedMedicationList.Add(MedicationList[i]);
    //    }
    //    UserMedicationSchedule.ItemsSource = PagedMedicationList;
    //}


    async private void PopulateListView()
    {
        try
        {
            if (MedSelected.frequency.Contains("|"))
            {
                var Freq = MedSelected.frequency.Split('|');

                //As Required Medication 
                if (Freq[0] == "As Required")
                {
                    //Only Populate With Feedback items 
                    if (MedSelected.feedback == null)
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;

                    }
                    else
                    {
                        nodatastack.IsVisible = false;
                        datastack.IsVisible = true;
                        NotTakenFrame.Background = Colors.White;
                        NotTakenFrame.BorderColor = Colors.White;
                        NotTakenFrame.Opacity = 1;
                        NotTakenFrame.IsEnabled = false;
                        NotRecordedFrame.Background = Colors.White;
                        NotRecordedFrame.BorderColor = Colors.White;
                        NotRecordedFrame.Opacity = 1;
                        NotRecordedFrame.IsEnabled = false;


                        foreach (var item in MedSelected.feedback)
                        {

                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");

                            var NewMed = new usersupplement();
                            NewMed.MedDateTime = RecordTime;
                            NewMed.Colour = Colors.LightGreen;
                            NewMed.Action = "Taken";
                            NewMed.id = item.id;
                            NewMed.Dosage = item.Recorded;
                            MedicationList.Add(NewMed);

                        }

                        foreach (var item in MedicationList)
                        {
                            item.unit = MedSelected.unit;
                        }

                        foreach (var item in MedicationList)
                        {

                            if (item.Dosage.Contains("|"))
                            {
                                var DosageSplit = item.Dosage.Split('|');
                                item.DosageOne = DosageSplit[0];
                                item.DosageTwo = DosageSplit[1];

                                var UnitSplit = item.unit.Split(' ');
                                item.UnitOne = UnitSplit[0] + " " + UnitSplit[1];
                                item.UnitTwo = UnitSplit[2];

                                item.DoubleDosage = true;
                                item.SingleDosage = false;
                            }
                            else
                            {
                                item.DoubleDosage = false;
                                item.SingleDosage = true;
                            }
                        }


                        // var sortedlist = MedicationList.OrderBy(t => t.MedDateTime);
                        //UserMedicationSchedule.ItemsSource = sortedlist;
                        //Filtered List 
                        TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();

                    }
                }
                else if (Freq[0] == "Daily")
                {
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;

                    DateTime? enddate = null;
                    if (!string.IsNullOrEmpty(MedSelected.enddate))
                    {
                        enddate = DateTime.Parse(MedSelected.enddate);
                    }

                    if (MedSelected.feedback == null)
                    {

                    }
                    else
                    {

                        foreach (var item in MedSelected.feedback)
                        {

                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            var CurrentTime = DateTime.Parse(item.datetime);
                            if (enddate == null || CurrentTime <= enddate.Value)
                            {
                                if (item.Recorded == "Taken")
                                {
                                    SetColour = Colors.LightGreen;
                                }
                                else
                                {
                                    SetColour = Color.FromArgb("#ff6666");
                                }
                                var NewMed = new usersupplement();
                                NewMed.MedDateTime = RecordTime;
                                NewMed.Colour = SetColour;
                                NewMed.Action = item.Recorded;
                                NewMed.id = item.id;
                                MedicationList.Add(NewMed);

                            }
                        }
                    }
                    foreach (var item in MedicationList)
                    {
                        foreach (var x in MedSelected.schedule)
                        {
                            if (item.id == x.id.ToString())
                            {
                                item.Dosage = x.Dosage;
                                item.unit = x.dosageunit;
                            }
                        }
                    }

                    var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;

                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(1))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);

                            if (enddate != null && scheduleDateTime > enddate.Value)
                            {
                                continue;
                            }

                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }

                            var newItem = new usersupplement
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit,
                                Colour = Colors.LightGray,
                                Action = "Not Recorded",
                            };

                            MedicationNotRecordedList.Add(newItem);
                        }
                    }

                    foreach (var item in MedicationNotRecordedList)
                    {
                        if (!MedicationList.Any(m => m.MedDateTime == item.MedDateTime))
                        {
                            MedicationList.Add(item);
                        }
                    }

                    // Preprocess MedicationList
                    foreach (var medication in MedicationList)
                    {
                        if (DateTime.TryParse(medication.MedDateTime, out var parsedDate))
                        {
                            medication.DatetimeOrder = parsedDate;
                        }
                    }

                    //Attempt to Iterate through the List Faster. Not Fast Enough.
                    //var categorizedList = MedicationList
                    //    .OrderByDescending(m => m.DatetimeOrder)
                    //    .GroupBy(m => m.Action?.ToLowerInvariant())
                    //    .ToDictionary(g => g.Key, g => g.ToList());

                    //if (categorizedList.TryGetValue("taken", out var takenList))
                    //    TakenMedicationList = takenList.ToObservableCollection();

                    //if (categorizedList.TryGetValue("not taken", out var notTakenList))
                    //    NotTakenMedicationList = notTakenList.ToObservableCollection();

                    //if (categorizedList.TryGetValue("not recorded", out var notRecordedList))
                    //    NotRecordedMedicationList = notRecordedList.ToObservableCollection();

                    //// Update main list and height request
                    //UserMedicationSchedule.ItemsSource = MedicationList.OrderByDescending(m => m.DatetimeOrder).ToList();
                    //UserMedicationSchedule.HeightRequest = MedicationList.Count * 120;

                    //Check and Update Double Dosage 

                    foreach (var item in MedicationList)
                    {

                        if (item.Dosage != null)
                        {
                            if (item.Dosage.Contains("|"))
                            {
                                var DosageSplit = item.Dosage.Split('|');
                                item.DosageOne = DosageSplit[0];
                                item.DosageTwo = DosageSplit[1];
                                var UnitSplit = item.unit.Split(' ');
                                item.UnitOne = UnitSplit[0] + " " + UnitSplit[1];
                                item.UnitTwo = UnitSplit[2];
                                item.DoubleDosage = true;
                                item.SingleDosage = false;
                            }
                            else
                            {
                                item.DoubleDosage = false;
                                item.SingleDosage = true;
                            }
                        }
                    }


                    var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                    MedicationList.Clear();
                    foreach (var items in sortedList)
                    {
                        MedicationList.Add(items);
                    }
                    //UserMedicationSchedule.ItemsSource = sortedList;
                    //UserMedicationSchedule.HeightRequest = sortedList.Count * 120;

                    //Filtered List 
                    TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                    NotTakenMedicationList = MedicationList.Where(s => s.Action.Equals("Not Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                    NotRecordedMedicationList = MedicationList.Where(s => s.Action.Equals("Not Recorded", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();

                }
                else if (Freq[0] == "Weekly" || Freq[0] == "Weekly ")
                {
                    DateTime? enddate = null;
                    //Populate with Schedule and Medications Not Taken 
                    if (MedSelected.feedback == null)
                    {
                        //Do Nothing
                    }
                    else
                    {

                        foreach (var item in MedSelected.feedback)
                        {
                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");

                            if (!string.IsNullOrEmpty(MedSelected.enddate))
                            {
                                enddate = DateTime.Parse(MedSelected.enddate);
                            }

                            var CurrentTime = DateTime.Parse(item.datetime);
                            if (enddate == null || CurrentTime <= enddate.Value)
                            {
                                if (item.Recorded == "Taken")
                                {
                                    SetColour = Colors.LightGreen;
                                }
                                else
                                {
                                    SetColour = Color.FromArgb("#ff6666");
                                }
                                var NewMed = new usersupplement();
                                NewMed.MedDateTime = RecordTime;
                                NewMed.Colour = SetColour;
                                NewMed.Action = item.Recorded;
                                NewMed.id = item.id;
                                MedicationList.Add(NewMed);
                            }
                        }
                        foreach (var item in MedicationList)
                        {
                            foreach (var x in MedSelected.schedule)
                            {
                                if (item.id == x.id.ToString())
                                {
                                    item.Dosage = x.Dosage;
                                    item.unit = x.dosageunit;
                                }
                            }
                        }
                    }
                    var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;

                    //add the days to med schedule

                    if (Freq[1].Contains(','))
                    {
                        var i = 0;
                        foreach (var item in MedSelected.schedule)
                        {
                            var GetDay = MedSelected.TimeDosage[i].Split('|');
                            item.Day = GetDay[2];
                            i++;
                        }

                    }
                    else
                    {

                        foreach (var item in MedSelected.schedule)
                        {
                            item.Day = Freq[1];
                        }
                    }

                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(1))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            var Day = ParseDay(item.Day);
                            if (date.DayOfWeek != Day)
                            {
                                continue;
                            }
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);
                            if (enddate != null && scheduleDateTime > enddate.Value)
                            {
                                continue;
                            }
                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }
                            var newItem = new usersupplement
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit,
                                Colour = Colors.LightGray,
                                Action = "Not Recorded",
                            };
                            MedicationNotRecordedList.Add(newItem);
                        }
                    }
                    foreach (var item in MedicationNotRecordedList)
                    {
                        if (!MedicationList.Any(m => m.MedDateTime == item.MedDateTime))
                        {
                            MedicationList.Add(item);
                        }
                    }

                    //Check and Update Double Dosage 

                    foreach (var item in MedicationList)
                    {
                        if (item.Dosage.Contains("|"))
                        {
                            var DosageSplit = item.Dosage.Split('|');
                            item.DosageOne = DosageSplit[0];
                            item.DosageTwo = DosageSplit[1];

                            var UnitSplit = item.unit.Split(' ');
                            item.UnitOne = UnitSplit[0] + " " + UnitSplit[1];
                            item.UnitTwo = UnitSplit[2];

                            item.DoubleDosage = true;
                            item.SingleDosage = false;
                        }
                        else
                        {
                            item.DoubleDosage = false;
                            item.SingleDosage = true;
                        }
                    }


                    if (MedicationList.Count == 0)
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;
                    }
                    else
                    {
                        //var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                        //UserMedicationSchedule.ItemsSource = sortedList;
                        //UserMedicationSchedule.HeightRequest = sortedList.Count * 120;

                        //Filtered List 
                        TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                        NotTakenMedicationList = MedicationList.Where(s => s.Action.Equals("Not Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                        NotRecordedMedicationList = MedicationList.Where(s => s.Action.Equals("Not Recorded", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                    }
                }
                else if (Freq[0] == "Days Interval")
                {
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;

                    DateTime? enddate = null;
                    if (!string.IsNullOrEmpty(MedSelected.enddate))
                    {
                        enddate = DateTime.Parse(MedSelected.enddate);
                    }

                    if (MedSelected.feedback == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        foreach (var item in MedSelected.feedback)
                        {
                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            var CurrentTime = DateTime.Parse(item.datetime);
                            if (enddate == null || CurrentTime <= enddate.Value)
                            {
                                if (item.Recorded == "Taken")
                                {
                                    SetColour = Colors.LightGreen;
                                }
                                else
                                {
                                    SetColour = Color.FromArgb("#ff6666");
                                }
                                var NewMed = new usersupplement();
                                NewMed.MedDateTime = RecordTime;
                                NewMed.Colour = SetColour;
                                NewMed.Action = item.Recorded;
                                NewMed.id = item.id;
                                MedicationList.Add(NewMed);
                            }
                        }
                        foreach (var item in MedicationList)
                        {
                            foreach (var x in MedSelected.schedule)
                            {
                                if (item.id == x.id.ToString())
                                {
                                    item.Dosage = x.Dosage;
                                    item.unit = x.dosageunit;
                                }
                            }
                        }
                    }
                    var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;
                    var freq = MedSelected.frequency.Split('|');
                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(Int32.Parse(freq[1])))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);
                            if (enddate != null && scheduleDateTime > enddate.Value)
                            {
                                continue;
                            }
                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }
                            var newItem = new usersupplement
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit,
                                Colour = Colors.LightGray,
                                Action = "Not Recorded",
                            };
                            MedicationNotRecordedList.Add(newItem);
                        }
                    }
                    foreach (var item in MedicationNotRecordedList)
                    {
                        if (!MedicationList.Any(m => m.MedDateTime == item.MedDateTime))
                        {
                            MedicationList.Add(item);
                        }
                    }

                    //Check and Update Double Dosage 

                    foreach (var item in MedicationList)
                    {
                        if (item.Dosage.Contains("|"))
                        {
                            var DosageSplit = item.Dosage.Split('|');
                            item.DosageOne = DosageSplit[0];
                            item.DosageTwo = DosageSplit[1];

                            var UnitSplit = item.unit.Split(' ');
                            item.UnitOne = UnitSplit[0] + " " + UnitSplit[1];
                            item.UnitTwo = UnitSplit[2];

                            item.DoubleDosage = true;
                            item.SingleDosage = false;
                        }
                        else
                        {
                            item.DoubleDosage = false;
                            item.SingleDosage = true;
                        }
                    }


                    if (MedicationList.Count == 0)
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;
                    }
                    else
                    {
                        //var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                        //UserMedicationSchedule.ItemsSource = sortedList;
                        //UserMedicationSchedule.HeightRequest = sortedList.Count * 120;

                        //Filtered List 
                        TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                        NotTakenMedicationList = MedicationList.Where(s => s.Action.Equals("Not Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();
                        NotRecordedMedicationList = MedicationList.Where(s => s.Action.Equals("Not Recorded", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservable();

                    }
                }
            }
            else
            {
                //Other As Required 
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    DayOfWeek ParseDay(string day)
    {
        return day switch
        {
            "Mon" => DayOfWeek.Monday,
            "Tues" => DayOfWeek.Tuesday,
            "Tue" => DayOfWeek.Tuesday,
            "Wed" => DayOfWeek.Wednesday,
            "Thurs" => DayOfWeek.Thursday,
            "Fri" => DayOfWeek.Friday,
            "Sat" => DayOfWeek.Saturday,
            "Sun" => DayOfWeek.Sunday,
            _ => throw new ArgumentException("Invalid day format")
        };
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //var SelectedFrame = (sender) as Frame;
        try
        {
            var SF = (TappedEventArgs)e;
            string parameter = (string)SF.Parameter;
            AllFrame.Opacity = 0;
            TakenFrame.Opacity = 0;
            if (NotTakenFrame.IsEnabled != false)
            {
                NotTakenFrame.Opacity = 0;
            }
            if (NotTakenFrame.IsEnabled != false)
            {
                NotRecordedFrame.Opacity = 0;
            }
            var Check = string.Empty;
            if (parameter == "All")
            {
                AllFrame.Opacity = 0.2;

                UserMedicationSchedule.IsVisible = true;
                noFilterstack.IsVisible = false;

                NoDatalbl.Text = "No Records Have been added yet";
                if (MedicationList.Count > 0)
                {
                    UserMedicationSchedule.IsVisible = true;
                    noFilterstack.IsVisible = false;
                    FilterTimeLine.IsVisible = true;

                    UpdateFilterDates = MedicationList;
                    CalculateTimeLine();

                }
                else
                {
                    FilterTimeLine.IsVisible = false;
                    UserMedicationSchedule.IsVisible = false;
                    noFilterstack.IsVisible = true;

                }

            }
            else if (parameter == "Taken")
            {
                TakenFrame.Opacity = 0.2;

                //Only Show Taken Items 
                NoDatalbl.Text = "No Records Contain Taken";

                if (TakenMedicationList.Count > 0)
                {
                    UserMedicationSchedule.IsVisible = true;
                    noFilterstack.IsVisible = false;
                    FilterTimeLine.IsVisible = true;

                    UpdateFilterDates = TakenMedicationList;
                    CalculateTimeLine();
                    ;
                }
                else
                {
                    FilterTimeLine.IsVisible = false;
                    UserMedicationSchedule.IsVisible = false;
                    noFilterstack.IsVisible = true;
                }
            }
            else if (parameter == "NotTaken")
            {
                if (NotTakenFrame.BackgroundColor != Colors.White)
                {
                    NotTakenFrame.Opacity = 0.2;

                    //Only Show NotTaken Items 
                    NoDatalbl.Text = "No Records Contain Not Taken";

                    if (NotTakenMedicationList.Count > 0)
                    {
                        UserMedicationSchedule.IsVisible = true;
                        noFilterstack.IsVisible = false;
                        FilterTimeLine.IsVisible = true;

                        UpdateFilterDates = NotTakenMedicationList;
                        CalculateTimeLine();

                    }
                    else
                    {
                        UserMedicationSchedule.IsVisible = false;
                        noFilterstack.IsVisible = true;
                        FilterTimeLine.IsVisible = false;
                    }
                }
            }
            else if (parameter == "NotRecorded")
            {
                if (NotRecordedFrame.BackgroundColor != Colors.White)
                {
                    NotRecordedFrame.Opacity = 0.2;
                    //Only Show NotRecorded Items 
                    UpdateFilterDates = NotRecordedMedicationList;

                    if (NotRecordedMedicationList.Count > 0)
                    {
                        UserMedicationSchedule.IsVisible = true;
                        noFilterstack.IsVisible = false;
                        FilterTimeLine.IsVisible = true;

                        UpdateFilterDates = NotRecordedMedicationList;
                        CalculateTimeLine();
                    }
                    else
                    {
                        UserMedicationSchedule.IsVisible = false;
                        noFilterstack.IsVisible = true;
                        FilterTimeLine.IsVisible = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FilterMedicationSchedule(TimelineItem item, string lblTxt)
    {
        try
        {
            FilteredMedList.Clear();

            if (item.type == "Day")
            {
                FilteredMedList = UpdateFilterDates;
                FilterTimeLine.IsVisible = false;
            }
            else if (item.type == "Week")
            {
                var splititem = item.DateRange.Split('|');
                var InitalDate = splititem[0];
                var LastDate = splititem[1];
                var StartDate = DateTime.Parse(InitalDate);
                var EndDate = DateTime.Parse(LastDate);
                var filteredMedList = UpdateFilterDates
      .Where(x =>
      {
          var medDate = DateTime.Parse(x.MedDateTime).Date;
          return medDate >= StartDate.Date && medDate <= EndDate.Date;
      })
      .ToObservable();

                FilteredMedList = filteredMedList;
            }
            else if (item.type == "Month")
            {
                FilteredMedList = UpdateFilterDates.Where(x => DateTime.Parse(x.MedDateTime).ToString("MMMM yyyy") == item.DisplayText).ToObservable();
            }

            if (FilteredMedList.Count > 0)
            {

                UserMedicationSchedule.IsVisible = true;
                noFilterstack.IsVisible = false;
                UserMedicationSchedule.ItemsSource = FilteredMedList;
                UserMedicationSchedule.HeightRequest = FilteredMedList.Count * 120;
            }
            else
            {
                NoDatalbl.Text = lblTxt;
                UserMedicationSchedule.IsVisible = false;
                noFilterstack.IsVisible = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);

        }
    }

    private void FilterTimeLine_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Check Which Frame is Selected 
            var getitem = e.DataItem as TimelineItem;
            var selectedItem = getitem.DisplayText;

            if (AllFrame.Opacity == 0.2)
            {
                SelectedFrame = "All";
            }
            else if (TakenFrame.Opacity == 0.2)
            {
                SelectedFrame = "Taken";
            }
            else
            {
                if (NotTakenFrame.IsEnabled != false)
                {
                    if (NotTakenFrame.Opacity == 0.2)
                    {
                        SelectedFrame = "NotTaken";
                    }

                }
                if (NotTakenFrame.IsEnabled != false)
                {
                    if (NotRecordedFrame.Opacity == 0.2)
                    {
                        SelectedFrame = "NotRecorded";
                    }
                }
            }

            if (SelectedFrame == "All")
            {
                UpdateFilterDates = MedicationList;
                var lblTxt = "No Records Have been added yet";
                FilterMedicationSchedule(getitem, lblTxt);
            }
            else if (SelectedFrame == "Taken")
            {
                UpdateFilterDates = TakenMedicationList;
                var lblTxt = "No Records Contain Taken";
                FilterMedicationSchedule(getitem, lblTxt);
            }
            else if (SelectedFrame == "NotTaken")
            {
                UpdateFilterDates = NotTakenMedicationList;
                var lblTxt = "No Records Contain Not Taken";
                FilterMedicationSchedule(getitem, lblTxt);
            }
            else if (SelectedFrame == "NotRecorded")
            {
                UpdateFilterDates = NotRecordedMedicationList;
                var lblTxt = "No Records Contain Not Recorded";
                FilterMedicationSchedule(getitem, lblTxt);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }
}