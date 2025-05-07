using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Devices;
using Microsoft.Maui.ApplicationModel.Communication;

namespace PeopleWith
{
    public class CrashDetected
    {


        public async Task SentryCrashDetected(Exception Ex)
        {
            try
            {
                // Add/Remove as Needed
                string UserID = Preferences.Default.Get("userid", "Unknown");
                string UserEmail = Preferences.Default.Get("email", "Unknown");
                SentrySdk.ConfigureScope(scope =>
                {
                    scope.User = new SentryUser
                    {
                        Id = UserID,
                        Email = UserEmail
                    };

                    SentrySdk.CaptureException(Ex);
                });
            }
            catch (Exception ex)
            {

            }
        }

        //Old Code 
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

                    var Page = method?.DeclaringType?.FullName ?? "Unknown";
                    var Split = Page.Split('.');
                    var GetPage = Split[1];
                    var GetMethod = method?.Name ?? "Unknown";
                    var FoundPage = GetPage + " (" + GetMethod + ")";
                    if (FoundPage.Contains(">"))
                    {
                        string[] result = GetPage.Split(new char[] { '.', '>', '<', '+' }, StringSplitOptions.RemoveEmptyEntries);
                        FoundPage = result[0] + " (" + result[1] + ")";
                    }
                   
                    //Line not Working
                    //int linenumber = (Ex.StackTrace).frame.GetFileLineNumber();
                    //var GetLine = frame.GetFileLineNumber();

                    // Get the top stack frame
                    //var GetLine = stackTrace.GetFrame(stackTrace.FrameCount - 1);
                    //var line = GetLine.GetFileLineNumber();

                    int Line = await GetLinNo(Ex);
                    //var LineNumber = "Line: " + Line.ToString(); 

                    var errorDetails = new crashlog
                    {
                        page = FoundPage,
                        line = Line.ToString(),
                        exception = Ex.Message,
                        deleted = false,
                        userid = Helpers.Settings.UserKey,
                        stacktrace = Ex.StackTrace,
                        deviceos = DeviceInfo.Current.VersionString,  
                        devicemodel = GetModel.ToString()  
                    };
                    await DBCall(errorDetails); 

                }                      
            }
            catch (Exception ex)
            {

            }
           
        }

        async public Task<int> GetLinNo(Exception Ex)
        {
                var lineNumber = 0;
                const string lineSearch = ":line ";
                var index = Ex.StackTrace.LastIndexOf(lineSearch);
                if (index != -1)
                {
                    var lineNumberText = Ex.StackTrace.Substring(index + lineSearch.Length);
                    if (int.TryParse(lineNumberText, out lineNumber))
                    {
                    }
                }
                return lineNumber;
        }

        public async Task DBCall(crashlog Item) 
        {
            APICalls database = new APICalls();
            await database.InsertCrashLog(Item);
        }
    }
}
