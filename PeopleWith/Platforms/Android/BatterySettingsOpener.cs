using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Provider;
using PeopleWith;


namespace PeopleWith
{
    public static class BatterySettingsOpener
    {
        public static void OpenBatterySettings()
        {
            var context = Android.App.Application.Context;

            Intent intent = new Intent();
            intent.SetAction(Settings.ActionBatterySaverSettings);
            intent.SetFlags(ActivityFlags.NewTask);
            if (intent.ResolveActivity(context.PackageManager) != null)
            {
                context.StartActivity(intent);
            }
            else
            {
                Android.Util.Log.Warn("BatterySettingsOpener", "No activity found for battery saver settings.");
            }
        }

        public static bool IsBatterySaverEnabled()
        {
            var context = Android.App.Application.Context;
            var powerManager = (PowerManager)context.GetSystemService(Context.PowerService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                return powerManager.IsPowerSaveMode;
            }

            return false; 
        }
    }

}
