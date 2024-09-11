using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class UpdateListMainPage : ValueChangedMessage<object>
    {
        public UpdateListMainPage(object value) : base(value)
        {
        }
    }
}
