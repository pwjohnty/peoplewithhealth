using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;
//using Android.App;
//using Android.Content;
using Microsoft.Maui.Networking;
using System;

namespace PeopleWith
{
    public class ConnectivityService
    {
        public event EventHandler<bool> ConnectivityChanged;

        public ConnectivityService()
        {
            Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            //Connectivity.ConnectivityChanged += OnConnectivityChanged;
          
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            try
            {
                var isConnected = e.NetworkAccess == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
            catch (Exception Ex)
            {

            }
          
        }
    }
}
