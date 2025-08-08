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
using System.Text.Json;
using System.Collections.ObjectModel;
#if ANDROID
using Android.Content;
using Android.App;
#endif


namespace PeopleWith;

public partial class BiometricsLogin : ContentPage
{
    string one;
    string two;
    string ImageFilename = string.Empty; 
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

    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    try
    //    {
    //        await MainThread.InvokeOnMainThreadAsync(async () =>
    //        {
    //            await CheckProfilePic();
    //        });
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    public BiometricsLogin()
	{
        try
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            //Get Profile Picture
            CheckProfilePic();

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                fingerprint.Source = "fingerprint.png";
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                fingerprint.Source = "faceid.png";
            }

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

    private async void CheckProfilePic()
    {
        try
        {
            APICalls databse = new APICalls();
            ImageFilename = await databse.GetProfilePicture();
            //ImageFilename = Helpers.Settings.ProfilePic;

            if (!string.IsNullOrEmpty(ImageFilename))
            {
                ProfilePic.IsVisible = true;
                Initals.IsVisible = false;
                var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/profileuploads/{ImageFilename}?t={DateTime.UtcNow.Ticks}";
                //MainThread.BeginInvokeOnMainThread(() =>
                //{
                ProfilePic.Source = ImageSource.FromUri(new Uri(imagestring));
                //});
            }
            else
            {

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
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private async Task CheckProfilePic()
    //{
    //    try
    //    {
    //        APICalls databse = new APICalls();
    //        ImageFilename = await databse.GetProfilePicture();

    //        if (!string.IsNullOrEmpty(ImageFilename))
    //        {
    //            ProfilePic.IsVisible = true;
    //            Initals.IsVisible = false;
    //            var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/profileuploads/{ImageFilename}?t={DateTime.UtcNow.Ticks}";
    //            //MainThread.BeginInvokeOnMainThread(() =>
    //            //{
    //            ProfilePic.Source = ImageSource.FromUri(new Uri(imagestring));
    //            //});
    //        }
    //        else
    //        {

    //            var Nameone = Helpers.Settings.FirstName;
    //            var Nametwo = Helpers.Settings.Surname;
    //            if (!string.IsNullOrEmpty(Nameone))
    //            {
    //                one = Nameone.Substring(0, 1);
    //            }

    //            if (!string.IsNullOrEmpty(Nametwo))
    //            {
    //                two = Nametwo.Substring(0, 1);
    //            }
    //            if (!string.IsNullOrEmpty(one) && !string.IsNullOrEmpty(two))
    //            {
    //                var NameInt = one + two;
    //                Initals.Text = NameInt.ToUpper();
    //            }

    //            if (string.IsNullOrEmpty(Initals.Text))
    //            {
    //                Initals.Text = "PW";
    //            }
    //        }

    //        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
    //        {
    //            fingerprint.Source = "fingerprint.png";
    //        }
    //        else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
    //        {
    //            fingerprint.Source = "faceid.png";
    //        }
    //    }
    //    catch(Exception Ex)
    //    {

    //    }
    //}

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

                      //var checkNotif = Preferences.Get("PWPushNotification", String.Empty);


                    MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            await App.CheckNotificationAfterLogin(); 
                 
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