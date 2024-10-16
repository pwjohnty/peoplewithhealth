using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class videos
    {
        public string videoid { get; set; }
        public bool deleted { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string thumbnail { get; set; }
        public string filename { get; set; }
        public string category { get; set; }
        public string signupcodeid { get; set; }
        public string referral { get; set; }
        public string dateadded { get; set; }
        public string lenght { get; set; }
        public bool launch { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string dateandlength { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string subtitleshort { get; set; }
    }

    public class ApiResponseVideos
    {
        public ObservableCollection<videos> Value { get; set; }
    }
}
