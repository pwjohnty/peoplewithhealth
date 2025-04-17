using Foundation;
using HealthKit;
using PeopleWith;
using PeopleWith; // Replace with your actual namespace

[assembly: Dependency(typeof(IHealthService))]
namespace PeopleWith
{
    public class IHealthService : Healthinterface
    {
        public HKHealthStore HealthStore { get; set; }

        NSSet DataTypesToWrite
        {
            get
            {
                return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {

                });
            }
        }

        NSSet DataTypesToRead
        {
            get
            {
                return NSSet.MakeNSObjectSet<HKObjectType>(new HKObjectType[] {
                  //  HKQuantityType.Create(HKQuantityTypeIdentifier.Height),
                  //  HKCharacteristicType.Create(HKCharacteristicTypeIdentifier.DateOfBirth),
                    HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount),
                   // HKQuantityType.Create(HKQuantityTypeIdentifier.DistanceWalkingRunning),
                    //HKQuantityType.Create(HKQuantityTypeIdentifier.AppleExerciseTime),
                   // HKQuantityType.Create(HKQuantityTypeIdentifier.ActiveEnergyBurned),
                   // HKQuantityType.Create(HKQuantityTypeIdentifier.HeartRate)
                });
            }
        }

        public async Task<bool> GetHealthPermissionAsync()
        {
            try
            {


                var tcs = new TaskCompletionSource<bool>();

                if (HKHealthStore.IsHealthDataAvailable)
                {
                    HealthStore = new HKHealthStore();
                    HealthStore.RequestAuthorizationToShare(DataTypesToWrite, DataTypesToRead, (authorized, error) =>
                    {
                        tcs.SetResult(authorized);
                    });
                }
                else
                {
                    tcs.SetResult(false);
                }

                return await tcs.Task;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<double> FetchStepsAsync()
        {
            var tcs = new TaskCompletionSource<double>();

            var startDate = DateTime.Today;
            var endDate = DateTime.Now;
            var stepsQuantityType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);
            var predicate = HKQuery.GetPredicateForSamples((NSDate)startDate, (NSDate)endDate, HKQueryOptions.StrictStartDate);

            var query = new HKStatisticsQuery(stepsQuantityType, predicate, HKStatisticsOptions.CumulativeSum,
                (resultQuery, results, error) =>
                {
                    if (error != null)
                    {
                        tcs.SetResult(0.0);
                        return;
                    }

                    var totalSteps = results.SumQuantity() ?? HKQuantity.FromQuantity(HKUnit.Count, 0.0);
                    tcs.SetResult(totalSteps.GetDoubleValue(HKUnit.Count));
                });

            HealthStore.ExecuteQuery(query);

            return await tcs.Task;
        }

        //public void GetHealthPermissionAsync(Action<bool> completion)
        //{
        //    try
        //    {


        //        if (HKHealthStore.IsHealthDataAvailable)
        //        {
        //            HealthStore = new HKHealthStore();
        //            HealthStore.RequestAuthorizationToShare(DataTypesToWrite, DataTypesToRead, (bool authorized, NSError error) =>
        //            {
        //                completion(authorized);
        //            });
        //        }
        //        else
        //        {
        //            completion(false);
        //        }
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //}

        //public void FetchSteps(Action<double> completionHandler)
        //{
        //    try
        //    {
        //        var calendar = NSCalendar.CurrentCalendar;
        //        var startDate = DateTime.Today;
        //        var endDate = DateTime.Now;
        //        var stepsQuantityType = HKQuantityType.Create(HKQuantityTypeIdentifier.StepCount);

        //        var predicate = HKQuery.GetPredicateForSamples((NSDate)startDate, (NSDate)endDate, HKQueryOptions.StrictStartDate);

        //        var query = new HKStatisticsQuery(stepsQuantityType, predicate, HKStatisticsOptions.CumulativeSum,
        //                        (HKStatisticsQuery resultQuery, HKStatistics results, NSError error) =>
        //                        {
        //                            if (error != null && completionHandler != null)
        //                                completionHandler(0.0f);

        //                            var totalSteps = results.SumQuantity();
        //                            if (totalSteps == null)
        //                                totalSteps = HKQuantity.FromQuantity(HKUnit.Count, 0.0);

        //                            completionHandler(totalSteps.GetDoubleValue(HKUnit.Count));
        //                        });
        //        HealthStore.ExecuteQuery(query);
        //    }
        //    catch(Exception e)
        //    {

        //    }
        //}
    }
}
