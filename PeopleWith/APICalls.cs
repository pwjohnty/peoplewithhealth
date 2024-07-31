using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class APICalls
    {
        //add the names of the api followed with the url connection
        public const string Checkuseremail = "https://pwdevapi.peoplewith.com/api/user?$filter=email%20eq%20";
        public const string AddCrash = "https://pwdevapi.peoplewith.com/api/crashlog";
        public const string CheckSignUpCode = "https://pwdevapi.peoplewith.com/api/signupcode?$filter=signupcodeid%20eq%20";


    }
}
