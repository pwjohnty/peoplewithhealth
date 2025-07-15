//using AndroidX.Activity;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Networking;

namespace PeopleWith
{
    public partial class MainPage : ContentPage
    {
        int thirtytwo;
        ObservableCollection<user> updateuser = new ObservableCollection<user>();

        public MainPage()
        {
            InitializeComponent();

           // checkuser();   
            checkifuserisloggedin();
            checkwifion();
            Checkifappisupdated();
        }

        private async void checkwifion()
        {
            try
            {
                NetworkAccess accessType = Connectivity.Current.NetworkAccess;

                if (accessType == NetworkAccess.Internet)
                {
                    //Do Nothing 
                }
                else
                {
                    //var currentPage = Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault();
                    //if (!(currentPage is NoInternetPage))
                    //{
                    //    await Application.Current.MainPage.Navigation.PushAsync(new NoInternetPage());
                    //}

                    if (!(Application.Current.MainPage is NoInternetPage))
                    {
                        //SetMainPage(new NoInternetPage());
                        await Application.Current.MainPage.Navigation.PushAsync(new NoInternetPage());
                    }
                }
            }
            catch (Exception ex) 
            {
                
            }        
        }

        async void checkifuserisloggedin()
        {
            try
            {
                //Temporary Signin
                //Helpers.Settings.UserKey = "63C5E37F-7A27-4157-89D3-06EFB63D6A00"; 

                //var BoolCheck = Preferences.Default.Get("RunOnce", )

                var userid = Preferences.Default.Get("userid", string.Empty);
                var biometrics = Preferences.Default.Get("biometrics", true);
                var pincode = Preferences.Default.Get("pincode", string.Empty);
                string um = Preferences.Default.Get("usermigrated", string.Empty);

                if (!string.IsNullOrEmpty(userid))
                {
                    //check if user has migrated
                   // checkuser();

                    if(um == "false" || string.IsNullOrEmpty(um) || um == "False")
                    {
                        //go to migration assitant

                        await Navigation.PushAsync(new MigrationAssistant(), false);
                        return;
                    }

                    //User still Logged in 
                    if(string.IsNullOrEmpty(pincode))
                    {
                        //Set to Mainpage
                    }
                    else
                    {
                        if (pincode.Contains(","))
                        {
                            var split = pincode.Split(',');
                            if (split[0] == "On" || biometrics == true)
                            {
                                await Navigation.PushAsync(new BiometricsLogin(), false);
                                 //Application.Current.MainPage = new NavigationPage(new BiometricsLogin());
                            }
                            else
                            {
                                //Set to Mainpage
                            }

                        }
                        else
                        {
                            await Navigation.PushAsync(new BiometricsLogin(), false);
                            //Application.Current.MainPage = new NavigationPage(new BiometricsLogin());
                        }
                        
                    }


                }
                else
                {
                    //if (thirtytwo! > 0)
                    //{
                      //  Application.Current.MainPage = new NavigationPage(new MainPage());
                    //    thirtytwo = 1;
                    //}
                }


            }
            catch(Exception ex)
            {

            }
        }

        async void checkuser()
        {
            try
            {

               // updateuser = await database.GetuserDetails();
            }
            catch(Exception ex)
            {

            }
        }

        public async void Checkifappisupdated()
        {
            try
            {
                var versionCheckService = new VersionCheckService();
                bool Check = await versionCheckService.CheckForUpdate();
                if (Check)
                {
                   await Navigation.PushAsync(new UpdatePage(), false);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
               
                await Navigation.PushAsync(new RegisterPage(), false);
            }
            catch(Exception ex)
            {
                //await DisplayAlert(ex.Message, ex.StackTrace, "OK");
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            try
            {

                await Navigation.PushAsync(new LoginPage(), false);
            }
            catch (Exception ex)
            {
                //await DisplayAlert(ex.Message, ex.StackTrace, "OK");
            }
        }

        async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            try
            {
                string BackArrow = "PeopleWith"; 
                await Navigation.PushAsync(new PrivacyPolicy(BackArrow), false);
            }
            catch (Exception Ex)
            {

            }
        }
    }

}
