using Mopups.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

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
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
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


    private void NovoConsentData()
    {
        try
        {
            if (!String.IsNullOrEmpty(Helpers.Settings.SignUp))
            {
                var signup = Helpers.Settings.SignUp;
                if (signup.Contains("SAX"))
                { //All Novo SignupCodes 
                    NovoConsent.IsVisible = true;
                    NovoContentlbl.Text = Preferences.Default.Get("NovoContent", String.Empty);
                    NovoExitidlbl.Text = Preferences.Default.Get("NovoExitid", String.Empty);
                }
            }
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
                DiagLoading.IsVisible = true; 
                var Userid = Helpers.Settings.UserKey;
                APICalls database = new APICalls();

                var getDiagnosisTask = database.GetUserDiagnosisAsync(Userid);

                //var delayTask = Task.Delay(1000);

                //if (await Task.WhenAny(getDiagnosisTask, delayTask) == delayTask)
                //{
                    //await MopupService.Instance.PushAsync(new GettingReady("Loading Diagnosis") { });
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
                    item.dateofdiagnosis = "Unknown";

                    //For Pending Diagnosis Date
                    //item.PendingDiag = true;
                    //item.ActiveDiag = false;
                    //item.status = "Pending";
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

            //Code For Pending Medication From Nsat Reg Process
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
                //NovoConsent.Margin = new Thickness(20, 0, 20, 10);
            }
            else
            {
                EmptyStack.IsVisible = true;
                DiagnosisOverview.IsVisible = false;
                //NovoConsent.Margin = new Thickness(20, 300, 20, 10);
            }

            DiagLoading.IsVisible = false;
            //await MopupService.Instance.PopAllAsync(false);
            NovoConsentData();
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