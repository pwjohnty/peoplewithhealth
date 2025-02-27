using CommunityToolkit.Mvvm.Messaging;
using Mopups.PreBaked.AbstractClasses;
using Mopups.Services;
using Newtonsoft.Json;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Text.Json;

namespace PeopleWith;

public partial class AddExercise : ContentPage
{

    public ObservableCollection<exercise> AllExercises = new ObservableCollection<exercise>();
    public ObservableCollection<exercise> FilterResults = new ObservableCollection<exercise>();
    public ObservableCollection<exercise> FilterTabsList = new ObservableCollection<exercise>();
    //public ObservableCollection<userexercise> AllUserExercises = new ObservableCollection<userexercise>();
    //public userexercise NewuserExercise = new userexercise();
    //public userexercise InvestPassed = new userexercise();
    //public notesuserfeedback NotesPassed = new notesuserfeedback();
    public bool isEdit = false;
    public bool NoteUpdate = false;
    private bool FilterTabClicked = false;

    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
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

    public AddExercise()
	{
		InitializeComponent();
        //Here for now 
        GetDietInfo(); 

    }

    async private void GetDietInfo()
    {
        try
        {
            Exerciseloading.IsVisible = true;
            var Userid = Helpers.Settings.UserKey;
            APICalls database = new APICalls();

            //Get Investigation Details 
            var getExerciseTask = database.GetExerciseDetails();
            AllExercises = await getExerciseTask;

            //Remove Already Added items from List
            if (AllExercises.Count > 0)
            {
               // var InvestAdded = new ObservableCollection<exercise\>(AllExercises.Where(s => AllUserExercises.Any(x => x.investigationname == s.exercisetitle)));

               // foreach (var item in InvestAdded)
               // {
               //     AllExercises.Remove(item);
               // }
            }
            //FilterTabs 
            ExerciseListview.ItemsSource = AllExercises.OrderBy(s => s.exercisetitle);
            var count = AllExercises.Count.ToString();
            //Results inital count
            Results.Text = "Results" + " (" + count + ")";
            FilterResults = AllExercises;


            //Add Classiciation Filters 
            var distinctinvest = AllExercises
                .GroupBy(s => s.ShortGroup)
                .Select(g => g.First())
                .ToList().OrderBy(g => g.ShortGroup);

            FilterTabsList = new ObservableCollection<exercise>(distinctinvest);

            // Insert "All" at the beginning of the list
            var AddAll = new exercise
            {
                ShortGroup = "All"
            };
            FilterTabsList.Insert(0, AddAll);
            FilterTabs.ItemsSource = FilterTabsList;
            FilterTabs.DisplayMemberPath = "ShortGroup";
            Filterstack.IsVisible = true;
            //Symptomloading.IsVisible = false;

            //addtimepicker.Time = DateTime.Now.TimeOfDay;

            Exerciseloading.IsVisible = false;
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    private void Backbutton_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private void FilterTabs_ChipClicked(object sender, EventArgs e)
    {

    }

    private void ExerciseListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }
}