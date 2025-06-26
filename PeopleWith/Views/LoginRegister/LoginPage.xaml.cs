using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;
using System.Net;


namespace PeopleWith;

public partial class LoginPage : ContentPage
{
    public HttpClient Client = new HttpClient();
    string userid;
    bool IsResetPin; 
    ObservableCollection<user> users = new ObservableCollection<user>();
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

    public LoginPage()
	{
        try
        {
            InitializeComponent();
            IsResetPin = false; 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    public LoginPage(string Pin)
    {
        try
        {
            InitializeComponent();
            IsResetPin = true;  
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    static Regex ValidEmailRegex = CreateValidEmailRegex();

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

    private void UpdateOpacity(Image tickImage, Label textLabel, bool isValid)
    {
        tickImage.Opacity = isValid ? 1.0 : 0.2;
        textLabel.Opacity = isValid ? 1.0 : 0.2;

    }
    async private void firstpasswordentry_TextChanged(object sender, TextChangedEventArgs e)
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void emailentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            emailhelper.HasError = false;
            emailhelper.ErrorText = "";
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ForgottenPassword(), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Signin_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                Signin.IsEnabled = false;
                Handleemailframe();
                await Task.Delay(1500);
                Signin.IsEnabled = true;
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
            bool ValidEmail = false;
            bool ValidPassword = false; 
            //check email validation
            if (string.IsNullOrEmpty(emailentry.Text))
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
            if (chartick.Opacity != 1)
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

            //check if email is in the db

            Signinload.IsVisible = true;
            Signin.IsVisible = false; 

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
                users = userResponse.Value;

                if (users.Count == 0)
                {
                    emailhelper.ErrorText = "Email does not Exist";
                    emailhelper.HasError = true;
                    Signinload.IsVisible = false;
                    Signin.IsVisible = true;
                    Vibration.Vibrate();
                    emailentry.Focus();
                    return;
                }
                else
                {
                    if (users[0].deleted == true)
                    {
                        await DisplayAlert("Account Deleted", "Your account has been deleted", "OK");
                        Signinload.IsVisible = false;
                        Signin.IsVisible = true;
                        return;
                    }
                    else if (users[0].registrationstatus == "Onboarding")
                    {
                        await DisplayAlert("Account Onboarding", "Please use your email to continue registering", "OK");
                        Signinload.IsVisible = false;
                        Signin.IsVisible = true;
                        return;
                    }
                    else if (users[0].registrationstatus == "Withdrawn")
                    {
                        //Royal Brompton Prject 
                        if (users[0].signupcodeid.Contains("RBHTHCM"))
                        {
                            await DisplayAlert("Withdrawn from Project", "You have withdrawn from the project and can no longer access your account", "OK");
                        }
                        else
                        {
                            //All Other Studies
                            await DisplayAlert("Withdrawn from Study", "You have withdrawn from the study and can no longer access your account", "OK");
                        }
                        Signinload.IsVisible = false;
                        Signin.IsVisible = true;
                        return;
                    }

                    ValidEmail = true;

                    string passwordtocompare = users[0].password;
                    Byte[] UserPasswordByte = GetHash(firstpasswordentry.Text);
                    string userpassword = ByteArrayToHex(UserPasswordByte);

                    if (passwordtocompare == userpassword)
                    {
                        ValidPassword = true;
                        userid = users[0].userid;
                    }
                    else 
                    {
                        passhelper.ErrorText = "Password Incorrect, Try Again";
                        passhelper.HasError = true;
                        Signinload.IsVisible = false;
                        Signin.IsVisible = true;
                        Vibration.Vibrate();
                        firstpasswordentry.Focus();
                        return;
                    }
               }
            }

            //Correct Email and Password - Signin 
            if (ValidEmail ==  true && ValidPassword == true)
            {
                //Add back Helpers.Settings 
                Preferences.Set("userid", users[0].userid);
                Preferences.Set("firstname", users[0].firstname);
                Preferences.Set("surname", users[0].surname);
                Preferences.Set("gender", users[0].gender);
                Preferences.Set("email", users[0].email);
                Preferences.Set("password", users[0].password);
                Preferences.Set("age", users[0].dateofbirth);
                Preferences.Set("ethnicity", users[0].ethnicity);
                Preferences.Set("userpasswordhash", users[0].password);
                Preferences.Set("signupcode", users[0].signupcodeid);
                Preferences.Set("pincode", users[0].userpin);
                Preferences.Set("notifications", users[0].pushnotifications);
                Preferences.Set("usermigrated", users[0].usermigrated.ToString());

                AddBackTags();

                if (users[0].usermigrated == false)
                {
                    //add the userfeedback column
                    var newuseritem = new userfeedback();
                    newuseritem.userid = users[0].userid;

                    APICalls databasee = new APICalls();
                    //await databasee.InsertUserFeedback(newuseritem);

                    await Navigation.PushAsync(new MigrationAssistant(), false);
                    return;
                }


                //  Preferences.Set("launchvideo", "seen");
                //Preferences.Set("clinicaltrial", "Yes");
                //Change based on User Having Notifcations Enabled/Disabled
                // Preferences.Set("NotificationsEnabled", true);
                // Preferences.Set("validationcode", users[0].validationcode);

                //Add Push Notification Tags
                var setnotificationloginbool = true;
                await Task.Delay(2000); 
                Application.Current.MainPage = new NavigationPage(new MainDashboard(setnotificationloginbool));

            }
            else
            {
                Signinload.IsVisible = false;
                Signin.IsVisible = true;
                return; 
            }
        }
        catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is WebException ||
            ex is TaskCanceledException)
        {
            NotasyncMethod(ex);

        }
        catch (Exception ex)
        {
            NotasyncMethod(ex);
        }
    }
    private async void AddBackTags()
    {
        try
        {
            IList<string> tags = new List<string>();
            if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                tags.Add(Helpers.Settings.SignUp);
            }
            if (!string.IsNullOrEmpty(Helpers.Settings.UserKey))
            {
                tags.Add(Helpers.Settings.UserKey);
            }
            if (tags.Count > 0)
            {
                var notificationService = new PWNotificationService();
                await notificationService.AddTag(tags);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public byte[] GetHash(string data)
    {
        using (var md5 = MD5.Create())
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            return md5.ComputeHash(dataBytes);
        }
    }

    public string ByteArrayToHex(byte[] hash)
    {
        var hex = new StringBuilder(hash.Length * 2);
        foreach (byte b in hash)
            hex.AppendFormat("{0:x2}", b);

        return hex.ToString();
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

    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Navigation.RemovePage(this);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}