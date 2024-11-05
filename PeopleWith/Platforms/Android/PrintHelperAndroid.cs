using Android.Content;
using Android.Print;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;


namespace PeopleWith
{
    public static class PrintHelperAndroid
    {
        public static void Print(WebView webView)
        {
            var droidViewToPrint = webView.Handler.PlatformView as Android.Webkit.WebView;

            if (droidViewToPrint != null)
            {
                var printMgr = MainActivity.Instance.GetSystemService(Android.Content.Context.PrintService) as Android.Print.PrintManager;
                var Current = DateTime.Now.ToString("dd-MM-yy HH:mm:ss");
                var Docname = "PeopleWithHealthReport" + " " + Current; 
                PrintDocumentAdapter printAdapter = droidViewToPrint.CreatePrintDocumentAdapter(Docname);

                printMgr.Print("PrintJob", printAdapter, null);
            }
        }
    }
}
