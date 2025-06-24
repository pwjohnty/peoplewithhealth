using Mopups.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Storage;

namespace PeopleWith;

public partial class AllMood : ContentPage
{
    //Change to Mood when added 
    public ObservableCollection<usermood> AllMoods = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> itemstoRemove = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> SingleMood = new ObservableCollection<usermood>();
    bool initialload;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();

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

    public AllMood()
    {
        try
        {
            InitializeComponent();
            initialload = true;
            GetAllUserMoods();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AllMood(userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            initialload = true;
            userfeedbacklistpassed = userfeedbacklist;
            GetAllUserMoods();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AllMood(ObservableCollection<usermood> AllUserMoods)
    {
        try
        {
            InitializeComponent();
            AllMoods = AllUserMoods;
            initialload = false;
            GetAllUserMoods(); 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    public AllMood(ObservableCollection<usermood> AllUserMoods, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            AllMoods = AllUserMoods;
            initialload = false;
            userfeedbacklistpassed = userfeedbacklist;
            GetAllUserMoods();
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void GetAllUserMoods()
    {
        try
        {
            if(initialload == true)
            {
                MoodLoading.IsVisible = true; 
                var Userid = Helpers.Settings.UserKey;
                APICalls database = new APICalls();

                var getMoodsTask = database.GetUserMoodsAsync(Userid);

                //var delayTask = Task.Delay(1000);

                //if (await Task.WhenAny(getMoodsTask, delayTask) == delayTask)
                //{
                    //await MopupService.Instance.PushAsync(new GettingReady("Just Getting Mood Ready for you") { });
                //}

                AllMoods = await getMoodsTask;

                

            }

            foreach (var item in AllMoods)
            {
                var date = item.datetime;
                DateTime GetDate = DateTime.Parse(date);
                item.datetime = GetDate.ToString("dd/MM/yyyy HH:mm");
                if (item.title == "S**t")
                {
                    item.source = "shit.png";
                }

                if (item.deleted == true)
                {
                    itemstoRemove.Add(item);
                }
            }

            foreach (var item in itemstoRemove)
            {
                AllMoods.Remove(item);
            }

            if (AllMoods.Count > 0)
            {

                EmptyStack.IsVisible = false;
                MoodOverview.IsVisible = true;
                AllMoodView.ItemsSource = AllMoods
    .GroupBy(m => m.title  ) 
    .Select(g => g.OrderByDescending(f => DateTime.Parse(f.datetime)).First()) 
    .OrderByDescending(f => DateTime.Parse(f.datetime)) 
    .ToList();
                //AllMoodView.ItemsSource = AllMoods.OrderByDescending(f => DateTime.Parse(f.datetime)).ToList();
                AllMoodView.HeightRequest = AllMoods.Count * 80;
               // NovoConsent.Margin = new Thickness(20, 300, 20, 10);
            }
            else
            {
                EmptyStack.IsVisible = true;
                MoodOverview.IsVisible = false;
                //NovoConsent.Margin = new Thickness(20, 0, 20, 10);
            }

            MoodLoading.IsVisible = false;

            NovoConsentData();
            //await MopupService.Instance.PopAllAsync(false);

            //Test Crash 
           // int zero = 0;
          //  int crash = 5 / zero;


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
                await Navigation.PushAsync(new AddMood(AllMoods, "Add" , userfeedbacklistpassed));
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

    async private void AllMoodView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            SingleMood.Clear();
            var Mood = e.DataItem as usermood;
            var Title = Mood.title;

            var allsamemoods = new ObservableCollection<usermood>();

            foreach (var item in AllMoods)
            {
                if (Mood.id == item.id)
                {
                    SingleMood.Add(item);
                }


                if(item.title == Title)
                {
                    allsamemoods.Add(item);
                }
            }
            await Navigation.PushAsync(new SingleMood(SingleMood, AllMoods, allsamemoods, userfeedbacklistpassed));
        }

        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void MoodInfoTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("mood") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}