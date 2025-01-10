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
                if(e.Request.Title == "Complete EQ5D Questionnaire")
                {
                    //get the questionnaire 
                   // var getQuestionairesTask = await aPICalls.GetSingleQuestionnaires();

                   // if(getQuestionairesTask != null)
                   // {
                        await MainPage.Navigation.PushAsync(new AllQuestionnaires(),false);
                   // }

                    
                }
            }
            catch (Exception ex)
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
