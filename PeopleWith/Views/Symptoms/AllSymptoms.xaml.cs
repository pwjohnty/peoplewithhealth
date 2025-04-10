using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Mopups.Services;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;
namespace PeopleWith;
public partial class AllSymptoms : ContentPage
{
    public ObservableCollection<usersymptom> AllUserSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> itemstoremove = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> UserSymptomPassed = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> Test = new ObservableCollection<usersymptom>();
    public usersymptom GetAllSymptoms = new usersymptom();
    string previous;
    APICalls aPICalls = new APICalls();
    bool addsymptom;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch(Exception ex)
        {
            //Dunno 
        }
    }

    public AllSymptoms()
    {
        try
        {
            InitializeComponent();
            GetUserSymptoms();

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
      
    }

    public AllSymptoms(userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();

            userfeedbacklistpassed = userfeedbacklist;

            GetUserSymptoms();
            //CrashTest();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    public AllSymptoms(ObservableCollection<usersymptom> AllSymptoms, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            addsymptom = true;
            AllUserSymptoms.Clear();
            userfeedbacklistpassed = userfeedbacklist;

            AllUserSymptoms = AllSymptoms;
            GetUserSymptoms();
           
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
    async private void GetUserSymptoms()
    {
        try
        {

            if (!addsymptom)
            {
                SymLoading.IsVisible = true; 
                APICalls aPICalls = new APICalls();

                var getSymptomsTask = aPICalls.GetUserSymptomAsync();

                //var delayTask = Task.Delay(500);

                //if (await Task.WhenAny(getSymptomsTask, delayTask) == delayTask)
                //{
                   // await MopupService.Instance.PushAsync(new GettingReady("Loading Symptoms") { });
                //}

                AllUserSymptoms = await getSymptomsTask;

                
            }


            foreach(var item in AllUserSymptoms)
            {
                var allIntensities = new List<int>();
                var allTimestamps = new List<DateTime>();
                foreach (var x in item.feedback)
                {
                    if (x.action == "deleted")
                    {
                        //do nothing
                    }
                    else
                    {
                        if (int.TryParse(x.intensity, out int intensity))
                        {
                            allIntensities.Add(intensity);
                        }
                        if (DateTime.TryParse(x.timestamp, out DateTime timestamp))
                        {
                            allTimestamps.Add(timestamp);
                        }
                    }
                }
                if (allIntensities.Count > 0 && allTimestamps.Count > 0)
                {
                    int largest = allIntensities.Max();
                    int smallest = allIntensities.Min();
                    double average = allIntensities.Average();
                    DateTime lastUpdate = allTimestamps.Max();
                    var current = item.feedback.Where(f => f.action != "deleted").OrderByDescending(f => DateTime.Parse(f.timestamp)).FirstOrDefault()?.intensity;
                    int Score = Int32.Parse(current);
                    var Scorelabel = "";
                    if (Score == 0)
                    {
                        Scorelabel = "Absent";
                    }
                    else if (Score > 0 && Score <= 25)
                    {
                        Scorelabel = "Mild";
                    }
                    else if (Score > 25 && Score <= 50)
                    {
                        Scorelabel = "Moderate";
                    }
                    else if (Score > 50 && Score <= 75)
                    {
                        Scorelabel = "High";
                    }
                    else if (Score > 75 && Score <= 100)
                    {
                        Scorelabel = "Severe";
                    }
                    if (item.feedback.Count > 1)
                    {
                        var count = allIntensities.Count - 2;
                        if(count == -1)
                        {
                            count = 0;
                        }
                        allIntensities[count].ToString();
                        previous = allIntensities[count].ToString();
                    }
                    else
                    {
                        previous = "N/A";
                    }

                    //old
                    item.LastUpdated = lastUpdate.ToString("dd MMM");
                    item.LastUpdatedTime = lastUpdate.ToString("HH:mm, dd MMMM yyyy");
                    item.CurrentIntensity = current;
                    item.Score = Scorelabel;
                    item.LowIntensity = smallest.ToString();
                    item.HighIntensity = largest.ToString();
                    item.IntensityAverage = average.ToString("F0");
                    item.PreviousIntensity = previous;
                    //title too long, trim 
                    var title = item.symptomtitle;
                    if (title.Length > 30)
                    {

                        var trimtitle = item.symptomtitle;
                        string cut = trimtitle.Substring(0, 30);
                        item.Shorttitle = cut + "...";
                    }
                    else
                    {
                        item.Shorttitle = item.symptomtitle;
                    }


                }
            }

            SymptomOverview.IsVisible = true;

         
            foreach (var item in AllUserSymptoms)
            {
                if (item.deleted == true)
                {
                    itemstoremove.Add(item);
                }

                if(item.feedback.Count == 1)
                {
                    item.Firstadd = true;
                }
                else
                {
                    item.Firstadd = false;
                }
            }
            foreach (var item in itemstoremove)
            {
                AllUserSymptoms.Remove(item);
            }
            if (AllUserSymptoms.Count == 0)
            {
                EmptyStack.IsVisible = true;
                SymptomOverview.IsVisible = false;
                //NovoConsent.Margin = new Thickness(20, 300, 20, 10);
            }
            else
            {
                EmptyStack.IsVisible = false;
                SymptomOverview.IsVisible = true;

                var orderlist = AllUserSymptoms.OrderByDescending(x => DateTime.Parse(x.LastUpdatedTime)).ToList();
                AllSymptomView.ItemsSource = orderlist;
                //NovoConsent.Margin = new Thickness(20, 0, 20, 10);
                //populatelsitview();
            }
            SymLoading.IsVisible = false;
            NovoConsentData();
            //await MopupService.Instance.PopAllAsync(false);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async public void populatelsitview()
    {
        try
        {
            //  ListviewData.Clear();

            foreach (var item in AllUserSymptoms)
            {
                var allIntensities = new List<int>();
                var allTimestamps = new List<DateTime>();
                foreach (var x in item.feedback)
                {
                    if (int.TryParse(x.intensity, out int intensity))
                    {
                        allIntensities.Add(intensity);
                    }
                    if (DateTime.TryParse(x.timestamp, out DateTime timestamp))
                    {
                        allTimestamps.Add(timestamp);
                    }
                }
                if (allIntensities.Count > 0 && allTimestamps.Count > 0)
                {
                    int largest = allIntensities.Max();
                    int smallest = allIntensities.Min();
                    double average = allIntensities.Average();
                    DateTime lastUpdate = allTimestamps.Max();
                    var current = item.feedback.OrderByDescending(f => DateTime.Parse(f.timestamp)).FirstOrDefault()?.intensity;
                    int Score = Int32.Parse(current);
                    var Scorelabel = "";
                    if (Score == 0)
                    {
                        Scorelabel = "Absent";
                    }
                    else if (Score > 0 && Score <= 25)
                    {
                        Scorelabel = "Mild";
                    }
                    else if (Score > 25 && Score <= 50)
                    {
                        Scorelabel = "Moderate";
                    }
                    else if (Score > 50 && Score <= 75)
                    {
                        Scorelabel = "High";
                    }
                    else if (Score > 75 && Score <= 100)
                    {
                        Scorelabel = "Severe";
                    }
                    if (item.feedback.Count > 1)
                    {
                        var count = allIntensities.Count - 2;
                        allIntensities[count].ToString();
                        previous = allIntensities[count].ToString();
                    }
                    else
                    {
                        previous = "N/A";
                    }

                    //Add items to AlluserSymptoms
                   
                    item.LastUpdated = lastUpdate.ToString("dd/MM/yyyy HH:mm");
                    item.LastUpdatedTime = lastUpdate.ToString("HH:mm, dd MMMM yyyy");
                    item.CurrentIntensity = current;
                    item.Score = Scorelabel;
                    item.LowIntensity = smallest.ToString();
                    item.HighIntensity = largest.ToString();
                    item.IntensityAverage = average.ToString("F0");
                    item.PreviousIntensity = previous;
                    //title too long, trim 
                    var title = item.symptomtitle;
                    if (title.Length > 20)
                    {

                        var trimtitle = item.symptomtitle;
                        string cut = trimtitle.Substring(0, 20);
                        item.Shorttitle = cut + "...";
                    }
                    else
                    {
                        item.Shorttitle = item.symptomtitle;
                    }

                }

            }
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                //AllSymptomView.HeightRequest = ListviewData.Count * 100;
            }

            var sortedSymptoms = AllUserSymptoms.OrderByDescending(f => DateTime.Parse(f.LastUpdated)).ToList();
            AllUserSymptoms.Clear();
            foreach (var symptom in sortedSymptoms)
            {
                AllUserSymptoms.Add(symptom);
            }

            var orderlist = AllUserSymptoms.OrderByDescending(x => DateTime.Parse(x.LastUpdatedTime)).ToList();

            AllSymptomView.ItemsSource = orderlist;
            //GetAllSymptoms.AllSymptoms = AllUserSymptoms;
            //AllSymptomView.ItemsSource = GetAllSymptoms.AllSymptoms;
            //AllSymptomView.ItemsSource = sortedSymptoms; 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    public int FindLargest(List<int> list)
    {
        try
        {
            int max = int.MinValue;
            foreach (var num in list)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return 0;
        }      
    }
    public int FindSmallest(List<int> list)
    {
        try
        {
            int min = int.MaxValue;
            foreach (var num in list)
            {
                if (num < min)
                {
                    min = num;
                }
            }
            return min;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return 0;
        }
     
    }
     public  double CalculateAverage(List<int> list)
    {
        try
        {
            int sum = 0;
            foreach (var num in list)
            {
                sum += num;
            }
            return (double)sum / list.Count;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            return 0;
        }
        
    }
    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddSymptom(AllUserSymptoms, userfeedbacklistpassed));
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void AllSymptomView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
     
        try
        {
            UserSymptomPassed.Clear();

            var item = e.DataItem as usersymptom;

            UserSymptomPassed.Add(item);

            await Navigation.PushAsync(new SingleSymptom(UserSymptomPassed, AllUserSymptoms, userfeedbacklistpassed), false);
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
           await Navigation.PopAsync();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
       
    }
    async private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                UpdateFrame.InputTransparent = true; 
                if (AllUserSymptoms.Count == 0)
                {
                    await DisplayAlert("No Symptoms Added", "Try adding a symptom to access this feature", "Close");
                }
                else
                {
                    await Navigation.PushAsync(new UpdateAllSymptoms(AllUserSymptoms, userfeedbacklistpassed));
                }
                UpdateFrame.InputTransparent = false; 
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
    async private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                CompareFrame.InputTransparent = true; 
                if (AllUserSymptoms.Count == 0)
                {
                    await DisplayAlert("No Symptoms Added", "Try adding a symptom to access this feature", "Close");
                }
                else
                {
                    await Navigation.PushAsync(new CompareSymptoms(AllUserSymptoms));
                }
                CompareFrame.InputTransparent = false;
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
    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //await Navigation.PushAsync(new AddSymptom(AllUserSymptoms));
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void addNewToolbar_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                await Navigation.PushAsync(new AddSymptom(AllUserSymptoms, userfeedbacklistpassed));
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

    async private void SympInfoTapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PushAsync(new Infopopup("symptom") { });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}