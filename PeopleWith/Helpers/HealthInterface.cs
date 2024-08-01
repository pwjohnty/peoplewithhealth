using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public interface Healthinterface
    {
        void GetHealthPermissionAsync(Action<bool> completion);
        void FetchSteps(Action<double> completionHandler);
    }
}
