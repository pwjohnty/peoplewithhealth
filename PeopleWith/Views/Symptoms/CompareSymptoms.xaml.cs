using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Core;
using System.Collections.ObjectModel;
using System.Runtime.ExceptionServices;
namespace PeopleWith;
public partial class CompareSymptoms : ContentPage
{
    public ObservableCollection<usersymptom> AllUserSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> ChartSymptoms = new ObservableCollection<usersymptom>();
    public ObservableCollection<symptomprogression> ChartData = new ObservableCollection<symptomprogression>();
    public ObservableCollection<symptomprogression> RemoveChartData = new ObservableCollection<symptomprogression>();
    public ObservableCollection<symptomfeedback> Symptomfeedbacklist = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<ChipItem> ChipItems { get; set; }
    string CurrentChip;
    string DateRangetxt;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    public ObservableCollection<usersymptom> Newchartusersymptoms = new ObservableCollection<usersymptom>();

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

    protected override void OnDisappearing()
    {
        try
        {
            base.OnDisappearing();
            SymptomProgChart.Series.Clear();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    
    }
    public CompareSymptoms(ObservableCollection<usersymptom> AllSymptoms)
    {
        try
        {
            InitializeComponent();
            AllUserSymptoms = AllSymptoms;
            ChipItems = new ObservableCollection<ChipItem>();
            BindingContext = new ChipItems();

            foreach (var item in AllUserSymptoms)
            {
                foreach (var x in item.feedback)
                {
                    var Datetime = DateTime.Parse(x.timestamp).ToString("dd/MM/yy HH:mm");
                    x.formattedDateTime = Datetime;
                    x.DateTimeFormat = DateTime.Parse(x.timestamp);
                }
            }

            if (AllUserSymptoms.Count == 1)
            {
                //Dont Need All for one item
            }
            else
            {
                //ChipListView.Add("All")
                ChipItem add = new ChipItem
                {
                    Text = "All",
                    IsSelected = true
                };
                ChipItems.Add(add);
            }
            foreach (var item in AllUserSymptoms)
            {
                ChipItem add = new ChipItem
                {
                    Text = item.symptomtitle,
                    IsSelected = false
                };
                ChipItems.Add(add);
                //ChipListView.Add(item.symptomtitle);          
            }
            SymptomName.ItemsSource = ChipItems;
            SymptomName.BindingContext = ChipItems;
            SymptomName.SelectedItem = ChipItems[0];
            CurrentChip = ChipItems[0].Text;
            DateRange.Text = "DateRange: " + "All Time";
            ConfigureChart(isAllTime: true);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void ConfigureChart(int? numero = null, bool isAllTime = false)
    {
        try
        {
            SymptomProgChart.Series.Clear();
            var allTimestamps = new List<DateTime>();
            DateTime endDate = DateTime.Now;
            DateTime startDate;
            if (isAllTime)
            {
                startDate = DateTime.MinValue;
            }
            else
            {
                startDate = endDate.AddDays(-numero.GetValueOrDefault(0));
            }
            foreach (var item in AllUserSymptoms)
            {
                foreach (var x in item.feedback)
                {
                    DateTime timestamp;
                    if (DateTime.TryParse(x.timestamp, out timestamp))
                    {
                        if (timestamp >= startDate && timestamp <= endDate)
                        {
                            //x.formattedDateTime = timestamp.ToString("dd MMM HH:mm");
                            allTimestamps.Add(timestamp);
                        }
                    }
                }
            }
            if (allTimestamps.Any())
            {
                DateTime lastUpdate = allTimestamps.Max();
                DateTime firstUpdate = allTimestamps.Min();
                foreach (var item in AllUserSymptoms)
                {
                    if (CurrentChip == item.symptomtitle)
                    {
                        var filteredFeedback = item.feedback
                                          .Where(f => DateTime.Parse(f.timestamp) >= startDate && DateTime.Parse(f.timestamp) <= endDate)
                                          .OrderBy(f => DateTime.Parse(f.timestamp));

                        LineSeries series = new LineSeries
                        {
                            ItemsSource = filteredFeedback.ToList().OrderBy(x => x.DateTimeFormat),
                            XBindingPath = "DateTimeFormat",
                            YBindingPath = "intensity",
                            Label = item.symptomtitle,
                            EnableTooltip = true,
                            ShowTrackballLabel = true,
                            ShowMarkers = true,
                            StrokeWidth = 2
                        };
                        series.MarkerSettings = new ChartMarkerSettings
                        {
                            Type = Syncfusion.Maui.Charts.ShapeType.Circle,
                            StrokeWidth = 0,
                            Height = 8,
                            Width = 8,
                        };
                        SymptomProgChart.Series.Add(series);
                    }
                    else if (CurrentChip == "All")
                    {
                        var filteredFeedback = item.feedback
                                          .Where(f => DateTime.Parse(f.timestamp) >= startDate && DateTime.Parse(f.timestamp) <= endDate)
                                          .OrderBy(f => DateTime.Parse(f.timestamp));
                        LineSeries series = new LineSeries
                        {
                            ItemsSource = filteredFeedback.ToList().OrderBy(x => x.DateTimeFormat),
                            XBindingPath = "DateTimeFormat",
                            YBindingPath = "intensity",
                            Label = item.symptomtitle,
                            EnableTooltip = true,
                            ShowTrackballLabel = true,
                            ShowMarkers = true,
                            StrokeWidth = 2
                        };
                        series.MarkerSettings = new ChartMarkerSettings
                        {
                            Type = Syncfusion.Maui.Charts.ShapeType.Circle,
                            StrokeWidth = 0,
                            Height = 8,
                            Width = 8,
                        };
                        SymptomProgChart.Series.Add(series);
                    }
                }

            }
            else
            {
                DateRange.Text = "No data available for the selected range";
            }
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
            await Navigation.PopAsync();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void segmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Buttons.SelectionChangedEventArgs e)
    {
        try
        {
            var commandpar = e.NewValue.Text.ToString();
            if (commandpar == "7 Days")
            {
                ConfigureChart(6);
                var Date = DateTime.Now.ToString("dd MMM");
                var DateMinus = DateTime.Now.AddDays(-7).ToString("dd MMM");
                DateRange.Text = "DateRange: " + Date + " - " + DateMinus;
            }
            else if (commandpar == "14 Days")
            {
                ConfigureChart(13);
                var Date = DateTime.Now.ToString("dd MMM");
                var DateMinus = DateTime.Now.AddDays(-14).ToString("dd MMM");
                DateRange.Text = "DateRange: " + Date + " - " + DateMinus;
            }
            else if (commandpar == "21 Days")
            {
                ConfigureChart(20);
                var Date = DateTime.Now.ToString("dd MMM");
                var DateMinus = DateTime.Now.AddDays(-21).ToString("dd MMM");
                DateRange.Text = "DateRange: " + Date + " - " + DateMinus;
            }
            else if (commandpar == "All Time")
            {
                ConfigureChart(isAllTime: true);
                DateRange.Text = "DateRange: " + "All Time";
            }
        }
        catch(Exception Ex)
        {
            
        }
    }
    private void SymptomName_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var getitem = e.DataItem as ChipItem;
            CurrentChip = getitem.Text;
            if (getitem.Text == "All")
            {

                // If "All" is tapped while it's already selected and it's the only item selected — clear selection
                if (SymptomName.SelectedItems.Count == 1 && getitem.IsSelected)
                {
                    //SymptomName.SelectedItems.Remove(getitem);
                    getitem.IsSelected = false;
                    SymptomProgChart.Series.Clear();
                }
                else
                {
                    // Remove all non-"All" items
                    var nonAllItems = SymptomName.SelectedItems
                        .Where(item => item is ChipItem chip && chip.Text != "All")
                        .ToList();

                    foreach (var item in nonAllItems)
                    {
                        SymptomName.SelectedItems.Remove(item);
                        if (item is ChipItem chip) chip.IsSelected = false;
                    }

                    if (!getitem.IsSelected)
                    {
                        //SymptomName.SelectedItems.Add(getitem);
                        getitem.IsSelected = true;
                    }

                    SymptomProgChart.Series.Clear();
                    ConfigureChart(isAllTime: true);
                }

            }
            else
            {

                var allItem = SymptomName.SelectedItems
               .FirstOrDefault(item => item is ChipItem chip && chip.Text == "All") as ChipItem;

                if (allItem != null)
                {
                    SymptomName.SelectedItems.Remove(allItem);
                    allItem.IsSelected = false;
                }

                // Toggle tapped item
                if (SymptomName.SelectedItems.Contains(getitem))
                {
                    //SymptomName.SelectedItems.Remove(getitem);
                    getitem.IsSelected = false;
                    var existing = Newchartusersymptoms.FirstOrDefault(x => x.symptomtitle == getitem.Text);
                    if (existing != null)
                    {
                        Newchartusersymptoms.Remove(existing);
                    }
                }
                else
                {
                    //SymptomName.SelectedItems.Add(getitem);
                    getitem.IsSelected = true;

                    var finditem = AllUserSymptoms.FirstOrDefault(x => x.symptomtitle == getitem.Text);
                    if (finditem != null && !Newchartusersymptoms.Contains(finditem))
                    {
                        Newchartusersymptoms.Add(finditem);
                    }
                }

                SymptomProgChart.Series.Clear();

                // Check if any selected item is "All"

                // Update your symptom list based on the selected item
                //var finditem = AllUserSymptoms.FirstOrDefault(x => x.symptomtitle == getitem.Text);
                //if (finditem != null)
                //{
                //    if (Newchartusersymptoms.Contains(finditem))
                //        Newchartusersymptoms.Remove(finditem);
                //    else
                //        Newchartusersymptoms.Add(finditem);
                //}


                foreach (var item in Newchartusersymptoms)
                {
                   
                    

                        var filteredFeedback = item.feedback.OrderBy(f => DateTime.Parse(f.timestamp));
                        LineSeries series = new LineSeries
                        {
                            ItemsSource = filteredFeedback.ToList().OrderBy(x => x.DateTimeFormat),
                            XBindingPath = "DateTimeFormat",
                            YBindingPath = "intensity",
                            //Fill = Color.FromArgb("#FFC000"),
                            Label = item.symptomtitle,
                            EnableTooltip = true,
                            ShowTrackballLabel = true,
                            ShowMarkers = true,
                            StrokeWidth = 2
                        };
                        series.MarkerSettings = new ChartMarkerSettings
                        {
                            Type = Syncfusion.Maui.Charts.ShapeType.Circle,
                            StrokeWidth = 0,
                            Height = 8,
                            Width = 8
                            //Fill = Color.FromArgb("#FFC000")
                        };
                        SymptomProgChart.Series.Add(series);
                    

                }
                //var allTimestamps = new List<DateTime>();
                //foreach (var item in AllUserSymptoms)
                //{
                //    if (getitem.Text == item.symptomtitle)
                //    {
                //        foreach (var x in item.feedback)
                //        {
                //            if (DateTime.TryParse(x.timestamp, out DateTime timestamp))
                //            {
                //                allTimestamps.Add(timestamp);
                //            }
                //        }
                //    }

                //}
                //DateTime lastUpdate = allTimestamps.Max();
                //DateTime Firstupdate = allTimestamps.Min();
                //DateRange.Text = "DateRange: " + DateRangetxt;
            }
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    private void NumericalAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
    {

        if (sender is NumericalAxis axis)
        {
            if (e.Label == "0")
            {
                // Solid line for Y=0 (No dash)
                axis.MajorGridLineStyle.StrokeDashArray = null;
            }
            else
            {
                // Dashed lines for all other Y-values
                axis.MajorGridLineStyle.StrokeDashArray = new DoubleCollection { 5, 5 };
            }
        }
    }

}