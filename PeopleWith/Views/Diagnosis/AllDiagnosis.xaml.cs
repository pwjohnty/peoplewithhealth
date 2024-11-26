using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;
public partial class AllDiagnosis : ContentPage
{
    public ObservableCollection<userdiagnosis> DiagnosisList = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosisPassed = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> itemstoremove = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> PendingDiagnosis = new ObservableCollection<userdiagnosis>();
    bool initalload;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
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
    public AllDiagnosis()
    {
        try
        {
            InitializeComponent();
            initalload = true;
            GetAllUserDiagnosis();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }


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
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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

                //var delayTask = Task.Delay(1000);

                //if (await Task.WhenAny(getDiagnosisTask, delayTask) == delayTask)
                //{
                    await MopupService.Instance.PushAsync(new GettingReady("Loading Diagnosis") { });
                //}

                DiagnosisList = await getDiagnosisTask;

                
            }
    
            foreach (var item in DiagnosisList)
            {
                item.ActiveDiag = true; 
                item.PendingDiag = false;
                item.status = "Active";
                if (item.deleted == true)
                {
                    itemstoremove.Add(item);
                }
                else if (item.dateofdiagnosis == null)
                {
                    item.PendingDiag = true;
                    item.ActiveDiag = false;
                    item.status = "Pending";
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

            //var sortedDiagnosis = DiagnosisList.OrderByDescending(f => DateTime.Parse(f.dateofdiagnosis)).ToList();

            var sortedDiagnosis = DiagnosisList.OrderBy(x => x.status == "Pending" ? 0 : 1)
                    .ThenByDescending(x =>
                    {
                        DateTime parsedDate;
                        return DateTime.TryParse(x.dateofdiagnosis, out parsedDate) ? parsedDate : DateTime.MinValue;
                    }).ToList();

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

            await MopupService.Instance.PopAllAsync(false);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                await Navigation.PushAsync(new AddDiagnosis(DiagnosisList));
                AddBtn.IsEnabled = true;
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
            NotasyncMethod(Ex);
        }
    }

   async private void DiagInfoTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("diag") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}