using System.Collections.ObjectModel;
using Mopups.Services;

namespace PeopleWith;

public partial class SingleAllergies : ContentPage
{
    public ObservableCollection<userallergies> AllUserAllergies = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> AllergyPassed = new ObservableCollection<userallergies>();
    public ObservableCollection<allergies> Allergies = new ObservableCollection<allergies>();

    public SingleAllergies()
    {
        InitializeComponent();
    }

    public SingleAllergies(ObservableCollection<userallergies> PassedAllergy, ObservableCollection<userallergies> AllAllergies, ObservableCollection<allergies> PassedAllergies)
    {
        try
        {
            InitializeComponent();
            AllergyPassed = PassedAllergy;
            AllUserAllergies = AllAllergies;
            Allergies = PassedAllergies;

            AlergyTitle.Text = AllergyPassed[0].title;
            AllergyDate.Text = AllergyPassed[0].createdAt;
        }
        catch (Exception ex)
        {
            //Add Crash log 
        }

    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await DisplayAlert("Allergy Information", "There is no Information against this Allergy", "Close");
        }
        catch (Exception ex)
        {
            //Add Crash log 
        }
    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool Delete = await DisplayAlert("Delete Allergy", "Are you sure you would like the delete this Allergy? Once deleted it cannot be retrieved", "Delete", "Cancel");
            if (Delete == true)
            {

                foreach (var item in AllergyPassed)
                {
                    item.deleted = true;
                }

                APICalls database = new APICalls();
                await database.DeleteUserAllergy(AllergyPassed);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Allery Deleted") { });
                await Task.Delay(1500);

                foreach (var item in AllUserAllergies)
                {
                    if (item.id == AllergyPassed[0].id)
                    {
                        item.deleted = true;
                    }
                }
                await MopupService.Instance.PopAllAsync(false);
                await Navigation.PushAsync(new AllAllergies(AllUserAllergies, Allergies));

                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllAllergies);

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
        catch (Exception ex)
        {
            //Add Crash log 
        }


    }
}