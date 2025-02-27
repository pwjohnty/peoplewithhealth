using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class exercise
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public string exerciseid { get; set; }
        public bool deleted { get; set; }
        public string exercisetitle { get; set; }
        public string exercisedescription { get; set; }
        public string exerciseinformation { get; set; }
        public string exerciseunit { get; set; }
        public string grouping { get; set; }
        public string externalid { get; set; }
        public string active { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public string ShortGroup { get; set; }
    }

    public class APIExerciseResponse
    {
        public ObservableCollection<exercise> Value { get; set; }
    }
}
