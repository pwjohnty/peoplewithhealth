using PeopleWith;
using System;

namespace PeopleWith;

public partial class RegisterWithSignUpCodePage : ContentPage
{
    signupcode signupcodepassed;
    user userpassed;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Login"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public RegisterWithSignUpCodePage()
	{
        try
        {
            InitializeComponent();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public RegisterWithSignUpCodePage(signupcode usersignupcode, user userpass, double progresspassed)
    {
        try
        {
            InitializeComponent();

            signupcodepassed = usersignupcode;
            userpassed = userpass;

            progresspassed = progresspassed + 10;

            topprogress.SetProgress(progresspassed, 0);


            if (signupcodepassed.referral == "NOVO")
            {
                heightandweightstack.IsVisible = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }      
    }
}