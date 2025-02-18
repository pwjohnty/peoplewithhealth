using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class userdiet
    {

        [System.Text.Json.Serialization.JsonIgnore]
        public string id { get; set; }
        public bool deleted { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime createdAt { get; set; }
        public string userid { get; set; }
        public string dietid { get; set; }
        public string diettitle { get; set; }      
        public string datestarted { get; set; }
        public string dateended { get; set; }
        public bool active { get; set; }

        [JsonConverter(typeof(DietNotesConverter))]
        public ObservableCollection<userNotesFeedback> notes { get; set; }


    }

    public class rawuserdiet
    {
        public string id { get; set; }
        public bool deleted { get; set; }
        public DateTime createdAt { get; set; }
        public string userid { get; set; }
        public string dietid { get; set; }
        public string diettitle { get; set; }
        public string datestarted { get; set; }
        public string dateended { get; set; }
        public bool active { get; set; }
        public string notes { get; set; }
    }

    public class userNotesFeedback
    {
        public string id { get; set; }
        public string deleted { get; set; }
        public string datetime { get; set; }
        public string notes { get; set; }
    }

    public class APIUserDietResponse
    {
        public ObservableCollection<userdiet> Value { get; set; }
    }

    public class SingleUserDiet
    {
        public ObservableCollection<rawuserdiet> Value { get; set; }
    }
}
