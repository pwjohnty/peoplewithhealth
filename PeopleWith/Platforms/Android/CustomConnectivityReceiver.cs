using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Net;
using Microsoft.Maui.ApplicationModel;

namespace PeopleWith
{
    [BroadcastReceiver(Enabled = true, Exported = false)]
    public class CustomConnectivityReceiver : BroadcastReceiver
    {
        public event EventHandler<bool> ConnectivityChanged;

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action == ConnectivityManager.ConnectivityAction)
            {
                var connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
                var networkInfo = connectivityManager?.ActiveNetworkInfo;
                bool isConnected = networkInfo != null && networkInfo.IsConnected;

                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
    }
}
