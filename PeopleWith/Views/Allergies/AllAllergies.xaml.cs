using Mopups.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace PeopleWith;

public partial class AllAllergies : ContentPage
{
    public ObservableCollection<userallergies> AllUserAllergies = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> itemstoremove = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> PassedAllergy = new ObservableCollection<userallergies>();
    public ObservableCollection<allergies> AllergiesList = new ObservableCollection<allergies>();
    bool InitalLoad;
    public Stopwatch stopWatch = new Stopwatch();
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

    public AllAllergies()
    {
        try
        {
            InitializeComponent();
            InitalLoad = true;
            GetAllUserAllergies();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    public AllAllergies(ObservableCollection<userallergies> AllAllergiesPassed, ObservableCollection<allergies> allergies)
    {
        try
        {
            InitializeComponent();
            AllUserAllergies = AllAllergiesPassed;
            AllergiesList = allergies;
            InitalLoad = false;
            GetAllUserAllergies();      
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }  
    }

    async private void GetAllUserAllergies()
    {
        try
        {
            if (InitalLoad == true)
            {
                AllergyLoading.IsVisible = true; 
                var Userid = Helpers.Settings.UserKey;
                APICalls database = new APICalls();
              
                var getAllUserAllergiesTask = database.GetUserAllergiesAsync(Userid);
                var getAllergiesListTask = database.GetAsyncAllergies();

                //var delayTask = Task.Delay(1000);

                
                //if (await Task.WhenAny(Task.WhenAll(getAllUserAllergiesTask, getAllergiesListTask), delayTask) == delayTask)
                //{
                   
                    //await MopupService.Instance.PushAsync(new GettingReady("Loading Allergies") { });
                //}
                
                AllUserAllergies = await getAllUserAllergiesTask;
                AllergiesList = await getAllergiesListTask;

                


                //foreach (var item in AllUserAllergies)
                //{
                //    for (int i = 0; i < AllergiesList.Count; i++)
                //    {
                //        if (AllergiesList[i].Allergyid == item.allergyid)
                //        {
                //            item.title = AllergiesList[i].Title;
                //        }
                //    }
                //}
                //stopWatch.Stop();
                
            }
            foreach (var item in AllUserAllergies)
            {
                if (item.deleted == true)
                {
                    itemstoremove.Add(item);
                }
            }

            foreach (var item in itemstoremove)
            {
                AllUserAllergies.Remove(item);
            }


            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                AllAllergiesView.HeightRequest = AllUserAllergies.Count * 80;
            }

            var sortedAllergies = AllUserAllergies.OrderByDescending(f => DateTime.Parse(f.createdAt)).ToList();

            AllUserAllergies.Clear();
            foreach (var Allergy in sortedAllergies)
            {
                AllUserAllergies.Add(Allergy);
            }

            if (AllUserAllergies.Count > 0)
            {
                AllAllergiesView.ItemsSource = AllUserAllergies;
                EmptyStack.IsVisible = false;
                DiagnosisOverview.IsVisible = true;
                //NovoConsent.Margin = new Thickness(20, 0, 20, 10);
            }
            else
            {
                EmptyStack.IsVisible = true;
                DiagnosisOverview.IsVisible = false;
                //NovoConsent.Margin = new Thickness(20, 300, 20, 10);
            }
            AllergyLoading.IsVisible = false;
            //await MopupService.Instance.PopAllAsync(false);

            NovoConsentData();
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
                Addbtn.IsEnabled = false;
                await Navigation.PushAsync(new AddAllergies(AllergiesList, AllUserAllergies));
                Addbtn.IsEnabled = true;
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

    async private void AllAllergiesView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            PassedAllergy.Clear();
            var allergy = e.DataItem as userallergies;
            var Title = allergy.title;
            foreach (var item in AllUserAllergies)
            {
                if (Title == item.title)
                {
                    PassedAllergy.Add(item);

                }
            }

            await Navigation.PushAsync(new SingleAllergies(PassedAllergy, AllUserAllergies, AllergiesList));
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async private void AllInfoTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("allergy") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}
