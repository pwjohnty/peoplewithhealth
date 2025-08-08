using PeopleWith;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel;
using Mopups.Services;
using Android.Telephony;
using Org.Apache.Http.Impl.Client;
using System.Collections.ObjectModel;
using System.Text.Json;
using Android.Content;
using AndroidX.Annotations;


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
            //_connectivityService = new ConnectivityService();
            //_connectivityService.ConnectivityChanged += OnConnectivityChanged;
            _connectivityService = new ConnectivityService();
            SubscribeConnectivity();

            Application.Current.UserAppTheme = AppTheme.Light;

            // Register notification tapped event
            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationTapped;

            //Register Syncfusion license
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRQRmRcUER0W0A=");
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NNaF5cXmBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXtfcXRcQ2BeUEVwWEZWYUA=");

            //  MainPage = new AppShell();
        }

        public static void ConfigureSentryUserScope()
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
            //return base.CreateWindow(activationState);
            return new Window(new NavigationPage(new MainPage()));
            //return new Window(new AppShell());
        }
        public static async Task SetMainPage(Page newRootPage)
        {
            //Set New RootPage
            if (Application.Current?.Windows?.FirstOrDefault() is Window window)
            {
                if (newRootPage is not NavigationPage && newRootPage is not Shell)
                {
                    newRootPage = new NavigationPage(newRootPage);
                }
                window.Page = newRootPage;
                //Application.Current.Windows[0].Page = newRootPage;
            }
        }

      
        private void SubscribeConnectivity()
        {
            _connectivityService.ConnectivityChanged += ConnectivityHandler;
        }

        private void UnsubscribeConnectivity()
        {
            _connectivityService.ConnectivityChanged -= ConnectivityHandler;
        }

        //private async void OnNotificationTapped(NotificationActionEventArgs e)
        //{
        //    try
        //    {
        //        if(e.Request.Title == "Complete your EQ-5D Questionnaire")
        //        {
        //            //get the questionnaire 
        //            // var getQuestionairesTask = await aPICalls.GetSingleQuestionnaires();

        //            // if(getQuestionairesTask != null)
        //            // {
        //            ///Application.Current.MainPage = new NavigationPage(new MainDashboard());
        //            //Application.Current.MainPage = new NavigationPage(new MainDashboard());

        //            App.SetMainPage(new NavigationPage(new MainDashboard()));

        //            if (DeviceInfo.Platform == DevicePlatform.iOS)
        //            {
        //                await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
        //            }
        //            else
        //            {
        //                await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new AndroidQuestionnaires("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        ConfigureSentryUserScope();
        //        SentrySdk.CaptureException(Ex);
        //    }
        //}

        private async void OnNotificationTapped(NotificationActionEventArgs e)
        {
            try
            {
                //App.SetMainPage(new NavigationPage(new MainDashboard()));
                string QuestionnaireID = string.Empty;
                var userfeedbacklist = await aPICalls.GetUserFeedback();
                if (e.Request.Title == "Complete your EQ-5D Questionnaire")
                {
                    QuestionnaireID = "A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF";
                }
                else if (e.Request.Title == "Complete your SF-36 General Health Questionnaire")
                {
                    QuestionnaireID = "DC6A9FD7-242B-4299-9672-D745669FEAF0";
                }
                else if (e.Request.Title == "Complete Your HOCM Baseline Questionnaire")
                {
                    QuestionnaireID = "BE72B2A1-0707-4E8D-8E82-022BA4F959F4";
                }
                if (!String.IsNullOrEmpty(QuestionnaireID))
                {
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        SetMainPageWithStack(new MainDashboard(), new AndroidQuestionnaires(QuestionnaireID, userfeedbacklist[0]));
                        //await (Application.Current.Windows[0].Page)?.Navigation.PushAsync(new AndroidQuestionnaires(QuestionnaireID), false);
                    }
                    else
                    {
                        SetMainPageWithStack(new MainDashboard(), new AndroidONLYQuestionnaires(QuestionnaireID, userfeedbacklist[0]));
                        //await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new AndroidONLYQuestionnaires(QuestionnaireID), false);
                    }
                }
            }
            catch (Exception Ex)
            {
                ConfigureSentryUserScope();
                SentrySdk.CaptureException(Ex);
            }
        }


        public static async Task SetMainPageWithStack(params Page[] pages)
        {
            if (Application.Current?.Windows.Any() != true || pages == null || pages.Length == 0) return;

            var navPage = new NavigationPage(pages[0]);

            Application.Current.Windows[0].Page = navPage;

            await Task.Delay(100);

            for (int i = 1; i < pages.Length; i++)
            {
                await navPage.Navigation.PushAsync(pages[i], false);
            }
        }

        protected async override void OnSleep()
        {
            try
            {
                UnsubscribeConnectivity();
            }
            catch (Exception Ex)
            {
                ConfigureSentryUserScope();
                SentrySdk.CaptureException(Ex);
            }
        }

        protected async override void OnResume()
        {
            try
            {
                base.OnResume();

                // Ensure MainPage and Navigation are not null
                if (MainPage?.Navigation?.NavigationStack == null) return;

                // Get the current page
                var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();
                if (currentPage == null) return;

                // Check the type of the current page and send appropriate messages
                if (currentPage is ProfileSection)
                {
                    MessagingCenter.Send<App>(this, "CallMethodOnPage");
                }
                else if (currentPage is MainDashboard)
                {
                    MessagingCenter.Send<App>(this, "CheckUserSettings");
                }

                SubscribeConnectivity();

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


        public static async Task AndroidPushNotificationTappedAsync(ObservableCollection<pushdata> NewNotification)
        {
            try
            {
                if(NewNotification == null) return;

                var Area = NewNotification.FirstOrDefault(x => (!string.IsNullOrEmpty(x.key) && x.key == "action"));

                if (Area != null)
                {
                    var userid = Preferences.Default.Get("userid", string.Empty);
                    var pincode = Preferences.Default.Get("pincode", string.Empty);

                    APICalls database = new APICalls();
                    var userfeedbacklist = await database.GetUserFeedback();
                    var feedback = userfeedbacklist?.FirstOrDefault(); 

                    //user not logged in 
                    if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(pincode)) return; 

                    if (Area.Data.ToLower().Contains("question"))
                    {
                        var QuestionID = NewNotification.FirstOrDefault(x => (!string.IsNullOrEmpty(x.key) && x.key == "questionnaireid"));

                        if (QuestionID != null)
                        {
                            if (!String.IsNullOrEmpty(QuestionID.Data))
                            {
                                if (feedback != null)
                                {
                                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                                    {
                                        await SetMainPageWithStack(new MainDashboard(), new AndroidQuestionnaires(QuestionID.Data, feedback));
                                    }
                                    else
                                    {
                                        await SetMainPageWithStack(new MainDashboard(), new AndroidONLYQuestionnaires(QuestionID.Data, feedback));
                                    }
                                }           
                            }
                        }                
                    }
                    else if (Area.Data.ToLower().Contains("med"))
                    {
                        await SetMainPageWithStack(new MainDashboard(), new AllMedications());
                    }
                    else if (Area.Data.ToLower().Contains("diag"))
                    {
                        await SetMainPageWithStack(new MainDashboard(), new AllDiagnosis());
                    }
                    else if (Area.Data.ToLower().Contains("meas"))
                    {
                        if (feedback != null)
                        {
                            await SetMainPageWithStack(new MainDashboard(), new MeasurementsPage(feedback));
                        }                    
                    }
                    else if (Area.Data.ToLower().Contains("symp"))
                    {
                        if (feedback != null)
                        {
                            await SetMainPageWithStack(new MainDashboard(), new AllSymptoms(feedback));
                        }                    
                    }

                    //Always Remove Data When this metod loads

                    var filePath = Path.Combine(FileSystem.AppDataDirectory, "PWPushNotification.json");

                    if (File.Exists(filePath))
                    {
                        try
                        {
                            File.Delete(filePath);
                        }
                        catch (Exception ex)
                        {
                            //Dunno
                        }
                    }

                    //var context = Android.App.Application.Context;
                    //var prefs = context.GetSharedPreferences("MyAppPrefs", FileCreationMode.Private);
                    //var editor = prefs.Edit();
                    //editor.Remove("PWPushNotification");
                    //editor.Apply();
                }
            }
            catch (Exception Ex)
            {
                ConfigureSentryUserScope();
                SentrySdk.CaptureException(Ex);
            }
        }

        public static async Task CheckNotificationAfterLogin()
        {
#if ANDROID
            //var context = Android.App.Application.Context;
            //var prefs = context.GetSharedPreferences("MyAppPrefs", FileCreationMode.Private);
            //var json = prefs.GetString("PWPushNotification", null);
            var json = string.Empty; 

            var filePath = Path.Combine(FileSystem.AppDataDirectory, "PWPushNotification.json");
            if (File.Exists(filePath))
            {
               json = await File.ReadAllTextAsync(filePath);
            }

            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    var dataItem = JsonSerializer.Deserialize<ObservableCollection<pushdata>>(json);
                    if (dataItem != null && dataItem.Count > 0)
                    {
                        await App.AndroidPushNotificationTappedAsync(dataItem);
                    }
                }
                catch
                {
                    await App.SetMainPage(new NavigationPage(new MainDashboard()));
                }
            }
            else
            {
                await App.SetMainPage(new NavigationPage(new MainDashboard()));
            }
#else

    //Ios for not Normal Navigation 
    await App.SetMainPage(new NavigationPage(new MainDashboard()));

#endif
        }


        private async void ConnectivityHandler(object sender, bool isConnected)
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
                    //New

                    if (!(Application.Current.MainPage is NoInternetPage))
                    {
                        await Application.Current.MainPage.Navigation.PushAsync(new NoInternetPage());
                    }
                    //Old
                    //var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();
                    //if (!(currentPage is NoInternetPage))
                    //{
                    //    await MainPage.Navigation.PushAsync(new NoInternetPage());
                    //}
                }
                else
                {
                    //New
                    var mainPage = Application.Current?.Windows.FirstOrDefault()?.Page;
                    if (mainPage != null)
                    {
                        var pageToRemove = mainPage.Navigation.NavigationStack.FirstOrDefault(p => p is NoInternetPage);
                        if (pageToRemove != null)
                        {
                            mainPage.Navigation.RemovePage(pageToRemove);
                        }
                    }
                    //Old
                    //var pageToRemove = MainPage.Navigation.NavigationStack.FirstOrDefault(p => p is NoInternetPage);
                    //if (pageToRemove != null)
                    //{
                    //    MainPage.Navigation.RemovePage(pageToRemove);
                    //}
                    //var noInternetPage = MainPage.Navigation.NavigationStack.FirstOrDefault(p => p is NoInternetPage);
                    //if (noInternetPage != null)
                    //{
                    //    await MainPage.Navigation.PopAsync();
                    //}
                }
            }
            catch (Exception Ex)
            {
            }
        }
    }
}
