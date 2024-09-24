using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;
public partial class AllDiagnosis : ContentPage
{
    public ObservableCollection<userdiagnosis> DiagnosisList = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosisPassed = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> itemstoremove = new ObservableCollection<userdiagnosis>();
    bool initalload;
    public AllDiagnosis()
    {
        InitializeComponent();
        initalload = true; 
        GetAllUserDiagnosis(); 
    }

    public AllDiagnosis(ObservableCollection<userdiagnosis> AllDiagnosis)
    {
        try
        {
            InitializeComponent();
            initalload = false;
            DiagnosisList = AllDiagnosis;
            GetAllUserDiagnosis();
      
        }
        catch (Exception ex)
        {
            //Add Crash log
        }
    }

    async private void GetAllUserDiagnosis()
    {
        try
        {
            if(initalload == true)
            {
                var Userid = Helpers.Settings.UserKey;
                APICalls database = new APICalls();

                var getDiagnosisTask = database.GetUserDiagnosisAsync(Userid);

                var delayTask = Task.Delay(1000);

                if (await Task.WhenAny(getDiagnosisTask, delayTask) == delayTask)
                {
                    await MopupService.Instance.PushAsync(new GettingReady("Loading Diagnosis") { });
                }

                DiagnosisList = await getDiagnosisTask;

                await MopupService.Instance.PopAllAsync(false);
            }
    
            foreach (var item in DiagnosisList)
            {
                if (item.deleted == true)
                {
                    itemstoremove.Add(item);
                }
            }

            foreach (var item in itemstoremove)
            {
                DiagnosisList.Remove(item);
            }

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                AllDiagnosisView.HeightRequest = DiagnosisList.Count * 80;
            }

            var sortedDiagnosis = DiagnosisList.OrderByDescending(f => DateTime.Parse(f.dateofdiagnosis)).ToList();

            DiagnosisList.Clear();
            foreach (var Diagnosis in sortedDiagnosis)
            {
                DiagnosisList.Add(Diagnosis);
            }

            if (DiagnosisList.Count > 0)
            {
                AllDiagnosisView.ItemsSource = DiagnosisList;
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
            await Navigation.PushAsync(new AddDiagnosis(DiagnosisList));
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }

    }

    async private void AllDiagnosisView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            DiagnosisPassed.Clear();
            var Diagnosis = e.DataItem as userdiagnosis;
            var Title = Diagnosis.diagnosistitle;
            foreach (var item in DiagnosisList)
            {
                if (Title == item.diagnosistitle)
                {
                    DiagnosisPassed.Add(item);

                }
            }

            await Navigation.PushAsync(new SingleDiagnosis(DiagnosisPassed, DiagnosisList));
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }
}