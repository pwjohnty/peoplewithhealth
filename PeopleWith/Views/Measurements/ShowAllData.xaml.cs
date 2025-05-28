using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Mopups.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Networking;



namespace PeopleWith;

public partial class ShowAllData : ContentPage
{
    ObservableCollection<usermeasurement> usermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    ObservableCollection<usermeasurement> deleeteusermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    ObservableCollection<usermeasurement> displayedMeasurements = new ObservableCollection<usermeasurement>();
    int batchSize = 50;
    int currentIndex = 0;
    private const int PageSize = 30;
    private int currentPage = 0;
    bool isLoading = false;
    bool IsNavigating = false; 
    APICalls aPICalls = new APICalls();

    ObservableCollection<usermeasurement> allusermeasurements = new ObservableCollection<usermeasurement>();
    ObservableCollection<measurement> allmeasurementlist = new ObservableCollection<measurement>();
    userfeedback userfeedbacklistpassed = new userfeedback();
    bool Result; 
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

    //protected override void OnDisappearing()
    //{
    //    try
    //    {
    //        //Clear the Toolbar item on Back Pressed and reset to Original
    //        this.ToolbarItems.Clear();
    //        ToolbarItem item = new ToolbarItem
    //        {
    //            Text = "Edit"

    //        };

    //        item.Clicked += ToolbarItem_Clicked;
    //        this.ToolbarItems.Add(item);
    //        //edit button clicked
    //        usermeasurementlist.IsEnabled = false;
    //       deletelbl.IsVisible = false;
    //        foreach (var items in displayedMeasurements)
    //        {
    //            items.Deleteisvis = false;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    public ShowAllData()
	{
        try
        {
            InitializeComponent();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public ShowAllData(ObservableCollection<usermeasurement> usermeasurementlistp, ObservableCollection<usermeasurement> allusermeasurementsp, ObservableCollection<measurement> allmeasurementlistpassed, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();

            usermeasurementlistpassed = usermeasurementlistp;

            measurementname.Text = usermeasurementlistpassed[0].measurementname;

            allusermeasurements = allusermeasurementsp;
            allmeasurementlist = allmeasurementlistpassed;
            userfeedbacklistpassed = userfeedbacklist;

            foreach (var item in usermeasurementlistpassed)
            {
                item.isnotsleepduration = true;
                item.issleepduration = false; 

                var dt = DateTime.Parse(item.inputdatetime);
                item.datechanged = dt.ToString("HH:mm, dd/MM/yyyy");
                if(item.unit == "Stones/Pounds")
                {
                    if (item.value.Contains("st"))
                    {

                    }
                    else
                    {
                        var split = item.value.Split('.');
                        var Newlbl = split[0] + "st" + " " + split[1] + "lbs";
                        item.value = Newlbl;
                    }
                }
                else if(item.unit == "Hours/Minutes")
                {
                    if(item.inputmethod != null)
                    {
                        //Sleep Score Added
                        item.isnotsleepduration = false;
                        item.issleepduration = true;
                    }
                    else
                    {
                        //No Sleep Score Added
                        item.isnotsleepduration = true;
                        item.issleepduration = false;
                    }
                }
            }

         

            usermeasurementlistpassed = new ObservableCollection<usermeasurement>(usermeasurementlistpassed.OrderByDescending(x => DateTime.Parse(x.inputdatetime)));

          //  if(usermeasurementlistpassed.Count > 50)
           // {
             //   var newlist = usermeasurementlistpassed.Take(50).ToList();
               // usermeasurementlist.ItemsSource = newlist;
               // usermeasurementlist.HeightRequest = newlist.Count * 100;
           // }


            displayedMeasurements.Clear();
            currentIndex = 0;
            LoadMoreItems(); // load the first batch
            usermeasurementlist.ItemsSource = displayedMeasurements;
           // usermeasurementlist.HeightRequest = displayedMeasurements.Count * 110;

            int itemHeight = 100; // estimate per item
            int maxVisibleItems = 50; // or however many you want to show max

           // var visibleCount = Math.Min(displayedMeasurements.Count, maxVisibleItems);
          //  usermeasurementlist.HeightRequest = displayedMeasurements.Count * itemHeight;
            // usermeasurementlist.HeightRequest = displayedMeasurements.Count * 100;

            // usermeasurementlist.ItemsSource = usermeasurementlistpassed;
            //usermeasurementlist.HeightRequest = usermeasurementlistpassed.Count * 100;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }  
    }

    void LoadMoreItems()
    {
        try
        {


            var itemsToLoad = usermeasurementlistpassed.Skip(currentPage * PageSize).Take(PageSize).ToList();

            foreach (var item in itemsToLoad)
                displayedMeasurements.Add(item);

            currentPage++;

            //if (isLoading) return;
            //isLoading = true;

            //var nextBatch = usermeasurementlistpassed.Skip(currentIndex).Take(batchSize).ToList();
            //foreach (var item in nextBatch)
            //    displayedMeasurements.Add(item);
            //usermeasurementlist.HeightRequest = displayedMeasurements.Count * 100;
            //currentIndex += nextBatch.Count;
            //isLoading = false;
        }
        catch(Exception ex)
        {

        }
    }


    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
              //  toolbaritem.IsEnabled = false;
                //edit button clicked
                usermeasurementlist.IsEnabled = true;
                deletelbl.IsVisible = true;
                // Iterate over the source collection and add each item to SelectedItems
                foreach (var item in displayedMeasurements)
                {
                    item.Deleteisvis = true;
                }

                this.ToolbarItems.Clear();


                ToolbarItem itemm = new ToolbarItem
                {
                    Text = "Delete"

                };

                itemm.Clicked += OnItemClicked;

                // "this" refers to a Page object
                this.ToolbarItems.Add(itemm);

                //  usermeasurementlist.SelectionMode = Syncfusion.Maui.ListView.SelectionMode.None;
                //toolbaritem.IsEnabled = true;
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

    private async void OnItemClicked(object sender, EventArgs e)
    {
        try
        {
            //done toolbar item clicked

            var getdeleteditems = displayedMeasurements.Where(x => x.Deleteisvis == true).ToList();


            if(getdeleteditems != null || getdeleteditems.Count > 0)
            {

            }

            if(deleeteusermeasurementlistpassed.Count == 0)
            {
                this.ToolbarItems.Clear();
                ToolbarItem items = new ToolbarItem
                {
                    Text = "Edit"

                };

                items.Clicked += ToolbarItem_Clicked;
                this.ToolbarItems.Add(items);
                //edit button clicked
                usermeasurementlist.IsEnabled = false;
                deletelbl.IsVisible = false;
                foreach (var itemss in usermeasurementlistpassed)
                {
                    itemss.Deleteisvis = false;
                }
                return;
            }

            if(deleeteusermeasurementlistpassed.Count == usermeasurementlistpassed.Count)
            {
                Result = await DisplayAlert("Delete Feedback", "Deleting this feedback will delete this measurement, are you sure you want to delete?", "Cancel", "Delete");
            }
            else 
            {
                Result = await DisplayAlert("Delete Feedback", "This permanetly deletes this feedback, are you sure you want to delete?", "Cancel", "Delete");
            }

            if (Result)
            {
                return;
            }


            deletelbl.IsVisible = false;

            foreach (var meas in usermeasurementlistpassed)
            {
                meas.Deleteisvis = false;
            }

          

 
            //delete item in local collection
            // Find items in the first collection that are also in the second collection
            var itemsToRemove = usermeasurementlistpassed.Where(item => deleeteusermeasurementlistpassed.Contains(item)).ToList();

            // Remove each item in the 'itemsToRemove' list from the 'firstCollection'
            foreach (var x in itemsToRemove)
            {
                usermeasurementlistpassed.Remove(x);
                allusermeasurements.Remove(x);
            }

            foreach(var y in deleeteusermeasurementlistpassed)
            {
                y.deleted = true;
            }

            //update single view
            //WeakReferenceMessenger.Default.Send(new SendItemMessage(usermeasurementlistpassed));

            var filteredMeasurements = allusermeasurements
        .GroupBy(m => m.measurementid)
        .Select(g => g.OrderByDescending(m => DateTime.Parse(m.inputdatetime))
        .First())
        .OrderByDescending(x => DateTime.Parse(x.inputdatetime))
        .ToList();

            // Convert the filtered and sorted list to an ObservableCollection
            ObservableCollection<usermeasurement> observableFilteredMeasurements = new ObservableCollection<usermeasurement>(filteredMeasurements);

            //update main page
            //WeakReferenceMessenger.Default.Send(new UpdateListMainPage(observableFilteredMeasurements));
            

            await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Feedback Updated") { });

            //await Task.Delay(1500);
            //delete the items
            if (deleeteusermeasurementlistpassed.Count > 0)
            {

                //Delete items from User Measurment
                await aPICalls.DeleteUserMeasurements(deleeteusermeasurementlistpassed);

                //update UserMeasurement Feedback 

                if (userfeedbacklistpassed.measurementfeedbacklist == null)
                {
                    userfeedbacklistpassed.measurementfeedbacklist = new ObservableCollection<feedbackdata>();
                }

                foreach (var x in userfeedbacklistpassed.measurementfeedbacklist)
                {
                    if (x.id != null)
                    {
                        for (int i = 0; i < deleeteusermeasurementlistpassed.Count; i++)
                        {
                            if (x.id == deleeteusermeasurementlistpassed[i].id)
                            {
                                x.action = "deleted";
                            }
                        }
                    }
                }

                string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.measurementfeedbacklist);
                userfeedbacklistpassed.measurementfeedback = newsymJson;

                await aPICalls.UserfeedbackUpdateMeasurementData(userfeedbacklistpassed);

            }



            //if (usermeasurementlistpassed.Count == 0)
            //{
            //    datastack.IsVisible = false;
            //    nodatastack.IsVisible = true;
            //    this.ToolbarItems.Clear();



            //}
            //else
            //{
            //add the edit button back 
            this.ToolbarItems.Clear();
                ToolbarItem item = new ToolbarItem
                {
                    Text = "Edit"

                };

                item.Clicked += ToolbarItem_Clicked;
                this.ToolbarItems.Add(item);
                //edit button clicked
                usermeasurementlist.IsEnabled = false;
               deletelbl.IsVisible = false;
            foreach (var items in usermeasurementlistpassed)
                {
                    items.Deleteisvis = false;
                }


            //userfeedbacklistpassed Comes from Dashboard so needs to be linked back to Measurements page
            await Navigation.PushAsync(new MeasurementsPage(allusermeasurements, allmeasurementlist, userfeedbacklistpassed));
            var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is MeasurementsPage);
            var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleMeasurement);

            if (pageToRemove != null)
            {
                Navigation.RemovePage(pageToRemove);
            }
            if (pageToRemoves != null)
            {
                Navigation.RemovePage(pageToRemoves);
            }
            Navigation.RemovePage(this);

            //}

            await Task.Delay(3000);

            await MopupService.Instance.PopAllAsync(false);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private void OnItemEdittwoClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        usermeasurementlist.IsEnabled = true;
    //        deletelbl.IsVisible = true;
    //        // Iterate over the source collection and add each item to SelectedItems
    //        foreach (var item in usermeasurementlistpassed)
    //        {
    //            item.Deleteisvis = true;
    //        }

    //        this.ToolbarItems.Clear();


    //        ToolbarItem itemm = new ToolbarItem
    //        {
    //            Text = "Done"

    //        };

    //        itemm.Clicked += OnItemClicked;

    //        // "this" refers to a Page object
    //        this.ToolbarItems.Add(itemm);

    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void usermeasurementlist_ItemAppearing(object sender, Syncfusion.Maui.ListView.ItemAppearingEventArgs e)
    {
        try
        {
            // If we're within the last 5 items, load more
            if (isLoading || currentIndex >= usermeasurementlistpassed.Count)
                return;

            if (e.DataItem == displayedMeasurements[displayedMeasurements.Count - 1])
            {
                LoadMoreItems();
            }
        }
        catch(Exception Ex)
        {

        }
    }

    private void LoadMoreButton_Clicked(object sender, EventArgs e)
    {

    }

    private void usermeasurementlist_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        try
        {
            // Load next page
            if ((currentPage * PageSize) < usermeasurementlistpassed.Count)
            {
                LoadMoreItems();
            }
        }
        catch(Exception ex)
        {

        }
    }

    private async void usermeasurementlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (IsNavigating) return;
                IsNavigating = true;
                var SelectedItem = e.CurrentSelection.FirstOrDefault() as usermeasurement;

                //Stops Code Running again when selected
                if (SelectedItem == null)
                {
                    IsNavigating = false;
                    return;
                }

                // Stops code from running again 
                usermeasurementlist.SelectedItem = null;

                if (SelectedItem != null)
                {
                    bool IsEditPage = true;
                    await Navigation.PushAsync(new AddMeasurement(SelectedItem, allusermeasurements, allmeasurementlist, userfeedbacklistpassed, IsEditPage));
                    IsNavigating = false;
                }
                IsNavigating = false;

            }
            else
            {
                IsNavigating = false;
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception ex)
        {
            IsNavigating = false;
            NotasyncMethod(ex);
        }
    }
}