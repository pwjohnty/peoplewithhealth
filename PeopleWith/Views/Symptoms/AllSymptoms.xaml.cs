using CommunityToolkit.Mvvm.Messaging;
using Syncfusion.Maui.ListView;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Mopups.Services;
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
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
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
            //CrashTest();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
      
    }

    public AllSymptoms(ObservableCollection<usersymptom> AllSymptoms)
    {
        try
        {
            InitializeComponent();
            addsymptom = true;
            AllUserSymptoms.Clear();

            AllUserSymptoms = AllSymptoms;
            GetUserSymptoms();
           
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    //async private void CrashTest()
    //{
    //    try
    //    {
    //        int number = 10;
    //        int result = number / 0;
    //    }
    //    catch(Exception Ex)
    //    {    
    //        await crashHandler.CrashDetectedSend(Ex);
    //    }
       
    //}
    async private void GetUserSymptoms()
    {
        try
        {
            if (!addsymptom)
            {
                APICalls aPICalls = new APICalls();

                var getSymptomsTask = aPICalls.GetUserSymptomAsync();

                var delayTask = Task.Delay(1000);

                if (await Task.WhenAny(getSymptomsTask, delayTask) == delayTask)
                {
                    await MopupService.Instance.PushAsync(new GettingReady("Loading Symptoms") { });
                }

                AllUserSymptoms = await getSymptomsTask;

                await MopupService.Instance.PopAllAsync(false);
            }


            foreach(var item in AllUserSymptoms)
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

                    item.LastUpdated = lastUpdate.ToString("dd MMM");
                    item.CurrentIntensity = current;
                    item.Score = Scorelabel;
                    item.LowIntensity = smallest.ToString();
                    item.HighIntensity = largest.ToString();
                    item.IntensityAverage = average.ToString("F0");
                    item.PreviousIntensity = previous;
                    //title too long, trim 
                    var title = item.symptomtitle;
                    if (title.Length > 25)
                    {

                        var trimtitle = item.symptomtitle;
                        string cut = trimtitle.Substring(0, 25);
                        item.Shorttitle = cut + "...";
                    }
                    else
                    {
                        item.Shorttitle = item.symptomtitle;
                    }


                }
            }

            AllSymptomView.ItemsSource = AllUserSymptoms;

            SymptomOverview.IsVisible = true;

          

            foreach (var item in AllUserSymptoms)
            {
                if (item.deleted == true)
                {
                    itemstoremove.Add(item);
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
            }
            else
            {
                EmptyStack.IsVisible = false;
                SymptomOverview.IsVisible = true;
                //populatelsitview();
            }

        }
        catch (Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex);
        }


    }
    async public void populatelsitview()
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

        GetAllSymptoms.AllSymptoms = AllUserSymptoms;
        AllSymptomView.ItemsSource = GetAllSymptoms.AllSymptoms;
    }
    public static int FindLargest(List<int> list)
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
        catch(Exception Ex)
        {
            //Fix Crash Log 
            return 0;
        }      
    }
    public static int FindSmallest(List<int> list)
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
        catch
        {
            //Add Crash log
            return 0;
        }
     
    }
    public static double CalculateAverage(List<int> list)
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
        catch
        {
            //Add Crash log
            return 0;
        }
        
    }
    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddSymptom(AllUserSymptoms));
        }
        catch (Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
    }
    async private void AllSymptomView_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
     
        try
        {
            UserSymptomPassed.Clear();

            var item = e.DataItem as usersymptom;

            UserSymptomPassed.Add(item);

            await Navigation.PushAsync(new SingleSymptom(UserSymptomPassed, AllUserSymptoms), false);
        }
        catch(Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex);
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
            await crashHandler.CrashDetectedSend(Ex);
        }
       
    }
    async private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        try
        {
            if (AllUserSymptoms.Count == 0)
            {
                await DisplayAlert("No Symptoms Added", "Try adding a symptom to access this feature", "Close");
            }
            else
            {
                await Navigation.PushAsync(new UpdateAllSymptoms(AllUserSymptoms));
            }

        }
        catch (Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
    }
    async private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        try
        {
            if (AllUserSymptoms.Count == 0)
            {
                await DisplayAlert("No Symptoms Added", "Try adding a symptom to access this feature", "Close");
            }
            else
            {
                await Navigation.PushAsync(new CompareSymptoms(AllUserSymptoms));
            }
        }
        catch (Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex);
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
            await crashHandler.CrashDetectedSend(Ex);
        }
    }
    async private void addNewToolbar_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new AddSymptom(AllUserSymptoms));
        }
        catch (Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
    }
}