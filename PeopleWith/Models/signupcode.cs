﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class signupcode
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        //public string createdAt { get; set; }
        //public string updatedAt { get; set; }
        //public string version { get; set; }
        //public bool deleted { get; set; }
        public string signupcodeid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string diagnosis { get; set; }
        public string medications { get; set; }
        public string symptoms { get; set; }
        public bool registrationconsent { get; set; }
        public bool additionalconsent { get; set; }
        public bool onscreenconsent { get; set; }
        public string onboardingurl { get; set; }
        public bool initialquestions { get; set; }
        public string referral { get; set; }
        public bool research { get; set; }

        public string logofilename { get; set; }

        public string externalidentifier { get; set; }

        public string dashboardimage { get; set; }

        public string sitetitle { get; set; }

        public bool studyopen { get; set; }

        public string conditiondetails { get; set; }

        public string signupcodeinformation { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ObservableCollection<signupcodeinformation> signupcodeinfolist { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public string shortdescription { get; set; }

        public string appdescription { get; set; }

    }

    public class ApiResponseSignUpCode
    {
        public ObservableCollection<signupcode> Value { get; set; }
    }
}
