//using System;
//using Android.Opengl;
//using Java.Time.Temporal;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using Microsoft.Azure.NotificationHubs;
using Mopups.Services;
using Plugin.LocalNotification;
using Syncfusion.Maui.Charts;
//using Sentry;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
using System.Reflection.Metadata;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Controls;
using Plugin.Maui.Health.Enums;
using Plugin.Maui.Health;
using System.Globalization;
using CommunityToolkit.Maui.Core.Extensions;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Messaging;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.Maui.DataSource.Extensions;
//using Xamarin.Google.Crypto.Tink.Subtle;

namespace PeopleWith;

public partial class MainDashboard : ContentPage
{
    ObservableCollection<user> UserDetails = new ObservableCollection<user>();
    consent NovoConsent = new consent();
    user AllUserDetails = new user();
    APICalls database = new APICalls();
    List<MedtimesDosages> ScheduleToday = new List<MedtimesDosages>();
    DateTime today = DateTime.Today;
    DateTime? nextDueTime = null;
    DateTime? nextDueTimeSupp = null;
    List<object> foryouuserlist = new List<object>();
    List<object> foryouuserlistsymptom = new List<object>();
    List<object> foryouuserlistmeds = new List<object>();
    List<object> foryouuserlistsupps = new List<object>();
    List<object> PopulateDashQuestions = new List<object>();
    ObservableCollection<userfeedback> userfeedbacklist = new ObservableCollection<userfeedback>();
    ObservableCollection<usermedication> AllUserMedications = new ObservableCollection<usermedication>();
    ObservableCollection<usersupplement> AllUserSupplements = new ObservableCollection<usersupplement>();
    public ObservableCollection<usersymptom> UserSymptomPassed = new ObservableCollection<usersymptom>();
    //DashQuestions Answers
    public ObservableCollection<registryData> DashQuestionAnswers = new ObservableCollection<registryData>();
    //DashQuestions Inputs
    public ObservableCollection<registryDataInputs> DashQuestionInputs = new ObservableCollection<registryDataInputs>();
    public ObservableCollection<IGrouping<string, registryDataInputs>> GroupedDashQuestions = new ObservableCollection<IGrouping<string, registryDataInputs>>();

    ObservableCollection<signupcode> signupcodecollection = new ObservableCollection<signupcode>();
    public ObservableCollection<questionnaire> questionnaires = new ObservableCollection<questionnaire>();

    //Update Null id's
    public ObservableCollection<usermeasurement> UserMeasurementUpdate = new ObservableCollection<usermeasurement>();
    public ObservableCollection<usersymptom> UserSymptomsUpdate = new ObservableCollection<usersymptom>();
    public ObservableCollection<usermood> UserMoodUpdate = new ObservableCollection<usermood>();

    bool setnotificationsfromlogin;
    bool IsNavigating = false;
    bool NoficiationsActive = false;
    bool BatterySaverOff = false;
    public bool MedNotifAdded = false;
    public bool SuppNotifAdded = false;
    ObservableCollection<SettingsOn> SettingstoShow = new ObservableCollection<SettingsOn>();
    MedSuppNotifications ScheduleNotifications = new MedSuppNotifications();
    public HttpClient Client = new HttpClient();
    public event EventHandler<bool> ConnectivityChanged;
    ObservableCollection<dashitem> dailytasklist = new ObservableCollection<dashitem>();
    // public TaskCompletionSource<bool> PageReady { get; set; } = new();
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    private readonly IHealth health;

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
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
            Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
        }
        catch (Exception Ex)
        {
            //Empty
        }
    }

    public class SettingsOn
    {
        public string img { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string guide { get; set; }

    }

    public MainDashboard()
    {
        try
        {
            InitializeComponent();

            health = HealthDataProvider.Default;
            //Get All user Details & Set Helpers.Settings
            Checkifuserhasmigrated();

            NavigationPage.SetHasNavigationBar(this, false);

            //string firstName = Preferences.Default.Get("userid", "Unknown");

            loadcatergories();

            //getuserfeedbackdata();

            checksignupinfo();

            MessagingCenter.Subscribe<App>(this, "CheckUserSettings", (sender) => { UserSettingsCheck(); });


            WeakReferenceMessenger.Default.Register<UpdateDashAnswers>(this, (r, m) =>
            {
                DashQuestionAnswers = m.Value;
            });


            //MessagingCenter.Subscribe<App>(this, "UpdateQuestionAnswers", (sender) => {

            //    DashQuestionAnswers =
            //});
            //MessagingCenter.Subscribe<App>(this, "CallNotifications", (sender) => { checknotifications(); });

            //MessagingCenter.Subscribe<App>(this, "CallBatterySaver", (sender) => { CheckbatterySaverON(); });


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

        //lbl.Text = firstName;
    }

    public MainDashboard(bool fromlogin)
    {
        try
        {

            InitializeComponent();

            setnotificationsfromlogin = fromlogin;

            //Get All user Details & Set Helpers.Settings
            Checkifuserhasmigrated();

            NavigationPage.SetHasNavigationBar(this, false);

            //string firstName = Preferences.Default.Get("userid", "Unknown");

            loadcatergories();

            // getuserfeedbackdata();

            checksignupinfo();

            MessagingCenter.Subscribe<App>(this, "CheckUserSettings", (sender) => { UserSettingsCheck(); });
            //MessagingCenter.Subscribe<App>(this, "CallNotifications", (sender) => { checknotifications(); });

            //MessagingCenter.Subscribe<App>(this, "CallBatterySaver", (sender) => { CheckbatterySaverON(); });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
        //lbl.Text = firstName;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                getuserfeedbackdata();
            });

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async void checksignupinfo()
    {
        try
        {
            if (string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                infotab.IsVisible = false;

                var getimagesource = new Uri("https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/PeopleWithApp-AppImage-TemplateNov24.png");

                maindashimage.Source = ImageSource.FromUri(getimagesource);

                return;
            }

            signupcodecollection = await database.GetUserSignUpCodeInfo(Helpers.Settings.SignUp);



            if (signupcodecollection.Count > 0)
            {

                var getimagesource = new Uri("https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + signupcodecollection[0].dashboardimage);

                maindashimage.Source = ImageSource.FromUri(getimagesource);
                maindashimage2.Source = ImageSource.FromUri(getimagesource);


                signuptitlelbl.Text = signupcodecollection[0].title;

                if (signupcodecollection[0].appdescription.Length > 600)
                {
                    signupcodecollection[0].shortdescription = signupcodecollection[0].appdescription.Substring(0, 600);
                    signupdetailslbl.Text = signupcodecollection[0].shortdescription + "...";
                }
                else
                {
                    signupdetailslbl.Text = signupcodecollection[0].appdescription;
                }



                foreach (var item in signupcodecollection[0].signupcodeinfolist)
                {
                    if (item.type == "pdf")
                    {
                        item.img = "pdficon.png";
                    }
                    else if (item.type == "video")
                    {
                        item.img = "videoicon.png";
                    }
                    else if (item.type == "image")
                    {
                        item.img = "imageicon.png";
                    }
                    else
                    {
                        item.img = "webicon.png";
                    }

                    item.type = item.type.ToUpper();

                    //if (signupcodecollection[0].referral == "NOVO")
                    //{
                    //    item.Padding = 8;
                    //}
                    //else if (signupcodecollection[0].referral == "SFEWH")
                    //{
                    //    item.Padding = 10;
                    //}
                    //else if (signupcodecollection[0].referral == "SFEAT")
                    //{
                    //    item.Padding = 7;
                    //}

                }

                infolist.ItemsSource = signupcodecollection[0].signupcodeinfolist;
                //infolist.HeightRequest = signupcodecollection[0].signupcodeinfolist.Count * 96;


                //hide Seemore Btn for Saxanda Signup Code 
                if (signupcodecollection[0].referral == "NOVO")
                {
                    morebtn.IsVisible = false;
                    MainDashimgFrame.IsVisible = false;
                    SecondDashimgFrame.IsVisible = true;
                    exitidHome.IsVisible = true;
                    exitidbrowse.IsVisible = true;
                    exitidexplore.IsVisible = true;

                    exitidHome.Text = signupcodecollection[0].externalidentifier;
                    exitidbrowse.Text = signupcodecollection[0].externalidentifier;
                    exitidexplore.Text = signupcodecollection[0].externalidentifier;

                    //Get Consent 
                    NovoConsent = await database.GetConsentAsync();
                    NovoConsent.exitid = signupcodecollection[0].externalidentifier;
                    var CleanString = String.Empty;
                    if (!String.IsNullOrEmpty(NovoConsent.content))
                    {
                        CleanString = NovoConsent.content.Replace("IE24SX00005", "");
                    }

                    Preferences.Default.Set("NovoExitid", NovoConsent.exitid);
                    Preferences.Default.Set("NovoContent", CleanString);
                }

                //Additional Dash Questions  SFEWH && SFECORE 
                //if (signupcodecollection[0].referral == "SFEWH" || signupcodecollection[0].referral == "SFECORE" || signupcodecollection[0].referral == "RBHTHCM")
                if (signupcodecollection[0].referral == "SFECORE" )
                {
                    DashQuestions();
                }

            }
            else
            {
                infotab.IsVisible = false;

                var getimagesource = new Uri("https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/PeopleWithApp-AppImage-TemplateNov24.png");

                maindashimage.Source = ImageSource.FromUri(getimagesource);

                return;
            }


            //get video support 
            if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                string ReturnType = "Filter";
                var allvideos = await database.GetAllVideos(ReturnType);
                var signupid = Helpers.Settings.SignUp;
                var FilterVidoes = new ObservableCollection<videos>(allvideos.Where(x =>
                        (!string.IsNullOrEmpty(x.referral) && x.referral.Contains(signupid)) ||
                        (!string.IsNullOrEmpty(x.signupcodeid) && x.signupcodeid.Contains(signupid))));


                videoslist.ItemsSource = FilterVidoes;

                //videoslist.HeightRequest = 180 * FilterVidoes.Count;


                if (FilterVidoes.Count == 0)
                {
                    novidimg.IsVisible = true;
                    novidlbl.IsVisible = true;
                }

            }
            else
            {
                vidhelplbl.IsVisible = false;
                videoslist.IsVisible = false;
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void DashQuestions()
    {
        try
        {
            //GetDashQuestions
            DashQuestionInputs = await database.GetDashQuestions();
            DashQuestionAnswers = await database.GetDashQuestionAnswers();

            foreach (var item in DashQuestionAnswers)
            {
                var dt = item.createdAt;
                string NewFormat = dt.ToString("dd/MM/yy");
                item.DateConverted = NewFormat;

                item.Title = DashQuestionInputs.Where(x => x.id == item.inputid).FirstOrDefault().dataTab;
                item.Question = DashQuestionInputs.Where(x => x.id == item.inputid).FirstOrDefault().label;

                item.HasNotes = !String.IsNullOrEmpty(item.notes);

            }

            string DescTemplate = "Please complete the questionnaire related to your ";

            // Group DashQuestionInputs by dataTab
            GroupedDashQuestions = new ObservableCollection<IGrouping<string, registryDataInputs>>(
                DashQuestionInputs
                    .Where(x => !string.IsNullOrEmpty(x.dataTab))
                    .GroupBy(x => x.dataTab)
            );

            // Initialize the collection for dash items
            var PopulateDashQuestions = new ObservableCollection<dashitem>();

            // Populate dash items with the first item from each group
            foreach (var group in GroupedDashQuestions)
            {
                var firstItem = group.FirstOrDefault();
                if (firstItem != null)
                {
                    var Data = new dashitem
                    {
                        Title = group.Key,
                        Type = $"{DescTemplate}{group.Key}"
                    };
                    PopulateDashQuestions.Add(Data);
                }
            }

            DashQuestionItems.ItemsSource = PopulateDashQuestions;

            string AddItems = string.Empty;
            var titles = PopulateDashQuestions.Select(g => g.Title).ToList();

            if (titles.Count == 1)
            {
                AddItems = titles[0];
            }
            else if (titles.Count > 1)
            {
                AddItems = string.Join(", ", titles.Take(titles.Count - 1)) + ", and " + titles.Last();
            }

            Descriptionlbl.Text = $"We capture additional questions based on your {AddItems}. Please complete the following sections.";



            //// Clear and populate the list
            //var uniqueTabs = DashQuestionInputs
            //    .Where(x => !string.IsNullOrEmpty(x.dataTab))
            //    .GroupBy(x => x.dataTab)
            //    .Select(g => g.First());

            //var PopulateDashQuestions = new ObservableCollection<dashitem>();

            //foreach (var item in uniqueTabs)
            //{
            //    var Data = new dashitem
            //    {
            //        Title = item.dataTab,
            //        Type = $"{DescTemplate}{item.dataTab}"
            //    };

            //    PopulateDashQuestions.Add(Data);
            //}

            //// Assign to the ListView
            //DashQuestionItems.ItemsSource = PopulateDashQuestions;

            //string AddItems = string.Empty;
            //var titles = PopulateDashQuestions.Select(g => g.Title).ToList();

            //if (titles.Count == 1)
            //{
            //    AddItems = titles[0];
            //}
            //else if (titles.Count > 1)
            //{
            //    AddItems = string.Join(", ", titles.Take(titles.Count - 1)) + ", and " + titles.Last();
            //}

            //Descriptionlbl.Text = "We capture additional questions based on your "  + AddItems +  " . Please complete the following sections.";

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    async void checknotificationsEnabled()
    {
        try
        {
            //if(DeviceInfo.Current.Platform == DevicePlatform.Android)
            //{
            //    PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
            //    if (status == PermissionStatus.Granted)
            //    {
            //        EnableNotifStack.IsVisible = false;
            //    }
            //    else
            //    {
            //        EnableNotifStack.IsVisible = true;
            //    }
            //}
            //else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            //{
            //    var notificationService = DependencyService.Get<INotificationService>();
            //    bool notificationsEnabled = await notificationService.CheckRequestNotificationPermissionAsync();

            //    if (notificationsEnabled)
            //    {
            //        EnableNotifStack.IsVisible = false;
            //    }
            //    else
            //    {
            //        EnableNotifStack.IsVisible = true;
            //    }
            //  }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getuserfeedbackdata()
    {
        try
        {

            //get all user feedback data

            // Start the stopwatch to measure elapsed time
            // Stopwatch stopwatch = new Stopwatch();
            // stopwatch.Start();

            foryouuserlist.Clear();

            // Retrieve all user feedback data
            userfeedbacklist = await database.GetUserFeedback();

            AllUserMedications = await database.GetUserMedicationsAsync();
            AllUserSupplements = await database.GetUserSupplementsAsync();

            findduemedications();

            findduesupplements();

            findsymptomdata();

            updateyourhealthdata();

            fixnulluserfeedback();

            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
               // getfitnesshealthdata();
            }

            // Stop the stopwatch after retrieval
            // stopwatch.Stop();



            var newItems = new List<dashitem>
{
    new dashitem { ContactImage = "healthreporticon.png", Title = "Generate your Health Report", BackgroundColor = Color.FromArgb("#e5f5fc"), Type = "Health Report" },
  //  new dashitem { ContactImage = "diagnosishome.png", Title = "Have you received a new diagnosis?", BackgroundColor = Color.FromArgb("#E6E6FA"), Type = "Diagnosis" },
  //  new dashitem { ContactImage = "appointhome.png", Title = "Record a new appointment", BackgroundColor =  Color.FromArgb("#ffcccb"), Type = "Appointments" }
};

            // Add new items to the existing list
            foryouuserlist.AddRange(newItems);
            // Randomize the order of the list
            var randomizedList = foryouuserlist.OrderBy(x => Guid.NewGuid()).ToList();

            // Set the randomized list as the ItemsSource
            activitylist.ItemsSource = randomizedList;


            listloader.IsVisible = false;
            activitylist.IsVisible = true;

            // Show the elapsed time in a DisplayAlert
            //await Application.Current.MainPage.DisplayAlert(
            //    "Data Retrieval Time",
            //    $"Data retrieval took {stopwatch.Elapsed}.",
            //    "OK"
            //);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void updateyourhealthdata()
    {
        try
        {

            //symptom data
            if (userfeedbacklist[0].symptomfeedbacklist != null)
            {
                var inputFormats = new[] {
    "M/d/yyyy h:mm:ss tt",
    "MM/dd/yyyy h:mm:ss tt",
    "M/d/yyyy H:mm:ss",
    "MM/dd/yyyy HH:mm:ss"
};

                var expectedFormat = "dd/MM/yyyy HH:mm:ss";
                var culture = new CultureInfo("en-GB");

                foreach (var x in userfeedbacklist[0].symptomfeedbacklist)
                {
                    if (!x.action.Contains("deleted"))
                    {
                        // Normalize special space character before AM/PM
                        x.datetime = x.datetime.Replace('\u202F', ' ').Trim();

                        if (DateTime.TryParseExact(x.datetime, inputFormats, culture, DateTimeStyles.None, out var parsed))
                        {
                            x.datetime = parsed.ToString(expectedFormat, culture); // Normalize to UK format

                            var ss = x.datetime;
                        }
                    }
                }


                //var groupedsymptoms = userfeedbacklist[0].symptomfeedbacklist.GroupBy(x => x.label).ToList();

                // Group by label and select the first item from each group
                var filteredSymptoms = userfeedbacklist[0].symptomfeedbacklist
                     .OrderByDescending(x => DateTime.Parse(x.datetime))
                     .Where(x => !x.action.Contains("deleted"))
                    .GroupBy(x => x.label)
                     .Select(g =>
                     {
                         var firstItem = g.First();
                         firstItem.label = firstItem.label.TrimEnd(); // Trim whitespace at the end
                         return firstItem;
                     })
                    .ToList();



                int symptomrecordedcount = 0;

                foreach (var item in filteredSymptoms)
                {

                    var convertdate = DateTime.Parse(item.datetime);
                    TimeSpan timeDifference = DateTime.Now.Date - convertdate.Date;

                    if (timeDifference.TotalDays < 1)
                    {
                        item.datetimestring = "Today";
                    }
                    else if (timeDifference.TotalDays == 1)
                    {
                        item.datetimestring = "Yesterday";
                    }
                    else
                    {
                        item.datetimestring = $"{(int)timeDifference.TotalDays} days ago";
                    }

                    var sl = userfeedbacklist[0].symptomfeedbacklist.Where(x => x.label == item.label).OrderByDescending(x => DateTime.Parse(x.datetime)).ToList();

                    var ifrecorded = userfeedbacklist[0].symptomfeedbacklist.Where(x => x.label == item.label).Where(x => DateTime.Parse(x.datetime).Date == DateTime.Now.Date).FirstOrDefault();

                    if (ifrecorded != null)
                    {
                        symptomrecordedcount++;
                    }

                    if (sl.Count >= 2)
                    {

                        var value0 = Convert.ToDouble(sl[0].value);
                        var value1 = Convert.ToDouble(sl[1].value);

                        var newestScore = double.IsNaN(value0) ? 0 : Convert.ToInt32(value0);
                        var previousScore = double.IsNaN(value1) ? 0 : Convert.ToInt32(value1);
                        // Compare the newest and the previous score
                        //   var newestScore = Convert.ToInt32(sl[0].value);      // Assuming .value represents the score
                        //   var previousScore = Convert.ToInt32(sl[1].value);

                        if (newestScore > previousScore)
                        {
                            item.img = "downarrow.png";
                            item.improvelbl = "Worsening";


                        }
                        else if (newestScore < previousScore)
                        {
                            item.img = "uparrow.png";
                            item.improvelbl = "Improving";
                        }
                        else
                        {
                            item.img = "equal.png";
                            item.improvelbl = "No Change";
                        }


                        item.nooftimesstring = sl.Count.ToString();

                        var scores = sl.Select(x => Convert.ToInt32(x.value)).ToList();

                        // Calculate highest score and average score
                        var highestScore = scores.Max();
                        var averageScore = scores.Average();

                        item.highstring = highestScore.ToString();
                        item.avgstring = averageScore.ToString("0");

                    }
                    else
                    {
                        //no change
                        item.img = "equal.png";
                        item.improvelbl = "No Change";

                        item.avgstring = "50";
                        item.highstring = "50";
                        item.nooftimesstring = "1";
                    }

                    //if(sl.Count < 3)
                    //{
                    //    item.symptominfo = "Recently added. Monitoring for trends";
                    //}
                    //else
                    //{
                    //    Random random = new Random();

                    //    int randomNumber = random.Next(1, 8);

                    //    //choose a random stat to show

                    //    if(randomNumber == 1)
                    //    {
                    //        //average per day
                    //        var days = sl.GroupBy(x => DateTime.Parse(x.datetime).Date).Count();
                    //        var count = sl.Count(x => x.label == item.label);
                    //        var sum = (double)count / days;


                    //        item.symptominfo = "Recorded " + sum.ToString() + " times per day on average";
                    //    }
                    //    else if(randomNumber == 2)
                    //    {

                    //        var maxScore = sl.Max(x => x.value);

                    //        item.symptominfo = "Highest frequency of " + maxScore + " recorded";
                    //    }
                    //    else if (randomNumber == 3)
                    //    {
                    //        // Date and time of the highest score recorded
                    //        var maxRecord = sl.OrderByDescending(x => x.value).First();
                    //        var maxDate = DateTime.Parse(maxRecord.datetime);
                    //        item.symptominfo = "Highest score recorded on " + maxDate.ToString("dd/MM/yyyy HH:mm");
                    //    }
                    //    else if (randomNumber == 4)
                    //    {
                    //        // Check if average score has increased or decreased over the last week
                    //        DateTime now = DateTime.Now;

                    //        var lastWeekData = sl.Where(x => DateTime.Parse(x.datetime) >= now.AddDays(-7)).ToList();
                    //        var priorWeekData = sl.Where(x => DateTime.Parse(x.datetime) >= now.AddDays(-14) && DateTime.Parse(x.datetime) < now.AddDays(-7)).ToList();

                    //        var lastWeekAverage = lastWeekData.Any() ? lastWeekData.Average(x => double.TryParse(x.value, out double result) ? result : 0) : 0;

                    //        var priorWeekAverage = priorWeekData.Any()
                    //            ? priorWeekData.Average(x => double.TryParse(x.value, out double result) ? result : 0)
                    //            : 0;

                    //        if (lastWeekAverage > priorWeekAverage)
                    //        {
                    //            item.symptominfo = "Average score has increased over the last week";
                    //        }
                    //        else if (lastWeekAverage < priorWeekAverage)
                    //        {
                    //            item.symptominfo = "Average score has decreased over the last week";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Average score remained stable over the last week";
                    //        }
                    //    }
                    //    else if (randomNumber == 5)
                    //    {
                    //        // Frequency increase over the last week
                    //        var lastWeekCount = sl.Count(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-7));
                    //        var priorWeekCount = sl.Count(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-14) && DateTime.Parse(x.datetime) < DateTime.Now.AddDays(-7));

                    //        if (lastWeekCount > priorWeekCount)
                    //        {
                    //            item.symptominfo = "Recording frequency has increased over the last week";
                    //        }
                    //        else if (lastWeekCount < priorWeekCount)
                    //        {
                    //            item.symptominfo = "Recording frequency has decreased over the last week";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Recording frequency remained the same over the last week";
                    //        }
                    //    }
                    //    else if (randomNumber == 6)
                    //    {
                    //        // Check if recorded daily in the last week
                    //        var lastWeekDays = sl.Where(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-7))
                    //                             .GroupBy(x => DateTime.Parse(x.datetime).Date)
                    //                             .Count();

                    //        if (lastWeekDays >= 7)
                    //        {
                    //            item.symptominfo = "Recorded daily in the last week";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Not recorded daily in the last week";
                    //        }
                    //    }
                    //    else if (randomNumber == 7)
                    //    {
                    //        // Consistency of recording frequency
                    //        //var dailyCounts = sl.Where(x => DateTime.Parse(x.datetime) >= DateTime.Now.AddDays(-7))
                    //        //                    .GroupBy(x => DateTime.Parse(x.datetime).Date)
                    //        //                    .Select(g => g.Count())
                    //        //                    .ToList();

                    //        //var mean = dailyCounts.Average();
                    //        //var variance = dailyCounts.Sum(c => Math.Pow(c - mean, 2)) / dailyCounts.Count;
                    //        //var stddev = Math.Sqrt(variance);

                    //        //if (stddev < 1)
                    //        //{
                    //        //    item.symptominfo = "Recording frequency is consistent";
                    //        //}
                    //        //else
                    //        //{
                    //        //    item.symptominfo = "Recording frequency is inconsistent";
                    //        //}
                    //    }
                    //    else if (randomNumber == 8)
                    //    {
                    //        // Additional custom trend: e.g., proportion of recordings in the morning/afternoon/evening
                    //        var morningCount = sl.Count(x => DateTime.Parse(x.datetime).Hour < 12);
                    //        var afternoonCount = sl.Count(x => DateTime.Parse(x.datetime).Hour >= 12 && DateTime.Parse(x.datetime).Hour < 18);
                    //        var eveningCount = sl.Count(x => DateTime.Parse(x.datetime).Hour >= 18);

                    //        if (morningCount > afternoonCount && morningCount > eveningCount)
                    //        {
                    //            item.symptominfo = "Most recordings occur in the morning";
                    //        }
                    //        else if (afternoonCount > morningCount && afternoonCount > eveningCount)
                    //        {
                    //            item.symptominfo = "Most recordings occur in the afternoon";
                    //        }
                    //        else
                    //        {
                    //            item.symptominfo = "Most recordings occur in the evening";
                    //        }
                    //    }


                    //    //item.symptominfo = "Recently added. Monitoring for trends";
                    //}
                    var ss = "";

                    //          item.symptominfo = foryouuserlistsymptom
                    //.Where(x => x.title.Contains(item.label))
                    //.ToList();
                    //  item.symptomlist = userfeedbacklist[0].symptomfeedbacklist.Where(x => x.label == item.label).ToList();
                }


                double progress = filteredSymptoms.Count > 0 ? (double)symptomrecordedcount / filteredSymptoms.Count * 100 : 0;

                // Update the progress bar value
                symprogressbar.Progress = progress;

                var SuppsLeft = filteredSymptoms.Count - symptomrecordedcount;
                if (SuppsLeft > 0)
                {
                    var SuppsRem = SuppsLeft + " " + "Symptoms to Record";
                    SympRemain.Text = SuppsRem;

                    //var newdaily = new dashitem();
                    //newdaily.Title = SuppsLeft + " Symptoms to Record";
                    //dailytasklist.Add(newdaily);

                }
                else
                {
                    var SuppsRem = "All Symptoms Updated";
                    SympRemain.Text = SuppsRem;
                }

                //symptom Name too long Trim 

                foreach (var item in filteredSymptoms)
                {
                    item.title = item.label;

                    if (item.label.Length > 21)
                    {
                        item.shortlabel = item.label.Substring(0, 18) + "...";
                    }
                    else
                    {
                        item.shortlabel = item.label;
                    }
                }

                symptomdetaillist.ItemsSource = filteredSymptoms;

                // Initialize a list to store data points for the last seven days
                var lastSevenDaysData = new List<feedbackdata>();

                // Loop through each of the past 7 days
                for (int i = 6; i >= 0; i--)
                {
                    var targetDate = DateTime.Now.Date.AddDays(-i);

                    // Count the items for the target date
                    int countForDay = userfeedbacklist[0].symptomfeedbacklist
                        .Count(item => DateTime.Parse(item.datetime).Date == targetDate);

                    // Add data point to the list
                    lastSevenDaysData.Add(new feedbackdata
                    {
                        datetime = targetDate.Date.ToString("dd MMM"),
                        Count = countForDay
                    });
                }


                //find max and add one
                int maxCount = lastSevenDaysData.Max(data => data.Count) + 1;
                chartnumaxis.Maximum = maxCount;
                // Bind the data to your chart


                symptomprogresschart.ItemsSource = lastSevenDaysData;
            }
            else
            {

                RecentHealthlbl.IsVisible = false;
                recentsymlbl.IsVisible = false;
                symptomdetaillist.IsVisible = false;
                symdataframelbl1.IsVisible = false;
                symdataframelbl2.IsVisible = false;
                SymptomProgChart.IsVisible = false;
                nosymdataframe.IsVisible = true;
                SympRemain.Text = "No Symptoms Added";
            }




            // Set the filtered list as the ItemsSource for the ListView
            if (userfeedbacklist[0].measurementfeedbacklist != null)
            {



                var filteredmeasurements = userfeedbacklist[0].measurementfeedbacklist
                    .Where(x => x.action != "deleted") // Remove Deleted items
        .OrderByDescending(x => DateTime.Parse(x.datetime))
    .GroupBy(x => x.label)
    .Select(g => g.First())  // Select only the first item in each group
    .ToList();

                foreach (var item in filteredmeasurements)
                {
                    var convertdate = DateTime.Parse(item.datetime);
                    TimeSpan timeDifference = DateTime.Now.Date - convertdate.Date;

                    if (timeDifference.TotalDays < 1)
                    {
                        item.datetimestring = "Today";
                    }
                    else if (timeDifference.TotalDays == 1)
                    {
                        item.datetimestring = "Yesterday";
                    }
                    else
                    {
                        item.datetimestring = $"{(int)timeDifference.TotalDays} days ago";
                    }

                    // Ensure userfeedbacklist has data
                    if (userfeedbacklist != null && userfeedbacklist.Count > 0)
                    {
                        if (item.label == "Blood Pressure")
                        {

                        }
                        else
                        {

                            item.symptomlist = userfeedbacklist[0].measurementfeedbacklist
                                .Where(x => x.label == item.label)
                                .ToList();
                        }
                    }

                    item.labelandunit = item.value + " " + item.unit;
                }


                foreach (var item in filteredmeasurements)
                {
                    if (item.unit == "Stones/Pounds")
                    {
                        if (item.value.Contains("st"))
                        {
                            //Do Nothing
                        }
                        else
                        {
                            var split = item.value.Split('.');
                            var Newlbl = split[0] + "st" + " " + split[1] + "lbs";
                            item.value = Newlbl;
                        }
                    }
                    else if (item.unit == "Feet/Inches")
                    {
                        if (item.value.Contains("'"))
                        {
                            //Do Nothing
                        }
                        else
                        {
                            var split = item.value.Split('.');
                            var Newlbl = split[0] + "'" + " " + split[1] + "\"";
                            item.value = Newlbl;
                        }
                    }
                    else if (item.unit == "Hours/Minutes")
                    {
                        if (!String.IsNullOrEmpty(item.value))
                        {
                            if (item.value.Contains("h"))
                            {
                                //Do Nothing
                            }
                            else
                            {
                                var split = item.value.Split('.');
                                var Newlbl = split[0] + "h" + " " + split[1] + "m";
                                item.value = Newlbl;
                            }
                        }
                    }
                }


                if (filteredmeasurements.Count > 0)
                {
                    // var takefivemeasurements = filteredmeasurements.Take(1).ToList();

                    // measurementdetaillist.ItemsSource = takefivemeasurements;
                    // measurementdetaillist.HeightRequest = 152 * takefivemeasurements.Count;
                    measlbl.IsVisible = true;
                    measlblsub.IsVisible = true;
                    measurementnochartdetaillist.IsVisible = true;
                    nomeasurementdataframe.IsVisible = false;

                    if (filteredmeasurements.Count > 5)
                    {
                        // Take the next four items for measurementnochartdetaillist
                        var nextFourMeasurements = filteredmeasurements.Skip(1).Take(5).ToList();

                        measurementnochartdetaillist.ItemsSource = nextFourMeasurements;
                        measurementnochartdetaillist.HeightRequest = 54 * nextFourMeasurements.Count;
                    }
                    else
                    {
                        var nextFourMeasurements = filteredmeasurements.ToList();

                        measurementnochartdetaillist.ItemsSource = nextFourMeasurements;
                        measurementnochartdetaillist.HeightRequest = 54 * nextFourMeasurements.Count;

                    }





                }
                else
                {
                    measlbl.IsVisible = false;
                    measlblsub.IsVisible = false;
                    measurementnochartdetaillist.IsVisible = false;
                    nomeasurementdataframe.IsVisible = true;
                    //  measurementdetaillist.ItemsSource = filteredmeasurements;
                    //  measurementdetaillist.HeightRequest = 152 * filteredmeasurements.Count;
                }

                var random = new Random();
                var selectedMeasurement = userfeedbacklist[0].measurementfeedbacklist[random.Next(userfeedbacklist[0].measurementfeedbacklist.Count)];

                if (selectedMeasurement != null)
                {
                    // var mostCommonMood = moodsForDay.MoodLabel;
                    if (selectedMeasurement.label == "Height")
                    {

                    }
                    else
                    {



                        var newItem2 = new dashitem
                        {
                            ContactImage = "measurementhome.png",
                            Title = "Update your " + selectedMeasurement.label,
                            Type = "Measurements",
                            BackgroundColor = Color.FromArgb("#e5f0fb") // Example color
                        };

                        foryouuserlist.Add(newItem2);
                    }
                }

                var newItem = new dashitem
                {
                    ContactImage = "measurementhome.png",
                    Title = "Start recording your measurements",
                    Type = "Measurements",
                    BackgroundColor = Color.FromArgb("#e5f0fb") // Example color
                };

                if (foryouuserlist.Contains(newItem))
                {
                    foryouuserlist.Remove(newItem);
                }

            }
            else
            {
                measlbl.IsVisible = false;
                measlblsub.IsVisible = false;
                // measurementdetaillist.IsVisible = false;
                measurementnochartdetaillist.IsVisible = false;
                nomeasurementdataframe.IsVisible = true;

                var newItem = new dashitem
                {
                    ContactImage = "measurementhome.png",
                    Type = "Measurements",
                    Title = "Start recording your measurements",
                    BackgroundColor = Color.FromArgb("#e5f0fb") // Example color
                };
                foryouuserlist.Add(newItem);


            }

            if (userfeedbacklist[0].moodfeedbacklist != null)
            {

                var sevenDaysAgo = DateTime.Now.AddDays(-7);

                var filteredMoods = userfeedbacklist[0].moodfeedbacklist
                    .Where(x => DateTime.Parse(x.datetime).Date >= sevenDaysAgo.Date && x.action != "deleted") // Filter last 7 days
                    .OrderByDescending(x => DateTime.Parse(x.datetime))
                    .GroupBy(x => x.label)
                    .Select(g => g.First())  // Select only the first item in each group
                    .ToList();

                var mooddata = new List<feedbackdata>();


                //new mood data listview

                // var sevenDaysAgo = DateTime.Now.AddDays(-6).Date; // last 7 days, inclusive of today
                var today = DateTime.Now.Date;

                var moodSummaryPerDay = new List<feedbackdata>();

                for (int i = 0; i < 7; i++)
                {
                    var currentDay = sevenDaysAgo.AddDays(i).Date;

                    var moodsForDay1 = userfeedbacklist[0].moodfeedbacklist
                        .Where(x =>
                        {
                            var moodDate = DateTime.Parse(x.datetime).Date;
                            return moodDate == currentDay && x.action != "deleted";
                        })
                        .ToList();

                    if (moodsForDay1.Count == 0)
                    {
                        // No mood entry for that day
                        moodSummaryPerDay.Add(new feedbackdata
                        {
                            shortlabel = currentDay.ToString("dd MMM"),
                            title = "close.png",
                            avgstring = "No Mood"
                        });
                    }
                    else
                    {
                        // Find the most common mood label
                        var topMood = moodsForDay1
                            .GroupBy(m => m.label)
                            .OrderByDescending(g => g.Count())
                            .First().Key;

                        moodSummaryPerDay.Add(new feedbackdata
                        {
                            shortlabel = currentDay.ToString("dd MMM"),
                            title = topMood.ToLower() + ".png",
                            avgstring = topMood
                        });



                    }
                }

                moodSummaryPerDay.Reverse();

                moodlistsummary.ItemsSource = moodSummaryPerDay;

                //foreach (var item in filteredMoods)
                //{
                //    var countForDay = userfeedbacklist[0].moodfeedbacklist
                //        .Where(x => x.label == item.label && DateTime.Parse(x.datetime) >= sevenDaysAgo && x.action != "deleted") // Apply date filter again
                //        .Count();

                //    mooddata.Add(new feedbackdata
                //    {
                //        label = item.label,
                //        Count = countForDay
                //    });
                //}

                //if(mooddata.Count > 0)
                //{
                //    int GetCount = mooddata.Sum(item => item.Count);
                //    TotalCountlbl.Text = GetCount.ToString();

                //    var DateTimeNOW = DateTime.Now.ToString("dd/MM/yy");
                //    var Weekago = DateTime.Now.AddDays(-7).ToString("dd/MM/yy");
                //    var Timeline = Weekago + " - " + DateTimeNOW;
                //    MoodTimelinelbl.Text = Timeline;
                //    moodchart.ItemsSource = mooddata;

                //    moodlbl.IsVisible = true;
                //    measlbl2.IsVisible = true;
                //    moodframe.IsVisible = true;
                //    nomooddataframe.IsVisible = false;
                //}
                //else
                //{
                //    moodlbl.IsVisible = true;
                //    measlbl2.IsVisible = true;
                //    moodframe.IsVisible = true;
                //    nomooddataframe.IsVisible = false;

                //    mooddata.Add(new feedbackdata
                //    {
                //        label = "No Data",
                //        Count = 1
                //    });

                //    var DateTimeNOW = DateTime.Now.ToString("dd/MM/yy");
                //    var Weekago = DateTime.Now.AddDays(-7).ToString("dd/MM/yy");
                //    var Timeline = Weekago + " - " + DateTimeNOW;
                //    MoodTimelinelbl.Text = Timeline;

                //    TotalCountlbl.Text = "0";

                //    moodchart.ItemsSource = mooddata;
                //}


                var targetDate = DateTime.Today;

                // Filter moods for the specified date
                var moodsForDay = userfeedbacklist[0].moodfeedbacklist
                    .Where(x => DateTime.Parse(x.datetime).Date == targetDate)
                    .GroupBy(x => x.label)
                    .Select(g => new
                    {
                        MoodLabel = g.Key,
                        Count = g.Count()
                    })
                    .OrderByDescending(x => x.Count)
                    .FirstOrDefault(); // Get the most common mood for the day

                if (moodsForDay != null)
                {
                    // Display the most common mood
                    var mostCommonMood = moodsForDay.MoodLabel;

                    var newItem2 = new dashitem
                    {
                        ContactImage = "moodhome.png",
                        Type = "Mood",
                        Title = "You're mostly feeling " + mostCommonMood + " today",
                        BackgroundColor = Color.FromArgb("#FFF8DC") // Example color
                    };

                    foryouuserlist.Add(newItem2);

                }
                else
                {
                    var newItem1 = new dashitem
                    {
                        ContactImage = "moodhome.png",
                        Type = "Mood",
                        Title = "How is your mood today?",
                        BackgroundColor = Color.FromArgb("#FFF8DC") // Example color
                    };

                    foryouuserlist.Add(newItem1);

                }

                var newItem = new dashitem
                {
                    ContactImage = "moodhome.png",
                    Type = "Mood",
                    Title = "Start updating your mood",
                    BackgroundColor = Color.FromArgb("#FFF8DC") // Example color
                };

                if (foryouuserlist.Contains(newItem))
                {

                    foryouuserlist.Remove(newItem);
                }

            }
            else
            {
                moodlbl.IsVisible = false;
                measlbl2.IsVisible = false;
                moodframe.IsVisible = false;
                nomooddataframe.IsVisible = true;

                var newItem = new dashitem
                {
                    ContactImage = "moodhome.png",
                    Type = "Mood",
                    Title = "Start updating your mood",
                    BackgroundColor = Color.FromArgb("#FFF8DC") // Example color
                };
                foryouuserlist.Add(newItem);


            }


            // dailytaskinfolist.ItemsSource = dailytasklist;

            //check if the questionnaire prompt is needed

            var signup = Helpers.Settings.SignUp;
            //Get Signup Questionnaries 
            var getQuestionairesTask = await database.GetQuestionnaires();
            questionnaires = getQuestionairesTask;




            if (signup.Contains("SFEWH"))
            {

                var QuestionList = new ObservableCollection<questionnaire>();
                SFEWHStudy.IsVisible = true;

                var ItemstoRemove = new List<string>();
                additionalquestionstab.IsVisible = true;

                if (signup.Contains("SFEWHA1"))
                {
                    var IDList = new List<string>
                        {
                            //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Diabetes ID
                            "73D47262-1B2C-4451-A4FC-978582D77FE0"
                        };

                    if (questionnaires != null)
                    {
                        QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => IDList.Contains(q.questionnaireid)));
                    }

                    questionnaireinfotext.Text = "Complete Diabetes Questionnaire";
                }
                else if (signup.Contains("SFEWHA2"))
                {

                    var IDList = new List<string>
                        {
                            //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Breast cancer ID
                            "F7FB770B-286F-4300-814D-E76AACB6DACF"
                        };

                    if (questionnaires != null)
                    {
                        QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => IDList.Contains(q.questionnaireid)));
                    }

                    questionnaireinfotext.Text = "Complete Breast Cancer Questionnaire";
                }
                else if (signup.Contains("SFEWHA3"))
                {

                    var IDList = new List<string>
                        {
                            //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Stress ID
                            "4A27076A-A2A3-45DD-A061-34A4F1799B20"
                        };

                    if (questionnaires != null)
                    {
                        QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => IDList.Contains(q.questionnaireid)));
                    }

                    questionnaireinfotext.Text = "Complete SF36 Questionnaire";
                }
                else if (signup.Contains("SFEWHA4"))
                {
                    var IDList = new List<string>
                        {
                            //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Stress ID
                            "4A27076A-A2A3-45DD-A061-34A4F1799B20"
                        };

                    if (questionnaires != null)
                    {
                        QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => IDList.Contains(q.questionnaireid)));
                    }

                    questionnaireinfotext.Text = "Complete SF36 Questionnaire";
                }

                if (userfeedbacklist[0].initialquestionnairefeedbacklist != null)
                {
                    //check dates
                    completequestionnaireborder.IsVisible = false;

                    if (userfeedbacklist[0].initialquestionnairefeedbacklist.Count > 1)
                    {
                        QuestionnairePrompt.IsVisible = false;
                    }
                    else
                    {
                        //Item to Remove 
                        ItemstoRemove.Add(userfeedbacklist[0].initialquestionnairefeedbacklist[0].label);
                        QuestionsIndicator.IsVisible = false;
                        var Filter = new ObservableCollection<questionnaire>(QuestionList.Where(q => !ItemstoRemove.Contains(q.title)));
                        QuestionsPrompt.ItemsSource = Filter;
                        QuestionsPrompt.IsSwipeEnabled = false;
                        QuestionnairePrompt.IsVisible = true;
                    }
                    //Use label to remove item 
                }
                else
                {
                    //completequestionnaireborder.IsVisible = true;

                    QuestionnairePrompt.IsVisible = true;
                    QuestionsIndicator.IsVisible = true;
                    QuestionsPrompt.IsSwipeEnabled = true;
                    QuestionsPrompt.ItemsSource = QuestionList;
                }

                //Check once complete 




            }
            else if (signup.Contains("SFECORE"))
            {
                var QuestionList = new ObservableCollection<questionnaire>();
                completequestionnaireborder.IsVisible = false;
                additionalquestionstab.IsVisible = true;
                SFECoreStudy.IsVisible = true;

                //SF-36
                string QID = "DC6A9FD7-242B-4299-9672-D745669FEAF0";
                if (questionnaires != null)
                {
                    QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => QID.Contains(q.questionnaireid)));
                }

                QuestionsIndicator.IsVisible = false;
                QuestionsPrompt.ItemsSource = QuestionList;
                QuestionsPrompt.IsSwipeEnabled = false;
                QuestionnairePrompt.IsVisible = true;


                if (userfeedbacklist[0].initialquestionnairefeedbacklist != null)
                {
                    QuestionnairePrompt.IsVisible = false;
                    completequestionnaireborder.IsVisible = false;

                    string retrievedId = Preferences.Get("SFEcoreNotID", string.Empty);

                    if (!string.IsNullOrEmpty(retrievedId) && int.TryParse(retrievedId, out int notificationId))
                    {
                        LocalNotificationCenter.Current.Cancel(notificationId);

                        //Delete SFEcoreNotID. No Longer Needed
                        Preferences.Remove("SFEcoreNotID");
                    }
                }
            }
            else if (signup.Contains("RBHTHCM"))
            {
                var QuestionList = new ObservableCollection<questionnaire>();
                completequestionnaireborder.IsVisible = false;
                //additionalquestionstab.IsVisible = true;
                //SFECoreStudy.IsVisible = true;

                //Hypertrophic Obstructive Cardiomyopathy Baseline Questionnaire
                string QID = "BE72B2A1-0707-4E8D-8E82-022BA4F959F4";
                if (questionnaires != null)
                {
                    QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => QID.Contains(q.questionnaireid)));
                }

                foreach(var item in QuestionList)
                {
                    item.title = "HOCM Baseline Questionnaire";
                    item.description = item.description.Replace(" completely, honestly, and without interruptions to the best of your ability.", "");
                }

                QuestionsIndicator.IsVisible = false;
                QuestionsPrompt.ItemsSource = QuestionList;
                QuestionsPrompt.IsSwipeEnabled = false;
                QuestionnairePrompt.IsVisible = true;

                //QuestionsPrompt.HeightRequest = 120;


                if (userfeedbacklist[0].initialquestionnairefeedbacklist != null)
                {
                    QuestionnairePrompt.IsVisible = false;
                    completequestionnaireborder.IsVisible = false;

                    string retrievedId = Preferences.Get("HOCMNotID", string.Empty);

                    if (!string.IsNullOrEmpty(retrievedId) && int.TryParse(retrievedId, out int notificationId))
                    {
                        LocalNotificationCenter.Current.Cancel(notificationId);

                        //Delete SFEcoreNotID. No Longer Needed
                        Preferences.Remove("BMSNotID");
                    }
                }
            }
            else if (signup.Contains("SFEAT"))
            {
                var QuestionList = new ObservableCollection<questionnaire>();
                completequestionnaireborder.IsVisible = false;

                if (userfeedbacklist[0].initialquestionnairefeedbacklist != null)
                {
                    //check dates
                    //if (userfeedbacklist[0].initialquestionnairefeedbacklist[0].action == "Completed Questionnaire")
                    //{
                    // completequestionnaireborder.IsVisible = false;
                    //}
                    //else
                    //{
                    // completequestionnaireborder.IsVisible = true;
                    //questionnaireinfotext.Text = "Complete EQ-5D 5L General Health Questionnaire";
                    //}
                    QuestionnairePrompt.IsVisible = false;
                    completequestionnaireborder.IsVisible = false;

                    string retrievedId = Preferences.Get("NsatNotID", string.Empty); // Returns empty string if not found
                                                                                     // Cancel Notification if ID exists and is a valid integer
                    if (!string.IsNullOrEmpty(retrievedId) && int.TryParse(retrievedId, out int notificationId))
                    {
                        LocalNotificationCenter.Current.Cancel(notificationId);

                        //Delete NsatNotID. No Longer Needed
                        Preferences.Remove("NsatNotID");
                    }
                }

                else
                {
                    completequestionnaireborder.IsVisible = false;


                    string QID = "A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF";
                    if (questionnaires != null)
                    {
                        QuestionList = new ObservableCollection<questionnaire>(questionnaires.Where(q => QID.Contains(q.questionnaireid)));
                    }

                    QuestionsIndicator.IsVisible = false;
                    QuestionsPrompt.ItemsSource = QuestionList;
                    QuestionsPrompt.IsSwipeEnabled = false;
                    QuestionnairePrompt.IsVisible = true;

                    questionnaireinfotext.Text = "Complete EQ-5D 5L General Health Questionnaire";


                }


            }



        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void fixnulluserfeedback()
    {
        try
        {
            if (userfeedbacklist != null)
            {

                if (userfeedbacklist[0].measurementfeedbacklist != null)
                {

                    bool CheckForNull = userfeedbacklist[0].measurementfeedbacklist.Any(x => string.IsNullOrEmpty(x.id));

                    if (CheckForNull)
                    {
                        //Get Usermeasurements 
                        UserMeasurementUpdate = await database.GetUserMeasurements();

                        foreach (var item in userfeedbacklist[0].measurementfeedbacklist)
                        {
                            if (string.IsNullOrEmpty(item.id))
                            {
                                //match user measurement to Value && inputdateTime 

                                var selectedMeasurement = UserMeasurementUpdate.Where(x => x.value == item.value && DateTime.Parse(x.inputdatetime) == DateTime.Parse(item.datetime)).FirstOrDefault();

                                if (selectedMeasurement != null)
                                {
                                    item.id = selectedMeasurement.id;
                                }
                            }
                        }

                        //update userfeedback table (Measurement Data)
                        if (userfeedbacklist[0].measurementfeedbacklist == null)
                        {
                            userfeedbacklist[0].measurementfeedbacklist = new ObservableCollection<feedbackdata>();
                        }

                        string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklist[0].measurementfeedbacklist);
                        userfeedbacklist[0].measurementfeedback = newsymJson;

                        await database.UserfeedbackUpdateMeasurementData(userfeedbacklist[0]);
                    }
                }


                if (userfeedbacklist[0].symptomfeedbacklist != null)
                {

                    //Check Symptoms 
                    bool CheckisNull = userfeedbacklist[0].symptomfeedbacklist.Any(x => string.IsNullOrEmpty(x.id));

                    if (CheckisNull)
                    {
                        //Get Usersymptoms 
                        UserSymptomsUpdate = await database.GetUserSymptomAsync();

                        foreach (var item in userfeedbacklist[0].symptomfeedbacklist)
                        {
                            if (string.IsNullOrEmpty(item.id))
                            {
                                //match user Mood to label && datetime 

                                var SelectedSymptom = UserSymptomsUpdate[0].feedback.Where(x => x.intensity == item.value && DateTime.Parse(x.timestamp) == DateTime.Parse(item.datetime)).FirstOrDefault();

                                if (SelectedSymptom != null)
                                {
                                    item.id = SelectedSymptom.symptomfeedbackid;
                                }
                            }
                        }

                        //update userfeedback table (Symptom Data)
                        if (userfeedbacklist[0].symptomfeedbacklist == null)
                        {
                            userfeedbacklist[0].symptomfeedbacklist = new ObservableCollection<feedbackdata>();
                        }

                        string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklist[0].symptomfeedbacklist);
                        userfeedbacklist[0].symptomfeedback = newsymJson;

                        await database.UserfeedbackUpdateSymptomData(userfeedbacklist[0]);

                    }
                }


                if (userfeedbacklist[0].moodfeedbacklist != null)
                {

                    //Check mood 
                    bool CheckNull = userfeedbacklist[0].moodfeedbacklist.Any(x => string.IsNullOrEmpty(x.id));

                    if (CheckNull)
                    {
                        //Get UserMood 
                        string userid = Preferences.Default.Get("userid", "Unknown");
                        UserMoodUpdate = await database.GetUserMoodsAsync(userid);

                        foreach (var item in userfeedbacklist[0].moodfeedbacklist)
                        {
                            if (string.IsNullOrEmpty(item.id))
                            {
                                //match user Mood to label && datetime 

                                var SelectedMood = UserMoodUpdate.Where(x => x.title == item.label && DateTime.Parse(x.datetime) == DateTime.Parse(item.datetime)).FirstOrDefault();

                                if (SelectedMood != null)
                                {
                                    item.id = SelectedMood.id;
                                }
                            }
                        }

                        //update userfeedback table (Mood Data)
                        if (userfeedbacklist[0].moodfeedbacklist == null)
                        {
                            userfeedbacklist[0].moodfeedbacklist = new ObservableCollection<feedbackdata>();
                        }

                        string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklist[0].moodfeedbacklist);
                        userfeedbacklist[0].moodfeedback = newsymJson;

                        await database.UserfeedbackUpdateMoodData(userfeedbacklist[0]);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getfitnesshealthdata()
    {
        try
        {

            //check if user has activated wearables

            // Retrieve all user feedback data
            //  var userfitnesslist = await database.GetUserFitnessData();

            //if(userfitnesslist.Count == 0 || userfitnesslist == null)
            //{
            //    healthdatagrid.IsVisible = true;
            //    return;
            //}


        //    if (string.IsNullOrEmpty(Helpers.Settings.FitnessData))
          //  {
            //    healthdatagrid.IsVisible = true;
              //  return;
           // }

            //check if they have any fitness health data added

            var hasPermission = await health.CheckPermissionAsync(HealthParameter.StepCount, PermissionType.Read);
            if (hasPermission)
            {


                var startOfDay = DateTime.Today;
                var now = DateTime.Now;

                var stepsCount = await health.ReadCountAsync(HealthParameter.StepCount, startOfDay, now);

                if (stepsCount == null)
                {
                    stepcountlbl.Text = "--";
                }
                else
                {

                    int roundedSteps = (int)Math.Round(stepsCount);

                    stepcountlbl.Text = roundedSteps.ToString();
                }
            }
            else
            {
                stepcountlbl.Text = "--";
            }


            //var hasPermission2 = await health.CheckPermissionAsync(HealthParameter.DistanceWalkingRunning, PermissionType.Read);
            //if (hasPermission2)
            //{


            //    var startOfDay = DateTime.Today;
            //    var now = DateTime.Now;

            //    var walkingdistance = await health.ReadLatestAvailableAsync(HealthParameter.DistanceWalkingRunning, "m");

            //    //int roundedDistance = (int)Math.Round(walkingdistance);

            //    double distanceKm = Math.Round(walkingdistance.Value.Value / 10, 1);

            //    distancelbl.Text = distanceKm.ToString();
            //}
            //else
            //{
            //    distancelbl.Text = "--";
            //}


            var hasPermission3 = await health.CheckPermissionAsync(HealthParameter.HeartRate, PermissionType.Read);
            if (hasPermission3)
            {


                var startOfDay = DateTime.Today;
                var now = DateTime.Now;

                var hr = await health.ReadLatestAvailableAsync(HealthParameter.HeartRate, "count/min");

                //int roundedDistance = (int)Math.Round(walkingdistance);

                // double heartrateround = Math.Round(heartrate.Value.Value);

                if (hr == null)
                {
                    heartratelbl.Text = "--";
                }
                else
                {

                    int roundedhr = (int)Math.Round(hr.Value.Value);

                    heartratelbl.Text = roundedhr.ToString();
                }
            }
            else
            {
                heartratelbl.Text = "--";
            }


            var hasPermission4 = await health.CheckPermissionAsync(HealthParameter.RespiratoryRate, PermissionType.Read);
            if (hasPermission4)
            {


                var startOfDay = DateTime.Today;
                var now = DateTime.Now;

                var hr = await health.ReadLatestAvailableAsync(HealthParameter.RespiratoryRate, "count/min");

                //int roundedDistance = (int)Math.Round(walkingdistance);

                // double heartrateround = Math.Round(heartrate.Value.Value);

                if (hr == null)
                {
                    resplbl.Text = "--";
                }
                else
                {

                    resplbl.Text = hr.Value.Value.ToString();
                }
            }
            else
            {
                resplbl.Text = "--";
            }


            healthdatagrid.IsVisible = false;
            healthgrid.IsVisible = true;


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void findsymptomdata()
    {
        try
        {
            // Clear previous results
            foryouuserlistsymptom.Clear();

            DateTime now = DateTime.Now;
            // Track symptom frequency in the last 3 days
            var lastThreeDays = now.AddDays(-3);
            var lastSevenDays = now.AddDays(-7);

            // Dictionary to track counts
            Dictionary<string, int> symptomFrequencyLast3Days = new Dictionary<string, int>();
            List<string> symptomsNotRecordedIn7Days = new List<string>();


            if (userfeedbacklist[0].symptomfeedbacklist == null)
            {

                var newItem = new dashitem
                {
                    ContactImage = "symptomshome.png",
                    Type = "Symptoms",
                    Title = "Start recording your symptoms today",
                    BackgroundColor = Color.FromArgb("#fff7ea") // Example color
                };
                foryouuserlist.Add(newItem);


                recentsymlbl.IsVisible = false;
                symptomdetaillist.IsVisible = false;

                symdataframelbl1.IsVisible = false;
                symdataframelbl2.IsVisible = false;
                SymptomProgChart.IsVisible = false;
                nosymdataframe.IsVisible = true;
                return;
            }


            var inputFormats = new[] {
   "M/d/yyyy h:mm:ss tt",
    "MM/dd/yyyy h:mm:ss tt",
    "M/d/yyyy H:mm:ss",
    "MM/dd/yyyy HH:mm:ss"
};
            var outputFormat = "dd/MM/yyyy HH:mm:ss";
            var ukCulture = new CultureInfo("en-GB");

            foreach (var x in userfeedbacklist[0].symptomfeedbacklist)
            {
                if (!x.action.Contains("deleted"))
                {
                    x.datetime = x.datetime.Replace('\u202F', ' ').Trim();

                    if (DateTime.TryParseExact(x.datetime, inputFormats, ukCulture, DateTimeStyles.None, out var parsed))
                    {
                        x.datetime = parsed.ToString(outputFormat, ukCulture);
                    }
                }
            }

            recentsymlbl.IsVisible = true;
            symptomdetaillist.IsVisible = true;
            symdataframelbl1.IsVisible = true;
            symdataframelbl2.IsVisible = true;
            SymptomProgChart.IsVisible = true;
            nosymdataframe.IsVisible = false;

            var newItemm = new dashitem
            {
                ContactImage = "symptomshome.png",
                Type = "Symptoms",
                Title = "Start recording your symptoms today",
                BackgroundColor = Color.FromArgb("#fff7ea") // Example color
            };
            if (foryouuserlist.Contains(newItemm))
            {
                foryouuserlist.Remove(newItemm);
            }

            //remove any deleted or symtpomdeleted items
            for (int i = userfeedbacklist[0].symptomfeedbacklist.Count - 1; i >= 0; i--)
            {
                var feedback = userfeedbacklist[0].symptomfeedbacklist[i];
                if (feedback.action == "deleted" || feedback.action == "symptomdeleted")
                {
                    userfeedbacklist[0].symptomfeedbacklist.RemoveAt(i);
                }
            }


            bool hasRecentUpdates = userfeedbacklist[0].symptomfeedbacklist.Any(e => e.action == "update" &&
                DateTime.TryParse(e.datetime, out DateTime dateTime) &&
                (now - dateTime).TotalHours <= 24);

            if (!hasRecentUpdates)
            {
                var newItem = new dashitem
                {
                    ContactImage = "symptomshome.png",
                    Type = "Symptoms",
                    Title = "You haven't recorded any symptom updates in the last 24 hours",
                    BackgroundColor = Color.FromArgb("#fff7ea") // Example color
                };
                foryouuserlistsymptom.Add(newItem);
            }
            // Group symptom entries by label for trend analysis
            var groupedSymptoms = userfeedbacklist[0].symptomfeedbacklist.GroupBy(s => s.label);

            foreach (var group in groupedSymptoms)
            {
                var symptomname = group.Key;
                var entries = group.ToList();

                // Track frequency for the last 3 days
                int recentFrequency = entries.Count(e => DateTime.TryParse(e.datetime, out DateTime entryDate) && entryDate >= lastThreeDays);
                symptomFrequencyLast3Days[symptomname] = recentFrequency;

                // Check for symptoms not recorded in the last 7 days
                var latestUpdatee = entries
                    .Where(e => e.action == "update" || e.action == "add")
                    .OrderByDescending(e => DateTime.Parse(e.datetime))
                    .FirstOrDefault();

                if (latestUpdatee == null || (now - DateTime.Parse(latestUpdatee.datetime)).TotalDays > 7)
                {
                    symptomsNotRecordedIn7Days.Add(symptomname);
                }


                // High-intensity detection
                var highIntensityEntry = entries.LastOrDefault(e => e.action == "update" && int.TryParse(e.value, out int value) && value >= 50);
                if (highIntensityEntry != null && DateTime.TryParse(highIntensityEntry.datetime, out DateTime highIntensityDateTime))
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = $"High intensity of {highIntensityEntry.value} recorded for {symptomname}",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Frequency detection for repeated updates in a short period
                if (entries.Count(e => e.action == "update" && DateTime.TryParse(e.datetime, out DateTime frequencyDateTime) && frequencyDateTime.Hour >= 9 && frequencyDateTime.Hour < 12) > 3)
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = "Frequent " + symptomname + " activity detected in the morning hours",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // New symptom detection
                if (entries.Any(e => e.action == "addNew"))
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = symptomname + " recently added. Monitoring for trends",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Worsening or improving trend detection
                var recentEntries = entries.Where(e => e.action == "update" && int.TryParse(e.value, out _)).OrderByDescending(e => DateTime.Parse(e.datetime)).Take(3).ToList();
                if (recentEntries.Count == 3)
                {
                    if (int.Parse(recentEntries[0].value) > int.Parse(recentEntries[1].value) && int.Parse(recentEntries[1].value) > int.Parse(recentEntries[2].value))
                    {
                        var newItem = new dashitem
                        {
                            ContactImage = "symptomshome.png",
                            Type = "Symptoms",
                            Title = symptomname + " intensity appears to be worsening",
                            BackgroundColor = Color.FromArgb("#fff7ea")
                        };
                        foryouuserlistsymptom.Add(newItem);
                    }
                    else if (int.Parse(recentEntries[0].value) < int.Parse(recentEntries[1].value) && int.Parse(recentEntries[1].value) < int.Parse(recentEntries[2].value))
                    {
                        var newItem = new dashitem
                        {
                            ContactImage = "symptomshome.png",
                            Type = "Symptoms",
                            Title = symptomname + " intensity appears to be improving",
                            BackgroundColor = Color.FromArgb("#fff7ea")
                        };
                        foryouuserlistsymptom.Add(newItem);
                    }
                }
                // Persistent high intensity
                var consecutiveHighIntensity = entries
                    .Where(e => e.action == "update" && int.TryParse(e.value, out int intensity) && intensity >= 50)
                    .OrderByDescending(e => DateTime.Parse(e.datetime))
                    .Take(3)
                    .Count();
                if (consecutiveHighIntensity >= 3)
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = "Persistent high intensity for " + symptomname + " reported",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Daily pattern detection
                var morningEntries = entries.Where(e => DateTime.TryParse(e.datetime, out DateTime morningDateTime) && morningDateTime.Hour >= 5 && morningDateTime.Hour < 12).Count();
                var eveningEntries = entries.Where(e => DateTime.TryParse(e.datetime, out DateTime eveningDateTime) && eveningDateTime.Hour >= 17 && eveningDateTime.Hour < 22).Count();
                if (morningEntries > 5)
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = symptomname + " commonly reported during morning hours",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                if (eveningEntries > 5)
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = symptomname + " commonly reported during evening hours",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
                // Infrequent updates
                var latestUpdate = entries.Where(e => e.action == "update").OrderByDescending(e => DateTime.Parse(e.datetime)).FirstOrDefault();
                if (latestUpdate != null && (now - DateTime.Parse(latestUpdate.datetime)).TotalDays > 7)
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = "No recent updates for " + symptomname + " in the past week",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }

                //Chris Wanted this Removed (To Suggestive)
                // Sudden intensity spike detection
                //var lastTwoEntries = entries.Where(e => e.action == "update").OrderByDescending(e => DateTime.Parse(e.datetime)).Take(2).ToList();
                //if (lastTwoEntries.Count == 2 && (int.Parse(lastTwoEntries[0].value) - int.Parse(lastTwoEntries[1].value)) > 20)
                //{
                //    var newItem = new
                //    {
                //        ContactImage = "symptomshome.png",
                //        Title = "Sudden spike in intensity detected for " + symptomname,
                //        BackgroundColor = "#fff7ea"
                //    };
                //    foryouuserlistsymptom.Add(newItem);
                //}

                // Time since first recorded entry
                var firstEntry = entries.OrderBy(e => DateTime.Parse(e.datetime)).FirstOrDefault();
                if (firstEntry != null)
                {
                    var daysSinceFirst = (now - DateTime.Parse(firstEntry.datetime)).TotalDays;
                    if (daysSinceFirst > 30)
                    {
                        var newItem = new dashitem
                        {
                            ContactImage = "symptomshome.png",
                            Type = "Symptoms",
                            Title = symptomname + " has been tracked for over " + Math.Round(daysSinceFirst) + " days",
                            BackgroundColor = Color.FromArgb("#fff7ea")
                        };
                        foryouuserlistsymptom.Add(newItem);
                    }
                }

            }

            // 1. Display the most recorded symptom in the last 3 days
            if (symptomFrequencyLast3Days.Count > 0)
            {
                var mostRecordedSymptom = symptomFrequencyLast3Days
                    .OrderByDescending(kv => kv.Value)
                    .FirstOrDefault();

                if (mostRecordedSymptom.Value > 0)
                {
                    var newItem = new dashitem
                    {
                        ContactImage = "symptomshome.png",
                        Type = "Symptoms",
                        Title = $"Most recorded symptom in the last 3 days: {mostRecordedSymptom.Key}",
                        BackgroundColor = Color.FromArgb("#fff7ea")
                    };
                    foryouuserlistsymptom.Add(newItem);
                }
            }

            // 2. Display symptoms not recorded in the last 7 days
            foreach (var symptom in symptomsNotRecordedIn7Days)
            {
                var newItem = new dashitem
                {
                    ContactImage = "symptomshome.png",
                    Type = "Symptoms",
                    Title = $"{symptom} has not been recorded in the last 7 days. Update now",
                    BackgroundColor = Color.FromArgb("#fff7ea")
                };
                foryouuserlistsymptom.Add(newItem);
            }


            Random random = new Random();

            // Select 3 random items from foryouuserlistsymptomlist
            var randomItems = foryouuserlistsymptom
                .OrderBy(x => random.Next())
                .Take(3)
                .ToList();

            // Add the selected items to foryoulist
            foryouuserlist.AddRange(randomItems);



        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void findduesupplements()
    {
        try
        {
            foryouuserlistsupps.Clear();

            //added to see if there is any due
            bool hasDueMedications = false;
            string timeoflastnotrecordedmed = "";

            int medicationsDueToday = 0;
            int recordedMedicationsToday = 0;

            if (AllUserSupplements.Count == 0)
            {
                var newItem = new dashitem
                {
                    ContactImage = "supphome.png",
                    Type = "Supplements",
                    Title = "Add your first Supplement",
                    BackgroundColor = Color.FromArgb("#f9f4e5") // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);

                nosupdataframe.IsVisible = true;

                SuppsRemain.Text = "No Scheduled Supplements";

                return;
            }

            nosupdataframe.IsVisible = false;


            foreach (var item in AllUserSupplements)
            {
                if (item.status != "Pending")
                {


                    DateTime startDate = DateTime.Parse(item.startdate);
                    DateTime? endDate = !string.IsNullOrEmpty(item.enddate) ? DateTime.Parse(item.enddate) : (DateTime?)null;
                    bool isOngoing = !endDate.HasValue || today <= endDate.Value;

                    var splitString = item.frequency.Split('|');
                    string frequencyType = splitString[0];

                    // Check if medication is due today based on frequency
                    bool isDueToday = false;

                    if (frequencyType == "Daily" && today >= startDate && isOngoing)
                    {
                        isDueToday = true;
                    }
                    else if (frequencyType == "Weekly" && today >= startDate && isOngoing)
                    {
                        // Get days of the week from frequency (e.g., "Mon,Wed,Fri")
                        var weekDays = splitString[1].Split(',');
                        var todayDayName = today.ToString("ddd");

                        // Check if today is one of the specified days
                        if (weekDays.Contains(todayDayName))
                        {
                            isDueToday = true;
                        }
                    }
                    else if (frequencyType == "Days Interval" && today >= startDate && isOngoing)
                    {
                        int dayInterval = Convert.ToInt32(splitString[1]);
                        var daysSinceStart = (today - startDate).Days;

                        // Check if today's date falls on an interval
                        if (daysSinceStart % dayInterval == 0)
                        {
                            isDueToday = true;
                        }
                    }
                    else if (frequencyType == "As Required" && item.feedback != null)
                    {
                        foreach (var feedback in item.feedback)
                        {
                            DateTime feedbackDate = DateTime.Parse(feedback.datetime);
                            if (feedbackDate.Date == today)
                            {
                                isDueToday = true;
                                break;
                            }
                        }
                    }

                    // If due today, add to the list
                    if (isDueToday)
                    {
                        hasDueMedications = true;

                        foreach (var medTime in item.schedule)
                        {
                            var time = TimeSpan.Parse(medTime.time);
                            DateTime scheduledTimeToday = DateTime.Today.Add(time); // Add time component to today's date

                            medicationsDueToday++;

                            if (scheduledTimeToday < DateTime.Now)
                            {
                                var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                                bool hasBeenRecorded = item.feedback != null && item.feedback
                                .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                                //bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                                // .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                                // If not recorded, update nextDueTime
                                if (!hasBeenRecorded)
                                {
                                    timeoflastnotrecordedmed = scheduledTimeToday.ToString("HH:mm");
                                }
                                else
                                {
                                    recordedMedicationsToday++;
                                }
                            }
                            // Check if this time is in the future and closer than the current nextDueTime
                            else if (scheduledTimeToday > DateTime.Now && (nextDueTime == null || scheduledTimeToday < nextDueTime))
                            {
                                // Check if this scheduled time has already been recorded in user feedback
                                //  bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                                //    .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                                var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                                bool hasBeenRecorded = item.feedback != null && item.feedback
                            .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                                // If not recorded, update nextDueTime
                                if (!hasBeenRecorded)
                                {
                                    nextDueTime = scheduledTimeToday;
                                }
                                else
                                {
                                    recordedMedicationsToday++;
                                }
                            }

                        }
                    }


                    if (hasDueMedications == false)
                    {
                        // Create a new item to add
                        var newItem = new dashitem
                        {
                            ContactImage = "supphome.png",
                            Type = "Supplements",
                            Title = "No Supplements Due Today",
                            BackgroundColor = Color.FromArgb("#f9f4e5") // Example color
                        };

                        // Add the new item to the list
                        foryouuserlistsupps.Add(newItem);


                        //possibly check if they are recording as required medications a lot in the last seven days

                    }

                    else if (nextDueTime == null)
                    {
                        //no more times

                        if (hasDueMedications)
                        {
                            //check if they have added feedback for previous meds for that day 


                            if (!string.IsNullOrEmpty(timeoflastnotrecordedmed))
                            {
                                var nnewItem = new dashitem
                                {
                                    ContactImage = "supphome.png",
                                    Type = "Supplements",
                                    Title = "Have you recorded your " + timeoflastnotrecordedmed + " Supplements?",
                                    BackgroundColor = Color.FromArgb("#f9f4e5") // Example color
                                };

                                // Add the new item to the list
                                foryouuserlistsupps.Add(nnewItem);
                            }


                            var newItem = new dashitem
                            {
                                ContactImage = "supphome.png",
                                Type = "Supplements",
                                Title = "No More Supplements Due Today",
                                BackgroundColor = Color.FromArgb("#f9f4e5") // Example color
                            };

                            // Add the new item to the list
                            foryouuserlistsupps.Add(newItem);
                        }

                    }
                    else
                    {

                        // Create a new item to add
                        var newItem = new dashitem
                        {
                            ContactImage = "supphome.png",
                            Type = "Supplements",
                            Title = "Record your " + nextDueTime.Value.ToString("HH:mm") + " Supplements",
                            BackgroundColor = Color.FromArgb("#f9f4e5") // Example color
                        };

                        // Add the new item to the list
                        foryouuserlistsupps.Add(newItem);

                    }
                }
            }

            double progress = medicationsDueToday > 0 ? (double)recordedMedicationsToday / medicationsDueToday * 100 : 0;

            // Update the progress bar value
            suppprogressbar.Progress = progress;

            var SuppsLeft = medicationsDueToday - recordedMedicationsToday;
            if (SuppsLeft > 0)
            {
                var SuppsRem = SuppsLeft + " " + "Supplements Remaining";
                SuppsRemain.Text = SuppsRem;

                //var newdaily = new dashitem();

                //newdaily.Title = SuppsLeft + " Supplements Remaining";
                //dailytasklist.Add(newdaily);

            }
            else
            {
                var SuppsRem = "All Supplements Recorded";
                SuppsRemain.Text = SuppsRem;
            }

            Random random = new Random();

            // Select 3 random items from foryouuserlistsymptomlist
            var randomItems = foryouuserlistsupps
                .OrderBy(x => random.Next())
                .Take(1)
                .ToList();

            foryouuserlist.AddRange(randomItems);

            //check if there any as required supplements
            if (AllUserSupplements.Any(x => x.frequency != null && x.frequency.Contains("As Required")))
            {

                var asRequiredMeds = AllUserSupplements
       .Where(x => x.frequency.Contains("As Required"))
       .ToList();

                var randomm = new Random();
                var randomMed = asRequiredMeds[randomm.Next(asRequiredMeds.Count)];

                var newItem = new dashitem
                {
                    ContactImage = "supphome.png",
                    Type = "Supplements",
                    Title = "Have you taken any " + randomMed.supplementtitle,
                    BackgroundColor = Color.FromArgb("#f9f4e5") // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);
            }


            if (setnotificationsfromlogin)
            {
                if (SuppNotifAdded == true) return;

                var daycount = 0;
                var mednottitle = "Supplement Reminder";

                foreach (var item in AllUserSupplements)
                {

                    if (!string.IsNullOrEmpty(item.enddate))
                    {
                        if (!item.frequency.Contains("As Required"))
                        {
                            if (DateTime.Now.Date > DateTime.Parse(item.enddate).Date)
                            {
                                //Don't Add/ Run through Process
                                continue;
                            }
                        }
                    }

                    foreach (var it in item.schedule)
                    {

                        //Random randomm = new Random();
                        //int randomNumberr = randomm.Next(100000, 100000001);
                        //it.id = randomNumberr;

                        //Handle Edited Supplement with Active true 

                        if (it.active != null)

                        {
                            if (it.active != "true")
                            {
                                //Skip as its the Previous Version
                                continue;
                            }
                        }


                        var timeconverted = TimeSpan.Parse(it.time);



                        if (item.frequency.Contains("Daily"))
                        {
                            if (string.IsNullOrEmpty(item.enddate))
                            {
                                await ScheduleNotifications.DailyNotifications(mednottitle, it.id, item.supplementtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate);
                            }
                            else
                            {
                                await ScheduleNotifications.DailyWithEndDateNotifications(mednottitle, it.id, item.supplementtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, item.enddate);
                            }


                        }
                        else if (item.frequency.Contains("Days Interval"))
                        {

                            var splitfrequency = item.frequency.Split('|');
                            var DIdaycount = Convert.ToInt32(splitfrequency[1]);



                            if (string.IsNullOrEmpty(item.enddate))
                            {
                                await ScheduleNotifications.DaysIntervalNotifications(mednottitle, it.id, item.supplementtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, DIdaycount);
                            }
                            else
                            {
                                await ScheduleNotifications.DaysIntervalWithEndDateNotifications(mednottitle, it.id, item.supplementtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, item.enddate, DIdaycount);
                            }
                        }
                        else if (item.frequency.Contains("Weekly"))
                        {
                            var splitfrequency = item.frequency.Split("|");

                            var daylist = new List<string>();
                            //means there is multiple days so loop through list
                            if (splitfrequency[1].Contains(','))
                            {
                                // Split the string by commas and trim any whitespace
                                var days = splitfrequency[1].Split(',').Select(day => day.Trim());

                                // Add each day to the daylist
                                daylist.AddRange(days);
                            }
                            else
                            {
                                // If there's only one day, add it directly
                                daylist.Add(splitfrequency[1].Trim());
                            }



                            foreach (var wday in daylist)
                            {
                                if (string.IsNullOrEmpty(item.enddate))
                                {
                                    await ScheduleNotifications.WeeklyNotifications(mednottitle, it.id, item.supplementtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, wday);
                                }
                                else
                                {
                                    await ScheduleNotifications.WeeklyWithEndDateNotifications(mednottitle, it.id, item.supplementtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, wday, item.enddate);
                                }

                            }


                        }


                    }
                }

                //Added to stop The Code Running again 
                SuppNotifAdded = true;

            }


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void findduemedications()
    {
        try
        {
            foryouuserlistmeds.Clear();

            //added to see if there is any due
            bool hasDueMedications = false;
            string timeoflastnotrecordedmed = "";

            int medicationsDueToday = 0;
            int recordedMedicationsToday = 0;

            if (AllUserMedications.Count == 0)
            {
                var newItem = new dashitem
                {
                    ContactImage = "medicinehome.png",
                    Type = "Medications",
                    Title = "Add your first Medication",
                    BackgroundColor = Color.FromArgb("#e5f9f4") // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);

                nomeddataframe.IsVisible = true;

                MedsRemain.Text = "No Scheduled Medications";

                return;
            }

            nomeddataframe.IsVisible = false;

            foreach (var item in AllUserMedications)
            {
                if (item.status != "Pending")
                {


                    DateTime startDate = DateTime.Parse(item.startdate);
                    DateTime? endDate = !string.IsNullOrEmpty(item.enddate) ? DateTime.Parse(item.enddate) : (DateTime?)null;
                    bool isOngoing = !endDate.HasValue || today <= endDate.Value;

                    var splitString = item.frequency.Split('|');
                    string frequencyType = splitString[0];

                    // Check if medication is due today based on frequency
                    bool isDueToday = false;

                    if (frequencyType == "Daily" && today >= startDate && isOngoing)
                    {
                        isDueToday = true;
                    }
                    else if (frequencyType == "Weekly" && today >= startDate && isOngoing)
                    {
                        // Get days of the week from frequency (e.g., "Mon,Wed,Fri")
                        var weekDays = splitString[1].Split(',');
                        var todayDayName = today.ToString("ddd");

                        // Check if today is one of the specified days
                        if (weekDays.Contains(todayDayName))
                        {
                            isDueToday = true;
                        }
                    }
                    else if (frequencyType == "Days Interval" && today >= startDate && isOngoing)
                    {
                        int dayInterval = Convert.ToInt32(splitString[1]);
                        var daysSinceStart = (today - startDate).Days;

                        // Check if today's date falls on an interval
                        if (daysSinceStart % dayInterval == 0)
                        {
                            isDueToday = true;
                        }
                    }
                    else if (frequencyType == "As Required" && item.feedback != null)
                    {
                        foreach (var feedback in item.feedback)
                        {
                            DateTime feedbackDate = DateTime.Parse(feedback.datetime);
                            if (feedbackDate.Date == today)
                            {
                                isDueToday = true;
                                break;
                            }
                        }
                    }

                    // If due today, add to the list
                    if (isDueToday)
                    {
                        hasDueMedications = true;

                        foreach (var medTime in item.schedule)
                        {
                            medicationsDueToday++;

                            var time = TimeSpan.Parse(medTime.time);
                            DateTime scheduledTimeToday = DateTime.Today.Add(time); // Add time component to today's date

                            if (scheduledTimeToday < DateTime.Now)
                            {
                                var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                                bool hasBeenRecorded = item.feedback != null && item.feedback
                                .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                                //bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                                // .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                                // If not recorded, update nextDueTime
                                if (!hasBeenRecorded)
                                {
                                    timeoflastnotrecordedmed = scheduledTimeToday.ToString("HH:mm");
                                }
                                else
                                {
                                    recordedMedicationsToday++;
                                }
                            }
                            // Check if this time is in the future and closer than the current nextDueTime
                            else if (scheduledTimeToday > DateTime.Now && (nextDueTime == null || scheduledTimeToday < nextDueTime))
                            {
                                // Check if this scheduled time has already been recorded in user feedback
                                //  bool hasBeenRecorded = item.feedback != null && item.feedback.Where(x => x.id == medTime.Feedbackid)
                                //    .Any(feedback => feedback.datetime.Contains(scheduledTimeToday.ToString("HH:mm, dd/MM/yyyy")));

                                var dt = scheduledTimeToday.ToString("dd/MM/yyyy");

                                bool hasBeenRecorded = item.feedback != null && item.feedback
                            .Any(x => x.id == medTime.id.ToString() && x.datetime.Contains(dt));

                                // If not recorded, update nextDueTime
                                if (!hasBeenRecorded)
                                {
                                    nextDueTime = scheduledTimeToday;
                                }
                                else
                                {
                                    recordedMedicationsToday++;
                                }
                            }

                        }
                    }
                    //double progress = medicationsDueToday > 0 ? (double)recordedMedicationsToday / medicationsDueToday * 100 : 0;

                    //// Update the progress bar value
                    //medprogressbar.Progress = progress;

                    //var SuppsLeft = medicationsDueToday - recordedMedicationsToday;
                    //if (SuppsLeft > 0)
                    //{
                    //    var SuppsRem = SuppsLeft + " " + "Medications Remaining";
                    //    MedsRemain.Text = SuppsRem;

                    //    var newdaily = new dashitem();

                    //    newdaily.Title = SuppsLeft + " Medications Remaining";
                    //    dailytasklist.Add(newdaily);

                    //}
                    //else
                    //{
                    //    var SuppsRem = "All Medications Recorded";
                    //    MedsRemain.Text = SuppsRem;
                    //}

                    // Update the gradient stops
                    //                var gradientBrush = new LinearGradientBrush
                    //                {
                    //                    StartPoint = new Point(0, 0),
                    //                    EndPoint = new Point(1, 0),
                    //                    GradientStops =
                    //{
                    //    // Teal fills up to the progress point
                    //    new GradientStop { Color = Colors.Teal, Offset = (float)Math.Max(progress, 0.001f) },
                    //    // #e5f9f4 fills the remainder
                    //    //new GradientStop { Color = Color.FromHex("#e5f9f4"), Offset = 1 }
                    //}
                    //                };

                    //// If progress is 0, set Due colour to all 
                    //if (progress == 0)
                    //{
                    //    gradientBrush.GradientStops.Clear();
                    //    gradientBrush.GradientStops.Add(new GradientStop { Color = Color.FromHex("#e5f9f4"), Offset = 1 });
                    //}

                    // Apply the gradient
                    //medprogressbar.ProgressFill = gradientBrush;

                    //if (medicationsDueToday > 0)
                    //{
                    //    // Calculate the percentage of recorded medications for the progress bar
                    //    if(recordedMedicationsToday > 0)
                    //    {
                    //        double percentageRecorded = medicationsDueToday > 0 ? (double)recordedMedicationsToday / medicationsDueToday * 100 : 0;

                    //        medprogressbar.Progress = percentageRecorded;
                    //    }
                    //    else
                    //    {

                    //        medprogressbar.Progress = medicationsDueToday;
                    //    }

                    //}
                    //else
                    //{
                    //    // No medications are due, set progress to 100
                    //    medprogressbar.Progress = 100;
                    //}


                    if (hasDueMedications == false)
                    {
                        // Create a new item to add
                        var newItem = new dashitem
                        {
                            ContactImage = "medicinehome.png",
                            Type = "Medications",
                            Title = "No Medications Due Today",
                            BackgroundColor = Color.FromArgb("#e5f9f4") // Example color
                        };

                        // Add the new item to the list
                        foryouuserlistmeds.Add(newItem);

                        //possibly check if they are recording as required medications a lot in the last seven days

                    }

                    else if (nextDueTime == null)
                    {
                        //no more times

                        if (hasDueMedications)
                        {
                            //check if they have added feedback for previous meds for that day 


                            if (!string.IsNullOrEmpty(timeoflastnotrecordedmed))
                            {
                                var nnewItem = new dashitem
                                {
                                    ContactImage = "medicinehome.png",
                                    Type = "Medications",
                                    Title = "Have you recorded your " + timeoflastnotrecordedmed + " Medications?",
                                    BackgroundColor = Color.FromArgb("#e5f9f4") // Example color
                                };

                                // Add the new item to the list
                                foryouuserlistmeds.Add(nnewItem);
                            }


                            var newItem = new dashitem
                            {
                                ContactImage = "medicinehome.png",
                                Type = "Medications",
                                Title = "No More Medications Due Today",
                                BackgroundColor = Color.FromArgb("#e5f9f4") // Example color
                            };

                            // Add the new item to the list
                            foryouuserlistmeds.Add(newItem);
                        }

                    }
                    else
                    {

                        // Create a new item to add
                        var newItem = new dashitem
                        {
                            ContactImage = "medicinehome.png",
                            Type = "Medications",
                            Title = "Record your " + nextDueTime.Value.ToString("HH:mm") + " Medications",
                            BackgroundColor = Color.FromArgb("#e5f9f4") // Example color
                        };

                        // Add the new item to the list
                        foryouuserlistmeds.Add(newItem);


                    }
                }

            }

            double progress = medicationsDueToday > 0 ? (double)recordedMedicationsToday / medicationsDueToday * 100 : 0;

            // Update the progress bar value
            medprogressbar.Progress = progress;

            var SuppsLeft = medicationsDueToday - recordedMedicationsToday;
            if (SuppsLeft > 0)
            {
                var SuppsRem = SuppsLeft + " " + "Medications Remaining";
                MedsRemain.Text = SuppsRem;

                //var newdaily = new dashitem();

                //newdaily.Title = SuppsLeft + " Medications Remaining";
                //dailytasklist.Add(newdaily);

            }
            else
            {
                var SuppsRem = "All Medications Recorded";
                MedsRemain.Text = SuppsRem;
            }

            Random random = new Random();

            // Select 3 random items from foryouuserlistsymptomlist
            var randomItems = foryouuserlistmeds
                .OrderBy(x => random.Next())
                .Take(1)
                .ToList();

            foryouuserlist.AddRange(randomItems);


            //check if there any as required medications
            if (AllUserMedications.Any(x => x.frequency != null && x.frequency.Contains("As Required")))
            {

                var asRequiredMeds = AllUserMedications
       .Where(x => x.frequency != null && x.frequency.Contains("As Required"))
       .ToList();

                var randomm = new Random();
                var randomMed = asRequiredMeds[randomm.Next(asRequiredMeds.Count)];

                var newItem = new dashitem
                {
                    ContactImage = "medicinehome.png",
                    Type = "Medications",
                    Title = "Have you taken any " + randomMed.medicationtitle,
                    BackgroundColor = Color.FromArgb("#e5f9f4") // Example color
                };

                // Add the new item to the list
                foryouuserlist.Add(newItem);
            }



            //add the med notifications if they have logged in again
            if (setnotificationsfromlogin)
            {
                if (MedNotifAdded == true) return;
                var daycount = 0;
                var mednottitle = "Medication Reminder";

                foreach (var item in AllUserMedications)
                {
                    if (!string.IsNullOrEmpty(item.enddate))
                    {
                        if (!item.frequency.Contains("As Required"))
                        {
                            if (DateTime.Now.Date > DateTime.Parse(item.enddate).Date)
                            {
                                //Don't Add/ Run through Process
                                continue;
                            }
                        }
                    }

                    foreach (var it in item.schedule)
                    {

                        //Random randomm = new Random();
                        //int randomNumberr = randomm.Next(100000, 100000001);

                        //it.id = randomNumberr;


                        //Handle Edited Medication with Active true 

                        if (it.active != null)
                        {
                            if (it.active != "true")
                            {
                                //Skip as its the Previous Version
                                continue;
                            }
                        }


                        var timeconverted = TimeSpan.Parse(it.time);



                        if (item.frequency.Contains("Daily"))
                        {
                            if (string.IsNullOrEmpty(item.enddate))
                            {
                                await ScheduleNotifications.DailyNotifications(mednottitle, it.id, item.medicationtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate);
                            }
                            else
                            {
                                await ScheduleNotifications.DailyWithEndDateNotifications(mednottitle, it.id, item.medicationtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, item.enddate);
                            }


                        }
                        else if (item.frequency.Contains("Days Interval"))
                        {

                            var splitfrequency = item.frequency.Split('|');
                            var DIdaycount = Convert.ToInt32(splitfrequency[1]);



                            if (string.IsNullOrEmpty(item.enddate))
                            {
                                await ScheduleNotifications.DaysIntervalNotifications(mednottitle, it.id, item.medicationtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, DIdaycount);
                            }
                            else
                            {
                                await ScheduleNotifications.DaysIntervalWithEndDateNotifications(mednottitle, it.id, item.medicationtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, item.enddate, DIdaycount);
                            }
                        }
                        else if (item.frequency.Contains("Weekly"))
                        {
                            var splitfrequency = item.frequency.Split("|");

                            var daylist = new List<string>();
                            //means there is multiple days so loop through list
                            if (splitfrequency[1].Contains(','))
                            {
                                // Split the string by commas and trim any whitespace
                                var days = splitfrequency[1].Split(',').Select(day => day.Trim());

                                // Add each day to the daylist
                                daylist.AddRange(days);
                            }
                            else
                            {
                                // If there's only one day, add it directly
                                daylist.Add(splitfrequency[1].Trim());
                            }



                            foreach (var wday in daylist)
                            {
                                if (string.IsNullOrEmpty(item.enddate))
                                {
                                    await ScheduleNotifications.WeeklyNotifications(mednottitle, it.id, item.medicationtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, wday);
                                }
                                else
                                {
                                    await ScheduleNotifications.WeeklyWithEndDateNotifications(mednottitle, it.id, item.medicationtitle, it.Dosage, it.dosageunit, timeconverted, item.startdate, wday, item.enddate);
                                }

                            }


                        }


                    }
                }

                //Added to Stop Supps from Activiting Again 
                MedNotifAdded = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void loadcatergories()
    {
        try
        {

            // Sentry Checker
            //SentrySdk.CaptureMessage("Hello Sentry");

            // Add all categories for help videos
            var allcatvideolist = new List<dashitem>
            {
                new dashitem { Type = "Symptoms", ContactImage = "symptomshome.png", Title = "Symptoms", BackgroundColor = Color.FromArgb("#fff7ea") },
                new dashitem { Type = "Medications", ContactImage = "medicinehome.png", Title = "Medications", BackgroundColor = Color.FromArgb("#e5f9f4") },
                new dashitem {Type = "Schedule",   ContactImage = "schedulehome.png", Title = "Schedule", BackgroundColor = Color.FromArgb("#eff7ed") },
                new dashitem { Type = "Supplements", ContactImage = "supphome.png", Title = "Supplements", BackgroundColor = Color.FromArgb("#f9f4e5") },
                new dashitem { Type = "Measurements",  ContactImage = "measurementhome.png", Title = "Measurements", BackgroundColor = Color.FromArgb("#e5f0fb") },
                new dashitem {Type = "Diagnosis",  ContactImage = "diagnosishome.png", Title = "Diagnosis", BackgroundColor = Color.FromArgb("#E6E6FA") },
                new dashitem { Type = "Mood", ContactImage = "moodhome.png", Title = "Mood", BackgroundColor = Color.FromArgb("#FFF8DC") },
                new dashitem { Type = "Diet", ContactImage ="diethome.png", Title = "Diet", BackgroundColor = Color.FromArgb("#e8efd8") },
                new dashitem { Type = "Investigations", ContactImage ="investhome.png", Title = "Investigations", BackgroundColor = Color.FromArgb("#F5E6E8") },
                new dashitem { Type = "Activity", ContactImage ="activityhome.png", Title = "Daily Activity", BackgroundColor = Color.FromArgb("#fce9d9") },
                //new dashitem { Type = "Exercise", ContactImage ="investhome.png", Title = "Exercise/Activity", BackgroundColor = Color.FromArgb("#FDBA74") },
                //new dashitem { Type = "Food Diary", ContactImage ="fooddiaryhome.png", Title = "Food Diary", BackgroundColor = Color.FromArgb("#ECE5C1") },
                new dashitem { Type = "Appointments",  ContactImage = "appointhome.png", Title = "Appointments", BackgroundColor = Color.FromArgb("#ffe4e1") },
                new dashitem {Type = "HCP",  ContactImage = "hcphome.png", Title = "HCPs", BackgroundColor = Color.FromArgb("#CBC3E3") },
                new dashitem { Type = "Questionnaires", ContactImage = "questionnairehome.png", Title = "Questionnaires", BackgroundColor = Color.FromArgb("#fff9ec") },
                new dashitem { Type = "Allergy",  ContactImage = "allergenhome.png", Title = "Allergens", BackgroundColor = Color.FromArgb("#FFF5EE") },
                new dashitem { Type = "Health Report",  ContactImage = "healthreporticon.png", Title = "Health Report", BackgroundColor = Color.FromArgb("#ededed") },

            };

            if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                var signup = Helpers.Settings.SignUp;

                var Array = new List<string>{ "Appointments", "HCP"};

                if (signup.Contains("RBHTHCM"))
                {
                    allcatvideolist = allcatvideolist.Where(x => !Array.Contains(x.Type)).ToList();
                }
            }



            allhelpvideocatlist.ItemsSource = allcatvideolist;

            var extendedCatvideolist = new List<dashitem>(allcatvideolist)
            {
              new dashitem { Type = "Videos", ContactImage = "videoicon.png", Title = "Help Videos", BackgroundColor = Color.FromArgb("#e9e9e9") },
              new dashitem { Type = "Profile", ContactImage = "profileicon.png", Title = "Profile", BackgroundColor = Color.FromArgb("#deeff5") }
            };

            // Set the extended list as the data source for catergorieslist
            catergorieslist.ItemsSource = extendedCatvideolist;


            var listforaccountlist = new List<dashitem>
{
    new dashitem { Title = "Account Settings" },
    new dashitem { Title = "Developer Feedback & Support" },
    new dashitem { Title = "Terms Of Use" }
};

            // Set the height of the account list based on the item count
            accountlist.HeightRequest = listforaccountlist.Count * 48;
            accountlist.ItemsSource = listforaccountlist;

            //foryouuserlist = new List<object>
            //{
            //   // new { ContactImage = "symptomshome.png", Title = "Update Backache", BackgroundColor = "#fff7ea" },
            //    new { ContactImage = "healthreporticon.png", Title = "Generate your Health Report", BackgroundColor = "#e5f5fc" },
            //    new { ContactImage = "diagnosishome.png", Title = "Have you received a new diagnosis ?", BackgroundColor = "#E6E6FA" },
            //    new { ContactImage = "appointhome.png", Title = "Record a new appointment", BackgroundColor = "#ffcccb" },

            //};

            // activitylist.ItemsSource = foryouuserlist;

            //checknotifications();

            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //{
            //    CheckbatterySaverON(); 

            //}
            UserSettingsCheck();

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                healthgrid.IsVisible = false;
            }
            else
            {
                //hidden for now
                healthgrid.IsVisible = false;
            }


            //CreateSettings();

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //async void checknotifications()
    //{
    //    try
    //    {

    //        var check = await LocalNotificationCenter.Current.AreNotificationsEnabled();

    //        if (!check)
    //        {
    //            //pushnotifcationsframe.IsVisible = true;
    //            NoficiationsActive = false; 
    //        }
    //        else
    //        {
    //            //pushnotifcationsframe.IsVisible = false;
    //            NoficiationsActive = true;
    //        }

    //    }
    //    catch(Exception ex)
    //    {

    //    }
    //}

    //async void CheckbatterySaverON()
    //{
    //    try
    //    {

    //        var status = Battery.EnergySaverStatus;

    //        if (status == EnergySaverStatus.On)
    //        {
    //            //AndroidbatteryOptimise.IsVisible = true;
    //            bool BatterySaverOff = false;
    //        }
    //        else if (status == EnergySaverStatus.Off)
    //        {
    //            //AndroidbatteryOptimise.IsVisible = false;
    //            bool BatterySaverOff = true;
    //        }
    //        else
    //        {
    //            //Ignore Unknown Status 
    //            //AndroidbatteryOptimise.IsVisible = false;
    //            bool BatterySaverOff = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    async void UserSettingsCheck()
    {
        try
        {
            SettingstoShow.Clear();
            //Add Items 
            var Alerts = new SettingsOn
            {
                img = "pushnoticon.png",
                title = "Turn On Notifications",
                description = "Stay on top of your health journey with timely reminders! By turning on notifications for our app, you'll never miss important updates.",
                guide = "Turn On"
            };

            var Saver = new SettingsOn
            {
                img = "batterysaver.png",
                title = "Turn Off Battery Saver",
                description = "Battery Saver may block notifications when the app is closed. To avoid missing alerts, please disable it.",
                guide = "Turn Off"
            };

            //Check Notifications Enabled

            var check = await LocalNotificationCenter.Current.AreNotificationsEnabled();

            if (!check)
            {
                NoficiationsActive = false;
            }
            else
            {
                NoficiationsActive = true;
            }

            //Check batter Saver Off (Android)
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                var status = Battery.EnergySaverStatus;

                if (status == EnergySaverStatus.On)
                {
                    BatterySaverOff = false;
                }
                else if (status == EnergySaverStatus.Off)
                {
                    BatterySaverOff = true;
                }
                else
                {
                    //Status Unknown Set to Faslse
                    BatterySaverOff = false;
                }
            }

            //Add To Settings List 
            if (NoficiationsActive == false)
            {
                SettingstoShow.Add(Alerts);
            }

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                if (BatterySaverOff == false)
                {
                    SettingstoShow.Add(Saver);
                }
            }


            //Show hide Items 
            if (SettingstoShow.Count > 0)
            {
                if (SettingstoShow.Count > 1)
                {
                    SettingsIND.IsVisible = true;
                    SettingsCarousel.IsSwipeEnabled = true;
                }
                else
                {
                    SettingsIND.IsVisible = false;
                    SettingsCarousel.IsSwipeEnabled = false;
                }
                SettingsCarousel.ItemsSource = SettingstoShow;
                SettingsPrompt.IsVisible = true;
            }
            else
            {
                SettingsPrompt.IsVisible = false;
                SettingsIND.IsVisible = false;
            }

        }
        catch (Exception ex)
        {

        }
    }

    async private void Checkifuserhasmigrated()
    {
        try
        {
            // UserDetails = await database.GetuserDetails();

            //string um = Preferences.Default.Get("usermigrated", "false");

            //if (um == "True" || string.IsNullOrEmpty(um) || um == "False")
            //{

            //        //go to migration assitant

            //        await Navigation.PushAsync(new MigrationAssistant(), false);
            //        return;                              
            //}

            // Assuming UserDetails[0].dateofbirth is a string in a format like "yyyy-MM-dd"
            string dateOfBirthString = Helpers.Settings.Age;

            if (string.IsNullOrEmpty(dateOfBirthString) || dateOfBirthString == "")
            {
                // Display the age in the label
                agelbl.Text = "--";
            }
            else
            {
                // Convert the string to DateTime
                DateTime dateOfBirth = DateTime.Parse(dateOfBirthString);

                if (dateOfBirth.Date.ToString("dd/MM/yyyy") == "01/01/1900")
                {
                    agelbl.Text = "--";
                }
                else
                {
                    // Calculate the age
                    int age = DateTime.Now.Year - dateOfBirth.Year;

                    // Check if the birthday has occurred this year, if not subtract 1
                    if (DateTime.Now < dateOfBirth.AddYears(age))
                    {
                        age--;
                    }

                    // Display the age in the label
                    agelbl.Text = age.ToString();
                }

            }


            //AllUserDetails.gender = UserDetails[0].gender;
            //Helpers.Settings.Gender = UserDetails[0].gender;

            if (String.IsNullOrEmpty(Helpers.Settings.Gender))
            {
                genderlbl.Text = "--";
            }
            else
            {
                genderlbl.Text = Helpers.Settings.Gender;
            }


            //   AllUserDetails.ethnicity = UserDetails[0].ethnicity;
            //  Helpers.Settings.Ethnicity = UserDetails[0].ethnicity;

            if (String.IsNullOrEmpty(Helpers.Settings.Ethnicity))
            {
                ethlbl.Text = "--";
            }
            else
            {
                ethlbl.Text = Helpers.Settings.Ethnicity;
            }



        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void tabsview_TabItemTapped(object sender, Syncfusion.Maui.TabView.TabItemTappedEventArgs e)
    {
        try
        {
            if (e.TabItem.Header == "Home")
            {

                hometab.ImageSource = ImageSource.FromFile("dashiconactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreinactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseinactive.png");
                additionalquestionstab.ImageSource = ImageSource.FromFile("questiondashgrey.png");

                hometab.TextColor = Color.FromArgb("#031926");
                infotab.TextColor = Color.FromArgb("#b3babd");
                profiletab.TextColor = Color.FromArgb("#b3babd");
                additionalquestionstab.TextColor = Color.FromArgb("#b3babd");


                hometab.FontFamily = "HankenGroteskBold";
                infotab.FontFamily = "HankenGroteskRegular";
                profiletab.FontFamily = "HankenGroteskRegular";
                additionalquestionstab.FontFamily = "HankenGroteskRegular";
            }
            else if (e.TabItem.Header == "Explore")
            {
                hometab.ImageSource = ImageSource.FromFile("dashiconinactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseinactive.png");
                additionalquestionstab.ImageSource = ImageSource.FromFile("questiondashgrey.png");

                infotab.TextColor = Color.FromArgb("#031926");
                hometab.TextColor = Color.FromArgb("#b3babd");
                profiletab.TextColor = Color.FromArgb("#b3babd");
                additionalquestionstab.TextColor = Color.FromArgb("#b3babd");

                infotab.FontFamily = "HankenGroteskBold";
                hometab.FontFamily = "HankenGroteskRegular";
                profiletab.FontFamily = "HankenGroteskRegular";
                additionalquestionstab.FontFamily = "HankenGroteskRegular";
            }
            else if (e.TabItem.Header == "Browse")
            {
                hometab.ImageSource = ImageSource.FromFile("dashiconinactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreinactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseactive.png");
                additionalquestionstab.ImageSource = ImageSource.FromFile("questiondashgrey.png");

                profiletab.TextColor = Color.FromArgb("#031926");
                infotab.TextColor = Color.FromArgb("#b3babd");
                hometab.TextColor = Color.FromArgb("#b3babd");
                additionalquestionstab.TextColor = Color.FromArgb("#b3babd");


                profiletab.FontFamily = "HankenGroteskBold";
                hometab.FontFamily = "HankenGroteskRegular";
                infotab.FontFamily = "HankenGroteskRegular";
                additionalquestionstab.FontFamily = "HankenGroteskRegular";


            }
            else if (e.TabItem.Header == "Questions")
            {
                profiletab.TextColor = Color.FromArgb("#b3babd");
                infotab.TextColor = Color.FromArgb("#b3babd");
                hometab.TextColor = Color.FromArgb("#b3babd");
                additionalquestionstab.TextColor = Color.FromArgb("#031926");


                profiletab.FontFamily = "HankenGroteskRegular";
                hometab.FontFamily = "HankenGroteskRegular";
                infotab.FontFamily = "HankenGroteskRegular";
                additionalquestionstab.FontFamily = "HankenGroteskBold";


                hometab.ImageSource = ImageSource.FromFile("dashiconinactive.png");
                infotab.ImageSource = ImageSource.FromFile("dashexploreinactive.png");
                profiletab.ImageSource = ImageSource.FromFile("dashbrowseinactive.png");
                additionalquestionstab.ImageSource = ImageSource.FromFile("questiondashblack.png");
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void allhelpvideocatlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as dashitem;
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            if (IsNavigating) return;
            IsNavigating = true;
            if (item != null && item.Title == "Medications")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoMeds", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllMedications(), false);
                }
            }

            else if (item != null && item.Title == "Symptoms")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoSyms", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
                }
            }
            else if (item != null && item.Title == "Supplements")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoSupps", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllSupplements(), false);
                }
            }
            else if (item != null && item.Title == "Measurements")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoMeas", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
                }
            }
            else if (item != null && item.Title == "Diagnosis")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoDiag", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllDiagnosis(), false);
                }
            }
            else if (item != null && item.Title == "Mood")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoMood", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
                }
            }
            else if (item != null && item.Title == "Appointments")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoAppt", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllAppointments(), false);
                }
            }
            else if (item != null && item.Title == "HCPs")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoHcp", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new HCPs(), false);
                }
            }
            else if (item != null && item.Title == "Questionnaires")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoQues", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllQuestionnaires(), false);
                }
            }
            else if (item != null && item.Title == "Allergens")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoAllerg", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllAllergies(), false);
                }
            }
            else if (item != null && item.Title == "Health Report")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoHeRep", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new HealthReport(), false);
                }
            }
            else if (item != null && item.Title == "Schedule")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoSched", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new MainSchedule(), false);
                }

            }
            else if (item != null && item.Title == "Food Diary")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoFood", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllFoodDiary(), false);
                }

            }
            else if (item != null && item.Title == "Diet")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoDiet", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllDiet(), false);
                }

            }
            else if (item != null && item.Title == "Investigations")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoInvest", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllInvestigations(), false);
                }

            }
            else if (item != null && item.Title == "Daily Activity")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoActivity", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new ActivitySchedule(), false);
                }

            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
            //await Navigation.PushAsync(new ErrorPage()) ,false);  
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {

            //see more button
            if (morebtn.Text == "See more")
            {
                signupdetailslbl.Text = signupcodecollection[0].appdescription;
                morebtn.Text = "See less";

            }
            else
            {
                signupdetailslbl.Text = signupcodecollection[0].shortdescription;
                morebtn.Text = "See more";
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void infolist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as signupcodeinformation;
            if (IsNavigating) return;
            IsNavigating = true;
            item.type = item.type.ToLower();

            if (item.type == "pdf")
            {
                var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;
                await Browser.OpenAsync(pdflink, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide
                });
            }
            else if (item.type == "video")
            {
                var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;
                string imgPath = pdflink + ".mp4";
                //  var launchvid = new videosupport();
                // launchvid.URL = item.Filename;
                // await Navigation.PushAsync(new AllVideos(), false);

                // var vid = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/DiagnosisFirstAdd.mp4";
                bool FromDash = true;
                await Navigation.PushAsync(new NewPageVideoPlayer(item, FromDash), false);
            }
            else if (item.type == "image")
            {
                var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;

                await MopupService.Instance.PushAsync(new imagePopUp(pdflink) { });

            }
            else
            {
                await Browser.OpenAsync(item.link, BrowserLaunchMode.SystemPreferred);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    async private void HealthReportBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Health Report";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoHeRep", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new HealthReport(), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    async private void QuestionBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Questionnaires";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoQues", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllQuestionnaires(), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //profile section tapped 

        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Navigation.PushAsync(new ProfileSection(), false);
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void catergorieslist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as dashitem;

            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            if (IsNavigating) return;
            IsNavigating = true;

            if (item != null && item.Title == "Medications")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoMeds", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllMedications(), false);
                }
            }

            else if (item != null && item.Title == "Symptoms")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoSyms", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
                }
            }
            else if (item != null && item.Title == "Supplements")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoSupps", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllSupplements(), false);
                }
            }
            else if (item != null && item.Title == "Measurements")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoMeas", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
                }
            }
            else if (item != null && item.Title == "Diagnosis")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoDiag", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllDiagnosis(), false);
                }
            }
            else if (item != null && item.Title == "Mood")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoMood", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
                }
            }
            else if (item != null && item.Title == "Appointments")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoAppt", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllAppointments(), false);
                }
            }
            else if (item != null && item.Title == "HCPs")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoHcp", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new HCPs(), false);
                }
            }
            else if (item != null && item.Title == "Questionnaires")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoQues", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllQuestionnaires(), false);
                }
            }
            else if (item != null && item.Title == "Allergens")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoAllerg", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllAllergies(), false);
                }
            }
            else if (item != null && item.Title == "Health Report")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoHeRep", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new HealthReport(), false);
                }
            }
            else if (item != null && item.Title == "Schedule")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoSched", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new MainSchedule(), false);
                }

            }
            else if (item != null && item.Title == "Help Videos")
            {
                await Navigation.PushAsync(new AllVideos(), false);
            }
            else if (item != null && item.Title == "Profile")
            {
                await Navigation.PushAsync(new ProfileSection(), false);
            }
            else if (item != null && item.Title == "Food Diary")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoFood", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllFoodDiary(), false);
                }

            }
            else if (item != null && item.Title == "Diet")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoDiet", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllDiet(), false);
                }

            }

            else if (item != null && item.Title == "Investigations")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoInvest", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new AllInvestigations(), false);
                }

            }

            else if (item != null && item.Title == "Daily Activity")
            {
                string Area = item.Title;
                bool Check = Preferences.Default.Get("NovoActivity", false);
                if (Check && NovoSignup)
                {
                    await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                }
                else
                {
                    await Navigation.PushAsync(new ActivitySchedule(), false);
                }

            }

            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            //fix here 
            //no symptom data button click
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Symptoms";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoSyms", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        try
        {
            //no symptom data button click
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Mood";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoMood", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void accountlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as dashitem;
            if (IsNavigating) return;
            IsNavigating = true;

            if (item != null && item.Title == "Account Settings")
            {
                await Navigation.PushAsync(new ProfileSection(), false);
            }
            else if (item != null && item.Title == "Developer Feedback & Support")
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
            else if (item != null && item.Title == "Terms Of Use")
            {
                await Navigation.PushAsync(new PrivacyPolicy(), false);
            }

            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
        }
    }

    private async void Button_Clicked_3(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Supplements";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoSupps", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllSupplements(), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Navigation.PushAsync(new SearchPage(), false);
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
        }
    }

    private async void Button_Clicked_4(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Measurements";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoMeas", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void activitylist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var tappedItem = e.DataItem as dashitem;
            if (tappedItem != null)
            {

                if (IsNavigating) return;
                IsNavigating = true;
                // Access properties dynamically
                var bc = tappedItem.Type;
                var text = tappedItem.Title;

                var signup = Helpers.Settings.SignUp;
                bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");

                //health report
                if (bc == "Health Report")
                {
                    bool Check = Preferences.Default.Get("NovoHeRep", false);
                    if (Check && NovoSignup)
                    {
                        await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                    }
                    else
                    {
                        await Navigation.PushAsync(new HealthReport(), false);
                    }
                }
                else if (bc == "Diagnosis")
                {
                    string Area = text;
                    bool Check = Preferences.Default.Get("NovoDiag", false);
                    if (Check && NovoSignup)
                    {
                        await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                    }
                    else
                    {
                        await Navigation.PushAsync(new AllDiagnosis(), false);
                    }
                }
                else if (bc == "Appointments")
                {
                    bool Check = Preferences.Default.Get("NovoAppt", false);
                    if (Check && NovoSignup)
                    {
                        await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                    }
                    else
                    {
                        await Navigation.PushAsync(new AllAppointments(), false);
                    }
                }
                else if (bc == "Measurements")
                {
                    bool Check = Preferences.Default.Get("NovoMeas", false);
                    if (Check && NovoSignup)
                    {
                        await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                    }
                    else
                    {
                        await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
                    }
                }
                else if (bc == "Symptoms")
                {
                    bool Check = Preferences.Default.Get("NovoSyms", false);
                    if (Check && NovoSignup)
                    {
                        await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                    }
                    else
                    {
                        await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
                    }
                }
                else if (bc == "Mood")
                {
                    bool Check = Preferences.Default.Get("NovoMood", false);
                    if (Check && NovoSignup)
                    {
                        await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                    }
                    else
                    {
                        await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
                    }
                }
                else if (bc == "Supplements")
                {
                    if (text.Contains(":") || text.Contains("Have you taken"))
                    {
                        string Area = "Schedule";
                        bool Check = Preferences.Default.Get("NovoSched", false);
                        if (Check && NovoSignup)
                        {
                            await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                        }
                        else
                        {
                            await Navigation.PushAsync(new MainSchedule(), false);
                        }
                    }
                    else
                    {
                        string Area = text;
                        bool Check = Preferences.Default.Get("NovoSupps", false);
                        if (Check && NovoSignup)
                        {
                            await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                        }
                        else
                        {
                            await Navigation.PushAsync(new AllSupplements(), false);
                        }

                    }
                }
                else if (bc == "Medications")
                {
                    if (text.Contains(":") || text.Contains("Have you taken"))
                    {
                        string Area = "Schedule";
                        bool Check = Preferences.Default.Get("NovoSched", false);
                        if (Check && NovoSignup)
                        {
                            await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
                        }
                        else
                        {
                            await Navigation.PushAsync(new MainSchedule(), false);
                        }
                    }
                    else
                    {
                        bool Check = Preferences.Default.Get("NovoMeds", false);
                        if (Check && NovoSignup)
                        {
                            await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, bc, userfeedbacklist[0]) { });
                        }
                        else
                        {
                            await Navigation.PushAsync(new AllMedications(), false);
                        }
                    }
                }

            }

            IsNavigating = false;


        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void Button_Clicked_5(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            string Area = "Medications";
            bool Check = Preferences.Default.Get("NovoMeds", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllMedications(), false);
            }
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    async private void symptomdetaillist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Novo Needs Added Here
            UserSymptomPassed.Clear();
            if (IsNavigating) return;
            IsNavigating = true;
            var item = e.DataItem as feedbackdata;

            // UserSymptomPassed.Add(item);

            await Navigation.PushAsync(new UpdateSingleSymptom(userfeedbacklist[0], item.title, item.value), false);
            IsNavigating = false;

            // await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Measurements";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoMeas", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Mood";
            bool Check = Preferences.Default.Get("NovoMood", false);
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped_4(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Symptoms";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoSyms", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllSymptoms(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped_5(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Schedule";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoSched", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new MainSchedule(), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }

    }

    private async void measurementnochartdetaillist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as feedbackdata;
            if (IsNavigating) return;
            IsNavigating = true;

            string Area = "Measurements";
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            bool Check = Preferences.Default.Get("NovoMeas", false);
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private void TurnonNotif_Clicked(object sender, EventArgs e)
    {
        try
        {
            AppInfo.ShowSettingsUI();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private async void Button_Clicked_7(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (DeviceInfo.Platform == DevicePlatform.Android)
    //        {
    //            // Request and capture the permission status on Android
    //            PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

    //            if (status == PermissionStatus.Denied)
    //            {
    //                AppInfo.ShowSettingsUI();
    //            }
    //            else
    //            {
    //                await LocalNotificationCenter.Current.RequestNotificationPermission();
    //                checknotifications();
    //            }
    //        }
    //        else
    //        {
    //                await LocalNotificationCenter.Current.RequestNotificationPermission();
    //                checknotifications();
    //        }
    //    }
    //    catch(Exception ex)
    //    {

    //    }
    //}


    private async void TurnoffBatterySaver(object sender, EventArgs e)
    {
        try
        {

#if ANDROID
                        BatterySettingsOpener.OpenBatterySettings();
#endif
        }
        catch (Exception ex)
        {

        }
    }
    private async void TapGestureRecognizer_Tapped_6(object sender, TappedEventArgs e)
    {
        try
        {
            //womans health questionnaire tapped
            var signup = Helpers.Settings.SignUp;
            if (IsNavigating) return;
            IsNavigating = true;

            if (signup.Contains("SFEWHA1"))
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires("73D47262-1B2C-4451-A4FC-978582D77FE0", userfeedbacklist[0]), false);
                }
                else
                {

                    // await Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    //await Navigation.PushAsync(new AndroidQuestionnaires(item), false);
                    await Navigation.PushAsync(new AndroidQuestionnaires("73D47262-1B2C-4451-A4FC-978582D77FE0", userfeedbacklist[0]), false);
                }

            }
            else if (signup.Contains("SFEWHA2"))
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires("F7FB770B-286F-4300-814D-E76AACB6DACF", userfeedbacklist[0]), false);
                }
                else
                {

                    // await Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    //await Navigation.PushAsync(new AndroidQuestionnaires(item), false);
                    await Navigation.PushAsync(new AndroidQuestionnaires("F7FB770B-286F-4300-814D-E76AACB6DACF", userfeedbacklist[0]), false);
                }


            }
            else if (signup.Contains("SFEWHA3"))
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires("DC6A9FD7-242B-4299-9672-D745669FEAF0", userfeedbacklist[0]), false);
                }
                else
                {

                    // await Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    //await Navigation.PushAsync(new AndroidQuestionnaires(item), false);
                    await Navigation.PushAsync(new AndroidQuestionnaires("DC6A9FD7-242B-4299-9672-D745669FEAF0", userfeedbacklist[0]), false);
                }


            }
            else if (signup.Contains("SFEWHA4"))
            {
                if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires("DC6A9FD7-242B-4299-9672-D745669FEAF0", userfeedbacklist[0]), false);
                }
                else
                {

                    // await Navigation.PushAsync(new QuestionnairePage("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"), false);
                    //await Navigation.PushAsync(new AndroidQuestionnaires(item), false);
                    await Navigation.PushAsync(new AndroidQuestionnaires("DC6A9FD7-242B-4299-9672-D745669FEAF0", userfeedbacklist[0]), false);
                }
            }
            else if (signup.Contains("SFEAT"))
            {
                await Navigation.PushAsync(new AndroidQuestionnaires("A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF", userfeedbacklist[0]), false);
            }

            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void TapGestureRecognizer_Tapped_7(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Browser.OpenAsync("https://www.truthaboutweight.global/ie/en.html", BrowserLaunchMode.SystemPreferred);
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
        }
    }

    private async void videoslist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            var Getitem = e.DataItem as videos;
            bool FromDash = true;

            await Navigation.PushAsync(new NewPageVideoPlayer(Getitem, FromDash), false);
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
        }
    }

    private async void TapGestureRecognizer_Tapped_8(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Navigation.PushAsync(new DashQuestionnaire("Medical History"), false);
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void TapGestureRecognizer_Tapped_9(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Navigation.PushAsync(new DashQuestionnaire("Menstural Cycle"), false);
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void TapGestureRecognizer_Tapped_10(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Navigation.PushAsync(new DashQuestionnaire("Treatment"), false);
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void TapGestureRecognizer_Tapped_11(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            var signupcode = Helpers.Settings.SignUp;
            if (signupcode.Contains("SFEWH"))
            {
                await Navigation.PushAsync(new DashQuestionnaire("Previous Responses", "list"), false);
            }
            else if (signupcode.Contains("SFECORE"))
            {
                //Get title 
                //var targetId = DashQuestionAnswers[0].inputid;
                //var GetTitle = DashQuestionInputs.Where(x => x.id == targetId).FirstOrDefault().dataTab; 
                await Navigation.PushAsync(new DashStudyQuestions(DashQuestionAnswers), false);
            }
            IsNavigating = false;

        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void Button_Clicked_6(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            await Navigation.PushAsync(new HealthDataPage(), false);
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void QuestionnaireTappedEvent(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            if (sender is VisualElement visualElement && visualElement.BindingContext is questionnaire tappedItem)
            {
                var signup = Helpers.Settings.SignUp;

                if (!string.IsNullOrEmpty(signup))
                {
                    await Navigation.PushAsync(new AndroidQuestionnaires(tappedItem.questionnaireid, userfeedbacklist[0]), false);
                }

            }
            IsNavigating = false;
        }
        catch (Exception ex)
        {
            IsNavigating = false;
        }
    }

    private async void SettingButttonClicked(object sender, TappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;

            SettingsOn GetItem = new SettingsOn();
            var btnTapped = sender as Border;
            if (btnTapped?.BindingContext is SettingsOn TappedItem)
            {
                GetItem = TappedItem;
            }
            if (GetItem == null) return;

            if (GetItem.title == "Turn On Notifications")
            {
                if (DeviceInfo.Platform == DevicePlatform.Android)
                {
                    // Request and capture the permission status on Android
                    PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();

                    if (status == PermissionStatus.Denied)
                    {
                        AppInfo.ShowSettingsUI();
                    }
                    else
                    {
                        await LocalNotificationCenter.Current.RequestNotificationPermission();
                        UserSettingsCheck();
                    }
                }
                else
                {
                    await LocalNotificationCenter.Current.RequestNotificationPermission();
                    UserSettingsCheck();
                }
            }
            else
            {
#if ANDROID
                      BatterySettingsOpener.OpenBatterySettings();
#endif
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
        }
    }





    private async void TapGestureRecognizer_Tapped_12(object sender, TappedEventArgs e)
    {
        try
        {

            await Navigation.PushAsync(new AllFitness(), false);
        }
        catch (Exception ex)
        {

        }
    }

    private async void moodlistsummary_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            string Area = "Mood";
            bool Check = Preferences.Default.Get("NovoMood", false);
            var signup = Helpers.Settings.SignUp;
            bool NovoSignup = !string.IsNullOrEmpty(signup) && signup.Contains("SAX");
            if (Check && NovoSignup)
            {
                await MopupService.Instance.PushAsync(new NovoConsentScreen(NovoConsent, Area, userfeedbacklist[0]) { });
            }
            else
            {
                await Navigation.PushAsync(new AllMood(userfeedbacklist[0]), false);
            }
            IsNavigating = false;
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }


    private async void DashQuestionItems_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as dashitem;

            var selectedGroup = GroupedDashQuestions.FirstOrDefault(g => g.Key == item.Title);

            ObservableCollection<registryDataInputs> selectedItems = selectedGroup != null
                ? new ObservableCollection<registryDataInputs>(selectedGroup)
                : new ObservableCollection<registryDataInputs>();

            await Navigation.PushAsync(new DashStudyQuestions(selectedItems, DashQuestionAnswers), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }


    //private async void Button_Clicked_6(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //update button on symptom list clicked
        //       // Show the elapsed time in a DisplayAlert
        //        await Application.Current.MainPage.DisplayAlert(
        //            "Test buttton click",
        //            $"Data retrieval took",
        //            "OK"
        //        );


        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //async private void measurementdetaillist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
        //{
        //    try
        //    {
        //        await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist[0]), false);
        //    }
        //    catch (Exception Ex)
        //    {
        //        NotasyncMethod(Ex);
        //    }
        //}
    }