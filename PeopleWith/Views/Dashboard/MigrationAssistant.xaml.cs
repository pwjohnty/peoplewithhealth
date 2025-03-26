namespace PeopleWith;

using Syncfusion.Maui.ProgressBar;
using System.Collections.ObjectModel;

public partial class MigrationAssistant : ContentPage
{
    ObservableCollection<user> updateuser = new ObservableCollection<user>();
    APICalls database = new APICalls();

    public MigrationAssistant()
	{
		InitializeComponent();

        var Steps = new ObservableCollection<StepProgressBarItem>();

        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });
        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });
        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });
        Steps.Add(new StepProgressBarItem() { PrimaryText = "" });

        stepbar.ItemsSource = Steps;

        getuserdetails();

    }

    async void getuserdetails()
    {
        try
        {
            updateuser = await database.GetuserDetails();
        }
        catch(Exception ex)
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
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void confirmcodepin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {
            confirmcodepin.IsEnabled = false;
            confirmcodepin.IsEnabled = true;
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void codepin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            if (e.PropertyName == nameof(codepin))
            {
                incorrectcodelbl.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
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
            //Leave Empty
        }
    }

    private async void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //next button clicked

            if (emailframe.IsVisible == true)
            {
                stepbar.ActiveStepIndex = 1;
                emailframe.IsVisible = false;
                notificationstack.IsVisible = true;
                skipbtn.IsVisible = true;

                PermissionStatus status;
                nextbtn.Text = "Allow";

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // Request and capture the permission status on Android
                    status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                    if (status == PermissionStatus.Granted)
                    {
                        // Set notifications as enabled
                        nextbtn.Text = "Next";

                    }
                    else
                    {
                        // Set notifications as disabled
                        updateuser[0].pushnotifications = "Disabled";

                    }
                }
                else
                {
                    // Request permission on iOS via dependency service
                    var notificationService = DependencyService.Get<INotificationService>();
                    bool isGranted = await notificationService.CheckRequestNotificationPermissionAsync();

                    // Set notifications based on whether permission was granted
                    if (isGranted)
                    {
                        // Set notifications as enabled
                        nextbtn.Text = "Next";
                    }
                    else
                    {
                        // Set notifications as disabled
                        updateuser[0].pushnotifications = "Disabled";
                    }



                }


                

            }
            else if (faceidstack.IsVisible == true)
            {

                if (string.IsNullOrEmpty(codepin.PINValue) || string.IsNullOrEmpty(confirmcodepin.PINValue))
                {
                    incorrectcodelbl.Text = "Please enter a PIN";
                    incorrectcodelbl.IsVisible = true;
                    Vibration.Vibrate();
                    return;
                }

                if (codepin.PINValue == confirmcodepin.PINValue)
                {
                    //ask if they want to use faceid


                    faceidstack.IsVisible = false;
                    //notificationstack.IsVisible = true;
                   
                    


                    stepbar.ActiveStepIndex = 3;

                    skipbtn.IsVisible = false;


                    Helpers.Settings.PinCode = codepin.PINValue.ToString(); 
                    updateuser[0].userpin = codepin.PINValue.ToString();

                    bottomgrid.IsVisible = false;
                    topgrid.IsVisible = false;
                    faceidstack.IsVisible = false;
                    migratestack.IsVisible = true;


                }
                else
                {
                    incorrectcodelbl.Text = "PINs does not match";
                    incorrectcodelbl.IsVisible = true;
                    Vibration.Vibrate();
                    return;
                }


                var timer = new System.Timers.Timer(1500); // Update every 100ms
                timer.Elapsed += (sender, e) =>
                {
                    // Increase progress by a small amount (1% of the total) each time
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (topprogress.Progress <= 100)
                        {
                            topprogress.Progress += 5;  // Increase by 1% each update
                        }
                        else
                        {
                            timer.Stop();
                            topprogress.Progress = 100;// Stop timer once progress is 100%
                        }
                    });
                };
                timer.Start();


                string url = "https://portal.peoplewith.com/migration/migration-assistant.php?uid=" + Helpers.Settings.UserKey;

                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Send a GET request to the URL
                        HttpResponseMessage response = await client.GetAsync(url);

                        // Check if the response is successful
                        if (response.IsSuccessStatusCode)
                        {
                            timer.Stop();
                            topprogress.Progress = 100;
                            // Read the response content as a string
                            string content = await response.Content.ReadAsStringAsync();

                            // Check if content is not empty
                            if (!string.IsNullOrEmpty(content))
                            {
                                topprogress.IsVisible = false;
                                mdlbl.Text = "Success";
                                md2lbl.Text = "Data successfully migrated";
                                migimg.Source = "successtick.png";
                                updateuserbtn.IsVisible = true;
                                Console.WriteLine("Response received:");
                                Console.WriteLine(content); // Or process the response as needed
                            }
                            else
                            {
                                Console.WriteLine("No content returned from the URL.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to retrieve content. Status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occurred: " + ex.Message);
                    }
                }



            }
            else if(notificationstack.IsVisible == true)
            {
                if (nextbtn.Text == "Next")
                {
                    //means notifications are turned on
                }
                else
                {


                    AppInfo.ShowSettingsUI();
                }

                stepbar.ActiveStepIndex = 2;

                nextbtn.Text = "Next";

                notificationstack.IsVisible = false;
                skipbtn.IsVisible = false;
               // bottomgrid.IsVisible = false;
               // topgrid.IsVisible = false;
                faceidstack.IsVisible = true;

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // Request and capture the permission status on Android
                   var sstatus = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                    if (sstatus == PermissionStatus.Granted)
                    {
                        // Set notifications as enabled
                        updateuser[0].pushnotifications = "True";

                    }
                    else
                    {
                        // Set notifications as disabled
                        updateuser[0].pushnotifications = "Disabled";
                    }
                }
                else
                {
                    // Request permission on iOS via dependency service
                    var notificationService = DependencyService.Get<INotificationService>();
                    bool isGranted = await notificationService.CheckRequestNotificationPermissionAsync();

                    // Set notifications based on whether permission was granted
                    if (isGranted)
                    {
                        // Set notifications as enabled
                        updateuser[0].pushnotifications = "True";
                    }
                    else
                    {
                        // Set notifications as disabled
                        updateuser[0].pushnotifications = "Disabled";
                    }



                }

                if(emailcheck.IsChecked)
                {
                    updateuser[0].emailnotifications = true;
                }
                else
                {
                    updateuser[0].emailnotifications = false;
                }

               


            }
            


        }
        catch(Exception ex)
        {

        }
    }

    private async void updateuserbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            updateuser[0].usermigrated = true;

            await database.UpdateUser(updateuser[0]);

            //update the helpers

            Preferences.Default.Set("pincode", updateuser[0].userpin);
            Preferences.Default.Set("notifications", updateuser[0].pushnotifications);
            Preferences.Default.Set("usermigrated", updateuser[0].usermigrated.ToString());


            Application.Current.MainPage = new NavigationPage(new MainDashboard());



        }
        catch(Exception ex)
        {

        }
    }
}