using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Plugin.Maui.SegmentedControl;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using Syncfusion.Maui.Core.Hosting;
using Plugin.LocalNotification;
using Plugin.Maui.Biometric;
using SkiaSharp.Views.Maui.Controls.Hosting;
using CommunityToolkit.Maui;
using Maui.FreakyControls.Extensions;
using Microsoft.Maui.Handlers;

namespace PeopleWith
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .ConfigureSyncfusionCore()
                .UseMauiCommunityToolkitMediaElement()
                .UseMauiCommunityToolkit()
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseSegmentedControl()
                .UseSkiaSharp()
#if ANDROID
                .ConfigureMauiHandlers(handlers => handlers.AddHandler<Microsoft.Maui.Controls.Entry, PINView.Maui.Platforms.Android.Handlers.EntryHandler>())
#endif
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("HankenGrotesk-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("HankenGrotesk-Bold.ttf", "OpenSansSemibold");
                    fonts.AddFont("HankenGrotesk-Bold.ttf", "HankenGroteskBold");
                    fonts.AddFont("HankenGrotesk-Light.ttf", "HankenGroteskLight");
                    fonts.AddFont("HankenGrotesk-SemiBold.ttf", "HankenGroteskSemiBold");
                    fonts.AddFont("HankenGrotesk-Regular.ttf", "HankenGroteskRegular");
                })
                .ConfigureMopups();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.UseMauiApp<App>().ConfigureMauiHandlers((handlers) => {
#if IOS
               handlers.AddHandler(typeof(Shell), typeof(CustomShellRenderer));  
#endif
            });

            //Remove Border on Entry (IOS)
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("Borderless", (handler, view) =>
            {
#if IOS
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            //Remove Border on DatePicker (IOS)
            Microsoft.Maui.Handlers.DatePickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
#if IOS
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            //Remove Border on TimePicker (IOS)
            Microsoft.Maui.Handlers.TimePickerHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            {
#if IOS
            handler.PlatformView.BackgroundColor = UIKit.UIColor.Clear;
            handler.PlatformView.Layer.BorderWidth = 0;
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            //builder.Services.AddSingleton(typeof(IFingerprint), CrossFingerprint.Current);
            builder.ConfigureSyncfusionCore();
            builder.InitializeFreakyControls();

            // Use with Dependency Injection
            builder.Services.AddSingleton<IBiometric>(BiometricAuthenticationService.Default);
            return builder.Build();
        }
    }
}
