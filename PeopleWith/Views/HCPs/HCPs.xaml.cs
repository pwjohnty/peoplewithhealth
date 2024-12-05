using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class HCPs : ContentPage
{
    public ObservableCollection<hcp> AllUserHCPs = new ObservableCollection<hcp>();
    APICalls aPICalls = new APICalls();
    bool HCPAdded;
    //Connectivity Changed 
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

    public HCPs()
	{
        try
        {
            InitializeComponent();
            GetAllHCPS();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
	}

    public HCPs(ObservableCollection<hcp> AllHCPs)
    {
        try
        {
            InitializeComponent();
            AllUserHCPs = AllHCPs;
            HCPAdded = true;
            GetAllHCPS();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void GetAllHCPS()
    {
        try
        {
            if(!HCPAdded)
            {
                HcpLoading.IsVisible = true; 
                var getHCPSTask = aPICalls.GetUserHCP();

                //var delayTask = Task.Delay(1000);

                //if (await Task.WhenAny(getHCPSTask, delayTask) == delayTask)
                //{
                //    await MopupService.Instance.PushAsync(new GettingReady("Loading HCP's") { });
                //}

                AllUserHCPs = await getHCPSTask;

                //await MopupService.Instance.PopAllAsync(false);
            }
           
            foreach(var item in AllUserHCPs)
            {
                item.fullname = item.firstname + " " + item.surname; 
            }

            if(AllUserHCPs.Count > 0)
            {
                HCPListViewGrid.IsVisible = true;
                EmptyStack.IsVisible = false;
                HCPListView.ItemsSource = AllUserHCPs; 
            }
            else
            {
                HCPListViewGrid.IsVisible = false;
                EmptyStack.IsVisible = true;
            }
            HcpLoading.IsVisible = false; 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                await Navigation.PushAsync(new AddHCPs(AllUserHCPs), false);
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

    async private void HCPListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var SelectedHCP = e.DataItem as hcp;
                await Navigation.PushAsync(new SingleHCP(SelectedHCP, AllUserHCPs), false);
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

    async private void HCPInfoTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("hcp") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}