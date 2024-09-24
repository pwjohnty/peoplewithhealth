using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith
{
    public class CrashDetected
    {
        public async Task CrashDetectedSend(Exception Ex)
        {
            try
            {
                var stackTrace = new System.Diagnostics.StackTrace(Ex);
                var frame = stackTrace.GetFrame(0);

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                var GetModel = sb.AppendLine(DeviceInfo.Current.Model);
                var GetOS = sb.AppendLine(DeviceInfo.Current.VersionString);

                if (frame != null)
                {
                    var method = frame.GetMethod();
                    var GetPage = method?.DeclaringType?.FullName ?? "Unknown";
                    string[] result = GetPage.Split(new char[] { '.', '>' }, StringSplitOptions.RemoveEmptyEntries);
                    var Split = result[1].Split('+');
                    string FoundPage = Split[0] + Split[1] + ">"; 
                    //Line not Working
                    var GetLine = frame.GetFileLineNumber();
                    var errorDetails = new crashlog
                    {
                        page = FoundPage,
                        line = GetLine.ToString(),
                        exception = Ex.Message,
                        deleted = false,
                        userid = Helpers.Settings.UserKey,
                        stacktrace = Ex.StackTrace,
                        deviceos = DeviceInfo.Current.VersionString,  // Uncomment if needed
                        devicemodel = GetModel.ToString()  // Uncomment if needed
                    };
                    await DBCall(errorDetails); // Make sure to await DBCall

                }                      
            }
            catch (Exception ex)
            {

            }
           
        }

        public async Task DBCall(crashlog Item) // Changed from void to Task
        {
            APICalls database = new APICalls();
            await database.InsertCrashLog(Item);
        }
    }
}
