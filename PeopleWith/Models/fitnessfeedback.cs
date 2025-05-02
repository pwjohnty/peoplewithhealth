using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class fitnessfeedback
    {
        public string id { get; set; }

        public string name { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
        public string label { get; set; }

        public string datetime { get; set; }

        public string source { get; set; }
         
        public string context { get; set; }

    }

}
