using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class UpdateAppFeedback : ValueChangedMessage<object>
    {
        public UpdateAppFeedback(object value) : base(value)
        {
        }
    }
}
