using CommunityToolkit.Mvvm.Messaging;
using Mopups.PreBaked.AbstractClasses;
using Mopups.Services;
using Newtonsoft.Json;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;


namespace PeopleWith;

public partial class AddInvestigations : ContentPage
{
    public ObservableCollection<investigation> AllInvests = new ObservableCollection<investigation>();
    public ObservableCollection<investigation> FilterResults = new ObservableCollection<investigation>();
    public ObservableCollection<investigation> FilterTabsList = new ObservableCollection<investigation>();
    public ObservableCollection<userinvestigation> AllUserInvests = new ObservableCollection<userinvestigation>();
    public userinvestigation NewuserInvest = new userinvestigation();
    public userinvestigation InvestPassed = new userinvestigation();
    public notesuserfeedback NotesPassed = new notesuserfeedback();
    public bool isEdit = false;
    public bool NoteUpdate = false;
    private bool FilterTabClicked = false;

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

    public AddInvestigations(ObservableCollection<userinvestigation> userInvest)
    {
        InitializeComponent();
        AllUserInvests = userInvest;
        GetDietInfo();

    }

    public AddInvestigations(userinvestigation UserInvestPassed)
    {
        InitializeComponent();
        InvestPassed = UserInvestPassed;
        FirstStack.IsVisible = false;
        SecondStack.IsVisible = true;
        Investtitlelbl.Text = InvestPassed.investigationname;
        adddatepicker.MaximumDate = DateTime.Now;
        addtimepicker.Time = DateTime.Now.TimeOfDay;
        AddInvestBtn.Text = "Add Investigation Notes";
    }

    public AddInvestigations(userinvestigation UserInvestPassed, bool Editis)
    {
        InitializeComponent();
        InvestPassed = UserInvestPassed;
        isEdit = Editis;
        FirstStack.IsVisible = false;
        SecondStack.IsVisible = true;
        Investtitlelbl.Text = InvestPassed.investigationname;

        adddatepicker.Date = DateTime.Parse(InvestPassed.investigationdate).Date;
        addtimepicker.Time = DateTime.Parse(InvestPassed.investigationdate).TimeOfDay;

        var DateCreated = InvestPassed.createdAt.ToString("dd/MM/yyyy HH:mm");

        AddInvestBtn.Text = "Update Investigation";

        foreach (var item in InvestPassed.notes)
        {
            if (item.datetime == DateCreated)
            {
                Notes.Text = item.notes;
                NoteUpdate = true;
                return;
            }
        }

    }

    public AddInvestigations(userinvestigation UserInvestPassed, notesuserfeedback PassedNotes)
    {
        InitializeComponent();
        InvestPassed = UserInvestPassed;
        NotesPassed = PassedNotes;
        FirstStack.IsVisible = false;
        SecondStack.IsVisible = true;
        Investtitlelbl.Text = InvestPassed.investigationname;
        adddatepicker.MaximumDate = DateTime.Now;
        addtimepicker.Time = DateTime.Now.TimeOfDay;

        adddatepicker.Date = DateTime.Parse(NotesPassed.datetime).Date;
        addtimepicker.Time = DateTime.Parse(NotesPassed.datetime).TimeOfDay;

        Notes.Text = NotesPassed.notes;

        AddInvestBtn.Text = "Update Notes";
        DeleteNotesBtn.IsVisible = true;
        Deletelbl.IsVisible = true;
    }

    async private void GetDietInfo()
    {
        try
        {
            Investloading.IsVisible = true;
            var Userid = Helpers.Settings.UserKey;
            APICalls database = new APICalls();

            //Get Investigation Details 
            var getInvestTask = database.GetInvestigationDetails();
            AllInvests = await getInvestTask;

            //Remove Already Added items from List
            if (AllInvests.Count > 0)
            {
                var InvestAdded = new ObservableCollection<investigation>(AllInvests.Where(s => AllUserInvests.Any(x => x.investigationname == s.investigationtitle)));

                foreach (var item in InvestAdded)
                {
                    AllInvests.Remove(item);
                }
            }
            //FilterTabs 
            InvestListview.ItemsSource = AllInvests.OrderBy(s => s.investigationtitle);
            var count = AllInvests.Count.ToString();
            //Results inital count
            Results.Text = "Results" + " (" + count + ")";
            FilterResults = AllInvests;


            //Add Classiciation Filters 
            var distinctinvest = AllInvests
                .GroupBy(s => s.ShortGroup)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.ShortGroup);

            FilterTabsList = new ObservableCollection<investigation>(distinctinvest);

            // Insert "All" at the beginning of the list
            var AddAll = new investigation
            {
                ShortGroup = "All"
            };
            FilterTabsList.Insert(0, AddAll);
            FilterTabs.ItemsSource = FilterTabsList;
            FilterTabs.DisplayMemberPath = "ShortGroup";
            Filterstack.IsVisible = true;
            //Symptomloading.IsVisible = false;

            addtimepicker.Time = DateTime.Now.TimeOfDay;

            Investloading.IsVisible = false;
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public AddInvestigations()
    {
        InitializeComponent();
    }

    private void Backbutton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (SecondStack.IsVisible == true)
            {
                AndroidBtn.IsVisible = false;
                IOSBtn.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                FirstStack.IsVisible = true;
                SecondStack.IsVisible = false;
                InvestListview.SelectedItem = null;
                searchbar.Focus();
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FilterTabs_ChipClicked(object sender, EventArgs e)
    {
        try
        {
            var tappedFrame = sender as SfChip;
            var item = tappedFrame.Text;
            FilterTabClicked = true;

            if (item == "All")
            {
                var count = FilterResults.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                InvestListview.ItemsSource = FilterResults.OrderBy(s => s.investigationtitle);
                InvestListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;
            }
            else
            {

                var filteredInvest = new ObservableCollection<investigation>(FilterResults.Where(s => s.ShortGroup.Contains(item, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.investigationtitle);
                var count = filteredInvest.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                InvestListview.ItemsSource = filteredInvest;
                InvestListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;

            }


            Device.BeginInvokeOnMainThread(() =>
            {
                FilterTabClicked = false;
            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (FilterTabClicked) return;

            var Characters = searchbar.Text.ToString();
            var filteredInvest = new ObservableCollection<investigation>(FilterResults.Where(s => s.investigationtitle.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.investigationtitle);
            InvestListview.ItemsSource = filteredInvest;
            var count = filteredInvest.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";

            if (count == "0")
            {
                NoResultslbl.IsVisible = true;
                InvestListview.IsVisible = false;
            }
            else
            {
                InvestListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
            }

            //If FilterTabs item is Selected - UnSelect it 
            if (string.IsNullOrEmpty(searchbar.Text) || searchbar.Text == "")
            {
                FilterTabs.SelectedItem = FilterTabsList[0];
            }
            else
            {
                if (FilterTabs.SelectedItem != null)
                {
                    FilterTabs.SelectedItem = null;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (Filterstack.IsVisible == false)
            {
                Filterstack.IsVisible = true;
            }
            else
            {
                Filterstack.IsVisible = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void InvestListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                if (searchbar.IsSoftInputShowing())
                    await searchbar.HideSoftInputAsync(System.Threading.CancellationToken.None);
                var Invest = e.DataItem as investigation;
                var Investitle = Invest.investigationtitle.ToString();
                bool result = await DisplayAlert("Confirm Investigation", "Are you sure you want to add " + Investitle + "?", "OK", "Cancel");


                if (result)
                {
                    FirstStack.IsVisible = false;
                    SecondStack.IsVisible = true;
                    Investtitlelbl.Text = Invest.investigationtitle;
                    NewuserInvest.investigationname = Invest.investigationtitle;
                    NewuserInvest.investigationid = Invest.investigationid;
                    NewuserInvest.userid = Helpers.Settings.UserKey;
                    NavigationPage.SetHasNavigationBar(this, false);
                    if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                    {
                        AndroidBtn.IsVisible = true;
                    }
                    else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                    {
                        IOSBtn.IsVisible = true;
                    }

                }
                else
                {
                    // Declined
                    InvestListview.SelectedItems.Clear();
                }

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

    private void adddatepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }

    private void addtimepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }

    private async void AddInvestBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                AddInvestBtn.IsEnabled = false;
                var Date = adddatepicker.Date.ToString("dd/MM/yyyy");
                var Time = addtimepicker.Time.ToString(@"hh\:mm");
                NewuserInvest.investigationdate = Date + " " + Time;

                //Add Notes 

                if (!string.IsNullOrEmpty(Notes.Text))
                {
                    var NotesFeedback = new notesuserfeedback();
                    Random random = new Random();
                    int notid = random.Next(100000, 100000001);
                    NotesFeedback.id = notid.ToString();
                    NotesFeedback.deleted = "Active";
                    var CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    NotesFeedback.datetime = CurrentDateTime;
                    NotesFeedback.notes = Notes.Text;
                    if (NewuserInvest.notes == null)
                    {
                        NewuserInvest.notes = new ObservableCollection<notesuserfeedback>();
                    }
                    NewuserInvest.notes.Add(NotesFeedback);
                }

                APICalls database = new APICalls();

                //Update UserINvestigation Table 
                if (isEdit == true)
                {
                    InvestPassed.investigationdate = NewuserInvest.investigationdate;

                    //update Date
                    if (!string.IsNullOrEmpty(Notes.Text))
                    {
                        if (NoteUpdate == true)
                        {
                            var DateCreated = InvestPassed.createdAt.ToString("dd/MM/yyyy HH:mm");
                            //Update First item
                            foreach (var item in InvestPassed.notes)
                            {
                                if (item.datetime == DateCreated)
                                {
                                    item.notes = Notes.Text;
                                }
                            }
                        }
                        else
                        {
                            //Add New item 
                            InvestPassed.notes.Add(NewuserInvest.notes[0]);
                        }
                    }
                    else
                    {
                        if (NoteUpdate == true)
                        {
                            var DateCreated = InvestPassed.createdAt.ToString("dd/MM/yyyy HH:mm");
                            //Update First item
                            foreach (var item in InvestPassed.notes)
                            {
                                if (item.datetime == DateCreated)
                                {
                                    item.deleted = "deleted";
                                }
                            }
                        }
                    }


                    await database.UpdateUserInvestigation(InvestPassed);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Investigation Updated") { });
                    await Task.Delay(1500);
                }
                else
                {
                   
                    if(AddInvestBtn.Text == "Add Investigation")
                    {
                        //Add New Investigation
                        NewuserInvest = await database.PostUserInvestigation(NewuserInvest);

                        await MopupService.Instance.PushAsync(new PopupPageHelper("Investigation Added") { });
                        await Task.Delay(1500);

                    }
                    else
                    {
                        //Add Notes 
                        InvestPassed.notes.Add(NewuserInvest.notes[0]);
                        await database.AddInvestigationNotesAsync(InvestPassed);

                        await MopupService.Instance.PushAsync(new PopupPageHelper("Investigation Notes Added") { });
                        await Task.Delay(1500);
                    }
                   
                }

                await MopupService.Instance.PopAllAsync(false);

                await Navigation.PushAsync(new AllInvestigations());
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(p => p is AllInvestigations);
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleInvestigations);
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

                AddInvestBtn.IsEnabled = true;
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

    private async void DeleteNotesBtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                DeleteNotesBtn.IsEnabled = false;

                APICalls database = new APICalls();

                foreach (var item in InvestPassed.notes)
                {
                    if (item.id == NotesPassed.id)
                    {
                        item.deleted = "deleted";
                    }
                }

                await database.AddInvestigationNotesAsync(InvestPassed);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Investigation Notes Deleted") { });
                await Task.Delay(1500);

                await MopupService.Instance.PopAllAsync(false);

                await Navigation.PushAsync(new AllInvestigations());
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(p => p is AllInvestigations);
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleInvestigations);
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

                DeleteNotesBtn.IsEnabled = true;
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