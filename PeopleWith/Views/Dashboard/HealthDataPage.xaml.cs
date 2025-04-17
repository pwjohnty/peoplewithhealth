//using Maui.Health;
using System;
using Microsoft.Maui.Controls;
using Plugin.Maui.Health.Enums;
using Plugin.Maui.Health;

namespace PeopleWith;

public partial class HealthDataPage : ContentPage
{
    private readonly IHealth health;
    //  HealthManager healthManager;
    public HealthDataPage()
	{
		InitializeComponent();


        health = HealthDataProvider.Default;
        checkhealthdata();
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
}