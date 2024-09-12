using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class AllAllergies : ContentPage
{
    public ObservableCollection<userallergies> AllUserAllergies = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> itemstoremove = new ObservableCollection<userallergies>();
    public ObservableCollection<userallergies> PassedAllergy = new ObservableCollection<userallergies>();
    public ObservableCollection<allergies> AllergiesList = new ObservableCollection<allergies>();
    bool InitalLoad;

    public AllAllergies()
    {
        try
        {
            InitializeComponent();
            InitalLoad = true;
            GetAllUserAllergies(); 
        }
        catch (Exception ex)
        {
            //Add Crash log
        }
    }
    public AllAllergies(ObservableCollection<userallergies> AllAllergiesPassed, ObservableCollection<allergies> allergies)
    {
        try
        {
            InitializeComponent();
            AllUserAllergies = AllAllergiesPassed;
            AllergiesList = allergies;
            InitalLoad = false;
            GetAllUserAllergies();
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }

    async private void GetAllUserAllergies()
    {
        try
        {
            if (InitalLoad == true)
            {
                var Userid = Helpers.Settings.UserKey;
                APICalls database = new APICalls();
                AllUserAllergies = await database.GetUserAllergiesAsync(Userid);
                AllergiesList = await database.GetAsyncAllergies();
                foreach (var item in AllUserAllergies)
                {
                    for (int i = 0; i < AllergiesList.Count; i++)
                    {
                        if (AllergiesList[i].Allergyid == item.allergyid)
                        {
                            item.title = AllergiesList[i].Title;
                        }
                    }
                }
            }
            foreach (var item in AllUserAllergies)
            {
                if (item.deleted == true)
                {
                    itemstoremove.Add(item);
                }
            }

            foreach (var item in itemstoremove)
            {
                AllUserAllergies.Remove(item);
            }


            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                AllAllergiesView.HeightRequest = AllUserAllergies.Count * 80;
            }

            var sortedAllergies = AllUserAllergies.OrderByDescending(f => DateTime.Parse(f.createdAt)).ToList();

            AllUserAllergies.Clear();
            foreach (var Allergy in sortedAllergies)
            {
                AllUserAllergies.Add(Allergy);
            }

            if (AllUserAllergies.Count > 0)
            {
                AllAllergiesView.ItemsSource = AllUserAllergies;
                EmptyStack.IsVisible = false;
                DiagnosisOverview.IsVisible = true;
            }
            else
            {
                EmptyStack.IsVisible = true;
                DiagnosisOverview.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            //Add Crash logs 
        }

    }


    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddAllergies(AllergiesList, AllUserAllergies));
        }
        catch (Exception Ex)
        {
            //Add Crash Log
        }
    }

    async private void AllAllergiesView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            PassedAllergy.Clear();
            var allergy = e.DataItem as userallergies;
            var Title = allergy.title;
            foreach (var item in AllUserAllergies)
            {
                if (Title == item.title)
                {
                    PassedAllergy.Add(item);

                }
            }

            await Navigation.PushAsync(new SingleAllergies(PassedAllergy, AllUserAllergies, AllergiesList));
        }
        catch (Exception Ex)
        {
            //Add Crash Log
        }

    }
}
