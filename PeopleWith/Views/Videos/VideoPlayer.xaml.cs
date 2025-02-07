using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using System.ComponentModel;
using System.Diagnostics;
namespace PeopleWith;
public partial class VideoPlayer : ContentPage
{
    videos SelectedVideo = new videos();
    videoengage VideoEngagement = new videoengage();
    CrashDetected crashHandler = new CrashDetected();
    public Stopwatch PlayDuration = new Stopwatch();
    public Stopwatch PauseDuration = new Stopwatch();
    private bool isPlaying = false;
    private bool isPaused = false;
    APICalls database = new APICalls();
    bool fromdash;
    signupcodeinformation vidfromdash = new signupcodeinformation();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public VideoPlayer(videos VideoSelected)
    {
        try
        {
            InitializeComponent();
            SelectedVideo = VideoSelected;
            VideoThumbnail.Source = SelectedVideo.thumbnail;
            Titlelbl.Text = SelectedVideo.title;
            SubTitlelbl.Text = SelectedVideo.subtitle;
            Dateandlenthlbl.Text = "Date Added: " + SelectedVideo.dateadded;
            lengthlbl.Text = SelectedVideo.lenght;
         //   MediaElement.Source = SelectedVideo.filename;
            VideoEngagement.userid = Helpers.Settings.UserKey;
            VideoEngagement.videoid = VideoSelected.videoid;

        }
        catch (Exception Ex)
        {

            NotasyncMethod(Ex);
        }
    }

    public VideoPlayer(signupcodeinformation vidpassed)
    {
        try
        {
            InitializeComponent();

            fromdash = true;

            vidfromdash = vidpassed;
            //  SelectedVideo = VideoSelected;
            var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + vidfromdash.thumbnail;
            VideoThumbnail.Source = pdflink;
            Titlelbl.Text = vidfromdash.title;
            SubTitlelbl.Text = vidfromdash.description;
           // Dateandlenthlbl.Text = "Date Added: " + SelectedVideo.dateadded;
           // lengthlbl.Text = SelectedVideo.lenght;
            //   MediaElement.Source = SelectedVideo.filename;
            VideoEngagement.userid = Helpers.Settings.UserKey;
          //  VideoEngagement.videoid = VideoSelected.videoid;

        }
        catch (Exception Ex)
        {

            NotasyncMethod(Ex);
        }
    }

    private void MediaElement_MediaEnded(object sender, EventArgs e)
    {
        try
        {
            VideoDetails.IsVisible = true;
            Video.IsVisible = false;
            PlayDuration.Stop();
            isPlaying = false;
            VideoEngagement.closeaction = "VideoCompletion";
            NavigationPage.SetHasNavigationBar(this, true);
            //if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            //{
            //    AndroidBtn.IsVisible = false;
            //}
            //else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
            //{
            //    IOSBtn.IsVisible = false;
            //}
            closevideobtn.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void MediaElement_MediaFailed(object sender, CommunityToolkit.Maui.Core.Primitives.MediaFailedEventArgs e)
    {
        try
        {
            VideoEngagement.closeaction = "MeidaFailed";
            UpdateVideoEngagement();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void MediaElement_PositionChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaPositionChangedEventArgs e)
    {
        try
        {
            //used to Determin is the user Skips the video (No Place to store in DB) 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void MediaElement_StateChanged(object sender, CommunityToolkit.Maui.Core.Primitives.MediaStateChangedEventArgs e)
    {
        try
        {
            if (isPlaying == true || isPaused == true)
            {
                if (e.NewState == MediaElementState.Paused)
                {
                    // Media is paused
                    PlayDuration.Stop();
                    PauseDuration.Start();
                    isPaused = true;
                    isPlaying = false;
                }
                else if (e.NewState == MediaElementState.Playing)
                {
                    // The media is playing
                    PauseDuration.Stop();
                    PlayDuration.Start();
                    isPaused = false;
                    isPlaying = true;
                }
                else
                {
                    //Do Nothing Video Completed
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {


            //// Create the MediaElement
            //var mediaElement = new MediaElement
            //{
            //    HorizontalOptions = LayoutOptions.Center,
            //    BackgroundColor = Colors.Black,  // Set background color
            //    ShouldAutoPlay = true,           // Autoplay the video
            //    Aspect = Aspect.AspectFit            // Set aspect ratio (AspectFit or AspectFill)
            //   // WidthRequest = 300,              // Set the desired width (adjust as needed)
            //    //HeightRequest = 200             // Set the desired height (adjust as needed)
            //};



            //// Set the media source (replace with your video file path or URL)
            //mediaElement.Source = SelectedVideo.filename;

            // Add the MediaElement to your page layout

            //Video.Children.Add(mediaElement);

            if (fromdash)
            {
                var pdflink = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + vidfromdash.link;
                string imgPath = pdflink + ".mp4";

                await Navigation.PushAsync(new NewPageVideoPlayer(pdflink, vidfromdash.VideoID), false);
                return;
            }
            else
            {


                await Navigation.PushAsync(new NewPageVideoPlayer(SelectedVideo.filename, SelectedVideo.videoid), false);
                return;
            }

            var zeroTimeSpan = TimeSpan.Zero;
            PlayDuration.Reset();
            PauseDuration.Reset();
            VideoDetails.IsVisible = false;
            Video.IsVisible = true;
            // Get the screen height
            var screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            var mediaElementHeight = screenHeight * 0.8;

            // Apply the calculated height to the MediaElement
         //  MediaElement.HeightRequest = mediaElementHeight;

           // MediaElement.IsVisible = true;
           // MediaElement.Source = SelectedVideo.filename;
           // MediaElement.Play();
            isPlaying = true;
            PlayDuration.Start();
            VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm");
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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void MediaElement_SizeChanged(object sender, EventArgs e)
    {
        try
        {
            //User Enters Fullscreen (No Place to Store in DB)
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    public async void UpdateVideoEngagement()
    {
        try
        {
            PlayDuration.Stop();
            PauseDuration.Stop();
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
         //   await MediaElement.SeekTo(TimeSpan.Zero);
         //   MediaElement.Stop();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void BacKArrow_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (Video.IsVisible == true)
            {
                Video.IsVisible = false;
                VideoDetails.IsVisible = true;
                VideoEngagement.closeaction = "UserClosed";
                IOSBtn.IsVisible = false;
                AndroidBtn.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                UpdateVideoEngagement();
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void closevideobtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (Video.IsVisible == true)
            {
              //  MediaElement.Stop();
              //  MediaElement.Source = null;
                Video.IsVisible = false;
                VideoDetails.IsVisible = true;
                VideoEngagement.closeaction = "UserClosed";
                closevideobtn.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                UpdateVideoEngagement();

              

                var pages = Navigation.NavigationStack.ToList();
                int i = 0;
                foreach (var page in pages)
                {
                    if (i == 0)
                    {
                    }
                    else if (i == 1)
                    {
                       // Navigation.RemovePage(page);
                    }
                    else
                    {
                        Navigation.PopModalAsync();
                    }
                    i++;
                }

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Navigation.PopModalAsync();
        }
        catch(Exception ex)
        {
        }
    }
}