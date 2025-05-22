using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Net;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.AppCompat.App;
//using Firebase.Messaging;
using Microsoft.Azure.NotificationHubs;
using Plugin.Fingerprint;
using static Android.Provider.Settings;
using static Android.Provider.SyncStateContract;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;
using Microsoft.Maui;
using Android.Runtime;
using AndroidX.Activity;
using Android.Provider;

namespace PeopleWith
{
    [Activity(Theme = "@style/Maui.SplashTheme", LaunchMode = LaunchMode.SingleTop,  MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private NotificationHubClient hub;
        private static string? GetDeviceId() => Secure.GetString(Android.App.Application.Context.ContentResolver, Secure.AndroidId);
        public static MainActivity Instance { get; private set; }

        protected override async void OnCreate(Bundle savedInstanceState)
        {

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                Sentry.SentrySdk.CaptureException(args.Exception);
                System.Diagnostics.Debug.WriteLine("Unhandled Exception: " + args.Exception.ToString());

            };


            base.OnCreate(savedInstanceState);
            // Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
            // Window.SetNavigationBarColor(Android.Graphics.Color.Transparent);

            //Android on back Pressed 
            //OnBackPressedDispatcher.AddCallback(this, new BackPressed(this));


            //OnBackPressedDispatcher.OnBackPressed += async (sender, e) =>
            //{
            //    var navigation = Application.Current.MainPage?.Navigation;

            //    if (navigation != null && navigation.NavigationStack.Count > 1)
            //    {
            //        await MainThread.InvokeOnMainThreadAsync(async () =>
            //        {
            //            await navigation.PopAsync();
            //        });
            //    }
            //    else
            //    {
            //        Finish(); // or moveTaskToBack(true) if you want it to go to background
            //    }
            //};

            Instance = this;
            //Initalize Fingerprint 
            //CrossFingerprint.SetCurrentActivityResolver(() => this);

            // Set the screen orientation to portrait
            RequestedOrientation = ScreenOrientation.Portrait;
            initFontScale();

            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo;

            // Register the custom connectivity receiver
            CustomConnectivityReceiver customReceiver = new CustomConnectivityReceiver();
            IntentFilter intentFilter = new IntentFilter(ConnectivityManager.ConnectivityAction);
            RegisterReceiver(customReceiver, intentFilter);


            try
            {
                //Firebase.FirebaseApp.InitializeApp(this);


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

        //internal class BackPressed : OnBackPressedCallback
        //{
        //    private readonly Activity activity;
        //    private long backPressed;
        //    public BackPressed(Activity activity) : base(true)
        //    {
        //        this.activity = activity;
        //    }
        //    public override void HandleOnBackPressed()
        //    {
        //        var navigation = Microsoft.Maui.Controls.Application.Current?.MainPage?.Navigation;
        //        if (navigation is not null && navigation.NavigationStack.Count <= 1 && navigation.ModalStack.Count <= 0)
        //        {
        //            const int delay = 2000;
        //            if (backPressed + delay > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
        //            {
        //                activity.FinishAndRemoveTask();
        //                Process.KillProcess(Process.MyPid());
        //            }
        //            else
        //            {
        //                activity.MoveTaskToBack(true);
        //                //Toast.MakeText(activity, "Close", ToastLength.Long)?.Show();
        //                //backPressed = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        //            }
        //        }
        //    }
        //}

        private void initFontScale()
        {

            Resources.Configuration.FontScale = 1.0f;
            Resources.UpdateConfiguration(Resources.Configuration, Resources.DisplayMetrics);
            //Configuration configuration = Resources.Configuration;
            //configuration.FontScale = (float)1;
            ////0.85 small, 1 standard, 1.15 big，1.3 more bigger ，1.45 supper big 
            //DisplayMetrics metrics = new DisplayMetrics();
            //WindowManager.DefaultDisplay.GetMetrics(metrics);
            //metrics.ScaledDensity = configuration.FontScale * metrics.Density;
            //// BaseContext.Resources.UpdateConfiguration(configuration, metrics);
            //BaseContext.Resources.DisplayMetrics.SetTo(metrics);
            //BaseContext.ApplicationContext.CreateConfigurationContext(configuration);

        }

        private async void CreateNotificationChannel()
        {
            try
            {
                var token = await SecureStorage.GetAsync("FireBaseToken");

                if (token == null)
                {
                   // token = FirebaseMessaging.Instance.GetToken().ToString();
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

        //public static void OpenBatterySettings(Activity activity)
        //{
        //    Intent intent = new Intent();
        //    intent.SetAction(Settings.Panel.ActionBatterySettings);
        //    activity.StartActivity(intent);
        //}

        public override void OnBackPressed()
        {
            //    var currentPage = MainPage.Navigation.NavigationStack.LastOrDefault();

            //    var navigation = Application.Current.MainPage;

            //    if (navigation != null && navigation.NavigationStack.Count > 1)
            //    {
            //        // Remove the top page
            //        MainThread.BeginInvokeOnMainThread(async () =>
            //        {
            //            await navigation.PopAsync(); 
            //        });
            //    }
            //    else
            //    {
            //        // It's the root page, allow default back behavior (e.g., app exits)
            //        base.OnBackPressed();
            //    }
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
                if (intent == null) return;

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

        public override bool DispatchTouchEvent(MotionEvent? e)
        {
            if (e!.Action == MotionEventActions.Down)
            {
                var focusedElement = CurrentFocus;
                if (focusedElement is EditText editText)
                {
                    var editTextLocation = new int[2];
                    editText.GetLocationOnScreen(editTextLocation);
                    var clearTextButtonWidth = 100; // syncfusion clear button at the end of the control
                    var editTextRect = new Rect(editTextLocation[0], editTextLocation[1], editText.Width + clearTextButtonWidth, editText.Height);
                    //var editTextRect = editText.GetGlobalVisibleRect(editTextRect);  //not working in MAUI, always returns 0,0,0,0
                    var touchPosX = (int)e.RawX;
                    var touchPosY = (int)e.RawY;
                    if (!editTextRect.Contains(touchPosX, touchPosY))
                    {
                        editText.ClearFocus();
                        var inputService = GetSystemService(Context.InputMethodService) as InputMethodManager;
                        inputService?.HideSoftInputFromWindow(editText.WindowToken, 0);
                    }
                }
            }
            return base.DispatchTouchEvent(e);
        }


    }
}
