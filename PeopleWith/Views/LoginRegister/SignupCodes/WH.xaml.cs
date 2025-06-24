using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using Microsoft.VisualBasic;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;

namespace PeopleWith;

public partial class WH : ContentPage
{
    public List<string> primaryconditionlist = new List<string>();
    bool isEditing;
    bool validdob;
    bool isEditingsurgery;
    bool validdobsurgery;
    ObservableCollection<symptom> allsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<symptom> filteredsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<symptom> additionalfilteredsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<medication> allmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<diagnosis> alldiagnosislist = new ObservableCollection<diagnosis>();
    public userdiagnosis userdiag = new userdiagnosis();
    string surgery;

    ObservableCollection<symptom> symptomchipselectedlist = new ObservableCollection<symptom>();


    ObservableCollection<usersymptom> symptomstoadd = new ObservableCollection<usersymptom>();
    ObservableCollection<usersymptom> symptomstoCheck = new ObservableCollection<usersymptom>();

    ObservableCollection<medication> filteredmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<medication> additionalfilteredmedicationlist = new ObservableCollection<medication>();

    ObservableCollection<medication> medicationchipselectedlist = new ObservableCollection<medication>();
    ObservableCollection<usermedication> medicationstoadd = new ObservableCollection<usermedication>();
    ObservableCollection<usermedication> medicationstoCheck = new ObservableCollection<usermedication>();
    ObservableCollection<userresponse> userresponselist = new ObservableCollection<userresponse>();

    ObservableCollection<userdiagnosis> adduserdiagnosis = new ObservableCollection<userdiagnosis>();

    question ctquestion;
    question commprefquestion;
    question occupationquestion;
    question genderatbirthquestion;
    question dietquestion;
    question alcoholquestion;
    question stressquestion;
    question exercisequestion;
    question exercisequestion2;
    question exercisequestion3;
    question exercisequestion4;
    question exercisequestion5;
    question smokingquestion;
    question smokingquestionyes;
    question smokingquestionno;
    question hotflushesquestion;
    answer answersmokingyes;
    answer answersmokingno;
    answer answerexercise4;
    answer answerexercise5;

    string CommandPassed;
    ObservableCollection<answer> GetAnswers = new ObservableCollection<answer>();
    ObservableCollection<answer> GetCommPref = new ObservableCollection<answer>();

    private List<symptom> previousSelection = new();
    private List<medication> Selectionprevious = new();

    ObservableCollection<usermeasurement> addusermeasurements = new ObservableCollection<usermeasurement>();
    userfeedback UserFeedbackToAdd = new userfeedback();
    userdiet DietToAdd = new userdiet(); 

    List<string> commprefaddedlist = new List<string>();

    public consent additonalconsent = new consent();

    user newuser;
    signupcode signupcodeinfo;

    //Symptoms list lazy loader
    private int SympageSize = 50; 
    private int SymcurrentPage = 0;
    private bool SymisLoading = false;
    private bool SymhasMoreItems = true;
    ObservableCollection<symptom> SearchSymptomsList = new ObservableCollection<symptom>();
    ObservableCollection<symptom> FilterSymptomsList = new ObservableCollection<symptom>();

    //Medications list lazy loader
    private int MedpageSize = 50;
    private int MedcurrentPage = 0;
    private bool MedisLoading = false;
    private bool MEdhasMoreItems = true;
    ObservableCollection<medication> SearchMedicationsList = new ObservableCollection<medication>();
    ObservableCollection<medication> FilterMedicationsList = new ObservableCollection<medication>();

    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    APICalls database = new APICalls();
    ObservableCollection<postcode> Allpostcodes = new ObservableCollection<postcode>();
    public WH()
	{
		InitializeComponent();
	}

    public WH(user userp, ObservableCollection<symptom> symtpomsp, ObservableCollection<medication> medicationsp, signupcode signupcodeinfop, double progressp, ObservableCollection<question> requestions, ObservableCollection<answer> reganswers, consent addtionalcon)
    {
        try
        {

            InitializeComponent();

            topprogress.SetProgress(progressp + 6, 0);

            //additionalsymptomchiplistnrat.SelectionChanged += additionalsymptomchiplistnrat_SelectionChanged;
          //  primaryconditionlist.Add("Adrenal Cortical Cancer (ACC)");
          //  primaryconditionlist.Add("Phaeo Para Syndromes (Paraganglioma or Phaeochromocytoma (PPGL))");

          //  pclist.ItemsSource = primaryconditionlist;

            newuser = userp;
            allsymptomlist = symtpomsp;
            allmedicationlist = medicationsp;
            signupcodeinfo = signupcodeinfop;

            additonalconsent = addtionalcon;

            if (!string.IsNullOrEmpty(signupcodeinfo.externalidentifier))
            {
                extidlbl.Text = signupcodeinfo.externalidentifier;
            }

            var splitlist = signupcodeinfo.symptoms.Split(',').ToList();

            foreach (var item in splitlist)
            {
                var symptom = allsymptomlist.Where(x => x.symptomid == item).SingleOrDefault();

                if (symptom != null)
                {
                    filteredsymptomlist.Add(symptom);
                }


            }

            symptomchiplistnrat.ItemsSource = filteredsymptomlist;
            additionlsymlist.ItemsSource = allsymptomlist;

            additionalsymptomchiplistnrat.ItemsSource = additionalfilteredsymptomlist;


            var splitmedlist = signupcodeinfo.medications.Split(',').ToList();

            foreach (var item in splitmedlist)
            {
                var med = allmedicationlist.Where(x => x.medicationid == item).SingleOrDefault();

                if (med != null)
                {
                    filteredmedicationlist.Add(med);
                }
            }

            medicationchiplistnrat.ItemsSource = filteredmedicationlist;
            additionlmedlist.ItemsSource = allmedicationlist;
            additionalmedicationchiplistnrat.ItemsSource = additionalfilteredmedicationlist;

            //occupation
            occupationquestion = requestions.Where(x => x.title.Contains("Current Occupation")).SingleOrDefault();

            if (occupationquestion != null)
            {

                occupationtitle.Text = occupationquestion.title;
                occupationdetails.Text = occupationquestion.directions;

                GetAnswers = reganswers.Where(x => x.questionid == occupationquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                occupationlist.ItemsSource = GetAnswersorder;
            }


            //get lifestyle questions and answers

            var lsquestions = requestions.Where(x => x.category == "Lifestyle");

            if (lsquestions != null)
            {
                genderatbirthquestion = lsquestions.Where(x => x.title.Contains("Gender assigned at birth")).SingleOrDefault();
                
                if (genderatbirthquestion != null) 
                {
                    genderatbirthlbl.Text = genderatbirthquestion.title;
                    genderatbirthlblinfo.Text = genderatbirthquestion.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == genderatbirthquestion.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    genderatbirthlist.ItemsSource = GetAnswersorder;
                }

                dietquestion = lsquestions.Where(x => x.title.Contains("Diet")).SingleOrDefault();

                if (dietquestion != null)
                {
                    dietlbl.Text = dietquestion.title;
                    dietinfo.Text = dietquestion.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == dietquestion.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    dietlist.ItemsSource = GetAnswersorder;
                }

                alcoholquestion = lsquestions.Where(x => x.title.Contains("Alcohol")).SingleOrDefault();

                if (alcoholquestion != null)
                {
                    alcohollbl.Text = alcoholquestion.title;
                    alcoholinfo.Text = alcoholquestion.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == alcoholquestion.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    alcohollist.ItemsSource = GetAnswersorder;
                }

                stressquestion = lsquestions.Where(x => x.title.Contains("Stress")).SingleOrDefault();

                if (stressquestion != null)
                {
                    stresslbl.Text = stressquestion.title;
                    stressinfo.Text = stressquestion.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == stressquestion.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    stresslist.ItemsSource = GetAnswersorder;
                }


                smokingquestion = lsquestions.Where(x => x.title.Contains("Smoking (including")).SingleOrDefault();

                if (smokingquestion != null)
                {
                    smokinglbl.Text = smokingquestion.title;
                    smokinginfo.Text = smokingquestion.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == smokingquestion.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    smokinglist.ItemsSource = GetAnswersorder;

                    smokingquestionyes = lsquestions.Where(x => x.title.Contains("If Yes, How many cigarettes")).SingleOrDefault();

                    if (smokingquestionyes != null)
                    {
                        answersmokingyes = reganswers.Where(x => x.questionid == smokingquestionyes.questionid).SingleOrDefault();
                    }

                    smokingquestionno = lsquestions.Where(x => x.title.Contains("If No, How many cigarettes")).SingleOrDefault();

                    if (smokingquestionno != null)
                    {
                        answersmokingno = reganswers.Where(x => x.questionid == smokingquestionno.questionid).SingleOrDefault();
                    }

                }

                exercisequestion = lsquestions.Where(x => x.title.Contains("Type of Exercise")).SingleOrDefault();

                if (exercisequestion != null)
                {
                    exerciselbl.Text = exercisequestion.title;
                    exerciseinfolbl.Text = exercisequestion.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == exercisequestion.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    exerciselist.ItemsSource = GetAnswersorder;
                }

                exercisequestion2 = lsquestions.Where(x => x.title.Contains("Aerobic Exercise")).SingleOrDefault();

                if (exercisequestion2 != null)
                {
                    exerciselbl2.Text = exercisequestion2.title;
                    exerciseinfolbl2.Text = exercisequestion2.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == exercisequestion2.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    exerciselist2.ItemsSource = GetAnswersorder;
                }

                exercisequestion3 = lsquestions.Where(x => x.title.Contains("Resistance Exercise")).SingleOrDefault();

                if (exercisequestion3 != null)
                {
                    exerciselbl3.Text = exercisequestion3.title;
                    exerciseinfolbl3.Text = exercisequestion3.directions;

                    GetAnswers = reganswers.Where(x => x.questionid == exercisequestion3.questionid).ToObservableCollection();

                    var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                    exerciselist3.ItemsSource = GetAnswersorder;
                }

                exercisequestion4 = lsquestions.Where(x => x.title.Contains("Exercise - Average duration")).SingleOrDefault();

                if (exercisequestion4 != null)
                {
                    exerciselbl4.Text = exercisequestion4.title;
                    exerciseinfolbl4.Text = exercisequestion4.directions;

                    answerexercise4 = reganswers.Where(x => x.questionid == exercisequestion4.questionid).SingleOrDefault();

                }

                exercisequestion5 = lsquestions.Where(x => x.title.Contains("Exercise - Average number")).SingleOrDefault();

                if (exercisequestion5 != null)
                {
                    exerciselbl5.Text = exercisequestion5.title;
                    exerciseinfolbl5.Text = exercisequestion5.directions;

                    answerexercise5 = reganswers.Where(x => x.questionid == exercisequestion5.questionid).SingleOrDefault();

                }


            }

            //get conditons 
       


            ctquestion = requestions.Where(x => x.title.Contains("Clinical Trials")).SingleOrDefault();

            if (ctquestion != null)
            {

                ctnamequestion.Text = ctquestion.title;
                ctquestiondes.Text = ctquestion.directions;

                GetAnswers = reganswers.Where(x => x.questionid == ctquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                ctlist.ItemsSource = GetAnswersorder;
            }

            commprefquestion = requestions.Where(x => x.title.Contains("Communication Preferences")).SingleOrDefault();

            if (commprefquestion != null)
            {

                commtitlequestion.Text = commprefquestion.title;
                commprefquestiondes.Text = commprefquestion.directions;

                var getanswers = reganswers.Where(x => x.questionid == commprefquestion.questionid).ToList();

                GetCommPref = getanswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                compreflist.ItemsSource = GetCommPref;
            }

            hotflushesquestion = lsquestions.Where(x => x.title.Contains("Hot Flushes")).SingleOrDefault();

            if (hotflushesquestion != null)
            {

                HotFlushquestion.Text = hotflushesquestion.title;
                HotFlushquestiondes.Text = hotflushesquestion.directions;

                var getanswers = reganswers.Where(x => x.questionid == hotflushesquestion.questionid).ToList();

                var GetAnswersorder = getanswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                hotFlushlist.ItemsSource = GetAnswersorder;
            }


            getpostcodes();
            getdiagnosis();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

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

    private void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //next button clicked
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                nextbtn.IsEnabled = false;
                skipbtn.IsEnabled = false;
                var Command = (sender) as Button;
                CommandPassed = Command.CommandParameter.ToString();

                if (occupationframe.IsVisible == true)
                {
                    Handleoccupationframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(postcodeframe.IsVisible == true)
                {
                    Handlepostcodeframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(hwframe.IsVisible == true)
                {
                    Handlehwframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(genderatbirthframe.IsVisible == true)
                {
                    Handlegenderframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (dietframe.IsVisible == true)
                {
                    Handledietframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (alcoholframe.IsVisible == true)
                {
                    Handlealcoholframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (stressframe.IsVisible == true)
                {
                    Handlestressframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(smokingframe.IsVisible == true)
                {
                    Handlesmokingframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(exerciseframe.IsVisible == true)
                {
                    Handleexerciseframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(conditionframe.IsVisible == true)
                {
                    HandleDiagframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(symptomsframe.IsVisible == true)
                {
                    Handlesymptomsframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (medicationsframe.IsVisible == true)
                {
                    Handlemedsframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (HotFlushesframe.IsVisible == true)
                {
                    HandleHFframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if (comprefframe.IsVisible == true)
                {
                    Handlecomprefframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled = true;
                }
                else if(ctframe.IsVisible == true)
                {
                    HandleCTframe();
                    nextbtn.IsEnabled = true;
                    skipbtn.IsEnabled= true;
                       
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



    private void skipbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            if(conditionframe.IsVisible == true)
            {
                adduserdiagnosis.Clear();

                var newuserdiag = new userdiagnosis();

                newuserdiag.diagnosistitle = "Menopause";
                newuserdiag.diagnosisid = "4C6FA56A-19DC-400F-8578-A0FAE3727070";
                newuserdiag.userid = newuser.userid;

              
                adduserdiagnosis.Add(newuserdiag);

                conditionframe.IsVisible = false;
                symptomsframe.IsVisible = true;

                UpdateProgress();

            }
            else if(symptomsframe.IsVisible == true)
            {
                symptomstoadd.Clear();

                symptomsframe.IsVisible = false;
                medicationsframe.IsVisible = true;

                UpdateProgress();


            }
            else if(medicationsframe.IsVisible == true)
            {
                medicationstoadd.Clear();

                medicationsframe.IsVisible = false;
                HotFlushesframe.IsVisible = true;
                skipbtn.IsVisible = false;
                UpdateProgress();
            }
            else if(comprefframe.IsVisible == true)
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
        catch(Exception ex)
        {

        }
    }

    async void getpostcodes()
    {
        try
        {


            Allpostcodes = await database.GetAsyncPostcode();




        }
        catch(Exception ex)
        {

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
                if(!string.IsNullOrEmpty(signupcodeinfo.diagnosis))
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
        catch (Exception ex)
        {
        }
    }

    async void Handleoccupationframe()
    {
        try
        {

            if (occupationlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }


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


            occupationframe.IsVisible = false;
            postcodeframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();


        }
        catch(Exception ex)
        {

        }
    }

    async void Handlepostcodeframe()
    {
        try
        {

            if (!Allpostcodes.Any(p => p.postcodebrick == postcodetext.Text.TrimEnd()))
            {
                Vibration.Vibrate();
                return;
            }

            newuser.postcode = postcodetext.Text.TrimEnd();

            postcodeframe.IsVisible = false;
            hwframe.IsVisible = true;
            heightslider.Value = 100;
            weightslider.Value = 70;
            skipbtn.IsVisible = false;

            UpdateProgress();

        }
        catch(Exception ex)
        {

        }
    }

    async void Handlehwframe()
    {
        try
        {

            if (string.IsNullOrEmpty(heightlbl.Text) && string.IsNullOrEmpty(weightlbl.Text))
            {
                Vibration.Vibrate();
                return;
            }

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
          
            hwframe.IsVisible = false;
            genderatbirthframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();

        }
        catch (Exception ex)
        {

        }
    }

    async void Handlegenderframe()
    {
        try
        {

            if (genderatbirthlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == genderatbirthquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = genderatbirthlist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = genderatbirthquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);


            genderatbirthframe.IsVisible = false;
            dietframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();


        }
        catch (Exception ex)
        {
        }
    }

    async void Handledietframe()
    {
        try
        {

            if (dietlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == dietquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = dietlist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = dietquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");


            if (adddietlbl.IsVisible)
            {
                //check if value is added

                if(string.IsNullOrEmpty(dietetext.Text))
                {
                    dietetext.Focus();
                    Vibration.Vibrate();
                    return;
                }

                response.notes = dietetext.Text;
            }

            userresponselist.Add(response);


            dietframe.IsVisible = false;
            alcoholframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();


        }
        catch (Exception ex)
        {
        }
    }

    async void Handlealcoholframe()
    {
        try
        {

            if (alcohollist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == alcoholquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = alcohollist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = alcoholquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);


            alcoholframe.IsVisible = false;
            stressframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();


        }
        catch (Exception ex)
        {
        }
    }

    async void Handlestressframe()
    {
        try
        {
            if (stresslist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == stressquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = stresslist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = stressquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

            if (addstresslbl.IsVisible)
            {
                //check if value is added

                if (string.IsNullOrEmpty(stresstext.Text))
                {
                    stresstext.Focus();
                    Vibration.Vibrate();
                    return;
                }

                response.notes = stresstext.Text;
            }

            userresponselist.Add(response);


            stressframe.IsVisible = false;
            smokingframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();
        }
        catch(Exception ex)
        {

        }
    }

    async void Handlesmokingframe()
    {
        try
        {

            if (smokinglist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            if (smokingfloat.IsVisible)
            {
                if (string.IsNullOrEmpty(smokingtext.Text))
                {
                    smokingtext.Focus();
                    Vibration.Vibrate();
                    return;
                }
            }


                var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == smokingquestion.questionid);

                if (itemToRemove != null)
                {
                    userresponselist.Remove(itemToRemove);
                }

                var itemToRemovee = userresponselist.FirstOrDefault(q => q.questionid == smokingquestionyes.questionid);

                if (itemToRemovee != null)
                {
                    userresponselist.Remove(itemToRemovee);
                }

                var itemToRemoveee = userresponselist.FirstOrDefault(q => q.questionid == smokingquestionno.questionid);

                if (itemToRemoveee != null)
                {
                    userresponselist.Remove(itemToRemoveee);
                }

                var item = smokinglist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = smokingquestion.questionid;
            response.answerid = item.answerid;
                response.userid = newuser.userid;
                response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);


            if(smokingfloat.IsVisible)
            {
                var responsee = new userresponse();
                if(item.label == "Yes")
                {
                    responsee.questionid = smokingquestionyes.questionid;
                    responsee.answerid = answersmokingyes.answerid;
                }
                else
                {
                    responsee.questionid = smokingquestionno.questionid;
                    responsee.answerid = answersmokingno.answerid;
                }

                    responsee.userid = newuser.userid;
                    responsee.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                userresponselist.Add(responsee);
            }

            smokingframe.IsVisible = false;
            exerciseframe.IsVisible = true;
            skipbtn.IsVisible = false;

            UpdateProgress();


        }
        catch(Exception ex)
        {

        }
    }

    async void Handleexerciseframe()
    {
        try
        {

            if (exerciselist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            if (addexlbl.IsVisible)
            {
                if (string.IsNullOrEmpty(extext.Text))
                {
                    extext.Focus();
                    Vibration.Vibrate();
                    return;
                }

            }

            if(exerciselist2.IsVisible == true)
            {
                if (exerciselist2.SelectedItem == null)
                {
                    Vibration.Vibrate();
                    return;
                }
            }

            if (exerciselist3.IsVisible == true)
            {
                if (exerciselist3.SelectedItem == null)
                {
                    Vibration.Vibrate();
                    return;
                }
            }

            //Also checks Value is between 1 and 1440 (1 day) 
            if(string.IsNullOrEmpty(exerciseonetext.Text) || Int32.Parse(exerciseonetext.Text) < 1 || Int32.Parse(exerciseonetext.Text) > 1440)
            {
                exerciseonetext.Focus();
                Vibration.Vibrate();
                return;
            }
            //Also checks Value is between 1 and 7 (Total No. of days in a week ) 
            if (string.IsNullOrEmpty(exercisetwotext.Text) || Int32.Parse(exercisetwotext.Text) < 1 || Int32.Parse(exercisetwotext.Text) > 7)
            {
                exercisetwotext.Focus();
                Vibration.Vibrate();
                return;
            }


            //all validation done start adding now

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == exercisequestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var itemToRemove1 = userresponselist.FirstOrDefault(q => q.questionid == exercisequestion2.questionid);

            if (itemToRemove1 != null)
            {
                userresponselist.Remove(itemToRemove1);
            }

            var itemToRemove2 = userresponselist.FirstOrDefault(q => q.questionid == exercisequestion2.questionid);

            if (itemToRemove2 != null)
            {
                userresponselist.Remove(itemToRemove2);
            }

            var itemToRemove3 = userresponselist.FirstOrDefault(q => q.questionid == exercisequestion3.questionid);

            if (itemToRemove3 != null)
            {
                userresponselist.Remove(itemToRemove3);
            }

            var itemToRemove4 = userresponselist.FirstOrDefault(q => q.questionid == exercisequestion4.questionid);

            if (itemToRemove4 != null)
            {
                userresponselist.Remove(itemToRemove4);
            }

            var itemToRemove5 = userresponselist.FirstOrDefault(q => q.questionid == exercisequestion5.questionid);

            if (itemToRemove5 != null)
            {
                userresponselist.Remove(itemToRemove5);
            }

            var item = exerciselist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = exercisequestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            if (addexlbl.IsVisible)
            {
                response.notes = extext.Text;
            }

            userresponselist.Add(response);


            if(exerciselist2.IsVisible == true)
            {

                var item2 = exerciselist2.SelectedItem as answer;

                var response2 = new userresponse();
                response2.questionid = exercisequestion2.questionid;
                response2.answerid = item.answerid;
                response2.userid = newuser.userid;
                response2.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
             

                userresponselist.Add(response2);

            }
            else if(exerciselist3.IsVisible == true)
            {
                var item3 = exerciselist3.SelectedItem as answer;

                var response3 = new userresponse();
                response3.questionid = exercisequestion3.questionid;
                response3.answerid = item.answerid;
                response3.userid = newuser.userid;
                response3.responsedate = DateTime.Now.ToString("dd/MM/yyyy");


                userresponselist.Add(response3);
            }


          

            var response4 = new userresponse();
            response4.questionid = exercisequestion4.questionid;
            response4.answerid = answerexercise4.answerid;
            response4.userid = newuser.userid;
            response4.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response4.notes = exerciseonetext.Text;

            userresponselist.Add(response4);


            var response5 = new userresponse();
            response5.questionid = exercisequestion5.questionid;
            response5.answerid = answerexercise5.answerid;
            response5.userid = newuser.userid;
            response5.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response5.notes = exercisetwotext.Text;

            userresponselist.Add(response5);


            exerciseframe.IsVisible = false;
          //  ctframe.IsVisible = true;
            if (!string.IsNullOrEmpty(signupcodeinfo.diagnosis))
            {

                conditionframe.IsVisible = true;
            }
            else
            {
                symptomsframe.IsVisible = true;
            }
            skipbtn.IsVisible = true;

            UpdateProgress();


        }
        catch (Exception ex)
        {

        }
    }

 

    async void HandleDiagframe()
    {
        try
        {

            if(conditionlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            //add the diagnosis menopause


            var newuserdiag = new userdiagnosis();

            newuserdiag.diagnosistitle = "Menopause";
            newuserdiag.diagnosisid = "4C6FA56A-19DC-400F-8578-A0FAE3727070";
            newuserdiag.userid = newuser.userid;

            if(!adduserdiagnosis.Contains(newuserdiag))
            {
                adduserdiagnosis.Add(newuserdiag);
            }

            

            conditionframe.IsVisible = false;
            symptomsframe.IsVisible = true;
            skipbtn.IsVisible = true;

        }
        catch(Exception ex)
        {

        }
    }

    async void Handlesymptomsframe()
    {
        try
        {
            //check if there are any symptoms added and add them into a collection

            symptomstoadd.Clear();
            foreach (var item in symptomchipselectedlist)
            {
                var neewitem = new usersymptom();
                neewitem.symptomid = item.symptomid;
                neewitem.symptomtitle = item.title;

                var sf = new symptomfeedback();
                sf.timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                Guid newUUID = Guid.NewGuid();
                sf.symptomfeedbackid = newUUID.ToString();
                sf.intensity = "50";
                sf.notes = null;
                sf.triggers = null;
                sf.interventions = null;
                sf.duration = null;
                sf.action = "add";

                var newcollection = new ObservableCollection<symptomfeedback>
                {
                    sf
                };

                neewitem.feedback = newcollection;

                neewitem.userid = newuser.userid;

                symptomstoadd.Add(neewitem);

            }


            symptomsframe.IsVisible = false;
            medicationsframe.IsVisible = true;
            skipbtn.IsVisible = true;

            UpdateProgress();

        }
        catch(Exception ex)
        {

        }
    }

    async void Handlemedsframe()
    {
        try
        {
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
            HotFlushesframe.IsVisible = true;
            skipbtn.IsVisible = false;
            UpdateProgress();


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Handlecomprefframe()
    {
        try
        {

            if (compreflist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

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

            UpdateProgress();

        }
        catch(Exception ex)
        {

        }
    }
    async void HandleCTframe()
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



            //   ctframe.IsVisible = false;
            //   comprefframe.IsVisible = true;
            //   skipbtn.IsVisible = false;

            // UpdateProgress();

            UpdateProgress();

            if(newuser.signupcodeid.Contains("A1"))
            {
                newuser.registrycondition = "WHA1";
            }
            else if (newuser.signupcodeid.Contains("A2"))
            {
                newuser.registrycondition = "WHA2";
            }
            else if (newuser.signupcodeid.Contains("A3"))
            {
                newuser.registrycondition = "WHA3";
            }
            else if (newuser.signupcodeid.Contains("A4"))
            {
                newuser.registrycondition = "WHA4";
            }

            await Navigation.PushAsync(new RegisterFinalPage(newuser, topprogress.Progress, userresponselist, additonalconsent, symptomstoadd, medicationstoadd, adduserdiagnosis, addusermeasurements, DietToAdd), false);


        }
        catch (Exception ex)
        {

        }
    }

    async void HandleHFframe()
    {
        try
        {
            if (hotFlushlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var item = hotFlushlist.SelectedItem as answer;

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == hotflushesquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            //add the question
            var response = new userresponse();
            response.questionid = hotflushesquestion.questionid;
            response.answerid = item.answerid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response.userid = newuser.userid;
            userresponselist.Add(response);

            HotFlushesframe.IsVisible = false;
            comprefframe.IsVisible = true;
            skipbtn.IsVisible = true;


            UpdateProgress();

        }
        catch (Exception ex)
        {

        }
    }


    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //priv policy tapped
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
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void UpdateProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress += 2.5;
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
            topprogress.Progress = topprogress.Progress -= 2.5;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void symptomchiplistnrat_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {

            if (e == null) return;

            // Addded Item Clicked
            if (e.AddedItem != null)
            {
                var Item = e.AddedItem as symptom;

                if (!symptomchipselectedlist.Contains(Item))
                {
                    symptomchipselectedlist.Add(Item);
                }
            }

            // Remove Item Clicked
            if (e.RemovedItem != null)
            {
                var Item = e.RemovedItem as symptom;

                if (symptomchipselectedlist.Contains(Item))
                {
                    symptomchipselectedlist.Remove(Item);
                }
            }
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    private void medicationchiplistnrat_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
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
            //Leave Empty
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

    //private void hotflusheslist_ItemTapped_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    //{
    //    try
    //    {
    //        var item = e.DataItem as answer;

    //        if (commprefaddedlist.Contains(item.answerid))
    //        {
    //            commprefaddedlist.Remove(item.answerid);
    //        }
    //        else
    //        {
    //            commprefaddedlist.Add(item.answerid);
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    //private void additionlsymlist_RemainingItemsThresholdReached_1(object sender, EventArgs e)
    //{

    //}

    private async void SympSearchList_RemainingItemsThresholdReached(object sender, EventArgs e)
    {
        try
        {
            if (SymisLoading || !SymhasMoreItems) return;

            SymisLoading = true;

            await LoadNextPageAsync();

            SymisLoading = false;
        }
        catch (Exception ex)
        {
        }
    }

    private async Task LoadNextPageAsync()
    {
        try
        {
            var startIndex = SymcurrentPage * SympageSize;
            var nextItems = FilterSymptomsList
                .Skip(startIndex)
                .Take(SympageSize)
                .ToList();

            foreach (var item in nextItems)
            {
                SearchSymptomsList.Add(item);
            }

            SymcurrentPage++;

            if (nextItems.Count < SympageSize)
            {
                SymhasMoreItems = false;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
        }    
    }

    private async void searchsymsentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var SymptomText = e.NewTextValue?.Trim();

            if (string.IsNullOrEmpty(SymptomText) || SymptomText.Length < 3)
            {
                additionlsymlist.IsVisible = false;
                NoResultslbl.IsVisible = false;
                return;
            }

            // Filter the list based on the search
            FilterSymptomsList = new ObservableCollection<symptom>(allsymptomlist.Where(s => s.title.Contains(SymptomText, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(m => m.title).ToObservableCollection();

            if(FilterSymptomsList.Count == 0)
            {
                additionlsymlist.IsVisible = false;
                NoResultslbl.IsVisible = true;
                additionlsymlist.ItemsSource = null;
                return;
            }

            SymcurrentPage = 0;
            SymhasMoreItems = true;
            SearchSymptomsList.Clear();

            await LoadNextPageAsync();

            additionlsymlist.ItemsSource = SearchSymptomsList;
            additionlsymlist.IsVisible = true;
            NoResultslbl.IsVisible = false;
      
        }
        catch (Exception ex)
        {
            //Leave Empty
        }
    }

    //private void additionlsymlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    //{
    //    try
    //    {
    //        //additional symptom list tapped

    //        var item = e.DataItem as symptom;

    //        searchsymsentry.IsEnabled = false;
    //        searchsymsentry.IsEnabled = true;

    //        // Convert the selected item to a ChipItem
    //        if (additionalfilteredsymptomlist.Contains(item))
    //        {
    //            additionalfilteredsymptomlist.Remove(item);
    //            symptomchipselectedlist.Remove(item);
    //        }
    //        else
    //        {

    //            additionalfilteredsymptomlist.Add(item);
    //            symptomchipselectedlist.Add(item);
    //        }

    //        if (additionalfilteredsymptomlist.Count == 0)
    //        {
    //            addlbl1.IsVisible = false;
    //            additionalsymptomchiplistnrat.IsVisible = false;
    //        }
    //        else
    //        {
    //            addlbl1.IsVisible = true;
    //            additionalsymptomchiplistnrat.IsVisible = true;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

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

    //private async void additionlmedlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    //{
    //    try
    //    {
    //        //additional symptom list tapped

    //        var item = e.DataItem as medication;

    //        searchmedentry.IsEnabled = false;
    //        searchmedentry.IsEnabled = true;

    //        // Convert the selected item to a ChipItem
    //        if (additionalfilteredmedicationlist.Contains(item))
    //        {
    //            additionalfilteredmedicationlist.Remove(item);
    //            medicationchipselectedlist.Remove(item);
    //        }
    //        else
    //        {

    //            additionalfilteredmedicationlist.Add(item);
    //            medicationchipselectedlist.Add(item);
    //        }

    //        if (additionalfilteredmedicationlist.Count == 0)
    //        {
    //            addmedlbl1.IsVisible = false;
    //            additionalmedicationchiplistnrat.IsVisible = false;
    //        }
    //        else
    //        {
    //            addmedlbl1.IsVisible = true;
    //            additionalmedicationchiplistnrat.ItemsSource = additionalfilteredmedicationlist;
    //            additionalmedicationchiplistnrat.IsVisible = true;
    //        }
    //        await Task.Delay(500);
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

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

            //if (postcodelist.ItemsSource?.Cast <ObservableCollection>().Count() == 1)
            //{
            //    if (postcodelist.SelectedItem != null)
            //    {
            //        postcodelist.SelectedItem = null;
            //    }
            //}


            if (string.IsNullOrEmpty(e.NewTextValue))
            {
               // FilterResults.Clear();
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
                  //  var filteredmeds = new ObservableCollection<postcode>(Allpostcodes.Where(s => s.postcodebrick.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.postcodebrick);

                                    var filteredmeds = new ObservableCollection<postcode>(
                    Allpostcodes
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
                    // allvideolist.HeightRequest = 70 * filteredmeds.Count();
                }

            }


        }
        catch(Exception ex)
        {

        }
    }

    private void postcodelist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as postcode;

            if (item != null)
            {
                var NewItem = new ObservableCollection<postcode>();
                NewItem.Add(item); 
                postcodetext.Text = item.postcodebrick;
                postcodelist.ItemsSource = NewItem; 
                postcodelist.HeightRequest = 65;
            }


        }
        catch(Exception ex)
        {

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
        catch (Exception ex)
        {
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
        catch (Exception ex)
        {
        }
    }

    private void smokinglist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as answer;


            if(item != null)
            {
                if(item.label == "Yes")
                {
                    smokingfloat.IsVisible = true;
                    smokingquestioninfo.Text = smokingquestionyes.title;

                }
                else if(item.label == "No")
                {
                    smokingfloat.IsVisible = true;
                    smokingquestioninfo.Text = smokingquestionno.title;
                }
                else
                {
                    smokingfloat.IsVisible = false;
                    smokingquestioninfo.Text = "";
                }
            }



        }
        catch(Exception ex)
        {

        }
    }

    private void exerciselist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as answer;

            if(item.label == "Aerobic")
            {
                exerciselbl2.IsVisible = true;
                exerciseinfolbl2.IsVisible = true;
                exerciselist2.IsVisible = true;

                exerciselbl3.IsVisible = false;
                exerciseinfolbl3.IsVisible = false;
                exerciselist3.IsVisible = false;

                addexlbl.IsVisible = false;
                exfloat.IsVisible = false;
                extext.Text = string.Empty;
            }
            else if (item.label == "Resistance")
            {
                exerciselbl2.IsVisible = false;
                exerciseinfolbl2.IsVisible = false;
                exerciselist2.IsVisible = false;

                exerciselbl3.IsVisible = true;
                exerciseinfolbl3.IsVisible = true;
                exerciselist3.IsVisible = true;

                addexlbl.IsVisible = false;
                exfloat.IsVisible = false;
                extext.Text = string.Empty;

            }
            else
            {
                addexlbl.IsVisible = true;
                exfloat.IsVisible = true;


                exerciselbl2.IsVisible = false;
                exerciseinfolbl2.IsVisible = false;
                exerciselist2.IsVisible = false;

                exerciselbl3.IsVisible = false;
                exerciseinfolbl3.IsVisible = false;
                exerciselist3.IsVisible = false;
            }

            exerciselbl4.IsVisible = true;
            exerciseinfolbl4.IsVisible = true;

            exerciseonefloat.IsVisible = true;

            exerciselbl5.IsVisible = true;
            exerciseinfolbl5.IsVisible = true;

            exercisetwofloat.IsVisible = true;  



        }
        catch(Exception ex)
        {

        }
    }

    private void dietlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as answer;

            if(item.label.Contains("Other"))
            {
                adddietlbl.IsVisible = true;
                dietfloat.IsVisible = true;
            }
            else
            {
                adddietlbl.IsVisible = false;
                dietfloat.IsVisible = false;
                dietetext.Text = string.Empty;
            }

            DietToAdd = null;

            if (item.label.Contains("Other"))
            {
                return;
            }
            else
            {
                DietToAdd = new userdiet();
                var AddDiet = new userdiet();
                AddDiet.userid = newuser.userid;
                AddDiet.datestarted = DateTime.Now.Date.ToString("dd/MM/yyyy");

                if (item.answerid == "29F8D326-23F9-401F-9CB7-37368F60924A" || item.label == "Vegetarian")
                {
                    AddDiet.diettitle = "Vegetarian Diet";
                    AddDiet.dietid = "59120ABF-A96E-470E-AF3F-26ABE992F1A6";
                }
                else if (item.answerid == "69D8DE85-BF86-4FB4-B3C8-3DBE215CFC21" || item.label == "Dairy Free")
                {
                    AddDiet.diettitle = "Dairy Free (Lactose-Free Diet)";
                    AddDiet.dietid = "FD13132F-82CA-4E50-AD8D-1DE069D2524F";
                }
                else if (item.answerid == "8F43A920-485A-4040-B26C-078F6C36B6BF" || item.label == "Gluten Free")
                {
                    AddDiet.diettitle = "Gluten-Free Diet";
                    AddDiet.dietid = "FDB2C391-F172-40A4-8B21-A72AB8B6CC72";
                }
                else if (item.answerid == "8FB873F9-66B8-448E-A360-F6AE1EC7CC50" || item.label == "Pescatarian")
                {
                    AddDiet.diettitle = "Pescatarian";
                    AddDiet.dietid = "972F4A6A-993B-4C6E-B58C-C94AC37B477D";
                }
                else if (item.answerid == "B4B423D1-395A-4F7D-9700-F9D652D0260A" || item.label == "Vegan")
                {
                    AddDiet.diettitle = "Vegan Diet";
                    AddDiet.dietid = "0566612F-691F-4FBB-B96C-1FB1E8458A2F";
                }

                DietToAdd = AddDiet;
            }           
        }
        catch(Exception ex)
        {

        }
    }

    private void stresslist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {

            var item = e.DataItem as answer;

            if (item.label.Contains("Other"))
            {
                addstresslbl.IsVisible = true;
                stressfloat.IsVisible = true;
            }
            else
            {
                addstresslbl.IsVisible = false;
                stressfloat.IsVisible = false;
                stresstext.Text = string.Empty;
            }




        }
        catch (Exception ex)
        {

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
        catch(Exception ex)
        {

        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //back button clicked             
                if (ctframe.IsVisible == true)
                {
                    ctframe.IsVisible = false;
                    comprefframe.IsVisible = true;
                    skipbtn.IsVisible = true;
                }
                else if (comprefframe.IsVisible == true)
                {
                    comprefframe.IsVisible = false;
                    skipbtn.IsVisible = false;
                    HotFlushesframe.IsVisible = true;
                }
                else if (HotFlushesframe.IsVisible == true)
                {
                    HotFlushesframe.IsVisible = false;
                    skipbtn.IsVisible = true;
                    medicationsframe.IsVisible = true;
                }
                else if (medicationsframe.IsVisible == true)
                {
                    medicationsframe.IsVisible = false;
                    symptomsframe.IsVisible = true;

                }
                else if (symptomsframe.IsVisible == true)
                {
                    symptomsframe.IsVisible = false;

                    if (!string.IsNullOrEmpty(signupcodeinfo.diagnosis))
                    {

                        conditionframe.IsVisible = true;
                    }
                    else
                    {
                        exerciseframe.IsVisible = true;
                    }

                }
                else if (conditionframe.IsVisible == true)
                {
                    conditionframe.IsVisible = false;
                    exerciseframe.IsVisible = true;


                }
                else if (exerciseframe.IsVisible == true)
                {
                    exerciseframe.IsVisible = false;
                    smokingframe.IsVisible = true;


                }
                else if (smokingframe.IsVisible == true)
                {
                    smokingframe.IsVisible = false;
                    stressframe.IsVisible = true;


                }
                else if (stressframe.IsVisible == true)
                {
                    stressframe.IsVisible = false;
                    alcoholframe.IsVisible = true;


                }
                else if (alcoholframe.IsVisible == true)
                {
                    alcoholframe.IsVisible = false;
                    dietframe.IsVisible = true;


                }
                else if (dietframe.IsVisible == true)
                {
                    dietframe.IsVisible = false;
                    genderatbirthframe.IsVisible = true;


                }
                else if (genderatbirthframe.IsVisible == true)
                {
                    genderatbirthframe.IsVisible = false;
                    hwframe.IsVisible = true;
                    //MessagingCenter.Send<object>(this, "RemoveProgress");
                    //Navigation.RemovePage(this);
                    //return;


                }
                else if(hwframe.IsVisible == true)
                {
                    hwframe.IsVisible = false;
                    postcodeframe.IsVisible = true;

                }
                else if(postcodeframe.IsVisible == true)
                {
                    postcodeframe.IsVisible = false;
                    occupationframe.IsVisible = true;
                }
                else if(occupationframe.IsVisible == true)
                {
                    MessagingCenter.Send<object>(this, "RemoveProgress");
                    Navigation.RemovePage(this);
                    return;
                }

                BackProgress();
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

    private void postcodelist_SizeChanged(object sender, EventArgs e)
    {
        try
        {
            var viewCell = sender as View;
            if (viewCell != null)
            {
                double itemHeight = viewCell.Height;
                //postcodelist.HeightRequest = itemHeight; 
            }
        }
        catch (Exception Ex)
        {
            //Ignore
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
                //postcodelist.SelectedItem = null;
                postcodetext.Text = item.postcodebrick;
                postcodelist.ItemsSource = NewItem;
                postcodelist.HeightRequest = 65;
            }

         
        }
        catch (Exception ex)
        {

        }
    }

    private void additionlsymlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {

            //var currentSelection = e.CurrentSelection.Cast<symptom>().ToList();

            //// Determine newly selected items
            //var SelectedItem = currentSelection.Except(previousSelection).FirstOrDefault();

            //// Update the previous selection list
            //if (currentSelection != null)
            //{
            //    previousSelection = currentSelection;
            //}
            //else
            //{
            //    SelectedItem = previousSelection;
            //}

            //// Find newly selected (added) item
            //var newlySelected = currentSelection.Except(previousSelection).FirstOrDefault();

            //// Find deselected (removed) item
            //var deselected = previousSelection.Except(currentSelection).FirstOrDefault();

            //// Update previous selection for next event
            //previousSelection = currentSelection;

            //// Pick whichever changed (selected or deselected)
            //var selectedItem = newlySelected ?? deselected;

            //if (selectedItem == null)
            //    return; // No change, nothing to do


            //searchsymsentry.IsEnabled = false;
            //searchsymsentry.IsEnabled = true;

            //// Convert the selected item to a ChipItem
            //if (additionalfilteredsymptomlist.Contains(SelectedItem))
            //{
            //    additionalfilteredsymptomlist.Remove(SelectedItem);
            //    symptomchipselectedlist.Remove(SelectedItem);
            //}
            //else
            //{

            //    additionalfilteredsymptomlist.Add(SelectedItem);
            //    symptomchipselectedlist.Add(SelectedItem);
            //}

            //if (additionalfilteredsymptomlist.Count == 0)
            //{
            //    addlbl1.IsVisible = false;
            //    additionalsymptomchiplistnrat.IsVisible = false;
            //}
            //else
            //{
            //    addlbl1.IsVisible = true;
            //    additionalsymptomchiplistnrat.IsVisible = true;
            //}

            var currentSelection = e.CurrentSelection.Cast<symptom>().ToList();

            var newlySelectedItems = currentSelection.Except(previousSelection).ToList();
            var deselectedItems = previousSelection.Except(currentSelection).ToList();

            previousSelection = currentSelection;

            searchsymsentry.IsEnabled = false;
            searchsymsentry.IsEnabled = true;

            foreach (var item in newlySelectedItems)
            {
                if (!additionalfilteredsymptomlist.Contains(item))
                {
                    additionalfilteredsymptomlist.Add(item);
                    symptomchipselectedlist.Add(item);
                }
            }

            foreach (var item in deselectedItems)
            {
                if (additionalfilteredsymptomlist.Contains(item))
                {
                    additionalfilteredsymptomlist.Remove(item);
                    symptomchipselectedlist.Remove(item);
                }
            }

            bool hasItems = additionalfilteredsymptomlist.Count > 0;
            addlbl1.IsVisible = hasItems;
            additionalsymptomchiplistnrat.IsVisible = hasItems;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void additionlmedlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {

            //var currentSelection = e.CurrentSelection.Cast<medication>().ToList();

            //// Determine newly selected items
            //var SelectedItem = currentSelection.Except(Selectionprevious).FirstOrDefault();

            //// Update the previous selection list
            //Selectionprevious = currentSelection;

            //searchsymsentry.IsEnabled = false;
            //searchsymsentry.IsEnabled = true;

            //searchmedentry.IsEnabled = false;
            //searchmedentry.IsEnabled = true;

            //// Convert the selected item to a ChipItem
            //if (additionalfilteredmedicationlist.Contains(SelectedItem))
            //{
            //    additionalfilteredmedicationlist.Remove(SelectedItem);
            //    medicationchipselectedlist.Remove(SelectedItem);
            //}
            //else
            //{

            //    additionalfilteredmedicationlist.Add(SelectedItem);
            //    medicationchipselectedlist.Add(SelectedItem);
            //}

            //if (additionalfilteredmedicationlist.Count == 0)
            //{
            //    addmedlbl1.IsVisible = false;
            //    additionalmedicationchiplistnrat.IsVisible = false;
            //}
            //else
            //{
            //    addmedlbl1.IsVisible = true;
            //    additionalmedicationchiplistnrat.ItemsSource = additionalfilteredmedicationlist;
            //    additionalmedicationchiplistnrat.IsVisible = true;
            //}


            var currentSelection = e.CurrentSelection.Cast<medication>().ToList();

            var newlySelectedItems = currentSelection.Except(Selectionprevious).ToList();
            var deselectedItems = Selectionprevious.Except(currentSelection).ToList();

            Selectionprevious = currentSelection;

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
            addlbl1.IsVisible = hasItems;
            additionalmedicationchiplistnrat.IsVisible = hasItems;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}