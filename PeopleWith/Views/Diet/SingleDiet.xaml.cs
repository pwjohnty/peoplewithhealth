using Mopups.Services;

namespace PeopleWith;

public partial class SingleDiet : ContentPage
{
   userdiet userdietPassed = new userdiet();

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
    public SingleDiet()
	{
		InitializeComponent();
    }

    public SingleDiet(userdiet Dietpassed)
    {
        InitializeComponent();
        userdietPassed = Dietpassed;

        Dietitle.Text = userdietPassed.diettitle;
        lblStart.Text = userdietPassed.datestarted;

        if (!String.IsNullOrEmpty(userdietPassed.dateended))
        {
            lblEnd.Text = userdietPassed.dateended; 
        }
        else
        {
            lblEnd.Text = "Ongoing";
        }

        if(userdietPassed.notes.Count > 0)
        {
            AllDietNotes.ItemsSource = userdietPassed.notes.OrderByDescending(x => DateTime.Parse(x.datetime));
            DietNotesInsights.IsVisible = true; 
        }
        else
        {
            DietNotesInsights.IsVisible = false;
        }
    }

    private async void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool isEdit = true; 
            await Navigation.PushAsync(new AddDiet(userdietPassed, isEdit), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new DietInfo(userdietPassed), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void AddDataBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddDiet(userdietPassed), false);
        }
        catch( Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Deletebtn_Clicked(object sender, EventArgs e)
    {
       try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                Deletebtn.IsEnabled = false;


                bool Delete = await DisplayAlert("Delete Diet", "Are you sure you would like the delete this Diet? Once deleted it cannot be retrieved", "Continue", "Cancel");
                if (Delete == true)
                {
                    //Delete Item 
                    userdietPassed.deleted = true;
                    APICalls database = new APICalls();
                    await database.DeleteUserDiet(userdietPassed);


                    await MopupService.Instance.PushAsync(new PopupPageHelper("Diet Deleted") { });
                    await Task.Delay(3000);

                    await MopupService.Instance.PopAllAsync(false);

                    await Navigation.PushAsync(new AllDiet());
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllDiet);
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                    Deletebtn.IsEnabled = true;
                }
                else
                {
                    Deletebtn.IsEnabled = true;
                    return;
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

    private async void AllDietNotes_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Selecteditem = e.DataItem as userNotesFeedback;
            await Navigation.PushAsync(new AddDiet(userdietPassed, Selecteditem), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }
}