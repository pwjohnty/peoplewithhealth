using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using Plugin.Maui.Biometric;
using PINView.Maui;
using Microsoft.Maui;
using Syncfusion.Maui.Core.Internals;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Mopups.Services;
using CommunityToolkit.Mvvm.Messaging;

namespace PeopleWith;

public partial class BiometricsLogin : ContentPage
{
    string one;
    string two;
    public event EventHandler<bool> ConnectivityChanged;
    public readonly struct UpdateBiometrics { }
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Login"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public BiometricsLogin()
	{
        try
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                fingerprint.Source = "fingerprint.png"; 
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                fingerprint.Source = "faceid.png";
            }

            var Nameone = Helpers.Settings.FirstName;
            var Nametwo = Helpers.Settings.Surname;
            if (!string.IsNullOrEmpty(Nameone))
            {
                one = Nameone.Substring(0, 1);
            }

            if (!string.IsNullOrEmpty(Nametwo))
            {
                two = Nametwo.Substring(0, 1);
            }
            if (!string.IsNullOrEmpty(one) && !string.IsNullOrEmpty(two))
            {
                var NameInt = one + two; 
                Initals.Text = NameInt.ToUpper();
            }

            if (string.IsNullOrEmpty(Initals.Text))
            {
                Initals.Text = "PW";
            }

            var pincode = Preferences.Default.Get("pincode", string.Empty);

            //Return to Normal Opacticy 
            //WeakReferenceMessenger.Default.Register<BiometricsOpacity>(this, (r, m) =>
            //{
            //    this.Opacity = m.Value;
            //});
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async private void Pincode_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {
            var GetPin = Helpers.Settings.PinCode;         
            if (e.PIN == "////")
            {
                //Used to Simulate Correct Password When Using Biometrics over Pin
                return; 
            }
            else
            {
                var connectivity = Connectivity.Current;
                bool Isconnected = connectivity.NetworkAccess == NetworkAccess.Internet;

                if (Helpers.Settings.PinCode == e.PIN)
                {
                        if (!Isconnected)
                        {
                            await MopupService.Instance.PushAsync(new Infopopup("biometrics") { });
                            //this.Opacity = 0.2; 
                            Pincode.PINValue = null;
                            return;
                        }

                        LoadingInd.IsVisible = true;
                        Loadinglbl.IsVisible = true;
                        PinKeyPad.IsVisible = false;
                        ForgotPassword.IsVisible = false;

                        await Task.Run(async () =>
                        {
                            // Simulate some processing that may take up to 2 seconds
                            await Task.Delay(500);
                        });

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new NavigationPage(new MainDashboard());
                        });
                    }
                    else
                    {
                        Pincode.PINValue = null;
                        Vibration.Vibrate();
                    }
            }        
        }
        catch (Exception Ex)
        {
            PinKeyPad.IsVisible = true;
            ForgotPassword.IsVisible = true;
            Name.IsVisible = true;
            incorrectcodelbl.IsVisible = false;
            LoadingInd.IsVisible = false;
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new LoginPage("Reset Pin"), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void Pincode_Focused(object sender, FocusEventArgs e)
    {
        try
        {
            Pincode.Unfocus();
        }
        catch (Exception Ex)
        {

        }
    }
}