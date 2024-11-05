using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
        public class userquestionnaireresponse
        {
            public string questionid { get; set; }
            public ObservableCollection<answers> answer { get; set; }
        }
        public class answers
        {
            public string answerid { get; set; }
            public string answervalue { get; set; }
            public string answeroptions { get; set; }
        }

  
    
}
