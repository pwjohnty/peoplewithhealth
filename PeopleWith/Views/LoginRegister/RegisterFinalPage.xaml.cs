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
using Plugin.LocalNotification;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Newtonsoft.Json.Linq;

namespace PeopleWith;

public partial class RegisterFinalPage : ContentPage
{
    user newuser;
    double progressamount;
    public ObservableCollection<userresponse> userresponsepassed;
    public ObservableCollection<usermeasurement> usermeasurementpassed;
    public ObservableCollection<userdiagnosis> userdiagnosispassed = new ObservableCollection<userdiagnosis>();
    public consent additonalconsent = new consent();
    ObservableCollection<usermedication> medicationstoadd = new ObservableCollection<usermedication>();
    ObservableCollection<usersymptom> symptomstoadd = new ObservableCollection<usersymptom>();
    public HttpClient Client = new HttpClient();
    userdiagnosis userdiag;
    bool SignPadhaddata = false;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();
    userfeedback UserFeedbackToAdd = new userfeedback();
    userdiet DietToAdd = new userdiet(); 

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

    private void ConfigureClient()
    {
        try
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
            Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
        }
        catch (Exception Ex)
        {
            //Empty
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

    public RegisterFinalPage(user userpass, double progress, ObservableCollection<userresponse> userresponsep, consent additonalcon, ObservableCollection<usersymptom> usersymptompassed, ObservableCollection<usermedication> usermedicationspassed, ObservableCollection<userdiagnosis> userdiagpassed, ObservableCollection<usermeasurement> usermeasurementspass, userdiet DietPassed)
    {
        try
        {
            InitializeComponent();

            //user with SFEAT code

            newuser = userpass;
            userresponsepassed = userresponsep;

            symptomstoadd = usersymptompassed;
            medicationstoadd = usermedicationspassed;
            //userdiag = userdiagpassed;
            usermeasurementpassed = usermeasurementspass;
            userdiagnosispassed = userdiagpassed;
            DietToAdd = DietPassed;

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

    public RegisterFinalPage(user userpass, double progress, ObservableCollection<userresponse> userresponsep, consent additonalcon, ObservableCollection<usersymptom> usersymptompassed, ObservableCollection<usermedication> usermedicationspassed, ObservableCollection<userdiagnosis> userdiagpassed, ObservableCollection<usermeasurement> usermeasurementspass)
    {
        try
        {
            InitializeComponent();

            //user with SFECORE code

            newuser = userpass;
            userresponsepassed = userresponsep;

            symptomstoadd = usersymptompassed;
            medicationstoadd = usermedicationspassed;
            //userdiag = userdiagpassed;
            usermeasurementpassed = usermeasurementspass;
            userdiagnosispassed = userdiagpassed;

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
            //test

            await LocalNotificationCenter.Current.RequestNotificationPermission();

            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //{
            //    await Permissions.RequestAsync<Permissions.PostNotifications>();
            //}
            //else
            //{
            //    var notificationService = DependencyService.Get<INotificationService>();
            //    await notificationService.RequestNotificationPermissionAsync();

            //}

            // notificationstack.IsVisible = false;
            // nextbtn.Text = "Finish";
            //// healthdatastack.IsVisible = true;

            // UpdateProgress();
            // topgrid.IsVisible = false
            backbtn.IsVisible = false;
           // bottomstack.IsVisible = false;
            notificationstack.IsVisible = false;
            tcstack.IsVisible = true;

            if (additonalconsent.signaturepad == true)
            {
                signaturePadStack.IsVisible = true;
            }

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
            await DisplayAlert("Error", Ex.Source.ToString() + Ex.InnerException.ToString(), "OK");
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
                //DependencyService.Get<Healthinterface>().GetHealthPermissionAsync(async (result) =>
                //{
                //    var a = result;
                //    if (result)
                //    {

                //        //topgrid.IsVisible = false;
                //        //bottomstack.IsVisible = false;
                //        //healthdatastack.IsVisible = false;
                //        //finishstack.IsVisible = true;
                //        //await Task.Delay(3000);

                //        //finishstack.IsVisible = false;
                //    }
                //    else
                //    {
                //        topgrid.IsVisible = false;
                //        bottomstack.IsVisible = false;
                //        healthdatastack.IsVisible = false;
                //        finishstack.IsVisible = true;
                //      //  await Task.Delay(3000);

                //        //finishstack.IsVisible = false;
                //    }
                //});
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
                        if (nextbtn.BackgroundColor != Color.FromArgb("#031926"))
                        {
                            if (tccheckbox.IsChecked == false)
                            {
                                Vibration.Vibrate();
                                tcframe.BorderColor = Colors.Red;
                                nextbtn.IsEnabled = true;
                                return;
                            }

                            if (SignPadhaddata == false)
                            {
                                Vibration.Vibrate();
                                if(DeviceInfo.Current.Platform == DevicePlatform.Android)
                                {
                                    AndroidSign.Stroke = Colors.Red;
                                }
                                else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                                {
                                    IOSSign.Stroke = Colors.Red;
                                }
                                
                                nextbtn.IsEnabled = true;
                                return;
                            }

                            HandleTCframe();
                            nextbtn.IsEnabled = true;
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

                var checknotifications = await LocalNotificationCenter.Current.AreNotificationsEnabled();

                if ((checknotifications))
                {
                    newuser.pushnotifications = "True";
                }
                else
                {
                    newuser.pushnotifications = "Disabled";
                }

                //PermissionStatus status;

                //if (DeviceInfo.Platform == DevicePlatform.Android)
                //{
                //    // Request and capture the permission status on Android
                //    status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                //    if (status == PermissionStatus.Granted)
                //    {
                //        // Set notifications as enabled
                //        newuser.pushnotifications = "True";
                //    }
                //    else
                //    {
                //        // Set notifications as disabled
                //        newuser.pushnotifications = "Disabled";
                //    }
                //}
                //else
                //{
                //    // Request permission on iOS via dependency service
                //    var notificationService = DependencyService.Get<INotificationService>();
                //    bool isGranted = await notificationService.CheckRequestNotificationPermissionAsync();

                //    // Set notifications based on whether permission was granted
                //    if (isGranted)
                //    {
                //        // Set notifications as enabled
                //        newuser.pushnotifications = "True";
                //    }
                //    else
                //    {
                //        // Set notifications as disabled
                //        newuser.pushnotifications = "Disabled";
                //    }
                //}


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

                var urll = $"https://pwapi.peoplewith.com/api/user/userid/{newuser.userid}";
                string json = System.Text.Json.JsonSerializer.Serialize<user>(newuser, serializerOptions);
                StringContent contentt = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                var response = await Client.PatchAsync(urll, contentt);

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
                            Uri uri = new Uri(string.Format("https://pwapi.peoplewith.com/api/userresponse", string.Empty));
                            // var url = APICalls.InsertUserResponse;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<userresponse>(userresponsepassed[i], serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await Client.PostAsync(uri, contenttt);

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
                        if (userfeedbacklistpassed.symptomfeedbacklist == null)
                        {
                            userfeedbacklistpassed.symptomfeedbacklist = new ObservableCollection<feedbackdata>();
                        }

                        userfeedbacklistpassed.userid = newuser.userid;

                        //add the symtpoms
                        foreach (var item in symptomstoadd)
                        {

                            var urls = APICalls.InsertUserSymptoms;
                            string jsonns = System.Text.Json.JsonSerializer.Serialize<usersymptom>(item, serializerOptions);
                            StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                            response = await Client.PostAsync(urls, contenttts);

                            if (response.IsSuccessStatusCode)
                            {
                            }
                            else
                            {
                                string errorcontent = await response.Content.ReadAsStringAsync();
                                var s = errorcontent;
                            }


                            //add to feedback list
                            var newsym = new feedbackdata();
                            newsym.value = "50";
                            newsym.datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                            newsym.action = "add";
                            newsym.label = item.symptomtitle;
                            newsym.id = item.feedback[0].symptomfeedbackid;
                            

                            userfeedbacklistpassed.symptomfeedbacklist.Add(newsym);
                        }

                        string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.symptomfeedbacklist);
                        userfeedbacklistpassed.symptomfeedback = newsymJson;

                     //   APICalls databaseenew = new APICalls();
                     //   await databaseenew.InsertUserFeedback(userfeedbacklistpassed);

                    }

                    if (medicationstoadd != null || medicationstoadd.Count != 0)
                    {
                        //add the medications
                        //Ensures the same Diagnosis isnt added twice
                        var distinctMeds = medicationstoadd.GroupBy(x => x.medicationid).Select(g => g.First());
                        //Previous 
                        //foreach (var item in medicationstoadd)
                        foreach (var item in distinctMeds)
                        {
                            var url = APICalls.InsertUserMedications;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<usermedication>(item, serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await Client.PostAsync(url, contenttt);

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
                        response = await Client.PostAsync(url, contenttt);

                        if (response.IsSuccessStatusCode)
                        {
                        }
                    }

                    if (userdiagnosispassed != null || userdiagnosispassed.Count != 0)
                    {
                        //Ensures the same Diagnosis isnt added twice
                        var distinctDiagnoses = userdiagnosispassed.GroupBy(x => x.diagnosisid).Select(g => g.First());
                        //Previous 
                        //foreach (var item in userdiagnosispassed)
                        foreach (var item in distinctDiagnoses)
                        {
                            //add the user diagnosis
                            var url = APICalls.InsertUserDiagnosis;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<userdiagnosis>(item, serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await Client.PostAsync(url, contenttt);

                            if (response.IsSuccessStatusCode)
                            {
                            }
                        }
                    }
                    if(DietToAdd != null)
                    {
                        //add the user Diet
                        var url = APICalls.InsertUserDiet;
                        string jsonn = System.Text.Json.JsonSerializer.Serialize<userdiet>(DietToAdd, serializerOptions);
                        StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                        response = await Client.PostAsync(url, contenttt);

                        if (response.IsSuccessStatusCode)
                        {
                        }
                    }

                    if (usermeasurementpassed != null)
                    {
                        if (userfeedbacklistpassed.measurementfeedbacklist == null)
                        {
                            userfeedbacklistpassed.measurementfeedbacklist = new ObservableCollection<feedbackdata>();
                        }

                        //add the user measurement
                        foreach (var item in usermeasurementpassed)
                        {
                            var url = APICalls.InsertUserMeasurements;
                            string jsonn = System.Text.Json.JsonSerializer.Serialize<usermeasurement>(item, serializerOptions);
                            StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                            response = await Client.PostAsync(url, contenttt);
                            string ID = String.Empty; 
                            if (response.IsSuccessStatusCode)
                            {
                                // Read the response content as a string
                                string responseContent = await response.Content.ReadAsStringAsync();
                                var jsonResponse = JObject.Parse(responseContent);
                                ID = jsonResponse["value"]?[0]?["id"]?.ToString();

                            }

                            //add to feedback list
                            var newmeas = new feedbackdata();
                            newmeas.id = ID;
                            newmeas.value = item.value;
                            newmeas.datetime = item.inputdatetime;
                            newmeas.action = "update";
                            newmeas.label = item.measurementname;
                            newmeas.unit = item.unit;

                            userfeedbacklistpassed.measurementfeedbacklist.Add(newmeas);
                        }

                        string newMeasJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.measurementfeedbacklist);
                        userfeedbacklistpassed.measurementfeedback = newMeasJson;
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
                    if (DeviceInfo.Platform == DevicePlatform.iOS)
                    {

                        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

                        Stream drawingStream = await (DrawingView.GetImageStream(drawingpad.Lines, new Size(150, 150),
                                Microsoft.Maui.Graphics.Colors.Transparent, cts.Token));


                        if (drawingStream != null)
                        {
                            await blobClient.UploadAsync(drawingStream);
                        }
                    }
                    else
                    {
                        Stream signatureStream = await signpad.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Jpeg);

                        if (signatureStream != null)
                        {
                            await blobClient.UploadAsync(signatureStream);
                        }
                    }



                    // Upload the signature image stream to Azure Blob Storage


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

                if(newuser.referral != null || !string.IsNullOrEmpty(newuser.referral))
                {
                    if(newuser.referral == "NOVO")
                    {
                        Preferences.Default.Set("NovoMeds", true);
                        Preferences.Default.Set("NovoSyms", true);
                        Preferences.Default.Set("NovoSupps", true);
                        Preferences.Default.Set("NovoMeas", true);
                        Preferences.Default.Set("NovoDiag", true);
                        Preferences.Default.Set("NovoMood", true);
                        Preferences.Default.Set("NovoAppt", true);
                        Preferences.Default.Set("NovoHcp", true);
                        Preferences.Default.Set("NovoQues", true);
                        Preferences.Default.Set("NovoAllerg", true);
                        Preferences.Default.Set("NovoHeRep", true);
                        Preferences.Default.Set("NovoSched", true);
                        Preferences.Default.Set("NovoFood", true);
                        Preferences.Default.Set("NovoDiet", true);
                        Preferences.Default.Set("NovoInvest", true);
                        Preferences.Default.Set("NovoActivity", true);
                    }
                }



                //add the userfeedback column
               // var newuseritem = new userfeedback();
                userfeedbacklistpassed.userid = newuser.userid;

                APICalls databasee = new APICalls();
                await databasee.InsertUserFeedback(userfeedbacklistpassed);


                //add questionnaire notification for nsat 
                if(newuser.signupcodeid != null)
                {
                    if(newuser.signupcodeid.Contains("SFEAT"))
                    {
                        var notification = new NotificationRequest
                        {
                            NotificationId = new Random().Next(1, int.MaxValue),
                            Title = "Complete your EQ-5D Questionnaire",
                            Description = "Please take a moment to complete your EQ-5D questionnaire",
                            BadgeNumber = 0,
                           
                             Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                             {
                                 Priority = Plugin.LocalNotification.AndroidOption.AndroidPriority.High, // ?? Set priority here
                             },

                            // Add any custom data you need to retrieve when the notification is tapped
                            //ReturningData = "action=questionnaire&studyid=IID3",

                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = DateTime.Now.AddDays(1),
                                RepeatType = NotificationRepeat.No,
                                NotifyRepeatInterval = null,
                               
                            }

                        };

                        LocalNotificationCenter.Current.Show(notification);

                        //Save NotificationiD to local Storage
                        Preferences.Set("NsatNotID", notification.NotificationId.ToString());
                        
                    }
                    else if (newuser.signupcodeid.Contains("SFECORE"))
                    {
                        var notification = new NotificationRequest
                        {
                            NotificationId = new Random().Next(1, int.MaxValue),
                            Title = "Complete your SF-36 General Health Questionnaire",
                            Description = "Please take a moment to complete your SF-36 questionnaire",
                            BadgeNumber = 0,

                            Android = new Plugin.LocalNotification.AndroidOption.AndroidOptions
                            {
                                Priority = Plugin.LocalNotification.AndroidOption.AndroidPriority.High, // ?? Set priority here
                            },

                            // Add any custom data you need to retrieve when the notification is tapped
                            //ReturningData = "action=questionnaire&studyid=IID3",

                            Schedule = new NotificationRequestSchedule
                            {
                                NotifyTime = DateTime.Now.AddDays(1),
                                RepeatType = NotificationRepeat.No,
                                NotifyRepeatInterval = null,

                            }
                        };

                        LocalNotificationCenter.Current.Show(notification);

                        //Save NotificationiD to local Storage
                        Preferences.Set("SFEcoreNotID", notification.NotificationId.ToString());
                    }
                }


                // Preferences.Default.Set("validationcode", newuser.validationcode);
                await Task.Run(async () =>
                {
                    // Simulate some processing that may take up to seconds
                    await Task.Delay(100);
                });

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage = new NavigationPage(new MainDashboard());
                });

                //Task.Run(async () =>
                //{
                //    await Task.Delay(100); // Simulate processing time if necessary
                //    MainThread.BeginInvokeOnMainThread(async () =>
                //    {
                //        Application.Current.MainPage = new NavigationPage(new MainDashboard());
                //    });
                //});

                //Application.Current.MainPage = new NavigationPage(new MainDashboard());

                //Application.Current.MainPage = new NavigationPage(new MainDashboard());
                //Task.Run(async () =>
                //{
                //    await Task.Delay(3000); 
                //     MainThread.BeginInvokeOnMainThread(async () =>
                //    {
                //        Application.Current.MainPage = new NavigationPage(new MainDashboard());
                //    });
                //});


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

            if (check)
            {
                //SignaturePad Not required
                if (additonalconsent.signaturepad == false)
                {
                    tcframe.BorderColor = Color.FromRgba("#BFDBF7");
                    nextbtn.BackgroundColor = Color.FromArgb("#031926");
                }
                else
                {
                    if (SignPadhaddata == true)
                    {
                        tcframe.BorderColor = Color.FromRgba("#BFDBF7");
                        nextbtn.BackgroundColor = Color.FromArgb("#031926");
                    }
                    else
                    {
                        tcframe.BorderColor = Color.FromRgba("#BFDBF7");
                        nextbtn.BackgroundColor = Colors.LightGray;
                    }
                }
            }
            else
            {
                nextbtn.BackgroundColor = Colors.LightGray;
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

                    if(additonalconsent.signaturepad == true)
                    {
                        signaturePadStack.IsVisible = true;
                    }

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

    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if(DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    signpad.Clear(); 
                }
                else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                {
                    drawingpad.Clear();
                }

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

    async private void signpad_DrawCompleted(object sender, EventArgs e)
    {
        try
        {
            var signatureStream = await signpad.GetStreamAsync(Syncfusion.Maui.Core.ImageFileFormat.Jpeg);
            bool isSignatureBlank = signatureStream.Length == 0;


            if (!isSignatureBlank)
            {

                SignPadhaddata = true;
                AndroidSign.Stroke = Color.FromArgb("#BFDBF7");

                if (tccheckbox.IsChecked == true)
                {
                    nextbtn.BackgroundColor = Color.FromArgb("#031926");
                }
                else
                {
                    nextbtn.BackgroundColor = Colors.LightGray;
                }
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

    private async void DrawingView_DrawingLineCompleted(object sender, CommunityToolkit.Maui.Core.DrawingLineCompletedEventArgs e)
    {
        try
        {
            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

            Stream drawingStream = await (DrawingView.GetImageStream(drawingpad.Lines, new Size(150, 150),
                    Microsoft.Maui.Graphics.Colors.Transparent, cts.Token));
            bool isSignatureBlank = drawingStream.Length == 0;


            if (!isSignatureBlank)
            {

                SignPadhaddata = true;
                IOSSign.Stroke = Color.FromArgb("#BFDBF7");

                if (tccheckbox.IsChecked == true)
                {
                    nextbtn.BackgroundColor = Color.FromArgb("#031926");
                }
                else
                {
                    nextbtn.BackgroundColor = Colors.LightGray;
                }
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
}