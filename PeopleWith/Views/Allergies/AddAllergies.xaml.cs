using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AddAllergies : ContentPage
{
    public ObservableCollection<allergies> allergies = new ObservableCollection<allergies>();
    public ObservableCollection<allergies> FilterResults = new ObservableCollection<allergies>();
    public ObservableCollection<userallergies> AlluserAllergies = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> AllergytoAdd = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> AddedAllergy = new ObservableCollection<userallergies>();
    public ObservableCollection<allergies> itemstoremove = new ObservableCollection<allergies>();
    //private PopupViewModel viewModel;

    public AddAllergies()
    {
        InitializeComponent();
    }

    public AddAllergies(ObservableCollection<allergies> allergieslist, ObservableCollection<userallergies> AllAllergiesPassed)
    {
        try
        {
            InitializeComponent();
            AlluserAllergies = AllAllergiesPassed;
            allergies = allergieslist;
            //viewModel = BindingContext as PopupViewModel;

            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();  
            Allergyloading.IsVisible = true;

            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //await DisplayAlert("Time", ts.ToString(), "Close");

            //Get Allergies user already added to remove from listview
            var AllergyAdded = new ObservableCollection<allergies>(allergies.Where(s => AllAllergiesPassed.Any(x => x.title == s.Title)));

            foreach (var item in AllergyAdded)
            {
                itemstoremove.Add(item);
            }

            //Remove items from list
            foreach (var item in itemstoremove)
            {
                allergies.Remove(item);
            }

            FilterResults = allergies;

            AllergyListview.ItemsSource = allergies.OrderBy(s => s.Title);
            var count = allergies.Count.ToString();

            //Results inital count
            Results.Text = "Results" + " (" + count + ")";

            Allergyloading.IsVisible = false;
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }


    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = searchbar.Text.ToString();
            var filteredSymptoms = new ObservableCollection<allergies>(FilterResults.Where(s => s.Title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.Title);

            AllergyListview.ItemsSource = filteredSymptoms;

            var count = filteredSymptoms.Count().ToString();

            Results.Text = "Results" + " (" + count + ")";
        }
        catch (Exception Ex)
        {

        }

    }

    async private void AllergyListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            if (searchbar.IsSoftInputShowing())
            await searchbar.HideSoftInputAsync(System.Threading.CancellationToken.None);
            var Allergy = e.DataItem as allergies;
            var Allergytitle = Allergy.Title.ToString();
            //popup.HeaderTitle = Allergy.Title.ToString();
            //popup.IsOpen = true;
            //bool result = await viewModel.ShowPopupAsync();
            bool result = await DisplayAlert(Allergytitle, "Would you like to add this Allergy?", "Accept", "Decline");

            if (result)
            {

                NavigationPage.SetHasNavigationBar(this, false);
                var Userid = Helpers.Settings.UserKey;

                var NewAllergy = new userallergies();
                NewAllergy.userid = Userid;
                NewAllergy.allergyid = Allergy.Allergyid;

                AllergytoAdd.Add(NewAllergy);

                APICalls database = new APICalls();
                AddedAllergy = await database.PostUserAllergiesAsync(AllergytoAdd);
                await MopupService.Instance.PushAsync(new PopupPageHelper("Allery Added") { });
                await Task.Delay(1500);

                foreach (var item in AddedAllergy)
                {
                    AlluserAllergies.Add(item);
                }
                foreach (var item in AlluserAllergies)
                {
                    for (int i = 0; i < allergies.Count; i++)
                    {
                        if (allergies[i].Allergyid == item.allergyid)
                        {
                            item.title = allergies[i].Title;
                        }

                    }
                }
                await MopupService.Instance.PopAllAsync(false);
                await Navigation.PushAsync(new AllAllergies(AlluserAllergies, allergies));
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllAllergies);
                if (pageToRemoves != null)
                {

                    Navigation.RemovePage(pageToRemoves);
                }
                Navigation.RemovePage(this);

            }
            else
            {
                // Declined
                AllergyListview.SelectedItems.Clear(); 
            }
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }

    }
}