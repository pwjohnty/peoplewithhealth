using Foundation;
using HealthKit;
using PeopleWith.Platforms.iOS;
using System.Threading.Tasks;
using PeopleWith;

[assembly: Dependency(typeof(HealthPermissionServiceiOS))]
namespace PeopleWith
{
    public class HealthPermissionServiceiOS : IHealthPermissionService
    {
        private readonly HKHealthStore healthStore = new HKHealthStore();

        public async Task<bool> RequestHealthPermissionsAsync()
        {

            var tcs = new TaskCompletionSource<bool>();



            // Define what data you want to read
            var readTypes = new NSSet(
                HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount),
                HKQuantityType.Create(HKQuantityTypeIdentifier.HeartRate),
                HKQuantityType.Create(HKQuantityTypeIdentifier.ActiveEnergyBurned)
            );

            var success = await healthStore.RequestAuthorizationToShareAsync(
        null,
         readTypes);
    

            // Await the task to return the result
            return await tcs.Task;




        }

   
    }
}
