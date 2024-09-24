using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllMedications : ContentPage
{
	APICalls aPICalls = new APICalls();
    public ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    public AllMedications()
	{
		InitializeComponent();


		getusermedications();

	}


	async void getusermedications()
	{
		try
		{
            AllUserMedications = await aPICalls.GetUserMedicationsAsync();


			AllUserMedsList.ItemsSource = AllUserMedications;
        }
		catch(Exception ex)
		{

		}
	}

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			await Navigation.PushAsync(new AddMedication(), false);
		}
		catch(Exception ex)
		{

		}
    }
}