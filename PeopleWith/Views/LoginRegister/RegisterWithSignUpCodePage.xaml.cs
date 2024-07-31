using MauiApp1;

namespace PeopleWith;

public partial class RegisterWithSignUpCodePage : ContentPage
{
    signupcode signupcodepassed;
    user userpassed;
	public RegisterWithSignUpCodePage()
	{
		InitializeComponent();
	}

    public RegisterWithSignUpCodePage(signupcode usersignupcode, user userpass)
    {
        InitializeComponent();


        signupcodepassed = usersignupcode;
        userpassed = userpass;


        signupinfotitle.Text = "Welcome to " + signupcodepassed.title;


    }
}