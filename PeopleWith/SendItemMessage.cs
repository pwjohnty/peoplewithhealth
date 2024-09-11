using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class SendItemMessage : ValueChangedMessage<object>
    {
        public SendItemMessage(object value) : base(value)
        {
        }
    }
}
