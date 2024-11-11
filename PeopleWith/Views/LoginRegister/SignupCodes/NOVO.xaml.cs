//using Android.Net.Wifi.Aware;
//using AuthenticationServices;
using PeopleWith;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class NOVO : ContentPage
{
    bool heightformatting;
    string heightinput;
    string weightinput;
    public user userpassed;
    public double progresspassed;
    public signupcode signupcodepassed;
    ObservableCollection<question> regquestionlist = new ObservableCollection<question>();
    ObservableCollection<answer> reganswerlist = new ObservableCollection<answer>();
    public usermeasurement heightmeasurement = new usermeasurement();
    public usermeasurement weightmeasurement = new usermeasurement();
    ObservableCollection<userresponse> userresponselist = new ObservableCollection<userresponse>();
    List<string> commprefaddedlist = new List<string>();
    ObservableCollection<usermeasurement> usermeasurementlist = new ObservableCollection<usermeasurement>();
    public consent additonalconsent = new consent();
    string CommandPassed;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

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

    public NOVO()
	{
        try
        {
            InitializeComponent();
            heightinput = "Ft";
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        } 
    }

    public NOVO(user userp, double progressp, signupcode signupcodep, ObservableCollection<question> requestions, ObservableCollection<answer> reganswers, consent addtionalcon)
    {
        try
        {
            InitializeComponent();

            heightinput = "Ft";

            userpassed = userp;
            progresspassed = progressp;

            topprogress.SetProgress(progresspassed, 0);
            signupcodepassed = signupcodep;

            regquestionlist = requestions;
            reganswerlist = reganswers;

            additonalconsent = addtionalcon;


            extidlbl.Text = signupcodepassed.externalidentifier;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SfLinearGauge_LabelCreated(object sender, Syncfusion.Maui.Gauges.LabelCreatedEventArgs e)
    {
        try
        {
            if (heightinput == "Ft")
            {

                if (e.Text == "0")
                    e.Text = "0'";
                else if (e.Text == "12")
                    e.Text = "1'";
                else if (e.Text == "24")
                    e.Text = "2'";
                else if (e.Text == "36")
                    e.Text = "3'";
                else if (e.Text == "48")
                    e.Text = "4'";
                else if (e.Text == "60")
                    e.Text = "5'";
                else if (e.Text == "72")
                    e.Text = "6'";
                else if (e.Text == "84")
                    e.Text = "7'";
                else if (e.Text == "96")
                    e.Text = "8'";
            }
            else
            {
                //do nothing for cm
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void LinearShapePointer_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
    {
        try
        {
            var value = e.Value;
            heighterrorlbl.IsVisible = false;

            if (heightinput == "Ft")
            {

                int feet = (int)(value / 12); // 1 foot = 12 inches
                int inches = (int)(value % 12); // Remaining inches

                var stringvalue = feet + "' " + inches + "''";

                heightinputlbl.Text = stringvalue;
            }
            else
            {
                int OtherInt = Convert.ToInt32(value);

                heightinputlbl.Text = OtherInt.ToString() + " cm";
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void weightgauge_LabelCreated(object sender, Syncfusion.Maui.Gauges.LabelCreatedEventArgs e)
    {
        try
        {
            if (weightinput == "Stone")
            {
                if (e.Text == "0")
                    e.Text = "0";
                else if (e.Text == "14")
                    e.Text = "1";
                else if (e.Text == "28")
                    e.Text = "2";
                else if (e.Text == "42")
                    e.Text = "3";
                else if (e.Text == "56")
                    e.Text = "4";
                else if (e.Text == "70")
                    e.Text = "5";
                else if (e.Text == "84")
                    e.Text = "6";
                else if (e.Text == "98")
                    e.Text = "7";
                else if (e.Text == "112")
                    e.Text = "8";
                else if (e.Text == "126")
                    e.Text = "9";
                else if (e.Text == "140")
                    e.Text = "10";
                else if (e.Text == "154")
                    e.Text = "11";
                else if (e.Text == "168")
                    e.Text = "12";
                else if (e.Text == "182")
                    e.Text = "13";
                else if (e.Text == "196")
                    e.Text = "14";
                else if (e.Text == "210")
                    e.Text = "15";
                else if (e.Text == "224")
                    e.Text = "16";
                else if (e.Text == "238")
                    e.Text = "17";
                else if (e.Text == "252")
                    e.Text = "18";
                else if (e.Text == "266")
                    e.Text = "19";
                else if (e.Text == "280")
                    e.Text = "20";
                else if (e.Text == "294")
                    e.Text = "21";
                else if (e.Text == "308")
                    e.Text = "22";
                else if (e.Text == "322")
                    e.Text = "23";
                else if (e.Text == "336")
                    e.Text = "24";
                else if (e.Text == "350")
                    e.Text = "25";
            }
        }
        catch (Exception ex)
        {
           //Leave Empty
        }
    }

    private void LinearShapePointer_ValueChanged_1(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
    {
        try
        {
            var value = e.Value;
            weighterrorlbl.IsVisible = false;

            if (weightinput == "Kg")
            {

                int OtherInt = Convert.ToInt32(value);

                weightinputlbl.Text = OtherInt.ToString() + " kg";
            }
            else
            {

                int stone = (int)(value / 14); // 1 foot = 12 inches
                int pounds = (int)(value % 14); // Remaining inches

                var stringvalue = stone + "st " + pounds + "lbs";

                weightinputlbl.Text = stringvalue;
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void SegmentedControl_ValueChanged(object sender, Plugin.Maui.SegmentedControl.ValueChangedEventArgs e)
    {
        try
        {
            var selectedvalue = e.NewValue;


            if (selectedvalue != null)
            {
                //     heightinputentry.Opacity = 1;
                //   heightinputentry.Text = string.Empty;
                //   heightinputlbl.Text = string.Empty;

                if (selectedvalue == 0)
                {
                    heightinput = "Ft";
                    heightgauge.Minimum = 0;
                    heightgauge.Maximum = 96;
                    heightgauge.Interval = 12;


                    if (!string.IsNullOrEmpty(heightinputlbl.Text))
                    {
                        if (heightinputlbl.Text.Contains("cm"))
                        {
                            //convert cm to ft
                            var getcm = heightinputlbl.Text.Split(' ');

                            var cm = Convert.ToDouble(getcm[0]);

                            double totalInches = cm / 2.54;
                            int feet = (int)(totalInches / 12);
                            double inches = totalInches % 12;



                            //update label
                            heightinputlbl.Text = feet.ToString() + "' " + inches.ToString() + "''";

                            //update guage
                            heightpointerguage.Value = totalInches;


                        }
                    }
                }
                else
                {
                    heightinput = "CM";
                    heightgauge.Minimum = 0;
                    heightgauge.Maximum = 250;
                    heightgauge.Interval = 25;


                    if (!string.IsNullOrEmpty(heightinputlbl.Text))
                    {
                        if (heightinputlbl.Text.Contains("''"))
                        {
                            //convert ft to cm
                            var ftin = heightinputlbl.Text.Split(' ');

                            var ftnum = new String(ftin[0].Where(Char.IsDigit).ToArray());
                            var innum = new String(ftin[1].Where(Char.IsDigit).ToArray());

                            var ft = Convert.ToDouble(ftnum);
                            var ins = Convert.ToDouble(innum);

                            double fttotal = (ft * 30.48);

                            double instotal = (ins * 2.54);

                            var total = fttotal + instotal;

                            //update label
                            heightinputlbl.Text = fttotal.ToString() + "' " + instotal.ToString() + "''";

                            //update guage
                            heightpointerguage.Value = total;


                        }
                    }

                }

            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void SegmentedControlweight_ValueChanged(object sender, Plugin.Maui.SegmentedControl.ValueChangedEventArgs e)
    {
        try
        {
            var selectedvalue = e.NewValue;




            if (selectedvalue != null)
            {
                // heightinputentry.Opacity = 1;
                // heightinputentry.Text = string.Empty;

                // weightinputlbl.Text = string.Empty;

                if (selectedvalue == 0)
                {
                    weightinput = "Kg";
                    weightgauge.Minimum = 0;
                    weightgauge.Maximum = 200;
                    weightgauge.Interval = 20;

                    if (!string.IsNullOrEmpty(weightinputlbl.Text))
                    {
                        if (weightinputlbl.Text.Contains("st"))
                        {
                            //convert stone to kg
                            var getstlbs = weightinputlbl.Text.Split(' ');

                            var stonenum = new String(getstlbs[0].Where(Char.IsDigit).ToArray());
                            var lbsnum = new String(getstlbs[1].Where(Char.IsDigit).ToArray());

                            var convertst = Convert.ToDouble(stonenum);
                            var convertlbs = Convert.ToDouble(lbsnum);

                            double totalPounds = (convertst * 14) + convertlbs;
                            var total = totalPounds * 0.453592;


                            //update label
                            weightinputlbl.Text = total.ToString() + "kg";

                            //update guage
                            weightguagepointer.Value = total;
                        }
                    }

                }
                else
                {
                    weightinput = "Stone";
                    weightgauge.Minimum = 0;
                    weightgauge.Maximum = 350;
                    weightgauge.Interval = 14;

                    if (!string.IsNullOrEmpty(weightinputlbl.Text))
                    {
                        if (weightinputlbl.Text.Contains("kg"))
                        {
                            //convert kg to stone
                            var getkg = weightinputlbl.Text.Split(' ');
                            var kg = Convert.ToDouble(getkg[0]);
                            //stones calulation
                            double totalPounds = kg * 2.20462;
                            int stones = (int)(totalPounds / 14);
                            double pounds = totalPounds % 14;

                            //update label
                            weightinputlbl.Text = stones.ToString() + "st " + pounds + "lbs";

                            //update guage
                            weightguagepointer.Value = totalPounds;
                        }
                    }

                    // weightgauge.MaximumLabelsCount = 0;
                }
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                nextbtn.IsEnabled = false;
                skipbtn.IsEnabled = false;
                var Command = (sender) as Button;
                CommandPassed = Command.CommandParameter.ToString();

                if (heightandweightframe.IsVisible == true)
                {
                    Handleheightandweightframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (countyframe.IsVisible == true)
                {
                    Handlecountyframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (useframe.IsVisible == true)
                {
                    Handleuseframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (comprefframe.IsVisible == true)
                {
                    Handlecommprefframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
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

    //private async void skipbtn_Clicked(object sender, EventArgs e)
    //{
    //    if(useframe.IsVisible == true)
    //    {
    //        useframe.IsVisible = false;
    //        comprefframe.IsVisible = true;

    //        UpdateProgress();
    //    }
    //    else if(comprefframe.IsVisible == true)
    //    {
    //        UpdateProgress();

    //        await Navigation.PushAsync(new RegisterFinalPage(userpassed, topprogress.Progress, userresponselist, usermeasurementlist, additonalconsent), false);

    //    }
    //}

    async void Handleheightandweightframe()
    {
        try
        {

            //check height

            if(string.IsNullOrEmpty(heightinputlbl.Text))
            {
                heighterrorlbl.IsVisible = true;
                Vibration.Vibrate();
                return;

            }

            if (string.IsNullOrEmpty(weightinputlbl.Text))
            {
                weighterrorlbl.IsVisible = true;
                Vibration.Vibrate();
                return;

            }

            //add the measurements

            heightmeasurement.measurementid = "AF1907F7-ECB3-43F1-A53D-5BF88D2D31F7";
            heightmeasurement.measurementname = "Height";
            heightmeasurement.status = "Active";
            heightmeasurement.userid = userpassed.userid;


            if (heightinput == "Ft")
            {
                var ftin = heightinputlbl.Text.Split(' ');

                var ftnum = new String(ftin[0].Where(Char.IsDigit).ToArray());
                var innum = new String(ftin[1].Where(Char.IsDigit).ToArray());
                heightmeasurement.unit = "Feet and Inches";
                heightmeasurement.value = ftnum + "." + innum;
            }
            else
            {
                var getcm = heightinputlbl.Text.Split(' ');
                heightmeasurement.unit = "cm";
                heightmeasurement.value = getcm[0];
            }

            usermeasurementlist.Add(heightmeasurement);

            weightmeasurement.measurementid = "08404437-A3AC-4887-BEBC-01D72CBFF17D";
            weightmeasurement.measurementname = "Weight";
            weightmeasurement.status = "Active";
            weightmeasurement.userid = userpassed.userid;

            if (weightinput == "Kg")
            {
                var getkg = weightinputlbl.Text.Split(' ');
                weightmeasurement.value = getkg[0];
                weightmeasurement.unit = "kg";
            }
            else
            {
                var getstlbs = weightinputlbl.Text.Split(' ');

                var stonenum = new String(getstlbs[0].Where(Char.IsDigit).ToArray());
                var lbsnum = new String(getstlbs[1].Where(Char.IsDigit).ToArray());
                weightmeasurement.value = stonenum + "." + lbsnum;
                weightmeasurement.unit = "Stones and Pounds";
            }

            usermeasurementlist.Add(weightmeasurement);



            if (signupcodepassed.signupcodeid == "SAXIRPHAR")
            {
                //populate use list
                var getquestion = regquestionlist.Where(x => x.title.Contains("Pharmacy County")).SingleOrDefault();

                if (getquestion != null)
                {

                    countytitlequestion.Text = getquestion.title;
                    countyquestiondes.Text = getquestion.directions;

                    var getanswers = reganswerlist.Where(x => x.questionid == getquestion.questionid).ToList();

                    countylist.ItemsSource = getanswers;

                    heightandweightframe.IsVisible = false;
                    countyframe.IsVisible = true;
                    skipbtn.IsVisible = false;

                    UpdateProgress();
                }
            }
            else
            {
                var getquestion = regquestionlist.Where(x => x.title.Contains("How will you use")).SingleOrDefault();

                if (getquestion != null)
                {

                    usenamequestion.Text = getquestion.title;
                    usequestiondes.Text = getquestion.directions;

                    var getanswers = reganswerlist.Where(x => x.questionid == getquestion.questionid).ToList();

                    uselist.ItemsSource = getanswers;

                    heightandweightframe.IsVisible = false;
                    useframe.IsVisible = true;
                    skipbtn.IsVisible = true;

                    UpdateProgress();
                }
            }


        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Handlecountyframe()
    {
        try
        {
            if(countylist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var item = countylist.SelectedItem as answer;

            //add the question
            var response = new userresponse();
            response.questionid = "1D2E9160-4033-4C62-8910-0AF155AC54ED";
            response.answerid = item.answerid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);


            //populate use list
            var getquestion = regquestionlist.Where(x => x.title.Contains("How will you use")).SingleOrDefault();

            if (getquestion != null)
            {

                usenamequestion.Text = getquestion.title;
                usequestiondes.Text = getquestion.directions;

                var getanswers = reganswerlist.Where(x => x.questionid == getquestion.questionid).ToList();

                uselist.ItemsSource = getanswers;

                countyframe.IsVisible = false;
                useframe.IsVisible = true;
                skipbtn.IsVisible = true;

                UpdateProgress();
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async void Handleuseframe()
    {
        try
        {           

            if(CommandPassed == "Next")
            {
                if (uselist.SelectedItem == null)
                {
                    Vibration.Vibrate();
                    return;
                }

                var item = uselist.SelectedItem as answer;

                //add the question
                var response = new userresponse();
                response.questionid = "21B3A187-63CE-4E9C-BA38-B6391743B6D5";
                response.answerid = item.answerid;
                response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                userresponselist.Add(response);
            }

           

            //populate use list
            var getquestion = regquestionlist.Where(x => x.title.Contains("Communication Preferences")).SingleOrDefault();

            if (getquestion != null)
            {

                commtitlequestion.Text = getquestion.title;
                commprefquestiondes.Text = getquestion.directions;

                var getanswers = reganswerlist.Where(x => x.questionid == getquestion.questionid).ToList();

                compreflist.ItemsSource = getanswers;

                useframe.IsVisible = false;
                comprefframe.IsVisible = true;
                skipbtn.IsVisible = true;

                UpdateProgress();
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Handlecommprefframe()
    {
        try
        {
           

            if (CommandPassed == "Next")
            {
                if (compreflist.SelectedItem == null)
                {
                    Vibration.Vibrate();
                    return;
                }

                //add the question
                var response = new userresponse();
                response.questionid = "3C8FF935-9294-4DE6-A4E6-127ECE5D1A36";
                var getallanswers = string.Join(",", commprefaddedlist);
                response.answerid = getallanswers;
                response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                userresponselist.Add(response);
            }


            UpdateProgress();

            await Navigation.PushAsync(new RegisterFinalPage(userpassed, topprogress.Progress, userresponselist , usermeasurementlist, additonalconsent), false);


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async void UpdateProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress += 5;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void BackProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress -= 5;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void compreflist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as answer;

            if (commprefaddedlist.Contains(item.answerid))
            {
                commprefaddedlist.Remove(item.answerid);
            }
            else
            {
                commprefaddedlist.Add(item.answerid);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {

            //back button clicked
            if(comprefframe.IsVisible == true)
            {
                comprefframe.IsVisible = false;
                useframe.IsVisible = true;

                BackProgress();
            }
            else if(useframe.IsVisible  == true)
            {
                useframe.IsVisible = false;

                if(signupcodepassed.signupcodeid == "SAXIRPHAR")
                {
                    countyframe.IsVisible = true;
                }
                else
                {
                    heightandweightframe.IsVisible = true;

                }

                BackProgress();
            }
            else if(countyframe.IsVisible == true)
            {
                countyframe.IsVisible = false;
                heightandweightframe.IsVisible = true;

                BackProgress();
            }
            else if(heightandweightframe.IsVisible == true)
            {

                //remove the progress from previous page

                MessagingCenter.Send<object>(this, "RemoveProgress");
                Navigation.RemovePage(this);
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            string BackArrow = "PeopleWith";
            await Navigation.PushAsync(new PrivacyPolicy(BackArrow), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}