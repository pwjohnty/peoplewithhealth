using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class HCPs : ContentPage
{
    public ObservableCollection<hcp> AllUserHCPs = new ObservableCollection<hcp>();
    APICalls aPICalls = new APICalls();
    bool HCPAdded;
    public HCPs()
	{
		InitializeComponent();
        GetAllHCPS(); 
	}

    public HCPs(ObservableCollection<hcp> AllHCPs)
    {
        InitializeComponent();
        AllUserHCPs = AllHCPs;
        HCPAdded = true; 
        GetAllHCPS();
    }

    async private void GetAllHCPS()
    {
        try
        {
            if(!HCPAdded)
            {
                var getHCPSTask = aPICalls.GetUserHCP();

                var delayTask = Task.Delay(1000);

                if (await Task.WhenAny(getHCPSTask, delayTask) == delayTask)
                {
                    await MopupService.Instance.PushAsync(new GettingReady("Loading HCP's") { });
                }

                AllUserHCPs = await getHCPSTask;

                await MopupService.Instance.PopAllAsync(false);
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
        }
        catch (Exception Ex)
        {

        }
    }

    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddHCPs(AllUserHCPs), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void HCPListView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var SelectedHCP = e.DataItem as hcp;
            await Navigation.PushAsync(new SingleHCP(SelectedHCP, AllUserHCPs), false) ;
        }
        catch (Exception Ex)
        {

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
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }
}