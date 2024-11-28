using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using Plugin.Maui.Biometric;
using PINView.Maui;
using Microsoft.Maui;
using Syncfusion.Maui.Core.Internals;

namespace PeopleWith;

public partial class BiometricsLogin : ContentPage
{
    string one;
    string two;
    public BiometricsLogin()
	{
        try
        {
            InitializeComponent();


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
                Initals.Text = one + two;
                //Name.Text = Helpers.Settings.FirstName + " " + Helpers.Settings.Surname;
            }

            if (string.IsNullOrEmpty(Initals.Text))
            {
                Initals.Text = "PW";
                //Name.Text = "Enter Pin Code";
            }

            //Set Page Visible/Not Visible 
            var biometrics = Preferences.Default.Get("biometrics", true);
            var pincode = Preferences.Default.Get("pincode", string.Empty);
            if (pincode.Contains(","))
            {
                var split = pincode.Split(',');
                if (split[0] == "On")
                {
                    if (biometrics == false)
                    {
                        fingerprint.IsVisible = true;
                    }
                }
                else
                {
                    //Hide PinCode 
                    Pincode.IsVisible = false;
                    incorrectcodelbl.IsVisible = false;
                    PinKeyPad.IsVisible = false;
                    ForgotPassword.IsVisible = false;
                    Name.IsVisible = false;

                    if (biometrics == true)
                    {
                        CallFingerPrint();
                    }
                    //Set to Mainpage
                }
            }

        }
        catch (Exception Ex)
        {
           
        }

    }

    async public void CallFingerPrint()
    {
        try
        {

           await Fingerprint();
        }
        catch (Exception Ex)
        {

        }
    }

    async public Task Fingerprint()
    {
        try
        {
            await Task.Delay(2000);
            var result = await BiometricAuthenticationService.Default.AuthenticateAsync(new AuthenticationRequest()
            {
                Title = "Confirm your fingerprint to access your account",
                NegativeText = "USE PIN"
            }, CancellationToken.None);

            if (result.Status == BiometricResponseStatus.Success)
            {
                Application.Current.MainPage = new NavigationPage(new MainDashboard());
            }
            else
            {
               
            }
        }
        catch (Exception Ex)
        {

        }
    }


    async private void Pincode_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {
            var GetPin = Helpers.Settings.PinCode;
            if (GetPin.Contains(","))
            {
                var GetPinSplit = GetPin.Split(','); 
                if (GetPinSplit[1] == e.PIN)
                {
                    LoadingInd.IsVisible = true;
                    Loadinglbl.IsVisible = true; 
                    PinKeyPad.IsVisible = false;
                    ForgotPassword.IsVisible = false;
                    await Task.Delay(2000);
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
                }
                else
                {
                    Pincode.PINValue = null;
                    Vibration.Vibrate();
                }
            }
            else
            {
                if (Helpers.Settings.PinCode == e.PIN)
                {
                    LoadingInd.IsVisible = true;
                    Loadinglbl.IsVisible = true;
                    PinKeyPad.IsVisible = false;
                    ForgotPassword.IsVisible = false;
                    await Task.Delay(2000);
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
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

        }
    }

    private void Keypad_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {

        }
    }

    private void Pincode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {

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