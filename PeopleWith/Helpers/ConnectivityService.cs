using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Networking;

namespace PeopleWith
{
    public class ConnectivityService
    {
        public event EventHandler<bool> ConnectivityChanged;

        public ConnectivityService()
        {
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var isConnected = e.NetworkAccess == NetworkAccess.Internet;
            ConnectivityChanged?.Invoke(this, isConnected);
        }
    }
}
