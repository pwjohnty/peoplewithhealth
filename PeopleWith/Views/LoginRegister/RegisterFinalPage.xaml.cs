using MauiApp1;

namespace PeopleWith;

public partial class RegisterFinalPage : ContentPage
{
    user newuser;
    double progressamount;
    
	public RegisterFinalPage()
	{
		InitializeComponent();
	}

    public RegisterFinalPage(user userpass, double progress)
    {
        InitializeComponent();

        newuser = userpass;

        topprogress.SetProgress(progress, 0);


        //find out the amount left - only 2 pages left after this amount

        progressamount = (100 - progress) / 2;

        faceidstack.IsVisible = true;


    }

    async Task HandleNotificationframe()
    {
        try
        {
            //handle notification


            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                await Permissions.RequestAsync<Permissions.PostNotifications>();
            }
            else
            {
                var notificationService = DependencyService.Get<INotificationService>();
                await notificationService.RequestNotificationPermissionAsync();

            }

            notificationstack.IsVisible = false;
            nextbtn.Text = "Finish";
            healthdatastack.IsVisible = true;

            UpdateProgress();



        }
        catch (Exception ex)
        {

        }
    }

    async void HandleHealthdataframe()
    {
        try
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {

            }
            else
            {
                DependencyService.Get<IHealthService>().GetHealthPermissionAsync(async (result) =>
                {
                    var a = result;
                    if (result)
                    {

                        //topgrid.IsVisible = false;
                        //bottomstack.IsVisible = false;
                        //healthdatastack.IsVisible = false;
                        //finishstack.IsVisible = true;
                        //await Task.Delay(3000);

                        //finishstack.IsVisible = false;
                    }
                    else
                    {
                        topgrid.IsVisible = false;
                        bottomstack.IsVisible = false;
                        healthdatastack.IsVisible = false;
                        finishstack.IsVisible = true;
                        await Task.Delay(3000);

                        finishstack.IsVisible = false;
                    }
                });
            }



        }
        catch(Exception ex)
        {

        }
    }

    private void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if(faceidstack.IsVisible == true)
            {
                Handlepinstack();
            }

            else if(notificationstack.IsVisible == true)
            {
                HandleNotificationframe();
            }
            else if(healthdatastack.IsVisible == true)
            {
                HandleHealthdataframe();
            }
        }
        catch(Exception ex)
        {

        }
    }

    async void Handlepinstack()
    {
        try
        {

            if (string.IsNullOrEmpty(codepin.PINValue) || string.IsNullOrEmpty(confirmcodepin.PINValue))
            {
                incorrectcodelbl.Text = "Please enter a PIN";
                incorrectcodelbl.IsVisible = true;
                Vibration.Vibrate();
                return;
            }

            if(codepin.PINValue == confirmcodepin.PINValue)
            {
                newuser.userpin = codepin.PINValue.ToString();
                faceidstack.IsVisible = false;
                notificationstack.IsVisible = true;
                UpdateProgress();
                nextbtn.Text = "Allow";
                backbtn.IsVisible = false;

            }
            else
            {
                incorrectcodelbl.Text = "PINs does not match";
                incorrectcodelbl.IsVisible = true;
                Vibration.Vibrate();
                return;
            }

        }
        catch(Exception ex)
        {

        }
    }

    async void UpdateProgress()
    {
        try
        {

            topprogress.Progress = topprogress.Progress += progressamount;


        }
        catch (Exception ex)
        {

        }

    }

    private void codepin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {
            codepin.IsEnabled = false;
            codepin.IsEnabled = true;
        }
        catch(Exception ex )
        {

        }
    }

    private void confirmcodepin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {
            confirmcodepin.IsEnabled = false;
            confirmcodepin.IsEnabled = true;
        }
        catch(Exception ex)
        {

        }
    }

    private void codepin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            incorrectcodelbl.IsVisible = false;
        }
        catch(Exception ex)
        {

        }
    }

    private void confirmcodepin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            incorrectcodelbl.IsVisible = false;
        }
        catch (Exception ex)
        {

        }
    }
}