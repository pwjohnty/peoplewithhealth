using PeopleWith;
using System;

namespace PeopleWith;

public partial class RegisterWithSignUpCodePage : ContentPage
{
    signupcode signupcodepassed;
    user userpassed;
	public RegisterWithSignUpCodePage()
	{
		InitializeComponent();
	}

    public RegisterWithSignUpCodePage(signupcode usersignupcode, user userpass, double progresspassed)
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
}