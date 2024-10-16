//using System;
using System;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class MainDashboard : ContentPage
{
    ObservableCollection<user> UserDetails = new ObservableCollection<user>();
    user AllUserDetails = new user();
    APICalls database = new APICalls();
    public MainDashboard()
	{
		InitializeComponent();

        //Get All user Details & Set Helpers.Settings
        GetUserDetails();

		NavigationPage.SetHasNavigationBar(this, false);

        string firstName = Preferences.Default.Get("userid", "Unknown");
		lbl.Text = firstName;
    }

    async private void GetUserDetails()
    {
        try
        {
            UserDetails = await database.GetuserDetails();
         
            AllUserDetails.firstname = UserDetails[0].firstname;
            Helpers.Settings.FirstName = UserDetails[0].firstname; 

            AllUserDetails.surname = UserDetails[0].surname;
            Helpers.Settings.Surname = UserDetails[0].surname; 

            AllUserDetails.email = UserDetails[0].email;
            Helpers.Settings.Email = UserDetails[0].email;

            AllUserDetails.dateofbirth = UserDetails[0].dateofbirth;
            Helpers.Settings.Age = UserDetails[0].dateofbirth;

            AllUserDetails.gender = UserDetails[0].gender;
            Helpers.Settings.Gender = UserDetails[0].gender;

            AllUserDetails.ethnicity = UserDetails[0].ethnicity;
            Helpers.Settings.Ethnicity = UserDetails[0].ethnicity;

            AllUserDetails.signupcodeid = UserDetails[0].signupcodeid;
            Helpers.Settings.SignUp = UserDetails[0].signupcodeid;

            AllUserDetails.referral = UserDetails[0].referral;
            //No Referral Helper.settings. 

            AllUserDetails.biometrics = UserDetails[0].biometrics;
            Helpers.Settings.biometrics = UserDetails[0].biometrics;

            //if (UserDetails[0].userpin.Contains(","))
            //{
            //    var split = UserDetails[0].userpin.Split(','); 
            //    AllUserDetails.userpin = split[1];
            //    Helpers.Settings.PinCode = split[1];
            //}
            //else
            //{
                AllUserDetails.userpin = UserDetails[0].userpin;
                Helpers.Settings.PinCode = UserDetails[0].userpin;
            //}


            AllUserDetails.password = UserDetails[0].password; 
            Helpers.Settings.Password = UserDetails[0].password;
            Helpers.Settings.UserPasswordHash = UserDetails[0].password;

        }
        catch (Exception ex)
        {

        }
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

    async private void Button_Clicked_5(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ProfileSection(AllUserDetails), false);
        }
        catch (Exception Ex)
        {

        }

    }

    async private void AppointBtn_Clicked(object sender, EventArgs e)
    {

        try
        {
            await Navigation.PushAsync(new Appointments(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void HCPBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new HCPs(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void VideoBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AllVideos(), false);
        }
        catch (Exception Ex)
        {

        }
    }
}