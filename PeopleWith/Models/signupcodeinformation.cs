using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class signupcodeinformation
    {

        public string title { get; set; }
        public string description { get; set; }
        public string link { get; set; }
        public string thumbnail { get; set; }

        public string type { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public ImageSource img { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string VideoID { get; set; }

        //private string DecodeTrademark(string input)
        //{
        //    return input?.Replace("\\u00AE", "®");
        //}

        //string EncodeTrademark(string input)
        //{
        //    return input?.Replace("®", "\\u00AE");
        //}

    }

}
