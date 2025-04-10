using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;

namespace PeopleWith;

public partial class DiagnosisInfo : ContentPage
{
    public ObservableCollection<Information> infoSource = new ObservableCollection<Information>();
    diagnosis DiagnosisInfoPass = new diagnosis(); 
    public DiagnosisInfo(diagnosis DiagnosisInfoPassed, String Title)
	{
		InitializeComponent();
        titlelbl.Text = Title;
        DiagnosisInfoPass = DiagnosisInfoPassed;

        //Code for Now, Will be Changed when more options are added to Measurement Information 

        var GetInfo = DiagnosisInfoPass.Diagnosisinformation.Split('|');

        var Info = new Information();
        //Getinfo[0] = Description 
        Info.title = GetInfo[0];
        //Getinfo[1] = WebLink 
        Info.link = GetInfo[1];
        //Getinfo[2] = Type (In this Instance 'Web')
        Info.type = GetInfo[2];
        //Set Image 
        Info.img = "webicon.png";

        infoSource.Add(Info);
        infolist.ItemsSource = infoSource;
    }

    async private void infolist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        var item = e.DataItem as Information;
        if (item.type == "pdf")
        {
            var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;
            await Browser.OpenAsync(pdflink, new BrowserLaunchOptions
            {
                LaunchMode = BrowserLaunchMode.SystemPreferred,
                TitleMode = BrowserTitleMode.Hide
            });
        }
        else if (item.type == "video")
        {
            var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.link;
            string imgPath = pdflink + ".mp4";
        }
        else
        {
            await Browser.OpenAsync(item.link, BrowserLaunchMode.SystemPreferred);
        }

    }
}