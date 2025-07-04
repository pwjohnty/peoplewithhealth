using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel;
using System.Text.RegularExpressions;

namespace PeopleWith;

public partial class AllergyInfo : ContentPage
{
    public ObservableCollection<Information> infoSource = new ObservableCollection<Information>();
    allergies AllergyInfoPass = new allergies(); 
    public AllergyInfo(allergies InfoPassed, string Title)
	{
		InitializeComponent();
        titlelbl.Text = Title;
        AllergyInfoPass = InfoPassed;

        //Code for Now, Will be Changed when more options are added to Measurement Information 

        var GetInfo = AllergyInfoPass.Allergyinformation.Split('|');

        var Info = new Information();
        //Getinfo[0] = Description 
        Info.title = GetInfo[0];
        //Getinfo[1] = WebLink 
        string extracted = ExtractDomain(GetInfo[1]);
        Info.shortlink = extracted;
        Info.link = GetInfo[1];
        //Getinfo[2] = Type (In this Instance 'Web')
        Info.type = GetInfo[2] + "site";
        //Set Image 
        Info.img = "link.png";

        Bodylbl.Text = GetInfo[0];

        infoSource.Add(Info);
        infolist.ItemsSource = infoSource;
    }

    static string ExtractDomain(string url)
    {
        try
        {
            Match match = Regex.Match(url, @"//([^/]+)");
            return match.Success ? match.Groups[1].Value : string.Empty;
        }
        catch (Exception ex)
        {
            return null;
            //Dunno 
        }
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