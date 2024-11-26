using Mopups.Services;
using System.Collections.ObjectModel;

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
            await crashHandler.CrashDetectedSend(Ex);
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

    async private void GetAllUserMoods()
    {
        try
        {
            if(initialload == true)
            {
                var Userid = Helpers.Settings.UserKey;
                APICalls database = new APICalls();

                var getMoodsTask = database.GetUserMoodsAsync(Userid);

                //var delayTask = Task.Delay(1000);

                //if (await Task.WhenAny(getMoodsTask, delayTask) == delayTask)
                //{
                    await MopupService.Instance.PushAsync(new GettingReady("Just Getting Mood Ready for you") { });
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
                    item.source = "Shit.png";
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
                AllMoodView.ItemsSource = AllMoods.OrderByDescending(f => DateTime.Parse(f.datetime)).ToList();
                AllMoodView.HeightRequest = AllMoods.Count * 80;
            }
            else
            {
                EmptyStack.IsVisible = true;
                MoodOverview.IsVisible = false;
            }

            await MopupService.Instance.PopAllAsync(false);
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
            foreach (var item in AllMoods)
            {
                if (Mood.id == item.id)
                {
                    SingleMood.Add(item);
                }
            }
            await Navigation.PushAsync(new SingleMood(SingleMood, AllMoods));
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