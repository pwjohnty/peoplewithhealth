//using System;
//using Android.Opengl;
//using Java.Time.Temporal;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;

namespace PeopleWith;

public partial class MainDashboard : ContentPage
{
    ObservableCollection<user> UserDetails = new ObservableCollection<user>();
    user AllUserDetails = new user();
    APICalls database = new APICalls();
    List<MedtimesDosages> ScheduleToday = new List<MedtimesDosages>();
    DateTime today = DateTime.Today;
    DateTime? nextDueTime = null;
    DateTime? nextDueTimeSupp = null;
    List<object> foryouuserlist = new List<object>();
    List<object> foryouuserlistsymptom = new List<object>();
    ObservableCollection<userfeedback> userfeedbacklist = new ObservableCollection<userfeedback>();
    ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    ObservableCollection<usersupplement> AllUserSupplements = new ObservableCollection<usersupplement>();

    ObservableCollection<signupcode> signupcodecollection = new ObservableCollection<signupcode>();
    public MainDashboard()
	{
		InitializeComponent();

        //Get All user Details & Set Helpers.Settings
        Checkifuserhasmigrated();

		NavigationPage.SetHasNavigationBar(this, false);

        string firstName = Preferences.Default.Get("userid", "Unknown");

       

        loadcatergories();

       // getuserfeedbackdata();

        checksignupinfo();

		//lbl.Text = firstName;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

      
        getuserfeedbackdata();
    }

    async void checksignupinfo()
    {
        try
        {
            if(string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                infotab.IsVisible = false;

                var getimagesource = new Uri("https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/PeopleWithApp-AppImage-TemplateFeb23.png");

                maindashimage.Source = ImageSource.FromUri(getimagesource);

                return;
            }

            signupcodecollection = await database.GetUserSignUpCodeInfo(Helpers.Settings.SignUp);


            if (signupcodecollection != null)
            {
                var getimagesource = new Uri("https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + signupcodecollection[0].dashboardimage);

                maindashimage.Source = ImageSource.FromUri(getimagesource);
                maindashimage2.Source = ImageSource.FromUri(getimagesource);

                signuptitlelbl.Text = signupcodecollection[0].title;

                if (signupcodecollection[0].description.Length > 600)
                {
                    signupcodecollection[0].shortdescription = signupcodecollection[0].description.Substring(0, 600);
                    signupdetailslbl.Text = signupcodecollection[0].shortdescription + "...";
                }
                else
                {
                    signupdetailslbl.Text = signupcodecollection[0].description;
                }



                foreach(var item in signupcodecollection[0].signupcodeinfolist)
                {
                    if (item.type == "pdf")
                    {
                        item.img = "pdficon.png";
                    }
                    else if (item.type == "video")
                    {
                        item.img = "videoicon.png";
                    }
                    else if (item.type == "image")
                    {
                        item.img = "imageicon.png";
                    }
                    else
                    {
                        item.img = "webicon.png";
                    }
                }

                infolist.ItemsSource = signupcodecollection[0].signupcodeinfolist;
                infolist.HeightRequest = signupcodecollection[0].signupcodeinfolist.Count * 92;
              

            }
            else
            {
                //infotab.IsVisible = false;
            }


            //get video support 
            if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {

                var allvideos = await database.GetAllVideoswithsignupcode();

                videoslist.ItemsSource = allvideos;

                videoslist.HeightRequest = 180 * allvideos.Count;
            }
            else
            {
                vidhelplbl.IsVisible = false;
                videoslist.IsVisible = false;
            }
        



        }
        catch(Exception ex)
        {

        }
    }

    async void getuserfeedbackdata()
    {
        try
        {

            //get all user feedback data

            // Start the stopwatch to measure elapsed time
            // Stopwatch stopwatch = new Stopwatch();
            // stopwatch.Start();

            foryouuserlist.Clear();

            // Retrieve all user feedback data
            userfeedbacklist = await database.GetUserFeedback();
            AllUserMedications = await database.GetUserMedicationsAsync();
            AllUserSupplements = await database.GetUserSupplementsAsync();

            findduemedications();

            findduesupplements();

            findsymptomdata();


            updateyourhealthdata();



            // Stop the stopwatch after retrieval
           // stopwatch.Stop();



            // Randomize the order of the list
            var randomizedList = foryouuserlist.OrderBy(x => Guid.NewGuid()).ToList();

            // Set the randomized list as the ItemsSource
            activitylist.ItemsSource = randomizedList;

            listloader.IsVisible = false;
            activitylist.IsVisible = true;

            // Show the elapsed time in a DisplayAlert
            //await Application.Current.MainPage.DisplayAlert(
            //    "Data Retrieval Time",
            //    $"Data retrieval took {stopwatch.Elapsed}.",
            //    "OK"
            //);
        }
        catch(Exception ex)
        {

        }
    }

    async void updateyourhealthdata()
    {
        try
        {

            //symptom data
            if (userfeedbacklist[0].symptomfeedbacklist != null)
            {
                //var groupedsymptoms = userfeedbacklist[0].symptomfeedbacklist.GroupBy(x => x.label).ToList();

                // Group by label and select the first item from each group
                var filteredSymptoms = userfeedbacklist[0].symptomfeedbacklist
                     .OrderByDescending(x => DateTime.Parse(x.datetime))
                    .GroupBy(x => x.label)
                    .Select(g => g.First())  // Select only the first item in each group
                    .ToList();



                int symptomrecordedcount = 0;

                foreach (var item in filteredSymptoms)
                {

                    var convertdate = DateTime.Parse(item.datetime);
                    TimeSpan timeDifference = DateTime.Now.Date - convertdate.Date;

                    if (timeDifference.TotalDays < 1)
                    {
                        item.datetimestring = "Today";
                    }
                    else if (timeDifference.TotalDays == 1)
                    {
                        item.datetimestring = "Yesterday";
                    }
                    else
                    {
                        item.datetimestring = $"{(int)timeDifference.TotalDays} days ago";
                    }

                    var sl = userfeedbacklist[0].symptomfeedbacklist.Where(x => x.label == item.label).OrderByDescending(x => DateTime.Parse(x.datetime)).ToList();

                    var ifrecorded = userfeedbacklist[0].symptomfeedbacklist.Where(x => x.label == item.label).Where(x => DateTime.Parse(x.datetime).Date == DateTime.Now.Date).FirstOrDefault();

                    if (ifrecorded != null)
                    {
                        symptomrecordedcount++;
                    }

                    if (sl.Count >= 2)
                    {
                        // Compare the newest and the previous score
                        var newestScore = Convert.ToInt32(sl[0].value);      // Assuming .value represents the score
                        var previousScore = Convert.ToInt32(sl[1].value);

                        if (newestScore > previousScore)
                        {
                            item.img = "downarrow.png";
                            item.improvelbl = "Worsening";


                        }
                        else if (newestScore < previousScore)
                        {
                            item.img = "uparrow.png";
                            item.improvelbl = "Improving";
                        }
                        else
                        {
                            item.img = "equal.png";
                            item.improvelbl = "No Change";
                        }


                        item.nooftimesstring = sl.Count.ToString();

                        var scores = sl.Select(x => Convert.ToInt32(x.value)).ToList();

                        // Calculate highest score and average score
                        var highestScore = scores.Max();
                        var averageScore = scores.Average();

                        item.highstring = highestScore.ToString();
                        item.avgstring = averageScore.ToString("0");

                    }
                    else
                    {
                        //no change
                        item.img = "equal.png";
                        item.improvelbl = "No Change";

                        item.avgstring = "50";
                        item.highstring = "50";
                        item.nooftimesstring = "1";
                    }

                    //if(sl.Count < 3)
                    //{
                    //    item.symptominfo = "Recently added. Monitoring for trends";
                    //}
                    //else
                    //{
                    //    Random random = new Random();

                    //    int randomNumber = random.Next(1, 8);

                    //    //choose a random stat to show

                    //    if(randomNumber == 1)
                    //    {
                    //        //average per day
                    //        var days = sl.GroupBy(x => DateTime.Parse(x.datetime).Date).Count();
                    //        var count = sl.Count(x => x.label == item.label);
                    //        var sum = (double)count / days;


                    //        item.symptominfo = "Recorded " + sum.ToString() + " times per day on average";
                    //    }
                    //    else if(randomNumber == 2)
                    //    {

                    //        var maxScore = sl.Max(x => x.value);

                    //        item.symptominfo = "Highest frequency of " + maxScore + " recorded";
                    //    }
                    //    else if (randomNumber == 3)
                    //    {
                    //        // Date and time of the highest score recorded
                    //        var maxRecord = sl.OrderByDescending(x => x.value).First();
                    //        var maxDate = DateTime.Parse(maxRecord.datetime);
                    //        item.symptominfo = "Highest score recorded on " + maxDate.ToString("dd/MM/yyyy HH:mm");
                    //    }
                    //    else if (randomNumber == 4)
                    //    {
                    //        // Check if average score has increased or decreased over the last week
                    //        DateTime now = DateTime.Now;

                    //        var lastWeekData = sl.Where(x => DateTime.Parse(x.datetime) >= now.AddDays(-7)).ToList();
                    //        var priorWeekData = sl.Where(x => DateTime.Parse(x.datetime) >= now.AddDays(-14) && DateTime.Parse(x.datetime) < now.AddDays(-7)).ToList();

                    //        var lastWeekAverage = lastWeekData.Any() ? lastWeekData.Average(x => double.TryParse(x.value, out double result) ? result : 0) : 0;

                    //        var priorWeekAverage = priorWeekData.Any()
                    //            ? priorWeekData.Average(x => double.TryParse(x.value, out double result) ? result : 0)
                    //            : 0;

                    //        if (lastWeekAverage > priorWeekAverage)
                    //        {
                    //            item.symptominfo = "Average score has increased over the last week";
                    //        }
                    //        else if (lastWeekAverage < priorWeekAverage)
                    //        {
                    //            item.symptominfo = "Average score has decreased over the last week";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Average score remained stable over the last week";
                    //        }
                    //    }
                    //    else if (randomNumber == 5)
                    //    {
                    //        // Frequency increase over the last week
                    //        var lastWeekCount = sl.Count(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-7));
                    //        var priorWeekCount = sl.Count(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-14) && DateTime.Parse(x.datetime) < DateTime.Now.AddDays(-7));

                    //        if (lastWeekCount > priorWeekCount)
                    //        {
                    //            item.symptominfo = "Recording frequency has increased over the last week";
                    //        }
                    //        else if (lastWeekCount < priorWeekCount)
                    //        {
                    //            item.symptominfo = "Recording frequency has decreased over the last week";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Recording frequency remained the same over the last week";
                    //        }
                    //    }
                    //    else if (randomNumber == 6)
                    //    {
                    //        // Check if recorded daily in the last week
                    //        var lastWeekDays = sl.Where(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-7))
                    //                             .GroupBy(x => DateTime.Parse(x.datetime).Date)
                    //                             .Count();

                    //        if (lastWeekDays >= 7)
                    //        {
                    //            item.symptominfo = "Recorded daily in the last week";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Not recorded daily in the last week";
                    //        }
                    //    }
                    //    else if (randomNumber == 7)
                    //    {
                    //        // Consistency of recording frequency
                    //        //var dailyCounts = sl.Where(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-7))
                    //        //                    .GroupBy(x => DateTime.Parse(x.datetime).Date)
                    //        //                    .Select(g => g.Count())
                    //        //                    .ToList();

                    //        //var mean = dailyCounts.Average();
                    //        //var variance = dailyCounts.Sum(c => Math.Pow(c - mean, 2)) / dailyCounts.Count;
                    //        //var stddev = Math.Sqrt(variance);

                    //        //if (stddev < 1)
                    //        //{
                    //        //    item.symptominfo = "Recording frequency is consistent";
                    //        //}
                    //        //else
                    //        //{
                    //        //    item.symptominfo = "Recording frequency is inconsistent";
                    //        //}
                    //    }
                    //    else if (randomNumber == 8)
                    //    {
                    //        // Additional custom trend: e.g., proportion of recordings in the morning/afternoon/evening
                    //        var morningCount = sl.Count(x => DateTime.Parse(x.datetime).Hour < 12);
                    //        var afternoonCount = sl.Count(x => DateTime.Parse(x.datetime).Hour >= 12 && DateTime.Parse(x.datetime).Hour < 18);
                    //        var eveningCount = sl.Count(x => DateTime.Parse(x.datetime).Hour >= 18);

                    //        if (morningCount > afternoonCount && morningCount > eveningCount)
                    //        {
                    //            item.symptominfo = "Most recordings occur in the morning";
                    //        }
                    //        else if (afternoonCount > morningCount && afternoonCount > eveningCount)
                    //        {
                    //            item.symptominfo = "Most recordings occur in the afternoon";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Most recordings occur in the evening";
                    //        }
                    //    }


                    //    //item.symptominfo = "Recently added. Monitoring for trends";
                    //}
                    var ss = "";

                    //          item.symptominfo = foryouuserlistsymptom
                    //.Where(x => x.title.Contains(item.label))
                    //.ToList();
                    //  item.symptomlist = userfeedbacklist[0].symptomfeedbacklist.Where(x => x.label == item.label).ToList();
                }


                double percentageRecorded = filteredSymptoms.Count > 0 ? (double)symptomrecordedcount / filteredSymptoms.Count * 100 : 0;

                symprogressbar.Progress = percentageRecorded;

                symptomdetaillist.ItemsSource = filteredSymptoms;

                // Initialize a list to store data points for the last seven days
                var lastSevenDaysData = new List<feedbackdata>();

                // Loop through each of the past 7 days
                for (int i = 6; i >= 0; i--)
                {
                    var targetDate = DateTime.Now.Date.AddDays(-i);

                    // Count the items for the target date
                    int countForDay = filteredSymptoms
                        .Count(item => DateTime.Parse(item.datetime).Date == targetDate);

                    // Add data point to the list
                    lastSevenDaysData.Add(new feedbackdata
                    {
                        datetime = targetDate.Date.ToString("dd MMM"),
                        Count = countForDay
                    });
                }


                //find max and add one
                int maxCount = lastSevenDaysData.Max(data => data.Count) + 1;
                chartnumaxis.Maximum = maxCount;
                // Bind the data to your chart
                symptomprogresschart.ItemsSource = lastSevenDaysData;
            }
           



            // Set the filtered list as the ItemsSource for the ListView
            if (userfeedbacklist[0].measurementfeedbacklist != null)
            {



                var filteredmeasurements = userfeedbacklist[0].measurementfeedbacklist
        .OrderByDescending(x => DateTime.Parse(x.datetime))
    .GroupBy(x => x.label)
    .Select(g => g.First())  // Select only the first item in each group
    .ToList();

                foreach (var item in filteredmeasurements)
                {
                    var convertdate = DateTime.Parse(item.datetime);
                    TimeSpan timeDifference = DateTime.Now.Date - convertdate.Date;

                    if (timeDifference.TotalDays < 1)
                    {
                        item.datetimestring = "Today";
                    }
                    else if (timeDifference.TotalDays == 1)
                    {
                        item.datetimestring = "Yesterday";
                    }
                    else
                    {
                        item.datetimestring = $"{(int)timeDifference.TotalDays} days ago";
                    }

                    // Ensure userfeedbacklist has data
                    if (userfeedbacklist != null && userfeedbacklist.Count > 0)
                    {
                        if (item.label == "Blood Pressure")
                        {

                        }
                        else
                        {

                            item.symptomlist = userfeedbacklist[0].measurementfeedbacklist
                                .Where(x => x.label == item.label)
                                .ToList();
                        }
                    }

                    item.labelandunit = item.value + " " + item.unit;
                }


                if (filteredmeasurements.Count > 1)
                {
                    var takefivemeasurements = filteredmeasurements.Take(1).ToList();

                    measurementdetaillist.ItemsSource = takefivemeasurements;
                    measurementdetaillist.HeightRequest = 152 * takefivemeasurements.Count;

                    if (filteredmeasurements.Count > 5)
                    {

                        // Take the next four items for measurementnochartdetaillist
                        var nextFourMeasurements = filteredmeasurements.Skip(1).Take(4).ToList();

                        measurementnochartdetaillist.ItemsSource = nextFourMeasurements;
                        measurementnochartdetaillist.HeightRequest = 52 * nextFourMeasurements.Count;
                    }
                    else
                    {
                        var nextFourMeasurements = filteredmeasurements.Skip(1).ToList();

                        measurementnochartdetaillist.ItemsSource = nextFourMeasurements;
                        measurementnochartdetaillist.HeightRequest = 52 * nextFourMeasurements.Count;

                    }





                }
                else
                {
                    measurementdetaillist.ItemsSource = filteredmeasurements;
                    measurementdetaillist.HeightRequest = 152 * filteredmeasurements.Count;
                }

                measlbl.IsVisible = true;
                measurementdetaillist.IsVisible = true;
                measurementnochartdetaillist.IsVisible = true;
                nomeasurementdataframe.IsVisible = false;

                var random = new Random();
                var selectedMeasurement = userfeedbacklist[0].measurementfeedbacklist[random.Next(userfeedbacklist[0].measurementfeedbacklist.Count)];

                if(selectedMeasurement != null)
                {
                   // var mostCommonMood = moodsForDay.MoodLabel;

                    var newItem2 = new
                    {
                        ContactImage = "measurementhome.png",
                        Title = "Update your " + selectedMeasurement.label,
                        BackgroundColor = "#e5f0fb" // Example color
                    };

                    foryouuserlist.Add(newItem2);
                }

                var newItem = new
                {
                    ContactImage = "measurementhome.png",
                    Title = "Start recording your measurements",
                    BackgroundColor = "#e5f0fb" // Example color
                };

                if (foryouuserlist.Contains(newItem))
                {
                    foryouuserlist.Remove(newItem);
                }

            }
            else
            {
                measlbl.IsVisible = false;
                measurementdetaillist.IsVisible = false;
                measurementnochartdetaillist.IsVisible = false;
                nomeasurementdataframe.IsVisible = true;

                var newItem = new
                {
                    ContactImage = "measurementhome.png",
                    Title = "Start recording your measurements",
                    BackgroundColor = "#e5f0fb" // Example color
                };
                foryouuserlist.Add(newItem);


            }

            if (userfeedbacklist[0].moodfeedbacklist != null)
            {

                var filteredmoods = userfeedbacklist[0].moodfeedbacklist
              .OrderByDescending(x => DateTime.Parse(x.datetime))
    .GroupBy(x => x.label)
    .Select(g => g.First())  // Select only the first item in each group
    .ToList();


                var mooddata = new List<feedbackdata>();

                foreach (var item in filteredmoods)
                {
                    var countForDay = userfeedbacklist[0].moodfeedbacklist.Where(x => x.label == item.label).Count();

                    mooddata.Add(new feedbackdata
                    {
                        label = item.label,
                        Count = countForDay
                    });

                }


                moodchart.ItemsSource = mooddata;

                moodlbl.IsVisible = true;
                moodframe.IsVisible = true;
                nomooddataframe.IsVisible = false;

                var targetDate = DateTime.Today;

                // Filter moods for the specified date
                var moodsForDay = userfeedbacklist[0].moodfeedbacklist
                    .Where(x => DateTime.Parse(x.datetime).Date == targetDate)
                    .GroupBy(x => x.label)
                    .Select(g => new
                    {
                        MoodLabel = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .FirstOrDefault(); // Get the most common mood for the day

                if (moodsForDay != null)
                {
                    // Display the most common mood
                    var mostCommonMood = moodsForDay.MoodLabel;

                    var newItem2 = new
                    {
                        ContactImage = "moodhome.png",
                        Title = "You're mostly feeling " + mostCommonMood + " today",
                        BackgroundColor = "#FFF8DC" // Example color
                    };

                    foryouuserlist.Add(newItem2);

                }
                else
                {
                    var newItem1 = new
                    {
                        ContactImage = "moodhome.png",
                        Title = "No mood recorded today",
                        BackgroundColor = "#FFF8DC" // Example color
                    };

                    foryouuserlist.Add(newItem1);

                }

                var newItem = new
                {
                    ContactImage = "moodhome.png",
                    Title = "Start updating your mood",
                    BackgroundColor = "#FFF8DC" // Example color
                };

                if (foryouuserlist.Contains(newItem))
                {

                    foryouuserlist.Remove(newItem);
                }

            }
            else
            {
                moodlbl.IsVisible = false;
                moodframe.IsVisible = false;
                nomooddataframe.IsVisible = true;

                var newItem = new
                {
                    ContactImage = "moodhome.png",
                    Title = "Start updating your mood",
                    BackgroundColor = "#FFF8DC" // Example color
                };
                foryouuserlist.Add(newItem);


            }

        }
        catch(Exception ex)
        {

        }
    }


    private void findsymptomdata()
    {
        try
        {
            // Clear previous results
            foryouuserlistsymptom.Clear();

            DateTime now = DateTime.Now;
            // Track symptom frequency in the last 3 days
            var lastThreeDays = now.AddDays(-3);
            var lastSevenDays = now.AddDays(-7);

            // Dictionary to track counts
            Dictionary<string, int> symptomFrequencyLast3Days = new Dictionary<string, int>();
            List<string> symptomsNotRecordedIn7Days = new List<string>();


            if (userfeedbacklist[0].symptomfeedbacklist == null)
            {

                var newItem = new
                {
                    ContactImage = "symptomshome.png",
                    Title = "Start recording your symptoms today",
                    BackgroundColor = "#fff7ea" // Example color
                };
                foryouuserlist.Add(newItem);


                recentsymlbl.IsVisible = false;
                symptomdetaillist.IsVisible = false;
                symdataframe.IsVisible = false;
                nosymdataframe.IsVisible = true;
                return;
            }

            recentsymlbl.IsVisible = true;
            symptomdetaillist.IsVisible = true;
            symdataframe.IsVisible = true;
            nosymdataframe.IsVisible = false;

            var newItemm = new
            {
                ContactImage = "symptomshome.png",
                Title = "Start recording your symptoms today",
                BackgroundColor = "#fff7ea" // Example color
            };
            if (foryouuserlist.Contains(newItemm))
            {
                foryouuserlist.Remove(newItemm);
            }


            bool hasRecentUpdates = userfeedbacklist[0].symptomfeedbacklist.Any(e => e.action == "update" &&
                DateTime.TryParse(e.datetime, out DateTime dateTime) &&
                (now - dateTime).TotalHours <= 24);

            if (!hasRecentUpdates)
            {
                var newItem = new
                {
                    ContactImage = "symptomshome.png",
                    Title = "You haven't recorded any symptom updates in the last 24 hours",
                    BackgroundColor = "#fff7ea" // Example color
                };
                foryouuserlistsymptom.Add(newItem);
            }
            // Group symptom entries by label for trend analysis
            var groupedSymptoms = userfeedbacklist[0].symptomfeedbacklist.GroupBy(s => s.label);

            foreach (var group in groupedSymptoms)
            {
                var symptomname = group.Key;
                var entries = group.ToList();

                // Track frequency for the last 3 days
                int recentFrequency = entries.Count(e => DateTime.TryParse(e.datetime, out DateTime entryDate) && entryDate >= lastThreeDays);
                symptomFrequencyLast3Days[symptomname] = recentFrequency;

                // Check for symptoms not recorded in the last 7 days
                var latestUpdatee = entries
                    .Where(e => e.action == "update")
                    .OrderByDescending(e => DateTime.Parse(e.datetime))
                    .FirstOrDefault();

                if (latestUpdatee == null || (now - DateTime.Parse(latestUpdatee.datetime)).TotalDays > 7)
                {
                    symptomsNotRecordedIn7Days.Add(symptomname);
                }


                // High-intensity detection
                var highIntensityEntry = entries.LastOrDefault(e => e.action == "update" && int.TryParse(e.value, out int value) && value >= 50);
                if (highIntensityEntry != null && DateTime.TryParse(highIntensityEntry.datetime, out DateTime highIntensityDateTime))
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = $"High intensity of {highIntensityEntry.value} recorded for {symptomname}",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Frequency detection for repeated updates in a short period
                if (entries.Count(e => e.action == "update" && DateTime.TryParse(e.datetime, out DateTime frequencyDateTime) && frequencyDateTime.Hour >= 9 && frequencyDateTime.Hour < 12) > 3)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = "Frequent " + symptomname + " activity detected in the morning hours",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // New symptom detection
                if (entries.Any(e => e.action == "addNew"))
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = symptomname + " recently added. Monitoring for trends",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Worsening or improving trend detection
                var recentEntries = entries.Where(e => e.action == "update" && int.TryParse(e.value, out _)).OrderByDescending(e => DateTime.Parse(e.datetime)).Take(3).ToList();
                if (recentEntries.Count == 3)
                {
                    if (int.Parse(recentEntries[0].value) > int.Parse(recentEntries[1].value) && int.Parse(recentEntries[1].value) > int.Parse(recentEntries[2].value))
                    {
                        var newItem = new
                        {
                            ContactImage = "symptomshome.png",
                            Title = symptomname + " intensity appears to be worsening",
                            BackgroundColor = "#fff7ea"
                        };
                        foryouuserlistsymptom.Add(newItem);
                    }
                    else if (int.Parse(recentEntries[0].value) < int.Parse(recentEntries[1].value) && int.Parse(recentEntries[1].value) < int.Parse(recentEntries[2].value))
                    {
                        var newItem = new
                        {
                            ContactImage = "symptomshome.png",
                            Title = symptomname + " intensity appears to be improving",
                            BackgroundColor = "#fff7ea"
                        };
                        foryouuserlistsymptom.Add(newItem);
                    }
                }
                // Persistent high intensity
                var consecutiveHighIntensity = entries
                    .Where(e => e.action == "update" && int.TryParse(e.value, out int intensity) && intensity >= 50)
                    .OrderByDescending(e => DateTime.Parse(e.datetime))
                    .Take(3)
                    .Count();
                if (consecutiveHighIntensity >= 3)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = "Persistent high intensity for " + symptomname + " reported",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Daily pattern detection
                var morningEntries = entries.Where(e => DateTime.TryParse(e.datetime, out DateTime morningDateTime) && morningDateTime.Hour >= 5 && morningDateTime.Hour < 12).Count();
                var eveningEntries = entries.Where(e => DateTime.TryParse(e.datetime, out DateTime eveningDateTime) && eveningDateTime.Hour >= 17 && eveningDateTime.Hour < 22).Count();
                if (morningEntries > 5)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = symptomname + " commonly reported during morning hours",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                if (eveningEntries > 5)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = symptomname + " commonly reported during evening hours",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Infrequent updates
                var latestUpdate = entries.Where(e => e.action == "update").OrderByDescending(e => DateTime.Parse(e.datetime)).FirstOrDefault();
                if (latestUpdate != null && (now - DateTime.Parse(latestUpdate.datetime)).TotalDays > 7)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = "No recent updates for " + symptomname + " in the past week",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Sudden intensity spike detection
                var lastTwoEntries = entries.Where(e => e.action == "update").OrderByDescending(e => DateTime.Parse(e.datetime)).Take(2).ToList();
                if (lastTwoEntries.Count == 2 && (int.Parse(lastTwoEntries[0].value) - int.Parse(lastTwoEntries[1].value)) > 20)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = "Sudden spike in intensity detected for " + symptomname,
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Time since first recorded entry
                var firstEntry = entries.OrderBy(e => DateTime.Parse(e.datetime)).FirstOrDefault();
                if (firstEntry != null)
                {
                    var daysSinceFirst = (now - DateTime.Parse(firstEntry.datetime)).TotalDays;
                    if (daysSinceFirst > 30)
                    {
                        var newItem = new
                        {
                            ContactImage = "symptomshome.png",
                            Title = symptomname + " has been tracked for over " + Math.Round(daysSinceFirst) + " days",
                            BackgroundColor = "#fff7ea"
                        };
                        foryouuserlistsymptom.Add(newItem);
                    }
                }
            
            }

            // 1. Display the most recorded symptom in the last 3 days
            if (symptomFrequencyLast3Days.Count > 0)
            {
                var mostRecordedSymptom = symptomFrequencyLast3Days
                    .OrderByDescending(kv => kv.Value)
                    .FirstOrDefault();

                if (mostRecordedSymptom.Value > 0)
                {
                    var newItem = new
                    {
                        ContactImage = "symptomshome.png",
                        Title = $"Most recorded symptom in the last 3 days: {mostRecordedSymptom.Key}",
                        BackgroundColor = "#fff7ea"
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
            }

            // 2. Display symptoms not recorded in the last 7 days
            foreach (var symptom in symptomsNotRecordedIn7Days)
            {
                var newItem = new
                {
                    ContactImage = "symptomshome.png",
                    Title = $"{symptom} has not been recorded in the last 7 days. Update now",
                    BackgroundColor = "#fff7ea"
                };
                foryouuserlistsymptom.Add(newItem);
            }


            Random random = new Random();

            // Select 3 random items from foryouuserlistsymptomlist
            var randomItems = foryouuserlistsymptom
                .OrderBy(x => random.Next())
                .Take(3)
                .ToList();

            // Add the selected items to foryoulist
            foryouuserlist.AddRange(randomItems);



        }
        catch(Exception ex)
        {

        }
    }

    async void findduesupplements()
    {
        try
        {
            //added to see if there is any due
            bool hasDueMedications = false;
            string timeoflastnotrecordedmed = "";

            int medicationsDueToday = 0;
            int recordedMedicationsToday = 0;

            if (AllUserSupplements.Count == 0)
            {
                var newItem = new
                {
                    ContactImage = "supphome.png",
                    Title = "Add your first Supplement",
                    BackgroundColor = "#f9f4e5" // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);

                nosupdataframe.IsVisible = true;

                return;
            }

            nosupdataframe.IsVisible = false;


            foreach (var item in AllUserSupplements)
            {
                DateTime startDate = DateTime.Parse(item.startdate);
                DateTime? endDate = !string.IsNullOrEmpty(item.enddate) ? DateTime.Parse(item.enddate) : (DateTime?)null;
                bool isOngoing = !endDate.HasValue || today <= endDate.Value;

                var splitString = item.frequency.Split('|');
                string frequencyType = splitString[0];

                // Check if medication is due today based on frequency
                bool isDueToday = false;

                if (frequencyType == "Daily" && today >= startDate && isOngoing)
                {
                    isDueToday = true;
                }
                else if (frequencyType == "Weekly" && today >= startDate && isOngoing)
                {
                    // Get days of the week from frequency (e.g., "Mon,Wed,Fri")
                    var weekDays = splitString[1].Split(',');
                    var todayDayName = today.ToString("ddd");

                    // Check if today is one of the specified days
                    if (weekDays.Contains(todayDayName))
                    {
                        isDueToday = true;
                    }
                }
                else if (frequencyType == "Days Interval" && today >= startDate && isOngoing)
                {
                    int dayInterval = Convert.ToInt32(splitString[1]);
                    var daysSinceStart = (today - startDate).Days;

                    // Check if today's date falls on an interval
                    if (daysSinceStart % dayInterval == 0)
                    {
                        isDueToday = true;
                    }
                }
                else if (frequencyType == "As Required" && item.feedback != null)
                {
                    foreach (var feedback in item.feedback)
                    {
                        DateTime feedbackDate = DateTime.Parse(feedback.datetime);
                        if (feedbackDate.Date == today)
                        {
                            isDueToday = true;
                            break;
                        }
                    }
                }

                // If due today, add to the list
                if (isDueToday)
                {
                    hasDueMedications = true;

                    foreach (var medTime in item.schedule)
                    {
                        var time = TimeSpan.Parse(medTime.time);
                        DateTime scheduledTimeToday = DateTime.Today.Add(time); // Add time component to today's date

                        medicationsDueToday++;

                        if (scheduledTimeToday < DateTime.Now)
                        {
                            var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                            bool hasBeenRecorded = item.feedback != null && item.feedback
                            .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                            //bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                            // .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                            // If not recorded, update nextDueTime
                            if (!hasBeenRecorded)
                            {
                                timeoflastnotrecordedmed = scheduledTimeToday.ToString("HH:mm");
                            }
                            else
                            {
                                recordedMedicationsToday++;
                            }
                        }
                        // Check if this time is in the future and closer than the current nextDueTime
                        else if (scheduledTimeToday > DateTime.Now && (nextDueTime == null || scheduledTimeToday < nextDueTime))
                        {
                            // Check if this scheduled time has already been recorded in user feedback
                            //  bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                            //    .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                            var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                            bool hasBeenRecorded = item.feedback != null && item.feedback
                        .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                            // If not recorded, update nextDueTime
                            if (!hasBeenRecorded)
                            {
                                nextDueTime = scheduledTimeToday;
                            }
                            else
                            {
                                recordedMedicationsToday++;
                            }
                        }

                    }
                }
            }

            double percentageRecorded = medicationsDueToday > 0 ? (double)recordedMedicationsToday / medicationsDueToday * 100 : 0;

            suppprogressbar.Progress = percentageRecorded;

            if (hasDueMedications == false)
            {
                // Create a new item to add
                var newItem = new
                {
                    ContactImage = "supphome.png",
                    Title = "No Supplements Due Today",
                    BackgroundColor = "#f9f4e5" // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);


                //possibly check if they are recording as required medications a lot in the last seven days

            }

            else if (nextDueTime == null)
            {
                //no more times

                if (hasDueMedications)
                {
                    //check if they have added feedback for previous meds for that day 


                    if (!string.IsNullOrEmpty(timeoflastnotrecordedmed))
                    {
                        var nnewItem = new
                        {
                            ContactImage = "supphome.png",
                            Title = "Have you recorded your " + timeoflastnotrecordedmed + " Supplements ?",
                            BackgroundColor = "#f9f4e5" // Example color
                        };

                        // Add the new item to the list
                        foryouuserlist.Add(nnewItem);
                    }


                    var newItem = new
                    {
                        ContactImage = "supphome.png",
                        Title = "No More Supplements Due Today",
                        BackgroundColor = "#f9f4e5" // Example color
                    };

                    // Add the new item to the list
                    foryouuserlist.Add(newItem);
                }

            }
            else
            {

                // Create a new item to add
                var newItem = new
                {
                    ContactImage = "supphome.png",
                    Title = "Record your " + nextDueTime.Value.ToString("HH:mm") + " Supplements",
                    BackgroundColor = "#f9f4e5" // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);


            }
        }
        catch(Exception ex)
        {

        }
    }

    async void findduemedications()
    {
        try
        {
            //added to see if there is any due
            bool hasDueMedications = false;
            string timeoflastnotrecordedmed = "";

            int medicationsDueToday = 0;
            int recordedMedicationsToday = 0;

            if(AllUserMedications.Count == 0)
            {
                var newItem = new
                {
                    ContactImage = "medicinehome.png",
                    Title = "Add your first Medication",
                    BackgroundColor = "#e5f9f4" // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);

                nomeddataframe.IsVisible = true;

                return;
            }

            nomeddataframe.IsVisible = false;

            foreach (var item in AllUserMedications)
            {
                DateTime startDate = DateTime.Parse(item.startdate);
                DateTime? endDate = !string.IsNullOrEmpty(item.enddate) ? DateTime.Parse(item.enddate) : (DateTime?)null;
                bool isOngoing = !endDate.HasValue || today <= endDate.Value;

                var splitString = item.frequency.Split('|');
                string frequencyType = splitString[0];

                // Check if medication is due today based on frequency
                bool isDueToday = false;

                if (frequencyType == "Daily" && today >= startDate && isOngoing)
                {
                    isDueToday = true;
                }
                else if (frequencyType == "Weekly" && today >= startDate && isOngoing)
                {
                    // Get days of the week from frequency (e.g., "Mon,Wed,Fri")
                    var weekDays = splitString[1].Split(',');
                    var todayDayName = today.ToString("ddd");

                    // Check if today is one of the specified days
                    if (weekDays.Contains(todayDayName))
                    {
                        isDueToday = true;
                    }
                }
                else if (frequencyType == "Days Interval" && today >= startDate && isOngoing)
                {
                    int dayInterval = Convert.ToInt32(splitString[1]);
                    var daysSinceStart = (today - startDate).Days;

                    // Check if today's date falls on an interval
                    if (daysSinceStart % dayInterval == 0)
                    {
                        isDueToday = true;
                    }
                }
                else if (frequencyType == "As Required" && item.feedback != null)
                {
                    foreach (var feedback in item.feedback)
                    {
                        DateTime feedbackDate = DateTime.Parse(feedback.datetime);
                        if (feedbackDate.Date == today)
                        {
                            isDueToday = true;
                            break;
                        }
                    }
                }

                // If due today, add to the list
                if (isDueToday)
                {
                    hasDueMedications = true;

                    foreach (var medTime in item.schedule)
                    {
                        medicationsDueToday++;

                        var time = TimeSpan.Parse(medTime.time);
                        DateTime scheduledTimeToday = DateTime.Today.Add(time); // Add time component to today's date

                        if (scheduledTimeToday < DateTime.Now)
                        {
                            var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                            bool hasBeenRecorded = item.feedback != null && item.feedback
                            .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                            //bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                            // .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                            // If not recorded, update nextDueTime
                            if (!hasBeenRecorded)
                            {
                                timeoflastnotrecordedmed = scheduledTimeToday.ToString("HH:mm");
                            }
                            else
                            {
                                recordedMedicationsToday++;
                            }
                        }
                        // Check if this time is in the future and closer than the current nextDueTime
                        else if (scheduledTimeToday > DateTime.Now && (nextDueTime == null || scheduledTimeToday < nextDueTime))
                        {
                            // Check if this scheduled time has already been recorded in user feedback
                            //  bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                            //    .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                            var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                            bool hasBeenRecorded = item.feedback != null && item.feedback
                        .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                            // If not recorded, update nextDueTime
                            if (!hasBeenRecorded)
                            {
                                nextDueTime = scheduledTimeToday;
                            }
                            else
                            {
                                recordedMedicationsToday++;
                            }
                        }

                    }
                }
            }

            // Calculate the percentage of recorded medications for the progress bar
            double percentageRecorded = medicationsDueToday > 0 ? (double)recordedMedicationsToday / medicationsDueToday * 100 : 0;
            medprogressbar.Progress = percentageRecorded;


            if (hasDueMedications == false)
            {
                // Create a new item to add
                var newItem = new
                {
                    ContactImage = "medicinehome.png",
                    Title = "No Medications Due Today",
                    BackgroundColor = "#e5f9f4" // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);

                //possibly check if they are recording as required medications a lot in the last seven days

            }

            else if (nextDueTime == null)
            {
                //no more times

                if (hasDueMedications)
                {
                    //check if they have added feedback for previous meds for that day 


                    if (!string.IsNullOrEmpty(timeoflastnotrecordedmed))
                    {
                        var nnewItem = new
                        {
                            ContactImage = "medicinehome.png",
                            Title = "Have you recorded your " + timeoflastnotrecordedmed + " Medications ?",
                            BackgroundColor = "#e5f9f4" // Example color
                        };

                        // Add the new item to the list
                        foryouuserlist.Add(nnewItem);
                    }


                    var newItem = new
                    {
                        ContactImage = "medicinehome.png",
                        Title = "No More Medications Due Today",
                        BackgroundColor = "#e5f9f4" // Example color
                    };

                    // Add the new item to the list
                    foryouuserlist.Add(newItem);
                }

            }
            else
            {

                // Create a new item to add
                var newItem = new
                {
                    ContactImage = "medicinehome.png",
                    Title = "Record your " + nextDueTime.Value.ToString("HH:mm") + " Medications",
                    BackgroundColor = "#e5f9f4" // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);


            }
        }
        catch(Exception ex)
        {

        }
    }

    async void loadcatergories()
    {
        try
        {
            // Add all categories for help videos
            var allcatvideolist = new List<object>
            {
                new { ContactImage = "symptomshome.png", Title = "Symptoms", BackgroundColor = "#fff7ea" },
                new { ContactImage = "medicinehome.png", Title = "Medications", BackgroundColor = "#e5f9f4" },
                new { ContactImage = "supphome.png", Title = "Supplements", BackgroundColor = "#f9f4e5" },
                new { ContactImage = "measurementhome.png", Title = "Measurements", BackgroundColor = "#e5f0fb" },
                new { ContactImage = "diagnosishome.png", Title = "Diagnosis", BackgroundColor = "#E6E6FA" },
                new { ContactImage = "moodhome.png", Title = "Mood", BackgroundColor = "#FFF8DC" },
                new { ContactImage = "appointhome.png", Title = "Appointments", BackgroundColor = "#ffcccb" },
                new { ContactImage = "hcphome.png", Title = "HCPs", BackgroundColor = "#CBC3E3" },
                new { ContactImage = "questionnairehome.png", Title = "Questionnaires", BackgroundColor = "#fff9ec" },
                new { ContactImage = "allergenhome.png", Title = "Allergens", BackgroundColor = "#FFF5EE" }
            };

            allhelpvideocatlist.ItemsSource = allcatvideolist;

            var extendedCatvideolist = new List<object>(allcatvideolist)
            {
              new { ContactImage = "videoicon.png", Title = "Help Videos", BackgroundColor = "#e9e9e9" },
              new { ContactImage = "profileicon.png", Title = "Profile", BackgroundColor = "#deeff5" }
            };

            // Set the extended list as the data source for catergorieslist
            catergorieslist.ItemsSource = extendedCatvideolist;


            var listforaccountlist = new List<object>
{
    new { Title = "Account Settings" },
    new { Title = "Developer Feedback & Support" },
    new { Title = "Terms Of Use" }
};

            // Set the height of the account list based on the item count
            accountlist.HeightRequest = listforaccountlist.Count * 48;
            accountlist.ItemsSource = listforaccountlist;

            foryouuserlist = new List<object>
            {
               // new { ContactImage = "symptomshome.png", Title = "Update Backache", BackgroundColor = "#fff7ea" },
                new { ContactImage = "healthreporticon.png", Title = "Generate your Health Report", BackgroundColor = "#e5f5fc" },
                new { ContactImage = "diagnosishome.png", Title = "Have you received a new diagnosis ?", BackgroundColor = "#E6E6FA" },
                new { ContactImage = "appointhome.png", Title = "Record a new appointment", BackgroundColor = "#ffcccb" },
    
            };

           // activitylist.ItemsSource = foryouuserlist;


        }
        catch (Exception ex)
        {

        }
    }

    async private void Checkifuserhasmigrated()
    {
        try
        {
            // UserDetails = await database.GetuserDetails();

            //string um = Preferences.Default.Get("usermigrated", "false");

            //if (um == "True" || string.IsNullOrEmpty(um) || um == "False")
            //{
                
            //        //go to migration assitant

            //        await Navigation.PushAsync(new MigrationAssistant(), false);
            //        return;


                
                
            //}
         
           
            // Assuming UserDetails[0].dateofbirth is a string in a format like "yyyy-MM-dd"
            string dateOfBirthString = Helpers.Settings.Age;

            // Convert the string to DateTime
            DateTime dateOfBirth = DateTime.Parse(dateOfBirthString);

            // Calculate the age
            int age = DateTime.Now.Year - dateOfBirth.Year;

            // Check if the birthday has occurred this year, if not subtract 1
            if (DateTime.Now < dateOfBirth.AddYears(age))
            {
                age--;
            }

            // Display the age in the label
            agelbl.Text = age.ToString();

            //AllUserDetails.gender = UserDetails[0].gender;
           //Helpers.Settings.Gender = UserDetails[0].gender;

            genderlbl.Text = Helpers.Settings.Gender;

         //   AllUserDetails.ethnicity = UserDetails[0].ethnicity;
          //  Helpers.Settings.Ethnicity = UserDetails[0].ethnicity;

            ethlbl.Text = Helpers.Settings.Ethnicity;

          

        }
        catch (Exception ex)
        {

        }
    }

    private void tabsview_TabItemTapped(object sender, Syncfusion.Maui.TabView.TabItemTappedEventArgs e)
    {
        try
        {
            if (e.TabItem.Header == "Home")
            {

                hometab.ImageSource = ImageSource.FromFile("dashiconactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreinactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseinactive.png");

                hometab.TextColor = Color.FromArgb("#031926");
                infotab.TextColor = Color.FromArgb("#b3babd");
                profiletab.TextColor = Color.FromArgb("#b3babd");

                hometab.FontFamily = "HankenGroteskBold";
                infotab.FontFamily = "HankenGroteskRegular";
                profiletab.FontFamily = "HankenGroteskRegular";
            }
            else if (e.TabItem.Header == "Explore")
            {
                hometab.ImageSource = ImageSource.FromFile("dashiconinactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseinactive.png");

                infotab.TextColor = Color.FromArgb("#031926");
                hometab.TextColor = Color.FromArgb("#b3babd");
                profiletab.TextColor = Color.FromArgb("#b3babd");

                infotab.FontFamily = "HankenGroteskBold";
                hometab.FontFamily = "HankenGroteskRegular";
                profiletab.FontFamily = "HankenGroteskRegular";
            }
            else if (e.TabItem.Header == "Browse")
            {
                hometab.ImageSource = ImageSource.FromFile("dashiconinactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreinactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseactive.png");

                profiletab.TextColor = Color.FromArgb("#031926");
                infotab.TextColor = Color.FromArgb("#b3babd");
                hometab.TextColor = Color.FromArgb("#b3babd");


                profiletab.FontFamily = "HankenGroteskBold";
                hometab.FontFamily = "HankenGroteskRegular";
                infotab.FontFamily = "HankenGroteskRegular";


            }
 
        }
        catch (Exception ex)
        {

        }
    }

    private async void allhelpvideocatlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as dynamic;

            if (item != null && item.Title == "Medications")
            {
                await Navigation.PushAsync(new AllMedications(), false);
            }

            else if (item != null && item.Title == "Symptoms")
            {
                await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
            }
            else if(item != null && item.Title == "Supplements")
            {
                await Navigation.PushAsync(new AllSupplements(), false);
            }
            else if (item != null && item.Title == "Measurements")
            {
                await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
            }
            else if (item != null && item.Title == "Diagnosis")
            {
                await Navigation.PushAsync(new AllDiagnosis(), false);
            }
            else if (item != null && item.Title == "Mood")
            {
                await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
            }
            else if (item != null && item.Title == "Appointments")
            {
                await Navigation.PushAsync(new Appointments(), false);
            }
            else if (item != null && item.Title == "HCPs")
            {
                await Navigation.PushAsync(new HCPs(), false);
            }
            else if (item != null && item.Title == "Questionnaires")
            {
                await Navigation.PushAsync(new AllQuestionnaires(), false);
            }
            else if (item != null && item.Title == "Allergens")
            {
                await Navigation.PushAsync(new AllAllergies(), false);
            }

        }
        catch(Exception ex)
        {

        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {

            //see more button
            if(morebtn.Text == "See more")
            {
                signupdetailslbl.Text = signupcodecollection[0].description;
                morebtn.Text = "See less";

            }
            else
            {
                signupdetailslbl.Text = signupcodecollection[0].shortdescription;
                morebtn.Text = "See more";
            }

        }
        catch(Exception ex)
        {

        }
    }

    private async void infolist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as signupcodeinformation;

            if (item.type == "pdf")
            {
                var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;
                await Browser.OpenAsync(pdflink, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide
                });
            }
            else if (item.type == "video")
            {
                var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;
                string imgPath = pdflink + ".mp4";
              //  var launchvid = new videosupport();
               // launchvid.URL = item.Filename;
               // await Navigation.PushModalAsync(new AndroidSingleView(launchvid));
            }
            else
            {
                await Browser.OpenAsync(item.link, BrowserLaunchMode.SystemPreferred);
            }
         
        }
        catch (Exception ex)
        {
        }
    }

    async private void HealthReportBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new HealthReport(), false);
        }
        catch (Exception Ex)
        {

        }
    }

   async  private void QuestionBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AllQuestionnaires(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    private async  void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //profile section tapped 

        try
        {
            await Navigation.PushAsync(new ProfileSection(), false);

        }
        catch(Exception ex)
        {

        }


    }

    private async void catergorieslist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as dynamic;

            if (item != null && item.Title == "Medications")
            {
                await Navigation.PushAsync(new AllMedications(), false);
            }

            else if (item != null && item.Title == "Symptoms")
            {
                await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
            }
            else if (item != null && item.Title == "Supplements")
            {
                await Navigation.PushAsync(new AllSupplements(), false);
            }
            else if (item != null && item.Title == "Measurements")
            {
                await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
            }
            else if (item != null && item.Title == "Diagnosis")
            {
                await Navigation.PushAsync(new AllDiagnosis(), false);
            }
            else if (item != null && item.Title == "Mood")
            {
                await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
            }
            else if (item != null && item.Title == "Appointments")
            {
                await Navigation.PushAsync(new Appointments(), false);
            }
            else if (item != null && item.Title == "HCPs")
            {
                await Navigation.PushAsync(new HCPs(), false);
            }
            else if (item != null && item.Title == "Questionnaires")
            {
                await Navigation.PushAsync(new AllQuestionnaires(), false);
            }
            else if (item != null && item.Title == "Allergens")
            {
                await Navigation.PushAsync(new AllAllergies(), false);
            }
            else if (item != null && item.Title == "Help Videos")
            {
                await Navigation.PushAsync(new AllVideos(), false);
            }
            else if (item != null && item.Title == "Profile")
            {
                await Navigation.PushAsync(new ProfileSection(), false);
            }

        }
        catch (Exception ex)
        {

        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            //no symptom data button click
            await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);

        }
        catch(Exception ex)
        {

        }
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            //no symptom data button click
            await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);

        }
        catch (Exception ex)
        {

        }
    }

    private async void accountlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as string;

            if(item == "Account Settings")
            {
                await Navigation.PushAsync(new ProfileSection(), false);
            }
            else if(item.Contains("Developer Feedback"))
            {
                if (Email.Default.IsComposeSupported)
                {

                    string subject = "";
                    string body = "Userid: " + Helpers.Settings.UserKey;
                    string[] recipients = new[] { "support@peoplewith.com" };

                    var message = new EmailMessage
                    {
                        Subject = subject,
                        Body = body,
                        BodyFormat = EmailBodyFormat.PlainText,
                        To = new List<string>(recipients)
                    };

                    await Email.Default.ComposeAsync(message);
                }
            }
            else if(item == "Terms Of Use")
            {

            }

        }
        catch(Exception ex)
        {

        }
    }

    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AllSupplements(), false);
        }
        catch(Exception ex)
        {

        }
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new SearchPage(), false);
        }
        catch(Exception ex)
        {

        }
    }

    private async void Button_Clicked_4(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
        }
        catch(Exception ex)
        {

        }
    }
}