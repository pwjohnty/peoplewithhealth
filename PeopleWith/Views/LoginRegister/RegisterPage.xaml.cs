using PeopleWith;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml;
//using Windows.System.UserProfile;
using static Microsoft.Maui.ApplicationModel.Permissions;
using static Microsoft.Maui.Controls.Device;
//using CoreImage;
//using static Android.Gms.Common.Apis.Api;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PeopleWith;

public partial class RegisterPage : ContentPage
{
    user newuser = new user();
    bool nosignupcodebtn;
    public HttpClient Client = new HttpClient();
    public List<string> genlist = new List<string>();
    public List<string> ethnicitylist = new List<string>();
    bool isEditing;
    bool validdob;
    signupcode signupcodeinfo;
    bool heightformatting;
    string heightinput;
    string weightinput;
    List<string> heightinputlist = new List<string>();
    ObservableCollection<question> regquestionlist = new ObservableCollection<question>();
    ObservableCollection<answer> reganswerlist = new ObservableCollection<answer>();
    consent additionalconsent = new consent();
    ObservableCollection<symptom> allsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<medication> allmedicationlist = new ObservableCollection<medication>();
    int progresstoupdate = 11;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    bool onboarding;

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

    public RegisterPage()
	{
        try
        {

        InitializeComponent();

        genlist.Add("Male");
        genlist.Add("Female");
        genlist.Add("Other");

        genderlist.ItemsSource = genlist;

        ethnicitylist.Add("White");
        ethnicitylist.Add("White English");
        ethnicitylist.Add("White Welsh");
        ethnicitylist.Add("White Scottish");
        ethnicitylist.Add("White Northern Irish");
        ethnicitylist.Add("White Irish");
        ethnicitylist.Add("White Gypsy or Irish Traveller");
        ethnicitylist.Add("White Other");
        ethnicitylist.Add("Mixed White and Black Caribbean");
        ethnicitylist.Add("Mixed White and Black African");
        ethnicitylist.Add("Mixed White Other");
        ethnicitylist.Add("Asian Indian");
        ethnicitylist.Add("Asian Pakistani");
        ethnicitylist.Add("Asian Bangladeshi");
        ethnicitylist.Add("Asian Chinese");
        ethnicitylist.Add("Asian Other");
        ethnicitylist.Add("Black African ");
        ethnicitylist.Add("Black African American");
        ethnicitylist.Add("Black Caribbean");
        ethnicitylist.Add("Black Other");
        ethnicitylist.Add("Arab");
        ethnicitylist.Add("Hispanic");
        ethnicitylist.Add("Latino");
        ethnicitylist.Add("Native American");
        ethnicitylist.Add("Pacific Islander");
        ethnicitylist.Add("Other");
        ethnicitylist.Add("Prefer not to disclose");

        ethnicitylist.Sort();

        ethlist.ItemsSource = ethnicitylist;


        heightinput = "Ft";

        MessagingCenter.Subscribe<object>(this, "RemoveProgress", async (sender) =>
        {
            BackProgress();
        });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void firstpasswordentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            string password = e.NewTextValue;

            // Check each requirement and update the UI
            bool hasMinChars = password.Length >= 8;
            bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?-]");
            bool hasCapitalLetter = Regex.IsMatch(password, @"[A-Z]");
            bool hasNumber = Regex.IsMatch(password, @"[0-9]");

            // Update UI
            UpdateOpacity(chartick, charlbl, hasMinChars);
            UpdateOpacity(specialtick, speciallbl, hasSpecialChar);
            UpdateOpacity(capitaltick, capitallbl, hasCapitalLetter);
            UpdateOpacity(numtick, numlbl, hasNumber);

            //change the text back to orginal colour not error colour
            charlbl.TextColor = Color.FromArgb("#031926");
            speciallbl.TextColor = Color.FromArgb("#031926");
            capitallbl.TextColor = Color.FromArgb("#031926");
            numlbl.TextColor = Color.FromArgb("#031926");
        }
        catch (Exception ex)
        {
          //Leave Empty
        }
    }

    private void UpdateOpacity(Image tickImage, Label textLabel, bool isValid)
    {
        tickImage.Opacity = isValid ? 1.0 : 0.2;
        textLabel.Opacity = isValid ? 1.0 : 0.2;

    }

    private void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                nextbtn.IsEnabled = false;
                if (emailframe.IsVisible == true)
                {
                    Handleemailframe();
                    nextbtn.IsEnabled = true;
                }
                else if (confirmemailframe.IsVisible == true)
                {
                    Handleconfirmemailframe();
                    nextbtn.IsEnabled = true;
                }
                else if (signupcodeframe.IsVisible == true)
                {
                    Handlesignupcodeframe();
                    nextbtn.IsEnabled = true;
                }
                else if (signupinfostack.IsVisible == true)
                {
                    Handlesignupinfoframe();
                    nextbtn.IsEnabled = true;
                }
                else if (genderframe.IsVisible == true)
                {
                    Handlegenderframe();
                    nextbtn.IsEnabled = true;
                }
                else if (dobstack.IsVisible == true)
                {
                    Handledobframe();
                    nextbtn.IsEnabled = true;
                }
                else if (ethstack.IsVisible == true)
                {
                    Handleethnicityframe();
                    nextbtn.IsEnabled = true;
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

    async void Handleemailframe()
    {
        try
        {      
            //check email validation
            if(string.IsNullOrEmpty(emailentry.Text) )
            {
                emailhelper.ErrorText = "Please enter an email address";
                emailhelper.HasError = true;
                Vibration.Vibrate();
                emailentry.Focus();
                return;
            }

            //trim email address
           emailentry.Text = emailentry.Text.Trim();

            if (!EmailIsValid(emailentry.Text))
            {
                emailhelper.ErrorText = "Please enter a valid email address";
                emailhelper.HasError = true;
                Vibration.Vibrate();
                emailentry.Focus();
                return;
            }

            //check password validation
            if(chartick.Opacity != 1)
            {
                charlbl.TextColor = Colors.Red;
                charlbl.Opacity = 1;
                Vibration.Vibrate();
                firstpasswordentry.Focus();
                return;
            }

            if (specialtick.Opacity != 1)
            {
                speciallbl.TextColor = Colors.Red;
                speciallbl.Opacity = 1;
                Vibration.Vibrate();
                firstpasswordentry.Focus();
                return;
            }

            if (numtick.Opacity != 1)
            {
                numlbl.TextColor = Colors.Red;
                numlbl.Opacity = 1;
                Vibration.Vibrate();
                firstpasswordentry.Focus();
                return;
            }

            if (capitaltick.Opacity != 1)
            {
                capitallbl.TextColor = Colors.Red;
                capitallbl.Opacity = 1;
                Vibration.Vibrate();
                firstpasswordentry.Focus();
                return;
            }


            //check confirm password
            if(firstpasswordentry.Text != confirmpassentry.Text)
            {
                confirmpasshelper.HasError = true;
                confirmpasshelper.ErrorText = "Passwords do not match";
                Vibration.Vibrate();
                confirmpassentry.Focus();
                return;

            }

            nextbtn.IsVisible = false;
            nextbtnloader.IsVisible = true;

            onboarding = false;

            //check if email is in the db
            
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var url = APICalls.Checkuseremail + "%27" + emailentry.Text + "%27";
            ConfigureClient();
            HttpResponseMessage response = await Client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<APIUserResponse>(content);
                ObservableCollection<user> users = userResponse.Value;

                if(users.Count > 0)
                {
                    //user Still Trying to register
                    if (users[0].registrationstatus == "Onboarding")
                    {
                        onboarding = true;
                        newuser.userid = users[0].userid;
                        //update the ui and progress bar
                        //emailframe.IsVisible = false;
                        //confirmemailframe.IsVisible = true;
                        //nextbtn.IsVisible = true;
                        //nextbtnloader.IsVisible = false;
                        //UpdateProgress();

                        ////add the user into the db with onboarding as the status
                        //newuser.email = emailentry.Text;
                        ////Spelling Mistake (Change)
                        //newuser.registrationstatus = "Onboarding";
                        //return;
                     

                    }
                    else
                    {
                        emailhelper.ErrorText = "Email address already in use";
                        emailhelper.HasError = true;
                        Vibration.Vibrate();
                        emailentry.Focus();
                        nextbtn.IsVisible = true;
                        nextbtnloader.IsVisible = false;
                        return;
                    }
                    
                }
                
            }

            //update the ui and progress bar
            emailframe.IsVisible = false;
            confirmemailframe.IsVisible = true;
            nextbtn.IsVisible = true;
            nextbtnloader.IsVisible = false;
            UpdateProgress();

            

            //add the user into the db with onboarding as the status
            newuser.email = emailentry.Text;
            //Spelling Mistake (Change)
            newuser.registrationstatus = "Onboarding";

            //generate validation code

            var randomnum = new Random();
            var num = randomnum.Next(10000, 99999);

            newuser.validationcode = num.ToString();

            string hashedPassword = await HashPasswordAsync(confirmpassentry.Text);
            newuser.password = hashedPassword;

            newuser.validationcode = num.ToString();

            if (onboarding)
            {
                var serializerOptionss = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var urll = $"https://pwapi.peoplewith.com/api/user/userid/{newuser.userid}";
                string jsonn = System.Text.Json.JsonSerializer.Serialize(new { password = newuser.password, validationcode = newuser.validationcode});

                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage responsee1 = null;

                responsee1 = await Client.PatchAsync(urll, contenttt);


                if (responsee1.IsSuccessStatusCode)
                {
                    string responseBody = await responsee1.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APIUserResponse>(responseBody);
                    var consent = userResponseconsent.Value[0];

                    newuser = consent;
                }

            }
            else
            {




                Uri uri = new Uri(string.Format("https://pwapi.peoplewith.com/api/user", string.Empty));
                string json = System.Text.Json.JsonSerializer.Serialize<user>(newuser, serializerOptions);
                StringContent contentt = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage responsee = null;

                response = await Client.PostAsync(uri, contentt);


                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APIUserResponse>(responseBody);
                    var consent = userResponseconsent.Value[0];

                    newuser = consent;
                }
            }

            //email generate
            StringContent mail_content = new StringContent(newuser.email, System.Text.Encoding.UTF8, "application/json");

            var emailresponse = await Client.PostAsync("https://peoplewithwebapp.azurewebsites.net/hub/email-validation.php?uid=" + newuser.email, mail_content);

           // check if the response is successful
           if (emailresponse.IsSuccessStatusCode)
            {
                await DisplayAlert("Email Sent", "An email containing your confirmation code has been sent. If the email is not in your inbox please check your junk mail. If an email is not received please contact: support@peoplewith.com", "Close"); 

                //string content = await emailresponse.content.readasstringasync();
                // debug.writeline(content); // uncomment this line if you want to debug the content
            }
            else
            {
                // handle the error
              //  string errorcontent = await emailresponse.content.readasstringasync();
                // debug.writeline($"error: {emailresponse.statuscode}, {errorcontent}");
            }
            //// Create an instance of HttpClient
            //using (HttpClient httpClient = new HttpClient())
            //{
            //    // Create the StringContent with the appropriate media type
            //    StringContent mail_content = new StringContent(newuser.Id, System.Text.Encoding.UTF8, "application/json");

            //    // Perform the POST request
            //    var emailresponse = await httpClient.PostAsync("https://peoplewithwebapp.azurewebsites.net/hub/submission-thankyou.php?userID=" + Helpers.Settings.UserKey, mail_content);

            //    // Check if the response is successful
            //    if (emailresponse.IsSuccessStatusCode)
            //    {
            //        string content = await emailresponse.Content.ReadAsStringAsync();
            //        // Debug.WriteLine(content); // Uncomment this line if you want to debug the content
            //    }
            //    else
            //    {
            //        // Handle the error
            //        string errorContent = await emailresponse.Content.ReadAsStringAsync();
            //        // Debug.WriteLine($"Error: {emailresponse.StatusCode}, {errorContent}");
            //    }
            //}

        }
        catch (Exception ex)
        { 
        }

    }

    async void Handleconfirmemailframe()
    {
        try
        {
            //add in validation for confirm code

            if(string.IsNullOrEmpty(emailconfigpin.PINValue))
            {
                // incorrectcodelbl.IsVisible = true;
                emailconfigpin.Focus();
                Vibration.Vibrate();
                return;
            }

            if(emailconfigpin.PINValue == newuser.validationcode)
            {
                confirmemailframe.IsVisible = false;
                signupcodeframe.IsVisible = true;
                UpdateProgress();
                

            }
            else
            {
                incorrectcodelbl.IsVisible = true;
                Vibration.Vibrate();
                await Task.Delay(3000);
                incorrectcodelbl.IsVisible = false;
                return;
            }

        



        }
        catch(Exception ex)
        {
            
        }
    }

    async void Handlesignupcodeframe()
    {
        try
        {
            if(nosignupcodebtn)
            {
                //continue to gender
                signupcodeframe.IsVisible = false;
                genderframe.IsVisible = true;
                nosignupbtn.BorderColor = Colors.Transparent;
                nosignupbtn.BackgroundColor = Color.FromArgb("#BFDBF7");
                nosignupbtn.TextColor = Color.FromArgb("#031926");
                UpdateProgress();
            }
            else
            {
                //check sign up code

                nextbtnloader.IsVisible = true;
                nextbtn.IsVisible = false;


                var url = APICalls.CheckSignUpCode + "%27" + signupcodetext.Text + "%27";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    var userResponse = JsonConvert.DeserializeObject<ApiResponseSignUpCode>(content);
                    ObservableCollection<signupcode> users = userResponse.Value;

                    if (users.Count == 0)
                    {
                        signupfloat.ErrorText = "Invalid sign-up code";
                        signupfloat.HasError = true;
                        Vibration.Vibrate();
                        signupcodetext.Focus();
                        nextbtn.IsVisible = true;
                        nextbtnloader.IsVisible = false;
                        nosignupbtn.BorderColor = Colors.Red;
                        nosignupbtn.TextColor = Colors.Red;
                        return;
                       // UpdateProgress();
                    }
                    else
                    {

                        nosignupbtn.BorderColor = Colors.LightGray;

                        //check if they have any questions and anwers


                        var urll = APICalls.Checksignupregquestions + "%27" + users[0].referral + "%27";

                            HttpResponseMessage responsee = await Client.GetAsync(urll);

                            if (responsee.IsSuccessStatusCode)
                            {
                                string contentt = await responsee.Content.ReadAsStringAsync();
                                var userResponsee = JsonConvert.DeserializeObject<ApiResponseQuestion>(contentt);
                                ObservableCollection<question> questions = userResponsee.Value;

                                if (questions.Count > 0)
                                {
                                
                                     foreach(var item in questions)
                                     {
                                         if(item.area == "Registration")
                                         {

                                             regquestionlist.Add(item);

                                         }
                                     }

                                //get answers for the questions


                                var urlanswers = APICalls.Checksignupreganswers + "%27" + users[0].referral + "%27";

                                HttpResponseMessage responseeanswers = await Client.GetAsync(urlanswers);

                                if (responseeanswers.IsSuccessStatusCode)
                                {
                                    string contenttanswers = await responseeanswers.Content.ReadAsStringAsync();
                                    var userResponseeanswer = JsonConvert.DeserializeObject<ApiResponseAnswer>(contenttanswers);
                                    reganswerlist = userResponseeanswer.Value;
                                }



                                 }

                            }

                            //check if there is additional consent
                        if(users[0].registrationconsent == true)
                        {
                            var urlconsent = APICalls.CheckConsentforsignupcode + "%27" + users[0].signupcodeid + "%27";

                            HttpResponseMessage responseconsent = await Client.GetAsync(urlconsent);

                            if (responseconsent.IsSuccessStatusCode)
                            {
                                string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                                var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseConsent>(contentconsent);
                                var consent = userResponseconsent.Value;

                                additionalconsent = consent.Where(x => x.area == "Registration").SingleOrDefault();

                            }

                        }

                        // This code runs on the UI thread after the background task is complete
                        // You can update the UI or perform other UI-related operations here
                        // Update the UI on the main thread
                       
                        MainThread.BeginInvokeOnMainThread(() =>
                        {

                            signupinfotitle.Text = "Welcome to " + users[0].title;
                            signupinfoimage.Source = ImageSource.FromUri(new Uri("https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/logos/" + users[0].logofilename));
                            signupcodeinfodes.Text = users[0].appdescription;
                        
                            signupcodeinfo = users[0];

                            newuser.signupcodeid = signupcodeinfo.signupcodeid;
                            newuser.referral = signupcodeinfo.referral;

                            if (!string.IsNullOrEmpty(signupcodeinfo.externalidentifier))
                            {
                                extidlbl.Text = signupcodeinfo.externalidentifier;
                            }

                            signupinfostack.IsVisible = true;
                            signupcodeframe.IsVisible = false;
                            nextbtnloader.IsVisible = false;
                            nextbtn.IsVisible = true;
                            progresstoupdate = 6;
                            UpdateProgress();
                        });

                        //has medications and symptoms to get
                        if(signupcodeinfo.referral == "SFEAT" || signupcodeinfo.referral == "SFEWH")
                        {
                            getmedandsymptoms();
                        }
                       
                    }
                  

                }




            }

        }
        catch(Exception ex )
        {

        }
    }

    async void Handlesignupinfoframe()
    {
        try
        {
            signupinfostack.IsVisible = false;
            genderframe.IsVisible = true;

            UpdateProgress();
        }
        catch(Exception ex)
        {

        }

    }

    async void Handlegenderframe()
    {
        try
        {
            if(string.IsNullOrEmpty(newuser.gender))
            {
                Vibration.Vibrate();
                return;
            }
            else
            {
                genderframe.IsVisible = false;
                dobstack.IsVisible = true;
                UpdateProgress();
            }
        }
        catch (Exception ex)
        {

        }
    }

    async void Handledobframe()
    {
        try
        {
            if(validdob)
            {
                newuser.dateofbirth = dateEntry.Text;
              

                //novo dont want ethnicity
                if (signupcodeinfo != null)
                {
                    if (signupcodeinfo.referral == "NOVO")
                    {
                        // UpdateProgress();
                        // heightandweightframe.IsVisible = true;

                        var progressnovo = topprogress.Progress + 5;
                        
                        await Navigation.PushAsync(new NOVO(newuser, progressnovo, signupcodeinfo, regquestionlist, reganswerlist, additionalconsent), false);
                        return;
                    }
                    else
                    {
                        dobstack.IsVisible = false;
                        ethstack.IsVisible = true;
                    }
                }
                else
                {
                    dobstack.IsVisible = false;
                    ethstack.IsVisible = true;
                   
                }

                UpdateProgress();

            }
            else
            {
                Vibration.Vibrate();
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    async void Handleethnicityframe()
    {
        try
        {
            if(string.IsNullOrEmpty(newuser.ethnicity))
            {
                Vibration.Vibrate();
                return;
            }
            else
            {
                //check if they have a sign up code 

                if (!string.IsNullOrEmpty(newuser.signupcodeid))
                {
                    //check signup code and go to page
                    if(signupcodeinfo.referral == "SFEAT")
                    {
                       
                       // UpdateProgress();
                        await Navigation.PushAsync(new SFENRAT(newuser, allsymptomlist, allmedicationlist, signupcodeinfo, topprogress.Progress, regquestionlist, reganswerlist, additionalconsent), false);
                    }
                    else if(signupcodeinfo.referral == "SFEWH")
                    {
                        await Navigation.PushAsync(new WH(newuser, allsymptomlist, allmedicationlist, signupcodeinfo, topprogress.Progress, regquestionlist, reganswerlist, additionalconsent), false);
                    }

                }
                else
                {




                    //pass user and page progress

                    UpdateProgress();
                    await Navigation.PushAsync(new RegisterFinalPage(newuser, topprogress.Progress, additionalconsent), false);
                }
            }
        }
        catch( Exception ex )
        {

        }
    }

    private async Task<string> HashPasswordAsync(string password)
    {
#pragma warning disable CS8603 // Possible null reference return.
        return await Task.Run(() =>
        {
            try
            {
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes = md5.ComputeHash(inputBytes);

                    // Convert the byte array to a hexadecimal string
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        sb.Append(hashBytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
             
                return null;
            }
        });
#pragma warning restore CS8603 // Possible null reference return.
    }

    async void UpdateProgress()
    {
        try
        {

            topprogress.Progress = topprogress.Progress += progresstoupdate;


        }
        catch (Exception ex)
        {

        }

    }


    async void BackProgress()
    {
        try
        {

            topprogress.Progress = topprogress.Progress -= 11;


        }
        catch (Exception ex)
        {

        }

    }

    static Regex ValidEmailRegex = CreateValidEmailRegex();
    /// <summary>
    /// Regex for a valid email
    /// </summary>
    /// <returns>The valid email regex.</returns>
    private static Regex CreateValidEmailRegex()
    {
        string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }
    internal static bool EmailIsValid(string emailAddress)
    {
        bool isValid = ValidEmailRegex.IsMatch(emailAddress);
        return isValid;
    }

    private void emailentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            emailhelper.HasError = false;
        }
        catch (Exception ex)
        {

        }
    }

    private void confirmpassentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            confirmpasshelper.HasError = false;
        }
        catch (Exception ex)
        {

        }
    }

    async private void emailconfigpin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {
            emailconfigpin.IsEnabled = false;
            emailconfigpin.IsEnabled = true;

            if (emailconfigpin.PINValue == newuser.validationcode)
            {
                confirmemailframe.IsVisible = false;
                signupcodeframe.IsVisible = true;
                UpdateProgress();
            }
            else
            {
                incorrectcodelbl.IsVisible = true;
                Vibration.Vibrate();
                await Task.Delay(3000);
                incorrectcodelbl.IsVisible = false;
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void nosignupbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (nosignupcodebtn)
            {
                nosignupcodebtn = false;
                nosignupbtn.FontFamily = "HankenGroteskRegular";
                nosignupbtn.TextColor = Colors.Gray;
                nosignupbtn.BackgroundColor = Colors.Transparent;
                nosignupbtn.BorderColor = Colors.LightGray;
                nosignupbtn.FontAttributes = FontAttributes.None;
            }
            else
            {
                nosignupcodebtn = true;

                nosignupbtn.BorderColor = Colors.Transparent;
                nosignupbtn.BackgroundColor = Color.FromArgb("#BFDBF7");
                nosignupbtn.TextColor = Color.FromArgb("#031926");
                nosignupbtn.FontAttributes = FontAttributes.Bold;
                nosignupbtn.FontFamily = "HankenGroteskBold";
                nosignupbtn.Unfocus();
                signupcodetext.Text = string.Empty;
                signupfloat.HasError = false;
                signupcodetext.IsEnabled = false;
                signupcodetext.IsEnabled = true;
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void signupcodetext_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if(e.NewTextValue != string.Empty)
            {
                nosignupbtn.FontFamily = "HankenGroteskRegular";
                nosignupbtn.TextColor = Colors.Gray;
                nosignupbtn.BackgroundColor = Colors.Transparent;
                nosignupbtn.BorderColor = Colors.LightGray;
                nosignupbtn.FontAttributes = FontAttributes.None;
                
                signupfloat.HasError = false;
                nosignupcodebtn = false;
               // signupcodetext.IsEnabled = false;
               // signupcodetext.IsEnabled = true;
            }
      

        }
        catch (Exception ex)
        {

        }
    }

    private void emailconfigpin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            incorrectcodelbl.IsVisible = false;
        }
        catch(Exception ex)
        {

        }
    }

    private void dateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (isEditing)
                return;

            isEditing = true;

            string input = e.NewTextValue;

            // Remove any non-numeric characters except '/'
            input = new string(input.Where(c => char.IsDigit(c) || c == '/').ToArray());

            // Remove existing slashes to reformat correctly
            input = input.Replace("/", string.Empty);

            // Limit the input to a maximum of 8 numeric characters (DDMMYYYY)
            if (input.Length >= 8)
            {
                input = input.Substring(0, 8);
                dateEntry.IsEnabled = false;
                dateEntry.IsEnabled = true;
            }

            // Insert slashes at the appropriate positions
            if (input.Length > 2)
                input = input.Insert(2, "/");

            if (input.Length > 5)
                input = input.Insert(5, "/");

            // Check for valid date parts and set the text color accordingly
            if (input.Length == 10)
            {
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    // Check if the date is between 1900 and today's year
                    int currentYear = DateTime.Now.Year;
                    if (date.Year >= 1900 && date.Year <= currentYear)
                    {
                        if (date.Date <= DateTime.Now.Date)
                        {
                            dateEntry.TextColor = Color.FromArgb("#031926"); // Valid date
                            validdob = true;
                        }
                        else
                        {
                            dateEntry.TextColor = Colors.Red; // Invalid date range
                            validdob = false;
                        }

                    }
                    else
                    {
                        dateEntry.TextColor = Colors.Red; // Invalid date
                        validdob = false;
                    }
                }
            }
            else
            {
                dateEntry.TextColor = Color.FromArgb("#031926"); // Intermediate input
                validdob = false;
            }

            dateEntry.Text = input;

            // Adjust cursor position
            dateEntry.CursorPosition = input.Length;

            isEditing = false;
        }
        catch(Exception ex)
        {

        }
    }

    private void genderlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;

            if (item != null)
            {
                newuser.gender = item;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void ethlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;

            if (item != null)
            {
                newuser.ethnicity = item;
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void heightinputentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (heightformatting) return;

            heightformatting = true;

            var currentText = e.NewTextValue;

            if (heightinput == "Ft")
            {

                // Remove non-digit characters
                var digits = new string(currentText.Where(char.IsDigit).ToArray());



                // Limit to 3 digits
                if (digits.Length >= 3)
                {
                    digits = digits.Substring(0, 3);
                    heightinputentry.IsEnabled = false;
                    heightinputentry.IsEnabled = true;
                }

                // Format the text
                string formattedText = string.Empty;

                if (digits.Length > 0)
                {
                    formattedText += $"{digits[0]}' ";
                }

                if (digits.Length > 2)
                {

                    // Remove existing single and double quotes to prevent duplication
                    //formattedText.Remove(3);
                    // formattedText.Replace("'", "").Replace("\"", "");
                    formattedText += $"{digits[1]}";
                    formattedText += $"{digits[2]}''";
                }

                else if (digits.Length > 1)
                {
                    formattedText += $"{digits[1]}''";
                }
             



                // Update the entry text and label
                if (!string.IsNullOrEmpty(formattedText))
                {
                    heightinputentry.Opacity = 0;
                    heightinputlbl.Text = formattedText;
                    heightinputlbl.IsVisible = true;
                }
                else
                {
                    heightinputentry.Opacity = 1;
                    heightinputlbl.IsVisible = false;
                }

            }
            else
            {
                //cm
            }

            heightformatting = false;
        }
        catch(Exception ex)
        {

        }
    }

    private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {
            var selectedvalue = e.NewIndex;


            if(selectedvalue != null)
            {
           //     heightinputentry.Opacity = 1;
             //   heightinputentry.Text = string.Empty;
             //   heightinputlbl.Text = string.Empty;

                if (selectedvalue == 0)
                {
                    heightinput = "Ft";
                    heightgauge.Minimum = 0;
                    heightgauge.Maximum = 96;
                    heightgauge.Interval = 12;


                    if (!string.IsNullOrEmpty(heightinputlbl.Text))
                    {
                        if (heightinputlbl.Text.Contains("cm"))
                        {
                            //convert cm to ft
                            var getcm = heightinputlbl.Text.Split(' ');

                            var cm = Convert.ToDouble(getcm[0]);

                            double totalInches = cm / 2.54;
                            int feet = (int)(totalInches / 12);
                            double inches = totalInches % 12;



                            //update label
                            heightinputlbl.Text = feet.ToString() + "' " + inches.ToString() + "''";

                            //update guage
                            heightpointerguage.Value = totalInches;


                        }
                    }
                }
                else
                {
                    heightinput = "CM";
                    heightgauge.Minimum = 0;
                    heightgauge.Maximum = 250;
                    heightgauge.Interval = 25;


                    if (!string.IsNullOrEmpty(heightinputlbl.Text))
                    {
                        if (heightinputlbl.Text.Contains("''"))
                        {
                            //convert ft to cm
                            var ftin = heightinputlbl.Text.Split(' ');

                            var ftnum = new String(ftin[0].Where(Char.IsDigit).ToArray());
                            var innum = new String(ftin[1].Where(Char.IsDigit).ToArray());

                            var ft = Convert.ToDouble(ftnum);
                            var ins = Convert.ToDouble(innum);

                            double fttotal = (ft * 30.48);
                                
                            double instotal =(ins * 2.54);

                            var total = fttotal + instotal;

                            //update label
                            heightinputlbl.Text = fttotal.ToString() + "' " + instotal.ToString() + "''";

                            //update guage
                            heightpointerguage.Value = total;


                        }
                    }

                }
                
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void SfLinearGauge_LabelCreated(object sender, Syncfusion.Maui.Gauges.LabelCreatedEventArgs e)
    {
        try
        {
            if (heightinput == "Ft")
            {

                if (e.Text == "0")
                    e.Text = "0'";
                else if (e.Text == "12")
                    e.Text = "1'";
                else if (e.Text == "24")
                    e.Text = "2'";
                else if (e.Text == "36")
                    e.Text = "3'";
                else if (e.Text == "48")
                    e.Text = "4'";
                else if (e.Text == "60")
                    e.Text = "5'";
                else if (e.Text == "72")
                    e.Text = "6'";
                else if (e.Text == "84")
                    e.Text = "7'";
                else if (e.Text == "96")
                    e.Text = "8'";
            }
            else
            {
                //do nothing for cm
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void LinearShapePointer_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
    {
        try
        {
            var value = e.Value;

            if (heightinput == "Ft")
            {

                int feet = (int)(value / 12); // 1 foot = 12 inches
                int inches = (int)(value % 12); // Remaining inches

                var stringvalue = feet + "' " + inches + "''";

                heightinputlbl.Text = stringvalue;
            }
            else
            {
                int OtherInt = Convert.ToInt32(value);

                heightinputlbl.Text = OtherInt.ToString() + " cm";
            }
        }
        catch(Exception ex) 
        { 
        }

    }

    private void weightgauge_LabelCreated(object sender, Syncfusion.Maui.Gauges.LabelCreatedEventArgs e)
    {
        try
        {
            if(weightinput == "Stone")
            {
                if (e.Text == "0")
                    e.Text = "0";
                else if (e.Text == "14")
                    e.Text = "1";
                else if (e.Text == "28")
                    e.Text = "2";
                else if (e.Text == "42")
                    e.Text = "3";
                else if (e.Text == "56")
                    e.Text = "4";
                else if (e.Text == "70")
                    e.Text = "5";
                else if (e.Text == "84")
                    e.Text = "6";
                else if (e.Text == "98")
                    e.Text = "7";
                else if (e.Text == "112")
                    e.Text = "8";
                else if (e.Text == "126")
                    e.Text = "9";
                else if (e.Text == "140")
                    e.Text = "10";
                else if (e.Text == "154")
                    e.Text = "11";
                else if (e.Text == "168")
                    e.Text = "12";
                else if (e.Text == "182")
                    e.Text = "13";
                else if (e.Text == "196")
                    e.Text = "14";
                else if (e.Text == "210")
                    e.Text = "15";
                else if (e.Text == "224")
                    e.Text = "16";
                else if (e.Text == "238")
                    e.Text = "17";
                else if (e.Text == "252")
                    e.Text = "18";
                else if (e.Text == "266")
                    e.Text = "19";
                else if (e.Text == "280")
                    e.Text = "20";
                else if (e.Text == "294")
                    e.Text = "21";
                else if (e.Text == "308")
                    e.Text = "22";
                else if (e.Text == "322")
                    e.Text = "23";
                else if (e.Text == "336")
                    e.Text = "24";
                else if (e.Text == "350")
                    e.Text = "25";
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void LinearShapePointer_ValueChanged_1(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
    {
        try
        {
            var value = e.Value;

            if (weightinput == "Kg")
            {

                int OtherInt = Convert.ToInt32(value);

                weightinputlbl.Text = OtherInt.ToString() + " kg";
            }
            else
            {

                int stone = (int)(value / 14); // 1 foot = 12 inches
                int pounds = (int)(value % 14); // Remaining inches

                var stringvalue = stone + "st " + pounds + "lbs";

                weightinputlbl.Text = stringvalue;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void segmentedControlweight_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {
            var selectedvalue = e.NewIndex;

          


            if (selectedvalue != null)
            {
                // heightinputentry.Opacity = 1;
                // heightinputentry.Text = string.Empty;

               // weightinputlbl.Text = string.Empty;

                if (selectedvalue == 0)
                {
                    weightinput = "Kg";
                    weightgauge.Minimum = 0;
                    weightgauge.Maximum = 200;
                    weightgauge.Interval = 20;

                    if (!string.IsNullOrEmpty(weightinputlbl.Text))
                    {
                        if (weightinputlbl.Text.Contains("st"))
                        {
                            //convert stone to kg
                            var getstlbs = weightinputlbl.Text.Split(' ');
                         
                            var stonenum = new String(getstlbs[0].Where(Char.IsDigit).ToArray());
                            var lbsnum = new String(getstlbs[1].Where(Char.IsDigit).ToArray());

                            var convertst = Convert.ToDouble(stonenum);
                            var convertlbs = Convert.ToDouble(lbsnum);

                            double totalPounds = (convertst * 14) + convertlbs;
                            var total = totalPounds * 0.453592;


                            //update label
                              weightinputlbl.Text = total.ToString() + "kg";

                            //update guage
                              weightguagepointer.Value = total;
                        }
                    }

                }
                else
                {
                    weightinput = "Stone";
                    weightgauge.Minimum = 0;
                    weightgauge.Maximum = 350;
                    weightgauge.Interval = 14;

                    if (!string.IsNullOrEmpty(weightinputlbl.Text))
                    {
                        if (weightinputlbl.Text.Contains("kg"))
                        {
                            //convert kg to stone
                            var getkg = weightinputlbl.Text.Split(' ');
                            var kg = Convert.ToDouble(getkg[0]);
                            //stones calulation
                            double totalPounds = kg * 2.20462;
                            int stones = (int)(totalPounds / 14);
                            double pounds = totalPounds % 14;

                            //update label
                            weightinputlbl.Text = stones.ToString() + "st " + pounds + "lbs";

                            //update guage
                            weightguagepointer.Value = totalPounds;
                        }
                    }

                    // weightgauge.MaximumLabelsCount = 0;
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void SegmentedControl_ValueChanged(object sender, Plugin.Maui.SegmentedControl.ValueChangedEventArgs e)
    {
        try
        {
            var selectedvalue = e.NewValue;


            if (selectedvalue != null)
            {
                //     heightinputentry.Opacity = 1;
                //   heightinputentry.Text = string.Empty;
                //   heightinputlbl.Text = string.Empty;

                if (selectedvalue == 0)
                {
                    heightinput = "Ft";
                    heightgauge.Minimum = 0;
                    heightgauge.Maximum = 96;
                    heightgauge.Interval = 12;


                    if (!string.IsNullOrEmpty(heightinputlbl.Text))
                    {
                        if (heightinputlbl.Text.Contains("cm"))
                        {
                            //convert cm to ft
                            var getcm = heightinputlbl.Text.Split(' ');

                            var cm = Convert.ToDouble(getcm[0]);

                            double totalInches = cm / 2.54;
                            int feet = (int)(totalInches / 12);
                            double inches = totalInches % 12;



                            //update label
                            heightinputlbl.Text = feet.ToString() + "' " + inches.ToString() + "''";

                            //update guage
                            heightpointerguage.Value = totalInches;


                        }
                    }
                }
                else
                {
                    heightinput = "CM";
                    heightgauge.Minimum = 0;
                    heightgauge.Maximum = 250;
                    heightgauge.Interval = 25;


                    if (!string.IsNullOrEmpty(heightinputlbl.Text))
                    {
                        if (heightinputlbl.Text.Contains("''"))
                        {
                            //convert ft to cm
                            var ftin = heightinputlbl.Text.Split(' ');

                            var ftnum = new String(ftin[0].Where(Char.IsDigit).ToArray());
                            var innum = new String(ftin[1].Where(Char.IsDigit).ToArray());

                            var ft = Convert.ToDouble(ftnum);
                            var ins = Convert.ToDouble(innum);

                            double fttotal = (ft * 30.48);

                            double instotal = (ins * 2.54);

                            var total = fttotal + instotal;

                            //update label
                            heightinputlbl.Text = fttotal.ToString() + "' " + instotal.ToString() + "''";

                            //update guage
                            heightpointerguage.Value = total;


                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {

        }
    }

    private void SegmentedControlweight_ValueChanged(object sender, Plugin.Maui.SegmentedControl.ValueChangedEventArgs e)
    {
        try
        {
            var selectedvalue = e.NewValue;




            if (selectedvalue != null)
            {
                // heightinputentry.Opacity = 1;
                // heightinputentry.Text = string.Empty;

                // weightinputlbl.Text = string.Empty;

                if (selectedvalue == 0)
                {
                    weightinput = "Kg";
                    weightgauge.Minimum = 0;
                    weightgauge.Maximum = 200;
                    weightgauge.Interval = 20;

                    if (!string.IsNullOrEmpty(weightinputlbl.Text))
                    {
                        if (weightinputlbl.Text.Contains("st"))
                        {
                            //convert stone to kg
                            var getstlbs = weightinputlbl.Text.Split(' ');

                            var stonenum = new String(getstlbs[0].Where(Char.IsDigit).ToArray());
                            var lbsnum = new String(getstlbs[1].Where(Char.IsDigit).ToArray());

                            var convertst = Convert.ToDouble(stonenum);
                            var convertlbs = Convert.ToDouble(lbsnum);

                            double totalPounds = (convertst * 14) + convertlbs;
                            var total = totalPounds * 0.453592;


                            //update label
                            weightinputlbl.Text = total.ToString() + "kg";

                            //update guage
                            weightguagepointer.Value = total;
                        }
                    }

                }
                else
                {
                    weightinput = "Stone";
                    weightgauge.Minimum = 0;
                    weightgauge.Maximum = 350;
                    weightgauge.Interval = 14;

                    if (!string.IsNullOrEmpty(weightinputlbl.Text))
                    {
                        if (weightinputlbl.Text.Contains("kg"))
                        {
                            //convert kg to stone
                            var getkg = weightinputlbl.Text.Split(' ');
                            var kg = Convert.ToDouble(getkg[0]);
                            //stones calulation
                            double totalPounds = kg * 2.20462;
                            int stones = (int)(totalPounds / 14);
                            double pounds = totalPounds % 14;

                            //update label
                            weightinputlbl.Text = stones.ToString() + "st " + pounds + "lbs";

                            //update guage
                            weightguagepointer.Value = totalPounds;
                        }
                    }

                    // weightgauge.MaximumLabelsCount = 0;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    async void getmedandsymptoms()
    {
        try
        {
            //get all the symptoms
            var urlsymptom = APICalls.GetSymptoms;
            ConfigureClient();
            HttpResponseMessage responseconsent = await Client.GetAsync(urlsymptom);

            if (responseconsent.IsSuccessStatusCode)
            {
                string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseSymptom>(contentconsent);
                var consent = userResponseconsent.Value;

                allsymptomlist = consent;

              //  additionalconsent = consent.Where(x => x.area == "Registration").SingleOrDefault();

            }

            //get all medications
            //get all the symptoms
            var urlmedications = APICalls.GetMedications;

            HttpResponseMessage responsemeds = await Client.GetAsync(urlmedications);

            if (responsemeds.IsSuccessStatusCode)
            {
                string contentmeds = await responsemeds.Content.ReadAsStringAsync();
                var userResponsemed = JsonConvert.DeserializeObject<ApiResponseMedication>(contentmeds);
                var meds = userResponsemed.Value;

                allmedicationlist = meds;

                //  additionalconsent = consent.Where(x => x.area == "Registration").SingleOrDefault();
            
            }


        }
        catch(Exception ex)
        {

        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //back button

            if(ethstack.IsVisible == true)
            {
                ethstack.IsVisible = false;
                dobstack.IsVisible = true;

                BackProgress();
            }
            else if(dobstack.IsVisible == true)
            {
                dobstack.IsVisible = false;
                genderframe.IsVisible = true;

                BackProgress();
            }
            else if(genderframe.IsVisible == true)
            {
                genderframe.IsVisible = false;

                if (signupcodeinfo == null)
                {
                    //has no sign up code
                   
                    signupcodeframe.IsVisible = true;
                }
                else
                {
                    signupinfostack.IsVisible = true;
                }

                BackProgress();
            }
            else if(signupinfostack.IsVisible == true)
            {
                signupinfostack.IsVisible = false;
                signupcodeframe.IsVisible = true;

                regquestionlist.Clear();
                reganswerlist.Clear();
                newuser.signupcodeid = null;
                newuser.referral = null;
                additionalconsent = new consent();
                allsymptomlist.Clear();
                allmedicationlist.Clear();
                BackProgress();
            }
            else if(signupcodeframe.IsVisible == true)
            {
                signupcodeframe.IsVisible = false;
                confirmemailframe.IsVisible = true;

                BackProgress();
            }
            else if(confirmemailframe.IsVisible == true)
            {
                confirmemailframe.IsVisible = false;
                emailframe.IsVisible = true;

                BackProgress();
            }
            else if(emailframe.IsVisible == true)
            {
                Navigation.RemovePage(this);
            }
        }
        catch(Exception ex)
        {

        }
    }

    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            string BackArrow = "PeopleWith";
            await Navigation.PushAsync(new PrivacyPolicy(BackArrow), false);
        }
        catch (Exception Ex)
        {

        }
    }

    async private void ResendCodeTapped(object sender, TappedEventArgs e)
    {

        var randomnum = new Random();
        var num = randomnum.Next(10000, 99999);

        newuser.validationcode = num.ToString();

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var url = $"https://pwapi.peoplewith.com/api/user/userid/{newuser.userid}";
        string json = System.Text.Json.JsonSerializer.Serialize(new { validationcode = num});

        StringContent contentt = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage responsee = null;
        ConfigureClient();
        responsee = await Client.PatchAsync(url, contentt);


        if (responsee.IsSuccessStatusCode)
        {
            string responseBody = await responsee.Content.ReadAsStringAsync();
            var userResponseconsent = JsonConvert.DeserializeObject<APIUserResponse>(responseBody);
            var consent = userResponseconsent.Value[0];

            newuser = consent;
        }

        //email generate
        StringContent mail_content = new StringContent(newuser.email, System.Text.Encoding.UTF8, "application/json");

        var emailresponse = await Client.PostAsync("https://peoplewithwebapp.azurewebsites.net/hub/email-validation.php?uid=" + newuser.email, mail_content);

        // check if the response is successful
        if (emailresponse.IsSuccessStatusCode)
        {
            await DisplayAlert("Email Sent", "Email Containing Confirmation Code Sent, If the email is not in your inbox. Check your Junk Mail", "Close");
        }
        else
        {
        }
    }
}