using PeopleWith;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleWith.Platforms.Android;
using Android.Bluetooth;
using static Android.App.DownloadManager;
using Android.Health.Connect;
using Android.Webkit;
using Android.Content;

[assembly: Dependency(typeof(HealthData))]
namespace PeopleWith.Platforms.Android
{
    public class HealthData : Healthinterface
    {
        //private readonly Context _context;
       // private readonly HealthConnectClient _healthConnectClient;

        public HealthData()
        {
            //_context = Android.App.Application.Context;
            //_healthConnectClient = HealthConnectClient.GetClient(_context);
        }
        public void GetHealthPermissionAsync(Action<bool> completion)
        {
            //Task.Run(async () =>
            //{
            //    var request = new PermissionRequest.Builder()
            //        .AddReadPermission(HealthDataTypes.StepCount)
            //        .Build();

            //    var result = await _healthConnectClient.RequestPermissionsAsync(request);
            //    completion(result.IsSuccess);
            //});
            
        }

        public void FetchSteps(Action<double> completionHandler)
        {
            //Task.Run(async () =>
            //{
            //    var query = new DataReadRequest.Builder()
            //        .SetDataType(HealthDataTypes.StepCount)
            //        .Build();

            //    var response = await _healthConnectClient.GetDataAsync(query);

            //    double totalSteps = 0;

            //    foreach (var record in response)
            //    {
            //        totalSteps += record.StepCount;
            //    }

            //    completionHandler(totalSteps);
            //});
        }

    }
}
