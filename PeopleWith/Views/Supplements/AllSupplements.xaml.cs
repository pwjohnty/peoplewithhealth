using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllSupplements : ContentPage
{
    APICalls aPICalls = new APICalls();
    public ObservableCollection<usersupplement> AllUserSupplements = new ObservableCollection<usersupplement>();
    public AllSupplements()
	{
        try
        {
            InitializeComponent();
            getusersupplements();
        }
        catch (Exception Ex)
        {

        }
    }

    public AllSupplements(ObservableCollection<usersupplement> AllUserSupps)
    {
        try
        {
            InitializeComponent();
            AllUserSupplements.Clear();

            AllUserSupplements = AllUserSupps;

            AllUserSuppsList.ItemsSource = AllUserSupplements;
        }
        catch (Exception Ex)
        {

        }

    }


    async void getusersupplements()
    {
        try
        {
            var getSupplementsTask = aPICalls.GetUserSupplementsAsync();

            var delayTask = Task.Delay(1000);

            if (await Task.WhenAny(getSupplementsTask, delayTask) == delayTask)
            {
                //Comment Back in When Code Merged
                //await MopupService.Instance.PushAsync(new GettingReady("Loading Supplements") { });
            }

            AllUserSupplements = await getSupplementsTask;

            AllUserSuppsList.ItemsSource = AllUserSupplements;

            await MopupService.Instance.PopAllAsync(false);

        }
        catch (Exception ex)
        {

        }
    }

    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddSupplements(AllUserSupplements), false);
        }
        catch (Exception ex)
        {

        }
    }

    async private void AllUserSuppsList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //usersupplement SelectedSupp = e.DataItem as usersupplement;
            //await Navigation.PushAsync(new SingleMedication(AllUserSupplements, SelectedSupp), false);
        }
        catch (Exception Ex)
        {

        }
    }
}