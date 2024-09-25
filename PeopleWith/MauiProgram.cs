using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using Plugin.Maui.SegmentedControl;
using Syncfusion.Maui.Core.Hosting;
using Plugin.LocalNotification;

namespace PeopleWith
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .ConfigureSyncfusionCore()
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseSegmentedControl()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
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

            return builder.Build();
        }
    }
}
