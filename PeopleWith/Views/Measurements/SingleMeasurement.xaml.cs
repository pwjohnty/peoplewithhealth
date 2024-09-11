using CommunityToolkit.Mvvm.Messaging;
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
    bool newmeasurement;
    public SingleMeasurement()
	{
		InitializeComponent();
	}

    public SingleMeasurement(measurement measurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed)
    {
        //measurement

        InitializeComponent();
        measurementpassed = measurementp;
        usermeasurementlistpassed = usermeasurementsp;
        measurementlist = measurementlistpassed;

        measurementname.Text = measurementpassed.measurementname;
        newmeasurement = true;

        lblvalue.Text = "No Data";

        datelbl.Text = "Today";

        charticon.IsVisible = false;
    }

    public SingleMeasurement(usermeasurement usermeasurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed)
    {
        //usermeasurement

        InitializeComponent();
        usermeasurementpassed = usermeasurementp;
        usermeasurementlistpassed = usermeasurementsp;
        measurementlist = measurementlistpassed;

        measurementname.Text = usermeasurementpassed.measurementname;


        unitlbl.Text = usermeasurementpassed.unit;
        detailslbl.IsVisible = true;
        detailsframe.IsVisible = true;
        showallbtn.IsVisible = true;

        populatechart();

        WeakReferenceMessenger.Default.Register<SendItemMessage>(this, (r, m) =>
        {
            usermeasurementlistpassed = (ObservableCollection<usermeasurement>)m.Value;
            populatechart();
        });

    }

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

                    var num = Convert.ToDouble(item.value);
                    item.numconverted = num;
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
                    var mindate = (DateTime)usermeasurementchartlist.Min(x => x.dateconverted);
                    var maxdate = (DateTime)usermeasurementchartlist.Max(x => x.dateconverted);
                    //add a min and max date so the chart looks better
                    var minusermeasurement = new usermeasurement();
                    var maxusermeasurement = new usermeasurement();
                    minusermeasurement.inputdatetime = mindate.AddDays(-1).ToString("dd/MM/yyyy HH:mm");
                    usermeasurementchartlist.Add(minusermeasurement);
                    maxusermeasurement.inputdatetime = maxdate.AddDays(+1).ToString("dd/MM/yyyy HH:mm");
                    usermeasurementchartlist.Add(maxusermeasurement);
                    orderlistbydate = new ObservableCollection<usermeasurement>(usermeasurementchartlist.OrderBy(x => DateTime.Parse(x.inputdatetime)).ToList());
                    datachart.Series.Clear();

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

                    NumericalAxis secondaryAxis = new NumericalAxis();
                    secondaryAxis.LabelStyle.TextColor = Colors.LightGray;
                    secondaryAxis.LabelStyle.FontFamily = "HankenGroteskRegular";
                    secondaryAxis.AxisLineStyle.Stroke = Colors.LightGray;
                    secondaryAxis.MajorTickStyle.Stroke = Colors.LightGray;
                    secondaryAxis.AxisLineStyle.StrokeWidth = 1;//Hide Axis line 
                    secondaryAxis.MajorTickStyle.StrokeWidth = 1;//Hide TickLines 
                    secondaryAxis.IsVisible = true;
                    secondaryAxis.Minimum = minvaluetwo - 5;
                    secondaryAxis.Maximum = maxvalue + 5;
                    datachart.YAxes.Add(secondaryAxis);

                    ChartMarkerSettings chartMarker = new ChartMarkerSettings();
                    chartMarker.Type = ShapeType.Circle;
                    chartMarker.Fill = Colors.White;
                    chartMarker.Stroke = Color.FromRgba("#031926");
                    chartMarker.StrokeWidth = 2;
                    chartMarker.Height = 8;
                    chartMarker.Width = 8;

                    LineSeries columnseries = new LineSeries
                    {
                        ItemsSource = orderlistbydate,
                        XBindingPath = "inputdatetime",
                        YBindingPath = "BPone",
                        Fill = Color.FromArgb("#031926"),
                        //ShapeType = ChartScatterShapeType.Ellipse,
                        // ScatterHeight = 15,
                        // ScatterWidth = 15,
                        ShowMarkers = true,
                        StrokeWidth = 2,
                        MarkerSettings = chartMarker,
                        EnableTooltip = true,
                        EnableAnimation = true
                    };

                    ChartMarkerSettings chartMarkers = new ChartMarkerSettings();
                    chartMarkers.Type = ShapeType.Circle;
                    chartMarkers.Fill = Colors.White;
                    chartMarkers.Stroke = Color.FromRgba("#BFDBF7");
                    chartMarkers.StrokeWidth = 2;
                    chartMarkers.Height = 8;
                    chartMarkers.Width = 8;


                    LineSeries columnseriestwo = new LineSeries
                    {
                        ItemsSource = orderlistbydate,
                        XBindingPath = "inputdatetime",
                        YBindingPath = "BPtwo",
                        Fill = Color.FromArgb("#BFDBF7"),
                        // ShapeType = ChartScatterShapeType.Ellipse,
                        // ScatterHeight = 15,
                        // ScatterWidth = 15,
                        ShowMarkers = true,
                        MarkerSettings = chartMarkers,
                        StrokeWidth = 2,
                        EnableTooltip = true,
                        EnableAnimation = true
                    };

                  
                    /* columnseries.DataMarker = new ChartDataMarker();
                     columnseries.DataMarker.ShowLabel = false;
                     columnseries.DataMarker.ShowMarker = true;
                     columnseries.DataMarker.MarkerType = DataMarkerType.Ellipse;
                     columnseries.DataMarker.MarkerWidth = 20;
                     columnseries.DataMarker.MarkerHeight = 20;
                     columnseries.DataMarker.MarkerColor = Color.FromHex("#0F9FE2");
                    */
                    datachart.Series.Add(columnseries);
                    datachart.Series.Add(columnseriestwo);
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
                    usermeasurementchartlist.Add(minusermeasurement);

                    maxusermeasurement.inputdatetime = maxdate.AddDays(+1).ToString("dd/MM/yyyy HH:mm");
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



                    DataPointSelectionBehavior selection = new DataPointSelectionBehavior();
                    //   selection.SelectionBrush = Colors.Red;
                    selection.SelectionChanged += OnSelectionChanged;

                    LineSeries columnseries = new LineSeries
                    {
                        ItemsSource = orderlistbydate,
                        XBindingPath = "inputdatetime",
                        YBindingPath = "value",
                        Fill = Color.FromHex("#BFDBF7"),
                        // ShapeType = ChartScatterShapeType.Ellipse,
                        // ScatterHeight = 15,
                        // ScatterWidth = 15,
                        StrokeWidth = 2,
                        EnableTooltip = true,
                        EnableAnimation = true,
                        ShowMarkers = true,
                        ShowTrackballLabel = false,
                        SelectionBehavior = selection,
                        MarkerSettings = chartMarker
                    };

                    columnseries.ShowDataLabels = false;

                    //ChartTrackballBehavior trackball = new ChartTrackballBehavior();
                    //trackball.ShowLine = true;
                    //trackball.DisplayMode = LabelDisplayMode.FloatAllPoints;
                    //datachart.TrackballBehavior = trackball;

                    datachart.Series.Add(columnseries);
                }

                lblvalue.Text = orderlistbydate[orderlistbydate.Count - 2].value;
                lblunit.Text = orderlistbydate[orderlistbydate.Count - 2].unit;

                var convertdate = DateTime.Parse(orderlistbydate[orderlistbydate.Count - 2].inputdatetime);

                datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");
            }

        }
        catch(Exception ex)
        {

        }
    }

    private void OnSelectionChanged(object sender, ChartSelectionChangedEventArgs e)
    {
        try
        {

            var es = e.NewIndexes[0];

            lblvalue.Text = orderlistbydate[es].value;

            var convertdate = DateTime.Parse(orderlistbydate[es].inputdatetime);

            datelbl.Text = convertdate.ToString("HH:mm, dd MMMM yyyy");


        }
        catch (Exception ex)
        {

        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (newmeasurement)
            {
                await Navigation.PushAsync(new AddMeasurement(measurementpassed, usermeasurementlistpassed, measurementlist), false);
            }
            else
            {
                await Navigation.PushAsync(new AddMeasurement(usermeasurementpassed, usermeasurementlistpassed, measurementlist), false);
            }
        }
        catch(Exception ex )
        {

        }
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {
            //show all button clicked
            var usermeasurementalldatalist = new ObservableCollection<usermeasurement>(usermeasurementlistpassed.Where(x => x.measurementid == usermeasurementpassed.measurementid));
            await Navigation.PushAsync(new ShowAllData(usermeasurementalldatalist, usermeasurementlistpassed, measurementlist), false);
        }
        catch(Exception ex)
        {

        }
    }
}