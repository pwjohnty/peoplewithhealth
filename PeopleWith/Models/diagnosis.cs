using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class diagnosis
    {
        string diagnosisid;
        bool deleted;
        string title;
        string description;
        string status;
        string userid;
        string classification;
        string diagnosisinformation;
        string grouping;

        [JsonProperty(PropertyName = "diagnosisid")]
        public string Diagnosisid
        {
            get { return diagnosisid; }
            set { diagnosisid = value; }
        }
        [JsonProperty(PropertyName = "deleted")]
        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        [JsonProperty(PropertyName = "title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        [JsonProperty(PropertyName = "description")]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [JsonProperty(PropertyName = "status")]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        [JsonProperty(PropertyName = "userid")]
        public string Userid
        {
            get { return userid; }
            set { userid = value; }
        }


        [JsonProperty(PropertyName = "classification")]
        public string Classification
        {
            get { return classification; }
            set { classification = value; }
        }


        [JsonProperty(PropertyName = "diagnosisinformation")]
        public string Diagnosisinformation
        {
            get { return diagnosisinformation; }
            set { diagnosisinformation = value; }
        }


        [JsonProperty(PropertyName = "grouping")]
        public string Grouping
        {
            get { return grouping; }
            set { grouping = value; }
        }

    }
}
