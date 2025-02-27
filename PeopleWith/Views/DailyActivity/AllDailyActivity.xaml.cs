using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllDailyActivity : ContentPage
{
    public ObservableCollection<userdailyactivity> AllUserActivity = new ObservableCollection<userdailyactivity>(); 
    List<DateTime> dateList = new List<DateTime>();
    public DateTime dateforschedule = new DateTime();
    public List<Schedule> changeddatesforlistview = new List<Schedule>();
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public AllDailyActivity()
	{
        try
        {

            InitializeComponent();

            GetActivityInfo();

            //get 7 days before and 7 days after todays date

            // Get today's date
            DateTime today = DateTime.Today;

            // Initialize a list to hold the dates
            // Add 7 days before today's date
            for (int i = -30; i <= 3; i++)
            {
                dateList.Add(today.AddDays(i));
            }

            foreach (var item in dateList)
            {
                var newitem = new Schedule();

                newitem.Day = item.Day.ToString();
                newitem.Date = item.Date.ToString("ddd");

                if (item.Date > DateTime.Now.Date)
                {
                    newitem.Bgcolour = Colors.Transparent;
                    newitem.Bordercolour = Color.FromArgb("#fce9d9");
                    newitem.Op = 0.5;
                }
                else
                {
                    newitem.Bgcolour = Color.FromArgb("#fce9d9");
                    newitem.Bordercolour = Colors.Transparent;
                    newitem.Op = 1;
                }

                changeddatesforlistview.Add(newitem);
            }

            ActivityDates.ItemsSource = changeddatesforlistview;

            // Find today's date in the list

            var indexForToday = dateList.IndexOf(today);

            // Check if today's date is in the list
            if (indexForToday >= 0)
            {
                // Set the selected item to today's date
                ActivityDates.SelectedItem = changeddatesforlistview[indexForToday];

                var dateforlabel = dateList[indexForToday];

                datelbl.Text = dateforlabel.ToString("dddd, dd MMMM");
                dateforschedule = dateList[indexForToday];

                // Scroll to today's date and try to center it
                ActivityDates.ScrollTo(changeddatesforlistview[indexForToday], ScrollToPosition.Center, true);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
	
	}


    private async void GetActivityInfo()
    {
        try
        {
            //Get User Activity Here
            APICalls database = new APICalls();

            var GetActivityTask = database.GetUserActivityAsync();
            AllUserActivity = await GetActivityTask;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void AddBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddDailyActivity(), false); 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void ActivityDates_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }

    private void AllDietView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        //Here open Overlay
    }

    //private async void Testbtn_Clicked(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        APICalls database = new APICalls();

    //        var getActivityTask = database.GetAuthTest();
    //        var Item = await getActivityTask;
            
    //    }
    //    catch (Exception Ex)
    //    {

    //    }
    //}
}