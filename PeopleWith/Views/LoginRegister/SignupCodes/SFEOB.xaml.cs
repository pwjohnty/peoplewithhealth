using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.VisualBasic;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;

namespace PeopleWith;
public partial class SFEOB : ContentPage
{
    user newuser;
    signupcode signupcodeinfo;
    bool isNavigating = false; 
    List<string> MaritalOptiosn = new List<string>();  
    ObservableCollection<symptom> allsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<medication> allmedicationlist = new ObservableCollection<medication>();
    public consent additonalconsent = new consent();
    APICalls database = new APICalls();
    bool ResetCheck = false;
    bool isEditing;
    private bool isResetting = false;
    private List<medication> previousSelection = new();
    ObservableCollection<postcode> Allpostcodes = new ObservableCollection<postcode>();
    ObservableCollection<diagnosis> alldiagnosislist = new ObservableCollection<diagnosis>();
    ObservableCollection<medication> filteredmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<medication> additionalfilteredmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<medication> medicationchipselectedlist = new ObservableCollection<medication>();
    ObservableCollection<userresponse> userresponselist = new ObservableCollection<userresponse>();
    ObservableCollection<question> AllQuestions = new ObservableCollection<question>();
    ObservableCollection<answer> AllAnswers = new ObservableCollection<answer>();
    ObservableCollection<answer> GetAnswers = new ObservableCollection<answer>();
    ObservableCollection<answer> GetCommPref = new ObservableCollection<answer>();
    ObservableCollection<usermedication> medicationstoadd = new ObservableCollection<usermedication>();
    ObservableCollection<usermeasurement> addusermeasurements = new ObservableCollection<usermeasurement>();
    ObservableCollection<userdiagnosis> adduserdiagnosis = new ObservableCollection<userdiagnosis>();
    ObservableCollection<usersymptom> symptomstoadd = new ObservableCollection<usersymptom>();
    List<string> commprefaddedlist = new List<string>();

    //Medication Lazy loader Data
    private int MedpageSize = 50;
    private int MedcurrentPage = 0;
    private bool MedisLoading = false;
    private bool MEdhasMoreItems = true;
    ObservableCollection<medication> SearchMedicationsList = new ObservableCollection<medication>();
    ObservableCollection<medication> FilterMedicationsList = new ObservableCollection<medication>();

    //Questions 
    question maritalstatusquestion;
    question occupationquestion;
    question ctquestion;
    question commprefquestion;


    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Login"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public SFEOB()
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


    public SFEOB(user userp, ObservableCollection<symptom> symtpomsp, ObservableCollection<medication> medicationsp, signupcode signupcodeinfop, double progressp, ObservableCollection<question> requestions, ObservableCollection<answer> reganswers, consent addtionalcon)
    {
        try
        {
            InitializeComponent();

            topprogress.SetProgress(progressp + 6, 0);
        
            newuser = userp;
            allsymptomlist = symtpomsp;
            allmedicationlist = medicationsp;
            signupcodeinfo = signupcodeinfop;
            additonalconsent = addtionalcon;
            AllQuestions = requestions;
            AllAnswers = reganswers;

            if (!string.IsNullOrEmpty(signupcodeinfo.externalidentifier))
            {
                extidlbl.Text = signupcodeinfo.externalidentifier;
            }

            LoadAllQuestionsData(); 
            getpostcodes();
            getdiagnosis();
            GetMedications();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

  
    async void UpdateProgress(string Direction)
    {
        try
        {           
            if (Direction == "Forward")
            {
                topprogress.Progress = topprogress.Progress += 2.5;
            }
            else
            {
                topprogress.Progress = topprogress.Progress -= 2.5;
            }
            
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void LoadAllQuestionsData()
    {
        try
        {
            //marital Status
            maritalstatusquestion = AllQuestions.Where(x => x.title.Contains("Marital Status")).SingleOrDefault();

            if (maritalstatusquestion != null)
            {
                maritialstatustitle.Text = maritalstatusquestion.title;
                maritialstatusdetails.Text = maritalstatusquestion.directions;

                GetAnswers = AllAnswers.Where(x => x.questionid == maritalstatusquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                maritallist.ItemsSource = GetAnswersorder;
            }

            //occupation
            occupationquestion = AllQuestions.Where(x => x.title.Contains("Current Occupation")).SingleOrDefault();

            if (occupationquestion != null)
            {
                occupationtitle.Text = occupationquestion.title;
                occupationdetails.Text = occupationquestion.directions;

                GetAnswers = AllAnswers.Where(x => x.questionid == occupationquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                occupationlist.ItemsSource = GetAnswersorder;
            }

            //Clinicaltrials
            ctquestion = AllQuestions.Where(x => x.title.Contains("Clinical Trials")).SingleOrDefault();

            if (ctquestion != null)
            {

                ctnamequestion.Text = ctquestion.title;
                ctquestiondes.Text = ctquestion.directions;

                GetAnswers = AllAnswers.Where(x => x.questionid == ctquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                ctlist.ItemsSource = GetAnswersorder;
            }

            commprefquestion = AllQuestions.Where(x => x.title.Contains("Communication Preferences")).SingleOrDefault();

            if (commprefquestion != null)
            {

                commtitlequestion.Text = commprefquestion.title;
                commprefquestiondes.Text = commprefquestion.directions;

                var getanswers = AllAnswers.Where(x => x.questionid == commprefquestion.questionid).ToList();

                GetCommPref = getanswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                compreflist.ItemsSource = GetCommPref;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async void getpostcodes()
    {
        try
        {
            Allpostcodes = await database.GetAsyncPostcode();

            //Preset HW Data
            heightslider.Value = 100;
            weightslider.Value = 70;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getdiagnosis()
    {
        try
        {
            ObservableCollection<diagnosis> diagnosisCollection = new ObservableCollection<diagnosis>();

            if (signupcodeinfo.diagnosis.Contains(","))
            {
                var splititem = signupcodeinfo.diagnosis.Split(',').ToList();

                foreach (var item in splititem)
                {
                    var nd = new diagnosis();
                    nd.Diagnosisid = item;
                    diagnosisCollection.Add(nd);
                }

                foreach (var item in diagnosisCollection)
                {

                    var dia = await database.GetAsyncSingleDiagnosis(item);
                    alldiagnosislist.Add(dia);
                }

                conditionlist.ItemsSource = alldiagnosislist;
                conditionlist.HeightRequest = alldiagnosislist.Count * 110;
            }
            else
            {
                if (!string.IsNullOrEmpty(signupcodeinfo.diagnosis))
                {

                    var nd = new diagnosis();
                    nd.Diagnosisid = signupcodeinfo.diagnosis;
                    diagnosisCollection.Add(nd);

                    foreach (var item in diagnosisCollection)
                    {
                        var dia = await database.GetAsyncSingleDiagnosis(item);
                        alldiagnosislist.Add(dia);
                    }

                    conditionlist.ItemsSource = alldiagnosislist;
                    conditionlist.HeightRequest = alldiagnosislist.Count * 110;

                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void GetMedications()
    {
        try
        {
            var splitmedlist = signupcodeinfo.medications.Split(',').ToList();

            foreach (var item in splitmedlist)
            {
                var med = allmedicationlist.Where(x => x.medicationid == item).SingleOrDefault();

                if (med != null)
                {
                    filteredmedicationlist.Add(med);
                }
            }

            medicationchips.ItemsSource = filteredmedicationlist;
            additionlmedlist.ItemsSource = allmedicationlist;
            additionalmedicationchips.ItemsSource = additionalfilteredmedicationlist;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }     
    }

    async void HandleMartialStatus()
    {
        try
        {
            if (maritallist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }
            else
            {

                //ScrolltoTop to fix swithcing page issue 
                Fixscrollview();

                var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == maritalstatusquestion.questionid);

                if (itemToRemove != null)
                {
                    userresponselist.Remove(itemToRemove);
                }

                var item = maritallist.SelectedItem as answer;

                var response = new userresponse();
                response.questionid = maritalstatusquestion.questionid;
                response.answerid = item.answerid;
                response.userid = newuser.userid;
                response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                userresponselist.Add(response);

                //Change Page 
                maritalstatusframe.IsVisible = false;
                occupationframe.IsVisible = true;
                skipbtn.IsVisible = false;
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void HandleOccupation()
    {
        try
        {
            if (occupationlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }


            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == occupationquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = occupationlist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = occupationquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);

            //Change Page 
            occupationframe.IsVisible = false;
            postcodeframe.IsVisible = true;
            skipbtn.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Handlepostcode()
    {
        try
        {

            if (!Allpostcodes.Any(p => p.postcodebrick == postcodetext.Text.TrimEnd()))
            {
                Vibration.Vibrate();
                return;
            }

            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            newuser.postcode = postcodetext.Text.TrimEnd();

            postcodeframe.IsVisible = false;
            hwbframe.IsVisible = true;
            skipbtn.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    async void HandleHWB()
    {
        try
        {
            //bool CheckBoth = string.IsNullOrEmpty(diaentry.Text) || string.IsNullOrEmpty(sysentry.Text);
            if (string.IsNullOrEmpty(heightlbl.Text) && string.IsNullOrEmpty(weightlbl.Text))
            {
                Vibration.Vibrate();
                return;
            }

            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            //range for blood Pressure 
            //var Systolicint = int.Parse(sysentry.Text);
            //var diastolicint = int.Parse(diaentry.Text);
            //if (Systolicint > 220 || Systolicint < 10)
            //{
            //    Vibration.Vibrate();
            //    return;
            //}

            //if (diastolicint > 150 || diastolicint < 10)
            //{
            //    Vibration.Vibrate();
            //    return;
            //}

            addusermeasurements.Clear();
            //add height, weight as measurment and calulate bmi and add it as a measurement

            int wholeNumberValue = (int)Math.Round(weightslider.Value);
            int wholeNumberValueheight = (int)Math.Round(heightslider.Value);

            double weight = wholeNumberValue; // User input for weight in kg
            double height = wholeNumberValueheight; // User input for height in cm

            double heightMeters = height / 100; // Convert cm to meters
            double bmi = weight / (heightMeters * heightMeters);
            var bmitotal = Math.Round(bmi, 2);

            //add height
            var umheight = new usermeasurement();
            umheight.measurementid = "AF1907F7-ECB3-43F1-A53D-5BF88D2D31F7";
            umheight.measurementname = "Height";
            umheight.value = height.ToString();
            umheight.unit = "cm";
            umheight.userid = newuser.userid;
            umheight.inputdatetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            addusermeasurements.Add(umheight);


            //add weight
            var umweight = new usermeasurement();
            umweight.measurementid = "08404437-A3AC-4887-BEBC-01D72CBFF17D";
            umweight.measurementname = "Weight";
            umweight.value = weight.ToString();
            umweight.unit = "Kg";
            umweight.userid = newuser.userid;
            umweight.inputdatetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            addusermeasurements.Add(umweight);


            //add bmi
            var umbmi = new usermeasurement();
            umbmi.measurementid = "D1913657-D2FD-4174-9E30-CD20B57BE9A6";
            umbmi.measurementname = "BMI";
            umbmi.value = bmitotal.ToString();
            umbmi.unit = "BMI";
            umbmi.userid = newuser.userid;
            umbmi.inputdatetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            addusermeasurements.Add(umbmi);

            //add blood Pressure
            //var umbp = new usermeasurement();
            //umbp.measurementid = "F13744F2-AB32-4AE5-9830-6BB0DEE6DF80";
            //umbp.measurementname = "Blood Pressure";
            //umbp.value = sysentry.Text + "/" + diaentry.Text;
            //umbp.unit = "Systolic/Diastolic";
            //umbp.userid = newuser.userid;
            //umbp.inputdatetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            //addusermeasurements.Add(umbp);

            hwbframe.IsVisible = false;
            conditionframe.IsVisible = true;
            skipbtn.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void HandleCF()
    {
        try 
        {
            if (conditionlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            var newuserdiag = new userdiagnosis();
             
            //Add Obesity Diagnosis 
            newuserdiag.diagnosistitle = "Obesity";
            newuserdiag.diagnosisid = "75A15B19-6E0D-4CF3-AB40-7186387B68F5";
            newuserdiag.userid = newuser.userid;

            //Ensure its not already in it
            if (!adduserdiagnosis.Contains(newuserdiag))
            {
                adduserdiagnosis.Add(newuserdiag);
            }

            //8B0E4BE0-0362-44C1-9DA3-7E9012B53C24 (Type 1 Diabetes)
            //76953F05-BF71-4384-BB8B-51F0C7B35529 (Type 2 Diabetes)

            bool SetpageCheck = adduserdiagnosis.Any(x => x.diagnosisid.Contains("8B0E4BE0-0362-44C1-9DA3-7E9012B53C24") || x.diagnosisid.Contains("76953F05-BF71-4384-BB8B-51F0C7B35529"));
            if (SetpageCheck)
            {
                DiabeticTypeTitle.Text = DiabeticTypeTitle.Text = adduserdiagnosis.FirstOrDefault(x => x.diagnosisid == "8B0E4BE0-0362-44C1-9DA3-7E9012B53C24"
                || x.diagnosisid == "76953F05-BF71-4384-BB8B-51F0C7B35529")?.diagnosistitle;
                conditionframe.IsVisible = false;
                DiabeticSelected.IsVisible = true;
                skipbtn.IsVisible = false;
            }
            else
            {
                conditionframe.IsVisible = false;
                medicationsframe.IsVisible = true;
                skipbtn.IsVisible = true;
            }           
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }

    async void HandleAddCon()
    {
        try
        {
            if (diagdatecheckbox.IsChecked == true)
            {
                //Can be null
            }
            else
            {
                if (String.IsNullOrEmpty(DiabetiesDate.Text))
                {
                    Vibration.Vibrate();
                    DiabetiesDate.Focus();
                    return; 
                }
                if(DiabetiesDate.TextColor == Colors.Red)
                {
                    Vibration.Vibrate();
                    DiabetiesDate.Focus();
                    return;
                }
                        
            }

            foreach (var item in adduserdiagnosis)
            {
                if(item.diagnosisid == "8B0E4BE0-0362-44C1-9DA3-7E9012B53C24" || item.diagnosisid == "76953F05-BF71-4384-BB8B-51F0C7B35529")
                {
                    if (diagdatecheckbox.IsChecked != true)
                    {
                        item.dateofdiagnosis = DiabetiesDate.Text;
                    }
                    else
                    {
                        item.dateofdiagnosis = null;
                    }
                }
            }

            DiabeticSelected.IsVisible = false;
            medicationsframe.IsVisible = true;
            skipbtn.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void HandleMedication()
    {
        try
        {
            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            medicationstoadd.Clear();
            foreach (var item in medicationchipselectedlist)
            {

                var newitem = new usermedication();
                newitem.status = "Pending";
                newitem.medicationid = item.medicationid;
                newitem.medicationtitle = item.title;
                newitem.userid = newuser.userid;

                medicationstoadd.Add(newitem);
            }

            medicationsframe.IsVisible = false;
            comprefframe.IsVisible = true;
            skipbtn.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void HandleComP()
    {
        try
        {
            if (compreflist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == commprefquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            //add the question
            var response = new userresponse();
            response.questionid = commprefquestion.questionid;
            var getallanswers = string.Join(",", commprefaddedlist);
            response.answerid = getallanswers;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response.userid = newuser.userid;
            userresponselist.Add(response);

            comprefframe.IsVisible = false;
            ctframe.IsVisible = true;
            skipbtn.IsVisible = false;
        }
        catch (Exception Ex)
        {

        }
    }

    async void HandleCT()
    {
        try
        {
            if (ctlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var item = ctlist.SelectedItem as answer;

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == ctquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            //add the question
            var response = new userresponse();
            response.questionid = ctquestion.questionid;
            response.answerid = item.answerid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response.userid = newuser.userid;
            userresponselist.Add(response);

            if (newuser.signupcodeid.Contains("SFECORE"))
            {
                newuser.registrycondition = "Weight Management";
            }
            else if (newuser.signupcodeid.Contains("???"))
            {
                newuser.registrycondition = "";
            }
          
            await Navigation.PushAsync(new RegisterFinalPage(newuser, topprogress.Progress, userresponselist, additonalconsent, symptomstoadd, medicationstoadd, adduserdiagnosis, addusermeasurements, signupcodeinfo), false);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Fixscrollview()
    {
        try
        {
            if (MainScrollView.ScrollY > 0)
            {
                await MainScrollView.ScrollToAsync(0, 0, true);
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void postcodelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var item = e.CurrentSelection.FirstOrDefault() as postcode;

            if (item != null)
            {
                var NewItem = new ObservableCollection<postcode>();
                NewItem.Add(item);
                postcodetext.Text = item.postcodebrick;
                postcodelist.ItemsSource = NewItem;
                postcodelist.HeightRequest = 65;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void postcodetext_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            PostCodeLoading.IsVisible = true;
            postcodelist.IsVisible = false;
            NoResultslblPost.IsVisible = false;

            //Check if itemsource count is greater than 1 then remove selceted item

            if (postcodelist.ItemsSource is ObservableCollection<postcode> items && items.Count == 1)
            {
                postcodelist.SelectedItem = null;
            }

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                postcodelist.IsVisible = false;
                PostCodeLoading.IsVisible = false;
                NoResultslblPost.IsVisible = false;
            }
            else
            {
                var countofcharacters = e.NewTextValue.Length;

                if (countofcharacters > 1)
                {
                    PostCodeLoading.IsVisible = false;
                    NoResultslblPost.IsVisible = false;
                    var Characters = e.NewTextValue;

                    var filteredmeds = new ObservableCollection<postcode>(Allpostcodes
                        .Where(s => s.postcodebrick.Contains(Characters, StringComparison.OrdinalIgnoreCase))
                        .OrderBy(m => m.postcodebrick));

                    postcodelist.ItemsSource = filteredmeds;
                    PostCodeLoading.IsVisible = false;
                    postcodelist.IsVisible = false;

                    if (filteredmeds.Count() == 0)
                    {
                        NoResultslblPost.IsVisible = true;
                    }
                    else
                    {
                        postcodelist.IsVisible = true;
                        NoResultslblPost.IsVisible = false;

                        double GetHeight = Convert.ToDouble(filteredmeds.Count() / 4.0);
                        if (GetHeight.ToString().Contains("."))
                        {
                            var GetInt = GetHeight.ToString().Split('.');
                            int SetInt = Int32.Parse(GetInt[0]);
                            if (SetInt == 0)
                            {
                                postcodelist.HeightRequest = 65;
                            }
                            else
                            {
                                postcodelist.HeightRequest = (SetInt * 65) + 65;
                            }

                        }
                        else
                        {
                            postcodelist.HeightRequest = (GetHeight * 65) + 65;
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

    private void heightslider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        try
        {
            double sliderValue = heightslider.Value;
            int wholeNumberValue = (int)Math.Round(sliderValue);
            heightlbl.Text = $"{wholeNumberValue} cm";
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private void weightslider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        try
        {
            double sliderValue = weightslider.Value;
            int wholeNumberValue = (int)Math.Round(sliderValue);
            weightlbl.Text = $"{wholeNumberValue} kg";
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private async void Skip_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (conditionframe.IsVisible == true)
            {
                adduserdiagnosis.Clear();

                var newuserdiag = new userdiagnosis();


                newuserdiag.diagnosistitle = "Obesity";
                newuserdiag.diagnosisid = "75A15B19-6E0D-4CF3-AB40-7186387B68F5";
                newuserdiag.userid = newuser.userid;

                //Ensure its not already in it
                if (!adduserdiagnosis.Contains(newuserdiag))
                {
                    adduserdiagnosis.Add(newuserdiag);
                }


                conditionframe.IsVisible = false;
                medicationsframe.IsVisible = true;
                skipbtn.IsVisible = true;

                UpdateProgress("Forward");

            }
            else if (medicationsframe.IsVisible == true)
            {
                medicationstoadd.Clear();

                medicationsframe.IsVisible = false;
                comprefframe.IsVisible = true;
                skipbtn.IsVisible = false;
                UpdateProgress("Forward");
            }
            else if (comprefframe.IsVisible == true)
            {

                var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == commprefquestion.questionid);

                if (itemToRemove != null)
                {
                    userresponselist.Remove(itemToRemove);
                }

                comprefframe.IsVisible = false;
                ctframe.IsVisible = true;
                skipbtn.IsVisible = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Next_Clicked(object sender, EventArgs e)
    {
        try
        {   
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                nextbtn.IsEnabled = false;
                skipbtn.IsEnabled = false;

                if (maritalstatusframe.IsVisible == true)
                {
                    HandleMartialStatus();
                }
                else if (occupationframe.IsVisible == true)
                {
                    HandleOccupation();
                }
                else if (postcodeframe.IsVisible == true)
                {
                    Handlepostcode();
                }
                else if (hwbframe.IsVisible == true)
                {
                    HandleHWB();
                }
                else if (conditionframe.IsVisible == true)
                {
                    HandleCF(); 
                }
                else if(DiabeticSelected.IsVisible == true)
                {
                    HandleAddCon();
                }
                else if (medicationsframe.IsVisible == true)
                {
                    HandleMedication();
                }
                else if (ctframe.IsVisible == true)
                {
                    HandleCT();
                }
                else if (comprefframe.IsVisible == true)
                {
                    HandleComP();
                }

                nextbtn.IsEnabled = true;
                skipbtn.IsEnabled = true;
                UpdateProgress("Forward");
                
            }
            else
            {
                nextbtn.IsEnabled = true;
                skipbtn.IsEnabled = true;
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Back_Clicked(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps
                if (isNavigating == true) return;
                isNavigating = true;

                if (ctframe.IsVisible == true)
                {
                    ctframe.IsVisible = false;
                    comprefframe.IsVisible = true;
                    skipbtn.IsVisible = true;
                }
                else if (comprefframe.IsVisible == true)
                {
                    comprefframe.IsVisible = false;
                    medicationsframe.IsVisible = true;
                    skipbtn.IsVisible = true;
                }
                else if (medicationsframe.IsVisible == true)
                {
                    //8B0E4BE0-0362-44C1-9DA3-7E9012B53C24 (Type 1 Diabetes)
                    //76953F05-BF71-4384-BB8B-51F0C7B35529 (Type 2 Diabetes)

                    bool SetpageCheck = adduserdiagnosis.Any(x => x.diagnosisid.Contains("8B0E4BE0-0362-44C1-9DA3-7E9012B53C24") || x.diagnosisid.Contains("76953F05-BF71-4384-BB8B-51F0C7B35529"));
                    if (SetpageCheck)
                    {
                        medicationsframe.IsVisible = false;
                        DiabeticSelected.IsVisible = true;
                        skipbtn.IsVisible = false;
                    }
                    else
                    {
                        medicationsframe.IsVisible = false;
                        conditionframe.IsVisible = true;
                        skipbtn.IsVisible = true;
                    }
                }
                else if (DiabeticSelected.IsVisible == true)
                {
                    DiabeticSelected.IsVisible = false;
                    conditionframe.IsVisible = true;
                    skipbtn.IsVisible = true;
                }
                else if (conditionframe.IsVisible == true)
                {
                    conditionframe.IsVisible = false;
                    hwbframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (hwbframe.IsVisible == true)
                {
                    hwbframe.IsVisible = false;
                    postcodeframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (postcodeframe.IsVisible == true)
                {
                    postcodeframe.IsVisible = false;
                    occupationframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (occupationframe.IsVisible == true)
                {
                    occupationframe.IsVisible = false;
                    maritalstatusframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (maritalstatusframe.IsVisible == true)
                {
                    MessagingCenter.Send<object>(this, "RemoveProgress");
                    Navigation.RemovePage(this);
                    return;
                }

                isNavigating = false; 
                UpdateProgress("Back");
            }
            else
            {
                isNavigating = false;
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void PrivacyPolicy_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                string BackArrow = "PeopleWith";
                await Navigation.PushAsync(new PrivacyPolicy(BackArrow), false);
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

    private void conditionlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            adduserdiagnosis.Clear();

            var item = e.DataItem as diagnosis;
            var newud = new userdiagnosis();

            newud.diagnosisid = item.Diagnosisid;
            newud.diagnosistitle = item.Title;
            newud.userid = newuser.userid;

            adduserdiagnosis.Add(newud);

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private void sysentry_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    try
    //    {
    //        var entry = sender as Entry;

    //        if (entry.Text.Contains("."))
    //        {
    //            // Remove the dot and reset the text
    //            entry.Text = entry.Text.Replace(".", "");
    //            // Move cursor to the end
    //            entry.CursorPosition = entry.Text.Length;
    //        }
    //        else if (entry.Text.Contains(","))
    //        {
    //            // Remove the dot and reset the text
    //            entry.Text = entry.Text.Replace(",", "");
    //            // Move cursor to the end
    //            entry.CursorPosition = entry.Text.Length;
    //        }
    //    }
    //    catch(Exception Ex) 
    //    { 
    //    }
    //}

    //private void diaentry_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    try
    //    {
    //        var entry = sender as Entry;

    //        if (entry.Text.Contains("."))
    //        {
    //            // Remove the dot and reset the text
    //            entry.Text = entry.Text.Replace(".", "");
    //            // Move cursor to the end
    //            entry.CursorPosition = entry.Text.Length;
    //        }
    //        else if (entry.Text.Contains(","))
    //        {
    //            // Remove the dot and reset the text
    //            entry.Text = entry.Text.Replace(",", "");
    //            // Move cursor to the end
    //            entry.CursorPosition = entry.Text.Length;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //    }
    //}

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
        }
    }

    private void medicationchips_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            if (e == null) return;

            // Addded Item Clicked
            if (e.AddedItem != null)
            {
                var Item = e.AddedItem as medication;

                if (!medicationchipselectedlist.Contains(Item))
                {
                    medicationchipselectedlist.Add(Item);
                }
            }

            // Remove Item Clicked
            if (e.RemovedItem != null)
            {
                var Item = e.RemovedItem as medication;

                if (medicationchipselectedlist.Contains(Item))
                {
                    medicationchipselectedlist.Remove(Item);
                }
            }
        }
        catch (Exception Ex)
        {
        }
    }

    private void additionlmedlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var currentSelection = e.CurrentSelection.Cast<medication>().ToList();

            var newlySelectedItems = currentSelection.Except(previousSelection).ToList();
            var deselectedItems = previousSelection.Except(currentSelection).ToList();

            previousSelection = currentSelection;

            searchmedentry.IsEnabled = false;
            searchmedentry.IsEnabled = true;

            foreach (var item in newlySelectedItems)
            {
                if (!additionalfilteredmedicationlist.Contains(item))
                {
                    additionalfilteredmedicationlist.Add(item);
                    medicationchipselectedlist.Add(item);
                }
            }

            foreach (var item in deselectedItems)
            {
                if (additionalfilteredmedicationlist.Contains(item))
                {
                    additionalfilteredmedicationlist.Remove(item);
                    medicationchipselectedlist.Remove(item);
                }
            }

            bool hasItems = additionalfilteredmedicationlist.Count > 0;
            addmedlbl1.IsVisible = hasItems;
            additionalmedicationchips.IsVisible = hasItems;
        }
        catch (Exception Ex)
        {
        }
    }
    private async void MedSearchList_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        try
        {
            if (MedisLoading || !MEdhasMoreItems) return;

            MedisLoading = true;

            await LoadNextPagetwoAsync();

            MedisLoading = false;
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadNextPagetwoAsync()
    {
        try
        {
            var startIndex = MedcurrentPage * MedpageSize;
            var nextItems = FilterMedicationsList
                .Skip(startIndex)
                .Take(MedpageSize)
                .ToList();

            foreach (var item in nextItems)
            {
                SearchMedicationsList.Add(item);
            }

            MedcurrentPage++;

            if (nextItems.Count < MedpageSize)
            {
                MEdhasMoreItems = false;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
        }
    }
    private async void searchmedentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var MedText = e.NewTextValue?.Trim();

            if (string.IsNullOrEmpty(MedText) || MedText.Length < 3)
            {
                additionlmedlist.IsVisible = false;
                NoResultslbl2.IsVisible = false;
                return;
            }

            // Filter the list based on the search
            FilterMedicationsList = new ObservableCollection<medication>(allmedicationlist.Where(s => s.title.Contains(MedText, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(m => m.title).ToObservableCollection();

            if (FilterMedicationsList.Count == 0)
            {
                additionlmedlist.IsVisible = false;
                NoResultslbl2.IsVisible = true;
                additionlmedlist.ItemsSource = null;
                return;
            }

            MedcurrentPage = 0;
            MEdhasMoreItems = true;
            SearchMedicationsList.Clear();

            await LoadNextPagetwoAsync();

            additionlmedlist.ItemsSource = SearchMedicationsList;
            additionlmedlist.IsVisible = true;
            NoResultslbl2.IsVisible = false;      
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    private void DiabetiesDate_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            if (isResetting)
                return;

            if (isEditing)
                return;

#if ANDROID
            var handler = DiabetiesDate.Handler as Microsoft.Maui.Handlers.EntryHandler;
            var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
            if (editText != null)
            {
                editText.EmojiCompatEnabled = false;
                editText.SetTextKeepState(DiabetiesDate.Text);
            }
#endif


            if (diagdatecheckbox.IsChecked == true)
            {
                if (ResetCheck)
                {
                    ResetCheck = false;
                    return;
                }

                diagdatecheckbox.IsChecked = false;
            }

            isEditing = true;

            string input = e.NewTextValue;

            // Remove any non-numeric characters except '/'
            input = new string(input.Where(c => char.IsDigit(c) || c == '/').ToArray());

            // Remove existing slashes to reformat correctly
            input = input.Replace("/", string.Empty);

            // Limit the input to a maximum of 8 numeric characters (DDMMYYYY)
            if (input.Length >= 8)
            {
                input = input.Substring(0, 8);
                DiabetiesDate.IsEnabled = true;
            }

            // Insert slashes at the appropriate positions
            if (input.Length > 2)
                input = input.Insert(2, "/");

            if (input.Length > 5)
                input = input.Insert(5, "/");

            // Check for valid date parts and set the text color accordingly
            if (input.Length == 10)
            {
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    // Check if the date is between 1900 and today's year
                    int currentYear = DateTime.Now.Year;
                    if (date.Year >= 1900 && date.Year <= currentYear)
                    {
                        if (date.Date <= DateTime.Now.Date)
                        {
                            DiabetiesDate.TextColor = Color.FromArgb("#031926"); // Valid date

                        }
                        else
                        {
                            DiabetiesDate.TextColor = Colors.Red; // Invalid date range
                            Vibration.Vibrate();
                            DiabetiesDate.Focus(); 
                        }

                    }
                    else
                    {
                        DiabetiesDate.TextColor = Colors.Red; // Invalid date range
                        Vibration.Vibrate();
                        DiabetiesDate.Focus();
                    }
                }
                else
                {
                    DiabetiesDate.TextColor = Colors.Red; // Invalid date
                    Vibration.Vibrate();
                    DiabetiesDate.Focus();
                }
            }
            else
            {
                DiabetiesDate.TextColor = Color.FromArgb("#031926"); // Intermediate input
            }

            DiabetiesDate.Text = input;

            // Adjust cursor position
            DiabetiesDate.CursorPosition = input.Length;
            isEditing = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void diagdatecheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(DiabetiesDate.Text))
            {
                isResetting = true;
                ResetCheck = true;

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    DiabetiesDate.Text = string.Empty;

                    await Task.Delay(100);
                    isResetting = false;
                });
            }

        }
        catch (Exception Ex)
        {

        }
    }
}