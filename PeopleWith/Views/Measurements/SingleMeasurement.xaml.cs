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
    measurement MeasureInfo = new measurement();
    ObservableCollection<usermeasurement> GroupedSleepData = new ObservableCollection<usermeasurement>();

    //List<VerticalLinePoint> verticalLinePoints = new List<VerticalLinePoint>();
    bool newmeasurement;
    //string WebpageLink;
    //public string WeborPdf;

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
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    //public class GroupedSleepDataItem
    //{
    //    //Stacked
    //    //public DateTime Date { get; set; }
    //    // public string FormattedDate => Date.ToString("dd/MM/yyyy");
    //    // public List<usermeasurement> Measurements { get; set; }

    //    //Column Chart 
    //    public DateTime Date { get; set; }
    //    public string FormattedDate => Date.ToString("dd/MM/yyyy");
    //    public double TotalValue { get; set; }
    //    public string unit { get; set; }
    //    public string value { get; set; }
    //}

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

            Measurementinfo();
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

            Measurementinfo(); 

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

    async void Measurementinfo()
    {
        try
        {
            //var StringId = usermeasurementpassed.measurementid;
            MeasureInfo.measurementid = usermeasurementpassed.measurementid;
            APICalls Database = new APICalls();
            var GetmeasureInfo  = await Database.GetMeasurementsInfo(MeasureInfo);
            MeasureInfo = GetmeasureInfo; 
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

    async public Task<double> ConvertStonePoundsToPounds(string input)
    {
        try
        {
            string cleanInput = input.Replace("st", "").Replace("lbs", "").Trim();
            string[] parts = cleanInput.Split(' ');

            int stone = Int32.Parse(parts[0]);
            int pounds = Int32.Parse(parts[1]);

            //var stone = parts[0];
            //var pounds = parts[1];

            // Convert stone to pounds (1 stone = 14 pounds)
            double totalPounds = Convert.ToDouble((stone * 14) + pounds);
            //double newvalue = Convert.ToDouble(stone + "." + pounds);

            //return newvalue;
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
                    if (usermeasurementpassed.unit == "Feet/Inches")
                    {
                        if (item.value.Contains("'"))
                        {
                            var num = await ConvertFeetInchesToInches(item.value);
                            item.numconverted = num;
                        }
                        else
                        {
                            //Check This
                            item.numconverted = Convert.ToDouble(item.value);
                        }
                    }
                    else if (usermeasurementpassed.unit == "Stones/Pounds")
                    {
                        if (item.value.Contains("st"))
                        {
                            var num = await ConvertStonePoundsToPounds(item.value);
                            item.numconverted = num;
                        }
                        else
                        {
                            //Check This
                            item.numconverted = Convert.ToDouble(item.value);
                        }

                    }
                    else if (usermeasurementpassed.unit == "Hours/Minutes")
                    {
                        string clean = item.value.Replace("h", "").Replace("m", "").Trim();
                        var Splitvalue = clean.Split(' ');
                        var getnum = Splitvalue[0] + "." + Splitvalue[1];
                        var num = Convert.ToDouble(getnum);
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
                // Attempt at Stacking Chart Data //
                // StackingColumnSeries //
                //else if (usermeasurementpassed.measurementname == "Sleep Duration")
                //{
                //     Group data by date
                //     GroupedSleepData = usermeasurementchartlist
                //        .GroupBy(x => DateTime.Parse(x.inputdatetime).Date)
                //        .Select(g => new GroupedSleepDataItem
                //        {
                //            Date = g.Key,
                //            Measurements = g.ToList()
                //        }).OrderBy(g => g.Date)
                //        .ToList();

                //     Clear existing chart configuration
                //    datachart.Series.Clear();
                //    datachart.YAxes.Clear();

                //     Configure Y-Axis based on total sleep durations
                //    double maxValue = (double)GroupedSleepData.Max(group => group.Measurements.Sum(x => x.numconverted));

                //    double minValue = (double)GroupedSleepData.Min(group => group.Measurements.Sum(x => x.numconverted));

                //    datachart.YAxes.Add(new NumericalAxis
                //    {
                //        Minimum = Math.Floor(minValue) - 1,
                //        Maximum = Math.Ceiling(maxValue) + 1,
                //        LabelStyle = new ChartAxisLabelStyle { TextColor = Colors.LightGray, FontSize = 10 }
                //    });

                //     Add series for each group
                //    foreach (var group in GroupedSleepData)
                //    {
                //         Initial sleep session in LightBlue
                //        var firstMeasurement = group.Measurements.FirstOrDefault();
                //        if (firstMeasurement != null)
                //        {
                //            var firstSeries = new StackingColumnSeries
                //            {
                //                ItemsSource = new List<usermeasurement> { firstMeasurement },
                //                XBindingPath = "FormattedDate",
                //                YBindingPath = "numconverted",
                //                Fill = Colors.LightBlue,
                //                EnableTooltip = true,
                //                DataLabelSettings = new CartesianDataLabelSettings
                //                {
                //                    LabelPlacement = DataLabelPlacement.Outer
                //                }
                //            };
                //            datachart.Series.Add(firstSeries);
                //        }

                //         Additional sessions stacked with different colors
                //        var additionalMeasurements = group.Measurements.Skip(1).ToList();
                //        foreach (var measurement in additionalMeasurements)
                //        {
                //            var stackedSeries = new StackingColumnSeries
                //            {
                //                ItemsSource = new List<usermeasurement> { measurement },
                //                XBindingPath = "FormattedDate",
                //                YBindingPath = "numconverted",
                //                Fill = Colors.LightGreen,
                //                EnableTooltip = true,
                //                DataLabelSettings = new CartesianDataLabelSettings
                //                {
                //                    LabelPlacement = DataLabelPlacement.Outer
                //                }
                //            };
                //            datachart.Series.Add(stackedSeries);
                //        }
                //    }

                //     Add zoom/pan behavior
                //    datachart.ZoomPanBehavior = new ChartZoomPanBehavior
                //    {
                //        EnablePanning = true,
                //        EnablePinchZooming = false
                //    };
                //}

                else if (usermeasurementpassed.measurementname == "Sleep Duration")
                {
                    // Group the data by date (ignoring time) and sum the values
                    var groupedData = usermeasurementchartlist
      .GroupBy(
          x => DateTime.Parse(x.inputdatetime).Date, // Group by date only
          (key, group) => new
          {
              dateconverted = key,
              numconverted = group.Sum(g => ConvertToMinutes(g.numconverted.GetValueOrDefault())),
              unit = "Hours/Minutes",
              value = ConvertMinutesToHoursAndMinutes(group.Sum(g => ConvertToMinutes(g.numconverted.GetValueOrDefault())))
          })
      .OrderByDescending(g => g.dateconverted).Take(7)
      .ToList();

                    // Map grouped data into ObservableCollection<usermeasurement>
                    GroupedSleepData = new ObservableCollection<usermeasurement>(
                        groupedData.Select(g => new usermeasurement
                        {
                            dateconverted = g.dateconverted,
                            numconverted = g.numconverted,
                            unit = g.unit,
                            value = g.value + "\n" + g.dateconverted.ToString("dd/MM/yy")
                        }
                    ).OrderBy(d=> d.dateconverted));

                    // Calculate min/max values based on the grouped data
                    double maxvalue = (double)GroupedSleepData.Max(g => g.numconverted) + 60;
                    double minvalue = (double)GroupedSleepData.Min(g => g.numconverted) - 60;
                    if(minvalue < 1)
                    {
                        minvalue = 0;
                    }

                    // Sort the list by date
                    orderlistbydate = new ObservableCollection<usermeasurement>(usermeasurementchartlist.OrderBy(x => DateTime.Parse(x.inputdatetime)).ToList());

                    // Clear any existing data
                    datachart.Series.Clear();
                    datachart.XAxes.Clear();
                    datachart.YAxes.Clear();

                    CategoryAxis primaryAxis = new CategoryAxis();
                    primaryAxis.LabelStyle.TextColor = Colors.Transparent;
                    primaryAxis.ShowMajorGridLines = false;
                    primaryAxis.AxisLineStyle.Stroke = Colors.Transparent;
                    primaryAxis.MajorTickStyle.Stroke = Colors.Transparent;
                    datachart.XAxes.Add(primaryAxis);

                    // Configure X-Axis
                    //CategoryAxis xAxis = new CategoryAxis
                    //{
                    //    LabelStyle = new ChartAxisLabelStyle
                    //    {
                    //        TextColor = Colors.Transparent,
                    //        FontSize = 10
                    //    },
                    //    AxisLineStyle = new ChartLineStyle
                    //    {
                    //        Stroke = Colors.Transparent,
                    //        StrokeWidth = 0
                    //    },
                    //    ShowMajorGridLines = false,
                    //    EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Shift
                    //};
                    //datachart.XAxes.Add(xAxis);

                    // Configure Y-Axis
                    NumericalAxis yAxis = new NumericalAxis
                    {
                        LabelStyle = new ChartAxisLabelStyle
                        {
                            TextColor = Colors.LightGray,
                            FontSize = 10
                        },
                        Minimum = minvalue,
                        Maximum = maxvalue, 
                    };
                    yAxis.LabelCreated += OnYAxisLabelCreated;
                    datachart.YAxes.Add(yAxis);

                    // Data point selection behavior
                    DataPointSelectionBehavior selection = new DataPointSelectionBehavior
                    {
                        //SelectionBrush = Colors.Red
                    };
                    selection.SelectionChanged += OnSelectionChanged;

                    if(GroupedSleepData.Count < 7)
                    {
                        //DateTime max = (DateTime)GroupedSleepData.Max(g => g.dateconverted);
                        DateTime min = (DateTime)GroupedSleepData.Min(g => g.dateconverted);
                        int GetCount = 7 - GroupedSleepData.Count;

                        for (int i = GetCount; i > 0; i--)
                        {
                            DateTime AddDays = min.AddDays(-i);
                            GroupedSleepData.Add(new usermeasurement
                            {
                                dateconverted = AddDays,
                                //numconverted = existingEntry.numconverted,
                                //unit = existingEntry.unit,
                                //value = existingEntry.value + "\n" + existingEntry.dateconverted.ToString("dd/MM/yy")
                            });
                        }

                        GroupedSleepData = new ObservableCollection<usermeasurement>(GroupedSleepData.OrderBy(b => b.dateconverted));


                    }

                    // Configure the Column Series
                    ColumnSeries columnSeries = new ColumnSeries
                    {
                        ItemsSource = GroupedSleepData.OrderBy(d => d.dateconverted),
                        XBindingPath = "dateconverted", 
                        YBindingPath = "numconverted", 
                        Width = 0.4,
                        CornerRadius = new CornerRadius(5, 5, 5, 5),
                        EnableTooltip = true,
                        Fill = Colors.LightBlue,
                        Stroke = Colors.Transparent,
                        StrokeWidth = 0,
                        ShowDataLabels = false,
                        EnableAnimation = true,
                        TooltipTemplate = datachart.Resources["tooltipTemplate"] as DataTemplate,
                        SelectionBehavior = selection,
                        DataLabelSettings = new CartesianDataLabelSettings
                        {
                            LabelPlacement = DataLabelPlacement.Outer,
                        }
                    };
                    datachart.Series.Add(columnSeries);

                    // Add horizontal scrolling behavior
                    ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior
                    {
                        EnablePanning = true, // Enable horizontal scrolling
                        EnablePinchZooming = false // Disable pinch-to-zoom
                    };
                    datachart.ZoomPanBehavior = zoomPanBehavior;
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

                    if (usermeasurementpassed.unit == "Feet/Inches" || usermeasurementpassed.unit == "Stones/Pounds")
                    {
                       
                        ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior
                        {
                            EnablePanning = true, // Enable horizontal scrolling
                            EnablePinchZooming = false // Disable pinch-to-zoom
                        };
                        datachart.ZoomPanBehavior = zoomPanBehavior;
                    }
                    else
                    {
                        ChartZoomPanBehavior zooming = new ChartZoomPanBehavior()
                        {
                            EnablePinchZooming = true
                        };

                        datachart.ZoomPanBehavior = zooming;
                    }

                    
                    CategoryAxis primaryAxis = new CategoryAxis();
                    primaryAxis.LabelStyle.TextColor = Colors.Transparent;
                    primaryAxis.ShowMajorGridLines = false;
                    primaryAxis.AxisLineStyle.Stroke = Colors.Transparent;
                    primaryAxis.MajorTickStyle.Stroke = Colors.Transparent;
                    datachart.XAxes.Add(primaryAxis);


                    if (usermeasurementpassed.unit == "Feet/Inches")
                    {



                        NumericalAxis secondaryAxis = new NumericalAxis
                        {
                            LabelStyle = new ChartAxisLabelStyle
                            {
                                TextColor = Colors.LightGray,
                                FontFamily = "HankenGroteskRegular",
                                FontSize = 8
                            },
                            AxisLineStyle = new ChartLineStyle
                            {
                                Stroke = Colors.LightGray,
                                StrokeWidth = 1 // Axis line width
                            },
                            MajorTickStyle = new ChartAxisTickStyle
                            {
                                Stroke = Colors.LightGray,
                                StrokeWidth = 1 // Tick line width
                            },
                            IsVisible = true,
                            Minimum = minvalue - 5,
                            Maximum = maxvalue + 5
                        };

                        // Customizing the Y-axis labels
                        secondaryAxis.LabelCreated += (sender, args) =>
                        {
                            if (args.Label is string labelText && double.TryParse(labelText, out double value))
                            {
                                int inches = (int)value;
                                int feet = inches / 12;
                                int remainingInches = inches % 12;
                                args.Label = $"{feet}' {remainingInches}\"";
                            }

                        };

                        datachart.YAxes.Add(secondaryAxis);

                    }
                    else if (usermeasurementpassed.unit == "Stones/Pounds")
                    {

                        NumericalAxis secondaryAxis = new NumericalAxis
                        {
                            LabelStyle = new ChartAxisLabelStyle
                            {
                                TextColor = Colors.LightGray,
                                FontFamily = "HankenGroteskRegular",
                                FontSize = 8
                            },
                            AxisLineStyle = new ChartLineStyle
                            {
                                Stroke = Colors.LightGray,
                                StrokeWidth = 1 // Axis line width
                            },
                            MajorTickStyle = new ChartAxisTickStyle
                            {
                                Stroke = Colors.LightGray,
                                StrokeWidth = 1 // Tick line width
                            },
                            IsVisible = true,
                            Minimum = minvalue - 5,
                            Maximum = maxvalue + 5
                        };

                        // Customizing the Y-axis labels
                        secondaryAxis.LabelCreated += (sender, args) =>
                        {
                            if (args.Label is string labelText && double.TryParse(labelText, out double value))
                            {
                                int lbs = (int)value;
                                int stones = lbs / 14;
                                int pounds = lbs % 14;
                                args.Label = $"{stones}st {pounds}lbs";
                            }
                        };

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

                    //else if (usermeasurementpassed.unit == "Hours/Minutes")
                    //{
                    //    LineSeries columnseries = new LineSeries
                    //    {
                    //        ItemsSource = orderlistbydate,
                    //        XBindingPath = "inputdatetime",
                    //        YBindingPath = "numconverted",
                    //        Fill = Color.FromHex("#BFDBF7"),
                    //        StrokeWidth = 2,
                    //        EnableTooltip = true,
                    //        EnableAnimation = true,
                    //        ShowMarkers = true,
                    //        ShowTrackballLabel = false,
                    //        TooltipTemplate = datachart.Resources["tooltipTemplate"] as DataTemplate,
                    //        SelectionBehavior = selection,
                    //        MarkerSettings = chartMarker
                    //    };
                    //    columnseries.ShowDataLabels = false;

                    //    datachart.Series.Add(columnseries);
                    //}
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
                if(usermeasurementpassed.unit == "Hours/Minutes")
                {

                    int count = GroupedSleepData.OrderBy(d => d.dateconverted).Count() - 1;
                    var getvalue = GroupedSleepData[count].value.Split('\n');
                    lblvalue.Text = getvalue[0];
                    lblunit.Text = GroupedSleepData[count].unit;

                    var convertdate = GroupedSleepData[count].dateconverted;

                    datelbl.Text = convertdate.ToString("dd MMMM yyyy");
                }
                else
                {
                    lblvalue.Text = orderlistbydate[orderlistbydate.Count - 2].value;
                    lblunit.Text = orderlistbydate[orderlistbydate.Count - 2].unit;

                    var convertdate = DateTime.Parse(orderlistbydate[orderlistbydate.Count - 2].inputdatetime);

                    datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");
                }


            }

        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    private double ConvertToMinutes(double numconverted)
    {
        int hours = (int)numconverted;
        var Checknum = numconverted.ToString();
        if (Checknum.Contains("."))
        {
            var Splitmins = numconverted.ToString().Split('.');
            if (Splitmins[1].Length == 1)
            {
                Splitmins[1] = Splitmins[1] + "0";
            }
            int minutes = int.Parse(Splitmins[1]);
            return (hours * 60) + minutes;
        }
        else
        {
            return (hours * 60);
        }
     
    }

    private string ConvertMinutesToHoursAndMinutes(double totalMinutes)
    {
        int hours = (int)(totalMinutes / 60);
        int minutes = (int)(totalMinutes % 60);
        return $"{hours}h {minutes}m";
    }


    private void OnYAxisLabelCreated(object sender, ChartAxisLabelEventArgs args)
    {
        if (args.Label is string labelText && double.TryParse(labelText, out double totalMinutes))
        {
            try
            {
                int hours = (int)(totalMinutes / 60); 
                int minutes = (int)(totalMinutes % 60); 
                args.Label = $"{hours}h {minutes}m"; 
            }
            catch
            {
                args.Label = labelText; 
            }
        }
    }

    //private void OnYAxisLabelCreated(object sender, ChartAxisLabelEventArgs args)
    //{
    //    if (args.Label is string labelText && double.TryParse(labelText, out double value))
    //    {
    //        try
    //        {
    //            int hours = (int)value;
    //            int minutes = (int)((value - hours) * 60);
    //            args.Label = $"{hours}h {minutes}m";
    //        }
    //        catch
    //        {

    //        }
    //    }
    //}

    async private void OnSelectionChanged(object sender, ChartSelectionChangedEventArgs e)
    {
        try
        {

            var es = e.NewIndexes[0];

            if(usermeasurementpassed.measurementname == "Sleep Duration") 
            { 
            
                    var GetGrouped = GroupedSleepData[es].numconverted.ToString();

                    if (GetGrouped.Contains("h"))
                    {
                        lblvalue.Text = GetGrouped;
                    }
                    else
                    {
                    //var GetSleep = GetGrouped.Split('.');
                    //var Newlbl = GetSleep[0] + "h" + " " + GetSleep[1] + "m";
                    //lblvalue.Text = Newlbl;
                    var Groupie = GroupedSleepData[es].numconverted; 

                    // Convert mins to hours and minutes
                    int hours = (int)(Groupie / 60);
                    int minutes = (int)(Groupie % 60);

                    string formattedTime = $"{hours}h {minutes}m";
                    lblvalue.Text = formattedTime;
                }

                var convertdate = GroupedSleepData[es].dateconverted;
                datelbl.Text = convertdate.ToString("dd MMMM yyyy");
            }
            else
            {
                if (orderlistbydate[es].unit == "Stones/Pounds")
                {
                    if (orderlistbydate[es].value.Contains("st"))
                    {
                        lblvalue.Text = orderlistbydate[es].value;
                    }
                    else
                    {
                        var GetStones = orderlistbydate[es].value.Split('.');
                        var Newlbl = GetStones[0] + "st" + " " + GetStones[1] + "lbs";
                        lblvalue.Text = Newlbl;
                    }
                }
                else
                {
                    lblvalue.Text = orderlistbydate[es].value;
                }

                var convertdate = DateTime.Parse(orderlistbydate[es].inputdatetime);
                datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");

            }
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
                await Navigation.PushAsync(new ShowAllData(usermeasurementalldatalist, usermeasurementlistpassed, measurementlist, userfeedbacklistpassed), false);
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

                    //Remove All items From UserMeasurement Feedback 

                    if (userfeedbacklistpassed.measurementfeedbacklist == null)
                    {
                        userfeedbacklistpassed.measurementfeedbacklist = new ObservableCollection<feedbackdata>();
                    }

                    foreach (var x in userfeedbacklistpassed.measurementfeedbacklist)
                    {
                        if (x.id != null)
                        {
                            for (int i = 0; i < deleeteusermeasurementlistpassed.Count; i++)
                            {
                                if (x.id == deleeteusermeasurementlistpassed[i].id)
                                {
                                    x.action = "deleted";
                                }
                            }
                        }
                    }

                    string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.measurementfeedbacklist);
                    userfeedbacklistpassed.measurementfeedback = newsymJson;

                    await database.UserfeedbackUpdateMeasurementData(userfeedbacklistpassed);

                    Deltebtn.IsEnabled = true;

                    //userfeedbacklistpassed Comes from Dashboard so needs to be linked back to Measurements page
                    await Navigation.PushAsync(new MeasurementsPage(usermeasurementlistpassed, measurementlist, userfeedbacklistpassed));
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
            if(MeasureInfo.measurementinformation != null)
            {
                var Title = usermeasurementpassed.measurementname; 
                await Navigation.PushAsync(new MeasurementsInfo(MeasureInfo, Title), false); 
            }
            else
            {
                await DisplayAlert("Measurement Information", "No information or resources available for this Measurement", "Close");

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    //{
    //    try
    //    {

    //        if (WeborPdf == "Web")
    //        {
    //            mainstack.IsVisible = false;
    //            WebViewerStack.IsVisible = true;
    //            NavigationPage.SetHasNavigationBar(this, false);
    //            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
    //            {
    //                AndroidBtn.IsVisible = true;
    //            }
    //            else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
    //            {
    //                IOSBtn.IsVisible = true;
    //            }
    //        }
    //        else
    //        {
    //            var pdflink = WebpageLink;
    //            await Browser.OpenAsync(pdflink, new BrowserLaunchOptions
    //            {
    //                LaunchMode = BrowserLaunchMode.SystemPreferred,
    //                TitleMode = BrowserTitleMode.Hide
    //            });
    //        }

    //        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
    //        {
    //            WebView.HeightRequest = 700;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    //async private void BackBtn_Clicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (WebViewerStack.IsVisible == true)
    //        {
    //            WebViewerStack.IsVisible = false;
    //            mainstack.IsVisible = true;
    //            AndroidBtn.IsVisible = false;
    //            IOSBtn.IsVisible = false;
    //            NavigationPage.SetHasNavigationBar(this, true);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}
}