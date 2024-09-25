using System;

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

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
		try
		{
			await Navigation.PushAsync(new AllAllergies(), false); 
		}
		catch (Exception Ex)
		{
			
		}
    }


    private async void Button_Clicked_2Medications(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AllMedications(), false);
        }
        catch (Exception ex)
        {

        }
    }

    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AllDiagnosis(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void Button_Clicked_4(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AllMood(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void Button_Clicked_4Schedule(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new MainSchedule(), false);
        }
        catch (Exception ex)
        {

        }
    }

}