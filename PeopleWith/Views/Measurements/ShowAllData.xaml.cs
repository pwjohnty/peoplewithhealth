using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class ShowAllData : ContentPage
{
    ObservableCollection<usermeasurement> usermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    ObservableCollection<usermeasurement> deleeteusermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    APICalls aPICalls = new APICalls();

    ObservableCollection<usermeasurement> allusermeasurements = new ObservableCollection<usermeasurement>();
    ObservableCollection<measurement> allmeasurementlist = new ObservableCollection<measurement>();
    public ShowAllData()
	{
		InitializeComponent();
	}

    public ShowAllData(ObservableCollection<usermeasurement> usermeasurementlistp, ObservableCollection<usermeasurement> allusermeasurementsp, ObservableCollection<measurement> allmeasurementlistpassed)
    {
        InitializeComponent();

        usermeasurementlistpassed = usermeasurementlistp;

        measurementname.Text = usermeasurementlistpassed[0].measurementname;

        allusermeasurements = allusermeasurementsp;
        allmeasurementlist = allmeasurementlistpassed;

        foreach (var item in usermeasurementlistpassed)
        {
            var dt = DateTime.Parse(item.inputdatetime);
            item.datechanged = dt.ToString("HH:mm, dd/MM/yyyy");
        }

        usermeasurementlistpassed = new ObservableCollection<usermeasurement>(usermeasurementlistpassed.OrderByDescending(x => DateTime.Parse(x.inputdatetime)));

        usermeasurementlist.ItemsSource = usermeasurementlistpassed;
        usermeasurementlist.HeightRequest = usermeasurementlistpassed.Count * 100;

       
    }


    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //edit button clicked
            usermeasurementlist.IsEnabled = true;
            deletelbl.IsVisible = true;
            // Iterate over the source collection and add each item to SelectedItems
            foreach (var item in usermeasurementlistpassed)
            {
                item.Deleteisvis = true;
            }

            this.ToolbarItems.Clear();


            ToolbarItem itemm = new ToolbarItem
            {
                Text = "Done"
                
            };

            itemm.Clicked += OnItemClicked;

            // "this" refers to a Page object
            this.ToolbarItems.Add(itemm);

            //  usermeasurementlist.SelectionMode = Syncfusion.Maui.ListView.SelectionMode.None;
        }
        catch(Exception ex)
        {

        }
    }

    private async void OnItemClicked(object sender, EventArgs e)
    {
        try
        {
            //done toolbar item clicked
            if(deleeteusermeasurementlistpassed.Count == 0)
            {
                return;
            }


            var result = await DisplayAlert("Delete Feedback", "This permanetly deletes this feedback, are you sure you want to delete?", "Cancel", "Delete");

            if (result)
            {
                return;
            }
          

            deletelbl.IsVisible = false;

            foreach (var item in usermeasurementlistpassed)
            {
                item.Deleteisvis = false;
            }

          

 
            //delete item in local collection
            // Find items in the first collection that are also in the second collection
            var itemsToRemove = usermeasurementlistpassed.Where(item => deleeteusermeasurementlistpassed.Contains(item)).ToList();

            // Remove each item in the 'itemsToRemove' list from the 'firstCollection'
            foreach (var item in itemsToRemove)
            {
                usermeasurementlistpassed.Remove(item);
                allusermeasurements.Remove(item);
            }

            //update single view
            WeakReferenceMessenger.Default.Send(new SendItemMessage(allusermeasurements));

            var filteredMeasurements = allusermeasurements
        .GroupBy(m => m.measurementid)
        .Select(g => g.OrderByDescending(m => DateTime.Parse(m.inputdatetime))
        .First())
        .OrderByDescending(x => DateTime.Parse(x.inputdatetime))
        .ToList();

            // Convert the filtered and sorted list to an ObservableCollection
            ObservableCollection<usermeasurement> observableFilteredMeasurements = new ObservableCollection<usermeasurement>(filteredMeasurements);

            //update main page
            WeakReferenceMessenger.Default.Send(new UpdateListMainPage(observableFilteredMeasurements));

            await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Feedback Updated") { });

            //await Task.Delay(1500);
            //delete the items
            if (deleeteusermeasurementlistpassed.Count > 0)
            {
                await aPICalls.DeleteUserMeasurements(deleeteusermeasurementlistpassed);
                await Task.Delay(1500);
                //update the single view
               

                //update the main measurements page
                // Create a wrapper instance containing both collections
              //  var collectionsWrapper = new CollectionsMessage<usermeasurement, measurement>(allusermeasurements, allmeasurementlist);

                // Send a ValueChangedMessage containing the wrapper
               // WeakReferenceMessenger.Default.Send(new ValueChangedMessage<CollectionsMessage<usermeasurement>>(allusermeasurements));

            }

         

            if (usermeasurementlistpassed.Count == 0)
            {
                datastack.IsVisible = false;
                nodatastack.IsVisible = true;
                this.ToolbarItems.Clear();

            }
            else
            {
                //add the edit button back 
                this.ToolbarItems.Clear();


                ToolbarItem itemm = new ToolbarItem
                {
                    Text = "Edit"

                };

                itemm.Clicked += OnItemEdittwoClicked;

                // "this" refers to a Page object
                this.ToolbarItems.Add(itemm);
            }


            
            await MopupService.Instance.PopAllAsync(false);

        }
        catch(Exception ex)
        {

        }
    }

    private void OnItemEdittwoClicked(object sender, EventArgs e)
    {
        try
        {
            usermeasurementlist.IsEnabled = true;
            deletelbl.IsVisible = true;
            // Iterate over the source collection and add each item to SelectedItems
            foreach (var item in usermeasurementlistpassed)
            {
                item.Deleteisvis = true;
            }

            this.ToolbarItems.Clear();


            ToolbarItem itemm = new ToolbarItem
            {
                Text = "Done"

            };

            itemm.Clicked += OnItemClicked;

            // "this" refers to a Page object
            this.ToolbarItems.Add(itemm);

        }
        catch (Exception ex)
        {

        }
    }

    private async void usermeasurementlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //delete listview button tapped
            var item = e.DataItem as usermeasurement;

            if(deleeteusermeasurementlistpassed.Contains(item))
            {
                deleeteusermeasurementlistpassed.Remove(item);
            }
            else
            {
                deleeteusermeasurementlistpassed.Add(item);
            }
            //var result = await DisplayAlert("Remove Measurment", "Are you sure you want to remove " + item.value, "OK", "Cancel");

            //if(result)
            //{
            //    usermeasurementlistpassed.Remove(item);
            //}


         


        }
        catch(Exception ex)
        {

        }
    }

 
}