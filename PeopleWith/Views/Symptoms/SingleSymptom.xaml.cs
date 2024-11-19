using Mopups.Services;
using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;
namespace PeopleWith;
public partial class SingleSymptom : ContentPage
{
    public ObservableCollection<symptomfeedback> SingleSymptomFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<symptomfeedback> TriggersFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<symptomfeedback> InterventionsFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<symptomfeedback> SymptomFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<symptomfeedback> Feedbacktodelete = new ObservableCollection<symptomfeedback>();
  //  public ObservableCollection<symptomprogression> SymptomChart = new ObservableCollection<symptomprogression>();
  //  public ObservableCollection<symptomprogression> SymptomData = new ObservableCollection<symptomprogression>();
    public ObservableCollection<usersymptom> PassedSymptom = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> AllSymptomData = new ObservableCollection<usersymptom>();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            NotasyncMethod(Ex);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }


    public SingleSymptom(ObservableCollection<usersymptom> SymptomPassed, ObservableCollection<usersymptom> AllSymptoms)
    {
        try
        {
            InitializeComponent();
            AllSymptomData = AllSymptoms;

            PassedSymptom = SymptomPassed;
            lblvalue.Text = PassedSymptom[0].CurrentIntensity;
            lblunit.Text = PassedSymptom[0].Score;
            var GetGet = DateTime.Parse(PassedSymptom[0].LastUpdatedTime); 
            datelbl.Text = GetGet.ToString("HH:mm, dd MMMM yyyy");

            NotifSwitch.ThumbColor = Color.FromHex("#E5E5E5");
            NotifSwitch.OnColor = Colors.ForestGreen;

            foreach (var item in PassedSymptom)
            {
                //if (item.Shorttitle.Contains("..."))
                //{
                //    //SymptomTitle.FontSize = 10; 
                //}
                //else
                //{
                //    //SymptomTitle.FontSize = 20;
                //}
                //SymptomTitle.Text = item.symptomtitle;
                symptomlbl.Text = item.symptomtitle;
                for (int i = 0; i < item.feedback.Count; i++)
                {
                    var AddFB = new symptomfeedback();
                    DateTime DateTimes = DateTime.Parse(item.feedback[i].timestamp);
                    string Convert = DateTimes.ToString("dd/MM/yy HH:mm");
                    AddFB.timestamp = Convert;
                    AddFB.symptomfeedbackid = item.feedback[i].symptomfeedbackid;
                    AddFB.intensity = item.feedback[i].intensity;

                    if (String.IsNullOrEmpty(item.feedback[i].interventions))
                    {
                        AddFB.interventions = "--";
                    }
                    else
                    {
                        string input = item.feedback[i].interventions;
                        string[] items = input.Split(',');
                        string formattedString = string.Join("\n", items);
                        AddFB.interventions = formattedString;
                        AddFB.InterventionBool = true;
                        AddFB.TriggerBool = false;
                    }
                    if (String.IsNullOrEmpty(item.feedback[i].triggers))
                    {
                        AddFB.triggers = "--";
                    }
                    else
                    {
                        string input = item.feedback[i].triggers;
                        string[] items = input.Split(',');
                        string formattedString = string.Join("\n", items);
                        AddFB.triggers = formattedString;
                        AddFB.InterventionBool = false;
                        AddFB.TriggerBool = true;
                    }
                    if (String.IsNullOrEmpty(item.feedback[i].notes) || item.feedback[i].notes == "null")
                    {
                        AddFB.notes = "--";
                    }
                    else
                    {
                        AddFB.notes = item.feedback[i].notes;
                    }
                    if (String.IsNullOrEmpty(item.feedback[i].duration) || item.feedback[i].duration == "null" || item.feedback[i].duration == "00 Hours 00 Minutes" || item.feedback[i].duration == "No Duration")
                    {
                        AddFB.duration = "--";
                    }
                    else
                    {
                        var split = item.feedback[i].duration;
                        var splits = split.Split(' ');
                        AddFB.duration = splits[0] + "H " + splits[2] + "m";
                    }
                    SymptomFeedback.Add(AddFB);
                }
            }

            var firstSymptomFeedback = SymptomFeedback
        .OrderBy(f => DateTime.Parse(f.timestamp))
        .FirstOrDefault();
            SingleSymptomFeedback.Add(firstSymptomFeedback);

            if (SymptomFeedback.Count > 1)
            {
                var allIntensities = new List<int>();
                foreach (var item in SymptomFeedback)
                {
                    allIntensities.Add(Int32.Parse(item.intensity));
                }
                int largest = allIntensities.Max();
                int smallest = allIntensities.Min();
                double average = allIntensities.Average();
                Lowestlbl.Text = smallest.ToString();
                Highestlbl.Text = largest.ToString();
                Averagelbl.Text = average.ToString("F0");
                int num = SymptomFeedback.Count - 2;
                Percentagelbl.Text = SymptomFeedback[num].intensity;
            }
            else
            {
                Highestlbl.Text = "N/A";
                Lowestlbl.Text = "N/A";
                Averagelbl.Text = "N/A";
                Percentagelbl.Text = "N/A";
            }
            foreach (var item in SymptomFeedback)
            {
                if (String.IsNullOrEmpty(item.triggers) || item.triggers != "--")
                {

                    TriggersFeedback.Add(item);

                }
                if (String.IsNullOrEmpty(item.interventions) || item.interventions != "--")
                {

                    InterventionsFeedback.Add(item);

                }
            }


            ConfigureChart();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void ConfigureChart()
    {
        try
        {
            foreach (var item in PassedSymptom)
            {
                ChartMarkerSettings chartMarker = new ChartMarkerSettings();
                chartMarker.Type = ShapeType.Circle;
                chartMarker.Fill = Colors.White;
                chartMarker.Stroke = Color.FromRgba("#031926");
                chartMarker.StrokeWidth = 2;
                chartMarker.Height = 8;
                chartMarker.Width = 8;

                ChartZoomPanBehavior zooming = new ChartZoomPanBehavior()
                {
                    EnablePinchZooming = true
                };

                SymptomProgChart.ZoomPanBehavior = zooming;

                DataPointSelectionBehavior selection = new DataPointSelectionBehavior();
                selection.SelectionChanged += OnSelectionChanged;

                LineSeries series = new LineSeries
                {
                    ItemsSource = item.feedback.OrderBy(f => DateTime.Parse(f.timestamp)),
                    XBindingPath = "timestamp",
                    YBindingPath = "intensity",
                    EnableTooltip = true,
                    ShowTrackballLabel = true,
                    EnableAnimation = true,
                    ShowMarkers = true,
                    MarkerSettings = chartMarker,
                    StrokeWidth = 2,
                    Fill = Colors.Orange,
                    SelectionBehavior = selection,
                };
                //series.MarkerSettings = new ChartMarkerSettings
                //{
                //    Type = Syncfusion.Maui.Charts.ShapeType.Circle,
                //    StrokeWidth = 2,
                //    Height = 8,
                //    Stroke = Colors.Orange,
                //    Width = 8,
                //    Fill = Colors.White
                //};
                SymptomProgChart.Series.Add(series);
            }

          

            if (SymptomFeedback.Count > 1)
            {
                var allTimestamps = new List<DateTime>();
                foreach (var item in PassedSymptom)
                {
                    foreach (var x in item.feedback)
                    {
                        if (DateTime.TryParse(x.timestamp, out DateTime timestamp))
                        {
                            allTimestamps.Add(timestamp);
                        }
                    }
                }
                DateTime lastUpdate = allTimestamps.Max();
                DateTime Firstupdate = allTimestamps.Min();
                if (Firstupdate.ToString("dd MMM") == lastUpdate.ToString("dd MMM"))
                {
                    symptomProgression.Text = "DateRange: " + Firstupdate.ToString("dd MMM");
                }
                else
                {
                    symptomProgression.Text = "DateRange: " + Firstupdate.ToString("dd MMM") + " - " + lastUpdate.ToString("dd MMM");
                }

                datestartedlbl.Text = Firstupdate.ToString("dd MMM");
            }
            else
            {
                DateTime GetDate = DateTime.Parse(SymptomFeedback[0].timestamp);
                datestartedlbl.Text = GetDate.ToString("dd MMM");
                symptomProgression.Text = "DateRange: " + GetDate.ToString("dd MMM");
            }
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async private void OnSelectionChanged(object sender, ChartSelectionChangedEventArgs e)
    {
        try
        {

            var es = e.NewIndexes[0];
            var GetFeedbackSelected = PassedSymptom[0].feedback;
            lblvalue.Text = GetFeedbackSelected[es].intensity;
            int Score = Int32.Parse(GetFeedbackSelected[es].intensity);
            var Scorelbl = String.Empty; 
            if (Score == 0)
            {
                Scorelbl = "Absent";
            }
            else if (Score > 0 && Score <= 25)
            {
                Scorelbl = "Mild";
            }
            else if (Score > 25 && Score <= 50)
            {
                Scorelbl = "Moderate";
            }
            else if (Score > 50 && Score <= 75)
            {
                Scorelbl = "High";
            }
            else if (Score > 75 && Score <= 100)
            {
                Scorelbl = "Severe";
            }
            lblunit.Text = Scorelbl; 
            var convertdate = DateTime.Parse(GetFeedbackSelected[es].timestamp);
            datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }
    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddDataBtn.IsEnabled = false;
                var AddData = "Add";
                await Navigation.PushAsync(new UpdateSingleSymptom(PassedSymptom, AddData, AllSymptomData));
                AddDataBtn.IsEnabled = true;
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
    
    async private void Button_Clicked_1(object sender, EventArgs e)
    {
        //Delete Symptom 
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                DeleteBtn.IsEnabled = false;
                bool Delete = await DisplayAlert("Delete Symptom", "Are you sure you would like the delete this Symptom? Once deleted it cannot be retrieved", "Delete", "Cancel");
                if (Delete == true)
                {
                    foreach (var item in PassedSymptom)
                    {
                        item.deleted = true;
                    }
                    APICalls database = new APICalls();
                    await database.DeleteSymptom(PassedSymptom);
                    DeleteBtn.IsEnabled = true;
                    //Symptom Deleted Message
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Deleted") { });
                    await Task.Delay(1500);


                    await MopupService.Instance.PopAllAsync(false);

                    foreach (var item in AllSymptomData)
                    {
                        if (item.id == PassedSymptom[0].id)
                        {
                            item.deleted = PassedSymptom[0].deleted;
                        }
                    }
                    await Navigation.PushAsync(new AllSymptoms(AllSymptomData));
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllSymptoms);
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                }
                else
                {
                    DeleteBtn.IsEnabled = true;
                    return;
                }
               
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

    async private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        try
        {
            
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }

    private async void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ShowAllSymptomData(PassedSymptom, PassedSymptom[0].feedback , AllSymptomData), false);
        }
        catch(Exception Ex)
        {
           NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Add Symptom Info Here
            await DisplayAlert("Symptom Information", "No Information is saved against this Symptom", "Close");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Switch_Toggled_1(object sender, ToggledEventArgs e)
    {

    }
}