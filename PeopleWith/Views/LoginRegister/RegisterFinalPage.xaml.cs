using Maui.FreakyControls;
using PeopleWith;
using PeopleWith.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Azure.Storage.Blobs;
using static System.Net.WebRequestMethods;
using Syncfusion.Maui.Core.Internals;

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
    bool SignPadhaddata = false;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }


    public RegisterFinalPage()
	{
        try
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            await Task.Delay(100);
            imganimation.IsAnimationPlaying = false;
            await Task.Delay(100);
            imganimation.IsAnimationPlaying = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public RegisterFinalPage(user userpass, double progress, consent additonalcon)
    {

        try
        {
            InitializeComponent();

            //userwithnosignupcode

            newuser = userpass;

            if (additonalcon != null)
            {
                additonalconsent = additonalcon;
            }


            topprogress.SetProgress(progress, 0);


            //find out the amount left - only 2 pages left after this amount

            progressamount = (100 - progress) / 2;

            faceidstack.IsVisible = true;

            skipbtn.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    public RegisterFinalPage(user userpass, double progress, ObservableCollection<userresponse> userresponsep, ObservableCollection<usermeasurement> usermeasurementp, consent additonalcon)
    {
        try
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public RegisterFinalPage(user userpass, double progress, ObservableCollection<userresponse> userresponsep, consent additonalcon, ObservableCollection<usersymptom> usersymptompassed, ObservableCollection<usermedication> usermedicationspassed, userdiagnosis userdiagpassed)
    {
        try
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                nextbtn.IsEnabled = false;
                if (faceidstack.IsVisible == true)
                {
                    Handlepinstack();
                    nextbtn.IsEnabled = true;
                }

                else if (notificationstack.IsVisible == true)
                {
                    HandleNotificationframe();
                    nextbtn.IsEnabled = true;
                }
                else if (healthdatastack.IsVisible == true)
                {
                    HandleHealthdataframe();
                }
                else if (tcstack.IsVisible == true)
                {
                    //Check if Signature pad is needed 
                    if (additonalconsent.signaturepad == false)
                    {

                        HandleTCframe();
                        nextbtn.IsEnabled = true;
                    }
                    else
                    {
                        if (nextbtn.BackgroundColor == Colors.LightGray)
                        {
                            if (tccheckbox.IsChecked == false)
                            {
                                Vibration.Vibrate();
                                tcframe.BorderColor = Colors.Red;
                                nextbtn.IsEnabled = true;
                                return;
                            }
                            else
                            {
                                if (SignPadhaddata == false)
                                {
                                    await DisplayAlert("Signature Missing", "Please sign the signature pad", "OK");
                                    nextbtn.IsEnabled = true;
                                    return;
                                }

                            }

                        }
                        else
                        {
                            HandleTCframe();
                            nextbtn.IsEnabled = true;
                        }
                    }

                }
                
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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
                //ask if they want to use faceid


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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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

                PermissionStatus status;

                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // Request and capture the permission status on Android
                    status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                    if (status == PermissionStatus.Granted)
                    {
                        // Set notifications as enabled
                        newuser.pushnotifications = "True";
                    }
                    else
                    {
                        // Set notifications as disabled
                        newuser.pushnotifications = "Disabled";
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
                        newuser.pushnotifications = "True";
                    }
                    else
                    {
                        // Set notifications as disabled
                        newuser.pushnotifications = "Disabled";
                    }
                }


                if (emailcheck.IsChecked)
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

                if(additonalconsent.signaturepad == true)
                {
                    string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=peoplewithappiamges;AccountKey=9maBMGnjWp6KfOnOuXWHqveV4LPKyOnlCgtkiKQOeA+d+cr/trKApvPTdQ+piyQJlicOE6dpeAWA56uD39YJhg==;EndpointSuffix=core.windows.net";

                    var backrandom = new Random();
                    var backrandomnum = backrandom.Next(1000, 10000000);
                    var backimagename = newuser.userid + "-" + DateTime.Now.ToString("HHmmssfff") + "-" + backrandomnum + ".Jpeg";

                    // Parse the connection string and create a blob client
                    BlobServiceClient blobServiceClient = new BlobServiceClient(StorageConnectionString);

                    // Get a reference to the container
                    BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("consentsignatures");

                    // Get a reference to the blob
                    BlobClient blobClient = containerClient.GetBlobClient(backimagename);

                    //Send Signature to Azure 
                    Stream signatureStream = await signpad.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Jpeg);

                    // Upload the signature image stream to Azure Blob Storage
                    if (signatureStream != null)
                    {
                        await blobClient.UploadAsync(signatureStream);
                    }

                    //User Consent 
                    var UpdateUserConsent = new userconsent();
                    UpdateUserConsent.userid = newuser.userid;
                    UpdateUserConsent.consentid = additonalconsent.consentid;
                    UpdateUserConsent.signaturefilename = backimagename;

                    APICalls database = new APICalls();
                    await database.PostUserConsentAsync(UpdateUserConsent); 

                }


                //add the user settings
                Preferences.Default.Set("userid", newuser.userid);
                Preferences.Default.Set("signupcode", newuser.signupcodeid);
                Preferences.Set("email", newuser.email);
                Preferences.Default.Set("pincode", newuser.userpin);
                Preferences.Default.Set("gender", newuser.gender);
                Preferences.Default.Set("ethnicity", newuser.ethnicity);
                Preferences.Default.Set("age", newuser.dateofbirth);
                Preferences.Default.Set("userpasswordhash", newuser.password);
                Preferences.Default.Set("notifications", newuser.pushnotifications);
                Preferences.Default.Set("usermigrated", newuser.usermigrated.ToString());



                //add the userfeedback column
                var newuseritem = new userfeedback();
                newuseritem.userid = newuser.userid;

                APICalls databasee = new APICalls();
                await databasee.InsertUserFeedback(newuseritem);

                // Preferences.Default.Set("validationcode", newuser.validationcode);

                await Task.Delay(3000);
                Application.Current.MainPage = new NavigationPage(new MainDashboard());


            }
            else
            {
                Vibration.Vibrate();
                tcframe.BorderColor = Colors.Red;
               // tcerror.IsVisible = true;
                return;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void UpdateProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress += progressamount;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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
        catch(Exception ex)
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
        catch(Exception ex)
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

    private void tccheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
           var check = e.Value;

           
            if(additonalconsent.signaturepad == false)
            {
                if (check)
                {
                    nextbtn.BackgroundColor = Color.FromArgb("#031926");
                }
                else
                {
                    nextbtn.BackgroundColor = Colors.LightGray;
                    tcframe.BorderColor = Color.FromRgba("#BFDBF7");
                }

                tcerror.IsVisible = false;
                tcframe.BorderColor = Color.FromRgba("#BFDBF7");
            }
            else 
            {
                signaturePadStack.IsVisible = true;
                ConsentBoxesStack.IsVisible = false;
                tcerror.IsVisible = false;
            }
            
        }
        catch( Exception ex )
        {
            //Leave Empty
        }
    }

    private void skipbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                skipbtn.IsEnabled = false; 
                if (faceidstack.IsVisible == true)
                {
                    faceidstack.IsVisible = false;
                    notificationstack.IsVisible = true;
                    UpdateProgress();
                }
                else if (notificationstack.IsVisible == true)
                {
                    notificationstack.IsVisible = false;
                    tcstack.IsVisible = true;
                    nextbtn.Text = "Agree and Finish";
                    nextbtn.BackgroundColor = Colors.LightGray;
                    backbtn.IsVisible = false;
                    topprogress.Progress = 100;
                    skipbtn.IsVisible = false;
                }
                skipbtn.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void BackProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress -= progressamount;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //back button
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (notificationstack.IsVisible == true)
                {
                    notificationstack.IsVisible = false;
                    faceidstack.IsVisible = true;
                    BackProgress();
                }
                else if (faceidstack.IsVisible == true)
                {
                    Navigation.RemovePage(this);
                }
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }        
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                string BackArrow = "PeopleWith";
                await Navigation.PushAsync(new PrivacyPolicy(BackArrow), false);
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private void signpad_StrokeCompleted(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bool check = !signpad.IsBlank;
    //        if (check)
    //        {
    //            nextbtn.BackgroundColor = Color.FromArgb("#031926");
    //        }
    //        else
    //        {
    //            nextbtn.BackgroundColor = Colors.LightGray;
    //        }

    //        tcerror.IsVisible = false;

    //    }
    //    catch (Exception Ex)
    //    {

    //    }
    //}

    async private void signpad_DrawCompleted(object sender, EventArgs e)
    {
        try
        {
            var signatureStream = await signpad.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Jpeg);
            bool isSignatureBlank = signatureStream.Length == 0;

            if (!isSignatureBlank)
            {
                SignPadhaddata = true; 
                nextbtn.BackgroundColor = Color.FromArgb("#031926");
            }
            else
            {
                SignPadhaddata = false;
                nextbtn.BackgroundColor = Colors.LightGray;
            }

        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                signpad.Clear();
                nextbtn.BackgroundColor = Colors.LightGray;
                SignPadhaddata = false;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }          
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}