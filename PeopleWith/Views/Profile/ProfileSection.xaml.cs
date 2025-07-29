using Mopups.Services;
using System.Collections.ObjectModel;
using System.Text;
using Plugin.LocalNotification;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Networking;
using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel;
using System.Reflection.Metadata.Ecma335;
using Microsoft.Extensions.Azure;
using Azure.Storage.Blobs.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace PeopleWith;

public partial class ProfileSection : ContentPage
{
    public List<user> UserDetailsList = new List<user>();
    public List<user> SettingsList = new List<user>();
    public List<user> PrivacyDetailsList = new List<user>();
    public ObservableCollection<user> UserData = new ObservableCollection<user>(); 
    user AllUserData = new user();
    string one;
    string two;
    private bool settingsOpened = false;
    private static readonly HttpClient Client = new HttpClient();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    AlertContent InitialQuestion;
    AlertContent SecondQuestion;
    private byte[] ResizedImage;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    bool isrunning = false; 

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
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
            if (!Client.DefaultRequestHeaders.Contains("X-MS-CLIENT-PRINCIPAL"))
            {
                Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
                Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
            }
        }
        catch (Exception Ex)
        {
            //Empty
        }
    }


    public class AlertContent
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Accept { get; set; }
        public string Cancel { get; set; }
    }

    //protected override async void OnAppearing()
    //{
    //    try
    //    {
    //        base.OnAppearing();
    //        //Check if Notifications has been updated
    //        GetSettings();

    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}


    //public ProfileSection()
    //{
    //	InitializeComponent();
    //}

    public ProfileSection()
    {
        try
        {
            InitializeComponent();
            // AllUserData = AllUserDetails;
            //Icon and user's Name 
            GetAllUserData();

          
            //Namelbl.Text = Helpers.Settings.FirstName + " " + Helpers.Settings.Surname;
            //if(Namelbl.Text == " ")
            //{
            //    Namelbl.Text = "Health Details"; 
            //}
            if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                //Royal Brompton (Project) 
                if (Helpers.Settings.SignUp.Contains("RBHTHCM"))
                {
                    DeleteAccount.Text = "Withdraw from project";

                    var ItemOne = new AlertContent
                    {
                        Title = "Withdraw from Project",
                        Message = "Are you sure you want to withdraw from this project?",
                        Accept = "Yes, Withdraw",
                        Cancel = "Cancel"
                    };

                    var ItemTwo = new AlertContent
                    {
                        Title = "This Action is Permanent",
                        Message = "Once you withdraw from the project, your participant will end, and your associated data may be removed in alignment to the project consent given when registering. This action cannot be reversed and your account cannot be recovered.\n\nDo you really wish to proceed?",
                        Accept = "Withdraw Permanently",
                        Cancel = "Cancel"
                    };

                    InitialQuestion = ItemOne;
                    SecondQuestion = ItemTwo;

                    DeleteDetails.Text = "Once you withdraw from the project, your participation and associated data cannot be retrieved. Ensure you no longer wish to take part before proceeding with withdrawal.";

                }
                else
                {
                    //All Other Signup Codes 
                    DeleteAccount.Text = "Withdraw from study";

                    var ItemOne = new AlertContent
                    {
                        Title = "Withdraw from Study",
                        Message = "Are you sure you want to withdraw from this study?",
                        Accept = "Yes, Withdraw",
                        Cancel = "Cancel"
                    };

                    var ItemTwo = new AlertContent
                    {
                        Title = "This Action is Permanent",
                        Message = "Once you withdraw from the study, your participant will end, and your associated data may be removed in alignment to the study consent given when registering. This action cannot be reversed and your account cannot be recovered.\n\nDo you really wish to proceed?",
                        Accept = "Withdraw Permanently",
                        Cancel = "Cancel"
                    };

                    InitialQuestion = ItemOne;
                    SecondQuestion = ItemTwo;

                    DeleteDetails.Text = "Once you withdraw from the study, your participation and associated data cannot be retrieved. Ensure you no longer wish to take part before proceeding with withdrawal.";
                }
            }
            else
            {
                var ItemOne = new AlertContent
                {
                    Title = "Delete Account",
                    Message = "Are you sure you want to delete this account?",
                    Accept = "Yes, Delete",
                    Cancel = "Cancel"
                };

                var ItemTwo = new AlertContent
                {
                    Title = "This Action is Permanent",
                    Message = "Once your account is deleted, it cannot be recovered.\n\nDo you really wish to proceed?",
                    Accept = "Delete Permanently",
                    Cancel = "Cancel"
                };

                InitialQuestion = ItemOne;
                SecondQuestion = ItemTwo;
            }

            Useridlbl.Text = Helpers.Settings.UserKey;

            string version = AppInfo.Current.VersionString;
            string build = AppInfo.Current.BuildString;
            ReleaseVersion.Text = "ReleaseVersion " + version + " | " + "Build Version " + build;

         

            GetHealthDetails();
            GetSettings();
            GetPrivacyDetails();

            //Set Profile Pic/ Initals 
           

            MessagingCenter.Subscribe<App>(this, "CallMethodOnPage", (sender) =>
            {
                GetSettings();
            });
            WeakReferenceMessenger.Default.Register<ProfilePicUpdate>(this, (r, m) =>
            {
                var ImageString = (String)m.Value;
                Initals.IsVisible = false;
                EditShow.IsVisible = false;
                ProfilePic.IsVisible = true;
                Camerapng.IsVisible = true; 
                var FullString = $"https://peoplewithappiamges.blob.core.windows.net/profileuploads/{ImageString}?t={DateTime.UtcNow.Ticks}";
                ProfilePic.Source = ImageSource.FromUri(new Uri(FullString));
            });
        }

        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

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

            string version = AppInfo.Current.VersionString;
            string build = AppInfo.Current.BuildString;
            ReleaseVersion.Text = "ReleaseVersion " + version + " | " + "Build Version " + build;  

            GetHealthDetails();
            GetSettings();
            GetPrivacyDetails();

            MessagingCenter.Subscribe<App>(this, "CallMethodOnPage", (sender) => {
                GetSettings();
            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void GetAllUserData()
    {
        try
        {
            await GetUserData();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async Task GetUserData()
    {
        try
        {
            APICalls Database = new APICalls();
            UserData = await Database.GetuserDetails();
            AllUserData.password = UserData[0].password;
            AllUserData.firstname = UserData[0].firstname;
            AllUserData.surname = UserData[0].surname;
            AllUserData.email = UserData[0].email;
            AllUserData.dateofbirth = UserData[0].dateofbirth;
            AllUserData.gender = UserData[0].gender;
            AllUserData.ethnicity = UserData[0].ethnicity;
            AllUserData.biometrics = UserData[0].biometrics;
            AllUserData.userpin = UserData[0].userpin;
            AllUserData.profilepicture = UserData[0].profilepicture;

            if (String.IsNullOrEmpty(AllUserData.profilepicture))
            {
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
                if (!string.IsNullOrEmpty(one) && !string.IsNullOrEmpty(two))
                {
                    var NameInt = one + two;
                    Initals.Text = NameInt.ToUpper();
                }

                if (string.IsNullOrEmpty(Initals.Text))
                {
                    Initals.Text = "PW";
                }
            }
            else
            {
                Initals.IsVisible = false;
                EditShow.IsVisible = false;
                ProfilePic.IsVisible = true;
                Camerapng.IsVisible = true;
                var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/profileuploads/{AllUserData.profilepicture}?t={DateTime.UtcNow.Ticks}";
                ProfilePic.Source = ImageSource.FromUri(new Uri(imagestring));
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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
            if (Helpers.Settings.Age.Contains(Helpers.Settings.DOB))
            {
                var n = Helpers.Settings.Age;
                var nn = n.Replace("00:00:00", string.Empty);
                DOB.SettingsItem = nn;
            }
            else
            {
                if (string.IsNullOrEmpty(Helpers.Settings.Age) || Helpers.Settings.Age == "")
                {
                    DOB.SettingsItem = "--";
                }
                else
                {
                    DateTime CheckDOB = new DateTime();
                    if (Helpers.Settings.DOB != null)
                    {
                        CheckDOB = DateTime.Parse(Helpers.Settings.Age);
                    }
                    if (CheckDOB.Date.ToString("dd/MM/yyyy") == "01/01/1900")
                    {
                        DOB.SettingsItem = "--";
                    }
                    else
                    {
                        DOB.SettingsItem = Helpers.Settings.DOB;
                    }
                }
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
            if (string.IsNullOrEmpty(Helpers.Settings.Ethnicity))
            {
                Ethnicity.SettingsItem = "--";
            }
            else
            {
                Ethnicity.SettingsItem = Helpers.Settings.Ethnicity;
            }
           

            UserDetailsList.Add(Ethnicity);

            UserDetails.ItemsSource = UserDetailsList;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

   async private void GetSettings()
    {
        try
        {
            Settings.ItemsSource = null;
            SettingsList.Clear();

            //Password
            var Password = new user();
            Password.SettingsTitle = "Reset Password";

            Password.SettingsItem = "*********";

            SettingsList.Add(Password);


            //Notifications
            var Notification = new user();
            Notification.SettingsTitle = "Notifications";

            //Notification.SettingsItem = Helpers.Settings.Notifications;

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
                if (status == PermissionStatus.Granted)
                {
                    Notification.SettingsItem = "Enabled";
                }
                else
                {
                    Notification.SettingsItem = "Disabled";
                }
            }
            else if(DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                //var notificationService = DependencyService.Get<INotificationService>();
                //bool notificationsEnabled = await notificationService.CheckRequestNotificationPermissionAsync();
                //Notification.SettingsItem = notificationsEnabled ? "Enabled" : "Disabled";

                var check = await LocalNotificationCenter.Current.AreNotificationsEnabled();

                if (!check)
                {
                    Notification.SettingsItem = "Disabled";
                }
                else
                {
                    Notification.SettingsItem = "Enabled";
                }

                //if (notificationsEnabled)
                //{
                //    Notification.SettingsItem = "Enabled";
                //}
                //else
                //{
                //    Notification.SettingsItem = "Disabled";
                //}

            }

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

            //SettingsList.Add(Signup);

            Settings.ItemsSource = SettingsList;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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

            //PrivacyDetailsList.Add(Permission);

            PrivacyList.ItemsSource = PrivacyDetailsList;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

   async private void UserDetails_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (isrunning) return;

                isrunning = true; 
                var ItemTapped = e.DataItem as user;
                var SelectedItem = ItemTapped.SettingsTitle;
                string Selected = "Health Details";
                await Navigation.PushAsync(new ProfileEdit(SelectedItem, Selected, AllUserData), false);
                isrunning = false; 
                //UserDetails.SelectedItem = null;
                // UserDetails.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex) 
        {
            UserDetails.IsEnabled = true;
            NotasyncMethod(Ex);
        }
    }

   async private void Settings_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (isrunning) return;

                isrunning = true;
                var ItemTapped = e.DataItem as user;
                var SelectedItem = ItemTapped.SettingsTitle;
                if (SelectedItem == "Notifications")
                {
                    if(ItemTapped.SettingsItem == "Enabled")
                    {
                        //Do Nothing 
                    }
                    else
                    {


                        // Open phone settings
                        AppInfo.ShowSettingsUI();
                        //GetSettings(); 

                        //if (DeviceInfo.Platform == DevicePlatform.Android)
                        //{

                        //    //await Permissions.RequestAsync<Permissions.PostNotifications>();
                        //    //if(That didnt work Do this) - Needs changed to open Notifications page- Then same for android 
                        //    var context = Android.App.Application.Context;

                        //    var intent = new Android.Content.Intent(Android.Provider.Settings.ActionAppNotificationSettings);
                        //    intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, context.PackageName);
                        //    intent.AddFlags(Android.Content.ActivityFlags.NewTask);

                        //    context.StartActivity(intent);
                        //}
                        //else
                        //{
                        //    var notificationService = DependencyService.Get<INotificationService>();
                        //    await notificationService.RequestNotificationPermissionAsync();

                        //}
                    }
                   
                }
                else
                {
                    string Selected = "Settings";
                    await Navigation.PushAsync(new ProfileEdit(SelectedItem, Selected, AllUserData), false);
                }
                isrunning = false;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            Settings.IsEnabled = true;
            NotasyncMethod(Ex);
        }
    }

    async private void PrivacyList_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (isrunning) return;

                isrunning = true;
                var ItemTapped = e.DataItem as user;
                var SelectedItem = ItemTapped.SettingsTitle;
                if (SelectedItem == "Terms of Use")
                {
                    await Navigation.PushAsync(new PrivacyPolicy(), false);
                }
                else if (SelectedItem == "About")
                {
                    //await Naivigaiton.PushAsync(new AboutPage(), flase);
                }
                else
                {
                    string Selected = "Privacy";
                    await Navigation.PushAsync(new ProfileEdit(SelectedItem, Selected, AllUserData), false);
                }

                isrunning = false;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            PrivacyList.IsEnabled = true;
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                if (ImproveSection.IsVisible == true)
                {
                    if (Email.Default.IsComposeSupported)
                    {

                        string subject = "";
                        string body = "Userid: " + Helpers.Settings.UserKey;
                        string[] recipients = new[] { "support@peoplewith.com" };

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

                //Add if CBD Signup Code Being Used 
                //if (CBDOrderSection.IsVisible == true)
                //{

                //}
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

    async private void Logout_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                Logout.IsEnabled = false; 
                //Limit No. of Taps 
                bool Answer = await DisplayAlert("Logout", "Are you sure you want to logout", "Logout", "Cancel");
                if (Answer)
                {
                    Logout HandleLogout = new Logout();
                }
                else
                {
                    Logout.IsEnabled = true; 
                    //Do Nothing
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
 
    async private void DeleteAccount_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                DeleteAccount.IsEnabled = false;

                bool Answer = await DisplayAlert(InitialQuestion.Title, InitialQuestion.Message, InitialQuestion.Accept, InitialQuestion.Cancel);

                //bool Answer = await DisplayAlert("Delete Account", "Are you sure you want to delete this Account? Once deleted it cannot be retrieved", "Delete Account", "Cancel");
                if (Answer)
                {

                    bool Confirm = await DisplayAlert(SecondQuestion.Title, SecondQuestion.Message, SecondQuestion.Accept, SecondQuestion.Cancel);

                    if (Confirm) 
                    {
                        //Delete Account
                        bool delete = true;
                        bool Success = false;
                        var regstatus = "Active";
                        var TimeDate = string.Empty;
                        string id = Helpers.Settings.UserKey;
                        var url = $"https://pwapi.peoplewith.com/api/user/userid/{id}";

                        if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
                        {
                            delete = false; 
                            regstatus = "Withdrawn";
                            TimeDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = delete, registrationstatus = regstatus, activationtimestamp = TimeDate });
                        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                        ConfigureClient();
                        using (var client = new HttpClient())
                        {
                            var request = new HttpRequestMessage(HttpMethod.Patch, url)
                            {
                                Content = content
                            };

                            var response = await Client.SendAsync(request);

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

                            //Remove The Following Novo Preferences if Neccesary 
                            if (!String.IsNullOrEmpty(Helpers.Settings.SignUp))
                            {
                                var signup = Helpers.Settings.SignUp;
                                if (signup.Contains("SAX"))
                                {   //Remove the Following 
                                    Preferences.Default.Remove("NovoMeds");
                                    Preferences.Default.Remove("NovoSyms");
                                    Preferences.Default.Remove("NovoSupps");
                                    Preferences.Default.Remove("NovoMeas");
                                    Preferences.Default.Remove("NovoDiag");
                                    Preferences.Default.Remove("NovoMood");
                                    Preferences.Default.Remove("NovoAppt");
                                    Preferences.Default.Remove("NovoHcp");
                                    Preferences.Default.Remove("NovoQues");
                                    Preferences.Default.Remove("NovoAllerg");
                                    Preferences.Default.Remove("NovoHeRep");
                                    Preferences.Default.Remove("NovoSched");
                                    Preferences.Default.Remove("NovoFood");
                                    Preferences.Default.Remove("NovoDiet");
                                    Preferences.Default.Remove("NovoInvest");
                                    Preferences.Default.Remove("NovoActivity");

                                }

                                if (signup.Contains("RBHTHCM"))
                                {
                                    await MopupService.Instance.PushAsync(new PopupPageHelper("Withdrawn from Project") { });
                                }
                                else
                                {
                                    await MopupService.Instance.PushAsync(new PopupPageHelper("Withdrawn from Study") { });
                                }
                            }
                            else
                            {
                                await MopupService.Instance.PushAsync(new PopupPageHelper("Account Deleted") { });
                            }

                            await Task.Delay(1500);
                            //Logout of Account 
                            Logout HandleLogout = new Logout();
                            await MopupService.Instance.PopAllAsync(false);
                        }
                    }
                    else
                    {
                        DeleteAccount.IsEnabled = true;
                        return; 
                    }                
                }
                else
                {
                    DeleteAccount.IsEnabled = true;
                    return; 
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

    private async void ProfileIConClicked(object sender, TappedEventArgs e)
    {
        try
        {
            if (ProfilePic.IsVisible == true)
            {
                await Navigation.PushAsync(new TakeProfilePicture(AllUserData.profilepicture), false);
            }
            else
            {
                await Navigation.PushAsync(new TakeProfilePicture(), false);
            }
        }
        catch (Exception Ex)
        {

        }
 
    }

    public async Task<FileResult> CapturePhotoAsync()
    {
        try
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permission Required", "Camera access is required to take add a Profile Picture", "Close");
                    return null;
                }
            }

            if (MediaPicker.Default.IsCaptureSupported)
            {
                return await MediaPicker.Default.CapturePhotoAsync();
            }

            return null;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return null;
        }
    }

}