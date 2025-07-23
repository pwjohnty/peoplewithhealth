using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
namespace PeopleWith;
public partial class BMS : ContentPage
{
    //Data passed 
    user newuser;
    signupcode signupcodeinfo;
    ObservableCollection<symptom> allsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<medication> allmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<question> AllQuestions = new ObservableCollection<question>();
    ObservableCollection<answer> AllAnswers = new ObservableCollection<answer>();
    public consent additonalconsent = new consent();

    //Symptom Data
    ObservableCollection<symptom> filteredsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<symptom> additionalfilteredsymptomlist = new ObservableCollection<symptom>();
    ObservableCollection<symptom> symptomchipselectedlist = new ObservableCollection<symptom>();
    private List<symptom> SelectionPrevious = new();

    bool isNavigating = false;
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

    ObservableCollection<answer> GetAnswers = new ObservableCollection<answer>();
    ObservableCollection<answer> GetCommPref = new ObservableCollection<answer>();
    ObservableCollection<usermedication> medicationstoadd = new ObservableCollection<usermedication>();
    ObservableCollection<usermeasurement> addusermeasurements = new ObservableCollection<usermeasurement>();
    ObservableCollection<userdiagnosis> adduserdiagnosis = new ObservableCollection<userdiagnosis>();
    ObservableCollection<usersymptom> symptomstoadd = new ObservableCollection<usersymptom>();
    List<string> commprefaddedlist = new List<string>();

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

    //Questions 
    question pregnancyquestion;
    question occupationquestion;
    question languagequestion;
    question ctquestion;

    APICalls database = new APICalls();
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

    public BMS()
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

    public BMS(user userp, ObservableCollection<symptom> symtpomsp, ObservableCollection<medication> medicationsp, signupcode signupcodeinfop, double progressp, ObservableCollection<question> requestions, ObservableCollection<answer> reganswers, consent addtionalcon)
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

            //Add Diagnosis 
            var newuserdiag = new userdiagnosis();
            newuserdiag.diagnosistitle = "Hypertrophic Obstructive Cardiomyopathy (HCM)";
            newuserdiag.diagnosisid = "439BB9F6-93AD-4BE4-843C-FD8199D859F1";
            newuserdiag.userid = newuser.userid;
            adduserdiagnosis.Add(newuserdiag);

            if (!string.IsNullOrEmpty(signupcodeinfo.externalidentifier))
            {
                extidlbl.Text = signupcodeinfo.externalidentifier;
            }

            LoadAllQuestionsData();
            getpostcodes();
            //getdiagnosis();
            GetSymptoms();
            GetMedications();
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

            //pregnancy
            pregnancyquestion = AllQuestions.Where(x => x.title.Contains("Are you currently pregnant")).SingleOrDefault();

            if (pregnancyquestion != null)
            {
                pregnancytitle.Text = pregnancyquestion.title;
                pregnancydetails.Text = pregnancyquestion.directions;

                GetAnswers = AllAnswers.Where(x => x.questionid == pregnancyquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                pregnancylist.ItemsSource = GetAnswersorder;
            }

            //Preffered language
            languagequestion = AllQuestions.Where(x => x.title.Contains("Preferred Language")).SingleOrDefault();

            if (languagequestion != null)
            {
                languagetitle.Text = languagequestion.title;
                languagedetails.Text = languagequestion.directions;

                GetAnswers = AllAnswers.Where(x => x.questionid == languagequestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                languagelist.ItemsSource = GetAnswersorder;
            }

            //Clinicaltrials
            ctquestion = AllQuestions.Where(x => x.title.Contains("clinical trials")).SingleOrDefault();

            if (ctquestion != null)
            {

                ctnamequestion.Text = ctquestion.title;
                ctquestiondes.Text = ctquestion.directions;

                GetAnswers = AllAnswers.Where(x => x.questionid == ctquestion.questionid).ToObservableCollection();

                var GetAnswersorder = GetAnswers.OrderBy(x => Convert.ToInt32(x.order)).ToObservableCollection();

                ctlist.ItemsSource = GetAnswersorder;
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

    //async void getdiagnosis()
    //{
    //    try
    //    {
    //        ObservableCollection<diagnosis> diagnosisCollection = new ObservableCollection<diagnosis>();

    //        if (signupcodeinfo.diagnosis.Contains(","))
    //        {
    //            var splititem = signupcodeinfo.diagnosis.Split(',').ToList();

    //            foreach (var item in splititem)
    //            {
    //                var nd = new diagnosis();
    //                nd.Diagnosisid = item;
    //                diagnosisCollection.Add(nd);
    //            }

    //            foreach (var item in diagnosisCollection)
    //            {

    //                var dia = await database.GetAsyncSingleDiagnosis(item);
    //                alldiagnosislist.Add(dia);
    //            }

    //            conditionlist.ItemsSource = alldiagnosislist;
    //            conditionlist.HeightRequest = alldiagnosislist.Count * 110;
    //        }
    //        else
    //        {
    //            if (!string.IsNullOrEmpty(signupcodeinfo.diagnosis))
    //            {

    //                var nd = new diagnosis();
    //                nd.Diagnosisid = signupcodeinfo.diagnosis;
    //                diagnosisCollection.Add(nd);

    //                foreach (var item in diagnosisCollection)
    //                {
    //                    var dia = await database.GetAsyncSingleDiagnosis(item);
    //                    alldiagnosislist.Add(dia);
    //                }

    //                conditionlist.ItemsSource = alldiagnosislist;
    //                conditionlist.HeightRequest = alldiagnosislist.Count * 110;
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        NotasyncMethod(Ex);
    //    }
    //}

    async void GetSymptoms()
    {
        try
        {
            var splitsymplist = signupcodeinfo.symptoms.Split(',').ToList();

            foreach (var item in splitsymplist)
            {
                var symp = allsymptomlist.Where(x => x.symptomid == item).SingleOrDefault();

                if (symp != null)
                {
                    filteredsymptomlist.Add(symp);
                }
            }
            symptomchiplist.ItemsSource = filteredsymptomlist;
            additionlsymlist.ItemsSource = allsymptomlist;
            additionalsymptomchiplist.ItemsSource = additionalfilteredsymptomlist;
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

    private void symptomchiplist_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
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

            if (FilterSymptomsList.Count == 0)
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

    private async void additionlsymlist_RemainingItemsThresholdReached(object sender, EventArgs e)
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

    private void additionlsymlist_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        try
        {
            var currentSelection = e.CurrentSelection.Cast<symptom>().ToList();

            var newlySelectedItems = currentSelection.Except(SelectionPrevious).ToList();
            var deselectedItems = SelectionPrevious.Except(currentSelection).ToList();

            SelectionPrevious = currentSelection;

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
            additionalsymptomchiplist.IsVisible = hasItems;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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

    private async void additionlmedlist_RemainingItemsThresholdReached(object sender, EventArgs e)
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

    private void Skip_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Add all skipable Items 
            if (symptomsframe.IsVisible == true)
            {
                symptomstoadd.Clear();

                symptomsframe.IsVisible = false;
                medicationsframe.IsVisible = true;
                skipbtn.IsVisible = true;

                UpdateProgress("Forward");

            }
            else if (medicationsframe.IsVisible == true)
            {
                medicationstoadd.Clear();

                medicationsframe.IsVisible = false;
                occupationframe.IsVisible = true;
                skipbtn.IsVisible = false;
                UpdateProgress("Forward");
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //Next Clicked 
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

                //Page One 
                if (postcodeframe.IsVisible == true)
                {
                    Handlepostcode();
                }
                //page Two
                else if (hwframe.IsVisible == true)
                {
                    HandleHW();
                }
                //page Three             
                else if (symptomsframe.IsVisible == true)
                {
                    Handlesymptoms();
                }
                //page Four
                else if (medicationsframe.IsVisible == true)
                {
                    Handlemedications();
                }
                //page Five
                else if (occupationframe.IsVisible == true)
                {
                    HandleOccupation();
                }
                //page Six
                else if (languageframe.IsVisible == true)
                {
                    Handlelangugaue();
                }
                //page Seven
                else if (pregnancyframe.IsVisible == true)
                {
                    HandlePregnancy();
                }
                //page Eight
                else if (ctframe.IsVisible == true)
                {
                    HandleCT();
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

    //Back Clicked 
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
                    pregnancyframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (pregnancyframe.IsVisible == true)
                {
                    pregnancyframe.IsVisible = false;
                    languageframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (languageframe.IsVisible == true)
                {
                    languageframe.IsVisible = false;
                    occupationframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (occupationframe.IsVisible == true)
                {
                    occupationframe.IsVisible = false;
                    medicationsframe.IsVisible = true;
                    skipbtn.IsVisible = true;
                }
                else if (medicationsframe.IsVisible == true)
                {
                    medicationsframe.IsVisible = false;
                    symptomsframe.IsVisible = true;
                    skipbtn.IsVisible = true;
                }
                else if (symptomsframe.IsVisible == true)
                {
                    symptomsframe.IsVisible = false;
                    hwframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (hwframe.IsVisible == true)
                {
                    hwframe.IsVisible = false;
                    postcodeframe.IsVisible = true;
                    skipbtn.IsVisible = false;
                }
                else if (postcodeframe.IsVisible == true)
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

    //Privacy policy Tapped 
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


    //Handle Items Progression 
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
            hwframe.IsVisible = true;
            skipbtn.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void HandleHW()
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
            symptomsframe.IsVisible = true;
            skipbtn.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Handlesymptoms()
    {
        try
        {
            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

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

        }
        catch (Exception ex)
        {

        }
    }

    async void Handlemedications()
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
            occupationframe.IsVisible = true;
            skipbtn.IsVisible = false;
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
            languageframe.IsVisible = true;
            skipbtn.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Handlelangugaue()
    {
        try
        {
            if (languagelist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == languagequestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = languagelist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = occupationquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);

            //Change Page 
            languageframe.IsVisible = false;
            pregnancyframe.IsVisible = true;
            skipbtn.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void HandlePregnancy()
    {
        try
        {
            if (pregnancylist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            //ScrolltoTop to fix swithcing page issue 
            Fixscrollview();

            var itemToRemove = userresponselist.FirstOrDefault(q => q.questionid == pregnancyquestion.questionid);

            if (itemToRemove != null)
            {
                userresponselist.Remove(itemToRemove);
            }

            var item = pregnancylist.SelectedItem as answer;

            var response = new userresponse();
            response.questionid = pregnancyquestion.questionid;
            response.answerid = item.answerid;
            response.userid = newuser.userid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            userresponselist.Add(response);

            //Change Page 
            pregnancyframe.IsVisible = false;
            ctframe.IsVisible = true;
            skipbtn.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
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

            if (newuser.signupcodeid.Contains("RBHTHCM"))
            {
                newuser.registrycondition = "HOCM";
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


}