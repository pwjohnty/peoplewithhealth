using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleWith;

[assembly: Dependency(typeof(HealthKitService))]
namespace PeopleWith
{
    internal class HealthKitService : IHealthKitService
    {
        public async Task<bool> RequestAuthorizationAsync()
        {
            var manager = new HealthKitManager();
            return await manager.RequestAuthorizationAsync();
        }

    }
}
