﻿using PeopleWith;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel;
using Mopups.Services;

namespace PeopleWith
{
    public partial class App : Application
    {
        private readonly ConnectivityService _connectivityService;
        APICalls aPICalls = new APICalls();
        public App()
        {
            InitializeComponent();

            // Initialize connectivity service
            _connectivityService = new ConnectivityService();
            _connectivityService.ConnectivityChanged += OnConnectivityChanged;

            Application.Current.UserAppTheme = AppTheme.Light;

            // Register notification tapped event
            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationTapped;

            //Register Syncfusion license
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRQRmRcUER0W0A=");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXtfcXRcQ2BeUEVwWEZWYUA=");

            //  MainPage = new AppShell();
        }

        void ConfigureSentryUserScope()
        {
            SentrySdk.ConfigureScope(scope =>
            {
                string UserID = Preferences.Default.Get("userid", "Unknown");
                string UserEmail = Preferences.Default.Get("email", "Unknown");
                scope.User = new SentryUser
                {
                    Id = UserID,
                    Email = UserEmail
                };
            });
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private async void OnNotificationTapped(NotificationActionEventArgs e)
        {
            try
            {
                if(e.Request.Title == "Complete your EQ-5D Questionnaire")
                {
                    //get the questionnaire 
                    // var getQuestionairesTask = await aPICalls.GetSingleQuestionnaires();

                    // if(getQuestionairesTask != null)
                    // {
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
                    //Application.Current.MainPage = new NavigationPage(new MainDashboard());

                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    }
                    else
                    {
                        await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new AndroidQuestionnaires("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    }
                }
            }
            catch (Exception Ex)
            {
                ConfigureSentryUserScope();
                SentrySdk.CaptureException(Ex);
            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();

                //NetworkAccess accessType = Connectivity.Current.NetworkAccess;

                // Ensure MainPage and Navigation are not null
                if (MainPage?.Navigation?.NavigationStack == null)
                    return;

                // Get the current page
                var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();
                if (currentPage == null)  return;

                // Check the type of the current page and send appropriate messages
                if (currentPage is ProfileSection)
                {
                    MessagingCenter.Send<App>(this, "CallMethodOnPage");
                }
                else if (currentPage is MainDashboard)
                {
                    MessagingCenter.Send<App>(this, "CheckUserSettings");
                    //MessagingCenter.Send<App>(this, "CallNotifications");
                    //#if ANDROID
                    //     MessagingCenter.Send<App>(this, "CallBatterySaver");
                    //#endif

                }

                //if (currentPage.GetType().Name == "ProfileSection")
                //{
                //    MessagingCenter.Send<App>(this, "CallMethodOnPage");
                //}
                //else if (currentPage.GetType().Name == "MainDashboard")
                //{
                //    MessagingCenter.Send<App>(this, "CallNotifications");
                //}
            }
            catch (Exception Ex)
            {
                ConfigureSentryUserScope();
                SentrySdk.CaptureException(Ex);
            }
        }



        private async void OnConnectivityChanged(object sender, bool isConnected)
        {
            try
            {

                //Close any popup page 
                if (MopupService.Instance.PopupStack.Count > 0)
                {
                    await MopupService.Instance.PopAsync();
                }
              
                if (!isConnected)
                {
                    // Check if NoInternetPage is already on the navigation stack
                    var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();
                    if (!(currentPage is NoInternetPage))
                    {
                        await MainPage.Navigation.PushAsync(new NoInternetPage());
                    }
                }
                else
                {
                        var pageToRemove = MainPage.Navigation.NavigationStack.FirstOrDefault(p => p is NoInternetPage);
                        if (pageToRemove != null)
                        {
                            MainPage.Navigation.RemovePage(pageToRemove);
                        }

                    //var noInternetPage = MainPage.Navigation.NavigationStack.FirstOrDefault(p => p is NoInternetPage);
                    //if (noInternetPage != null)
                    //{
                    //    await MainPage.Navigation.PopAsync();
                    //}
                }
            }
            catch(Exception Ex)
            {

            }
        }
    }
}
