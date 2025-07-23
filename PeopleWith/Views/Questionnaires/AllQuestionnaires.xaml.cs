using Mopups.Services;
using Syncfusion.Pdf.Lists;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Storage;

namespace PeopleWith;

public partial class AllQuestionnaires : ContentPage
{
	public questionnaire userquestionnaireinfo = new questionnaire();
	public ObservableCollection<questionnaire> questionnaires = new ObservableCollection<questionnaire>();
    public ObservableCollection<userquestionnaire> userQuestionnaires = new ObservableCollection<userquestionnaire>();
    APICalls aPICalls = new APICalls();
    userfeedback userfeedbacklistpassed = new userfeedback();

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

    public AllQuestionnaires(userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            userfeedbacklistpassed = userfeedbacklist; 
            getquestionnaires();
            //getuserquestionnaires();
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

    async void getquestionnaires()
	{
		try
		{
            QuesLoading.IsVisible = true;
            var getQuestionairesTask = await aPICalls.GetQuestionnaires();
            questionnaires = getQuestionairesTask;

            userQuestionnaires.Clear();
            var userquestionnaires = await aPICalls.GetUserQuestionnaires();

            if (questionnaires != null)
			{
				Allquestionnaires.ItemsSource = questionnaires;
            }
            else
            {
                //Show Empty Prompt If NO Completed Questionnaires 
            }        

            foreach (var item in userquestionnaires)
            {
                var questionnaire = questionnaires.Where(x => x.questionnaireid == item.questionnaireid).FirstOrDefault();

                if (questionnaire != null)
                {
                    item.questionnairename = questionnaire.title;

                }
            }

            var orderlist = userquestionnaires.OrderByDescending(x => DateTime.Parse(x.completedatetime)).ToList();

            foreach (var item in orderlist)
            {
                userQuestionnaires.Add(item);
            }
            Alluserquestionnaires.ItemsSource = userQuestionnaires;

            QuesLoading.IsVisible = false;
            NovoConsentData();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

	//async void getuserquestionnaires()
	//{
	//	try
	//	{
 //           userQuestionnaires.Clear();
 //           var userquestionnaires = await aPICalls.GetUserQuestionnaires();

	//		foreach(var item in userquestionnaires)
	//		{
	//			var questionnaire = questionnaires.Where(x => x.questionnaireid == item.questionnaireid).FirstOrDefault();

	//			if(questionnaire != null)
	//			{
	//				item.questionnairename = questionnaire.title;

 //               }              
 //           }

	//		var orderlist = userquestionnaires.OrderByDescending(x => DateTime.Parse(x.completedatetime)).ToList();

 //           foreach( var item in orderlist)
 //           {
 //               userQuestionnaires.Add(item); 
 //           }
 //           Alluserquestionnaires.ItemsSource = userQuestionnaires;

 //           QuesLoading.IsVisible = false;

 //       }
 //       catch (Exception Ex)
 //       {
 //           NotasyncMethod(Ex);
 //       }
 //   }
    private async void Allquestionnaires_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var item = e.DataItem as questionnaire;

                if(DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new AndroidONLYQuestionnaires(item.questionnaireid, userfeedbacklistpassed), false);
                }
                else
                {

                    // await Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    //await Navigation.PushAsync(new AndroidQuestionnaires(item), false);

                   //  await Navigation.PushAsync(new QuestionnairePage(item), false);
                    await Navigation.PushAsync(new AndroidQuestionnaires(item.questionnaireid, userfeedbacklistpassed), false);
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

    private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
		try
		{
            var index = e.NewIndex;

            if (index == 0)
            {
				Allquestionnaires.IsVisible = true;
				Alluserquestionnaires.IsVisible = false;
                noCompleteQues.IsVisible = false;
            }
			else if(index == 1)
			{
                if (userQuestionnaires.Count > 0)
                {
                    Allquestionnaires.IsVisible = false;
                    Alluserquestionnaires.IsVisible = true;
                    noCompleteQues.IsVisible = false;
                }
                else
                {
                    Allquestionnaires.IsVisible = false;
                    Alluserquestionnaires.IsVisible = false;
                    noCompleteQues.IsVisible = true;
                }
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

                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires(item, userquestionnaireinfo), false);
                }
                else
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires(item, userquestionnaireinfo), false);
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

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("question") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}