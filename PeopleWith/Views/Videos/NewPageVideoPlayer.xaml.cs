using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using System.Diagnostics;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;

namespace PeopleWith;

public partial class NewPageVideoPlayer : ContentPage
{
    signupcodeinformation vidfromdash = new signupcodeinformation();
    videos Signupvidfromdash = new videos();
    bool fromdash;

    //Video Metrics 
    videoengage VideoEngagement = new videoengage();
    public Stopwatch PlayDuration = new Stopwatch();
    public Stopwatch PauseDuration = new Stopwatch();
    private bool isPlaying = false;
    private bool isPaused = false;
    public bool ispageloading = true; 

    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    APICalls database = new APICalls();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ispageloading = false; 
    }

    public NewPageVideoPlayer()
	{
		InitializeComponent();
	}

    public NewPageVideoPlayer(string videolink, string VideoID)
    {

        //Navigation From Video Page
        InitializeComponent();

        MediaElement.Source = videolink;

        //if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        //{
        //    Task.Delay(100).ContinueWith(t =>
        //    {
        //        Device.BeginInvokeOnMainThread(() =>
        //        {
        //            Navigation.RemovePage(this);
        //        });
        //    });
        //    return;
        //}

        var zeroTimeSpan = TimeSpan.Zero;

        var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        var mediaElementHeight = screenHeight * 0.8;

        // Apply the calculated height to the MediaElement
        MediaElement.HeightRequest = mediaElementHeight;
        MediaElement.IsVisible = true;
        MediaElement.Source = videolink;
        MediaElement.Play();

        //Start Video Metrics Data
        isPlaying = true;
        PlayDuration.Start();
        VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm");
        VideoEngagement.videoid = VideoID;
        VideoEngagement.userid = Helpers.Settings.UserKey;


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

    public NewPageVideoPlayer(signupcodeinformation vidpassed, bool dash)
    {
        try
        {

            //Navigation From Dashboard
            InitializeComponent();

            //MediaElement.Source = videolink;
            vidfromdash = vidpassed;
            var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + vidfromdash.link;
            MediaElement.Source = pdflink;

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
            MediaElement.Source = pdflink;
            MediaElement.Play();

            //Start Video Metrics Data
            isPlaying = true;
            PlayDuration.Start();
            VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm");
            VideoEngagement.videoid = vidfromdash.VideoID;
            VideoEngagement.userid = Helpers.Settings.UserKey; 

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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            //var s = Ex.StackTrace.ToString();
        }
    }


    public NewPageVideoPlayer(videos vidpassed, bool dash)
    {
        try
        {

            //Navigation From Dashboard
            InitializeComponent();

            //MediaElement.Source = videolink;
            Signupvidfromdash = vidpassed;
            MediaElement.Source = vidpassed.filename;

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
            MediaElement.Source = vidpassed.filename;
            MediaElement.Play();

            //Start Video Metrics Data
            isPlaying = true;
            PlayDuration.Start();
            VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm");
            VideoEngagement.videoid = Signupvidfromdash.videoid;
            VideoEngagement.userid = Helpers.Settings.UserKey;

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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            //var s = Ex.StackTrace.ToString();
        }


    }

    private async void closevideobtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (Video.IsVisible == true)
            {
                MediaElement.Stop();
                isPlaying = false;
                if (!string.IsNullOrEmpty(VideoEngagement.closeaction))
                {
                    //Video Completed Text set to {VideoCompletion}
                }
                else
                {
                    VideoEngagement.closeaction = "UserClosed";
                }
               
                MediaElement.Source = null;
             //   Video.IsVisible = false;
              //  VideoDetails.IsVisible = true;
               // VideoEngagement.closeaction = "UserClosed";
                closevideobtn.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                UpdateVideoEngagement();



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
            NotasyncMethod(Ex);
            //  NotasyncMethod(Ex);
        }
    }

    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //    if (MediaElement != null)
    //    {
    //       // MediaElement.Stop();
    //    }
    //}

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
            //NotasyncMethod(Ex);
        }
    }

    public async Task UpdateVideoEngagement()
    {
        try
        {
            PlayDuration.Stop();
            PauseDuration.Stop();

            if (VideoEngagement == null)
                return;

            var zeroTimeSpan = TimeSpan.Zero;
            if (PauseDuration.Elapsed != zeroTimeSpan)
            {
                VideoEngagement.pauseduration = PauseDuration.Elapsed.ToString(@"mm\:ss");
            }
            if (PlayDuration.Elapsed != zeroTimeSpan)
            {
                VideoEngagement.watchduration = PlayDuration.Elapsed.ToString(@"mm\:ss");
            }

            await database.PostEngagementAsync(VideoEngagement);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex); // Ensure proper exception handling/logging
        }
    }

    // Video Completed 
    private async void MediaElement_MediaEnded(object sender, EventArgs e)
    {
        try
        {
            isPlaying = false;
            VideoEngagement.closeaction = "VideoCompletion";

            PlayDuration.Stop();
            PauseDuration.Stop();
            //await UpdateVideoEngagement();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    // Video Skipped Forward/Backward
    private void MediaElement_PositionChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaPositionChangedEventArgs e)
    {
        try
        {
            // No way to record in DB 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    // Video Played / Paused 
    private void MediaElement_StateChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaStateChangedEventArgs e)
    {
        try
        {
            if(ispageloading == true)
            {
                //Do Nothing - Causing False data on page load
            }
            else
            {
                if (MediaElement == null) return;

                if (MediaElement.CurrentState == MediaElementState.Stopped || MediaElement.CurrentState == MediaElementState.Paused)
                {
                    // Media is paused
                    PlayDuration.Stop();
                    PauseDuration.Start();
                    isPaused = true;
                    isPlaying = false;
                }
                else if (MediaElement.CurrentState == MediaElementState.Playing)
                {
                    // The media is playing
                    PauseDuration.Stop();
                    PlayDuration.Start();
                    isPaused = false;
                    isPlaying = true;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}