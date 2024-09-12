using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class allergies
    {
        string allergyid;
        bool deleted;
        string title;
        string description;
        string allergyinformation;
        string descriptor;

        [JsonProperty(PropertyName = "allergyid")]
        public string Allergyid
        {
            get { return allergyid; }
            set { allergyid = value; }
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

        [JsonProperty(PropertyName = "allergyinformation")]
        public string Allergyinformation
        {
            get { return allergyinformation; }
            set { allergyinformation = value; }
        }
        [JsonProperty(PropertyName = "descriptor")]
        public string Descriptor
        {
            get { return descriptor; }
            set { descriptor = value; }
        }
    }
}
