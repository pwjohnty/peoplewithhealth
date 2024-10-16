namespace PeopleWith
{
    public partial class MainPage : ContentPage
    {
        int thirtytwo; 
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
                //Temporary Signin
                //Helpers.Settings.UserKey = "63C5E37F-7A27-4157-89D3-06EFB63D6A00"; 

                //var BoolCheck = Preferences.Default.Get("RunOnce", )

                var userid = Preferences.Default.Get("userid", string.Empty);
                var biometrics = Preferences.Default.Get("biometrics", true);
                var pincode = Preferences.Default.Get("pincode", string.Empty);

                if (!string.IsNullOrEmpty(userid))
                {
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

                                Application.Current.MainPage = new NavigationPage(new BiometricsLogin());
                            }
                            else
                            {
                                //Set to Mainpage
                            }

                        }
                        else
                        {
                            Application.Current.MainPage = new NavigationPage(new BiometricsLogin());
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
