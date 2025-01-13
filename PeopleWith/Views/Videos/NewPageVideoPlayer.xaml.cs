using CommunityToolkit.Maui.Views;

namespace PeopleWith;

public partial class NewPageVideoPlayer : ContentPage
{

    bool fromdash;
	public NewPageVideoPlayer()
	{
		InitializeComponent();

	}


    public NewPageVideoPlayer(string videolink)
    {
        InitializeComponent();

        MediaElement.Source = videolink;

        if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            Task.Delay(100).ContinueWith(t =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.RemovePage(this);
                });
            });
            return;
        }

        var zeroTimeSpan = TimeSpan.Zero;
     //   PlayDuration.Reset();
     //   PauseDuration.Reset();
     //   VideoDetails.IsVisible = false;
     //   Video.IsVisible = true;
        // Get the screen height
        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        var mediaElementHeight = screenHeight * 0.8;

        // Apply the calculated height to the MediaElement
        MediaElement.HeightRequest = mediaElementHeight;

        MediaElement.IsVisible = true;
        MediaElement.Source = videolink;
        MediaElement.Play();
      //  isPlaying = true;
      //  PlayDuration.Start();
      //  VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm");
        //MediaElement.ShouldMute = false;
        NavigationPage.SetHasNavigationBar(this, false);

        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            //  AndroidBtn.IsVisible = true;
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            // mediaElement.HeightRequest = 200;
            // mediaElement.WidthRequest = 300;
            //IOSBtn.IsVisible = true;
        }

        closevideobtn.IsVisible = true;


    }

    public NewPageVideoPlayer(string videolink, bool dash)
    {
        try
        {
            InitializeComponent();

            MediaElement.Source = videolink;

            fromdash = dash;

            if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                Task.Delay(100).ContinueWith(t =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Navigation.RemovePage(this);
                    });
                });
                return;
            }

            var zeroTimeSpan = TimeSpan.Zero;
            //   PlayDuration.Reset();
            //   PauseDuration.Reset();
            //   VideoDetails.IsVisible = false;
            //   Video.IsVisible = true;
            // Get the screen height
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            var mediaElementHeight = screenHeight * 0.8;

            // Apply the calculated height to the MediaElement
            MediaElement.HeightRequest = mediaElementHeight;

            MediaElement.IsVisible = true;
            MediaElement.Source = videolink;
            MediaElement.Play();
            //  isPlaying = true;
            //  PlayDuration.Start();
            //  VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm");
            //MediaElement.ShouldMute = false;
            NavigationPage.SetHasNavigationBar(this, false);

            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                //  AndroidBtn.IsVisible = true;
            }
            else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            {
                // mediaElement.HeightRequest = 200;
                // mediaElement.WidthRequest = 300;
                //IOSBtn.IsVisible = true;
            }

            closevideobtn.IsVisible = true;
        }
        catch (Exception ex)
        {
            var s = ex.StackTrace.ToString();
        }


    }

    private async void closevideobtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (Video.IsVisible == true)
            {
                 MediaElement.Stop();
                  MediaElement.Source = null;
             //   Video.IsVisible = false;
              //  VideoDetails.IsVisible = true;
               // VideoEngagement.closeaction = "UserClosed";
                closevideobtn.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
               // UpdateVideoEngagement();



                var pages = Navigation.NavigationStack.ToList();
                int i = 0;

                if (fromdash)
                {
                    foreach (var page in pages)
                    {
                        if (i == 0)
                        {
                        }
                        else if (i == 1)
                        {
                             Navigation.RemovePage(page);
                        }
                        else
                        {
                            Navigation.RemovePage(page);
                        }
                        i++;
                    }
                }
                else
                {
                    foreach (var page in pages)
                    {
                        if (i == 0)
                        {
                        }
                        else if (i == 1)
                        {
                            // Navigation.RemovePage(page);
                        }
                        else if (i == 2)
                        {
                            // Navigation.RemovePage(page);
                        }
                        else
                        {
                            Navigation.RemovePage(page);
                        }
                        i++;
                    }
                }

               // await Navigation.PushAsync(new AllVideos(), false);

            }
        }
        catch (Exception Ex)
        {
          //  NotasyncMethod(Ex);
        }
    }

    private void ContentPage_Unloaded(object sender, EventArgs e)
    {
        try
        {
            // If you have a MediaElement, disconnect the handler when the page is unloaded
            if (MediaElement.Handler != null)
            {
                // Stop the video and disconnect the handler
               // MediaElement.Stop();
               // MediaElement.IsVisible = false;
             //   MediaElement.Handler?.DisconnectHandler();

          
            }
        }
        catch (Exception Ex)
        {
        }
    }
}