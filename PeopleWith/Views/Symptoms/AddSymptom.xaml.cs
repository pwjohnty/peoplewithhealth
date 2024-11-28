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
namespace PeopleWith;
public partial class AddSymptom : ContentPage
{
    public ObservableCollection<symptom> FilterResults = new ObservableCollection<symptom>();
    public ObservableCollection<symptom> FilterTabsList = new ObservableCollection<symptom>();
    public ObservableCollection<usersymptom> AddNewSymptom = new ObservableCollection<usersymptom>();
    public ObservableCollection<symptomfeedback> AddNewFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<symptom> itemstoremove = new ObservableCollection<symptom>();
    public ObservableCollection<usersymptom> SymptomsPassed = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> UpdatedAllUserSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<symptom> AlreadyAdded = new ObservableCollection<symptom>();
  // private PopupViewModel viewModel;
    string previous;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public AddSymptom(ObservableCollection<usersymptom> ItemsPassed, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            SymptomsPassed = ItemsPassed;
            userfeedbacklistpassed = userfeedbacklist;
            GetBCCall();
            //viewModel = BindingContext as PopupViewModel;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddSymptom(ObservableCollection<usersymptom> ItemsPassed)
    {
        try
        {
            InitializeComponent();
            SymptomsPassed = ItemsPassed;
            GetBCCall();
            //viewModel = BindingContext as PopupViewModel;
        }
        catch(Exception Ex) 
        {
            NotasyncMethod(Ex);
        }
    }
    async public void GetBCCall()
    {
        try
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();  
            Symptomloading.IsVisible = true;
            //Database call for All Symptoms 
            APICalls database = new APICalls();
            ObservableCollection<symptom> userSymptoms = await database.GetSymptomSearchAsync();
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //await DisplayAlert("Time", ts.ToString(), "Close");
            //Remove Pending and inactive symptoms 
            foreach (var item in userSymptoms)
            {
                if (item.status != "active")
                {
                    itemstoremove.Add(item);
                }
                //Symptom Classification = null, Change to 'Other' 
                if (item.classification == null)
                {
                    item.classification = "Other";
                }
            }
            //Get Symptoms user already added to remove from listview
            var SymptomAdded = new ObservableCollection<symptom>(userSymptoms.Where(s => SymptomsPassed.Any(x => x.symptomtitle == s.title)));
            foreach (var item in SymptomAdded)
            {
                itemstoremove.Add(item);
            }
            //Remove items from list
            foreach (var item in itemstoremove)
            {
                userSymptoms.Remove(item);
            }
            SymptomsListview.ItemsSource = userSymptoms.OrderBy(s => s.title);
            var count = userSymptoms.Count.ToString();
            //Results inital count
            Results.Text = "Results" + " (" + count + ")";
            FilterResults = userSymptoms;
            //Add Classiciation Filters 
            var distinctSymptoms = userSymptoms
                .GroupBy(s => s.classification)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.classification);
            FilterTabsList = new ObservableCollection<symptom>(distinctSymptoms);
            FilterTabs.ItemsSource = FilterTabsList;
            FilterTabs.DisplayMemberPath = "classification";
            Symptomloading.IsVisible = false;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void SymptomsListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                var symptom = e.DataItem as symptom;

                var result = await DisplayAlert("Confirm Symptom", "Are you sure you want to add " + symptom.title + "?", "Cancel", "OK");
                if (result)
                {

                    SymptomsListview.SelectedItems.Clear();
                    return;
                }
                else
                {
                    // popup.HeaderTitle = symptom.title.ToString();
                    //  popup.IsOpen = true;
                    var usersymptom = new symptomfeedback();
                    Guid newUUID = Guid.NewGuid();
                    usersymptom.symptomfeedbackid = newUUID.ToString().ToUpper();
                    usersymptom.timestamp = DateTime.Now.ToString();
                    usersymptom.intensity = "50";
                    usersymptom.notes = null;
                    usersymptom.triggers = null;
                    usersymptom.interventions = null;
                    usersymptom.duration = null;
                    AddNewFeedback.Add(usersymptom);
                    var NewSymptom = new usersymptom();
                    NewSymptom.userid = Helpers.Settings.UserKey;
                    NewSymptom.symptomtitle = symptom.title;
                    NewSymptom.symptomid = symptom.symptomid;
                    NewSymptom.feedback = AddNewFeedback;


                    APICalls database = new APICalls();
                    //insert to db
                    var returnedsymptom = await database.PostSymptomAsync(NewSymptom);

                    SymptomsPassed.Add(returnedsymptom);

                    var newsym = new feedbackdata();
                    newsym.value = "50";
                    newsym.datetime = AddNewFeedback[0].timestamp;
                    newsym.action = "add";
                    newsym.label = NewSymptom.symptomtitle;
                    newsym.id = usersymptom.symptomfeedbackid;

                    if (userfeedbacklistpassed.symptomfeedbacklist == null)
                    {
                        userfeedbacklistpassed.symptomfeedbacklist = new ObservableCollection<feedbackdata>();
                    }

                    userfeedbacklistpassed.symptomfeedbacklist.Add(newsym);

                    string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.symptomfeedbacklist);
                    userfeedbacklistpassed.symptomfeedback = newsymJson;


                    await database.UserfeedbackUpdateSymptomData(userfeedbacklistpassed);


                    await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Added") { });
                    await Task.Delay(1500);

                    await Navigation.PushAsync(new AllSymptoms(SymptomsPassed, userfeedbacklistpassed));

                    await MopupService.Instance.PopAllAsync(false);

                    var pageToRemoves = Navigation.NavigationStack.ToList();

                    int i = 0;

                    foreach (var page in pageToRemoves)
                    {
                        if (i == 0)
                        {
                        }
                        else if (i == 1 || i == 2 || i == 3)
                        {
                            Navigation.RemovePage(page);
                        }
                        else
                        {
                            //Navigation.RemovePage(page);
                        }
                        i++;
                    }

                }
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }

            //bool result = await viewModel.ShowPopupAsync();
            //if (result)
            //{
            //    // Accepted
            //    APICalls database = new APICalls();
            //    await database.PostSymptomAsync(AddNewSymptom[0]);
            //    var allUserSymptoms = await database.GetUserSymptomAsync(Helpers.Settings.UserKey);

            //    foreach (var item in allUserSymptoms)
            //    {
            //        if (item.deleted == false)
            //        {
            //            UpdatedAllUserSymptoms.Add(item);
            //        }
            //    }
            //    foreach (var item in UpdatedAllUserSymptoms)
            //    {
            //        if (item.symptomtitle == NewSymptom.symptomtitle)
            //        {
            //            SymptomsPassed.Add(item);
            //        }

            //    }
            //    await Navigation.PushAsync(new AllSymptoms(SymptomsPassed));
            //    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllSymptoms);
            //    if (pageToRemoves != null)
            //    {
            //        Navigation.RemovePage(pageToRemoves);
            //    }
            //    Navigation.RemovePage(this);
            //}
            //else
            //{
            //    // Declined
            //    await Navigation.PopAsync();
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = searchbar.Text.ToString();
            var filteredSymptoms = new ObservableCollection<symptom>(FilterResults.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
            SymptomsListview.ItemsSource = filteredSymptoms;
            var count = filteredSymptoms.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (Filterstack.IsVisible == true)
            {
                Filterstack.IsVisible = false;
                if(DeviceInfo.Platform == DevicePlatform.Android)
                {
                    ListViewStack.Margin = new Thickness(0, 0, 0, 0);
                }
                else if(DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    ListViewStack.Margin = new Thickness(0, -50, 0, 0);
                }
                
            }
            else
            {
                Filterstack.IsVisible = true;
                ListViewStack.Margin = new Thickness(0, 0, 0, 0);
            }
        }
        catch(Exception Ex)
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
            var filteredSymptoms = new ObservableCollection<symptom>(FilterResults.Where(s => s.classification.Contains(item, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
            var count = filteredSymptoms.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
            SymptomsListview.ItemsSource = filteredSymptoms;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }    
    }
    //private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    //{
    //}
}