//using AndroidX.Activity;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SearchPage : ContentPage
{
    APICalls aPICalls = new APICalls();
    public ObservableCollection<videos> AllVideosList = new ObservableCollection<videos>();
    public ObservableCollection<videos> FilterResults = new ObservableCollection<videos>();
    public SearchPage()
	{
		InitializeComponent();

		searchbar.Focus();


        getallhelpvideos();

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(200); // Add a short delay if required
        searchbar.Focus();
    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
		try
		{

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                FilterResults.Clear();
                allvideolist.IsVisible = false;
            }
            else
            {
                var countofcharacters = e.NewTextValue.Length;

                if (countofcharacters > 2)
                {
                    var Characters = e.NewTextValue;
                    var filteredmeds = new ObservableCollection<videos>(AllVideosList.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);

                    allvideolist.ItemsSource = filteredmeds;
                    allvideolist.IsVisible = true;
                    allvideolist.HeightRequest = 50 * filteredmeds.Count();
                }

            }


        }
		catch(Exception ex)
		{

		}
    }

    private void backbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            Navigation.RemovePage(this);
        }
        catch(Exception ex)
        {

        }
    }

    async void getallhelpvideos()
    {
        try
        {

            AllVideosList = await aPICalls.GetAllHelpVideos();

           // allvideolist.ItemsSource = GetVideos;


        }
        catch(Exception ex)
        {

        }
    }

    private async void allvideolist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var selectedvideo = e.DataItem as videos;

           // firststack.IsVisible = false;
          //  MediaElement.IsVisible = true;


           // MediaElement.Source = selectedvideo.filename;
           // MediaElement.ShouldAutoPlay = true;


          await Navigation.PushAsync(new VideoPlayer(selectedvideo), false);
        }
        catch(Exception ex)
        {

        }
    }
}