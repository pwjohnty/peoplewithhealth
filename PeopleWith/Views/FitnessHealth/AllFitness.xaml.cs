using Plugin.Maui.Health.Enums;
using Plugin.Maui.Health;
using System.Net;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Globalization;

using Plugin.Maui.Health.Constants;
using Newtonsoft.Json;


//using Android.Support.Customtabs.Trusted;

namespace PeopleWith;

public partial class AllFitness : ContentPage
{
    private readonly IHealth health;
    ObservableCollection<userfitnessdata> userfitnesslist = new ObservableCollection<userfitnessdata>();
    APICalls database = new APICalls();
    public List<string> q1list = new List<string>();
    public List<string> q2list = new List<string>();
    public List<string> q3list = new List<string>();
    public List<string> q4list = new List<string>();
    public List<string> q5list = new List<string>();

    public List<Plugin.Maui.Health.Models.Sample> Data1 { get; private set; } = new();
    public AllFitness()
	{
		InitializeComponent();

        health = HealthDataProvider.Default;

        uploadallfitnessdata();

        getalldata();
    }
    async void getalldata()
    {
        try
        {

            var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            if (hasPermission)
            {
                // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);
                stepsborder.IsVisible = true;

                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-30); // start of today (00:00 UTC)
                var unit = "";

                // Query step count data
                //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);



                var dailyTotals = new List<(string Date, double TotalSteps)>();

                for (int i = 6; i >= 0; i--)
                {
                    var day = DateTime.Today.AddDays(-i);
                    var dayStart = day;
                    var dayEnd = day.AddDays(1);

                    var totalStepss = await health.ReadCountAsync(HealthParameter.StepCount, dayStart, dayEnd);

                    dailyTotals.Add((day.ToString("dd/MM/yyyy"), totalStepss));
                }

                var orderedDailyTotals = dailyTotals
                    .OrderBy(d => DateTime.Parse(d.Date))
                    .Select(d => new
                    {
                        Date = d.Date,
                        TotalSteps = Math.Round(d.TotalSteps)
                    }).ToList();

                var lastday = orderedDailyTotals.TakeLast(1);
                symptomprogresschart.ItemsSource = lastday;

                if (orderedDailyTotals.Count == 0)
                {
                    // rrchart.IsVisible = false;
                    stepsnodata.IsVisible = true;
                    stepscountlbl.Text = "--";
                    totallbl.Text = "--";
                    return;

                }
                else
                {
                    //  rrchart.IsVisible = true;
                    stepsnodata.IsVisible = false;
                }

                //            Data1 = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);

                //            var distinctData = Data1
                //.GroupBy(d => new { d.From, d.Until })
                //.Select(g => g.First())
                //.ToList();

                //            var dailyTotals = distinctData
                //       .GroupBy(d => d.From?.ToString("dd/MM/yyyy"))
                //       .Select(g => new
                //       {
                //           Date = g.Key,
                //           TotalSteps = g.Sum(d => d.Value)
                //       })
                //       .OrderBy(x => DateTime.Parse(x.Date));


                //            var last7Days = dailyTotals.TakeLast(7);
                //            symptomprogresschart.ItemsSource = last7Days;



                // Take last 7 days


                // Calculate average
                var averageSteps = lastday.Average(x => x.TotalSteps);
                var totalSteps = lastday.Sum(x => x.TotalSteps);


                int roundedsteps = (int)Math.Round((decimal)averageSteps);
                int roundedtotal = (int)Math.Round((double)totalSteps);

                stepscountlbl.Text = roundedsteps.ToString();
                totallbl.Text = roundedtotal.ToString();


            }
            else
            {
                stepsborder.IsVisible = false;
            }



                var hasPermissionhr = await health.CheckPermissionAsync(HealthParameter.HeartRate, PermissionType.Read);
            if (hasPermissionhr)
            {
                hrborder.IsVisible = true;

                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);
                 var unit = "count/min";

                var data1 = await health.ReadAllAsync(HealthParameter.HeartRate, today, tomorrow, unit);

                var todayDataPoints = data1
                    .Where(d => d.From.HasValue)
                    .Select(d => new
                    {
                        Date = d.From.Value.ToString("HH:mm"),
                        Min = d.Value
                        
                    })
                    .OrderBy(d => d.Date)
                    .ToList();

                // Bind to your chart
                heartratechartdayonly.ItemsSource = todayDataPoints;

                hrdaychart.IsVisible = true;


                if (todayDataPoints.Count == 0)
                {
                   // rrchart.IsVisible = false;
                    hrnodata.IsVisible = true;
                    hrcountlbl.Text = "--";
                    hrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }
                //var endDate = DateTime.UtcNow;
                //var startDate = endDate.Date.AddDays(-1); // start of today (00:00 UTC)
                //var unit = "count/min";



                //var data1 = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, unit);


                //var dailyMinMax = data1
                //   .Where(d => d.From.HasValue) // ensure From is not null
                //   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                //   .Select(g => new
                //   {
                //       Date = g.Key.ToString("dd/MM/yyyy"),
                //       Min = g.Min(d => d.Value),
                //       Max = g.Max(d => d.Value)

                //   })
                //   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                //   .ToList();

                //heartratechart.ItemsSource = dailyMinMax;


                // Take last 7 days
                //  var last7Days = dailyMinMax.Take(7);

                // Calculate average
                var weeklyAverageHR = todayDataPoints.Min(x => x.Min);
                var weeklyMaxHR = todayDataPoints.Max(x => x.Min);


                int roundedsteps = (int)Math.Round((decimal)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                hrcountlbl.Text = roundedsteps.ToString();
                hrtotalbl.Text = roundedtotal.ToString();

            }
            else
            {
                hrborder.IsVisible = false;
            }

                var hasPermissionrr = await health.CheckPermissionAsync(HealthParameter.RespiratoryRate, PermissionType.Read);
            if (hasPermissionrr)
            {
                rrborder.IsVisible = true;

                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-1); // start of today (00:00 UTC)
                var unit = "count/min";



                var data11 = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, unit);


                var dailyMinMax = data11
                   .Where(d => d.From.HasValue) // ensure From is not null
                   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                   .Select(g => new
                   {
                       Date = g.Key.ToString("dd/MM/yyyy"),
                       Min = g.Min(d => d.Value),
                       Max = g.Max(d => d.Value)

                   })
                   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                rrchart.ItemsSource = dailyMinMax;


                if(dailyMinMax.Count == 0)
                {
                    rrchart.IsVisible = false;
                    rrnodata.IsVisible = true;
                    rrcountlbl.Text = "--";
                    rrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }
                    // Take last 7 days
                    // var last7Days = dailyMinMax.Take(7);

                    // Calculate average
                    var weeklyAverageHR = dailyMinMax.Min(x => x.Min);
                var weeklyMaxHR = dailyMinMax.Max(x => x.Max);


                int roundedsteps = (int)Math.Round((double)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);


                if (roundedsteps == 0)
                {
                    rrcountlbl.Text = "--";
                }
                else
                {
                    rrcountlbl.Text = roundedsteps.ToString();
                }

                if (roundedtotal == 0)
                {
                    rrtotalbl.Text = "--";
                }
                else
                {
                    rrtotalbl.Text = roundedtotal.ToString();
                }

            }
            else
            {
                rrborder.IsVisible = false;
            }


        }
		catch(Exception ex)
		{

		}
	}


    async void uploadallfitnessdata()
    {
        try
        {
            //check if there is any user fitness 

            // Retrieve all user feedback data
            userfitnesslist = await database.GetUserFitnessData();

            if(userfitnesslist != null)
            {

                if (userfitnesslist[0].stepfeedbacklist != null)
                {
                    // var stepfeedback = JsonConvert.DeserializeObject<List<fitnessfeedback>>(userfitnesslist[0].stepfeedback);

                    var lastItem = userfitnesslist[0].stepfeedbacklist.FirstOrDefault();


                    //get date

                    if (lastItem != null)
                    {
                        var endDate = DateTime.Parse(lastItem.datetime);
                        var now = DateTime.Now;

                        var current = new DateTime(now.Year, now.Month, 1);
                        var end = new DateTime(endDate.Year, endDate.Month, 1);

                        // Step 3: Create list of month strings
                        // Calculate month difference
                        int totalMonths = ((current.Year - endDate.Year) * 12) + (current.Month - endDate.Month) + 1;

                        // Generate list of month strings
                        List<string> monthList = Enumerable.Range(0, totalMonths)
                            .Select(i => endDate.AddMonths(i).ToString("MMMM yyyy"))
                            .ToList();

                        monthList.Reverse();
                        // Step 4: Bind to your ListView
                        stepsmonthlist.ItemsSource = monthList;

                    }


                }


            }

            //if(userfitnesslist == null || userfitnesslist.Count == 0)
            //{
            //    NavigationPage.SetHasNavigationBar(this, false);
            //    syncstack.IsVisible = true;
            //    mainstack.IsVisible = false;

            //    //add it a new user fitness


            //    var newuserfitness = new userfitnessdata();

            //    newuserfitness.userid = Helpers.Settings.UserKey;


            //    //step data

            //    var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            //    if (hasPermission)
            //    {
            //        // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);
            //        var stopwatch = Stopwatch.StartNew();

            //        var endDate = DateTime.UtcNow;
            //        var startDate = endDate.Date.AddYears(-10); // start of today (00:00 UTC)
            //        var unit = "";

            //        // Query step count data
            //        //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);

            //     //   syncstack.IsVisible = true;
            //      //  syncProgress.Progress = 0;

            //        // Start fake progress animation
 

            //        // Run the actual health sync
            //        await Task.Run(async () =>
            //        {
            //           var steps = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);
            //         //  var distance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");
            //           var heartRate = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");
            //        });

            //        // Stop and hide progress
            //        syncProgress.AbortAnimation("ProgressTo");
            //        syncProgress.Progress = 1;
            //        syncstack.IsVisible = false;


            //        //   var data1 = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);

            //        //   var walkingdistance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");

            //        // var hr = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");

            //        //   var resp = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, "count/min");

            //           stopwatch.Stop();

            //                        await Application.Current.MainPage.DisplayAlert(
            //        "Step Data Loaded",
            //        $"Retrieved entries in {stopwatch.Elapsed.TotalSeconds:F2} seconds.",
            //        "OK"
            //        );


            //    }


            //}



   

            }
        catch (Exception ex)
        {

        }
    }

    private async void accessbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            await Navigation.PushAsync(new HealthDataPage(), false);

        }
        catch (Exception ex)
        {

        }
    }

    private async void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {
            //steps changing

            var index = e.NewIndex;

            if (index == 0)
            {

                var dailyTotals = new List<(string Date, double TotalSteps)>();

                for (int i = 0; i >= 0; i--)
                {
                    var day = DateTime.Today.AddDays(-i);
                    var dayStart = day;
                    var dayEnd = day.AddDays(1);

                    var totalStepss = await health.ReadCountAsync(HealthParameter.StepCount, dayStart, dayEnd);

                    dailyTotals.Add((day.ToString("dd/MM/yyyy"), totalStepss));
                }

                var orderedDailyTotals = dailyTotals
                    .OrderBy(d => DateTime.Parse(d.Date))
                    .Select(d => new
                    {
                        Date = d.Date,
                        TotalSteps = Math.Round(d.TotalSteps)
                    }).ToList();

                symptomprogresschart.ItemsSource = orderedDailyTotals;

                if (orderedDailyTotals.Count == 0)
                {
                    // rrchart.IsVisible = false;
                    stepsnodata.IsVisible = true;
                    stepscountlbl.Text = "--";
                    totallbl.Text = "--";
                    return;

                }
                else
                {
                    //  rrchart.IsVisible = true;
                    stepsnodata.IsVisible = false;
                }


                var averageSteps = orderedDailyTotals.Average(x => x.TotalSteps);
                var totalSteps = orderedDailyTotals.Sum(x => x.TotalSteps);


                int roundedsteps = (int)Math.Round((decimal)averageSteps);
                int roundedtotal = (int)Math.Round((double)totalSteps);

                stepscountlbl.Text = roundedsteps.ToString();
                totallbl.Text = roundedtotal.ToString();

                stepsmonthlist.IsVisible = false;
                stepsmonthlist.SelectedItem = null;

            }

            if (index == 1)
            {


                var dailyTotals = new List<(string Date, double TotalSteps)>();

                for (int i = 6; i >= 0; i--)
                {
                    var day = DateTime.Today.AddDays(-i);
                    var dayStart = day;
                    var dayEnd = day.AddDays(1);

                    var totalStepss = await health.ReadCountAsync(HealthParameter.StepCount, dayStart, dayEnd);

                    dailyTotals.Add((day.ToString("dd/MM/yyyy"), totalStepss));
                }

                var orderedDailyTotals = dailyTotals
                    .OrderBy(d => DateTime.Parse(d.Date))
                    .Select(d => new
                    {
                        Date = d.Date,
                        TotalSteps = Math.Round(d.TotalSteps)
                    }).ToList();

                symptomprogresschart.ItemsSource = orderedDailyTotals;

                if (orderedDailyTotals.Count == 0)
                {
                    // rrchart.IsVisible = false;
                    stepsnodata.IsVisible = true;
                    stepscountlbl.Text = "--";
                    totallbl.Text = "--";
                    return;

                }
                else
                {
                    //  rrchart.IsVisible = true;
                    stepsnodata.IsVisible = false;
                }


                var averageSteps = orderedDailyTotals.Average(x => x.TotalSteps);
                var totalSteps = orderedDailyTotals.Sum(x => x.TotalSteps);


                int roundedsteps = (int)Math.Round((decimal)averageSteps);
                int roundedtotal = (int)Math.Round((double)totalSteps);

                stepscountlbl.Text = roundedsteps.ToString();
                totallbl.Text = roundedtotal.ToString();

                stepsmonthlist.IsVisible = false;
                stepsmonthlist.SelectedItem = null;

            }
            else if(index == 2)
            {



                var dailyTotals = new List<(string Date, double TotalSteps)>();

                for (int i = 30; i >= 0; i--)
                {
                    var day = DateTime.Today.AddDays(-i);
                    var dayStart = day;
                    var dayEnd = day.AddDays(1);

                    var totalStepss = await health.ReadCountAsync(HealthParameter.StepCount, dayStart, dayEnd);

                    dailyTotals.Add((day.ToString("dd/MM/yyyy"), totalStepss));
                }

                var orderedDailyTotals = dailyTotals
                    .OrderBy(d => DateTime.Parse(d.Date))
                    .Select(d => new
                    {
                        Date = d.Date,
                        TotalSteps = Math.Round(d.TotalSteps)
                    }).ToList();

                symptomprogresschart.ItemsSource = orderedDailyTotals;

                if (orderedDailyTotals.Count == 0)
                {
                   // rrchart.IsVisible = false;
                    stepsnodata.IsVisible = true;
                    stepscountlbl.Text = "--";
                    totallbl.Text = "--";
                    return;

                }
                else
                {
                  //  rrchart.IsVisible = true;
                    stepsnodata.IsVisible = false;
                }

                var averageSteps = orderedDailyTotals.Average(x => x.TotalSteps);
                var totalSteps = orderedDailyTotals.Sum(x => x.TotalSteps);


                int roundedsteps = (int)Math.Round((decimal)averageSteps);
                int roundedtotal = (int)Math.Round((double)totalSteps);

                stepscountlbl.Text = roundedsteps.ToString();
                totallbl.Text = roundedtotal.ToString();

                stepsmonthlist.IsVisible = true;
                stepsmonthlist.SelectedItem = null;
            }
         



        }
        catch (Exception ex)
        {

        }
    }

    private async void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {

            //heart rate segment

            var index = e.NewIndex;


            if(index == 0)
            {
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);
                var unit = "count/min";

                var data1 = await health.ReadAllAsync(HealthParameter.HeartRate, today, tomorrow, unit);

                var todayDataPoints = data1
                    .Where(d => d.From.HasValue)
                    .Select(d => new
                    {
                        Date = d.From.Value.ToString("HH:mm"),
                        Min = d.Value

                    })
                    .OrderBy(d => d.Date)
                    .ToList();

                // Bind to your chart
                heartratechartdayonly.ItemsSource = todayDataPoints;

                if (todayDataPoints.Count == 0)
                {
                    // rrchart.IsVisible = false;
                    hrnodata.IsVisible = true;
                    hrcountlbl.Text = "--";
                    hrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }

                var weeklyAverageHR = todayDataPoints.Min(x => x.Min);
                var weeklyMaxHR = todayDataPoints.Max(x => x.Min);


                int roundedsteps = (int)Math.Round((double)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                hrcountlbl.Text = roundedsteps.ToString();
                hrtotalbl.Text = roundedtotal.ToString();

                hrweekchart.IsVisible = false;
                hrdaychart.IsVisible = true;
               
            }
            else if(index == 1)
            {

              


                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-7); // start of today (00:00 UTC)
                var unit = "count/min";



                var data1 = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, unit);


                var dailyMinMax = data1
                   .Where(d => d.From.HasValue) // ensure From is not null
                   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                   .Select(g => new
                   {
                       Date = g.Key.ToString("dd/MM/yyyy"),
                       Min = g.Min(d => d.Value),
                       Max = g.Max(d => d.Value)

                   })
                   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                heartratechart.ItemsSource = dailyMinMax;

                if (dailyMinMax.Count == 0)
                {
                    // rrchart.IsVisible = false;
                    hrnodata.IsVisible = true;
                    hrcountlbl.Text = "--";
                    hrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }


                // Take last 7 days
                //  var last7Days = dailyMinMax.Take(7);

                // Calculate average
                var weeklyAverageHR = dailyMinMax.Min(x => x.Min);
                var weeklyMaxHR = dailyMinMax.Max(x => x.Max);


                int roundedsteps = (int)Math.Round((double)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                hrcountlbl.Text = roundedsteps.ToString();
                hrtotalbl.Text = roundedtotal.ToString();


                heartratechart.ItemsSource = dailyMinMax;

                hrdaychart.IsVisible = false;
                hrweekchart.IsVisible = true;



            }
            else if(index == 2)
            {
                hrdaychart.IsVisible = false;


                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-30); // start of today (00:00 UTC)
                var unit = "count/min";



                var data1 = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, unit);


                var dailyMinMax = data1
                   .Where(d => d.From.HasValue) // ensure From is not null
                   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                   .Select(g => new
                   {
                       Date = g.Key.ToString("dd/MM/yyyy"),
                       Min = g.Min(d => d.Value),
                       Max = g.Max(d => d.Value)

                   })
                   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                heartratechart.ItemsSource = dailyMinMax;


                if (dailyMinMax.Count == 0)
                {
                    // rrchart.IsVisible = false;
                    hrnodata.IsVisible = true;
                    hrcountlbl.Text = "--";
                    hrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }

                // Take last 7 days
                //  var last7Days = dailyMinMax.Take(7);

                // Calculate average
                var weeklyAverageHR = dailyMinMax.Min(x => x.Min);
                var weeklyMaxHR = dailyMinMax.Max(x => x.Max);


                int roundedsteps = (int)Math.Round((double)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                hrcountlbl.Text = roundedsteps.ToString();
                hrtotalbl.Text = roundedtotal.ToString();


                heartratechart.ItemsSource = dailyMinMax;

                hrdaychart.IsVisible = false;
                hrweekchart.IsVisible = true;
            }

        }
        catch(Exception ex)
        {

        }
    }

    private async void SfSegmentedControl_SelectionChanged_1(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {


            //rr segment

            var index = e.NewIndex;


            if(index == 0)
            {
                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-1); // start of today (00:00 UTC)
                var unit = "count/min";



                var data11 = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, unit);


                var dailyMinMax = data11
                   .Where(d => d.From.HasValue) // ensure From is not null
                   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                   .Select(g => new
                   {
                       Date = g.Key.ToString("dd/MM/yyyy"),
                       Min = g.Min(d => d.Value),
                       Max = g.Max(d => d.Value)

                   })
                   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                rrchart.ItemsSource = dailyMinMax;

                if (dailyMinMax.Count == 0)
                {
                    rrchart.IsVisible = false;
                    rrnodata.IsVisible = true;
                    rrcountlbl.Text = "--";
                    rrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }

                // Take last 7 days
                //   var last7Days = dailyMinMax.Take(7);

                // Calculate average
                var weeklyAverageHR = dailyMinMax.Min(x => x.Min);
                var weeklyMaxHR = dailyMinMax.Max(x => x.Max);


                int roundedsteps = (int)Math.Round((decimal)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                if(roundedsteps == 0)
                {
                    rrcountlbl.Text = "--";
                }
                else
                {
                    rrcountlbl.Text = roundedsteps.ToString();
                }

                if(roundedtotal == 0)
                {
                    rrtotalbl.Text = "--";
                }
                else
                {
                    rrtotalbl.Text = roundedtotal.ToString();
                }
                   
             
            }
            else if(index == 1)
            {
                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-7); // start of today (00:00 UTC)
                var unit = "count/min";



                var data11 = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, unit);


                var dailyMinMax = data11
                   .Where(d => d.From.HasValue) // ensure From is not null
                   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                   .Select(g => new
                   {
                       Date = g.Key.ToString("dd/MM/yyyy"),
                       Min = g.Min(d => d.Value),
                       Max = g.Max(d => d.Value)

                   })
                   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                rrchart.ItemsSource = dailyMinMax;

                if (dailyMinMax.Count == 0)
                {
                    rrchart.IsVisible = false;
                    rrnodata.IsVisible = true;
                    rrcountlbl.Text = "--";
                    rrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }

                // Take last 7 days
                //var last7Days = dailyMinMax.Take(7);

                // Calculate average
                var weeklyAverageHR = dailyMinMax.Min(x => x.Min);
                var weeklyMaxHR = dailyMinMax.Max(x => x.Max);


                int roundedsteps = (int)Math.Round((decimal)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                if (roundedsteps == 0)
                {
                    rrcountlbl.Text = "--";
                }
                else
                {
                    rrcountlbl.Text = roundedsteps.ToString();
                }

                if (roundedtotal == 0)
                {
                    rrtotalbl.Text = "--";
                }
                else
                {
                    rrtotalbl.Text = roundedtotal.ToString();
                }
            }
            else if(index == 2)
            {
                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-30); // start of today (00:00 UTC)
                var unit = "count/min";



                var data11 = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, unit);


                var dailyMinMax = data11
                   .Where(d => d.From.HasValue) // ensure From is not null
                   .GroupBy(d => d.From.Value.Date) // group by Date only (not including time)
                   .Select(g => new
                   {
                       Date = g.Key.ToString("dd/MM/yyyy"),
                       Min = g.Min(d => d.Value),
                       Max = g.Max(d => d.Value)

                   })
                   .OrderBy(x => DateTime.ParseExact(x.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
                   .ToList();

                rrchart.ItemsSource = dailyMinMax;

                if (dailyMinMax.Count == 0)
                {
                    rrchart.IsVisible = false;
                    rrnodata.IsVisible = true;
                    rrcountlbl.Text = "--";
                    rrtotalbl.Text = "--";
                    return;

                }
                else
                {
                    rrchart.IsVisible = true;
                    rrnodata.IsVisible = false;
                }

                // Take last 7 days
                //var last7Days = dailyMinMax.Take(7);

                // Calculate average
                var weeklyAverageHR = dailyMinMax.Min(x => x.Min);
                var weeklyMaxHR = dailyMinMax.Max(x => x.Max);


                int roundedsteps = (int)Math.Round((decimal)weeklyAverageHR);
                int roundedtotal = (int)Math.Round((double)weeklyMaxHR);

                if (roundedsteps == 0)
                {
                    rrcountlbl.Text = "--";
                }
                else
                {
                    rrcountlbl.Text = roundedsteps.ToString();
                }

                if (roundedtotal == 0)
                {
                    rrtotalbl.Text = "--";
                }
                else
                {
                    rrtotalbl.Text = roundedtotal.ToString();
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void stepsmonthlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var month = e.DataItem as string;


            if (!string.IsNullOrEmpty(month))
            {

                // Parse month string into DateTime (e.g. "June 2025" -> 2025-06-01)
                DateTime selectedMonth = DateTime.ParseExact(month, "MMMM yyyy", CultureInfo.InvariantCulture);

                // Get the start and end of that month
                DateTime monthStart = new DateTime(selectedMonth.Year, selectedMonth.Month, 1);
                DateTime monthEnd = monthStart.AddMonths(1);

                var dailyTotals = new List<(string Date, double TotalSteps)>();

                // Calculate days in the month
                int daysInMonth = (monthEnd - monthStart).Days;

                for (int i = 0; i < daysInMonth; i++)
                {
                    var day = monthStart.AddDays(i);
                    var dayStart = DateTime.SpecifyKind(day, DateTimeKind.Utc);
                    var dayEnd = DateTime.SpecifyKind(day.AddDays(1), DateTimeKind.Utc);

                    if (day > DateTime.Now)
                    {

                    }
                    else
                    {

                        // Assuming 'health.ReadCountAsync' is your step reading method
                        var totalStepss = await health.ReadCountAsync(HealthParameter.StepCount, dayStart, dayEnd);

                        dailyTotals.Add((day.ToString(), totalStepss));
                    }
                }

                // Calculate totals and averages for that month
                var orderedDailyTotals = dailyTotals
                    .OrderBy(d => DateTime.Parse(d.Date))
                    .Select(d => new
                    {
                        Date = DateTime.Parse(d.Date).ToString("dd"),
                        TotalSteps = Math.Round(d.TotalSteps)
                    }).ToList();

                symptomprogresschart.ItemsSource = orderedDailyTotals;

                if (orderedDailyTotals.Count == 0)
                {
                    stepsnodata.IsVisible = true;
                    stepscountlbl.Text = "--";
                    totallbl.Text = "--";
                    return;
                }
                else
                {
                    stepsnodata.IsVisible = false;
                }

                var averageSteps = orderedDailyTotals.Average(x => x.TotalSteps);
                var totalSteps = orderedDailyTotals.Sum(x => x.TotalSteps);

                stepscountlbl.Text = Math.Round(averageSteps).ToString();
                totallbl.Text = Math.Round(totalSteps).ToString();

            }


        }
        catch(Exception ex)
        {

        }
    }
}