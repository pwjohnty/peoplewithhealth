﻿using Mopups.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PeopleWith;

public partial class ProfileEdit : ContentPage

{
    user AllUserData = new user();
    public List<string> genlist = new List<string>();
    public List<string> ethnicitylist = new List<string>();
    HttpClient client = new HttpClient();
    string ItemTitle;
    string SelectedList;
    bool isEditing;
    bool validdob;
    string ValidationCode; 
    static Regex ValidEmailRegex = CreateValidEmailRegex();
    PasswordEncryption Encryption = new PasswordEncryption();
    public ProfileEdit(string SelectedItem, string Selected, user AllUserDetails)
    {
        try
        {
            InitializeComponent();
            ItemTitle = SelectedItem;
            AllUserData = AllUserDetails;
            SelectedList = Selected;
            if (SelectedList == "Health Details")
            {
                EditHealthDetails();
            }
            else if (SelectedList == "Settings")
            {
                EditSettings();
            }
            else if (SelectedList == "Privacy")
            {
                EditPrivacy();
            }
        }
        catch (Exception Ex)
        {

        }
    }
    //Health Details Populate 
    private void EditHealthDetails()
    {
        try
        {
            //Set Title Text
            HealthDetailslbl.Text = ItemTitle;
            HealthDetailsStack.IsVisible = true;

            if (ItemTitle == "Name")
            {
                NameStack.IsVisible = true;
                HealthDetailsUpdate.Text = "Update " + ItemTitle;
                if (!String.IsNullOrEmpty(AllUserData.firstname))
                {
                    FirstNameEntry.Text = AllUserData.firstname;
                }
                if (!String.IsNullOrEmpty(AllUserData.surname))
                {
                    SurNameEntry.Text = AllUserData.surname;
                }

            }
            else if (ItemTitle == "Email")
            {
                EmailStack.IsVisible = true;
                HealthDetailsUpdate.Text = "Update " + ItemTitle;

                if (!String.IsNullOrEmpty(AllUserData.email))
                {
                    EmailEntry.Text = AllUserData.email;
                }
            }
            else if (ItemTitle == "Date of Birth")
            {
                DateofBirthStack.IsVisible = true;
                HealthDetailsUpdate.Text = "Update " + ItemTitle;

                if (!String.IsNullOrEmpty(AllUserData.dateofbirth))
                {
                    DateofBirthEntry.Text = AllUserData.dateofbirth;
                }
            }
            else if (ItemTitle == "Gender")
            {
                GenderStack.IsVisible = true;
                HealthDetailsUpdate.Text = "Update " + ItemTitle;
                genlist.Add("Male");
                genlist.Add("Female");
                genlist.Add("Other");

                if (!String.IsNullOrEmpty(AllUserData.gender))
                {

                    //Puts Selected item to top of list 
                    genlist.Remove(AllUserData.gender);
                    genlist.Insert(0, AllUserData.gender);
                    genderlist.ItemsSource = genlist;
                    genderlist.SelectedItem = AllUserData.gender;

                }
                else
                {
                    genderlist.ItemsSource = genlist;
                }

            }
            else if (ItemTitle == "Ethnicity")
            {
                HealthDetailsUpdate.Text = "Update " + ItemTitle;
                EthnicityStack.IsVisible = true;

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

                if (!String.IsNullOrEmpty(AllUserData.ethnicity))
                {
                    //Puts Selected item to top of list 
                    ethnicitylist.Remove(AllUserData.ethnicity);
                    ethnicitylist.Insert(0, AllUserData.ethnicity);
                    ethlist.ItemsSource = ethnicitylist;
                    ethlist.SelectedItem = AllUserData.ethnicity;

                }
                else
                {
                    ethlist.ItemsSource = ethnicitylist;
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }

    //Settings Populate 
    private void EditSettings()
    {
        try
        {
            //Set Title Text
            SettingsStack.IsVisible = true;
            SettingsStacklbl.Text = ItemTitle;

            if (ItemTitle == "Reset Password")
            {
                //CreateNewPin();
                EmailConfirmStack.IsVisible = true;
                SettingsUpdate.Text = "Check Password"; 


            }
            else if (ItemTitle == "Notifications")
            {
                //Needs Added
                NotificationsStack.IsVisible = true;
                SettingsUpdate.Text = "Update " + ItemTitle;
            }
            else if (ItemTitle == "Signup Code")
            {
                //Create Password Reset Code
                SignupcodeStack.IsVisible = true; 
                SettingsUpdate.Text = "Update " + ItemTitle;
            }
        }
        catch (Exception Ex)
        {

        }
    }

    //Privacy Populate
    private void EditPrivacy()
    {
        try
        {
            PrivacyStack.IsVisible = true;
            PrivacyStacklbl.Text = ItemTitle;

            if (ItemTitle == "Reset Pin")
            {
                CurrentPinCodeStack.IsVisible = true; 
                PrivacyStacklbl.Text = "Enter Current Pin";
                PrivacyUpdate.IsVisible = false;
                PrivacyUpdate.Text = "Update Pin";
                CurrentPin.Focus(); 
            }
            else if (ItemTitle == "Permissions")
            {
                PermissionsStack.IsVisible = true;
                PrivacyUpdate.IsVisible = false;
                if (string.IsNullOrEmpty(AllUserData.userpin))
                {
                    AllUserData.pintoggled = false;
                }
                else
                {
                    AllUserData.pintoggled = true;
                }

                PinSwitch.IsToggled = AllUserData.pintoggled;
                if (AllUserData.biometrics == true)
                {
                    FingerSwitch.IsToggled = true;
                }

                PinSwitch.ThumbColor = Color.FromHex("#E5E5E5");
                PinSwitch.OnColor = Colors.Green;
                FingerSwitch.ThumbColor = Color.FromHex("#E5E5E5");
                FingerSwitch.OnColor = Colors.Green;

            }
        }
        catch (Exception Ex)
        {

        }
    }

    private void FirstNameENtry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //Nothing Needed 
        }
        catch (Exception Ex)
        {

        }
    }

    private void SurNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //Nothing Needed 
        }
        catch (Exception Ex)
        {

        }
    }

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

    async private void CreateNewPin()
    {
        try
        {
            bool PinAdded = false; 
            Random random = new Random();
            int randomNumber = random.Next(10000, 100000);
            ValidationCode = randomNumber.ToString();
            string id = Helpers.Settings.UserKey;
            var url = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

            string json = System.Text.Json.JsonSerializer.Serialize(new { validationcode = ValidationCode });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = content
                };

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    PinAdded = true;
                }

            }
        }
        catch (Exception Ex)
        {

        }
    }

    async void handleNameframe()
    {
        try
        {
            var FirstName = FirstNameEntry.Text;
            var SecondName = SurNameEntry.Text;
            bool FirstNameChanged = false;
            bool SecondNameChanged = false;
            if (!String.IsNullOrEmpty(FirstName))
            {
                string id = Helpers.Settings.UserKey;
                var url = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { firstname = FirstName });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        FirstNameChanged = true;
                    }
                }

            }

            if (!String.IsNullOrEmpty(SecondName))
            {
                string id = Helpers.Settings.UserKey;
                var url = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { surname = SecondName });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        SecondNameChanged = true;
                    }
                }
            }

            if (FirstNameChanged == true)
            {
                Helpers.Settings.FirstName = FirstName;
                AllUserData.firstname = FirstName; 
            }
            if (SecondNameChanged == true)
            {
                Helpers.Settings.Surname = SecondName;
                AllUserData.surname = SecondName; 
            }

            await MopupService.Instance.PushAsync(new PopupPageHelper("Name Updated") { });
            await Task.Delay(1500);
           
            await Navigation.PushAsync(new ProfileSection(AllUserData), false);

            var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
            if (pageToRemoves != null)
            {

                Navigation.RemovePage(pageToRemoves);
            }
            Navigation.RemovePage(this);

            await MopupService.Instance.PopAllAsync(false);

        }
        catch (Exception Ex)
        {

        }
    }


    async void Handleemailframe()
    {
        try
        {
            //check email validation
            if (string.IsNullOrEmpty(EmailEntry.Text))
            {
                emailhelper.ErrorText = "Please enter an email address";
                emailhelper.HasError = true;
                Vibration.Vibrate();
                EmailEntry.Focus();
                return;
            }

            //trim email address
            EmailEntry.Text = EmailEntry.Text.Trim();

            if (!EmailIsValid(EmailEntry.Text))
            {
                emailhelper.ErrorText = "Please enter a valid email address";
                emailhelper.HasError = true;
                Vibration.Vibrate();
                EmailEntry.Focus();
                return;
            }

            //check if email is in the db

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var url = APICalls.Checkuseremail + "%27" + EmailEntry.Text + "%27";

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<APIUserResponse>(content);
                ObservableCollection<user> users = userResponse.Value;

                if (users.Count > 0)
                {
                    emailhelper.ErrorText = "Email address already in use";
                    emailhelper.HasError = true;
                    Vibration.Vibrate();
                    EmailEntry.Focus();
                    return;
                }

            }

            //Udpate Email in the Db 
            bool EmailChanged = false;
            string Email = EmailEntry.Text;
            string id = Helpers.Settings.UserKey;
            var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

            string json = System.Text.Json.JsonSerializer.Serialize(new { email = Email });
            StringContent contents = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                {
                    Content = contents
                };

                var responses = await client.SendAsync(request);

                if (!responses.IsSuccessStatusCode)
                {
                    var errorResponse = await responses.Content.ReadAsStringAsync();
                }
                else
                {
                    EmailChanged = true;
                }
            }
            if (EmailChanged == true)
            {
                Helpers.Settings.Email = Email;
                AllUserData.email = Email;
                await MopupService.Instance.PushAsync(new PopupPageHelper("Email Updated") { });
                await Task.Delay(1500);

                await Navigation.PushAsync(new ProfileSection(AllUserData), false);

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
                if (pageToRemoves != null)
                {

                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

                await MopupService.Instance.PopAllAsync(false);
            }

        }
        catch (Exception ex)
        {
        }

    }

    async void handleDateofBirth()
    {
        try
        {
            if (validdob == true)
            {
                //Udpate DateofBirth in the Db 
                bool DateofBirth = false;
                string DOB = DateofBirthEntry.Text;
                string id = Helpers.Settings.UserKey;
                var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { dateofbirth = DOB });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        DateofBirth = true;
                    }
                }
                if (DateofBirth == true)
                {
                    Helpers.Settings.Age = DOB;
                    AllUserData.dateofbirth = DOB; 

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Date of Birth Updated") { });
                    await Task.Delay(1500);

                    await Navigation.PushAsync(new ProfileSection(AllUserData), false);

                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
                    if (pageToRemoves != null)
                    {

                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);

                    await MopupService.Instance.PopAllAsync(false);
                }
            }

        }
        catch (Exception Ex)
        {
        }
    }

    async private void handleGender() 
    {
        try
        {
            //Udpate DateofBirth in the Db 
            bool GenderChanged = false;
            string Gender = genderlist.SelectedItem.ToString();
            string id = Helpers.Settings.UserKey;
            var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

            string json = System.Text.Json.JsonSerializer.Serialize(new { gender = Gender });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                {
                    Content = content
                };

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    GenderChanged = true;
                }
            }
            if (GenderChanged == true)
            {
                Helpers.Settings.Gender = Gender;
                AllUserData.gender = Gender; 

                await MopupService.Instance.PushAsync(new PopupPageHelper("Gender Updated") { });
                await Task.Delay(1500);

                await Navigation.PushAsync(new ProfileSection(AllUserData), false);

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
                if (pageToRemoves != null)
                {

                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

                await MopupService.Instance.PopAllAsync(false);
            }
        }
        catch(Exception Ex)
        {

        }
    }

    async private void handleEthnicity()
    {
        try
        {
            //Udpate DateofBirth in the Db 
            bool EthnicityChanged = false;
            string Ethnicity = ethlist.SelectedItem.ToString();
            string id = Helpers.Settings.UserKey;
            var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

            string json = System.Text.Json.JsonSerializer.Serialize(new { ethnicity = Ethnicity });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                {
                    Content = content
                };

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    EthnicityChanged = true;
                }
            }
            if (EthnicityChanged == true)
            {
                Helpers.Settings.Ethnicity = Ethnicity;
                AllUserData.ethnicity = Ethnicity; 

                await MopupService.Instance.PushAsync(new PopupPageHelper("Ethnicity Updated") { });
                await Task.Delay(1500);

                await Navigation.PushAsync(new ProfileSection(AllUserData), false);

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
                if (pageToRemoves != null)
                {

                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

                await MopupService.Instance.PopAllAsync(false);

            }
        }
        catch (Exception Ex)
        {

        }
    }

    private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {

        }
    }

    private void DateofBirthEntry_TextChanged(object sender, TextChangedEventArgs e)
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
                DateofBirthEntry.IsEnabled = false;
                DateofBirthEntry.IsEnabled = true;
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
                        DateofBirthEntry.TextColor = Color.FromArgb("#031926"); // Valid date
                        validdob = true;
                    }
                    else
                    {
                        DateofBirthEntry.TextColor = Colors.Red; // Invalid date range
                        validdob = false;
                    }
                }
                else
                {
                    DateofBirthEntry.TextColor = Colors.Red; // Invalid date
                    validdob = false;
                }
            }
            else
            {
                DateofBirthEntry.TextColor = Color.FromArgb("#031926"); // Intermediate input
                validdob = false;
            }

            DateofBirthEntry.Text = input;

            // Adjust cursor position
            DateofBirthEntry.CursorPosition = input.Length;

            isEditing = false;
        }
        catch (Exception ex)
        {

        }
    }

    private void genderlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {

        }
    }

    private void ethlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {

        }
    }

    async private void HealthDetailsUpdate_Clicked(object sender, EventArgs e)
    {
        try
        {            
            if (NameStack.IsVisible == true)
            {
                handleNameframe(); 
            }
            else if (EmailStack.IsVisible == true) 
            {
                Handleemailframe();
            }
            else if (DateofBirthStack.IsVisible == true)
            {
                handleDateofBirth();
            }
            else if (GenderStack.IsVisible == true)
            {
                handleGender(); 
            }
            else if (EthnicityStack.IsVisible == true)
            {
                handleEthnicity(); 
            }


        }
        catch (Exception Ex)
        {

        }
    }

    private void CurrentPassword_TextChanged(object sender, TextChangedEventArgs e)
    {
        //Must Match Original 
        try
        {
           
        }
        catch(Exception Ex)
        {

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
        catch (Exception Ex)
        {
        }
    }

    private void confirmpassentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {
        }
    }

    async private void SettingsUpdate_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (EmailConfirmStack.IsVisible == true)
            {
                string Password = await Encryption.GetHashAsHex(InitialPasswordEntry.Text);
                if (Password == AllUserData.password)
                {
                    EmailConfirmStack.IsVisible = false;
                    PasswordStack.IsVisible = true;
                    SettingsUpdate.Text = "Reset Password";
                }
                else
                {
                    InitalPassword.HasError = true;
                    InitalPassword.ErrorText = "Password Incorrect, Try Again";
                    Vibration.Vibrate();
                    InitialPasswordEntry.Focus();
                    return;
                }

            }

            if (PasswordStack.IsVisible == true)
            {
                handlePasswordframe();
            }
            else if (NotificationsStack.IsVisible == true)
            {

            }
            else if (SignupcodeStack.IsVisible == true)
            {
                handlesignupcodeframe(); 
            }

        }
        catch (Exception Ex)
        {
        }
    }

    //async private void handleConfirmcode()
    //{
    //    try
    //    {
    //        //Check Code Matches 
    //        if (string.IsNullOrEmpty(emailconfigpin.PINValue))
    //        {
    //            // incorrectcodelbl.IsVisible = true;
    //            emailconfigpin.Focus();
    //            Vibration.Vibrate();
    //            return;
    //        }

    //        if (emailconfigpin.PINValue == ValidationCode)
    //        {
    //            EmailConfirmStack.IsVisible = false;
    //            PasswordStack.IsVisible = true;
    //            SettingsUpdate.IsVisible = true; 
    //            SettingsUpdate.Text = "Reset Password";
    //        }
    //        else
    //        {
    //            incorrectcodelbl.IsVisible = true;
    //            Vibration.Vibrate();
    //            emailconfigpin.PINValue = null; 
    //            return;
    //        }

    //    }
    //    catch (Exception Ex) 
    //    { 
    //    }

    //}

   async private void EmailReset_Clicked(object sender, EventArgs e)
    {
        try
        {
            await DisplayAlert("Email Reset", "A Password Reset Email has been sent to the registered Email Address", "Close");
        }
        catch (Exception Ex)
        {
           
        }
    }

    async private void handlePasswordframe()
    {
        try
        {
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

            //check confirm password
            if (firstpasswordentry.Text != confirmpassentry.Text)
            {
                confirmpasshelper.HasError = true;
                confirmpasshelper.ErrorText = "Passwords do not match";
                Vibration.Vibrate();
                confirmpassentry.Focus();
                return;

            }


            string Password = await Encryption.GetHashAsHex(firstpasswordentry.Text);
            string passwordtocompare = Helpers.Settings.UserPasswordHash;

            if (passwordtocompare == Password)
            {
                //Password cannot be the same as your last one 

                confirmpasshelper.HasError = true;
                confirmpasshelper.ErrorText = "New Password must be Different from previous";
                Vibration.Vibrate();
                confirmpassentry.Focus();
                return;
            }
            else
            {
                //Reset Pasword 
                bool passwordSuccess = false; 
                string id = Helpers.Settings.UserKey;
                var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { password = Password });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        passwordSuccess = true; 
                    }

                }

                if (passwordSuccess == true)
                {
                    //Update Password Helpers.settings 
                    //

                    Helpers.Settings.Password = Password;
                    Helpers.Settings.UserPasswordHash = Password;
                    AllUserData.password = Password;

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Password Updated") { });
                    await Task.Delay(1500);

                    await Navigation.PushAsync(new ProfileSection(AllUserData), false);

                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
                    if (pageToRemoves != null)
                    {

                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);

                    await MopupService.Instance.PopAllAsync(false);
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }

   

    async private void handlesignupcodeframe()
    {
        try
        {
            string signupcode = signupcodetext.Text;

            //Check if signup code valid 

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
                    return;
                }
                else
                {
                    //update Signup code 
                    //check if they have any questions and anwers


                    var urll = APICalls.Checksignupregquestions + "%27" + users[0].referral + "%27";

                    HttpResponseMessage responsee = await client.GetAsync(urll);

                    if (responsee.IsSuccessStatusCode)
                    {
                        string contentt = await responsee.Content.ReadAsStringAsync();
                        var userResponsee = JsonConvert.DeserializeObject<ApiResponseQuestion>(contentt);
                        ObservableCollection<question> questions = userResponsee.Value;

                        if (questions.Count > 0)
                        {

                            foreach (var item in questions)
                            {
                                if (item.area == "Registration")
                                {

                                    //regquestionlist.Add(item);

                                }
                            }

                            //get answers for the questions


                            var urlanswers = APICalls.Checksignupreganswers + "%27" + users[0].referral + "%27";

                            HttpResponseMessage responseeanswers = await client.GetAsync(urlanswers);

                            if (responseeanswers.IsSuccessStatusCode)
                            {
                                string contenttanswers = await responseeanswers.Content.ReadAsStringAsync();
                                var userResponseeanswer = JsonConvert.DeserializeObject<ApiResponseAnswer>(contenttanswers);
                                //reganswerlist = userResponseeanswer.Value;
                            }
                        }
                    }

                    //check if there is additional consent
                    if (users[0].registrationconsent == true)
                    {
                        var urlconsent = APICalls.CheckConsentforsignupcode + "%27" + users[0].signupcodeid + "%27";

                        HttpResponseMessage responseconsent = await client.GetAsync(urlconsent);

                        if (responseconsent.IsSuccessStatusCode)
                        {
                            string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                            var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseConsent>(contentconsent);
                            var consent = userResponseconsent.Value;

                            //additionalconsent = consent.Where(x => x.area == "Registration").SingleOrDefault();

                        }

                    }
                }

            }

        }
        catch (Exception Ex)
        {

        }
    }

   async private void signupcodetext_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {

        }
    }

   //async private void emailconfigpin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
   // {
   //     try
   //     {
   //         handleConfirmcode();
   //     }
   //     catch (Exception Ex)
   //     {

   //     }
   // }

    private void emailconfigpin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        try
        {
            
        }
        catch (Exception Ex)
        {

        }     
    }

   async private void PrivacyUpdate_Clicked(object sender, EventArgs e)
    {
        try
        {
            if(PinCodeStack.IsVisible == true)
            {
                handlePincode();
            }

        }
        catch (Exception Ex)
        {

        }
    }


    async private void handlePincode()
    {
        try
        {
            if (string.IsNullOrEmpty(codepin.PINValue) || string.IsNullOrEmpty(confirmcodepin.PINValue))
            {
                Pinincorrectlbl.Text = "Please enter a PIN";
                Pinincorrectlbl.IsVisible = true;
                Vibration.Vibrate();
                return;
            }

            if (codepin.PINValue == confirmcodepin.PINValue)
            {
                //Udpate DateofBirth in the Db 
                bool PinChanged = false;
                string PinCode = codepin.PINValue;
                string id = Helpers.Settings.UserKey;
                var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { userpin = PinCode });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        PinChanged = true;
                    }
                }
                if (PinChanged == true)
                {
                    Helpers.Settings.PinCode = PinCode;
                    AllUserData.userpin = PinCode;

                    Preferences.Set("pincode", PinCode);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Pin Code Updated") { });
                    await Task.Delay(1500);

                    await Navigation.PushAsync(new ProfileSection(AllUserData), false);

                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is ProfileSection);
                    if (pageToRemoves != null)
                    {

                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);

                    await MopupService.Instance.PopAllAsync(false);

                }
            }
            else
            {
                Pinincorrectlbl.Text = "PINs does not match";
                Pinincorrectlbl.IsVisible = true;
                Vibration.Vibrate();
                return;
            }
            
        }
        catch (Exception Ex)
        {

        }
    }

    private void CurrentPin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {
        try
        {

            if (AllUserData.userpin == CurrentPin.PINValue)
            {
                CurrentPinCodeStack.IsVisible = false;
                PinCodeStack.IsVisible = true;
                PrivacyUpdate.IsVisible = true;
                PrivacyStacklbl.Text = "Enter New Pin";

                CurrentPin.Unfocus();
                CurrentPin.AutoDismissKeyboard = true;

            }
            else
            {
                CurrentPinErrorlbl.IsVisible = true;
                CurrentPin.PINValue = null; 
                Vibration.Vibrate();
                CurrentPin.Focus();
                return;
            }
        }
        catch (Exception Ex)
        {

        }
    }

    private void CurrentPin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }

    private void codepin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {

    }

    private void codepin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }

    private void confirmcodepin_PINEntryCompleted(object sender, PINView.Maui.Helpers.PINCompletedEventArgs e)
    {

    }

    private void confirmcodepin_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {

    }

    async private void emailconfigpin_Focused(object sender, FocusEventArgs e)
    {
   
    }

    async private void PinSwitch_Toggled(object sender, ToggledEventArgs e)
    {
            try
            {
            //false Sets it to null 
            if (e.Value == false)
            {
                string id = Helpers.Settings.UserKey;
                var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";
    
                string json = System.Text.Json.JsonSerializer.Serialize(new { userpin = (string)null });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                    {
                        Content = content
                    };

                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        //Remove Pin Code 
                        Preferences.Set("pincode", string.Empty);
                    }
                }
             
            }
            else
            {
                //Set to true (Reset PinCode) 
            }
           
            }
            catch (Exception Ex)
            {

            }

    }

    async private void FingerSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            bool BiometicsBool = e.Value; 
            string id = Helpers.Settings.UserKey;
            var URL = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            string json = System.Text.Json.JsonSerializer.Serialize(new { biometrics = BiometicsBool });
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Patch, URL)
                {
                    Content = content
                };

                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                }
                else
                {
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
}