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

    public AllMood()
    {
        InitializeComponent();
        initialload = true;
        GetAllUserMoods(); 

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
            //Add Crash log 
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

                var delayTask = Task.Delay(1000);

                if (await Task.WhenAny(getMoodsTask, delayTask) == delayTask)
                {
                    await MopupService.Instance.PushAsync(new GettingReady("Just Getting Mood Ready for you") { });
                }

                AllMoods = await getMoodsTask;

                await MopupService.Instance.PopAllAsync(false);

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
                AllMoodView.ItemsSource = AllMoods;
            }
            else
            {
                EmptyStack.IsVisible = true;
                MoodOverview.IsVisible = false;
            }


        }
        catch (Exception ex)
        {
            //Add Crash logs 
        }

    }

    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddMood(AllMoods, "Add"));
        }
        catch (Exception Ex)
        {
            //Add Crash log 
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

        catch (Exception ex)
        {
            //Add Crash log
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
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }
}