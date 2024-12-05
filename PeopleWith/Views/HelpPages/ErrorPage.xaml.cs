namespace PeopleWith;

public partial class ErrorPage : ContentPage
{
    //String used to Determin is the Current Page is before or After Login/Reg
    string BeforeAfter = string.Empty; 
	public ErrorPage(String AfterBefore)
	{
		InitializeComponent();
        BeforeAfter = AfterBefore; 
        if(BeforeAfter == "Dashboard")
        {
            DashReturnbtn.IsVisible = true; 
        }
        else if (BeforeAfter == "Login")
        {
            LoginReturnbtn.IsVisible = true; 
        }
    }

    async private void DashReturnbtn_Clicked(object sender, EventArgs e)
    {
        try 
        {
            var pageToRemoves = Navigation.NavigationStack.ToList();
            if (pageToRemoves != null)
            {
                foreach (var item in pageToRemoves)
                {
                    if (item != Navigation.NavigationStack.FirstOrDefault())
                    {
                        Navigation.RemovePage(item);
                    }
                }
            }

            // Navigate to the MainDashboard page
            await Navigation.PushAsync(new MainDashboard(), false);
        }
        catch(Exception Ex)
        {

        }
    }

    async private void LoginReturnbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var pageToRemoves = Navigation.NavigationStack.ToList();
            if (pageToRemoves != null)
            {
                foreach (var item in pageToRemoves)
                {
                    if (item != Navigation.NavigationStack.FirstOrDefault())
                    {
                        Navigation.RemovePage(item);
                    }
                }
            }
            // Navigate to the Login page
            await Navigation.PushAsync(new LoginPage(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Dont Forget Back Arrow
            await Navigation.PushAsync(new PrivacyPolicy(), false);
        }
        catch (Exception Ex)
        {

        }
    }
}