//using AndroidX.Activity;
using MauiApp1;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
//using static Android.Gms.Common.Apis.Api;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PeopleWith;

public partial class RegisterPage : ContentPage
{
    user newuser = new user();
    bool nosignupcodebtn;
    HttpClient client = new HttpClient();
    public RegisterPage()
	{
		InitializeComponent();
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

            if(emailframe.IsVisible == true)
            {
                Handleemailframe();
                
            }
            else if(confirmemailframe.IsVisible == true)
            {
                Handleconfirmemailframe();
            }
            else if(signupcodeframe.IsVisible == true)
            {
                Handlesignupcodeframe();
            }
         



        }
        catch(Exception ex)
        {

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

            //check if email is in the db
            
            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var url = APICalls.Checkuseremail + "%27" + emailentry.Text + "%27";

            HttpResponseMessage response = await client.GetAsync(url);

            if(response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<UserResponse>(content);
                ObservableCollection<user> users = userResponse.Value;

                if(users.Count > 0)
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

            //update the ui and progress bar
            emailframe.IsVisible = false;
            confirmemailframe.IsVisible = true;
            nextbtn.IsVisible = true;
            nextbtnloader.IsVisible = false;
            UpdateProgress();

            //add the user into the db with onboarding as the status
            newuser.email = emailentry.Text;
            newuser.registrationstatus = "Onbaording";

            //generate validation code
            var randomnum = new Random();
            var num = randomnum.Next(10000, 99999);

            newuser.validationcode = num.ToString();

            string hashedPassword = await HashPasswordAsync(confirmpassentry.Text);
            newuser.password = hashedPassword;

            Uri uri = new Uri(string.Format("https://pwdevapi.peoplewith.com/api/user", string.Empty));
            string json = System.Text.Json.JsonSerializer.Serialize<user>(newuser, serializerOptions);
            StringContent contentt = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage responsee = null;

            response = await client.PostAsync(uri, contentt);


            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
            }

            //email generate
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

            if(emailconfigpin.PINValue == newuser.validationcode)
            {
                confirmemailframe.IsVisible = false;
                signupcodeframe.IsVisible = true;
                UpdateProgress();
                

            }
            else
            {
                incorrectcodelbl.IsVisible = true;
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

            }
            else
            {
                //check sign up code

                nextbtnloader.IsVisible = true;
                nextbtn.IsVisible = false;


                var url = APICalls.CheckSignUpCode + "%27" + signupcodetext.Text + "%27";

                HttpResponseMessage response = await client.GetAsync(url);

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
                        return;
                       // UpdateProgress();
                    }
                    else
                    {
                        signupinfotitle.Text = "Welcome to " + users[0].title;

                        signupinfostack.IsVisible = true;
                        signupcodeframe.IsVisible = false;
                        nextbtnloader.IsVisible = false;
                        nextbtn.IsVisible = true;
                        UpdateProgress();
                    }
                  

                }




            }

        }
        catch(Exception ex )
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

            topprogress.Progress = topprogress.Progress += 11;


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

    private void emailconfigpin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
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
}