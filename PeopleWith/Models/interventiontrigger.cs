 using Newtonsoft.Json;
using System.Collections.ObjectModel;
namespace PeopleWith
{
    public class interventiontrigger
    {
        public string interventiontriggerid { get; set; }
        public bool deleted { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string interventiontriggerinformation { get; set; }
        public string category { get; set; }
        public string type { get; set; }



    }

    public class ApiInterventionTrigger
    {
        public ObservableCollection<interventiontrigger> Value { get; set; }
    }

}
