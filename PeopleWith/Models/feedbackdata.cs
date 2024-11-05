using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class feedbackdata
    {

  
        public string datetime { get; set; }
        public string action { get; set; }
        public string value { get; set; }
        public string label { get; set; }

        public string medicationfeedbackid { get; set; }

        public ObservableCollection<userfeedback> symptomlist { get; set; }

    }

}
