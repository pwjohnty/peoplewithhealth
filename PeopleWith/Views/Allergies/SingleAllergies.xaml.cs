using System.Collections.ObjectModel;
using Mopups.Services;
using Microsoft.Maui.Networking;

namespace PeopleWith;

public partial class SingleAllergies : ContentPage
{
    public ObservableCollection<userallergies> AllUserAllergies = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> AllergyPassed = new ObservableCollection<userallergies>();
    public ObservableCollection<allergies> Allergies = new ObservableCollection<allergies>();
    allergies SelectedAllergy = new allergies(); 
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

            loadMedInformation();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async void loadMedInformation()
    {
        try
        {
            SelectedAllergy.Allergyid = AllergyPassed[0].allergyid;
            APICalls Database = new APICalls();
            var GetDiagInfo = await Database.GetAsyncSingleAllergy(SelectedAllergy);
            SelectedAllergy = GetDiagInfo;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if (SelectedAllergy.Allergyinformation != null)
            {
                var title = AllergyPassed[0].title;
                await Navigation.PushAsync(new AllergyInfo(SelectedAllergy, title), false);

            }
            else
            {
                await DisplayAlert("Diagnosis Information", "No information or resources available for this Allergy", "Close");

            }
            //await DisplayAlert("Allergy Information", "There is no Information against this Allergy", "Close");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                
                //Limit No. of Taps 
                Deletebtn.IsEnabled = false;
                bool Delete = await DisplayAlert("Delete Allergy", "Are you sure you want to delete this Allergy? Once deleted it cannot be retrieved", "Delete", "Cancel");
                if (Delete == true)
                {

                    foreach (var item in AllergyPassed)
                    {
                        item.deleted = true;
                    }

                    APICalls database = new APICalls();
                    await database.DeleteUserAllergy(AllergyPassed);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Allergy Deleted") { });
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
                    Deletebtn.IsEnabled = true;
                    return;
                }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
           
            }
            else
            {
                return;
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}