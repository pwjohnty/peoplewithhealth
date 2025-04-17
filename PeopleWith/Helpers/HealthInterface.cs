using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public interface Healthinterface
    {

        Task<bool> GetHealthPermissionAsync();
        Task<double> FetchStepsAsync();
        //   void GetHealthPermissionAsync(Action<bool> completion);
        // void FetchSteps(Action<double> completionHandler);
    }
}
