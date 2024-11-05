

using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllQuestionnaires : ContentPage
{
	public questionnaire userquestionnaireinfo = new questionnaire();
	public ObservableCollection<questionnaire> questionnaires = new ObservableCollection<questionnaire>();
    APICalls aPICalls = new APICalls();
    public AllQuestionnaires()
	{
		InitializeComponent();


		getquestionnaires();

		getuserquestionnaires();
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
		catch(Exception ex)
		{

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
		catch(Exception ex)
		{

		}
	}
    private async void Allquestionnaires_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{
			var item = e.DataItem as questionnaire;

			await Navigation.PushAsync(new QuestionnairePage(item), false);
			
		}
		catch(Exception ex)
		{

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
		catch(Exception ex)
		{

		}
    }

    private async void Alluserquestionnaires_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
		try
		{

			var item = e.DataItem as userquestionnaire;

			userquestionnaireinfo = questionnaires.Where(x => x.questionnaireid == item.questionnaireid).FirstOrDefault();


            await Navigation.PushAsync(new QuestionnairePage(item, userquestionnaireinfo), false);


		}
		catch(Exception ex)
		{

		}
    }
}