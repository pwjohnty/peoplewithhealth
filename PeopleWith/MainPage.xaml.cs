namespace PeopleWith
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage()
        {
            InitializeComponent();


           // Checkifappisupdated();


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
                await Navigation.PushAsync(new RegisterPage());
            }
            catch(Exception ex)
            {

            }
        }
    }

}
