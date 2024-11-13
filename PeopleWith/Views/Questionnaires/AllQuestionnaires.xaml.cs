using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllQuestionnaires : ContentPage
{
	public questionnaire userquestionnaireinfo = new questionnaire();
	public ObservableCollection<questionnaire> questionnaires = new ObservableCollection<questionnaire>();
    APICalls aPICalls = new APICalls();

    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

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

    public AllQuestionnaires()
	{
        try
        {
            InitializeComponent();
            getquestionnaires();
            getuserquestionnaires();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
	}

	async void getquestionnaires()
	{
		try
		{
			questionnaires = await aPICalls.GetQuestionnaires();

			if (questionnaires != null)
			{
				foreach (var item in questionnaires)
				{

				}

				Allquestionnaires.ItemsSource = questionnaires;
            }
		}
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

	async void getuserquestionnaires()
	{
		try
		{

			var userquestionnaires = await aPICalls.GetUserQuestionnaires();

			foreach(var item in userquestionnaires)
			{
				var questionnaire = questionnaires.Where(x => x.questionnaireid == item.questionnaireid).FirstOrDefault();

				if(questionnaire != null)
				{
					item.questionnairename = questionnaire.title;

                }              
            }

			var orderlist = userquestionnaires.OrderByDescending(x => DateTime.Parse(x.completedatetime)).ToList();

            Alluserquestionnaires.ItemsSource = orderlist;

		}
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void Allquestionnaires_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var item = e.DataItem as questionnaire;
                await Navigation.PushAsync(new QuestionnairePage(item), false);
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

    private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
		try
		{
            var index = e.NewIndex;

            if (index == 0)
            {
				Allquestionnaires.IsVisible = true;
				Alluserquestionnaires.IsVisible = false;
            }
			else if(index == 1)
			{
				Allquestionnaires.IsVisible = false;
				Alluserquestionnaires.IsVisible = true;
			}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Alluserquestionnaires_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var item = e.DataItem as userquestionnaire;
                userquestionnaireinfo = questionnaires.Where(x => x.questionnaireid == item.questionnaireid).FirstOrDefault();
                await Navigation.PushAsync(new QuestionnairePage(item, userquestionnaireinfo), false);
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