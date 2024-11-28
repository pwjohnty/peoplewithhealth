using Microsoft.VisualBasic;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class MeasurementsInfo : ContentPage
{
	measurement MeasureInfoPassed;
    public ObservableCollection<Information> infoSource = new ObservableCollection<Information>();
	public MeasurementsInfo(measurement MeasureInfo, string Title)
	{
		InitializeComponent();
        titlelbl.Text = Title; 
        MeasureInfoPassed = MeasureInfo;

        //Code for Now, Will be Changed when more options are added to Measurement Information 

        var GetInfo = MeasureInfoPassed.measurementinformation.Split('|');
        
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