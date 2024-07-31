namespace PeopleWith;

public partial class UpdatePage : ContentPage
{
	public UpdatePage()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
		try
		{
            // Redirect to App Store
            var appStoreUrl = DeviceInfo.Platform == DevicePlatform.iOS
                ? "https://apps.apple.com/app/1323985690" // Replace YOUR_APP_ID with your app's ID
                : "https://play.google.com/store/apps/details?id=com.peoplewith.peoplewith"; // Replace YOUR_PACKAGE_NAME with your app's package name

            await Launcher.Default.OpenAsync(new Uri(appStoreUrl));
        }
        catch
		{ 

		}
    }
}