using CommunityToolkit.Maui.Core.Primitives;
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

    protected override bool OnBackButtonPressed()
    {
        try
        {
            if (Video.IsVisible == true)
            {
                VideoDetails.IsVisible = true;
                Video.IsVisible = false;
                VideoEngagement.closeaction = "UserClosed";
                UpdateVideoEngagement();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception Ex)
        {
            return false;
        }

    }
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
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
            MediaElement.Source = SelectedVideo.filename;

            VideoEngagement.userid = Helpers.Settings.UserKey;
            VideoEngagement.videoid = VideoSelected.videoid;

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
            if(isPlaying == true  || isPaused == true)
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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            var zeroTimeSpan = TimeSpan.Zero;
            PlayDuration.Reset();
            PauseDuration.Reset();

            VideoDetails.IsVisible = false;
            Video.IsVisible = true;
            MediaElement.Play();
            isPlaying = true;
            PlayDuration.Start();
            VideoEngagement.datetimeaccessed = DateTime.Now.ToString("dd/MM/yy HH:mm"); 
            MediaElement.ShouldMute = false; 
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

            MediaElement.SeekTo(TimeSpan.Zero);
            MediaElement.Stop();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


}