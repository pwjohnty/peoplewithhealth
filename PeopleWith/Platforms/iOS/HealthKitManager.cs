using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using HealthKit;

namespace PeopleWith
{
    public class HealthKitManager
    {
        private readonly HKHealthStore healthStore;

        public HealthKitManager()
        {
            healthStore = new HKHealthStore();
        }

        public async Task<bool> RequestAuthorizationAsync()
        {
            if (!HKHealthStore.IsHealthDataAvailable)
                return false;

            var readTypes = new NSSet(
                HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount),
                HKQuantityType.Create(HKQuantityTypeIdentifier.HeartRate)
            );

            var writeTypes = new NSSet(
                HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount)
            );

            var success = await healthStore.RequestAuthorizationToShareAsync(writeTypes, readTypes);
            return success.Item1;
        }
    }
}
