using PeopleWith;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

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
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRQRmRcUER0W0A=");

            MainPage = new AppShell();
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

                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {
                        await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    }
                    else
                    {
                        await (Application.Current.MainPage as NavigationPage)?.Navigation.PushAsync(new AndroidQuestionnaires("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    }
                    // }


                }
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnResume()
        {
            try
            {
                base.OnResume();

                // Ensure MainPage and Navigation are not null
                if (MainPage?.Navigation?.NavigationStack == null)
                    return;

                // Get the current page
                var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();
                if (currentPage == null)  return;

                // Check the type of the current page and send appropriate messages
                if (currentPage.GetType().Name == "ProfileSection")
                {
                    MessagingCenter.Send<App>(this, "CallMethodOnPage");
                }
                else if (currentPage.GetType().Name == "MainDashboard")
                {
                    MessagingCenter.Send<App>(this, "CallNotifications");
                }
            }
            catch (Exception Ex)
            {

            }
        }



        private async void OnConnectivityChanged(object sender, bool isConnected)
        {
            try
            {
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
                    // Check if NoInternetPage is on the navigation stack and pop it
                    var noInternetPage = MainPage.Navigation.NavigationStack.FirstOrDefault(p => p is NoInternetPage);
                    if (noInternetPage != null)
                    {
                        await MainPage.Navigation.PopAsync();
                    }
                }
            }
            catch(Exception Ex)
            {

            }
        }
    }
}
