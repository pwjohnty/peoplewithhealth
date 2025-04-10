using System.Collections.ObjectModel;
using Mopups.Services;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Networking;

namespace PeopleWith;

public partial class AllVideos : ContentPage
{
    public ObservableCollection<videos> AllVideosList = new ObservableCollection<videos>();
    public ObservableCollection<videos> VideosList = new ObservableCollection<videos>();
    public ObservableCollection<videos> FilterListData = new ObservableCollection<videos>();
    //public ObservableCollection<videoengage> VideoEngagement = new ObservableCollection<videoengage>();
    videoengage SelectedVideoEngage = new videoengage();
    private bool FilterTabClicked = false; 
    APICalls aPICalls = new APICalls();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();


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
            VidsLoading.IsVisible = true;
            var signupCode = Helpers.Settings.SignUp;
            var GetVideos = aPICalls.GetAllVideos();
            //var GetEngagement = aPICalls.GetAllVideosEngagement(); 

            //var delayTask = Task.Delay(1000);

            //if (await Task.WhenAny(GetVideos, delayTask) == delayTask)
            //{
            //    await MopupService.Instance.PushAsync(new GettingReady("Loading Videos") { });
            //}

            AllVideosList = await GetVideos;

            foreach(var item in AllVideosList)
            {
                if (item.signupcodeid == null && item.referral == null)
                {
                    VideosList.Add(item);
                }
                else if ((item.signupcodeid != null && item.signupcodeid.Contains(signupCode)) ||
           (item.referral != null && item.referral.Contains(signupCode)))
                {
                    VideosList.Add(item);
                }
            }
            //VideoEngagement = await GetEngagement; 

           // await MopupService.Instance.PopAllAsync(false);

            VideosListview.ItemsSource = VideosList;
            //VideosListview.HeightRequest = AllVideosList.Count() * 110;

            //Set Filter chips
            var AddALL = new videos();
            AddALL.category = "All";
            VideosList.Add(AddALL);

            var FilterTabList = VideosList.GroupBy(s => s.category).Select(g => g.First()).ToList().OrderBy(g => g.category);
            FilterListData = new ObservableCollection<videos>(FilterTabList);
            var count = VideosList.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
   
            FilterTabs.ItemsSource = FilterListData;
            FilterTabs.DisplayMemberPath = "category";
            //FilterTabs.SelectedItem;
            VidsLoading.IsVisible = false;

            FilterTabs.SelectedItem = FilterListData[0];
        }
        catch (Exception Ex)
        {
            VidsLoading.IsVisible = false;
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
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
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
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
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

            if (FilterTabClicked) return;

            var Characters = searchbar.Text.ToString();

            string CleanString(string input)
            {
                return input.Replace("®", "").Trim();  // Code to Remove Saxenda ® - Causing Crash 
            }

            var FilteredVideo = VideosList.Where(s => !string.IsNullOrEmpty(s.title) && CleanString(s.title).Contains(CleanString(Characters), StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.title);

            var FilteredVideoCollection = new ObservableCollection<videos>(FilteredVideo);

            var count = FilteredVideoCollection.Count().ToString();
            Results.Text = "Results" + " (" + count + ")";
            VideosListview.ItemsSource = FilteredVideoCollection;

            if (count == "0")
            {
                NoResultslbl.IsVisible = true;
                VideosListview.IsVisible = false;
            }
            else
            {
                VideosListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
            }

            //If FilterTabs item is Selected - UnSelect it 
            if (string.IsNullOrEmpty(searchbar.Text) || searchbar.Text == "")
            {               
                 FilterTabs.SelectedItem = FilterListData[0];               
            }
            else
            {
                if (FilterTabs.SelectedItem != null)
                {
                    FilterTabs.SelectedItem = null;
                }
            }
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
            FilterTabClicked = true;
            VideosListview.IsVisible = false;
            var tappedFrame = sender as SfChip;
            var item = tappedFrame.Text;
            if(item == "All")
            {
                var count = VideosList.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                VideosListview.ItemsSource = VideosList;
                VideosListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                //VideosListview.HeightRequest = AllVideosList.Count() * 110;
            }
            else
            {
                var SignupCode = Helpers.Settings.SignUp;
                ObservableCollection<videos> FilteredVideo;

                if (item == "Saxenda®" || item == "Parkinson's Diease" || item == "Asthma" || item == "PeopleWith SfE")
                {
                    FilteredVideo = new ObservableCollection<videos>(
                        AllVideosList
                            .Where(s => s.referral?.Contains(SignupCode, StringComparison.OrdinalIgnoreCase) == true)
                            .OrderBy(m => m.title)
                    );
                }
                else
                {
                    FilteredVideo = new ObservableCollection<videos>(
                        AllVideosList
                            .Where(s => s.category?.Contains(item, StringComparison.OrdinalIgnoreCase) == true)
                            .OrderBy(m => m.title)
                    );
                }

                var count = FilteredVideo.Count().ToString();
                Results.Text = "Results" + " (" + count + ")";
                VideosListview.ItemsSource = null; 
                VideosListview.ItemsSource = FilteredVideo;
                VideosListview.IsVisible = true;
                NoResultslbl.IsVisible = false;
                //VideosListview.HeightRequest = FilteredVideo.Count() * 110;
            }

            searchbar.Text = string.Empty;

            Device.BeginInvokeOnMainThread(() =>
            {
                FilterTabClicked = false;
            });


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
                //VideosListview.MaximumHeightRequest = 585;
                Filterstack.IsVisible = false;
            }
            else
            {
                //VideosListview.MaximumHeightRequest = 535; 
                Filterstack.IsVisible = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}