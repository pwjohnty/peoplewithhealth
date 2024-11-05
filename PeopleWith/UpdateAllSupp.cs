using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class UpdateAllSupp : ValueChangedMessage<object>
    {
        public UpdateAllSupp(object value) : base(value)
        {
        }
    }
}
