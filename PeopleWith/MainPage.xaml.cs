namespace PeopleWith
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();


            // Checkifappisupdated();
            checkifuserisloggedin();

        }

        async void checkifuserisloggedin()
        {
            try
            {

                var userid = Preferences.Default.Get("userid", string.Empty);

                if(!string.IsNullOrEmpty(userid))
                {
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
                }


            }
            catch(Exception ex)
            {

            }
        }

        public async Task Checkifappisupdated()
        {
            try
            {
                //  await SharedNotificationService.AddTagsAsync(new string[] { "NewMarkTag" });

                var versionCheckService = new VersionCheckService();
                await versionCheckService.CheckForUpdate();


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
    }

}
