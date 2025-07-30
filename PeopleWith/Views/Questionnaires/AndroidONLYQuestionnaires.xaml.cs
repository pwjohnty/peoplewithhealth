using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using Mopups.Services;
using System.IO.Pipes;
using Microsoft.Maui.Networking;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices;
using System.Threading.Tasks;

namespace PeopleWith;

public partial class AndroidONLYQuestionnaires : ContentPage
{
    public questionnaire questionnairefromlist = new questionnaire();
    public int rownumber = 0;
    APICalls aPICalls = new APICalls();
    bool completeduserquestionnaire;
    public userquestionnaire completeuserquestionnaire = new userquestionnaire();
    public ObservableCollection<userquestionnaire> alluserquestionnaires = new ObservableCollection<userquestionnaire>();
    public ObservableCollection<questionanswerinfo> QuestionsInOrder = new ObservableCollection<questionanswerinfo>();
    public ObservableCollection<questionanswerinfo> SelectedAnswerList = new ObservableCollection<questionanswerinfo>();
    public int OrderNumber = 1;
    public double ValueofSlider = 0;
    ObservableCollection<string> answerid = new ObservableCollection<string>();
    String RecordID = null;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    bool InitalLoad = false;
    userfeedback userfeedbacklistpassed = new userfeedback();
    bool fromdash;

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

    public AndroidONLYQuestionnaires()
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

    public AndroidONLYQuestionnaires(string questionnaireid, userfeedback userfeedbacklist)
    {
        try
        {
            //from dash

            InitializeComponent();

            fromdash = true;

            userfeedbacklistpassed = userfeedbacklist;

            //get questionnaire detais
            getquestionnairedetails(questionnaireid);



        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public AndroidONLYQuestionnaires(string questionnaireid)
    {
        try
        {
            InitializeComponent();

            //notification tap


            //get questionnaire detais
            getquestionnairedetails(questionnaireid);


        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getquestionnairedetails(string questionnaireid)
    {
        try
        {
            InitalLoad = true;

            loadingstack.IsVisible = true;

            var uq = await aPICalls.GetSingleQuestionnaire(questionnaireid);



            if (uq != null)
            {

                questionnairefromlist = uq[0];



                questionnairetitlelbl.Text = questionnairefromlist.title;

                questionnairedeslbl.Text = questionnairefromlist.description;

                //alluserquestionnaires = alluserquestionnairespassed;

                populatequestionnaire();
                InitalLoad = false;
            }

        }
        catch (Exception Ex)
        {
        }
    }

    public AndroidONLYQuestionnaires(questionnaire questionnairepassed, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            InitalLoad = true;

            userfeedbacklistpassed = userfeedbacklist;
            questionnairefromlist = questionnairepassed;

            loadingstack.IsVisible = true;

            questionnairetitlelbl.Text = questionnairefromlist.title;

            questionnairedeslbl.Text = questionnairefromlist.description;

            //alluserquestionnaires = alluserquestionnairespassed;

            populatequestionnaire();
            InitalLoad = false;

            //  mainquestionnaire.ItemsSource = questionnairepassed;

            // mainquestionnaire.HeightRequest = 1000;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    public AndroidONLYQuestionnaires(userquestionnaire userquestionnairepassed, questionnaire questionnairepassed)
    {
        try
        {
            InitializeComponent();

            questionnairefromlist = questionnairepassed;

            loadingstack.IsVisible = true;

            completedquestionnairetitlelbl.Text = questionnairefromlist.title;

            completedquestionnairedeslbl.Text = questionnairefromlist.description;

            completeuserquestionnaire = userquestionnairepassed;

            completeduserquestionnaire = true;


            populatecompletedquestionnaire();

            //  mainquestionnaire.ItemsSource = questionnairepassed;

            // mainquestionnaire.HeightRequest = 1000;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void populatecompletedquestionnaire()
    {
        try
        {
            var GetCount = questionnairefromlist.questionanswerjsonlist.Count.ToString();
            foreach (var item in questionnairefromlist.questionanswerjsonlist)
            {
                var userquestionanswer = completeuserquestionnaire.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

                item.Addfreetext = false;
                item.Doublenumentry = false;
                item.Numericentry = false;
                item.Sliderscale = false;
                item.Isrequired = false;
                item.Bordercolor = Colors.White;
                item.questionnum = "Question " + item.questionorder + " of " + GetCount;

                if (item.questiontype == "singleselection")
                {
                    item.singleselection = true;
                    item.mulitpleselection = false;


                    item.AnswerOptions = new ObservableCollection<AnswerViewModel>();

                    int i = 0;

                    Random random = new Random();
                    int notid = random.Next(100000, 100000001);

                    foreach (var answer in item.questionanswers)
                    {


                        item.AnswerOptions.Add(new AnswerViewModel
                        {
                            answerid = answer.answerid,
                            questionid = item.questionid,
                            answeroptions = answer.answeroptions,
                            answertitle = answer.answertitle,
                            groupkey = notid.ToString(),
                            isVisible = false,  // Set to true since it's visible in this case
                            bordervis = false
                        });



                    }

                    //find out the selected item


                    //var selectedanswer = item.AnswerOptions.Where(x => x.answerid == userquestionanswer.answer[0].answerid).FirstOrDefault();

                    //var selectedIndex = item.AnswerOptions.IndexOf(selectedanswer);

                    ////only Set selected item to isvisible = true; for android loading issue 
                    //item.AnswerOptions[selectedIndex].selectedss = true;
                    //item.AnswerOptions[selectedIndex].isVisible = true;
                    //item.AnswerOptions[selectedIndex].bordervis = true;

                    // Filter and retain only the selected answers
                    var filteredAnswers = item.AnswerOptions.Where(x => userquestionanswer.answer.Any(it => it.answerid == x.answerid)).ToList();

                    // Update AnswerOptions with the filtered answers
                    item.AnswerOptions = new ObservableCollection<AnswerViewModel>(filteredAnswers);

                    foreach (var it in userquestionanswer.answer)
                    {
                        if (item.AnswerOptions.Count == 0)
                        {


                        }
                        else
                        {
                            item.AnswerOptions[0].selectedms = true;
                            item.AnswerOptions[0].isVisible = true;
                            item.AnswerOptions[0].bordervis = true;
                            item.AnswerOptions[0].ImgSource = "radiobutton.png";


                            if (!string.IsNullOrEmpty(it.answervalue))
                            {
                                item.freetextentry = it.answervalue;
                            }
                            else
                            {
                                item.freetextentry = null;
                                item.Addfreetext = false;
                            }
                        }
                        //if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                        //{
                        //    item.freetextentry = userquestionanswer.answer[0].answervalue;
                        //}
                    }



                    if (item.questionanswers.Any(x => x.answeroptions == "specifyfreetext"))
                    {
                        if (string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                        {
                            item.Addfreetext = false;
                        }
                        else
                        {
                            item.Addfreetext = true;
                            item.Addfreetextopacity = 0.5;
                            item.Addfreetextenabled = false;
                        }
                    }

                }
                else if (item.questiontype == "multipleselection")
                {
                    item.mulitpleselection = true;
                    item.singleselection = false;

                    item.AnswerOptions = new ObservableCollection<AnswerViewModel>();

                    foreach (var answer in item.questionanswers)
                    {
                        item.AnswerOptions.Add(new AnswerViewModel
                        {
                            answerid = answer.answerid,
                            questionid = item.questionid,
                            answeroptions = answer.answeroptions,
                            answertitle = answer.answertitle,
                            isVisible = false,  // Set to true since it's visible in this case
                            bordervis = false
                        });
                    }

                    // Filter and retain only the selected answers
                    var filteredAnswers = item.AnswerOptions.Where(x => userquestionanswer.answer.Any(it => it.answerid == x.answerid)).ToList();

                    // Update AnswerOptions with the filtered answers
                    item.AnswerOptions = new ObservableCollection<AnswerViewModel>(filteredAnswers);

                    foreach (var it in userquestionanswer.answer)
                    {

                        foreach (var option in item.AnswerOptions)
                        {
                            option.selectedms = true;
                            option.isVisible = true;
                            option.bordervis = true;
                            option.ImgSource = "radiobutton.png";
                        }

                        // item.AnswerOptions[0].selectedms = true;
                        // item.AnswerOptions[0].isVisible = true;
                        // item.AnswerOptions[0].bordervis = true;
                        //  item.AnswerOptions[0].ImgSource = "radiobutton.png";

                        if (!string.IsNullOrEmpty(it.answervalue))
                        {
                            item.freetextentry = it.answervalue;
                        }
                        else
                        {
                            //Set Entry to isVisible false 
                            item.Addfreetext = false;
                        }

                    }

                    if (item.questionanswers.Any(x => x.answeroptions == "specifyfreetext"))
                    {
                        if (string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                        {
                            item.Addfreetext = false;
                        }
                        else
                        {
                            item.Addfreetext = true;
                            item.Addfreetextopacity = 0.5;
                            item.Addfreetextenabled = false;
                        }
                    }
                }
                else if (item.questiontype == "freetext")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Addfreetext = true;
                    item.Addfreetextenabled = true;
                    item.Addfreetextopacity = 1;

                    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                    {
                        item.freetextentry = userquestionanswer.answer[0].answervalue;
                    }

                }
                else if (item.questiontype == "numeric")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Numericentry = true;

                    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                    {
                        item.numericentrytext = userquestionanswer.answer[0].answervalue;
                    }

                }
                else if (item.questiontype == "doublenumeric")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Doublenumentry = true;

                    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                    {
                        var splitnum = userquestionanswer.answer[0].answervalue.Split('|');

                        item.doubleentry1 = splitnum[0];
                        item.doubleentry2 = splitnum[1];
                    }

                }
                else if (item.questiontype == "scale110singleselection")
                {
                    item.answerid = item.questionanswers[0].answerid;

                    item.Sliderscale = true;

                    item.answertitle = item.questionanswers[0].answertitle;

                    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                    {
                        var convertnum = Convert.ToDouble(userquestionanswer.answer[0].answervalue);

                        //Might need to change to SliderValue
                        item.SliderValue = convertnum;
                    }

                }



            }

            if (completeduserquestionnaire)
            {
                completedquestionnaire.IsVisible = true;
                CompletedQuestionView.IsVisible = true;
                completedquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;
                completedquestionnaire.HeightRequest = questionnairefromlist.questionanswerjsonlist.Count * 180;
                //completedquestionnaire.MaximumHeightRequest = 1500;
                //completedquestionnaire.HeightRequest = CalculateHeight();

            }
            else
            {
                mainquestionnaire.IsVisible = true;
                MainQuestionScroll.IsVisible = true;

                mainquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;


                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    //  mainquestionnaire.HeightRequest = 3000;
                }

            }

            await Task.Delay(1000);
            loadingstack.IsVisible = false;
            datastack.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //double CalculateHeight()
    //{
    //    double totalHeight = 0;
    //    double header = 50; 
    //    double single = 120;
    //    double multiple = 300;
    //    double freetext = 80;
    //    double numeric = 80;
    //    double doublenumeric = 80;
    //    double scale = 50; 

    //    foreach (var item in questionnairefromlist.questionanswerjsonlist)
    //    {
    //        //double itemHeight = EstimateItemHeight(item);
    //        //totalHeight += itemHeight;
    //        totalHeight = totalHeight + header; 

    //        if (item.questiontype == "singleselection")
    //        {
    //            totalHeight = totalHeight + single; 
    //        }
    //        else if (item.questiontype == "multipleselection")
    //        {
    //            totalHeight = totalHeight + multiple;
    //        }
    //        else if (item.questiontype == "freetext")
    //        {
    //            totalHeight = totalHeight + freetext;
    //        }
    //        else if (item.questiontype == "numeric")
    //        {
    //            totalHeight = totalHeight + numeric;
    //        }
    //        else if (item.questiontype == "doublenumeric")
    //        {
    //            totalHeight = totalHeight + doublenumeric;
    //        }
    //        else if (item.questiontype == "scale110singleselection")
    //        {
    //            totalHeight = totalHeight + scale;
    //        }

    //    }

    //    return totalHeight;
    //}



    async void populatequestionnaire()
    {
        try
        {
            var GetCount = questionnairefromlist.questionanswerjsonlist.Count.ToString();

            foreach (var item in questionnairefromlist.questionanswerjsonlist)
            {
                item.Addfreetext = false;
                item.Doublenumentry = false;
                item.Numericentry = false;
                item.Sliderscale = false;
                item.Isrequired = false;
                item.Bordercolor = Colors.White;
                item.questionnum = "Question " + item.questionorder + " of " + GetCount;
                item.Hasanswered = false;

                if (item.questiontype == "singleselection")
                {
                    item.singleselection = true;
                    item.mulitpleselection = false;


                    item.AnswerOptions = new ObservableCollection<AnswerViewModel>();

                    int i = 0;

                    Random random = new Random();
                    int notid = random.Next(100000, 100000001);

                    foreach (var answer in item.questionanswers)
                    {


                        item.AnswerOptions.Add(new AnswerViewModel
                        {
                            answerid = answer.answerid,
                            questionid = item.questionid,
                            answeroptions = answer.answeroptions,
                            //answeroptions = "singleselection",
                            answertitle = answer.answertitle,
                            groupkey = notid.ToString(),
                            isVisible = true  // Set to true since it's visible in this case
                        });

                    }


                    if (item.questionanswers.Any(x => x.answeroptions == "specifyfreetext"))
                    {
                        item.Addfreetext = true;
                        item.Addfreetextopacity = 0.5;
                        item.Addfreetextenabled = false;
                    }


                }
                else if (item.questiontype == "multipleselection")
                {
                    item.mulitpleselection = true;
                    item.singleselection = false;

                    item.AnswerOptions = new ObservableCollection<AnswerViewModel>();

                    foreach (var answer in item.questionanswers)
                    {
                        item.AnswerOptions.Add(new AnswerViewModel
                        {
                            answerid = answer.answerid,
                            questionid = item.questionid,
                            answeroptions = answer.answeroptions,
                            //answeroptions = "multipleselection",
                            answertitle = answer.answertitle,
                            isVisible = true  // Set to true since it's visible in this case
                        });
                    }

                    if (item.questionanswers.Any(x => x.answeroptions == "specifyfreetext"))
                    {
                        item.Addfreetext = true;
                        item.Addfreetextopacity = 0.5;
                        item.Addfreetextenabled = false;
                    }
                }
                else if (item.questiontype == "freetext")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Addfreetext = true;
                    item.Addfreetextenabled = true;
                    item.Addfreetextopacity = 1;
                }
                else if (item.questiontype == "numeric")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Numericentry = true;
                }
                else if (item.questiontype == "doublenumeric")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Doublenumentry = true;
                }
                else if (item.questiontype == "scale110singleselection")
                {
                    item.answerid = item.questionanswers[0].answerid;

                    item.Sliderscale = true;
                    item.Hasanswered = true;
                    item.slidervalue = 0;
                    item.answertitle = item.questionanswers[0].answertitle;
                }
            }

            if (completeduserquestionnaire)
            {
                completedquestionnaire.IsVisible = true;
                CompletedQuestionView.IsVisible = true;
                //Dont Forget to Remove this ([0]);
                ObservableCollection<questionanswerinfo> Answer = new ObservableCollection<questionanswerinfo>();

                var getitem = questionnairefromlist.questionanswerjsonlist.First();
                Answer.Add(getitem);
                completedquestionnaire.ItemsSource = Answer;
                //completedquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;

            }
            else
            {
                mainquestionnaire.IsVisible = true;
                MainQuestionScroll.IsVisible = true;
                ObservableCollection<questionanswerinfo> Answer = new ObservableCollection<questionanswerinfo>();

                var getitem = questionnairefromlist.questionanswerjsonlist.First();
                Answer.Add(getitem);

                foreach (var item in questionnairefromlist.questionanswerjsonlist)
                {
                    if (item.questionorder == "1")
                    {
                        QuestionsInOrder.Add(item);
                    }
                }

                mainquestionnaire.ItemsSource = QuestionsInOrder;
            }

            await Task.Delay(1000);
            loadingstack.IsVisible = false;
            datastack.IsVisible = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedRadioButton_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //single selection

            if (e.IsChecked == true)
            {
                var item = (ExtendedRadioButton)sender;

                if (item != null)
                {

                    var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

                    if (getitem == null)
                    {
                        getitem = QuestionsInOrder[0];
                        item.IDRecord = RecordID;
                        //item.IDRecord = QuestionsInOrder[0].AnswerOptions.ToString();
                    }

                    if (item.IDRecord == "specifyfreetext")
                    {
                        getitem.Addfreetext = true;
                    }
                    else
                    {
                        getitem.Addfreetext = false;
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedCheckbox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //mulitple selection

            var item = (ExtendedCheckbox)sender;

            var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

            if (getitem == null)
            {
                if (QuestionsInOrder == null || QuestionsInOrder.Count == 0)
                {
                    return;
                }

                getitem = QuestionsInOrder[0];
                item.IDValue = QuestionsInOrder[0].answerid;
                item.IDRecord = RecordID;
                //item.IDRecord = QuestionsInOrder[0].AnswerOptions.ToString();

            }

            if (item.IDValue == null)
            {
                item.IDValue = QuestionsInOrder[0].answerid;
            }

            if (e.IsChecked == true)
            {

                if (!getitem.Selectedansweridlist.Contains(item.IDValue))
                {
                    getitem.Selectedansweridlist.Add(item.IDValue);
                }

                if (item != null)
                {
                    getitem.Bordercolor = Colors.White;
                    getitem.Isrequired = false;

                    if (item.IDRecord == "specifyfreetext")
                    {

                        //               }

                        //               bool Check = getitem.AnswerOptions
                        //.Any(x => !string.IsNullOrEmpty(x?.answeroptions) && x.answeroptions.Contains("specifyfreetext"));

                        //               if (Check)
                        //               {
                        //Item Contains Other Specify 
                        getitem.Addfreetextenabled = true;
                        getitem.Addfreetextopacity = 1;
                        getitem.Hasanswered = false;
                    }
                    else
                    {
                        //  getitem.Addfreetext = false;
                        getitem.Hasanswered = true;
                    }
                }
            }
            else
            {
                if (item != null)
                {

                    if (getitem.Selectedansweridlist.Contains(item.IDValue))
                    {
                        getitem.Selectedansweridlist.Remove(item.IDValue);
                    }

                    if (item.IDRecord == "specifyfreetext")
                    {
                        getitem.Addfreetextenabled = false;
                        getitem.Addfreetextopacity = 0.3;
                    }

                    bool check = getitem.Selectedansweridlist == null || getitem.Selectedansweridlist.Count == 0 || getitem.Selectedansweridlist[0] == null;
                    if (check)
                    {
                        getitem.Hasanswered = false;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            //single selection
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {

                if (e.Value == true)
                {
                    var item = (ExtendedRadioButton)sender;

                    if (item != null)
                    {
                        var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

                        if (getitem == null)
                        {
                            //check on iOS
                            return;
                            getitem = QuestionsInOrder[0];
                            item.IDValue = QuestionsInOrder[0].answerid;
                            item.IDRecord = RecordID;
                        }

                        getitem.Bordercolor = Colors.White;


                        getitem.Isrequired = false;

                        //clear list every time as only one allowed
                        getitem.Selectedansweridlist.Clear();

                        if (!getitem.Selectedansweridlist.Contains(item.IDValue))
                        {
                            getitem.Selectedansweridlist.Add(item.IDValue);
                        }

                        if (item.IDRecord == "specifyfreetext")
                        {
                            getitem.Addfreetextenabled = true;
                            getitem.Addfreetextopacity = 1;

                            if (getitem.freetextentry == null)
                            {
                                getitem.Hasanswered = false;
                            }
                            else
                            {
                                getitem.Hasanswered = true;
                            }
                        }
                        else
                        {
                            getitem.Addfreetextenabled = false;
                            getitem.Addfreetextopacity = 0.5;

                            //this is an option like yes, no etc with no extra text added
                            getitem.Hasanswered = true;
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

    private void ViewCell_Appearing(object sender, EventArgs e)
    {
        try
        {
            var viewcell = (ViewCell)sender;
            viewcell.View.BackgroundColor = (rownumber % 2 == 0) ? Colors.White : Colors.White;
            rownumber++;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            foreach (var item in QuestionsInOrder)
            {

                if (item.questionrequired)
                {
                    if (item.Hasanswered == false)
                    {

                        item.Bordercolor = Colors.Red;
                        item.Isrequired = true;
                        return;

                    }

                }
            }

            if (backbtn.IsVisible == false)
            {
                backbtn.IsVisible = true;
            }


            ValueofSlider = QuestionsInOrder[0].SliderValue;

            QuestionsInOrder.Clear();
            OrderNumber++;
            foreach (var x in questionnairefromlist.questionanswerjsonlist)
            {
                if (x.questionorder == OrderNumber.ToString())
                {
                    QuestionsInOrder.Add(x);
                    //var AddItem = new questionanswerinfo();
                    //AddItem.questionid = x.questionid;
                    //AddItem.SelectedAnswer = x.SelectedAnswer;
                    //AddItem.Selectedansweridlist = x.Selectedansweridlist;
                    //AddItem.questionorder = x.questionorder;
                    //AddItem.questiontype = x.questiontype;
                    //SelectedAnswerList.Add(AddItem);
                }
                var GetCount = questionnairefromlist.questionanswerjsonlist.Count;
                if (GetCount.ToString() == OrderNumber.ToString())
                {
                    NavigationStack.IsVisible = false;
                    submitbtn.IsVisible = true;
                }
            }

            //Added the following Foreach to Set Item Selection
            foreach (var item in QuestionsInOrder)
            {
                if (item.Selectedansweridlist == null) continue;

                if (item.Selectedansweridlist.Count > 0)
                {
                    var answerid = item.Selectedansweridlist[0];

                    var selectedanswer = item.AnswerOptions.Where(x => x.answerid == answerid).FirstOrDefault();

                    var selectedIndex = item.AnswerOptions.IndexOf(selectedanswer);

                    if (selectedIndex != -1)
                    {
                        if (item.singleselection == true)
                        {
                            item.AnswerOptions[selectedIndex].selectedss = true;

                        }
                        if (item.mulitpleselection == true)
                        {
                            item.AnswerOptions[selectedIndex].selectedms = true;
                        }
                    }

                    //Update if Has Answered if False
                    if (item.questionrequired)
                    {
                        if (item.Hasanswered == false)
                        {
                            item.Bordercolor = Colors.White;
                            item.questionrequired = true;
                            item.Isrequired = false;
                            item.Hasanswered = false;
                        }
                    }
                }
            }

            mainquestionnaire.ItemsSource = QuestionsInOrder;

        }
        catch (Exception Ex)
        {

        }
    }

    private async void backbtn_Clicked(object sender, EventArgs e)
    {
        try
        {


            foreach (var x in questionnairefromlist.questionanswerjsonlist)
            {
                if (x.questionorder == OrderNumber.ToString())
                {
                    //x.SliderValue = ValueofSlider;

                    answerid = x.Selectedansweridlist;

                }

            }

            answerid = null;
            OrderNumber--;

            QuestionsInOrder.Clear();

            foreach (var x in questionnairefromlist.questionanswerjsonlist)
            {
                if (x.questionorder == OrderNumber.ToString())
                {
                    //x.SliderValue = ValueofSlider;
                    x.Hasanswered = true;
                    answerid = x.Selectedansweridlist;

                    // x.Selectedansweridlist = new List<string>();
                    // x.Selectedansweridlist = answerid;
                    QuestionsInOrder.Add(x);

                    if (x.AnswerOptions != null)
                    {
                        RecordID = x.AnswerOptions[0].answeroptions;
                    }
                }
                var GetCount = questionnairefromlist.questionanswerjsonlist.Count;
                if (submitbtn.IsVisible == true)
                {
                    NavigationStack.IsVisible = true;
                    submitbtn.IsVisible = false;
                }
            }
            //var userquestionanswer = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == QuestionsInOrder[0].questionid).FirstOrDefault();
            foreach (var item in QuestionsInOrder)
            {
                if (answerid.Count != 0)
                {
                    item.Selectedansweridlist = answerid;
                    item.answerid = answerid[0];

                    var selectedanswer = item.AnswerOptions.Where(x => x.answerid == item.answerid).FirstOrDefault();

                    var selectedIndex = item.AnswerOptions.IndexOf(selectedanswer);

                    if (selectedIndex != -1)
                    {
                        //Missing .selectedss and check for each item
                        if (item.singleselection == true)
                        {
                            item.AnswerOptions[selectedIndex].selectedss = true;

                        }
                        if (item.mulitpleselection == true)
                        {
                            item.AnswerOptions[selectedIndex].selectedms = true;
                        }
                    }
                }


                if (!string.IsNullOrEmpty(item.selectedtextvalue))
                {
                    item.freetextentry = item.selectedtextvalue.TrimEnd();
                }

                if (item.Sliderscale == true)
                {
                    if (item.SliderValue == null || item.SliderValue == 0)
                    {
                        //if(ValueofSlider != 0)
                        //{
                        //    item.SliderValue = ValueofSlider; 
                        //}
                    }
                    else
                    {
                        //Do Nothing
                    }
                    //item.slidervalue = 
                }
            }


            //foreach (var it in userquestionanswer.answer)
            //{
            //    var selectedanswer = item.AnswerOptions.Where(x => x.answerid == it.answerid).FirstOrDefault();

            //    var selectedIndex = item.AnswerOptions.IndexOf(selectedanswer);

            //    item.AnswerOptions[selectedIndex].selectedms = true;



            //}

            //    if (item.questionanswers.Any(x => x.answeroptions == "specifyfreetext"))
            //    {
            //        //item.Addfreetext = true;
            //        //item.Addfreetextopacity = 0.5;
            //        //item.Addfreetextenabled = false;
            //    }
            //}
            //    else if (item.questiontype == "freetext")
            //{
            //    item.answerid = item.questionanswers[0].answerid;
            //    item.Addfreetext = true;
            //    item.Addfreetextenabled = true;
            //    item.Addfreetextopacity = 1;

            //    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
            //    {
            //        item.freetextentry = userquestionanswer.answer[0].answervalue;
            //    }

            //}
            //else if (item.questiontype == "numeric")
            //{
            //    item.answerid = item.questionanswers[0].answerid;
            //    item.Numericentry = true;

            //    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
            //    {
            //        item.numericentrytext = userquestionanswer.answer[0].answervalue;
            //    }

            //}
            //else if (item.questiontype == "doublenumeric")
            //{
            //    item.answerid = item.questionanswers[0].answerid;
            //    item.Doublenumentry = true;

            //    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
            //    {
            //        var splitnum = userquestionanswer.answer[0].answervalue.Split('|');

            //        item.doubleentry1 = splitnum[0];
            //        item.doubleentry2 = splitnum[1];
            //    }

            //}
            //else if (item.questiontype == "scale110singleselection")
            //{
            //    item.answerid = item.questionanswers[0].answerid;

            //    item.Sliderscale = true;

            //    item.answertitle = item.questionanswers[0].answertitle;

            //    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
            //    {
            //        var convertnum = Convert.ToInt32(userquestionanswer.answer[0].answervalue);

            //        item.slidervalue = convertnum;
            //    }

            //}

            if (OrderNumber.ToString() == "1")
            {
                backbtn.IsVisible = false;
            }

            // mainquestionnaire.ItemsSource = null;



            mainquestionnaire.ItemsSource = QuestionsInOrder;



        }
        catch (Exception Ex)
        {

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
                submitbtn.IsEnabled = false;
                //update the slider questions and answers
                foreach (var item in questionnairefromlist.questionanswerjsonlist)
                {
                    if (item.questiontype == "scale110singleselection")
                    {
                        //do nothing as 1 is already set
                        item.Hasanswered = true;
                        //check if they have moved it, if its null or empty add the 1 as default
                        if (string.IsNullOrEmpty(item.selectedtextvalue))
                        {
                            item.selectedtextvalue = "1";

                        }

                    }
                }

                //check if all questions are answered
                var countofmissedquestions = 0;

                foreach (var item in questionnairefromlist.questionanswerjsonlist)
                {

                    if (item.questionrequired)
                    {


                        if (item.Hasanswered == false)
                        {
                            item.Bordercolor = Colors.Red;
                            item.Isrequired = true;
                            countofmissedquestions++;
                        }

                    }
                }

                if (countofmissedquestions > 0)
                {
                    Vibration.Vibrate();
                    submitbtn.IsEnabled = true;
                    return;
                }


                //add the questions
                var collection = new ObservableCollection<userquestionnaire>();

                var newitem = new userquestionnaire();

                newitem.questionnaireid = questionnairefromlist.questionnaireid;

                newitem.userid = Helpers.Settings.UserKey;

                newitem.completedatetime = DateTime.Now.ToString("HH:mm dd/MM/yyyy");

                var newuserresponse = new ObservableCollection<userquestionnaireresponse>();



                foreach (var item in questionnairefromlist.questionanswerjsonlist)
                {

                    if (item.Hasanswered)
                    {
                        var userresponse = new userquestionnaireresponse();

                        userresponse.questionid = item.questionid;

                        //add answers
                        var useranswers = new ObservableCollection<answers>();

                        if (item.Selectedansweridlist.Count == 0)
                        {
                            item.Selectedansweridlist.Add(item.questionanswers[0].answerid);
                        }

                        for (int i = 0; i < item.Selectedansweridlist.Count; i++)
                        {
                            var newanswer = new answers();
                            newanswer.answerid = item.Selectedansweridlist[i];

                            if (item.questiontype == "multipleselection")
                            {
                                var getansweroptions = item.questionanswers.FirstOrDefault(x => x.answerid == item.Selectedansweridlist[i]);

                                if (getansweroptions != null && !string.IsNullOrEmpty(getansweroptions.answeroptions))
                                {
                                    if (getansweroptions.answeroptions == "specifyfreetext")
                                    {
                                        if (!string.IsNullOrEmpty(item.selectedtextvalue))
                                        {
                                            if (item.Addfreetextenabled)
                                            {
                                                newanswer.answervalue = item.selectedtextvalue.TrimEnd();
                                            }
                                        }
                                    }
                                }
                            }
                            else if (item.questiontype == "freetext" || item.questiontype == "numeric")
                            {
                                if (!string.IsNullOrEmpty(item.selectedtextvalue))
                                {
                                    newanswer.answervalue = item.selectedtextvalue.TrimEnd();
                                }
                            }
                            else if (item.questiontype == "doublenumeric")
                            {
                                newanswer.answervalue = item.doubleentryone.TrimEnd() + "|" + item.doubleentrytwo.TrimEnd();
                            }
                            else if (item.questiontype == "singleselection")
                            {
                                if (!string.IsNullOrEmpty(item.selectedtextvalue))
                                {
                                    if (item.Addfreetextenabled)
                                    {
                                        newanswer.answervalue = item.selectedtextvalue.TrimEnd();
                                    }
                                }
                            }
                            else if (item.questiontype == "scale110singleselection")
                            {
                                if (item.SliderValue == 0)
                                {
                                    item.SliderValue = 1;
                                }
                                newanswer.answervalue = item.SliderValue.ToString();
                            }


                            useranswers.Add(newanswer);
                        }

                        userresponse.answer = useranswers;

                        newuserresponse.Add(userresponse);

                    }


                }


                string jsonString = JsonConvert.SerializeObject(newuserresponse);

                newitem.questionanswerjson = jsonString;

                var uq = await aPICalls.PostUserQuestionnaire(newitem);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Questionnaire Submitted") { });
                await Task.Delay(1500);

                submitbtn.IsEnabled = true;

                //Removed to also update From Normal navigation
                //if (fromdash)
                //{

                var newfd = new feedbackdata();

                newfd.datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                newfd.action = "Completed Questionnaire";
                newfd.label = questionnairetitlelbl.Text;

                //add questionnaire details in feedback data

                if (userfeedbacklistpassed.initialquestionnairefeedbacklist == null)
                {
                    userfeedbacklistpassed.initialquestionnairefeedbacklist = new ObservableCollection<feedbackdata>();
                }

                //Only add the first instance of the questionnaire to remove the prompt on dash 
                bool exists = userfeedbacklistpassed.initialquestionnairefeedbacklist.Any(item => item.label == newfd.label);

                if (!exists)
                {
                    var Signup = Helpers.Settings.SignUp;
                    var ListArray = new List<string>();
                    if (!string.IsNullOrEmpty(Signup))
                    {
                        if (Signup.Contains("SFEWH"))
                        {
                            ListArray = new List<string>
                        {   //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Diabetes ID
                            "73D47262-1B2C-4451-A4FC-978582D77FE0"
                        };
                        }
                        else if (Signup.Contains("SFEWHA2"))
                        {
                            ListArray = new List<string>
                        {   //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Breast cancer ID
                            "F7FB770B-286F-4300-814D-E76AACB6DACF"
                        };
                        }
                        else if (Signup.Contains("SFEWHA3"))
                        {
                            ListArray = new List<string>
                        {    //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Stress ID
                            "4A27076A-A2A3-45DD-A061-34A4F1799B20"
                        };
                        }
                        else if (Signup.Contains("SFEWHA4"))
                        {
                            ListArray = new List<string>
                        {    //SF36 ID 
                            "DC6A9FD7-242B-4299-9672-D745669FEAF0",
                            //Stress ID
                            "4A27076A-A2A3-45DD-A061-34A4F1799B20"
                        };
                        }
                        else if (Signup.Contains("SFECORE"))
                        {
                            ListArray = new List<string>
                        {    //BODY-Q-Health-related
                            "9CC7F59A-8F48-41BC-BD5D-BA5B83A81BEE"
                        };
                        }
                        else if (Signup.Contains("RBHTHCM"))
                        {
                            ListArray = new List<string>
                        {    //HOCM Baseline 
                            "BE72B2A1-0707-4E8D-8E82-022BA4F959F4"
                        };
                        }
                        else if (Signup.Contains("SFEAT"))
                        {
                            ListArray = new List<string>
                         {    //EQ-5D 5L
                             "A37CF880-080D-40D4-8A8D-1C0CEEC2FEBF"
                         };
                        }

                        //only adds the required questionnaires to the Feedback 
                        bool CheckSame = ListArray.Any(L => L == newitem.questionnaireid);

                        if (CheckSame)
                        {
                            userfeedbacklistpassed.initialquestionnairefeedbacklist.Add(newfd);

                            string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.initialquestionnairefeedbacklist);
                            userfeedbacklistpassed.initialquestionnairefeedback = newsymJson;

                            await aPICalls.UserfeedbackUpdateQuestionnaireData(userfeedbacklistpassed);
                        }
                    }
                }

                //}
                //   alluserquestionnaires.Add(newitem);

                await Navigation.PushAsync(new AllQuestionnaires(userfeedbacklistpassed), false);

                await MopupService.Instance.PopAllAsync(false);

                var pageToRemoves = Navigation.NavigationStack.ToList();

                int ii = 0;

                foreach (var page in pageToRemoves)
                {
                    if (ii == 0)
                    {
                    }
                    else if (ii == 1 || ii == 2 || ii == 3)
                    {
                        Navigation.RemovePage(page);
                    }
                    else
                    {
                        //Navigation.RemovePage(page);
                    }
                    ii++;
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

    private void ExtendedEditor_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            //New
            if (e.NewTextValue == null) return;

            var Item = (ExtendedEditor)sender;

            if (Item.BindingContext is questionanswerinfo getitem)
            {
                if (!getitem.Addfreetext) return;

                getitem.selectedtextvalue = e.NewTextValue;
                getitem.numericentrytext = e.NewTextValue;

                if (!string.IsNullOrEmpty(e.NewTextValue))
                {
                    getitem.Bordercolor = Colors.White;
                    getitem.Isrequired = false;
                    getitem.Hasanswered = true;
                }
                else
                {
                    getitem.Hasanswered = false;
                    getitem.Isrequired = false;
                    getitem.selectedtextvalue = string.Empty;
                }
            }

            //Old 
            //var item = (ExtendedEditor)sender;

            //if (e.NewTextValue == null) return;

            //var getitem = questionnairefromlist.questionanswerjsonlist.FirstOrDefault(x => x.questionid == item.questionid);

            //if (getitem != null)
            //    {
            //        if (!getitem.Addfreetext) return;

            //        getitem.selectedtextvalue = e.NewTextValue;
            //        getitem.numericentrytext = e.NewTextValue;

            //        if (!string.IsNullOrEmpty(e.NewTextValue))
            //        {
            //            getitem.Bordercolor = Colors.White;
            //            getitem.Isrequired = false;
            //            getitem.Hasanswered = true;
            //        }
            //        else
            //        {
            //            getitem.Hasanswered = false;
            //            getitem.Isrequired = false;
            //            getitem.selectedtextvalue = string.Empty;
            //            //getitem.numericentrytext = string.Empty;
            //        }
            //    }                     
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //double numeric entry , first entry

            //New
            if (e.NewTextValue == null) return;

            var Item = (ExtendedEntry)sender;

            if (Item.BindingContext is questionanswerinfo getitem)
            {
                if (!getitem.Doublenumentry) return;

                getitem.doubleentryone = e.NewTextValue.ToString();

                if (!string.IsNullOrEmpty(e.NewTextValue))
                {
                    getitem.Bordercolor = Colors.White;
                    getitem.Isrequired = false;
                    getitem.Answerednumericentryone = true;

                    if (getitem.Answerednumericentryone == true && getitem.Answerednumericentrytwo == true)
                    {
                        getitem.Hasanswered = true;
                    }
                }
                else
                {
                    getitem.Answerednumericentryone = false;
                    getitem.Hasanswered = false;
                    getitem.doubleentryone = string.Empty;
                }
            }

            //Old
            //var item = (ExtendedEntry)sender;

            //if (e.NewTextValue == null) return;

            //var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

            //if (getitem != null)
            //{
            //    getitem.doubleentryone = e.NewTextValue.ToString();

            //    if (!string.IsNullOrEmpty(e.NewTextValue))
            //    {
            //        getitem.Bordercolor = Colors.White;
            //        getitem.Isrequired = false;
            //        getitem.Answerednumericentryone = true;

            //        if (getitem.Answerednumericentryone == true && getitem.Answerednumericentrytwo == true)
            //        {
            //            getitem.Hasanswered = true;
            //        }
            //    }
            //    else
            //    {
            //        getitem.Answerednumericentryone = false;
            //        getitem.Hasanswered = false;
            //        getitem.doubleentryone = string.Empty;
            //    }
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedEntry_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        try
        {
            //double numeric entry , second entry
            //New
            if (e.NewTextValue == null) return;

            var Item = (ExtendedEntry)sender;

            if (Item.BindingContext is questionanswerinfo getitem)
            {
                if (!getitem.Doublenumentry) return;

                getitem.doubleentrytwo = e.NewTextValue.ToString();

                if (!string.IsNullOrEmpty(e.NewTextValue))
                {

                    getitem.Bordercolor = Colors.White;
                    getitem.Isrequired = false;
                    getitem.Answerednumericentrytwo = true;

                    if (getitem.Answerednumericentryone == true && getitem.Answerednumericentrytwo == true)
                    {
                        getitem.Hasanswered = true;
                    }
                }
                else
                {
                    getitem.Answerednumericentrytwo = false;
                    getitem.Hasanswered = false;
                    getitem.doubleentrytwo = string.Empty;
                }
            }

            //Old
            //var item = (ExtendedEntry)sender;

            //if (e.NewTextValue == null) return;

            //var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

            //if (getitem != null)
            //{
            //    getitem.doubleentrytwo = e.NewTextValue.ToString();

            //    if (!string.IsNullOrEmpty(e.NewTextValue))
            //    {

            //        getitem.Bordercolor = Colors.White;
            //        getitem.Isrequired = false;
            //        getitem.Answerednumericentrytwo = true;

            //        if (getitem.Answerednumericentryone == true && getitem.Answerednumericentrytwo == true)
            //        {
            //            getitem.Hasanswered = true;
            //        }
            //    }
            //    else
            //    {
            //        getitem.Answerednumericentrytwo = false;
            //        getitem.Hasanswered = false;
            //        getitem.doubleentrytwo = string.Empty;
            //    }
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedEntry_TextChanged_2(object sender, TextChangedEventArgs e)
    {
        try
        {
            //numeric entry
            //New
            if (e.NewTextValue == null) return;

            var Item = (ExtendedEntry)sender;

            if (Item.BindingContext is questionanswerinfo getitem)
            {
                if (!getitem.Numericentry) return;

                getitem.selectedtextvalue = e.NewTextValue;
                getitem.numericentrytext = e.NewTextValue;

                if (!string.IsNullOrEmpty(e.NewTextValue))
                {
                    getitem.Bordercolor = Colors.White;
                    getitem.Isrequired = false;
                    getitem.Hasanswered = true;
                }
                else
                {
                    getitem.Hasanswered = false;
                    getitem.Isrequired = false;
                    getitem.selectedtextvalue = string.Empty;
                    //getitem.numericentrytext = string.Empty;
                }
            }

            //Old
            //var item = (ExtendedEntry)sender;

            //if (e.NewTextValue == null) return;

            //var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

            //if (getitem != null)
            //{
            //    if (getitem.Numericentry == false) return;

            //    getitem.selectedtextvalue = e.NewTextValue;
            //    getitem.numericentrytext = e.NewTextValue;

            //    if (!string.IsNullOrEmpty(e.NewTextValue))
            //    {
            //        getitem.Bordercolor = Colors.White;
            //        getitem.Isrequired = false;
            //        getitem.Hasanswered = true;
            //    }
            //    else
            //    {
            //        getitem.Hasanswered = false;
            //        getitem.Isrequired = false;
            //        getitem.selectedtextvalue = string.Empty;
            //        //getitem.numericentrytext = string.Empty;
            //    }
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        try
        {

            //slider changed 
            //New

            var Item = (ExtendedSlider)sender;

            if (Item.BindingContext is questionanswerinfo getitem)
            {
                if (!getitem.Sliderscale) return;

                if (getitem.questiontype == "scale110singleselection")
                {

                    if (getitem.SliderValue != 0)
                    {
                        //getitem.selectedtextvalue = QuestionsInOrder[0].selectedtextvalue;
                        //getitem.SliderValue = QuestionsInOrder[0].slidervalue; 
                    }
                    else
                    {
                        getitem.selectedtextvalue = e.NewValue.ToString();
                        getitem.SliderValue = e.NewValue;

                    }

                    if (!string.IsNullOrEmpty(e.NewValue.ToString()))
                    {
                        getitem.Bordercolor = Colors.White;
                        getitem.Isrequired = false;
                        getitem.Hasanswered = true;

                    }
                    else
                    {
                        getitem.Hasanswered = false;
                    }
                }
            }


            //Old 
            //var item = (ExtendedSlider)sender;

            //var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

            //if (getitem != null)
            //{
            //    if (getitem.questiontype == "scale110singleselection")
            //    {

            //        if (getitem.SliderValue != 0)
            //        {
            //            //getitem.selectedtextvalue = QuestionsInOrder[0].selectedtextvalue;
            //            //getitem.SliderValue = QuestionsInOrder[0].slidervalue; 
            //        }
            //        else
            //        {
            //            getitem.selectedtextvalue = e.NewValue.ToString();
            //            getitem.SliderValue = e.NewValue;

            //        }



            //        if (!string.IsNullOrEmpty(e.NewValue.ToString()))
            //        {
            //            getitem.Bordercolor = Colors.White;
            //            getitem.Isrequired = false;
            //            getitem.Hasanswered = true;

            //        }
            //        else
            //        {
            //            getitem.Hasanswered = false;
            //        }
            //    }
            //}
        }
        catch (Exception Ex)
        {

        }
    }

    private void completedquestionnaire_SizeChanged(object sender, EventArgs e)
    {
        try
        {
            var viewCell = sender as View;
            if (viewCell != null)
            {
                double itemHeight = viewCell.Height;
                //Comment out for now 
                //completedquestionnaire.HeightRequest = itemHeight;
            }
        }
        catch (Exception Ex)
        {

        }

    }

    private void mainquestionnaire_SizeChanged(object sender, EventArgs e)
    {
        try
        {
            var viewCell = sender as View;
            if (viewCell != null)
            {
                double itemHeight = viewCell.Height;
                //Comment out for now 
                //mainquestionnaire.HeightRequest = itemHeight;
            }
        }
        catch (Exception Ex)
        {

        }
    }

    private void ExtendedSFRadioButton_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //single selection
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {


                if (e.IsChecked == true)
                {
                    var item = (ExtendedSFRadioButton)sender;

                    if (item != null)
                    {
                        var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

                        if (getitem == null)
                        {
                            //check on iOS
                            return;
                            getitem = QuestionsInOrder[0];
                            item.IDValue = QuestionsInOrder[0].answerid;
                            item.IDRecord = RecordID;
                        }

                        getitem.Bordercolor = Colors.White;


                        getitem.Isrequired = false;

                        //clear list every time as only one allowed
                        getitem.Selectedansweridlist.Clear();

                        if (!getitem.Selectedansweridlist.Contains(item.IDValue))
                        {
                            getitem.Selectedansweridlist.Add(item.IDValue);
                        }

                        if (item.IDRecord == "specifyfreetext")
                        {
                            getitem.Addfreetextenabled = true;
                            getitem.Addfreetextopacity = 1;

                            if (getitem.freetextentry == null)
                            {
                                getitem.Hasanswered = false;
                            }
                            else
                            {
                                getitem.Hasanswered = true;
                            }
                        }
                        else
                        {
                            getitem.Addfreetextenabled = false;
                            getitem.Addfreetextopacity = 0.5;

                            //this is an option like yes, no etc with no extra text added
                            getitem.Hasanswered = true;
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
}