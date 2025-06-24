//using Maui.Health;
using System;
using Microsoft.Maui.Controls;
using Plugin.Maui.Health.Enums;
using Plugin.Maui.Health;
using Syncfusion.Maui.ProgressBar;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Core;
//using CloudKit;
using System.Diagnostics;

namespace PeopleWith;

public partial class HealthDataPage : ContentPage
{
    private readonly IHealth health;
    public List<string> q1list = new List<string>();
    public List<string> q2list = new List<string>();
    public List<string> q3list = new List<string>();
    public List<string> q4list = new List<string>();
    public List<string> q5list = new List<string>();
    ObservableCollection<userfitnessdata> userfitnesslist = new ObservableCollection<userfitnessdata>();
    APICalls database = new APICalls();
    //  HealthManager healthManager;
    public HealthDataPage()
	{
		InitializeComponent();

        var Steps = new ObservableCollection<StepProgressBarItem>();

        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });
        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });
        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });
       // Steps.Add(new StepProgressBarItem() { PrimaryText = "" });

        stepbar.ItemsSource = Steps;

        q1list.Add("Increase Steps");
        q1list.Add("Weight Loss");
        q1list.Add("Better Sleep");
        q1list.Add("See Insights and Trends");
        q1list.Add("No Reason");

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


        health = HealthDataProvider.Default;
        //checkhealthdata();
    }
    async void checkhealthdata()
    {
        try
        {

     
            //var hasPermission = await health.CheckPermissionAsync(Health.Enums.HealthParameter.StepCount, Health.Enums.PermissionType.Write);
            //if (hasPermission)
            //{
            //    var stepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);
            //}

            //            var parameters = new[]
            //         {
            //    HealthParameter.StepCount,
            //    HealthParameter.HeartRate,
            //    HealthParameter.MoveTime
            //};

            //foreach (var param in parameters)
            //{
            //    var granted = await health.CheckPermissionAsync(param, PermissionType.Read);
            //    // This will request permission if not already granted
            //    if (!granted)
            //    {
            //        // Optional: handle denial
            //        Console.WriteLine($"{param} permission was denied.");
            //    }
            //}

            var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            if (hasPermission)
            {
                // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);

                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-1); // start of today (00:00 UTC)
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


                // Convert to string
                string stepDataString = string.Join(Environment.NewLine, dailyTotals.Select(d =>
                    $"Date: {d.Date:yyyy-MM-dd}, Total Steps: {d.TotalSteps}"));

                // Display result
                stepslbl.Text = stepDataString;


            }

            }
        catch (Exception ex)
		{

		}
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.BloodGlucose, PermissionType.Read);
            if (hasPermission)
            {
                // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);

                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddDays(-1); // start of today (00:00 UTC)
                var unit = "";

                // Query step count data
                //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);


                var data1 = await health.ReadLatestAsync(HealthParameter.BloodAlcoholContent, startDate, endDate, unit);

              
                hrlbl.Text = data1.ToString();


            }
        }
        catch(Exception ex)
        {

        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var hasPermission = await health.CheckPermissionAsync(HealthParameter.ActiveEnergyBurned, PermissionType.Read);
        if (hasPermission)
        {
            // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);

            var endDate = DateTime.UtcNow;
            var startDate = endDate.Date.AddDays(-1); // start of today (00:00 UTC)
            var unit = "";

            // Query step count data
            //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);


            var data1 = await health.ReadLatestAsync(HealthParameter.ActiveEnergyBurned, startDate, endDate, unit);


            actlbl.Text = data1.ToString();


        }
    }

    private async void stepsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);

            if (hasPermission)
            {

                stepsborder.Stroke = Color.FromArgb("#42c501");
                stepsborder.StrokeThickness = 2;
                stepsbtn.IsVisible = false;
                stepstick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {
            
        }
    }

    private void startbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            if(startbtn.Text == "")
            {
                return;
            }
            else
            {
                //go to health page



            }


        }
        catch(Exception ex)
        {

        }
    }

    private async void walkingbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.DistanceWalkingRunning, PermissionType.Read);

            if (hasPermission)
            {

                walkingborder.Stroke = Color.FromArgb("#42c501");
                walkingborder.StrokeThickness = 2;
                walkingbtn.IsVisible = false;
                walkingtick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void steadybtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.WalkingSteadiness, PermissionType.Read);

            if (hasPermission)
            {

                steadyborder.Stroke = Color.FromArgb("#42c501");
                steadyborder.StrokeThickness = 2;
                steadybtn.IsVisible = false;
                steadytick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void movebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.MoveTime, PermissionType.Read);

            if (hasPermission)
            {

                moveborder.Stroke = Color.FromArgb("#42c501");
                moveborder.StrokeThickness = 2;
                movebtn.IsVisible = false;
                movetick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void hrbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.HeartRate, PermissionType.Read);

            if (hasPermission)
            {

                hrborder.Stroke = Colors.HotPink;
                hrborder.StrokeThickness = 2;
                hrbtn.IsVisible = false;
                hrtick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void electrobtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.ElectrodermalActivity, PermissionType.Read);

            if (hasPermission)
            {

                electroborder.Stroke = Colors.HotPink;
                electroborder.StrokeThickness = 2;
                electrobtn.IsVisible = false;
                electrotick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void bpbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.BloodPressureDiastolic, PermissionType.Read);

            if (hasPermission)
            {
                bpbtn.IsVisible = false;

                if (bp2btn.IsVisible == false)
                {
                    bpborder.Stroke = Colors.HotPink;
                    bpborder.StrokeThickness = 2;
                    bpbtn.IsVisible = false;
                    bptick.IsVisible = true;


                    startbtn.Text = "Get Started";
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void resbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.RespiratoryRate, PermissionType.Read);

            if (hasPermission)
            {

                resborder.Stroke = Color.FromArgb("#009fe3");
                resborder.StrokeThickness = 2;
                resbtn.IsVisible = false;
                restick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void sleepbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.SleepingWristTemperature, PermissionType.Read);

            if (hasPermission)
            {

                sleepborder.Stroke = Color.FromArgb("#CCCC00");
                sleepborder.StrokeThickness = 2;
                sleepbtn.IsVisible = false;
                sleeptick.IsVisible = true;


                startbtn.Text = "Get Started";
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async void bp2btn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var hasPermission = await health.CheckPermissionAsync(HealthParameter.BloodPressureSystolic, PermissionType.Read);

            if (hasPermission)
            {
                bp2btn.IsVisible = false;

                if (bpbtn.IsVisible == false)
                {
                    bpborder.Stroke = Colors.HotPink;
                    bpborder.StrokeThickness = 2;
                    bp2btn.IsVisible = false;
                    bptick.IsVisible = true;


                    startbtn.Text = "Get Started";
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private async Task<List<fitnessfeedback>> FetchFitnessFeedback(
    HealthParameter parameter, DateTime start, DateTime end, string unit)
    {
        try
        {
            var list = new List<fitnessfeedback>();

            var hasPermission = await health.CheckPermissionAsync(parameter, PermissionType.Read);
            if (!hasPermission) return list;


            var data = await health.ReadAllAsync(parameter, start, end, unit);

            list = data.Select(item => new fitnessfeedback
            {
                value = item.Value.ToString(),
                unit = item.Unit,
                source = item.Source,
                datetime = item.From.ToString()
            }).ToList();

            //foreach (var item in data)
            //{
            //    list.Add(new fitnessfeedback
            //    {
            //        value = item.Value.ToString(),
            //        unit = item.Unit,
            //        source = item.Source,
            //        datetime = item.From.ToString()
            //    });
            //}

            return list;
        }
        catch(Exception ex)
        {
            return null;
        }
    }


    private async Task<userfitnessdata> CreateUserFitnessData(DateTime startDate, DateTime endDate)
    {
        try
        {

            var stopwatch = Stopwatch.StartNew();

            var stepTask = FetchFitnessFeedback(HealthParameter.StepCount, startDate, endDate, "");
            var hrTask = FetchFitnessFeedback(HealthParameter.HeartRate, startDate, endDate, "count/min");
            var rrTask = FetchFitnessFeedback(HealthParameter.RespiratoryRate, startDate, endDate, "count/min");

            await Task.WhenAll(stepTask, hrTask, rrTask);

            var userfitness = new userfitnessdata
            {
                userid = Helpers.Settings.UserKey,
                //   stepfeedbacklist = stepTask.Result,
                //   heartratefeedbacklist = hrTask.Result,
                //   respiratoryratefeedbacklist = rrTask.Result,
                stepfeedback = System.Text.Json.JsonSerializer.Serialize(stepTask.Result),
                heartratefeedback = System.Text.Json.JsonSerializer.Serialize(hrTask.Result),
                respiratoryratefeedback = System.Text.Json.JsonSerializer.Serialize(rrTask.Result)
            };


            stopwatch.Stop();
            await Application.Current.MainPage.DisplayAlert(
                "Step Data Loaded",
                $"loading data {stopwatch.Elapsed.TotalSeconds:F2} seconds.",
                "OK"
            );

            //var userfitness = new userfitnessdata
            //{
            //    userid = Helpers.Settings.UserKey,
            //    stepfeedbacklist = await FetchFitnessFeedback(HealthParameter.StepCount, startDate, endDate, ""),
            //    heartratefeedbacklist = await FetchFitnessFeedback(HealthParameter.HeartRate, startDate, endDate, "count/min"),
            //    respiratoryratefeedbacklist = await FetchFitnessFeedback(HealthParameter.RespiratoryRate, startDate, endDate, "count/min")
            //};

            //userfitness.stepfeedback = System.Text.Json.JsonSerializer.Serialize(userfitness.stepfeedbacklist);
            //userfitness.heartratefeedback = System.Text.Json.JsonSerializer.Serialize(userfitness.heartratefeedbacklist);
            //userfitness.respiratoryratefeedback = System.Text.Json.JsonSerializer.Serialize(userfitness.respiratoryratefeedbacklist);

            return userfitness;
        }
        catch(Exception ex)
        {
            return null;
        }
    }


    async void uploadallfitnessdata()
    {
        try
        {

            if (userfitnesslist == null || userfitnesslist.Count == 0)
            {
                NavigationPage.SetHasNavigationBar(this, false);
                var stopwatch = Stopwatch.StartNew();

                var endDate = DateTime.UtcNow;
                var startDate = endDate.Date.AddMonths(-6);

                var userFitness = await CreateUserFitnessData(startDate, endDate);


                //add in questions and date time 
                userFitness.distancefeedback = DateTime.Now.ToString("dd/MM/yyyy HH:mm");


                var result = await database.InsertUserFitness(userFitness);

                if (result == null)
                {
                    // Try again with smaller range
                    startDate = endDate.Date.AddYears(-1);
                    userFitness = await CreateUserFitnessData(startDate, endDate);

                    stopwatch.Stop();
                    await Application.Current.MainPage.DisplayAlert(
                        "Step Data Loaded",
                        $"Retrieved entries in {stopwatch.Elapsed.TotalSeconds:F2} seconds.",
                        "OK"
                    );

                    stopwatch.Start();

                    result = await database.InsertUserFitness(userFitness);
                }

                if (result != null)
                {
                    loader.IsVisible = false;
                    migimg.IsVisible = true;
                    mdlbl.Text = "Successfully Uploaded";
                    md2lbl.Text = "Data successfully uploaded";
                    Preferences.Set("fitnessdata", "Activated");

                    await Task.Delay(2000);
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
                }
                else
                {
                    loader.IsVisible = false;
                    migimgwrong.IsVisible = true;
                    mdlbl.Text = "Something went wrong";
                    md2lbl.Text = "There seems to be an issue with uploading your data please contact support at PeopleWith";

                    await Task.Delay(2000);
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
                }

                stopwatch.Stop();
                await Application.Current.MainPage.DisplayAlert(
                    "Step Data Loaded",
                    $"Retrieved entries in {stopwatch.Elapsed.TotalSeconds:F2} seconds.",
                    "OK"
                );
            }





            //check if there is any user fitness 

            // Retrieve all user feedback data
            // userfitnesslist = await database.GetUserFitnessData();



            //        if (userfitnesslist == null || userfitnesslist.Count == 0)
            //        {
            //            NavigationPage.SetHasNavigationBar(this, false);
            //         //   syncstack.IsVisible = true;
            //        //    mainstack.IsVisible = false;

            //            //add it a new user fitness


            //            var newuserfitness = new userfitnessdata();

            //            newuserfitness.userid = Helpers.Settings.UserKey;

            //            var stopwatch = Stopwatch.StartNew();

            //            var endDate = DateTime.UtcNow;
            //            var startDate = endDate.Date.AddYears(-10); // start of today (00:00 UTC)
            //            var unit = "";

            //            newuserfitness.stepfeedbacklist = new ObservableCollection<fitnessfeedback>();
            //            newuserfitness.heartratefeedbacklist = new ObservableCollection<fitnessfeedback>();
            //            newuserfitness.respiratoryratefeedbacklist = new ObservableCollection<fitnessfeedback>();


            //            //step data

            //            var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            //            if (hasPermission)
            //            {
            //                // StepsCount = await health.ReadCountAsync(Enums.HealthParameter.StepCount, DateTime.Now.AddDays(-1), DateTime.Now);

            //                // Query step count data
            //                //  var data = await health.ReadLatestAsync(HealthParameter.StepCount, startDate, endDate, unit);

            //                //   syncstack.IsVisible = true;
            //                //  syncProgress.Progress = 0;

            //                // Start fake progress animation


            //                // Run the actual health sync
            //                await Task.Run(async () =>
            //                {
            //                    //  var steps = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);
            //                    //  var distance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");
            //                    // var heartRate = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");
            //                });

            //                // Stop and hide progress
            //                ////  syncProgress.AbortAnimation("ProgressTo");
            //                // syncProgress.Progress = 1;
            //                // syncstack.IsVisible = false;


            //                var data1 = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);



            //                foreach (var item in data1)
            //                {
            //                    newuserfitness.stepfeedbacklist.Add(new fitnessfeedback
            //                    {
            //                        // Map properties from `item` to `fitnessfeedback`
            //                        // e.g., Steps = item.Value, Time = item.Date

            //                        value = item.Value.ToString(),
            //                        unit = item.Unit,
            //                        source = item.Source,
            //                        //  name = item.Description,
            //                        datetime = item.From.ToString()

            //                    });
            //                }

            //                string json = System.Text.Json.JsonSerializer.Serialize(newuserfitness.stepfeedbacklist);
            //                newuserfitness.stepfeedback = json;

            //            }

            //            var hasPermissionhr = await health.CheckPermissionAsync(HealthParameter.HeartRate, PermissionType.Read);

            //            if (hasPermissionhr)
            //            {


            //                var hr = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");



            //                foreach (var item in hr)
            //                {
            //                    newuserfitness.heartratefeedbacklist.Add(new fitnessfeedback
            //                    {
            //                        // Map properties from `item` to `fitnessfeedback`
            //                        // e.g., Steps = item.Value, Time = item.Date

            //                        value = item.Value.ToString(),
            //                        unit = item.Unit,
            //                        source = item.Source,
            //                        //  name = item.Description,
            //                        datetime = item.From.ToString()

            //                    });
            //                }

            //                string jsonhr = System.Text.Json.JsonSerializer.Serialize(newuserfitness.heartratefeedbacklist);
            //                newuserfitness.heartratefeedback = jsonhr;
            //            }

            //            ////   var walkingdistance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");


            //            var hasPermissionrr = await health.CheckPermissionAsync(HealthParameter.RespiratoryRate, PermissionType.Read);


            //            if (hasPermissionrr)
            //            {
            //                var resp = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, "count/min");

            //                foreach (var item in resp)
            //                {
            //                    newuserfitness.respiratoryratefeedbacklist.Add(new fitnessfeedback
            //                    {
            //                        // Map properties from `item` to `fitnessfeedback`
            //                        // e.g., Steps = item.Value, Time = item.Date

            //                        value = item.Value.ToString(),
            //                        unit = item.Unit,
            //                        source = item.Source,
            //                        //  name = item.Description,
            //                        datetime = item.From.ToString()

            //                    });
            //                }


            //                string jsonres = System.Text.Json.JsonSerializer.Serialize(newuserfitness.respiratoryratefeedbacklist);
            //                newuserfitness.respiratoryratefeedback = jsonres;
            //            }







            //            // string newsymJson = System.Text.Json.JsonSerializer.Serialize(data1);
            //            // newuserfitness.stepfeedback = newsymJson;


            //            // string newsymJsonhr = System.Text.Json.JsonSerializer.Serialize(hr);
            //            // newuserfitness.heartratefeedback = newsymJsonhr;




            //            var result = await database.InsertUserFitness(newuserfitness);



            //            if (result == null)
            //            {

            //                //try it again with less years

            //                var newuserfitness1 = new userfitnessdata();

            //                newuserfitness1.userid = Helpers.Settings.UserKey;



            //                var endDate1 = DateTime.UtcNow;
            //                var startDate1 = endDate1.Date.AddYears(-2); // start of today (00:00 UTC)
            //                var unit1 = "";

            //                newuserfitness1.stepfeedbacklist = new ObservableCollection<fitnessfeedback>();
            //                newuserfitness1.heartratefeedbacklist = new ObservableCollection<fitnessfeedback>();
            //                newuserfitness1.respiratoryratefeedbacklist = new ObservableCollection<fitnessfeedback>();


            //                //step data

            //                var hasPermission1 = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            //                if (hasPermission1)
            //                {

            //                    var data1 = await health.ReadAllAsync(HealthParameter.StepCount, startDate, endDate, unit);



            //                    foreach (var item in data1)
            //                    {
            //                        newuserfitness1.stepfeedbacklist.Add(new fitnessfeedback
            //                        {
            //                            // Map properties from `item` to `fitnessfeedback`
            //                            // e.g., Steps = item.Value, Time = item.Date

            //                            value = item.Value.ToString(),
            //                            unit = item.Unit,
            //                            source = item.Source,
            //                            //  name = item.Description,
            //                            datetime = item.From.ToString()

            //                        });
            //                    }

            //                    string json = System.Text.Json.JsonSerializer.Serialize(newuserfitness1.stepfeedbacklist);
            //                    newuserfitness1.stepfeedback = json;

            //                }

            //                var hasPermissionhr1 = await health.CheckPermissionAsync(HealthParameter.HeartRate, PermissionType.Read);

            //                if (hasPermissionhr1)
            //                {


            //                    var hr = await health.ReadAllAsync(HealthParameter.HeartRate, startDate, endDate, "count/min");



            //                    foreach (var item in hr)
            //                    {
            //                        newuserfitness1.heartratefeedbacklist.Add(new fitnessfeedback
            //                        {
            //                            // Map properties from `item` to `fitnessfeedback`
            //                            // e.g., Steps = item.Value, Time = item.Date

            //                            value = item.Value.ToString(),
            //                            unit = item.Unit,
            //                            source = item.Source,
            //                            //  name = item.Description,
            //                            datetime = item.From.ToString()

            //                        });
            //                    }

            //                    string jsonhr = System.Text.Json.JsonSerializer.Serialize(newuserfitness1.heartratefeedbacklist);
            //                    newuserfitness1.heartratefeedback = jsonhr;
            //                }

            //                ////   var walkingdistance = await health.ReadAllAsync(HealthParameter.DistanceWalkingRunning, startDate, endDate, "m");


            //                var hasPermissionrr1 = await health.CheckPermissionAsync(HealthParameter.RespiratoryRate, PermissionType.Read);


            //                if (hasPermissionrr1)
            //                {
            //                    var resp = await health.ReadAllAsync(HealthParameter.RespiratoryRate, startDate, endDate, "count/min");

            //                    foreach (var item in resp)
            //                    {
            //                        newuserfitness1.respiratoryratefeedbacklist.Add(new fitnessfeedback
            //                        {
            //                            // Map properties from `item` to `fitnessfeedback`
            //                            // e.g., Steps = item.Value, Time = item.Date

            //                            value = item.Value.ToString(),
            //                            unit = item.Unit,
            //                            source = item.Source,
            //                            //  name = item.Description,
            //                            datetime = item.From.ToString()

            //                        });
            //                    }


            //                    string jsonres = System.Text.Json.JsonSerializer.Serialize(newuserfitness1.respiratoryratefeedbacklist);
            //                    newuserfitness1.respiratoryratefeedback = jsonres;
            //                }





            //                var result1 = await database.InsertUserFitness(newuserfitness1);

            //                if (result1 != null)
            //                {
            //                    loader.IsVisible = false;
            //                    migimg.IsVisible = true;
            //                    mdlbl.Text = "Successfully Uploaded";
            //                    md2lbl.Text = "Data successfully uploaded";
            //                    Preferences.Set("fitnessdata", "Activated");


            //                    await Task.Delay(2000);
            //                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
            //                }
            //                else
            //                {

            //                    loader.IsVisible = false;
            //                    migimgwrong.IsVisible = true;
            //                    mdlbl.Text = "Something went wrong";
            //                    md2lbl.Text = "There seems to be an issue with uploading your data please contact support at PeopleWith";

            //                    await Task.Delay(2000);
            //                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
            //                }

            //            }
            //            else
            //            {

            //                loader.IsVisible = false;
            //                migimg.IsVisible = true;
            //                mdlbl.Text = "Successfully Uploaded";
            //                md2lbl.Text = "Data successfully uploaded";
            //                Preferences.Set("fitnessdata", "Activated");


            //                await Task.Delay(2000);
            //                Application.Current.MainPage = new NavigationPage(new MainDashboard());

            //            }






            //                stopwatch.Stop();

            //                await Application.Current.MainPage.DisplayAlert(
            //"Step Data Loaded",
            //$"Retrieved entries in {stopwatch.Elapsed.TotalSeconds:F2} seconds.",
            //"OK"
            //);





            //        }





        }
        catch (Exception ex)
        {
            var error = ex.StackTrace.ToString();
        }
    }


    private void cancelbtn_Clicked(object sender, EventArgs e)
    {
        //cancel button 

        try
        {

        }
        catch (Exception ex)
        {

        }
    }

    private void nextbtn_Clicked(object sender, EventArgs e)
    {
        //next button 

        try
        {
            if(stepsbtn.IsVisible == true && hrbtn.IsVisible == true && resbtn.IsVisible == true)
            {
                Vibration.Vibrate();
                return;
            }

            if(sourcestack.IsVisible)
            {
                sourcestack.IsVisible = false;
                syncstack.IsVisible = true;
                stepbar.ActiveStepIndex = 1;
                uploadallfitnessdata();
            }
            else if(syncstack.IsVisible)
            {
                syncstack.IsVisible = false;

                stepbar.ActiveStepIndex = 2;
                infostack.IsVisible = true;
                mainscrollview.ScrollToAsync(0, 0, false);
                nextbtn.IsVisible = false;

            }
            else if(infostack.IsVisible)
            {
                infostack.IsVisible = false;
                topgrid.IsVisible = false;
                bottomstack.IsVisible = false;

                loadingstack.IsVisible = true;
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            infostack.IsVisible = false;
            topgrid.IsVisible = false;
            bottomstack.IsVisible = false;

            loadingstack.IsVisible = true;
        }
        catch(Exception ex)
        {

        }
    }
}