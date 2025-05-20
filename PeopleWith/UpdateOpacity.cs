using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class BiometricsOpacity : ValueChangedMessage<double>
    {
        public BiometricsOpacity(double value) : base(value)
        {
        }
    }

    public class LoginOpacity : ValueChangedMessage<double>
    {
        public LoginOpacity(double value) : base(value)
        {
        }
    }

    public class ResetOpacity : ValueChangedMessage<double>
    {
        public ResetOpacity(double value) : base(value)
        {
        }
    }
}
