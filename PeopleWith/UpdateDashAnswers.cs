using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    //public class UpdateDashAnswers : ValueChangedMessage<ObservableCollection<registryData>>
    //{
    //    public UpdateDashAnswers(ObservableCollection<registryData> AllAnswers) : base(AllAnswers)
    //    {
    //    }
    //}

    public class UpdateDashAnswers : ValueChangedMessage<ObservableCollection<registryData>>
    {
        public UpdateDashAnswers(ObservableCollection<registryData> AllAnswers) : base(AllAnswers)
        {
        }
    }
}
