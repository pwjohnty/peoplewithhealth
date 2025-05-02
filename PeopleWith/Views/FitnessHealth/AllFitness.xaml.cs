using Plugin.Maui.Health.Enums;
using Plugin.Maui.Health;
using System.Net;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

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
    public AllFitness()
	{
		InitializeComponent();

        health = HealthDataProvider.Default;

        q1list.Add("Increase Steps");
        q1list.Add("Weight Loss");
        q1list.Add("Better Sleep");
        q1list.Add("See Insights and Trends");

        q1.ItemsSource = q1list;

        q2list.Add("0-2000");
        q2list.Add("2000-4000");
        q2list.Add("4000-6000");
        q2list.Add("6000-8000");
        q2list.Add("8000-10000");
        q2list.Add("10000-12000");
        q2list.Add("12000-15000");
        q2list.Add("15000+");

        q2.ItemsSource = q2list;

        q3list.Add("0-1");
        q3list.Add("1-3");
        q3list.Add("3-5");
        q3list.Add("5-7");
        q3list.Add("8+");

        q3.ItemsSource = q3list;


        q4list.Add("Yes");
        q4list.Add("No");
   

        q4.ItemsSource = q4list;



        q5list.Add("Yes");
        q5list.Add("No");


        q5.ItemsSource = q5list;


        uploadallfitnessdata();

        getstepdata();
    }
    async void getstepdata()
    {
        try
        {

            var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            if (hasPermission)
            {
                // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);

                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-7); // start of today (00:00 UTC)
                var unit = "";

                // Query step count data
                //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);


                var data1 = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);

                var dailyTotals = data1
  .GroupBy(d => d.From?.ToString("dd/MM/yyyy"))
    .Select(g => new
    {
        Date = g.Key,
        TotalSteps = g.Sum(d => d.Value)
    })
    .OrderBy(x => x.Date);

                symptomprogresschart.ItemsSource = dailyTotals;


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



            if(userfitnesslist == null || userfitnesslist.Count == 0)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                syncstack.IsVisible = true;
                mainstack.IsVisible = false;

                //add it a new user fitness


                var newuserfitness = new userfitnessdata();

                newuserfitness.userid = Helpers.Settings.UserKey;


                //step data

                var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
                if (hasPermission)
                {
                    // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);
                    var stopwatch = Stopwatch.StartNew();

                    var endDate = DateTime.UtcNow;
                    var startDate = endDate.Date.AddYears(-10); // start of today (00:00 UTC)
                    var unit = "";

                    // Query step count data
                    //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);

                 //   syncstack.IsVisible = true;
                  //  syncProgress.Progress = 0;

                    // Start fake progress animation
 

                    // Run the actual health sync
                    await Task.Run(async () =>
                    {
                       var steps = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);
                     //  var distance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");
                       var heartRate = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");
                    });

                    // Stop and hide progress
                    syncProgress.AbortAnimation("ProgressTo");
                    syncProgress.Progress = 1;
                    syncstack.IsVisible = false;


                    //   var data1 = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);

                    //   var walkingdistance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");

                    // var hr = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");

                    //   var resp = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, "count/min");

                       stopwatch.Stop();

                                    await Application.Current.MainPage.DisplayAlert(
                    "Step Data Loaded",
                    $"Retrieved entries in {stopwatch.Elapsed.TotalSeconds:F2} seconds.",
                    "OK"
                    );


                }


            }



   

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
}