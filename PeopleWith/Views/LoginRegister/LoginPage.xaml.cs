using Microsoft.Maui.Layouts;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;


namespace PeopleWith;

public partial class LoginPage : ContentPage
{
    HttpClient client = new HttpClient();
    string userid;
    bool IsResetPin; 
    ObservableCollection<user> users = new ObservableCollection<user>(); 
    public LoginPage()
	{
        try
        {
            InitializeComponent();
            IsResetPin = false; 
        }
        catch (Exception Ex)
        {

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
        catch (Exception ex)
        {

        }
    }

    private void emailentry_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

   async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ForgottenPassword(), false);
        }
        catch (Exception Ex)
        {

        }
    }

    private void Signin_Clicked(object sender, EventArgs e)
    {
        try
        {
            Handleemailframe(); 
        }
        catch(Exception Ex)
        {

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

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var url = APICalls.Checkuseremail + "%27" + emailentry.Text + "%27";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<APIUserResponse>(content);
                users = userResponse.Value;

                if (users.Count == 0)
                {
                    emailhelper.ErrorText = "Email does not Exist";
                    emailhelper.HasError = true;
                    Vibration.Vibrate();
                    emailentry.Focus();
                    return;
                }
                else
                {
                    if (users[0].deleted == true)
                    {
                        await DisplayAlert("Account Deleted", "Your account has been deleted", "OK");
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
                Preferences.Set("launchvideo", "seen");
                Preferences.Set("clinicaltrial", "Yes");
   
                //Add Push Notification Tags
                AddBackTags(); 
                await Navigation.PushAsync(new MainDashboard(), false);
                
            }
            else
            {
                return; 
            }


        }
        catch (Exception ex)
        {
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
        catch (Exception ex)
        {

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
}