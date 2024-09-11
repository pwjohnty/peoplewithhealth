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

    public SingleSymptom(ObservableCollection<usersymptom> SymptomPassed, ObservableCollection<usersymptom> AllSymptoms)
    {
        InitializeComponent();
        //var viewModel = new symptomprogress();
        //BindingContext = viewModel;
        AllSymptomData = AllSymptoms;
        
        PassedSymptom = SymptomPassed;
        lblvalue.Text = PassedSymptom[0].CurrentIntensity;
        lblunit.Text = PassedSymptom[0].Score;
        datelbl.Text = PassedSymptom[0].LastUpdated;
        //AvgDataSymptom = SymptomAvgData;

        List<String> Chippy = new List<string>();
        Chippy.Add("All");
        Chippy.Add("Triggers");
        Chippy.Add("Interventions");
        Chips.ItemsSource = Chippy;

        foreach (var item in PassedSymptom)
        {
            if (item.Shorttitle.Contains("..."))
            {
                //SymptomTitle.FontSize = 10; 
            }
            else
            {
                //SymptomTitle.FontSize = 20;
            }
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
        if (SymptomFeedback.Count > 1)
        {
            SeemoreStack.IsVisible = true;
        }
        else
        {
            SeemoreStack.IsVisible = false;
        }
        var firstSymptomFeedback = SymptomFeedback
    .OrderBy(f => DateTime.Parse(f.timestamp))
    .FirstOrDefault();
        SingleSymptomFeedback.Add(firstSymptomFeedback);
        AllDataLV.ItemsSource = SingleSymptomFeedback;
        AllDataResult.Text = "All Data (" + SymptomFeedback.Count.ToString() + ")";
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
       // GetTriggers();
    }
    //public async void GetTriggers()
    //{
    //    APICalls database = new APICalls();
    //    ObservableCollection<interventiontrigger> Triggersintervention = await database.GetAsyncInterventionTrigger();
    //    foreach (var item in Triggersintervention)
    //    {
    //        if (item.type == "trigger")
    //        {
    //            Triggers.Add(item);
    //        }
    //        else if (item.type == "intervention")
    //        {
    //            Interventions.Add(item);
    //        }
    //    }
    //}
    async private void ConfigureChart()
    {
        try
        {
            //var symptomProgress = new symptomprogress();
            //SymptomData = symptomProgress.AddDataPoint(SymptomData);
            //SymptomProgChart.BindingContext = SymptomData;
            foreach (var item in PassedSymptom)
            {
                LineSeries series = new LineSeries
                {
                    ItemsSource = item.feedback.OrderBy(f => DateTime.Parse(f.timestamp)),
                    XBindingPath = "timestamp",
                    YBindingPath = "intensity",
                    EnableTooltip = true,
                    ShowTrackballLabel = true,
                    EnableAnimation = true,
                    ShowMarkers = false,
                    StrokeWidth = 2,
                    Fill = Colors.Orange
                };
                series.MarkerSettings = new ChartMarkerSettings
                {
                    Type = Syncfusion.Maui.Charts.ShapeType.Circle,
                    StrokeWidth = 2,
                    Height = 8,
                    Stroke = Colors.Orange,
                    Width = 8,
                    Fill = Colors.White
                };
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
                symptomProgression.Text = "DateRange: " + GetDate.ToString("dd MMM");
            }
        }
        catch(Exception ex)
        {

        }
        //symptomProgression.Text = sy
        //SymptomProgChart.Series.Add(series);
    }
    async private void Button_Clicked(object sender, EventArgs e)
    {
        var AddData = "Add";
        await Navigation.PushAsync(new UpdateSingleSymptom(PassedSymptom, AddData, AllSymptomData));
    }
    //private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    //{
    //    var commandpar = e.NewValue.Text.ToString();
    //    if (commandpar == "7 Days")
    //    {
    //        int numero = 6;
    //        ConfigureChart(numero);
    //    }
    //    else if (commandpar == "14 Days")
    //    {
    //        int numero = 13;
    //        ConfigureChart(numero);
    //    }
    //    else if (commandpar == "21 Days")
    //    {
    //        int numero = 20;
    //        ConfigureChart(numero);
    //    }
    //    else if (commandpar == "?")
    //    {
    //        int numero = SymptomChart.Count;
    //        ConfigureChart(numero);
    //    }
    //}
    private void AllDataLV_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
    }
    //Delete Symptom 
    async private void Button_Clicked_1(object sender, EventArgs e)
    {
    //    foreach (var item in PassedSymptom)
    //    {
    //        item.deleted = true;
    //    }
    //    APICalls database = new APICalls();
    //    await database.DeleteSymptom(PassedSymptom[0].userid, PassedSymptom[0].symptomid, PassedSymptom);

    //    foreach (var item in AllSymptomData)
    //    {
    //        if (item.id == PassedSymptom[0].id)
    //        {
    //            item.deleted = PassedSymptom[0].deleted;
    //        }
    //    }
    //    await Navigation.PushAsync(new AllSymptoms(AllSymptomData));
    //    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllSymptoms);
    //    if (pageToRemoves != null)
    //    {
    //        Navigation.RemovePage(pageToRemoves);
    //    }
    //    Navigation.RemovePage(this);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (seeall.Text == "See All")
        {
            seeall.Text = "Hide All";
            arrow.Rotation = 90;
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                AllDataLV.HeightRequest = SymptomFeedback.Count * 70;
            }
            AllDataLV.ItemsSource = SymptomFeedback.OrderBy(f => DateTime.Parse(f.timestamp));
        }
        else
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                AllDataLV.HeightRequest = SingleSymptomFeedback.Count * 70;
            }
            AllDataLV.ItemsSource = SingleSymptomFeedback;
            seeall.Text = "See All";
            arrow.Rotation = 270;
        }
    }

    async private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
    }
    async private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
    //    var frame = sender as Frame;
    //    var symptom = frame?.BindingContext as symptomfeedback;
    //    foreach (var item in SymptomFeedback.OrderBy(f => DateTime.Parse(f.timestamp)))
    //    {
    //        if (SingleSymptomFeedback[0].symptomfeedbackid == symptom.symptomfeedbackid)
    //        {
    //            await DisplayAlert("Inital Feedback", "This Feedback cannot be Edited or Deleted", "Close");
    //            return;
    //        }
    //        if (item.symptomfeedbackid == symptom.symptomfeedbackid)
    //        {
    //            //issue clicking outside of displayalert causes answer to be true 
    //            bool answer = await DisplayAlert("Symptom Feedback", "Would you like to edit or delete this feedback?", "Edit", "Delete");
    //            if (answer == true)
    //            {
    //                //Edit
    //                var EditData = item.symptomfeedbackid;
    //                await Navigation.PushAsync(new UpdateSingleSymptom(PassedSymptom, AvgDataSymptom, EditData, Triggers, Interventions, AllSymptomData));
    //                return;
    //            }
    //            else
    //            {
    //                //Delete
    //                bool Delete = await DisplayAlert("Delete Symptom Feedback", "Are you sure you would like the delete this record?. Once deleted it cannot be retrieved", "Delete", "Cancel");
    //                if (Delete == true)
    //                {
    //                    var DeleteData = item.symptomfeedbackid;
    //                    DeleteFeedbackData(DeleteData);
    //                    return;
    //                }
    //                else
    //                {
    //                    return;
    //                }
    //            }
    //        }
    //    }
    }
    //async void DeleteFeedbackData(string delete)
    //{

    //    foreach (var item in PassedSymptom)
    //    {
    //        foreach (var items in item.feedback)
    //        {

    //            if (delete == items.symptomfeedbackid)
    //            {
    //                Feedbacktodelete.Add(items);
    //            }
    //        }
    //        foreach (var feedback in Feedbacktodelete)
    //        {
    //            item.feedback.Remove(feedback);
    //        }
    //    }
    //    APICalls database = new APICalls();
    //    var userid = Helpers.Settings.UserKey;
    //    await database.PutSymptomAsync(userid, delete, PassedSymptom);
    //    SymptomFeedback.Clear();
    //    foreach (var item in PassedSymptom)
    //    {
    //        if (item.Shorttitle.Contains("..."))
    //        {
    //            //SymptomTitle.FontSize = 10;
    //        }
    //        else
    //        {
    //            //SymptomTitle.FontSize = 20;
    //        }
    //        //SymptomTitle.Text = item.symptomtitle;
    //        TItlelbl.Text = item.symptomtitle;
    //        for (int i = 0; i < item.feedback.Count; i++)
    //        {
    //            var AddFB = new symptomfeedback();
    //            DateTime DateTimes = DateTime.Parse(item.feedback[i].timestamp);
    //            string Convert = DateTimes.ToString("dd/MM/yy HH:mm");
    //            AddFB.timestamp = Convert;
    //            AddFB.symptomfeedbackid = item.feedback[i].symptomfeedbackid;
    //            AddFB.intensity = item.feedback[i].intensity;
    //            if (String.IsNullOrEmpty(item.feedback[i].interventions))
    //            {
    //                AddFB.interventions = "--";
    //            }
    //            else
    //            {
    //                AddFB.interventions = item.feedback[i].interventions;
    //            }
    //            if (String.IsNullOrEmpty(item.feedback[i].triggers))
    //            {
    //                AddFB.triggers = "--";
    //            }
    //            else
    //            {
    //                AddFB.triggers = item.feedback[i].triggers;
    //            }
    //            if (String.IsNullOrEmpty(item.feedback[i].notes) || item.feedback[i].notes == "null")
    //            {
    //                AddFB.notes = "--";
    //            }
    //            else
    //            {
    //                AddFB.notes = item.feedback[i].notes;
    //            }
    //            if (String.IsNullOrEmpty(item.feedback[i].duration) || item.feedback[i].duration == "null" || item.feedback[i].duration == "00 Hours 00 Minutes")
    //            {
    //                AddFB.duration = "--";
    //            }
    //            else
    //            {
    //                var split = item.feedback[i].duration;
    //                var splits = split.Split(' ');
    //                AddFB.duration = splits[0] + "H " + splits[2] + "m";
    //            }
    //            SymptomFeedback.Add(AddFB);
    //        }
    //    }
    //    if (SymptomFeedback.Count > 1)
    //    {
    //        SeemoreStack.IsVisible = true;
    //        AllDataLV.ItemsSource = SymptomFeedback.OrderBy(f => DateTime.Parse(f.timestamp));
    //    }
    //    else
    //    {
    //        SeemoreStack.IsVisible = false;
    //        AllDataLV.ItemsSource = SingleSymptomFeedback;

    //    }
    //    AllDataResult.Text = "All Data (" + SymptomFeedback.Count.ToString() + ")";
    //    if (SymptomFeedback.Count > 1)
    //    {
    //        Highestlbl.Text = PassedSymptom[0].HighIntensity;
    //        Lowestlbl.Text = PassedSymptom[0].LowIntensity;
    //        Averagelbl.Text = PassedSymptom[0].IntensityAverage;
    //        Percentagelbl.Text = PassedSymptom[0].PreviousIntensity;
    //    }
    //    else
    //    {
    //        Highestlbl.Text = "N/A";
    //        Lowestlbl.Text = "N/A";
    //        Averagelbl.Text = "N/A";
    //        Percentagelbl.Text = "N/A";
    //    }
    //    //Chips.SelectedItem = "All";
    //    await Navigation.PushAsync(new AllSymptoms(AllSymptomData));
    //    var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllSymptoms);
    //    if (pageToRemove != null)
    //    {
    //        Navigation.RemovePage(pageToRemove);
    //    }
    //    Navigation.RemovePage(this);
    //}


    private void Chips_ChipClicked(object sender, EventArgs e)
    {
        SeemoreStack.IsVisible = false;
        AllDataResult.FontSize = 20;
        var getitem = sender as Syncfusion.Maui.Core.SfChip;
        var itemclicked = getitem.Text;
        if (itemclicked == "All")
        {
            if (SymptomFeedback.Count > 1)
            {
                SeemoreStack.IsVisible = true;
            }
            else
            {
                SeemoreStack.IsVisible = false;

            }
            AllDataLV.ItemsSource = SingleSymptomFeedback;
            seeall.Text = "See All";
            arrow.Rotation = 270;
            AllDataLV.IsVisible = true;
            AllDataResult.Text = "All Data (" + SymptomFeedback.Count.ToString() + ")";
            EpmtyTriggers.IsVisible = false;
        }
        else if (itemclicked == "Triggers")
        {
            if (TriggersFeedback.Count == 0)
            {
                EpmtyTriggers.IsVisible = true;
                EpmtyTriggers.Text = "No Triggers Added";
                AllDataLV.IsVisible = false;
                AllDataResult.Text = "Triggers (" + TriggersFeedback.Count.ToString() + ")";
            }
            else
            {
                EpmtyTriggers.IsVisible = false;
                AllDataLV.IsVisible = true;
                AllDataLV.ItemsSource = TriggersFeedback.OrderBy(f => DateTime.Parse(f.timestamp)); ;
                AllDataResult.Text = "Triggers (" + TriggersFeedback.Count.ToString() + ")";
            }
        }
        else if (itemclicked == "Interventions")
        {
            AllDataResult.FontSize = 16;
            if (InterventionsFeedback.Count == 0)
            {
                EpmtyTriggers.IsVisible = true;
                EpmtyTriggers.Text = "No Interventions Added";
                AllDataLV.IsVisible = false;
                AllDataResult.Text = "Interventions (" + InterventionsFeedback.Count.ToString() + ")";
            }
            else
            {
                EpmtyTriggers.IsVisible = false;
                AllDataLV.IsVisible = true;
                AllDataLV.ItemsSource = InterventionsFeedback.OrderBy(f => DateTime.Parse(f.timestamp)); ;
                AllDataResult.Text = "Interventions (" + InterventionsFeedback.Count.ToString() + ")";
            }

        }
    }

    private async void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ShowAllSymptomData(PassedSymptom, PassedSymptom[0].feedback , AllSymptomData), false);
        }
        catch(Exception ex)
        {

        }
    }
}