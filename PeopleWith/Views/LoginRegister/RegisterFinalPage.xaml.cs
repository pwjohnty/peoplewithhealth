using PeopleWith;
using PeopleWith.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace PeopleWith;

public partial class RegisterFinalPage : ContentPage
{
    user newuser;
    double progressamount;
    public ObservableCollection<userresponse> userresponsepassed;
    public ObservableCollection<usermeasurement> usermeasurementpassed;
    public consent additonalconsent = new consent();
    ObservableCollection<usermedication> medicationstoadd = new ObservableCollection<usermedication>();
    ObservableCollection<usersymptom> symptomstoadd = new ObservableCollection<usersymptom>();
    HttpClient client = new HttpClient();
    userdiagnosis userdiag;

    public RegisterFinalPage()
	{
		InitializeComponent();

        //remove this
        topgrid.IsVisible = false;
        bottomstack.IsVisible = false;
        notificationstack.IsVisible = false;
       //tcstack.IsVisible = true;
        nextbtn.Text = "Agree and Finish";
        nextbtn.BackgroundColor = Colors.LightGray;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await Task.Delay(100);
        imganimation.IsAnimationPlaying = false;
        await Task.Delay(100);
        imganimation.IsAnimationPlaying = true;
    }

    public RegisterFinalPage(user userpass, double progress)
    {
        InitializeComponent();

        //userwithnosignupcode

        newuser = userpass;


        topprogress.SetProgress(progress, 0);


        //find out the amount left - only 2 pages left after this amount

        progressamount = (100 - progress) / 2;

        faceidstack.IsVisible = true;

        skipbtn.IsVisible = true;


    }

    public RegisterFinalPage(user userpass, double progress, ObservableCollection<userresponse> userresponsep, ObservableCollection<usermeasurement> usermeasurementp, consent additonalcon)
    {
        InitializeComponent();

        //user with NOVO code

        newuser = userpass;
        userresponsepassed = userresponsep;
        usermeasurementpassed = usermeasurementp;

        if(additonalcon != null)
        {
            Additonalconsentinfostack.IsVisible = true;
            additonalconsent = additonalcon;

            actitle.Text = additonalconsent.title;
            acsubtitle.Text = additonalconsent.subtitle;
            accontent.Text = additonalconsent.content;
        }

        

        topprogress.SetProgress(progress, 0);


        //find out the amount left - only 2 pages left after this amount

        progressamount = (100 - progress) / 2;

        faceidstack.IsVisible = true;


    }

    public RegisterFinalPage(user userpass, double progress, ObservableCollection<userresponse> userresponsep, consent additonalcon, ObservableCollection<usersymptom> usersymptompassed, ObservableCollection<usermedication> usermedicationspassed, userdiagnosis userdiagpassed)
    {
        InitializeComponent();

        //user with SFEAT code

        newuser = userpass;
        userresponsepassed = userresponsep;

        symptomstoadd = usersymptompassed;
        medicationstoadd = usermedicationspassed;
        userdiag = userdiagpassed;

        if (additonalcon != null)
        {
            Additonalconsentinfostack.IsVisible = true;
            additonalconsent = additonalcon;

            actitle.Text = additonalconsent.title;
            acsubtitle.Text = additonalconsent.subtitle;
            accontent.Text = additonalconsent.content;
        }



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

            // notificationstack.IsVisible = false;
            // nextbtn.Text = "Finish";
            //// healthdatastack.IsVisible = true;

            // UpdateProgress();
            // topgrid.IsVisible = false
            backbtn.IsVisible = false;
           // bottomstack.IsVisible = false;
            notificationstack.IsVisible = false;
            tcstack.IsVisible = true;
            skipbtn.IsVisible = false;
            nextbtn.Text = "Agree and Finish";
            backbtn.IsVisible = false;
            nextbtn.BackgroundColor = Colors.LightGray;

            topprogress.Progress = 100;
           // await Task.Delay(3000);

           // finishstack.IsVisible = false;



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
                DependencyService.Get<Healthinterface>().GetHealthPermissionAsync(async (result) =>
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
                      //  await Task.Delay(3000);

                        //finishstack.IsVisible = false;
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
            else if(tcstack.IsVisible == true)
            {
                HandleTCframe();
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
    
    async void HandleTCframe()
    {
        try
        {
            if(tccheckbox.IsChecked == true)
            {
                //reg complete , add user 
                this.BackgroundColor = Color.FromArgb("#fefcfe");
                tcstack.IsVisible = false;
                topgrid.IsVisible = false;
                bottomstack.IsVisible = false;
                finishstack.IsVisible = true;

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    imganimation.IsVisible = true;
                }
                else
                {
                    webLogo.IsVisible = true;
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                newuser.registrationstatus = "Active";
                newuser.deviceos = DeviceInfo.Current.VersionString;
                newuser.devicemodel = DeviceInfo.Model;
                newuser.usermigrated = true;

                if(notcheck.IsChecked)
                {
                    newuser.pushnotifications = "True";
                }
                else
                {
                    newuser.pushnotifications = "False";
                }

                if(emailcheck.IsChecked)
                {
                    newuser.emailnotifications = true;
                }
                else
                {
                    newuser.emailnotifications = false;
                }


                //upload the user
                var serializerOptions = new JsonSerializerOptions
                {
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                  //  WriteIndented = true
                };

                var urll = $"https://pwdevapi.peoplewith.com/api/user/userid/{newuser.userid}";
                string json = System.Text.Json.JsonSerializer.Serialize<user>(newuser, serializerOptions);
                StringContent contentt = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PatchAsync(urll, contentt);

                if (response.IsSuccessStatusCode)
                {
                }

                if (!string.IsNullOrEmpty(newuser.signupcodeid))
                {
                    //means they have sign up code so add additional stuff

                    //check if they any questions to add
                    if (userresponsepassed != null || userresponsepassed.Count != 0)
                    {
                        //add the inital questions and answers as user reponses
                        for (int i = 0; i < userresponsepassed.Count; i++)
                        {
                            userresponsepassed[i].id = "123";
                            Uri uri = new Uri(string.Format("https://pwdevapi.peoplewith.com/api/userresponse", string.Empty));
                            // var url = APICalls.InsertUserResponse;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<userresponse>(userresponsepassed[i], serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(uri, contenttt);

                            if (response.IsSuccessStatusCode)
                            {

                            }
                            else
                            {
                                string errorcontent = await response.Content.ReadAsStringAsync();
                                var s = errorcontent;
                            }
                        }

                    }

                    if (symptomstoadd != null || symptomstoadd.Count != 0)
                    {
                        //add the symtpoms
                        foreach (var item in symptomstoadd)
                        {

                            var urls = APICalls.InsertUserSymptoms;
                            string jsonns = System.Text.Json.JsonSerializer.Serialize<usersymptom>(item, serializerOptions);
                            StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(urls, contenttts);

                            if (response.IsSuccessStatusCode)
                            {
                            }
                            else
                            {
                                string errorcontent = await response.Content.ReadAsStringAsync();
                                var s = errorcontent;
                            }
                        }

                    }

                    if (medicationstoadd != null || medicationstoadd.Count != 0)
                    {
                        //add the medications
                        foreach (var item in medicationstoadd)
                        {
                            var url = APICalls.InsertUserMedications;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<usermedication>(item, serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(url, contenttt);

                            if (response.IsSuccessStatusCode)
                            {
                            }
                        }
                    }

                    if(userdiag != null)
                    {
                        //add the user diagnosis
                        var url = APICalls.InsertUserDiagnosis;
                        string jsonn = System.Text.Json.JsonSerializer.Serialize<userdiagnosis>(userdiag, serializerOptions);
                        StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                        response = await client.PostAsync(url, contenttt);

                        if (response.IsSuccessStatusCode)
                        {
                        }
                    }

                    if(usermeasurementpassed != null)
                    {
                        //add the user measurement
                        foreach (var item in usermeasurementpassed)
                        {
                            var url = APICalls.InsertUserMeasurements;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<usermeasurement>(item, serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await client.PostAsync(url, contenttt);

                            if (response.IsSuccessStatusCode)
                            {
                            }
                        }
                    }
                }
                else
                {
                    ///continue to walk in video or dash
                }


                //add the user settings
                Preferences.Default.Set("userid", newuser.userid);
                Preferences.Default.Set("signupcode", newuser.signupcodeid);

                if (!string.IsNullOrEmpty(newuser.userpin))
                {
                    Preferences.Default.Set("pincode", newuser.userpin);
                }
               

                await Task.Delay(3000);
                await Navigation.PushAsync(new MainDashboard(), false);
            

            }
            else
            {
                Vibration.Vibrate();
                tcframe.BorderColor = Colors.Red;
               // tcerror.IsVisible = true;
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

    private void tccheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
           var check = e.Value;

            if(check)
            {
                nextbtn.BackgroundColor = Color.FromArgb("#031926");
            }
            else
            {
                nextbtn.BackgroundColor = Colors.LightGray;
            }

         //   tcerror.IsVisible = false;
            tcframe.BorderColor = Color.FromRgba("#BFDBF7");
        }
        catch( Exception ex )
        {

        }
    }

    private void skipbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if(faceidstack.IsVisible == true)
            {
                faceidstack.IsVisible = false;
                notificationstack.IsVisible = true;
                UpdateProgress();
            }
            else if(notificationstack.IsVisible == true)
            {
                notificationstack.IsVisible = false;
                tcstack.IsVisible = true;
                nextbtn.Text = "Agree and Finish";
                nextbtn.BackgroundColor = Colors.LightGray;
                backbtn.IsVisible = false;
                topprogress.Progress = 100;
                skipbtn.IsVisible = false;
            }

        }
        catch(Exception ex)
        {

        }
    }

    async void BackProgress()
    {
        try
        {

            topprogress.Progress = topprogress.Progress -= progressamount;


        }
        catch (Exception ex)
        {

        }

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //back button
            
            if(notificationstack.IsVisible == true)
            {
                notificationstack.IsVisible = false;
                faceidstack.IsVisible = true;
                BackProgress();
            }
            else if(faceidstack.IsVisible == true)
            {

            }
        }
        catch(Exception ex)
        {

        }
    }
}