using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Maui.Layouts;
using Mopups.PreBaked.AbstractClasses;
using Mopups.Services;
using Newtonsoft.Json;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.Json;

namespace PeopleWith;

public partial class AddDiet : ContentPage
{
    public ObservableCollection<diet> AllDiets = new ObservableCollection<diet>();
    public ObservableCollection<diet> FilterResults = new ObservableCollection<diet>();
    public ObservableCollection<diet> FilterTabsList = new ObservableCollection<diet>();
    public ObservableCollection<userdiet> AllUserDiets = new ObservableCollection<userdiet>();
    public userdiet NewuserDiet = new userdiet();
    public userdiet DietPassed = new userdiet();
    public userNotesFeedback NotesPassed = new userNotesFeedback();
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
    public AddDiet(ObservableCollection<userdiet> userDiets)
	{
		InitializeComponent();
        AllUserDiets = userDiets;
        GetDietInfo();

    }

    public AddDiet(userdiet UserDietPassed)
    {
        InitializeComponent();
        DietPassed = UserDietPassed;
        FirstStack.IsVisible = false;
        AddNotesStack.IsVisible = true;
        Notestitlelbl.Text = DietPassed.diettitle;
        adddatepicker.MaximumDate = DateTime.Now;
        addtimepicker.Time = DateTime.Now.TimeOfDay;

    }

    public AddDiet(userdiet UserDietPassed, bool Editis)
    {
        InitializeComponent();
        DietPassed = UserDietPassed;
        isEdit = Editis; 
        FirstStack.IsVisible = false;
        SecondStack.IsVisible = true;
        Diettitlelbl.Text = DietPassed.diettitle;

        if (!String.IsNullOrEmpty(DietPassed.dateended))
        {
            //Set CheckBox
            enddatecheck.IsChecked = true;
            enddatepicker.Date = DateTime.Parse(DietPassed.dateended).Date;
        }
        startdatepicker.Date = DateTime.Parse(DietPassed.datestarted).Date;

        var DateCreated = DietPassed.createdAt.ToString("dd/MM/yyyy HH:mm");

        AddDietBtn.Text = "Update Diet";

        foreach (var item in DietPassed.notes)
        {
            if(item.datetime == DateCreated)
            {
                Notes.Text = item.notes;
                NoteUpdate = true; 
                return; 
            }
        }

    }

    public AddDiet(userdiet UserDietPassed, userNotesFeedback PassedNotes)
    {
        InitializeComponent();
        DietPassed = UserDietPassed;
        NotesPassed = PassedNotes; 
        FirstStack.IsVisible = false;
        AddNotesStack.IsVisible = true;
        Notestitlelbl.Text = DietPassed.diettitle;
        adddatepicker.MaximumDate = DateTime.Now;
        addtimepicker.Time = DateTime.Now.TimeOfDay;

        adddatepicker.Date = DateTime.Parse(NotesPassed.datetime).Date;
        addtimepicker.Time = DateTime.Parse(NotesPassed.datetime).TimeOfDay;

        NewNotes.Text = NotesPassed.notes;

        AddNotesBtn.Text = "Update Notes";
        DeleteNotesBtn.IsVisible = true;
        Deletelbl.IsVisible = true;
    }

    async private void GetDietInfo()
    {
        try
        {
            Dietloading.IsVisible = true;
            var Userid = Helpers.Settings.UserKey;
            APICalls database = new APICalls();

            //Get Diet Details 
            var getDietTask = database.GetDietDetails();
            AllDiets = await getDietTask;

            //Remove Already Added items from List
            if(AllUserDiets.Count > 0)
            {
                var DietAdded = new ObservableCollection<diet>(AllDiets.Where(s => AllUserDiets.Any(x => x.diettitle == s.diettitle))); 

                foreach (var item in DietAdded)
                {
                    AllDiets.Remove(item);
                }
            }
            //FilterTabs 
            DietListview.ItemsSource = AllDiets.OrderBy(s => s.diettitle);
            var count = AllDiets.Count.ToString();
            //Results inital count
            Results.Text = "Results" + " (" + count + ")";
            FilterResults = AllDiets;


            //Add Classiciation Filters 
            var distinctDiet = AllDiets
                .GroupBy(s => s.ShortGroup)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.ShortGroup);

            FilterTabsList = new ObservableCollection<diet>(distinctDiet);

            // Insert "All" at the beginning of the list
            var AddAll = new diet
            {
                ShortGroup = "All"
            };
            FilterTabsList.Insert(0, AddAll);
            FilterTabs.ItemsSource = FilterTabsList;
            FilterTabs.DisplayMemberPath = "ShortGroup";
            Filterstack.IsVisible = true;
            //Symptomloading.IsVisible = false;

            Dietloading.IsVisible = false;
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if(Filterstack.IsVisible == false)
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

    private async void DietListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
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
                var Diet = e.DataItem as diet;
                var Diettitle = Diet.diettitle.ToString();
                bool result = await DisplayAlert("Confirm Diet", "Are you sure you want to add " + Diettitle + "?", "OK", "Cancel");


                if (result)
                {
                    FirstStack.IsVisible = false;
                    SecondStack.IsVisible = true;
                    Diettitlelbl.Text = Diet.diettitle;
                    NewuserDiet.diettitle = Diet.diettitle;
                    NewuserDiet.dietid = Diet.dietid;
                    NewuserDiet.userid = Helpers.Settings.UserKey; 
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
                    DietListview.SelectedItems.Clear();
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
                DietListview.ItemsSource = FilterResults.OrderBy(s => s.diettitle);
                DietListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                searchbar.Text = String.Empty;
            }
            else
            {

                var filteredDiet = new ObservableCollection<diet>(FilterResults.Where(s => s.ShortGroup.Contains(item, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.diettitle);
                var count = filteredDiet.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                DietListview.ItemsSource = filteredDiet;
                DietListview.IsVisible = true;
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
            var filteredDiet = new ObservableCollection<diet>(FilterResults.Where(s => s.diettitle.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.diettitle);
            DietListview.ItemsSource = filteredDiet;
            var count = filteredDiet.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";

            if (count == "0")
            {
                NoResultslbl.IsVisible = true;
                DietListview.IsVisible = false;
            }
            else
            {
                DietListview.IsVisible = true;
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
                DietListview.SelectedItem = null;
                searchbar.Focus();
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void enddatecheck_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            if (!e.Value)
            {
                //ischecked
                enddategrid.Opacity = 0.2;
                enddatepicker.IsEnabled = false;
                NewuserDiet.dateended = null;
            }
            else
            {
                //ischecked
                enddategrid.Opacity = 1;
                enddatepicker.IsEnabled = true;
                enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);

            }


        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void startdatepicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            enddatepicker.MinimumDate = startdatepicker.Date.AddDays(1);
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private async void AddDietBtn_Clicked(object sender, EventArgs e)
    {
        try
        { 
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                AddDietBtn.IsEnabled = false;

                if (enddatecheck.IsChecked)
                {
                    NewuserDiet.dateended = enddatepicker.Date.ToString("dd/MM/yyyy");
                }

                NewuserDiet.datestarted = startdatepicker.Date.ToString("dd/MM/yyyy");
          
                //Add Notes 

                if (!string.IsNullOrEmpty(Notes.Text))
                {
                    var NotesFeedback = new userNotesFeedback();
                    Random random = new Random();
                    int notid = random.Next(100000, 100000001);
                    NotesFeedback.id = notid.ToString();
                    NotesFeedback.deleted = "Active";
                    var CurrentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    NotesFeedback.datetime = CurrentDateTime;
                    NotesFeedback.notes = Notes.Text;
                    if (NewuserDiet.notes == null)
                    {
                        NewuserDiet.notes = new ObservableCollection<userNotesFeedback>();
                    }
                    NewuserDiet.notes.Add(NotesFeedback);
                }

                APICalls database = new APICalls();

                //Update userDiet Table 
                if (isEdit == true)
                {
                    DietPassed.datestarted = NewuserDiet.datestarted;
                    if (enddatecheck.IsChecked)
                    {
                        DietPassed.dateended = NewuserDiet.dateended;
                    }

                    //update Date
                    if (!string.IsNullOrEmpty(Notes.Text))
                    {
                        if (NoteUpdate == true)
                        {
                            var DateCreated = DietPassed.createdAt.ToString("dd/MM/yyyy HH:mm");
                            //Update First item
                            foreach (var item in DietPassed.notes)
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
                            DietPassed.notes.Add(NewuserDiet.notes[0]);
                        }
                    }
                    else
                    {
                        if (NoteUpdate == true)
                        {
                            var DateCreated = DietPassed.createdAt.ToString("dd/MM/yyyy HH:mm");
                            //Update First item
                            foreach (var item in DietPassed.notes)
                            {
                                if (item.datetime == DateCreated)
                                {
                                    item.deleted = "deleted";
                                }
                            }
                        }
                    }
                  

                    await database.UpdateUserDiet(DietPassed);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Diet Updated") { });
                    await Task.Delay(1500);
                }
                else
                {
                    NewuserDiet = await database.PostUserDiet(NewuserDiet);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Diet Added") { });
                    await Task.Delay(1500);
                }
            
                await MopupService.Instance.PopAllAsync(false);

                await Navigation.PushAsync(new AllDiet());
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(p => p is AllDiet);
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleDiet);
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

                AddDietBtn.IsEnabled = true;
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

    private async void AddNotesBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                AddNotesBtn.IsEnabled = false;
                if (string.IsNullOrEmpty(NewNotes.Text))
                {
                    Vibration.Vibrate();
                    NewNotes.Focus();
                    AddNotesBtn.IsEnabled = true;
                    NotesEmpty.IsVisible = true;
                    Task.Run(async () =>
                    {
                        await Task.Delay(5000);
                        NotesEmpty.IsVisible = false;
                    });
                    return;
                }

                //Add New Notes 
                var NotesFeedback = new userNotesFeedback();
                NotesFeedback.deleted = "Active";
                var Date = adddatepicker.Date.ToString("dd/MM/yyyy");
                var Time = addtimepicker.Time.ToString(@"hh\:mm");
                NotesFeedback.datetime = Date + " " + Time;
                NotesFeedback.notes = NewNotes.Text;
                if (DietPassed.notes == null)
                {
                    DietPassed.notes = new ObservableCollection<userNotesFeedback>();
                }

                APICalls database = new APICalls();

                if (AddNotesBtn.Text == "Add Notes")
                {
                    Random random = new Random();
                    int notid = random.Next(100000, 100000001);
                    NotesFeedback.id = notid.ToString();

                    DietPassed.notes.Add(NotesFeedback);
                    await database.AddDietNotesAsymc(DietPassed);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Diet Notes Added") { });
                    await Task.Delay(1500);
                }
                else
                {
                    foreach(var item in DietPassed.notes)
                    {
                        if(item.id == NotesPassed.id)
                        {
                            item.datetime = NotesFeedback.datetime;
                            item.notes = NotesFeedback.notes;
                        }
                    }


                    await database.AddDietNotesAsymc(DietPassed);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Diet Notes Updated") { });
                    await Task.Delay(1500);
                }
               

                await MopupService.Instance.PopAllAsync(false);

                await Navigation.PushAsync(new AllDiet());
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(p => p is AllDiet);
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleDiet);
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
               
                Navigation.RemovePage(this);

                AddNotesBtn.IsEnabled = true;
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

                foreach (var item in DietPassed.notes)
                {
                    if (item.id == NotesPassed.id)
                    {
                        item.deleted = "deleted";
                    }
                }

                await database.AddDietNotesAsymc(DietPassed);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Diet Notes Deleted") { });
                await Task.Delay(1500);

                await MopupService.Instance.PopAllAsync(false);

                await Navigation.PushAsync(new AllDiet());
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(p => p is AllDiet);
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleDiet);
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