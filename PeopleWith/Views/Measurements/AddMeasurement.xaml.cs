using System.Collections.ObjectModel;
using Mopups.Services;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;
namespace PeopleWith;

public partial class AddMeasurement : ContentPage
{
    measurement measurementpassed;
    usermeasurement usermeasurementpassed;
    ObservableCollection<usermeasurement> usermeasurementlistpassed = new ObservableCollection<usermeasurement>();
    ObservableCollection<measurement> measurementlist = new ObservableCollection<measurement>();
    string inputvalue;
    string numvalueconverted;
    string measurementnamestring;
    string measurementid;
    string Stonesinput;
    string Poundsinput;
    APICalls aPICalls = new APICalls();
    bool validinput;
    int HourInput = 0;
    int MinInput = 0;
    public bool IsEditFeedback = false; 
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
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public AddMeasurement()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddMeasurement(measurement measurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();

            measurementpassed = measurementp;
            usermeasurementlistpassed = usermeasurementsp;
            measurementlist = measurementlistpassed;
            userfeedbacklistpassed = userfeedbacklist;

            measurementname.Text = "Add " + measurementpassed.measurementname;
            measurementnamestring = measurementpassed.measurementname;
            measurementid = measurementpassed.measurementid;

            var unitstringlist = new List<string>();

            if (measurementnamestring == "Blood Pressure")
            {
                unitentryframe.IsVisible = false;
                bpsysframe.IsVisible = true;
                bpdiaframe.IsVisible = true;

                var joinstring = measurementpassed.units.Replace(',', '/');

                unitstringlist.Add(joinstring);

                unitlist.ItemsSource = unitstringlist;

                unitlist.SelectedItem = joinstring;

                inputvalue = joinstring;

            }
            else if (measurementnamestring == "Sleep Duration")
            {
                //Set Hours/minutes Btn to pre-Selected 

                var joinstring = measurementpassed.units.Replace(',', '/');
                unitstringlist.Add(joinstring);
                unitlist.ItemsSource = unitstringlist;
                unitlist.SelectedItem = joinstring;
                //Set Sleep Duration Frame to Visible 

                unitentryframe.IsVisible = false;
                SleepDurationFrame.IsVisible = true;
                SleepQualFrame.IsVisible = true;

                //Sleep Quality List 

                List<string> SleepQual = new List<string>();
                SleepQual.Add("Excellent");
                SleepQual.Add("Good");
                SleepQual.Add("Fair");
                SleepQual.Add("Poor");
                SleepQualitySelect.ItemsSource = SleepQual;

            }
            else
            {
                unitstringlist = measurementpassed.units.Split(',').ToList();
                unitlist.ItemsSource = unitstringlist;
            }

            adddatepicker.Date = DateTime.Now;
            adddatepicker.MaximumDate = DateTime.Now;
            addtimepicker.Time = DateTime.Now.TimeOfDay;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    public AddMeasurement(usermeasurement usermeasurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();

            usermeasurementpassed = usermeasurementp;
            usermeasurementlistpassed = usermeasurementsp;
            measurementlist = measurementlistpassed;
            userfeedbacklistpassed = userfeedbacklist;

            measurementname.Text = "Add " + usermeasurementpassed.measurementname;
            measurementnamestring = usermeasurementpassed.measurementname;
            measurementid = usermeasurementpassed.measurementid;

            if (measurementnamestring == "Blood Pressure")
            {
                unitentryframe.IsVisible = false;
                bpsysframe.IsVisible = true;
                bpdiaframe.IsVisible = true;
            }

            if (measurementnamestring == "Weight" && usermeasurementpassed.unit == "Stones/Pounds")
            {
                unitentryframe.IsVisible = false;
                StonesPoundsframe.IsVisible = true;
                stlbl.Text = "St";
                lbslbl.Text = "lbs";
            }

            if (measurementnamestring == "Sleep Duration")
            {
                unitentryframe.IsVisible = false;
                SleepDurationFrame.IsVisible = true;
                SleepQualFrame.IsVisible = true;

                //Sleep Quality List 

                List<string> SleepQual = new List<string>();
                SleepQual.Add("Excellent");
                SleepQual.Add("Good");
                SleepQual.Add("Fair");
                SleepQual.Add("Poor");
                SleepQualitySelect.ItemsSource = SleepQual;
            }

            var unitstringlist = new List<string>();

            unitstringlist.Add(usermeasurementpassed.unit);

            unitlist.ItemsSource = unitstringlist;
            unitlist.SelectedItem = unitstringlist[0];

            inputvalue = unitstringlist[0];
            lblentryunit.Text = unitstringlist[0];

            unitentry.IsEnabled = true;

            adddatepicker.Date = DateTime.Now;
            adddatepicker.MaximumDate = DateTime.Now;
            addtimepicker.Time = DateTime.Now.TimeOfDay;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AddMeasurement(usermeasurement usermeasurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed, userfeedback userfeedbacklist, bool EditFeedback)
    {
        try
        {
            InitializeComponent();

            usermeasurementpassed = usermeasurementp;
            usermeasurementlistpassed = usermeasurementsp;
            measurementlist = measurementlistpassed;
            userfeedbacklistpassed = userfeedbacklist;
            IsEditFeedback = EditFeedback;

            //Hide Unit Select. Not Needed
            UnitGrid.IsVisible = false;
            MeasurementReadTitle.Margin = new Thickness(0, 20, 0, 0);

            measurementname.Text = "Edit " + usermeasurementpassed.measurementname;
            measurementnamestring = usermeasurementpassed.measurementname;
            measurementid = usermeasurementpassed.measurementid;

            if (measurementnamestring == "Blood Pressure")
            {
                unitentryframe.IsVisible = false;
                bpsysframe.IsVisible = true;
                bpdiaframe.IsVisible = true;

                var Value = String.Empty;
                // Use numconverted if not null else user value
                bool CheckBoth = usermeasurementpassed.numconverted != 0 || usermeasurementpassed.numconvertedtwo != 0;

                if (CheckBoth)
                {
                    sysentry.Text = usermeasurementpassed.numconverted.ToString();
                    diaentry.Text = usermeasurementpassed.numconvertedtwo.ToString();
                }
                else
                {
                    Value = usermeasurementpassed.value;

                    if (!string.IsNullOrEmpty(Value))
                    {
                        if (Value.Contains("/"))
                        {
                            var splitstring = Value.Split('/');
                            if (splitstring.Length >= 2)
                            {
                                sysentry.Text = splitstring[0];
                                diaentry.Text = splitstring[1];
                            }
                        }
                        else
                        {
                            // Do Something 
                        }
                    }
                }
            }

            if (measurementnamestring == "Weight" && usermeasurementpassed.unit == "Stones/Pounds")
            {
                unitentryframe.IsVisible = false;
                StonesPoundsframe.IsVisible = true;
                stlbl.Text = "St";
                lbslbl.Text = "lbs";

                var Value = String.Empty;

                // Use numconverted if not null else user value
                Value = usermeasurementpassed.value ?? string.Empty;

                if(!string.IsNullOrEmpty(Value))
                {
                    if (Value.Contains("."))
                    {
                        var splitstring = Value.Split('.');
                        if (splitstring.Length >= 2)
                        {
                            Stonesentry.Text = splitstring[0];
                            Poundsentry.Text = splitstring[1];
                        }
                    }
                    else if (Value.Contains("st") && Value.Contains("lbs"))
                    {
                        var RemoveItems = Value.Replace("st", "").Replace("lbs", "");
                        var splitstring = RemoveItems.Split(' ');
                        if (splitstring.Length >= 2)
                        {
                            Stonesentry.Text = splitstring[0];
                            Poundsentry.Text = splitstring[1];
                        }
                    }
                    else
                    {
                        // Do Something 
                    }
                }
               
            }

            if (measurementnamestring == "Sleep Duration")
            {
                unitentryframe.IsVisible = false;
                SleepDurationFrame.IsVisible = true;
                SleepQualFrame.IsVisible = true;

                //Sleep Quality List 

                List<string> SleepQual = new List<string>();
                SleepQual.Add("Excellent");
                SleepQual.Add("Good");
                SleepQual.Add("Fair");
                SleepQual.Add("Poor");
                SleepQualitySelect.ItemsSource = SleepQual;

                //Duration 

                var Value = String.Empty;

                // Use numconverted if not null else user value
                Value = usermeasurementpassed.numconverted?.ToString() ?? usermeasurementpassed.value;

                if (!string.IsNullOrEmpty(Value))
                {
                    if (Value.Contains("."))
                    {
                        var splitstring = Value.Split('.');
                        if (splitstring.Length >= 2)
                        {
                            hoursentry.Text = splitstring[0].PadLeft(2, '0');
                            minsentry.Text = splitstring[1].PadLeft(2, '0');
                        }
                    }
                    else if (Value.Contains("h") && Value.Contains("m"))
                    {
                        var RemoveItems = Value.Replace("h", "").Replace("m", "");
                        var splitstring = RemoveItems.Split(' ');
                        if (splitstring.Length >= 2)
                        {
                            hoursentry.Text = splitstring[0].PadLeft(2, '0');
                            minsentry.Text = splitstring[1].PadLeft(2, '0');
                        }
                    }
                    else
                    {
                        // Do Something 
                    }
                }

                //Sleep Quality if Applicable 
                if (!string.IsNullOrEmpty(usermeasurementpassed.inputmethod))
                {
                    SleepQualitySelect.SelectedItem = SleepQual.FirstOrDefault(usermeasurementpassed.inputmethod); 
                }

            }

            if (unitentryframe.IsVisible != false)
            {
                unitentry.Text = usermeasurementpassed.value; 
            }
            var unitstringlist = new List<string>();

            unitstringlist.Add(usermeasurementpassed.unit);

            unitlist.ItemsSource = unitstringlist;
            unitlist.SelectedItem = unitstringlist[0];

            inputvalue = unitstringlist[0];
            lblentryunit.Text = unitstringlist[0];

            unitentry.IsEnabled = true;


            //Set to Date Time of Item 
            adddatepicker.Date = DateTime.Parse(usermeasurementpassed.inputdatetime).Date;
            adddatepicker.MaximumDate = DateTime.Now;
            addtimepicker.Time = DateTime.Parse(usermeasurementpassed.inputdatetime).TimeOfDay;

            //Update Selected items
            SubmitBtn.Text = "Update";
            DeleteBtn.IsVisible = true;

            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            validinput = true;

            //Show Delete Button
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void unitlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;
            inputvalue = item;

            if (inputvalue == "Stones/Pounds")
            {
                unitentryframe.IsVisible = false;
                StonesPoundsframe.IsVisible = true;
                unitentry.IsEnabled = false;
                stlbl.Text = "St";
                lbslbl.Text = "lbs";
            }
            else if (inputvalue == "Systolic/Diastolic")
            {
                //Do Nothing 
            }
            else if (inputvalue == "Hours/Minutes")
            {
                //Do Nothing 
            }
            else
            {
                StonesPoundsframe.IsVisible = false;
                unitentryframe.IsVisible = true;
                unitentry.IsEnabled = true;
                lblentryunit.Text = item;
            }

            Stonesentry.Text = string.Empty;
            Poundsentry.Text = string.Empty;
            unitentry.Text = string.Empty;

            SubmitBtn.BackgroundColor = Colors.Gray;
            SubmitBtn.TextColor = Colors.LightGray;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            dtPicker.IsOpen = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void unitentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

#if ANDROID
                var handler = unitentry.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(unitentry.Text);
                }
#endif

            if (e.NewTextValue.Length > 8)
            {
                return;
            }
            if (e.NewTextValue.Length == 1 && e.NewTextValue == ".")
            {
                validinput = false;
                unitentry.Text = string.Empty;
                return;
            }

            string newvalue;

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                validinput = false;

                unitentry.Text = string.Empty;
            }
            else
            {


                if (measurementnamestring == "Weight")
                {
                    //weight
                    if (inputvalue == "Kg" || inputvalue == "kg")
                    {

                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 2.9 && convertodec >= 361.9)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 2 && convertoint >= 362)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }




                    }
                    else if (inputvalue == "Pounds")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 4.9 && convertodec >= 799.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 4 && convertoint >= 799)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                    }
                    else if (inputvalue == "lbs")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 4.9 && convertodec >= 799.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 4 && convertoint >= 799)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                    }
                    else if (inputvalue == "Stones")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 0.4 && convertodec >= 56.9)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {
                                if (convertoint < 1 && convertoint >= 57)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }

                else if (measurementnamestring == "Height")
                {

                    var Value = e.NewTextValue;
                    if (inputvalue == "cm")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 29.9 && convertodec >= 299.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 29)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 300)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                    else if (inputvalue == "Feet")
                    {

                        var valuestring = e.NewTextValue;
                        unitentry.Text = valuestring;
                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            //  else if (convertodec <= 4.9)
                            //  {
                            //     validinput = false;
                            //     
                            //  }
                            //  else
                            //  {
                            //      SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            //      validinput = true;

                            //      Double dc = Math.Round((Double)convertodec, 2);
                            //      numvalueconverted = dc.ToString();
                            //  }
                        }
                        else
                        {
                            //if (e.NewTextValue.Contains("'"))
                            //{
                            //    Value = e.NewTextValue.Replace("'", string.Empty); 
                            //}

                            var convertoint = Convert.ToInt32(Value);


                            if (e.NewTextValue.Length < 2)
                            {
                                if (convertoint >= 3 && convertoint <= 11)
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;

                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }



                    }
                    else if (inputvalue == "Inches")
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 5.9 && convertodec >= 89.9)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {
                                if (convertoint <= 6)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 90)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                    else if (inputvalue == "Feet/Inches")
                    {
                        var valuestring = e.NewTextValue;
                        if (valuestring == "' ")
                        {
                            unitentry.Text = "";
                            return;
                        }
                        else if (valuestring == "'")
                        {
                            unitentry.Text = "";
                            return;
                        }

                        if (e.NewTextValue.Length == 2 && e.OldTextValue.Length == 3)
                        {
                            unitentry.CursorPosition = 1;
                            return;
                        }


                        if (valuestring.Contains("'"))
                        {
                            valuestring = valuestring.Replace("'", "");
                        }

                        // Replace space
                        if (valuestring.Contains(" "))
                        {
                            valuestring = valuestring.Replace(" ", "");
                        }

                        // Replace double quote
                        if (valuestring.Contains("\""))
                        {
                            valuestring = valuestring.Replace("\"", "");
                        }

                        // Ensure Not Empty
                        if (string.IsNullOrEmpty(valuestring))
                        {
                            return;
                        }
                        else
                        {
                            if (valuestring.Length == 1)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var feet = Convert.ToInt32(firstnum);
                                unitentry.Text = $"{feet}' ";
                                unitentry.CursorPosition = unitentry.Text.Length;
                            }
                            else if (valuestring.Length == 2)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var secondnum = valuestring[1].ToString(); // Inches

                                var feet = Convert.ToInt32(firstnum);
                                var inches = Convert.ToInt32(secondnum);

                                //var formattedInches = inches < 10 ? "0" + inches : inches.ToString();

                                unitentry.Text = $"{feet}' {inches}\"";
                            }
                            else if (valuestring.Length == 3)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var inchString = valuestring.Substring(1, 2); //Inches

                                var feet = Convert.ToInt32(firstnum);
                                var inches = Convert.ToInt32(inchString);

                                // Ensure inches are valid (between 0-11)

                                //var formattedInches = inches < 10 ? "0" + inches : inches.ToString();

                                unitentry.Text = $"{feet}' {inches}\"";
                            }

                            if (valuestring.Length == 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;
                            }
                            else if (valuestring.Length == 2)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var secondnum = valuestring[1].ToString(); // Inches

                                // Convert feet and inches to integers
                                int feet = Convert.ToInt32(firstnum);
                                int inches = Convert.ToInt32(secondnum);

                                // Convert feet and inches to total inches
                                int totalInches = (feet * 12) + inches;

                                if (totalInches < 5)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (totalInches > 108)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;
                                }
                                else
                                {
                                    validinput = true;
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                }
                            }
                            else if (valuestring.Length == 3)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var inchString = valuestring.Substring(1, 2); //Inches

                                // Convert feet and inches to integers
                                int feet = Convert.ToInt32(firstnum);
                                int inches = Convert.ToInt32(inchString);

                                //Check For Valid inches 
                                if (inches < 0 || inches > 11)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;
                                    return;
                                }

                                // Convert feet and inches to total inches
                                int totalInches = (feet * 12) + inches;

                                if (totalInches < 5)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (totalInches > 108)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;
                                }
                                else
                                {
                                    validinput = true;
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;
                            }
                        }
                    }
                }

                else if (measurementnamestring == "Waist")
                {

                    if (inputvalue == "cm")
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 9.9 && convertodec >= 299.9)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 9)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 299)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                    else if (inputvalue == "Inches")
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 4 && convertodec >= 116.9)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 4)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 117)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                }
                else if (measurementnamestring == "Body Fat")
                {
                    if (inputvalue == "%")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec >= 100.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 100)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Haematocrit")
                {
                    if (inputvalue == "%")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec >= 100.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 100)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Heart/Pulse Rate")
                {
                    if (inputvalue == "BPM (Beats Per Minute)")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 30)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 350)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Iron Level")
                {
                    if (inputvalue == "mcg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 60)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 170)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Total Cholesterol Level")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 200)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 240)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "LDL Cholesterol Level")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 130)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 160)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "mmol/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Ferritin Level")
                {
                    if (inputvalue == "ug/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }

                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 500)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }

                    if (inputvalue == "ng/mL")
                    {
                        unitentry.Text = e.NewTextValue;
                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);
                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 80)
                            {
                                if (convertoint < 13)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 400)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }

                }
                else if (measurementnamestring == "HDL Cholesterol Level")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 50)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 60)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "mmol/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }

                }
                else if (measurementnamestring == "Triglycerides Level")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 150)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 200)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Platelet Count")
                {
                    if (inputvalue == "billion/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 2 && e.NewTextValue.Length <= 3)
                            {
                                /* if (convertoint < 0)
                                 {
                                    validinput = false;
                                    
                                 }
                                 else if (convertoint > 200)
                                 {
                                    validinput = false;
                                    
                                 }
                                 else
                               */ // {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                // }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "mcL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 2 && e.NewTextValue.Length <= 3)
                            {
                                /* if (convertoint < 0)
                                 {
                                    validinput = false;
                                    
                                 }
                                 else if (convertoint > 200)
                                 {
                                    validinput = false;
                                    
                                 }
                                 else
                               */ // {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                // }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Haemoglobin")
                {
                    if (inputvalue == "grams/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "grams/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Red Blood Cell Count")
                {
                    if (inputvalue == "Million cells/mcL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "Trillion cells/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Triiodothyronine (T3)")
                {
                    if (inputvalue == "ng/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                // Double dc = Math.Round((Double)convertodec, 2);
                                // numvalueconverted = dc.ToString();

                                // SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                // validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Thyroid-stimulating hormone (TSH)")
                {
                    if (inputvalue == "mIU/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                // Double dc = Math.Round((Double)convertodec, 2);
                                // numvalueconverted = dc.ToString();

                                // SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                // validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }

                else if (measurementnamestring == "HbA1c")
                {
                    if (inputvalue == "%")
                    {
                        unitentry.Text = e.NewTextValue;
                        var convertoint = Convert.ToDouble(e.NewTextValue);

                        if (convertoint < 5.0)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else if (convertoint > 14.1)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {
                            numvalueconverted = convertoint.ToString();
                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            SubmitBtn.TextColor = Colors.White;
                            validinput = true;
                        }

                    }
                    else if (inputvalue == "mmol/mol")
                    {
                        unitentry.Text = e.NewTextValue;
                        var convertoint = Convert.ToDouble(e.NewTextValue);

                        if (e.NewTextValue.Contains("."))
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {
                            if (convertoint < 31)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertoint > 131)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }

                    }
                }

                else if (measurementnamestring == "PSA Level")
                {
                    if (inputvalue == "mg/ml")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Erythrocyte Sediment Rate (ESR)")
                {
                    if (inputvalue == "mm/hr")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "White Blood Cell Count")
                {
                    if (inputvalue == "cells/mcL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 4)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "billion cells/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "C-reactive Protein Level")
                {
                    if (inputvalue == "mg/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Blood Oxygen Level (SaO2)")
                {
                    if (inputvalue == "%")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec >= 100.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 100)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "AST Level")
                {
                    if (inputvalue == "IU/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "ALP Level")
                {
                    if (inputvalue == "IU/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "ukat/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {


                            if (string.IsNullOrEmpty(e.NewTextValue))
                            {

                            }
                            else
                            {
                                var convertoint = Convert.ToInt32(e.NewTextValue);

                                if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                                {

                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;


                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }

                            }

                        }
                    }
                }
                else if (measurementnamestring == "Albumin Level")
                {
                    if (inputvalue == "g/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Lung Capacity/Volume Measurement")
                {
                    if (inputvalue == "mL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 3 && e.NewTextValue.Length <= 5)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Bilirubin Level")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "mmol/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "ALT Level")
                {
                    if (inputvalue == "IU/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Blood Glucose/Sugar Levels")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                    else if (inputvalue == "mmol/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Ketone Level (Blood)")
                {
                    if (inputvalue == "mmol/L")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "BNP (B-Type Natriuretic Peptide)")
                {
                    if (inputvalue == "pg/ml")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Thyroxine (T4)")
                {
                    if (inputvalue == "mcg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "T3 Resin Uptake (RU)")
                {
                    if (inputvalue == "ng/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Fever/Temperature ")
                {

                    if (inputvalue.Contains("°C"))
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 29.9 && convertodec >= 42.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 1);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {
                                if (convertoint <= 32)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 43)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                    else if (inputvalue.Contains("°F"))
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec <= 91.9 && convertodec >= 107.1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 91)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 108)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                }
                else if (measurementnamestring == "BMI")
                {
                    if (inputvalue == "BMI")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                            {

                                if (convertoint < 7)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 75)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Peak Flow Rate")
                {
                    //make the label show the most recent number 
                    unitentry.Text = e.NewTextValue;

                    if (e.NewTextValue.Contains("."))
                    {
                        var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                        var convertodec = Convert.ToDouble(e.NewTextValue);
                        if (countdots > 1)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            SubmitBtn.TextColor = Colors.White;
                            validinput = true;
                        }
                    }
                    else
                    {
                        var convertoint = Convert.ToInt32(e.NewTextValue);

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 4)
                        {

                            if (convertoint < 110)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertoint > 1200)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }
                }
                else if (measurementnamestring == "Respiration Rate (Breathes Per Minute)")
                {
                    //make the label show the most recent number 
                    unitentry.Text = e.NewTextValue;

                    if (e.NewTextValue.Contains("."))
                    {
                        var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                        var convertodec = Convert.ToDouble(e.NewTextValue);
                        if (countdots > 1)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            SubmitBtn.TextColor = Colors.White;
                            validinput = true;
                        }
                    }
                    else
                    {
                        var convertoint = Convert.ToInt32(e.NewTextValue);

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                        {

                            if (convertoint < 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertoint > 100)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }
                }
                else if (measurementnamestring == "Rheumatoid Factor Level")
                {
                    if (inputvalue == "IU/ml")
                    {
                        unitentry.Text = e.NewTextValue;
                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 0)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);
                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 80)
                            {
                                if (convertoint < 1)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 80)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Blood Pressure")
                {

                    unitlist.IsVisible = false;
                }
                else if (measurementnamestring == "Fluid Balance")
                {
                    unitentry.Text = e.NewTextValue;

                    if (e.NewTextValue.Contains("."))
                    {
                        var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                        var convertodec = Convert.ToDouble(e.NewTextValue);
                        if (countdots > 1)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            SubmitBtn.TextColor = Colors.White;
                            validinput = true;
                        }
                    }
                    else
                    {
                        var convertoint = Convert.ToInt32(e.NewTextValue);

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 6)
                        {
                            if (convertoint > 0)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Fluid Input")
                {
                    unitentry.Text = e.NewTextValue;

                    if (e.NewTextValue.Contains("."))
                    {
                        var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                        var convertodec = Convert.ToDouble(e.NewTextValue);
                        if (countdots > 1)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            SubmitBtn.TextColor = Colors.White;
                            validinput = true;
                        }
                    }
                    else
                    {
                        var convertoint = Convert.ToInt32(e.NewTextValue);

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 6)
                        {
                            if (convertoint > 0)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Fluid Output")
                {
                    unitentry.Text = e.NewTextValue;

                    if (e.NewTextValue.Contains("."))
                    {
                        var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                        var convertodec = Convert.ToDouble(e.NewTextValue);
                        if (countdots > 1)
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            SubmitBtn.TextColor = Colors.White;
                            validinput = true;
                        }
                    }
                    else
                    {
                        var convertoint = Convert.ToInt32(e.NewTextValue);

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 6)
                        {
                            if (convertoint > 0)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Sleep Duration")
                {
                    var convertoint = Convert.ToInt32(e.NewTextValue);
                    if (inputvalue == "Hours")
                    {
                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 37)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }
                    else
                    {

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 2161)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Estimated Glomerular Filtration Rate (eGFR)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "mL/min/1.73 m2")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 3)
                        {
                            if (convertoint > 0 && convertoint < 150)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Gamma-glutamyltransferase (GGT)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "IU/L")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 3)
                        {
                            if (convertoint > 0 && convertoint < 111)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Very Low-Density Lipoprotein Cholesterol (VLDL)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "mg/dL")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 51)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Thyroid Peroxidase Antibody (TPOAb)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "IU/mL")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 4)
                        {
                            if (convertoint > 0 && convertoint < 3001)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Thyroglobulin Antibody (TgAb)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "IU/mL")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 4)
                        {
                            if (convertoint > 0 && convertoint < 5001)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Thyroid-Stimulating Immunoglobulin (TSI)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "IU/L")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 4)
                        {
                            if (convertoint > 0 && convertoint < 5001)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }
                else if (measurementnamestring == "Calcitonin Level")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "pg/mL")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 3)
                        {
                            if (convertoint > 0 && convertoint < 121)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }
                }
                else if (measurementnamestring == "Lactate dehydrogenase (LDH)")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "IU/L")
                    {
                        if (ValueString.Length == 3)
                        {
                            if (convertoint > 99 && convertoint < 251)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }
                }
                else if (measurementnamestring == "Prothrombin time (PT)") //Can have Decimal point
                {
                    var ValueString = e.NewTextValue;
                    var convertoint = Convert.ToDouble(ValueString);
                    if (inputvalue == "Seconds")
                    {
                        var CheckDouble = convertoint.ToString();
                        if (CheckDouble.Contains("."))
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 5)
                            {
                                if (convertoint > 0 && convertoint < 21.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 2)
                            {
                                if (convertoint > 0 && convertoint < 21.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }

                else if (measurementnamestring == "Total Liver Proteins") //Can have Decimal point
                {
                    var ValueString = e.NewTextValue;

                    var convertoint = Convert.ToDouble(ValueString);
                    if (inputvalue == "g/dL")
                    {
                        var CheckDouble = convertoint.ToString();
                        if (CheckDouble.Contains("."))
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 5)
                            {
                                if (convertoint > 0 && convertoint < 10.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 2)
                            {
                                if (convertoint > 0 && convertoint < 10.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                    }
                }
                else if (measurementnamestring == "Fibrosis-4 (FIB-4)") //Can have Decimal point
                {
                    var ValueString = e.NewTextValue;

                    var convertoint = Convert.ToDouble(ValueString);
                    if (inputvalue == "score")
                    {
                        var CheckDouble = convertoint.ToString();
                        if (CheckDouble.Contains("."))
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 4)
                            {
                                if (convertoint > 0 && convertoint < 5.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 1)
                            {
                                if (convertoint > 0 && convertoint < 6)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                    }
                }
                else if (measurementnamestring == "Enhanced Liver Fibrosis (ELF)") //Can have Decimal point
                {
                    var ValueString = e.NewTextValue;

                    var convertoint = Convert.ToDouble(ValueString);
                    if (inputvalue == "score")
                    {
                        var CheckDouble = convertoint.ToString();
                        if (CheckDouble.Contains("."))
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 5)
                            {
                                if (convertoint > 0 && convertoint < 15.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            if (ValueString.Length >= 1 && ValueString.Length <= 2)
                            {
                                if (convertoint > 0 && convertoint < 15.1)
                                {
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                    numvalueconverted = convertoint.ToString();
                                }
                                else
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }

                    }
                }

                else if (measurementnamestring == "Proteinuria Level")
                {
                    if (inputvalue == "milligrams of protein per day")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                Double dc = Math.Round((Double)convertodec, 2);
                                numvalueconverted = dc.ToString();

                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 4)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }

                else if (measurementnamestring == "Oral Glucose Tolerance Test (OGTT)")
                {
                    if (inputvalue == "mg/dL")
                    {
                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (convertodec < 100)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else if (convertodec > 240)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                            else
                            {
                                numvalueconverted = convertodec.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                            }

                        }
                        else
                        {
                            var convertoint = Convert.ToInt32(e.NewTextValue);

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 100)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else if (convertoint > 240)
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    SubmitBtn.TextColor = Colors.White;
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                    }
                }

                else if (measurementnamestring == "Endometrial Thickness")
                {
                    var ValueString = e.NewTextValue;
                    if (e.NewTextValue.Contains("."))
                    {
                        ValueString = e.NewTextValue.Replace(".", "");
                        unitentry.Text = ValueString;
                    }
                    var convertoint = Convert.ToInt32(ValueString);
                    if (inputvalue == "mm")
                    {
                        if (ValueString.Length >= 1 && ValueString.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 21)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }
                    else
                    {

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 2161)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                SubmitBtn.TextColor = Colors.White;
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;

                            }
                        }
                        else
                        {
                            validinput = false;
                            SubmitBtn.BackgroundColor = Colors.Gray;
                            SubmitBtn.TextColor = Colors.LightGray;

                        }
                    }

                }

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                SubmitBtn.IsEnabled = false;
                if (validinput == false)
                {
                    Vibration.Vibrate();
                    SubmitBtn.IsEnabled = true;
                    return;
                }
                else
                {

                    if (IsEditFeedback)
                    {
                        //Edit Existing UserMeasurement
                        EditMeasurementData();
                    }
                    else
                    {
                        //add new usermeasurement 
                        var newmeasurment = new usermeasurement();
                        string userid = Preferences.Default.Get("userid", "Unknown");
                        newmeasurment.userid = userid;
                        newmeasurment.measurementid = measurementid;
                        newmeasurment.measurementname = measurementnamestring;

                        if (measurementnamestring == "Blood Pressure")
                        {
                            newmeasurment.value = sysentry.Text + "/" + diaentry.Text;
                        }
                        else if (measurementnamestring == "Weight" && inputvalue == "Stones/Pounds")
                        {

                            newmeasurment.value = Stonesentry.Text + "." + Poundsentry.Text;
                            //newmeasurment.value = Stonesinput + "st " + Poundsinput + "lbs";

                        }
                        else if (measurementnamestring == "Height" && inputvalue == "Feet/Inches")
                        {

                            var height = unitentry.Text.ToString();
                            string cleanInput = height.Replace("'", "").Replace("\"", "").Trim();
                            string[] parts = cleanInput.Split(' ');

                            var feet = parts[0];
                            var inch = parts[1];

                            //Max 9' 11" 

                            newmeasurment.value = feet + "." + inch;
                        }
                        else if (measurementnamestring == "Sleep Duration")
                        {
                            //Add Sleep Duration 
                            // Add Sleep Duration
                            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
                            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 00;

                            string formattedHours = hours.ToString();

                            // Format time as decimal string
                            string timeString = $"{formattedHours}.{minutes:D2}";

                            // Assign to measurement value
                            newmeasurment.value = timeString;

                            if (SleepQualitySelect.SelectedItem != null)
                            {
                                newmeasurment.inputmethod = SleepQualitySelect.SelectedItem.ToString();
                            }

                            //Add Input Value 
                            inputvalue = "Hours/Minutes";
                        }
                        else
                        {

                            newmeasurment.value = unitentry.Text.ToString();
                        }
                        newmeasurment.unit = inputvalue;
                        newmeasurment.status = "Active";
                        var dt = adddatepicker.Date + addtimepicker.Time;
                        newmeasurment.inputdatetime = dt.ToString("dd/MM/yyyy HH:mm:ss");

                        //insert to db
                        var returnedmeasurement = await aPICalls.InsertUsermeasurement(newmeasurment);

                        //insert to local collection
                        newmeasurment.id = returnedmeasurement.id;
                        usermeasurementlistpassed.Add(newmeasurment);

                        // Find the item in the measurementlist based on a condition
                        var checkitem = measurementlist.FirstOrDefault(x => x.measurementid == newmeasurment.measurementid);

                        // Check if the item exists before attempting to remove it
                        if (checkitem != null)
                        {
                            // Remove the item directly from the measurementlist
                            measurementlist.Remove(checkitem);
                        }

                        var newsym = new feedbackdata();
                        newsym.id = newmeasurment.id;
                        newsym.value = newmeasurment.value;
                        newsym.datetime = newmeasurment.inputdatetime;
                        newsym.action = "update";
                        newsym.label = newmeasurment.measurementname;
                        newsym.unit = inputvalue;

                        if (userfeedbacklistpassed.measurementfeedbacklist == null)
                        {
                            userfeedbacklistpassed.measurementfeedbacklist = new ObservableCollection<feedbackdata>();
                        }

                        userfeedbacklistpassed.measurementfeedbacklist.Add(newsym);

                        string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.measurementfeedbacklist);
                        userfeedbacklistpassed.measurementfeedback = newsymJson;

                        await aPICalls.UserfeedbackUpdateMeasurementData(userfeedbacklistpassed);

                        await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Added") { });
                        await Task.Delay(1500);
                        await Navigation.PushAsync(new MeasurementsPage(usermeasurementlistpassed, measurementlist, userfeedbacklistpassed), false);

                        await MopupService.Instance.PopAllAsync(false);

                        var pages = Navigation.NavigationStack.ToList();
                        int i = 0;
                        foreach (var page in pages)
                        {
                            if (i == 0)
                            {
                            }
                            else if (i == 1 || i == 2 || i == 3)
                            {
                                Navigation.RemovePage(page);
                            }
                            else
                            {
                                //Navigation.RemovePage(page);
                            }
                            i++;
                        }
                    }
                }

                SubmitBtn.IsEnabled = true;
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

    private async void EditMeasurementData()
    {
        try
        {
            //add new usermeasurement 
            var newmeasurment = new usermeasurement();
            string userid = Preferences.Default.Get("userid", "Unknown");
            newmeasurment.userid = userid;
            newmeasurment.measurementid = usermeasurementpassed.id;
            newmeasurment.measurementname = usermeasurementpassed.measurementname;

            if (measurementnamestring == "Blood Pressure")
            {
                newmeasurment.value = sysentry.Text + "/" + diaentry.Text;
            }
            else if (measurementnamestring == "Weight" && inputvalue == "Stones/Pounds")
            {

                newmeasurment.value = Stonesentry.Text + "." + Poundsentry.Text;
                //newmeasurment.value = Stonesinput + "st " + Poundsinput + "lbs";

            }
            else if (measurementnamestring == "Height" && inputvalue == "Feet/Inches")
            {

                var height = unitentry.Text.ToString();
                string cleanInput = height.Replace("'", "").Replace("\"", "").Trim();
                string[] parts = cleanInput.Split(' ');

                var feet = parts[0];
                var inch = parts[1];

                //Max 9' 11" 

                newmeasurment.value = feet + "." + inch;
            }
            else if (measurementnamestring == "Sleep Duration")
            {
                //Add Sleep Duration 
                // Add Sleep Duration
                int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
                int minutes = int.TryParse(minsentry.Text, out int m) ? m : 00;

                string formattedHours = hours.ToString();

                // Format time as decimal string
                string timeString = $"{formattedHours}.{minutes:D2}";

                // Assign to measurement value
                newmeasurment.value = timeString;

                if (SleepQualitySelect.SelectedItem != null)
                {
                    newmeasurment.inputmethod = SleepQualitySelect.SelectedItem.ToString();
                }
                //Add Input Value 
                inputvalue = "Hours/Minutes";
            }
            else
            {

                newmeasurment.value = unitentry.Text.ToString();
            }

            newmeasurment.status = "Active";
            var dt = adddatepicker.Date + addtimepicker.Time;

            //Update the 3 possible changes 
            newmeasurment.inputdatetime = dt.ToString("dd/MM/yyyy HH:mm:ss");

            usermeasurementpassed.value = newmeasurment.value;
            usermeasurementpassed.inputmethod = newmeasurment.inputmethod;
            usermeasurementpassed.inputdatetime = newmeasurment.inputdatetime;



            //Update UserFeedback 

            APICalls database = new APICalls();
            //Set Deleted to True in UserMedication
            await database.UpdateSingleMeasurement(usermeasurementpassed);


            //Update UserFeedback
            var Feedbacktoupdate = userfeedbacklistpassed.measurementfeedbacklist.FirstOrDefault(x => x.id == usermeasurementpassed.id);

            if (Feedbacktoupdate != null)
            {
                Feedbacktoupdate.datetime = usermeasurementpassed.inputdatetime;
                Feedbacktoupdate.value = usermeasurementpassed.value; 
            }

            string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.measurementfeedbacklist);
            userfeedbacklistpassed.measurementfeedback = newsymJson;

            await database.UserfeedbackUpdateMeasurementData(userfeedbacklistpassed);

            await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Feedback Updated") { });

            await Task.Delay(1000);
            await Navigation.PushAsync(new MeasurementsPage());
            await MopupService.Instance.PopAllAsync(false);

            var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleMeasurement);
            var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is MeasurementsPage);
            var pageToRemovess = Navigation.NavigationStack.FirstOrDefault(x => x is ShowAllData);

            if (pageToRemoves != null)
            {
                Navigation.RemovePage(pageToRemoves);
            }
            if (pageToRemove != null)
            {
                Navigation.RemovePage(pageToRemove);
            }
            if (pageToRemovess != null)
            {
                Navigation.RemovePage(pageToRemovess);
            }

            //Removes Show All 
            Navigation.RemovePage(this);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void sysentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ChcekBloodPressure(sender, e, "Sys");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void diaentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ChcekBloodPressure(sender, e, "Diag");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
   }

    private void ChcekBloodPressure(object sender, TextChangedEventArgs e, string entryType)
    {
        try
        {
            string inputValue = e.NewTextValue;

            if (string.IsNullOrEmpty(inputValue))
            {
                validinput = false;
                SubmitBtn.BackgroundColor = Colors.Gray;
                SubmitBtn.TextColor = Colors.LightGray;
                return;
            }

            if (inputValue.Length > 3)
            {
                if (entryType == "Sys")
                {
                    sysentry.Text = inputValue.Substring(0, 3);
                    return; 
                }
                else
                {
                    diaentry.Text = inputValue.Substring(0, 3);
                    return;
                }
            }

            //Remove '.' Not Needed as there are two Entries
            if (inputValue.Contains("."))
            {
                inputValue = inputValue.Replace(".", "");
            }

            if (entryType == "Sys")
            {
                sysentry.Text = inputValue;
            }
            else
            {
                diaentry.Text = inputValue;
            }

            int inputInt = Convert.ToInt32(inputValue);

            if (entryType == "Sys")
            {
                if (inputInt < 40 || inputInt > 240)
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                    return;
                }

                if (string.IsNullOrEmpty(diaentry.Text))
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                }
                else
                {
                    int poundsValue = Convert.ToInt32(diaentry.Text);
                    if (poundsValue >= 20 && poundsValue <= 180)
                    {
                        validinput = true;
                        SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                        SubmitBtn.TextColor = Colors.White;
                    }
                    else
                    {
                        validinput = false;
                        SubmitBtn.BackgroundColor = Colors.Gray;
                        SubmitBtn.TextColor = Colors.LightGray;
                    }
                }
            }
            else if (entryType == "Diag")
            {
                if (inputInt < 20 || inputInt > 180)
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                    return;
                }

                if (string.IsNullOrEmpty(sysentry.Text))
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                }
                else
                {
                    int stonesValue = Convert.ToInt32(sysentry.Text);
                    if (stonesValue >= 40 && stonesValue <= 240)
                    {
                        validinput = true;
                        SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                        SubmitBtn.TextColor = Colors.White;
                    }
                    else
                    {
                        validinput = false;
                        SubmitBtn.BackgroundColor = Colors.Gray;
                        SubmitBtn.TextColor = Colors.LightGray;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void Stonesentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ChcekFeetInches(sender, e, "Stones");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void Poundsentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            ChcekFeetInches(sender, e, "Pounds");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void ChcekFeetInches(object sender, TextChangedEventArgs e, string entryType)
    {
        try
        {
            string inputValue = e.NewTextValue;

            //if (inputValue.Length == 1 && inputValue == ".")
            //{
            //    if(entryType == "Stones")
            //    {
            //        Stonesentry.Text = string.Empty; 
            //    }
            //    else
            //    {
            //        Poundsentry.Text = string.Empty; 
            //    }
            //    validinput = false;
            //    SubmitBtn.BackgroundColor = Colors.Gray;
            //    SubmitBtn.TextColor = Colors.LightGray;
            //    return;
            //}

            if (string.IsNullOrEmpty(inputValue))
            {
                validinput = false;
                SubmitBtn.BackgroundColor = Colors.Gray;
                SubmitBtn.TextColor = Colors.LightGray;
                return;
            }

            //Max length is 50 st 13lbs they can exceed that but input = invalid
            if (inputValue.Length > 2)
            {
                if (entryType == "Stones")
                {
                    Stonesentry.Text = inputValue.Substring(0,2);
                    return;
                }
                else
                {
                    Poundsentry.Text = inputValue.Substring(0,2);
                    return;
                }
            }

            //Remove '.' Not Needed as there are two Entries
            if (inputValue.Contains("."))
            {
                inputValue = inputValue.Replace(".", "");
            }

            if (entryType == "Stones")
            {
                Stonesentry.Text = inputValue;
            }
            else
            {
                Poundsentry.Text = inputValue;
            }

            int inputInt = Convert.ToInt32(inputValue);

            if (entryType == "Stones")
            {
                if (inputInt < 0 || inputInt > 50)
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                    return;
                }

                if (string.IsNullOrEmpty(Poundsentry.Text))
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                }
                else
                {
                    int poundsValue = Convert.ToInt32(Poundsentry.Text);
                    if (poundsValue >= 0 && poundsValue <= 13)
                    {
                        validinput = true;
                        SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                        SubmitBtn.TextColor = Colors.White;
                    }
                    else
                    {
                        validinput = false;
                        SubmitBtn.BackgroundColor = Colors.Gray;
                        SubmitBtn.TextColor = Colors.LightGray;
                    }
                }
            }
            else if (entryType == "Pounds")
            {
                if (inputInt < 0 || inputInt > 13)
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                    return;
                }

                if (string.IsNullOrEmpty(Stonesentry.Text))
                {
                    validinput = false;
                    SubmitBtn.BackgroundColor = Colors.Gray;
                    SubmitBtn.TextColor = Colors.LightGray;
                }
                else
                {
                    int stonesValue = Convert.ToInt32(Stonesentry.Text);
                    if (stonesValue >= 0 && stonesValue <= 50)
                    {
                        validinput = true;
                        SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                        SubmitBtn.TextColor = Colors.White;
                    }
                    else
                    {
                        validinput = false;
                        SubmitBtn.BackgroundColor = Colors.Gray;
                        SubmitBtn.TextColor = Colors.LightGray;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void hoursentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //hours entry text changed
            var entry = sender as Entry;
            HourInput = 0;
            if (string.IsNullOrWhiteSpace(hoursentry.Text) || hoursentry.Text == "")
            {
                SubmitBtn.IsEnabled = false;
                validinput = false;
                SubmitBtn.BackgroundColor = Colors.Gray;
                SubmitBtn.TextColor = Colors.LightGray;
                return;
            }
            /*if (entry == null) */ 

            // Get the new text value
            var newText = e.NewTextValue;
            HourInput = Int32.Parse(newText);
            validinput = true;
            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            SubmitBtn.IsEnabled = true; 
            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void minsentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var entry = sender as Entry;
            //Check Hours/Mins Input Not Empty

            if (string.IsNullOrWhiteSpace(minsentry.Text) || minsentry.Text == "" )
            {
                SubmitBtn.IsEnabled = false;
                validinput = false;
                SubmitBtn.BackgroundColor = Colors.Gray;
                SubmitBtn.TextColor = Colors.LightGray;
                return;
            }

            MinInput = 0; 
            //if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;
            validinput = true;
            MinInput = Int32.Parse(newText); 
            SubmitBtn.IsEnabled = true;
            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }

                // Validate the value is within the range (0-59)
                if (int.TryParse(newText, out int minutes))
                {
                    if (minutes > 59)
                    {
                        newText = "00"; // Set to max value
                    }
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void Fiffteenminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            SubmitBtn.IsEnabled = true;
            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 15 minutes
            minutes += 15;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void Thirtyminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            SubmitBtn.IsEnabled = true;
            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 15 minutes
            minutes += 30;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void Sixtyminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            SubmitBtn.IsEnabled = true;
            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 60 minutes (equivalent to adding 1 hour)
            hours += 1;

            // Handle hour overflow (optional, wrap to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0; // Reset to 0 if over 23 (for 24-hour format)
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void Ninetyminsbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            SubmitBtn.IsEnabled = true;
            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
            SubmitBtn.TextColor = Colors.White;
            // Parse the current hour and minute values
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 1 hour and 30 minutes
            minutes += 30;
            hours += 1;

            // Handle minute overflow
            if (minutes >= 60)
            {
                minutes -= 60; // Adjust minutes
                hours += 1;    // Increment hours for overflow
            }

            // Handle hour overflow (optional, wrap to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0; // Reset to 0 if over 23 (for 24-hour format)
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void SleepQualitySelect_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Remove item from [usermeasurement]

            var userresponse = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this Measurement Feedback? Once deleted it cannot be retrieved", "Cancel", "Delete");

            if (!userresponse)
            {

                usermeasurementpassed.deleted = true;

                APICalls database = new APICalls();
                //Set Deleted to True in UserMedication
                await database.DeleteSingleMeasurement(usermeasurementpassed);

                var remove = userfeedbacklistpassed.measurementfeedbacklist.FirstOrDefault(x => x.id == usermeasurementpassed.id);

                if (remove != null)
                {
                    remove.action = "deleted";
                }

                //Set Deleted to True in UserFeedback measurementfeedback
                string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.measurementfeedbacklist);
                userfeedbacklistpassed.measurementfeedback = newsymJson;


                await database.UserfeedbackUpdateMeasurementData(userfeedbacklistpassed);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Feedback Deleted") { });

                await Task.Delay(1000);
                await Navigation.PushAsync(new MeasurementsPage());
                await MopupService.Instance.PopAllAsync(false);

                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleMeasurement);
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is MeasurementsPage);
                var pageToRemovess = Navigation.NavigationStack.FirstOrDefault(x => x is ShowAllData);

                if (pageToRemoves != null)
                {
                    Navigation.RemovePage(pageToRemoves);
                }
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                if (pageToRemovess != null)
                {
                    Navigation.RemovePage(pageToRemovess);
                }

                //Removes Show All 
                Navigation.RemovePage(this);

            }
            else
            {
                return; 
            }
        }

        catch (Exception ex)
        {

        }
    }
}