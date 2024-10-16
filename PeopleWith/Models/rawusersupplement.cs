using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class rawusersupplement
    {
        public string id { get; set; }
        public string userid { get; set; }
        public string supplementid { get; set; }
        public string supplementtitle { get; set; }
        public string schedule { get; set; }
        public string preparation { get; set; }
        public string formulation { get; set; }
        public string unit { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string frequency { get; set; }
        public string diagnosis { get; set; }
        public string status { get; set; }
        public string feedback { get; set; }
        public string details { get; set; }
        public bool deleted { get; set; }

        public string supplementquestions { get; set; }
    }
}
