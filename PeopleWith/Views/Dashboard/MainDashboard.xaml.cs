namespace PeopleWith;

public partial class MainDashboard : ContentPage
{
	public MainDashboard()
	{
		InitializeComponent();

		NavigationPage.SetHasNavigationBar(this, false);

        string firstName = Preferences.Default.Get("userid", "Unknown");
		lbl.Text = firstName;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		try
		{
			await Navigation.PushAsync(new MeasurementsPage(), false);

		}
		catch(Exception ex)
		{

		}
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
		try
		{
            await Navigation.PushAsync(new AllSymptoms(), false);
        }
		catch(Exception ex)
		{

		}
    }
}