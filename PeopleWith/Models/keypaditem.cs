using Plugin.Maui.Biometric;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mopups.Services;
using CommunityToolkit.Mvvm.Messaging;
using System.Text.Json;

namespace PeopleWith
{
    public class keypaditem : INotifyPropertyChanged
    {
        private string _pinValue = "";
        public string PinValue
        {
            get => _pinValue;
            set
            {
                if (_pinValue != value)
                {
                    _pinValue = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool permission = true;

        public bool Permission 
        {
            get => permission;
            set
            {
                if (permission != value)
                {
                    permission = value;
                    OnPropertyChanged();
                }
            }
        }


        private bool success = false; 

        public bool Success
        {
            get => success; 
            set
            {
                if (success != value)
                {
                    success = value;
                    OnPropertyChanged();
                }
            }
        } 

        public ICommand KeyTapped { get; private set; }
        public ICommand DeleteTapped { get; private set; }
        public ICommand FingerprintTapped { get; private set; }

        public keypaditem()
        {
            try
            {
                KeyTapped = new Command<string>(execute: (key) =>
                {
                    if (PinValue.Length < 4)
                    {
                        PinValue += key;
                        OnPropertyChanged(nameof(PinValue));
                    }
                });

                DeleteTapped = new Command(execute: () =>
                {
                    if (PinValue.Length > 0)
                    {
                        PinValue = PinValue.Substring(0, PinValue.Length - 1);
                        OnPropertyChanged(nameof(PinValue));
                    }
                });

                FingerprintTapped = new Command(execute: () =>
                {
                    Fingerprint(); 
                });
            }
            catch (Exception Ex)
            {

            }
           
        }

        async public void Fingerprint()
        {
            try
            {
                var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
                {
                    Title = "Confirm your fingerprint to access your account",
                    NegativeText = "USE PIN"
                }, CancellationToken.None);

                var connectivity = Connectivity.Current;
                bool Isconnected = connectivity.NetworkAccess == NetworkAccess.Internet;

                if (result.Status == BiometricResponseStatus.Success)
                {
                    if (!Isconnected)
                    {
                        await MopupService.Instance.PushAsync(new Infopopup("biometrics") { });
                        //WeakReferenceMessenger.Default.Send(new BiometricsOpacity(0.2));
                        return; 
                    }
                    PinValue = "////";
                    OnPropertyChanged(nameof(PinValue));

                    Permission = false;
                    Success = true;
                    //await Task.Delay(2000);
                    //Application.Current.MainPage = new NavigationPage(new MainDashboard());
                    
                                            // Simulate some processing that may take up to 2 seconds
                    await Task.Delay(500);

                    //var checkNotif = Preferences.Get("PWPushNotification", String.Empty);

                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await App.CheckNotificationAfterLogin();
                    });
                }
                else
                {
                    //if (!Isconnected)
                    //{
                    //    await MopupService.Instance.PushAsync(new Infopopup("biometrics") { });
                    //    WeakReferenceMessenger.Default.Send(new BiometricsOpacity(0.2));
                    //    return;
                    //}
                    //Failure
                }
            }
            catch(Exception Ex)
            {
                Permission = true;
                Success = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}