using Mopups.Services;

namespace PeopleWith;

public partial class SingleInvestigations : ContentPage
{
    userinvestigation InvestPassed = new userinvestigation();

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
    public SingleInvestigations()
	{
		InitializeComponent();
	}

    public SingleInvestigations(userinvestigation userinvestpassed)
    {
        InitializeComponent();
        InvestPassed = userinvestpassed;

        Investtitle.Text = InvestPassed.investigationname;
        lblStart.Text = InvestPassed.datewotime;

        if (InvestPassed.notes.Count > 0)
        {
            AllInvestNotes.ItemsSource = InvestPassed.notes.OrderByDescending(x => DateTime.Parse(x.datetime));
            InvestNotesInsights.IsVisible = true;
        }
        else
        {
            InvestNotesInsights.IsVisible = false;
        }
    }

    private async void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool isEdit = true;
            await Navigation.PushAsync(new AddInvestigations(InvestPassed, isEdit), false);
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
            await Navigation.PushAsync(new AddInvestigations(InvestPassed), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void AllInvestNotes_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Selecteditem = e.DataItem as notesuserfeedback;
            await Navigation.PushAsync(new AddInvestigations(InvestPassed, Selecteditem), false);
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
            await Navigation.PushAsync(new InvestInfo(InvestPassed), false);
        }
        catch (Exception Ex)
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


                bool Delete = await DisplayAlert("Delete Investigation", "Are you sure you would like the delete this INvestigation? Once deleted it cannot be retrieved", "Continue", "Cancel");
                if (Delete == true)
                {
                    //Delete Item 
                    InvestPassed.deleted = true;
                    APICalls database = new APICalls();
                    await database.DeleteUserInvestigation(InvestPassed);


                    await MopupService.Instance.PushAsync(new PopupPageHelper("Investigation Deleted") { });
                    await Task.Delay(3000);

                    await MopupService.Instance.PopAllAsync(false);

                    await Navigation.PushAsync(new AllInvestigations());
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllInvestigations);
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
}