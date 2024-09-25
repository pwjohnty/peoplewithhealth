using Mopups.Services;
using System.Text;

namespace PeopleWith;

public partial class ProfileSection : ContentPage
{
    public List<user> UserDetailsList = new List<user>();
    public List<user> SettingsList = new List<user>();
    public List<user> PrivacyDetailsList = new List<user>();
    user AllUserData = new user(); 
    string one;
    string two; 

    //public ProfileSection()
    //{
    //	InitializeComponent();
    //}

    public ProfileSection(user AllUserDetails)
    {
        try
        {
            InitializeComponent();
            AllUserData = AllUserDetails; 
            //Icon and user's Name 
            var FirstName = Helpers.Settings.FirstName;
            var Surname = Helpers.Settings.Surname;
            if (!string.IsNullOrEmpty(FirstName))
            {
                 one = FirstName.Substring(0, 1);
            }
    
            if (!string.IsNullOrEmpty(Surname))
            {
                 two = Surname.Substring(0, 1);
            }
            if(!string.IsNullOrEmpty(one) && !string.IsNullOrEmpty(two))
            {
                Initals.Text = one + two;
            }

            if (string.IsNullOrEmpty(Initals.Text))
            {
                Initals.Text = "PW"; 
            }
            //Namelbl.Text = Helpers.Settings.FirstName + " " + Helpers.Settings.Surname;
            //if(Namelbl.Text == " ")
            //{
            //    Namelbl.Text = "Health Details"; 
            //}
            Useridlbl.Text = Helpers.Settings.UserKey;
            //Needs Changed Placeholder for now 
            ReleaseVersion.Text = "ReleaseVersion " + "80" + " | " + "Build Version " + "80.1";  

            GetHealthDetails();
            GetSettings();
            GetPrivacyDetails();


        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }
    private void GetHealthDetails()
    {
        try
        {
            //Name 
            var Name = new user();
            Name.SettingsTitle = "Name";
            var fullname = Helpers.Settings.FirstName + " " + Helpers.Settings.Surname;
            if (fullname == " ")
            {
                Name.SettingsItem = "--";
            }
            else
            {
                Name.SettingsItem = fullname;
            }

            UserDetailsList.Add(Name);

            //Email
            var Email = new user();
            Email.SettingsTitle = "Email";
            Email.SettingsItem = Helpers.Settings.Email;

            UserDetailsList.Add(Email);

            //Date of Birth
            var DOB = new user();
            DOB.SettingsTitle = "Date of Birth";
            if (Helpers.Settings.Age.Contains("00:00:00"))
            {
                var n = Helpers.Settings.Age;
                var nn = n.Replace("00:00:00", string.Empty);
                DOB.SettingsItem = nn;
            }
            else
            {
                DOB.SettingsItem = Helpers.Settings.Age;
            }
            UserDetailsList.Add(DOB);

            //Gender
            var Gender = new user();
            Gender.SettingsTitle = "Gender";
            if (string.IsNullOrEmpty(Helpers.Settings.Gender))
            {
                Gender.SettingsItem = "--";
            }
            else
            {
                Gender.SettingsItem = Helpers.Settings.Gender;
            }
           
            UserDetailsList.Add(Gender);

            //Ethnicity
            var Ethnicity = new user();
            Ethnicity.SettingsTitle = "Ethnicity";
            Ethnicity.SettingsItem = Helpers.Settings.Ethnicity;

            UserDetailsList.Add(Ethnicity);

            UserDetails.ItemsSource = UserDetailsList;
        }
        catch (Exception Ex)
        {
            //Add Crash log 
        }
    }


    private void GetSettings()
    {
        try
        {
            //Password
            var Password = new user();
            Password.SettingsTitle = "Reset Password";

            Password.SettingsItem = "*********";

            SettingsList.Add(Password);


            //Notifications
            var Notification = new user();
            Notification.SettingsTitle = "Notifications";

            Notification.SettingsItem = "ON";

            SettingsList.Add(Notification);

            //SignupCode

            var Signup = new user();
            Signup.SettingsTitle = "Signup Code";
            var signupcode = Helpers.Settings.SignUp;
            if (string.IsNullOrEmpty(signupcode))
            {
                Signup.SettingsItem = "--";
            }
            else
            {
                Signup.SettingsItem = Helpers.Settings.SignUp;
            }

            SettingsList.Add(Signup);

            Settings.ItemsSource = SettingsList;
        }
        catch (Exception Ex)
        {

        }
    }

    private void GetPrivacyDetails()
    {
        try
        {
            //Terms of Use
            var Terms = new user();
            Terms.SettingsTitle = "Terms of Use";

            PrivacyDetailsList.Add(Terms);

            ////About
            //var About = new user();
            //About.SettingsTitle = "About";

            //PrivacyDetailsList.Add(About);

            //Pin
            var Pin = new user();
            Pin.SettingsTitle = "Reset Pin";

            PrivacyDetailsList.Add(Pin);

            //Permissions
            var Permission = new user();
            Permission.SettingsTitle = "Permissions";

            PrivacyDetailsList.Add(Permission);

            PrivacyList.ItemsSource = PrivacyDetailsList;
        }
        catch (Exception Ex)
        {

        }
    }

   async private void UserDetails_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var ItemTapped = e.DataItem as user;
            var SelectedItem = ItemTapped.SettingsTitle;
            string Selected = "Health Details"; 
            await Navigation.PushAsync(new ProfileEdit(SelectedItem, Selected, AllUserData), false); 
        }
        catch (Exception Ex) 
        {
        }
    }

   async private void Settings_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var ItemTapped = e.DataItem as user;
            var SelectedItem = ItemTapped.SettingsTitle;
            string Selected = "Settings";
            await Navigation.PushAsync(new ProfileEdit(SelectedItem, Selected, AllUserData), false);
        }
        catch (Exception Ex)
        {
        }
    }

    async private void PrivacyList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
          
            var ItemTapped = e.DataItem as user;
            var SelectedItem = ItemTapped.SettingsTitle;
            if(SelectedItem == "Terms of Use")
            {
                await Navigation.PushAsync(new PrivacyPolicy(), false); 
            }
            else if(SelectedItem == "About")
            {
                //await Naivigaiton.PushAsync(new AboutPage(), flase);
            }
            else
            {
                string Selected = "Privacy";
                await Navigation.PushAsync(new ProfileEdit(SelectedItem, Selected, AllUserData), false);
            }

        }
        catch (Exception Ex)
        {
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if(ImproveSection.IsVisible == true)
            {
                if (Email.Default.IsComposeSupported)
                {

                    string subject = "";
                    string body = "Userid: " + Helpers.Settings.UserKey ;
                    string[] recipients = new[] { "chris.johnston@peoplewith.com" };

                    var message = new EmailMessage
                    {
                        Subject = subject,
                        Body = body,
                        BodyFormat = EmailBodyFormat.PlainText,
                        To = new List<string>(recipients)
                    };

                    await Email.Default.ComposeAsync(message);
                }
            }
            if(CBDOrderSection.IsVisible == true)
            {

            }
        }
        catch (Exception Ex)
        {

        }
    }

 

    async private void Logout_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool Answer = await DisplayAlert("Logout", "Are you sure you would like to logout", "Accept", "Decline");
            if (Answer)
            {
                Logout HandleLogout = new Logout();
            }
            else
            {
                //Do Nothing
            }
               
        }
        catch (Exception Ex) 
        {
        }

    }

 
    async private void DeleteAccount_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool Answer = await DisplayAlert("Delete Account", "Are you sure you would like to Delete this account, Once Deleted it cannot be retrieved", "Accept", "Decline");
            if (Answer)
            {
                //Delete Account
                bool delete = true;
                bool Success = false; 
                string id = Helpers.Settings.UserKey;
                var url = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = delete });
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
                        Success = true;
                    }
                }


                    if (Success == true)
                    {
                        await MopupService.Instance.PushAsync(new PopupPageHelper("Account Deleted") { });
                        await Task.Delay(1500);
                        //Logout of Account 
                        Logout HandleLogout = new Logout();
                        await MopupService.Instance.PopAllAsync(false);
                    }
                }
            else
            {
                //Do Nothing
            }

        }
        catch (Exception Ex)
        {
        }
    }
}