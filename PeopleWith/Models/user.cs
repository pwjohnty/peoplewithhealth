using Microsoft.Datasync.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PeopleWith
{
    /// <summary>
    /// User Object
    /// </summary>
    public class user   
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string userid { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public DateTime UpdatedAt { get; set; }
        //public string Version { get; set; }
        public bool deleted { get; set; }
        public string firstname { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string gender { get; set; }
        public string registrationstatus { get; set; }
        public string dateofbirth { get; set; }
        public string ethnicity { get; set; }
        public string signupcodeid { get; set; }
        public string referral { get; set; }
        public string deviceos { get; set; }
        public string devicemodel { get; set; }
        public string pushnotifications { get; set; }
        public bool? emailnotifications { get; set; }
        public string externalpatientid { get; set; }
        public string primarycareid { get; set; }
        public string secondarycareid { get; set; }
        public string inviteid { get; set; }
        public string registrycondition { get; set; }
        public string activationtimestamp { get; set; }
        public string validationcode { get; set; }
        public string userpin { get; set; }
        public bool? usermigrated { get; set; }

        public bool? biometrics { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string SettingsTitle { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public string SettingsItem { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public bool pintoggled { get; set; }

    }

    public class APIUserResponse
    {
        public ObservableCollection<user> Value { get; set; }
    }

}
