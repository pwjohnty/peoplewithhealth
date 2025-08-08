using Android.App;
using Android.Runtime;
using Sentry;

[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]

namespace PeopleWith
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership) : base(handle, ownership)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                ConfigureSentryUserScope();
                SentrySdk.CaptureException(args.Exception);
            };
        }

        void ConfigureSentryUserScope()
        {
            SentrySdk.ConfigureScope(scope =>
            {
                string UserID = Preferences.Default.Get("userid", "Unknown");
                string UserEmail = Preferences.Default.Get("email", "Unknown");
                scope.User = new SentryUser
                {
                    Id = UserID,
                    Email = UserEmail
                };
            });
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
