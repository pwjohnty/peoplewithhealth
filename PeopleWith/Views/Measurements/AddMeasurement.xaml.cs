using System.Collections.ObjectModel;
using Mopups.Services;
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

    public AddMeasurement()
	{
		InitializeComponent();
	}

    public AddMeasurement(measurement measurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed)
    {
        InitializeComponent();

        measurementpassed = measurementp;
        usermeasurementlistpassed = usermeasurementsp;
        measurementlist = measurementlistpassed;

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
        else
        {
           


            unitstringlist = measurementpassed.units.Split(',').ToList();

            unitlist.ItemsSource = unitstringlist;

           
        }

  

        adddatepicker.Date = DateTime.Now;
        adddatepicker.MaximumDate = DateTime.Now;
        addtimepicker.Time = DateTime.Now.TimeOfDay;



    }



    public AddMeasurement(usermeasurement usermeasurementp, ObservableCollection<usermeasurement> usermeasurementsp, ObservableCollection<measurement> measurementlistpassed)
    {
        InitializeComponent();

        usermeasurementpassed = usermeasurementp;
        usermeasurementlistpassed = usermeasurementsp;
        measurementlist = measurementlistpassed;

        measurementname.Text = "Add " + usermeasurementpassed.measurementname;
        measurementnamestring = usermeasurementpassed.measurementname;
        measurementid = usermeasurementpassed.measurementid;

        if(measurementnamestring == "Blood Pressure")
        {
            unitentryframe.IsVisible = false;
            bpsysframe.IsVisible = true;
            bpdiaframe.IsVisible = true;
        }
        
        if(measurementnamestring == "Weight" && usermeasurementpassed.unit == "Stones/Pounds")
        {
            unitentryframe.IsVisible = false;
            StonesPoundsframe.IsVisible = true;
            stlbl.Text = "St";
            lbslbl.Text = "lbs";
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

    private void unitlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;
            inputvalue = item;

            if(inputvalue == "Stones/Pounds")
            {
                unitentryframe.IsVisible = false;
                StonesPoundsframe.IsVisible = true;
                unitentry.IsEnabled = false; 
                stlbl.Text = "St";
                lbslbl.Text = "lbs"; 
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

        }
        catch(Exception ex)
        {

        }
    }



    private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            dtPicker.IsOpen = true;
        }
        catch (Exception ex)
        {

        }
    }

    private void unitentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (e.NewTextValue.Length > 8)
            {
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

                            }
                            else if (convertodec <= 2.9 && convertodec >= 361.9)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 2 && convertoint >= 362)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec <= 4.9 && convertodec >= 799.1)
                            {
                                validinput = false;

                            }
                            else
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec <= 4.9 && convertodec >= 799.1)
                            {
                                validinput = false;

                            }
                            else
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec <= 0.4 && convertodec >= 56.9)
                            {
                                validinput = false;

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
                                if (convertoint < 1 && convertoint >= 57)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec <= 29.9 && convertodec >= 299.1)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 29)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 300)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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
                                    validinput = true;

                                }
                                else
                                {
                                    validinput = false;

                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec <= 5.9 && convertodec >= 89.9)
                            {
                                validinput = false;

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
                                if (convertoint <= 6)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 90)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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
                            }
                            else if (valuestring.Length == 2)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var secondnum = valuestring[1].ToString(); // Inches
                                validinput = true;
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                            }
                            else if (valuestring.Length == 3)
                            {
                                var firstnum = valuestring[0].ToString(); // Feet
                                var inchString = valuestring.Substring(1, 2); //Inches
                                var inches = Convert.ToInt32(inchString);

                                if (inches < 12)
                                {
                                    validinput = true;
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                }
                                else
                                {
                                    validinput = false;
                                }
                            }
                            else
                            {
                                validinput = false;
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

                            }
                            else if (convertodec <= 9.9 && convertodec >= 299.9)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 9)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 299)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec <= 4 && convertodec >= 116.9)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 4)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 117)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec >= 100.1)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 100)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec >= 100.1)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 100)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 350)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 170)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 240)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 160)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 500)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 400)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 60)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                                }
                                else if (convertoint > 200)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;
                                // }

                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;
                                // }

                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                        }
                        else if (convertoint > 14.1)
                        {
                            validinput = false;

                        }
                        else
                        {
                            numvalueconverted = convertoint.ToString();
                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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

                        }
                        else
                        {
                            if (convertoint < 31)
                            {
                                validinput = false;

                            }
                            else if (convertoint > 131)
                            {
                                validinput = false;

                            }
                            else
                            {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 4)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            }
                            else if (convertodec >= 100.1)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint < 0)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 100)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                    validinput = true;


                                }
                                else
                                {
                                    validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 3 && e.NewTextValue.Length <= 5)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {

                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;


                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                    }
                }
                else if (measurementnamestring == "Fever/Temperature ")
                {

                    if (inputvalue.Contains("C"))
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;

                            }
                            else if (convertodec <= 29.9 && convertodec >= 42.1)
                            {
                                validinput = false;

                            }
                            else
                            {

                                Double dc = Math.Round((Double)convertodec, 1);
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
                                if (convertoint <= 32)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 43)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

                            }
                        }

                        // var num = Convert.ToInt32(e.NewTextValue);




                    }
                    else if (inputvalue.Contains("F"))
                    {


                        unitentry.Text = e.NewTextValue;

                        if (e.NewTextValue.Contains("."))
                        {
                            var countdots = e.NewTextValue.ToCharArray().Count(x => x == '.');
                            var convertodec = Convert.ToDouble(e.NewTextValue);
                            if (countdots > 1)
                            {
                                validinput = false;

                            }
                            else if (convertodec <= 91.9 && convertodec >= 107.1)
                            {
                                validinput = false;

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

                            if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 3)
                            {
                                if (convertoint <= 91)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 108)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                                if (convertoint < 7)
                                {
                                    validinput = false;

                                }
                                else if (convertoint > 75)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }


                            }
                            else
                            {
                                validinput = false;

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

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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

                            }
                            else if (convertoint > 1200)
                            {
                                validinput = false;

                            }
                            else
                            {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;
                            }

                        }
                        else
                        {
                            validinput = false;

                        }
                    }
                }
                else if (measurementnamestring == "Respiration Rate")
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

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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

                            }
                            else if (convertoint > 100)
                            {
                                validinput = false;

                            }
                            else
                            {
                                numvalueconverted = convertoint.ToString();
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;
                            }

                        }
                        else
                        {
                            validinput = false;

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

                                }
                                else if (convertoint > 80)
                                {
                                    validinput = false;

                                }
                                else
                                {
                                    numvalueconverted = convertoint.ToString();
                                    SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                    validinput = true;
                                }

                            }
                            else
                            {
                                validinput = false;

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

                        }
                        else
                        {

                            Double dc = Math.Round((Double)convertodec, 1);
                            numvalueconverted = dc.ToString();

                            SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

                        }
                    }
                    else
                    {

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 2161)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

                        }
                    }

                }
                else if (measurementnamestring == "Glomerular Filtration Rate (GFR)")
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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

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
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

                        }
                    }
                    else
                    {

                        if (e.NewTextValue.Length >= 1 && e.NewTextValue.Length <= 2)
                        {
                            if (convertoint > 0 && convertoint < 2161)
                            {
                                SubmitBtn.BackgroundColor = Color.FromArgb("#031926");
                                validinput = true;
                                numvalueconverted = convertoint.ToString();
                            }
                            else
                            {
                                validinput = false;

                            }
                        }
                        else
                        {
                            validinput = false;

                        }
                    }

                }

            }
        }
        catch(Exception ex)
        {

        }
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if(validinput == false)
            {
                Vibration.Vibrate();
                return;
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
                    if(string.IsNullOrEmpty(Stonesinput) || string.IsNullOrEmpty(Poundsinput))
                    {
                        Vibration.Vibrate();
                        return;
                    }

                    if(Int32.Parse(Stonesinput) > 100)
                    {
                        Vibration.Vibrate();
                        return;
                    }
                    if(Int32.Parse(Poundsinput) > 13)
                    {
                        Vibration.Vibrate();
                        return;
                    }
                    else
                    {
                        newmeasurment.value = Stonesinput + "st " + Poundsinput + "lbs";
                    }

                }
                else
                {

                    newmeasurment.value = unitentry.Text.ToString();
                }
                newmeasurment.unit = inputvalue;
                newmeasurment.status = "Active";
                var dt = adddatepicker.Date + addtimepicker.Time;
                newmeasurment.inputdatetime = dt.ToString();

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


                await MopupService.Instance.PushAsync(new PopupPageHelper("Measurement Added") { });
                await Task.Delay(1500);
                await Navigation.PushAsync(new MeasurementsPage(usermeasurementlistpassed, measurementlist), false);

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
        catch(Exception ex )
        {

        }
    }

    private void sysentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (e.NewTextValue.Length > 3)
            {
                return;
            }
            if (measurementnamestring == "Blood Pressure")
            {
                inputvalue = "Systolic/Diastolic";
             
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
                        if (convertoint < 40)
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
                            //bloodpresuresysnum = convertoint.ToString();
                            if (string.IsNullOrEmpty(diaentry.Text))
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;
                            }
                            else
                            {
                                var convertdia = Convert.ToInt32(diaentry.Text);
                                if (convertdia >= 40 && convertdia <= 140)
                                {
                                    validinput = true;
                                    SubmitBtn.BackgroundColor = Color.FromHex("#0F9FE2");
                                    SubmitBtn.TextColor = Colors.White;
                                }
                            }
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
        catch
        {
            validinput = false;
            SubmitBtn.BackgroundColor = Colors.Gray;
            SubmitBtn.TextColor = Colors.LightGray;
        }
    }

    private void diaentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (e.NewTextValue.Length > 3)
            {
                return;
            }
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                validinput = false;
                SubmitBtn.BackgroundColor = Colors.Gray;
                SubmitBtn.TextColor = Colors.LightGray;
              
            }
            else
            {
                if (measurementnamestring == "Blood Pressure")
                {
                    inputvalue = "Systolic/Diastolic";
                   
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
                            if (convertoint < 40)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;
                            }
                            else if (convertoint > 140)
                            {
                                validinput = false;
                                SubmitBtn.BackgroundColor = Colors.Gray;
                                SubmitBtn.TextColor = Colors.LightGray;
                            }
                            else
                            {
                               // bloodpresuredianum = convertoint.ToString();
                                if (string.IsNullOrEmpty(sysentry.Text))
                                {
                                    validinput = false;
                                    SubmitBtn.BackgroundColor = Colors.Gray;
                                    SubmitBtn.TextColor = Colors.LightGray;
                                }
                                else
                                {
                                    var convertdia = Convert.ToInt32(sysentry.Text);
                                    if (convertdia >= 40 && convertdia <= 240)
                                    {
                                        validinput = true;
                                        SubmitBtn.BackgroundColor = Color.FromHex("#0F9FE2");
                                        SubmitBtn.TextColor = Colors.White;
                                    }
                                }
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
        catch (Exception ex)
        {
          
            SubmitBtn.BackgroundColor = Colors.Gray;
            SubmitBtn.TextColor = Colors.LightGray;
            //hideallstack.IsVisible = false;
            //errorstack.IsVisible = true;
         
        }
    }

    async private void Stonesentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            Stonesinput = e.NewTextValue;
            SubmitBtn.BackgroundColor = Color.FromHex("#0F9FE2");
            validinput = true; 
        }
        catch (Exception Ex)
        {

        }
    }

    async private void Poundsentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            Poundsinput = e.NewTextValue;
            SubmitBtn.BackgroundColor = Color.FromHex("#0F9FE2");
            validinput = true;
        }
        catch (Exception Ex)
        {

        }
    }
}