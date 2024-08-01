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
    public List<string> genlist = new List<string>();
    public List<string> ethnicitylist = new List<string>();
    bool isEditing;
    bool validdob;
    signupcode signupcodeinfo;
    public RegisterPage()
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
            //add in the sign up info page here
            else if(genderframe.IsVisible == true)
            {
                Handlegenderframe();
            }
            else if(dobstack.IsVisible == true)
            {
                Handledobframe();
            }
            else if(ethstack.IsVisible == true)
            {
                Handleethnicityframe();
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
                
                UpdateProgress();
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

                        signupcodeinfo = users[0];
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
                dobstack.IsVisible = false;

                //novo dont want ethnicity
                if (signupcodeinfo != null)
                {
                    if (signupcodeinfo.referral == "NOVO")
                    {
                        //skip to additional steps ie health kit, notifications, face id
                    }
                }
                else
                {

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
                //pass user and page progress
                
                UpdateProgress();
                await Navigation.PushAsync(new RegisterFinalPage(newuser, topprogress.Progress), false);
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
}