using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class PrivacyPolicy : ContentPage
{
    //ObservableCollection<privacy> PrivacyList = new ObservableCollection<privacy>();
    ObservableCollection<privacypolicy> PrivacyList = new ObservableCollection<privacypolicy>();

    public PrivacyPolicy(string BeforeLogin)
    {
        try
        {
            InitializeComponent();
            BackArrow.IsVisible = true;
            NavigationPage.SetHasNavigationBar(this, false);
            if(DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                AndroidBtn.IsVisible = true; 
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                IOSBtn.IsVisible = true; 
            }
            //ListViewGrid.Margin = (0, 0, 0, 0); 
            PopulatelistView();
        }
        catch (Exception Ex)
        {

        }
    }
    public PrivacyPolicy()
    {
        try
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            PopulatelistView();


        }
        catch (Exception Ex)
        {

        }
    }

        async void PopulatelistView()
        {
            try
            {
            //API Call 
            APICalls database = new APICalls();
            PrivacyList = await database.GetAsyncPrivacyPolicy();
            PrivPolicy.ItemsSource = PrivacyList;

            }
            catch (Exception Ex)
            {

            }
        }

    //private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    //{
    //    try
    //    {
    //        Navigation.RemovePage(this);
    //    }
    //    catch (Exception Ex)
    //    {

    //    }
    //}

    async private void AndroidBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.RemovePage(this);
        }
        catch (Exception Ex)
        {

        }
    }
}