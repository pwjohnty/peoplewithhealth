using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllInvestigations : ContentPage
{

    public ObservableCollection<userinvestigation> AllUserInvests = new ObservableCollection<userinvestigation>();

    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
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
    public AllInvestigations()
	{
		InitializeComponent();
        GetAllInvestInfo(); 

    }


    async private void GetAllInvestInfo()
    {

        try
        {
            InvestLoading.IsVisible = true;
            var Userid = Helpers.Settings.UserKey;
            APICalls database = new APICalls();

            //Get User Investigation Info 
            var getUserInvestTask = database.GetUserInvestigationAsync();

            AllUserInvests = await getUserInvestTask;

            if (AllUserInvests.Count > 0)
            {

                EmptyStack.IsVisible = false;
                InvestOverview.IsVisible = true;
                AllInvestView.ItemsSource = AllUserInvests
                .GroupBy(m => m.investigationname)
                .Select(g => g.OrderByDescending(f => DateTime.Parse(f.investigationdate)).First())
                .OrderByDescending(f => DateTime.Parse(f.investigationdate))
                .ToList();

                AllInvestView.HeightRequest = AllUserInvests.Count * 80;
            }
            else
            {
                EmptyStack.IsVisible = true;
                InvestOverview.IsVisible = false;
            }

            InvestLoading.IsVisible = false;

            NovoConsentData();
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
            await MopupService.Instance.PushAsync(new Infopopup("Invest") { });
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
                await Navigation.PushAsync(new AddInvestigations(AllUserInvests));
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

    private async void AllInvestView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                var getitem = e.DataItem as userinvestigation;

                await Navigation.PushAsync(new SingleInvestigations(getitem), false);
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
}