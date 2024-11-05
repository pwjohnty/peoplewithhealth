using Microsoft.Maui.Controls;
using UIKit;
using WebKit;

namespace PeopleWith
{
    public static class PrintHelperIOS
    {
        public static void Print(WebView webView)
        {
            // Use WKWebView instead of UIWebView
            var iosWebView = webView.Handler.PlatformView as WKWebView;

            if (iosWebView == null)
            {
                throw new InvalidOperationException("Unable to get WKWebView from the WebView.");
            }

            var printInfo = UIPrintInfo.PrintInfo;
            printInfo.OutputType = UIPrintInfoOutputType.General;

            var Current = DateTime.Now.ToString("dd-MM-yy HH:mm:ss");
            var Docname = "PeopleWithHealthReport" + " " + Current;

            printInfo.JobName = Docname;

            var printController = UIPrintInteractionController.SharedPrintController;
            printController.PrintInfo = printInfo;

            // Use WKWebView's PrintFormatter instead of UIWebView's PrintFormatter
            printController.PrintFormatter = iosWebView.ViewPrintFormatter;
            printController.Present(true, (handler, completed, error) => {
                if (!completed && error != null)
                {
                    Console.WriteLine($"Printing error: {error.LocalizedDescription}");
                }
            });
        }
    }
}
