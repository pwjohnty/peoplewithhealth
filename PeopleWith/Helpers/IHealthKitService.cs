using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public interface IHealthKitService
    {

        Task<bool> RequestAuthorizationAsync();
     //   Task<bool> RequestAuthorization();
     //   Task<double> GetStepCount(DateTime startDate, DateTime endDate);
    }
}
