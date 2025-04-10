using Mopups.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Storage;

namespace PeopleWith;

public partial class AllDiet : ContentPage
{
    
    public ObservableCollection<userdiet> AllUserDiets = new ObservableCollection<userdiet>();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public AllDiet()
	{
		InitializeComponent();
        GetAllDietInfo();

    }

    private void NovoConsentData()
    {
        try
        {
            if (!String.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                var signup = Helpers.Settings.SignUp;
                if (signup.Contains("SAX"))
                { //All Novo SignupCodes 
                    NovoConsent.IsVisible = true;
                    NovoContentlbl.Text = Preferences.Default.Get("NovoContent", String.Empty);
                    NovoExitidlbl.Text = Preferences.Default.Get("NovoExitid", String.Empty);
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void GetAllDietInfo()
    {

            try
            {
                    DietLoading.IsVisible = true;
                    var Userid = Helpers.Settings.UserKey;
                    APICalls database = new APICalls();

                    //Get Diet Details 
                    //var getDietTask = database.GetDietDetails();

                    //AllDiets = await getDietTask;

                    //Get User Diet Info 
                    var getUserDietTask = database.GetUserDietAsync();

                    AllUserDiets = await getUserDietTask;      

                    if (AllUserDiets.Count > 0)
                    {

                        EmptyStack.IsVisible = false;
                        DietOverview.IsVisible = true;
                        AllDietView.ItemsSource = AllUserDiets
                        .GroupBy(m => m.diettitle)
                        .Select(g => g.OrderByDescending(f => DateTime.Parse(f.datestarted)).First())
                        .OrderByDescending(f => DateTime.Parse(f.datestarted))
                        .ToList();

                       AllDietView.HeightRequest = AllUserDiets.Count * 80;
                    }
                    else
                    {
                        EmptyStack.IsVisible = true;
                        DietOverview.IsVisible = false;
                    }

                    DietLoading.IsVisible = false;

                    NovoConsentData();
            }
            catch (Exception Ex)
            {
                NotasyncMethod(Ex);
            }

    }

    private async void AddBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                await Navigation.PushAsync(new AddDiet(AllUserDiets));
                AddBtn.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("diet") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void AllDietView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var getitem = e.DataItem as userdiet;

            await Navigation.PushAsync(new SingleDiet(getitem), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}