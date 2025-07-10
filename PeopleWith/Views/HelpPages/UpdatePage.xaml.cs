using Microsoft.Maui.Devices;
namespace PeopleWith;

public partial class UpdatePage : ContentPage
{
	public UpdatePage()
	{
		InitializeComponent();

        string storeName = DeviceInfo.Platform == DevicePlatform.iOS ? "App Store" : "Google Play Store";
        string message = $"A new version of the app is available on the {storeName}. Please update to enjoy the latest features, improvements and fixes.";
        lblBody.Text = message;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		try
		{
                var appStoreUrl = DeviceInfo.Platform == DevicePlatform.iOS
                 ? "https://apps.apple.com/app/1323985690" 
                 : "https://play.google.com/store/apps/details?id=com.peoplewith.peoplewith"; 

                await Launcher.Default.OpenAsync(new Uri(appStoreUrl));

                Navigation.RemovePage(this);
        }
        catch
		{ 

		}
    }
}