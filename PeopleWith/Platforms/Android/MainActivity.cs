using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.AppCompat.App;
using Firebase.Messaging;
using Microsoft.Azure.NotificationHubs;
using static Android.Provider.Settings;
using static Android.Provider.SyncStateContract;

namespace PeopleWith
{
    [Activity(Theme = "@style/Maui.SplashTheme", LaunchMode = LaunchMode.SingleTop,  MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private NotificationHubClient hub;
        private static string? GetDeviceId() => Secure.GetString(Android.App.Application.Context.ContentResolver, Secure.AndroidId);

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           // Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
           // Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);

            // Set the screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            try
            {
                Firebase.FirebaseApp.InitializeApp(this);


                //var hubName = "PWDevHub";
                //var connectionString = "Endpoint=sb://PWDevelopment.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=ZiwsFi5CJVNru6prZMix/55OIDEZJvXumOSBkRjU4gM="; // Can be found in Access policy. Use Listen connection

                hub = NotificationHubClient.CreateClientFromConnectionString(Constants.ListenConnectionString, Constants.NotificationHubName);

                CreateNotificationChannel();
            }
            catch (Exception ex)
            {
            }

            // Handle notification tap if the activity was launched from a notification
            if (Intent?.Extras != null)
            {
                HandleNotificationTap(Intent);
            }

        }

        private async void CreateNotificationChannel()
        {
            try
            {
                var token = await SecureStorage.GetAsync("FireBaseToken");

                if (token == null)
                {
                    token = FirebaseMessaging.Instance.GetToken().ToString();
                }

                Helpers.Settings.Token = token;
                Helpers.Settings.DeviceID = GetDeviceId();

                //List<string> tags = new List<string>();

                //if (!string.IsNullOrEmpty(Helpers.Settings.UserKey))
                //{
                //    tags.Add(Helpers.Settings.UserKey);
                //}

                //if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
                //{
                //    tags.Add(Helpers.Settings.SignUp);
                //}

                //tags.Add("IID3");
                IList<string> tags = new List<string>();

                if (!string.IsNullOrEmpty(Helpers.Settings.UserKey))
                {
                    tags.Add(Helpers.Settings.UserKey);
                }

                if (!string.IsNullOrEmpty(Helpers.Settings.SignUp))
                {
                    tags.Add(Helpers.Settings.SignUp);
                }

                //tags.Add("MARK");

                var installation = new Microsoft.Azure.NotificationHubs.Installation
                {
                    InstallationId = GetDeviceId(),
                    PushChannel = token,
                    Platform = NotificationPlatform.FcmV1,
                    Tags = tags
                };
                await hub.CreateOrUpdateInstallationAsync(installation);

                //await SharedNotificationService.RegisterDeviceAsync(token, NotificationPlatform.Fcm, new string[] { "InitialTag" });


                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channelName = "default";
                    var channelDescription = string.Empty;
                    var channel = new NotificationChannel(channelName, channelName, NotificationImportance.High)
                    {
                        Description = channelDescription
                    };

                    var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                    notificationManager.CreateNotificationChannel(channel);
                }
            }
            catch(Exception ex) 
            { 

            }
        }

        public override void OnBackPressed()
        {
            // Handle the back button press
            // You can show a dialog, navigate to a different page, or cancel the action

            // For example, to cancel the back button press:
            // base.OnBackPressed(); // Uncomment this line if you want to allow the default behavior

            // Alternatively, if you want to perform some action:
            // DisplayAlert("Back button pressed", "The back button was pressed and handled.", "OK");

            // To cancel the back button action, do not call the base method
        }

        // Add this method to handle the navigation
        protected override void OnNewIntent(Intent intent)
        {
            try
            {
                base.OnNewIntent(intent);

                // Handle notification tap if a new intent is received
                if (intent?.Extras != null)
                {
                    HandleNotificationTap(intent);
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void HandleNotificationTap(Intent intent)
        {
            try
            {
                string studyidfornotification = intent.GetStringExtra("studyid");
                if (studyidfornotification == "IID3")
                {
                    var action = intent.GetStringExtra("action");
                    var questionnaire = intent.GetStringExtra("questionnaire");
                    var textsummary = intent.GetStringExtra("textsummary");
                    var questionnaireid = intent.GetStringExtra("questionnaireid");
                    string[] originalArray = { action, studyidfornotification, questionnaire, questionnaireid, textsummary };


                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        //   await NavigationPage.Navigation.PushAsync(new NotificationQuestion(originalArray));
                    });

                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                var errorMessage = ex.StackTrace.ToString();
                // Consider using a logging framework or analytics to track this error
            }
        }


    }
}
