using System.Collections.ObjectModel;
using Mopups.Services;
using Syncfusion.Maui.Core;

namespace PeopleWith;

public partial class AllVideos : ContentPage
{
    public ObservableCollection<videos> AllVideosList = new ObservableCollection<videos>();
    public ObservableCollection<videos> FilterListData = new ObservableCollection<videos>();
    //public ObservableCollection<videoengage> VideoEngagement = new ObservableCollection<videoengage>();
    videoengage SelectedVideoEngage = new videoengage(); 
    APICalls aPICalls = new APICalls();
    CrashDetected crashHandler = new CrashDetected();


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
    public AllVideos()
    {
        try
        {
            InitializeComponent();
            GetAllVideo();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);

        }
    }

    async void GetAllVideo()
    {
        try
        {

            var GetVideos = aPICalls.GetAllVideos();
            //var GetEngagement = aPICalls.GetAllVideosEngagement(); 

            var delayTask = Task.Delay(1000);

            if (await Task.WhenAny(GetVideos, delayTask) == delayTask)
            {
                await MopupService.Instance.PushAsync(new GettingReady("Loading Videos") { });
            }

            AllVideosList = await GetVideos;
            //VideoEngagement = await GetEngagement; 

            await MopupService.Instance.PopAllAsync(false);

            VideosListview.ItemsSource = AllVideosList;
            //VideosListview.HeightRequest = AllVideosList.Count() * 110;

            //Set Filter chips
            var AddALL = new videos();
            AddALL.category = "All";
            AllVideosList.Add(AddALL);

            var FilterTabList = AllVideosList.GroupBy(s => s.category).Select(g => g.First()).ToList().OrderBy(g => g.category);
            FilterListData = new ObservableCollection<videos>(FilterTabList);
            var count = AllVideosList.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
   
            FilterTabs.ItemsSource = FilterListData;
            FilterTabs.DisplayMemberPath = "category";
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void VideoCarousel_SelectionChanged(object sender, Syncfusion.Maui.Core.Carousel.SelectionChangedEventArgs e)
    {
        try
        {

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void VideosListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var selectedvideo = e.DataItem as videos;
            //foreach(var item in VideoEngagement)
            //{
            //   if(item.videoid == selectedvideo.videoid)
            //    {
            //        SelectedVideoEngage = item;
            //    }
            //}

            await Navigation.PushAsync(new VideoPlayer(selectedvideo), false); 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = searchbar.Text.ToString();

            string CleanString(string input)
            {
                return input.Replace("®", "").Trim();  // Code to Remove Saxenda ® - Causing Crash 
            }

            var FilteredVideo = AllVideosList.Where(s => !string.IsNullOrEmpty(s.title) && CleanString(s.title).Contains(CleanString(Characters), StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.title);

            var FilteredVideoCollection = new ObservableCollection<videos>(FilteredVideo);

            var count = FilteredVideoCollection.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
            VideosListview.ItemsSource = FilteredVideoCollection;
            //VideosListview.HeightRequest = FilteredVideo.Count() * 110;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FilterTabs_ChipClicked(object sender, EventArgs e)
    {
        try
        {
            var tappedFrame = sender as SfChip;
            var item = tappedFrame.Text;
            if(item == "All")
            {
                var count = AllVideosList.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                VideosListview.ItemsSource = AllVideosList;
                //VideosListview.HeightRequest = AllVideosList.Count() * 110;
            }
            else
            {
                var FilteredVideo = new ObservableCollection<videos>(AllVideosList.Where(s => s.category.Contains(item, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
                var count = FilteredVideo.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                VideosListview.ItemsSource = FilteredVideo;
                //VideosListview.HeightRequest = FilteredVideo.Count() * 110;
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
            if(Filterstack.IsVisible == true)
            {
                VideosListview.MaximumHeightRequest = 585;
                Filterstack.IsVisible = false;
            }
            else
            {
                VideosListview.MaximumHeightRequest = 535; 
                Filterstack.IsVisible = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}