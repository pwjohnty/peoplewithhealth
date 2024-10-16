using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleMood : ContentPage
{
    public ObservableCollection<usermood> AlluserMoods = new ObservableCollection<usermood>();
    public ObservableCollection<usermood> MoodPassed = new ObservableCollection<usermood>();

    public SingleMood()
    {
        InitializeComponent();
    }

    public SingleMood(ObservableCollection<usermood> singlemood, ObservableCollection<usermood> AllMoods)
    {
        try
        {
            InitializeComponent();
            AlluserMoods = AllMoods;
            MoodPassed = singlemood;

            Moodimg.Source = MoodPassed[0].source;
            MoodTitle.Text = MoodPassed[0].title;
            MoodDate.Text = MoodPassed[0].datetime;
            if (string.IsNullOrEmpty(MoodPassed[0].notes))
            {
                MoodNotes.Text = "No Notes Added";
            }
            else
            {
                MoodNotes.Text = "Notes: " + MoodPassed[0].notes;
            }
        }
        catch (Exception Ex)
        {
            //Add Crash log 
        }

    }

    async private void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddMood(AlluserMoods, MoodPassed[0].id));
        }
        catch (Exception Ex)
        {
            //Add Crash log 
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await DisplayAlert("Mood Information", "There is no Information against this mood", "Close");
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }

    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool Delete = await DisplayAlert("Delete Mood", "Are you sure you would like the delete this Mood? Once deleted it cannot be retrieved", "Delete", "Cancel");
            if (Delete == true)
            {

                foreach (var item in MoodPassed)
                {
                    item.deleted = true;
                }

                APICalls database = new APICalls();
                await database.DeleteUserMood(MoodPassed);
                await MopupService.Instance.PushAsync(new PopupPageHelper("Mood Deleted") { });
                await Task.Delay(1500);

                foreach (var item in AlluserMoods)
                {
                    if (item.id == MoodPassed[0].id)
                    {
                        item.deleted = true;
                    }
                }

                await MopupService.Instance.PopAllAsync(false);
                await Navigation.PushAsync(new AllMood(AlluserMoods));

                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllMood);

                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                Navigation.RemovePage(this);

                return;
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }

    }
}