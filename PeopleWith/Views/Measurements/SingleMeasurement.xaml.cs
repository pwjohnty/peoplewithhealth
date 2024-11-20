using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Azure;
using Mopups.Services;
using Syncfusion.Maui.Charts;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleMeasurement : ContentPage
{
    measurement measurementpassed;
    usermeasurement usermeasurementpassed;
    ObservableCollection<usermeasurement> usermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    ObservableCollection<usermeasurement> usermeasurementchartlist = new ObservableCollection<usermeasurement>();
    ObservableCollection<usermeasurement> orderlistbydate = new ObservableCollection<usermeasurement>();
    ObservableCollection<measurement> measurementlist = new ObservableCollection<measurement>();
    ObservableCollection<usermeasurement> deleeteusermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    //List<VerticalLinePoint> verticalLinePoints = new List<VerticalLinePoint>();
    bool newmeasurement;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();

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

    public class VerticalLinePoint
    {
        public string inputdatetime { get; set; }
        public double BPone { get; set; }
        public double BPtwo { get; set; } 
    }
    public SingleMeasurement()
	{
		InitializeComponent();
	}

    public SingleMeasurement(measurement measurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed, userfeedback userfeedbacklist)
    {
        //measurement
        try
        {
            InitializeComponent();
            measurementpassed = measurementp;
            usermeasurementlistpassed = usermeasurementsp;
            measurementlist = measurementlistpassed;
            userfeedbacklistpassed = userfeedbacklist;

            measurementname.Text = measurementpassed.measurementname;
            newmeasurement = true;

            lblvalue.Text = "No Data";

            datelbl.Text = "Today";

            charticon.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public SingleMeasurement(usermeasurement usermeasurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed, userfeedback userfeedbacklist)
    {
        //usermeasurement
        try
        {
            InitializeComponent();
            usermeasurementpassed = usermeasurementp;
            usermeasurementlistpassed = usermeasurementsp;
            measurementlist = measurementlistpassed;
            userfeedbacklistpassed = userfeedbacklist;

            measurementname.Text = usermeasurementpassed.measurementname;


            unitlbl.Text = usermeasurementpassed.unit;
            detailslbl.IsVisible = true;
            detailsframe.IsVisible = true;
            showallbtn.IsVisible = true;

            populatechart();

            WeakReferenceMessenger.Default.Register<SendItemMessage>(this, (r, m) =>
            {
                usermeasurementlistpassed = (ObservableCollection<usermeasurement>)m.Value;
                Task.Delay(100);
                populatechart();
            });
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async public Task<int> ConvertFeetInchesToInches(string input)
    {
        try
        {
            string cleanInput = input.Replace("'", "").Replace("\"", "").Trim();
            string[] parts = cleanInput.Split(' ');
            int feet = int.Parse(parts[0]);
            int inches = int.Parse(parts[1]);

            // Convert feet to inches (1 foot = 12 inches)
            int totalInches = (feet * 12) + inches;

            return totalInches;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
            return 0;
        }
    }

    //async public Task<List<string>> ConvertInchestoFeetInches(Double Min, Double Max)
    //{
    //    try
    //    {
    //        List<string> FeetInches = new List<string>(); 
    //        for(Double i = Min; i <= Max; i++)
    //        {
    //            int total = Convert.ToInt32(i); 
    //            int stones = total / 14;          
    //            int pounds = total % 14;

    //            FeetInches.Add($"{stones}st {pounds}lbs");
    //        }

    //        return FeetInches;
    //    }
    //    catch (Exception Ex)
    //    {
    //        await crashHandler.CrashDetectedSend(Ex);
    //        return null;
    //    }
    //}

    async public Task<int> ConvertStonePoundsToPounds(string input)
    {
        try
        {
            string cleanInput = input.Replace("st", "").Replace("lbs", "").Trim();
            string[] parts = cleanInput.Split(' ');

            int stone = int.Parse(parts[0]);
            int pounds = int.Parse(parts[1]);

            // Convert stone to pounds (1 stone = 14 pounds)
            int totalPounds = (stone * 14) + pounds;

            return totalPounds;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
            return 0;
        }
    }

    //async public Task<int> ConvertPoundsToStonePounds(Double Min, Double Max)
    //{
    //    try
    //    {
    //        string cleanInput = input.Replace("st", "").Replace("lbs", "").Trim();
    //        string[] parts = cleanInput.Split(' ');

    //        int stone = int.Parse(parts[0]);
    //        int pounds = int.Parse(parts[1]);

    //        // Convert stone to pounds (1 stone = 14 pounds)
    //        int totalPounds = (stone * 14) + pounds;

    //        return totalPounds;
    //    }
    //    catch (Exception Ex)
    //    {
    //        await crashHandler.CrashDetectedSend(Ex);
    //        return 0;
    //    }
    //}

    async void populatechart()
    {
        try
        {
            if (usermeasurementlistpassed.Count == 0)
            {
                Navigation.RemovePage(this);
                return;
            }

            datachart.Series.Clear();
            datachart.XAxes.Clear();
            datachart.YAxes.Clear();

            var usermeasurementchartlist = usermeasurementlistpassed.Where(x => x.measurementid == usermeasurementpassed.measurementid).ToList();

            if (usermeasurementchartlist.Count == 0)
            {
                Navigation.RemovePage(this);
                return;
            }


            foreach (var item in usermeasurementchartlist)
            {
                var date = DateTime.Parse(item.inputdatetime);
                item.dateconverted = date;

                if (usermeasurementpassed.measurementname == "Blood Pressure")
                {
                    var splitnum = item.value.Split('/');
                    item.BPone = splitnum[0];
                    var converttodouble = Convert.ToDouble(splitnum[0]);
                    item.numconverted = converttodouble;
                    item.BPtwo = splitnum[1];
                    var converttodoubletwo = Convert.ToDouble(splitnum[1]);
                    item.numconvertedtwo = converttodoubletwo;
              
                }
                else
                {
                    if(usermeasurementpassed.unit == "Feet/Inches")
                    {
                        var num = await ConvertFeetInchesToInches(item.value);
                        item.numconverted = num;
                    }
                    else if(usermeasurementpassed.unit == "Stones/Pounds")
                    {
                        var num = await ConvertStonePoundsToPounds(item.value);
                        item.numconverted = num;
                    }
                    else
                    {
                        var num = Convert.ToDouble(item.value);
                        item.numconverted = num;
                    }
                    
                }
            }
            if (usermeasurementpassed != null)
            {

                if (usermeasurementpassed.measurementname == "Blood Pressure")
                {
                    double maxvalue = (double)usermeasurementchartlist.Max(x => x.numconverted);
                    double minvalue = (double)usermeasurementchartlist.Min(x => x.numconverted);
                    double maxvaluetwo = (double)usermeasurementchartlist.Max(x => x.numconvertedtwo);
                    double minvaluetwo = (double)usermeasurementchartlist.Min(x => x.numconvertedtwo);

                    // Add a min and max date so the chart looks better
                    var mindate = (DateTime)usermeasurementchartlist.Min(x => x.dateconverted);
                    var maxdate = (DateTime)usermeasurementchartlist.Max(x => x.dateconverted);
                    var minusermeasurement = new usermeasurement();
                    var maxusermeasurement = new usermeasurement();
                    minusermeasurement.inputdatetime = mindate.AddDays(-1).ToString("dd/MM/yyyy HH:mm");
                    usermeasurementchartlist.Add(minusermeasurement);
                    maxusermeasurement.inputdatetime = maxdate.AddDays(+1).ToString("dd/MM/yyyy HH:mm");
                    usermeasurementchartlist.Add(maxusermeasurement);

                    // Sort the list by date
                    orderlistbydate = new ObservableCollection<usermeasurement>(usermeasurementchartlist.OrderBy(x => DateTime.Parse(x.inputdatetime)).ToList());
                    datachart.Series.Clear();

                    ChartZoomPanBehavior zooming = new ChartZoomPanBehavior()
                    {
                        EnablePinchZooming = true
                    };
                    datachart.ZoomPanBehavior = zooming;

                    // Configure primary and secondary axes
                    CategoryAxis primaryAxis = new CategoryAxis();
                    primaryAxis.LabelStyle.TextColor = Colors.Transparent;
                    primaryAxis.ShowMajorGridLines = false;
                    primaryAxis.AxisLineStyle.Stroke = Colors.Transparent;
                    primaryAxis.MajorTickStyle.Stroke = Colors.Transparent;
                    datachart.XAxes.Add(primaryAxis);

                    NumericalAxis secondaryAxis = new NumericalAxis();
                    secondaryAxis.LabelStyle.TextColor = Colors.LightGray;
                    secondaryAxis.LabelStyle.FontFamily = "HankenGroteskRegular";
                    secondaryAxis.AxisLineStyle.Stroke = Colors.LightGray;
                    secondaryAxis.MajorTickStyle.Stroke = Colors.LightGray;
                    secondaryAxis.AxisLineStyle.StrokeWidth = 1;
                    secondaryAxis.MajorTickStyle.StrokeWidth = 1;
                    secondaryAxis.IsVisible = true;
                    secondaryAxis.Minimum = minvaluetwo - 5;
                    secondaryAxis.Maximum = maxvalue + 5;
                    datachart.YAxes.Add(secondaryAxis);

 
                    ScatterSeries bpOneSeries = new ScatterSeries
                    {
                        ItemsSource = orderlistbydate,
                        XBindingPath = "inputdatetime",
                        YBindingPath = "BPone",
                        PointHeight = 8,    
                        PointWidth = 8,     
                        Fill = Color.FromRgba("#031926"),  
                        Stroke = Color.FromRgba("#031926"), 
                        StrokeWidth = 2,
                        EnableTooltip = true,
                        EnableAnimation = true
                    };

                    ScatterSeries bpTwoSeries = new ScatterSeries
                    {
                        ItemsSource = orderlistbydate,
                        XBindingPath = "inputdatetime",
                        YBindingPath = "BPtwo",
                        PointHeight = 8,
                        PointWidth = 8,
                        Fill = Colors.ForestGreen,  
                        Stroke = Colors.ForestGreen, 
                        StrokeWidth = 2,
                        EnableTooltip = true,
                        EnableAnimation = true
                    };


                    List<VerticalLinePoint> rangeColumnPoints = new List<VerticalLinePoint>();

                    var firstDate = orderlistbydate.First().inputdatetime;
                    var lastDate = orderlistbydate.Last().inputdatetime;

                    // Add an invisible point before the first valid data point, set BPone and BPtwo to 0 if null
                    rangeColumnPoints.Add(new VerticalLinePoint
                    {
                        inputdatetime = firstDate, 
                        BPone = orderlistbydate.First().BPone != null ? Double.Parse(orderlistbydate.First().BPone) : 0,  
                        BPtwo = orderlistbydate.First().BPtwo != null ? Double.Parse(orderlistbydate.First().BPtwo) : 0  
                    });

                    foreach (var item in orderlistbydate.Where(x => x.BPone != null && x.BPtwo != null))
                    {
                        rangeColumnPoints.Add(new VerticalLinePoint
                        {
                            inputdatetime = item.inputdatetime,
                            BPone = Double.Parse(item.BPone),  // Minimum Y-value
                            BPtwo = Double.Parse(item.BPtwo)   // Maximum Y-value
                        });
                    }

                    // Add an invisible point after the last valid data point
                    rangeColumnPoints.Add(new VerticalLinePoint
                    {
                        inputdatetime = lastDate,  // Padding after the last data point
                        BPone = orderlistbydate.Last().BPone != null ? Double.Parse(orderlistbydate.First().BPone) : 0,
                        BPtwo = orderlistbydate.Last().BPtwo != null ? Double.Parse(orderlistbydate.First().BPtwo) : 0
                    });


                    RangeColumnSeries rangeColumnSeries = new RangeColumnSeries
                    {
                        ItemsSource = rangeColumnPoints,
                        XBindingPath = "inputdatetime",
                        High = "BPtwo",  
                        Low = "BPone",   
                        Fill = Colors.Gray, 
                        Width = 0.02, 
                        EnableAnimation = true
                    };

                    // Add the range column series to the chart
                    datachart.Series.Add(rangeColumnSeries);
                    datachart.Series.Add(bpOneSeries);
                    datachart.Series.Add(bpTwoSeries);

                }
                else
                {



                    double maxvalue = (double)usermeasurementchartlist.Max(x => x.numconverted);
                    double minvalue = (double)usermeasurementchartlist.Min(x => x.numconverted);
                    var mindate = (DateTime)usermeasurementchartlist.Min(x => x.dateconverted);
                    var maxdate = (DateTime)usermeasurementchartlist.Max(x => x.dateconverted);

                    //add a min and max date so the chart looks better
                    var minusermeasurement = new usermeasurement();
                    var maxusermeasurement = new usermeasurement();

                    minusermeasurement.inputdatetime = mindate.AddDays(-1).ToString("dd/MM/yyyy HH:mm");
                    minusermeasurement.numconverted = null; 
                    usermeasurementchartlist.Add(minusermeasurement);

                    maxusermeasurement.inputdatetime = maxdate.AddDays(+1).ToString("dd/MM/yyyy HH:mm");
                    maxusermeasurement.numconverted = null;
                    usermeasurementchartlist.Add(maxusermeasurement);

                    orderlistbydate = new ObservableCollection<usermeasurement>(usermeasurementchartlist.OrderBy(x => DateTime.Parse(x.inputdatetime)).ToList());
                    datachart.Series.Clear();


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

                    datachart.ZoomPanBehavior = zooming;

                    CategoryAxis primaryAxis = new CategoryAxis();
                    primaryAxis.LabelStyle.TextColor = Colors.Transparent;
                    primaryAxis.ShowMajorGridLines = false;
                    primaryAxis.AxisLineStyle.Stroke = Colors.Transparent;
                    primaryAxis.MajorTickStyle.Stroke = Colors.Transparent;
                    datachart.XAxes.Add(primaryAxis);

                  
                    if (usermeasurementpassed.unit == "Feet/Inches")
                    {
                        NumericalAxis secondaryAxis = new NumericalAxis();
                        secondaryAxis.LabelStyle.TextColor = Colors.LightGray;
                        secondaryAxis.LabelStyle.FontFamily = "HankenGroteskRegular";
                        secondaryAxis.LabelStyle.LabelFormat = "0' In";
                        secondaryAxis.LabelStyle.FontSize = 8;
                        secondaryAxis.AxisLineStyle.Stroke = Colors.LightGray;
                        secondaryAxis.MajorTickStyle.Stroke = Colors.LightGray;
                        secondaryAxis.AxisLineStyle.StrokeWidth = 1;//Hide Axis line 
                        secondaryAxis.MajorTickStyle.StrokeWidth = 1;//Hide TickLines 
                        secondaryAxis.IsVisible = true;
                        secondaryAxis.Minimum = minvalue - 5;
                        secondaryAxis.Maximum = maxvalue + 5;
                        datachart.YAxes.Add(secondaryAxis);
                    }
                    else if (usermeasurementpassed.unit == "Stones/Pounds")
                    {
                        NumericalAxis secondaryAxis = new NumericalAxis();
                        secondaryAxis.LabelStyle.TextColor = Colors.LightGray;
                        secondaryAxis.LabelStyle.FontFamily = "HankenGroteskRegular";
                        secondaryAxis.LabelStyle.LabelFormat = "0' lbs";
                        secondaryAxis.LabelStyle.FontSize = 8;
                        secondaryAxis.AxisLineStyle.Stroke = Colors.LightGray;
                        secondaryAxis.MajorTickStyle.Stroke = Colors.LightGray;
                        secondaryAxis.AxisLineStyle.StrokeWidth = 1;//Hide Axis line 
                        secondaryAxis.MajorTickStyle.StrokeWidth = 1;//Hide TickLines 
                        secondaryAxis.IsVisible = true;
                        secondaryAxis.Minimum = minvalue - 5;
                        secondaryAxis.Maximum = maxvalue + 5;
                        datachart.YAxes.Add(secondaryAxis);
                    }
                    else
                    {
                        NumericalAxis secondaryAxis = new NumericalAxis();
                        secondaryAxis.LabelStyle.TextColor = Colors.LightGray;
                        secondaryAxis.LabelStyle.FontFamily = "HankenGroteskRegular";
                        secondaryAxis.AxisLineStyle.Stroke = Colors.LightGray;
                        secondaryAxis.MajorTickStyle.Stroke = Colors.LightGray;
                        secondaryAxis.AxisLineStyle.StrokeWidth = 1;//Hide Axis line 
                        secondaryAxis.MajorTickStyle.StrokeWidth = 1;//Hide TickLines 
                        secondaryAxis.IsVisible = true;
                        secondaryAxis.Minimum = minvalue - 5;
                        secondaryAxis.Maximum = maxvalue + 5;
                        datachart.YAxes.Add(secondaryAxis);
                    }


                    DataPointSelectionBehavior selection = new DataPointSelectionBehavior();
                    //   selection.SelectionBrush = Colors.Red;
                    selection.SelectionChanged += OnSelectionChanged;

                    if (usermeasurementpassed.unit == "Feet/Inches")
                    {
                        LineSeries columnseries = new LineSeries
                        {
                            ItemsSource = orderlistbydate,
                            XBindingPath = "inputdatetime",
                            YBindingPath = "numconverted",
                            Fill = Color.FromHex("#BFDBF7"),
                            StrokeWidth = 2,
                            EnableTooltip = true,
                            EnableAnimation = true,
                            ShowMarkers = true,
                            ShowTrackballLabel = false,
                            TooltipTemplate = datachart.Resources["tooltipTemplate"] as DataTemplate,
                            SelectionBehavior = selection,
                            MarkerSettings = chartMarker
                        };

                        columnseries.ShowDataLabels = false;
                        datachart.Series.Add(columnseries);
                    }
                    else if (usermeasurementpassed.unit == "Stones/Pounds")
                    {
                        LineSeries columnseries = new LineSeries
                        {
                            ItemsSource = orderlistbydate,
                            XBindingPath = "inputdatetime",
                            YBindingPath = "numconverted",
                            Fill = Color.FromHex("#BFDBF7"),
                            StrokeWidth = 2,
                            EnableTooltip = true,
                            EnableAnimation = true,
                            ShowMarkers = true,
                            ShowTrackballLabel = false,
                            TooltipTemplate = datachart.Resources["tooltipTemplate"] as DataTemplate,
                            SelectionBehavior = selection,
                            MarkerSettings = chartMarker
                        };
                        columnseries.ShowDataLabels = false;
                        datachart.Series.Add(columnseries);
                    }
                    else
                    {
                        LineSeries columnseries = new LineSeries
                        {
                            ItemsSource = orderlistbydate,
                            XBindingPath = "inputdatetime",
                            YBindingPath = "numconverted",
                            Fill = Color.FromHex("#BFDBF7"),
                            StrokeWidth = 2,
                            EnableTooltip = true,
                            EnableAnimation = true,
                            ShowMarkers = true,
                            ShowTrackballLabel = false,
                            SelectionBehavior = selection,
                            MarkerSettings = chartMarker
                        };
                        columnseries.ShowDataLabels = false;
                        datachart.Series.Add(columnseries);
                    }
                }

                lblvalue.Text = orderlistbydate[orderlistbydate.Count - 2].value;
                lblunit.Text = orderlistbydate[orderlistbydate.Count - 2].unit;

                var convertdate = DateTime.Parse(orderlistbydate[orderlistbydate.Count - 2].inputdatetime);

                datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");
            }

        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    async private void OnSelectionChanged(object sender, ChartSelectionChangedEventArgs e)
    {
        try
        {

            var es = e.NewIndexes[0];

            lblvalue.Text = orderlistbydate[es].value;

            var convertdate = DateTime.Parse(orderlistbydate[es].inputdatetime);

            datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                AddBtn.IsEnabled = false;
                if (newmeasurement)
                {
                    await Navigation.PushAsync(new AddMeasurement(measurementpassed, usermeasurementlistpassed, measurementlist, userfeedbacklistpassed), false);
                }
                else
                {
                    await Navigation.PushAsync(new AddMeasurement(usermeasurementpassed, usermeasurementlistpassed, measurementlist, userfeedbacklistpassed), false);
                }
                AddBtn.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch(Exception Ex )
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                showallbtn.IsEnabled = false;
                //show all button clicked
                var usermeasurementalldatalist = new ObservableCollection<usermeasurement>(usermeasurementlistpassed.Where(x => x.measurementid == usermeasurementpassed.measurementid));
                await Navigation.PushAsync(new ShowAllData(usermeasurementalldatalist, usermeasurementlistpassed, measurementlist), false);
                showallbtn.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }


        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    async private void Deltebtn_Clicked(object sender, EventArgs e)
    {
        //Delete Symptom 
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                Deltebtn.IsEnabled = false;
                bool Delete = await DisplayAlert("Delete Measurement", "Are you sure you would like the delete this Measurement? Once deleted it cannot be retrieved", "Delete", "Cancel");
                if (Delete == true)
                {
                    foreach (var item in usermeasurementlistpassed)
                    {
                        if (usermeasurementpassed.measurementid == item.measurementid)
                        {
                            item.deleted = true;
                            deleeteusermeasurementlistpassed.Add(item);
                        }
                    }

                    foreach (var item in deleeteusermeasurementlistpassed)
                    {
                        usermeasurementlistpassed.Remove(item);
                    }
                    APICalls database = new APICalls();

                    //    var filteredMeasurements = usermeasurementlistpassed
                    //.GroupBy(m => m.measurementid)
                    //.Select(g => g.OrderByDescending(m => DateTime.Parse(m.inputdatetime))
                    //.First())
                    //.OrderByDescending(x => DateTime.Parse(x.inputdatetime))
                    //.ToList();

                    // Convert the filtered and sorted list to an ObservableCollection
                    ObservableCollection<usermeasurement> observableFilteredMeasurements = new ObservableCollection<usermeasurement>(usermeasurementlistpassed);
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Deleted") { });

                    //update main page
                    //WeakReferenceMessenger.Default.Send(new UpdateListMainPage(observableFilteredMeasurements));

                    await database.DeleteUserMeasurements(deleeteusermeasurementlistpassed);

                    Deltebtn.IsEnabled = true;

                    await Navigation.PushAsync(new MeasurementsPage());
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is MeasurementsPage);
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                    await Task.Delay(1500);
                    await MopupService.Instance.PopAllAsync(false);
                }
                else
                {
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
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Add Symptom Info Here
            await DisplayAlert("Measurement Information", "No Information is saved against this Measurement", "Close");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}