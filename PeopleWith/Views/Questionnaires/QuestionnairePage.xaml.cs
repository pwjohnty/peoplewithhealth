
//using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Newtonsoft.Json;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using Mopups.Services;

namespace PeopleWith;

public partial class QuestionnairePage : ContentPage
{

    public questionnaire questionnairefromlist = new questionnaire();
    public int rownumber = 0;
    APICalls aPICalls = new APICalls();
    bool completeduserquestionnaire;
    public userquestionnaire completeuserquestionnaire = new userquestionnaire();
    public ObservableCollection<userquestionnaire> alluserquestionnaires = new ObservableCollection<userquestionnaire>();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

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

    public QuestionnairePage()
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

    public QuestionnairePage(string questionnaireid)
    {
        try
        {
            //from notification tap

            InitializeComponent();


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
            loadingstack.IsVisible = true;

            var uq = await aPICalls.GetSingleQuestionnaire(questionnaireid);



            if (uq != null)
            {

                questionnairefromlist = uq[0];

            

                questionnairetitlelbl.Text = questionnairefromlist.title;

                questionnairedeslbl.Text = questionnairefromlist.description;

                //alluserquestionnaires = alluserquestionnairespassed;

                populatequestionnaire();
            }

        }
        catch (Exception Ex)
        {
        }
    }

    public QuestionnairePage(questionnaire questionnairepassed)
    {
        try
        {
            InitializeComponent();

            questionnairefromlist = questionnairepassed;

            loadingstack.IsVisible = true;

            questionnairetitlelbl.Text = questionnairefromlist.title;

            questionnairedeslbl.Text = questionnairefromlist.description;

            //alluserquestionnaires = alluserquestionnairespassed;

            populatequestionnaire();

            //  mainquestionnaire.ItemsSource = questionnairepassed;

            // mainquestionnaire.HeightRequest = 1000;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }


    public QuestionnairePage(userquestionnaire userquestionnairepassed, questionnaire questionnairepassed)
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

            foreach (var item in questionnairefromlist.questionanswerjsonlist)
            {
                var userquestionanswer = completeuserquestionnaire.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

                item.Addfreetext = false;
                item.Doublenumentry = false;
                item.Numericentry = false;
                item.Sliderscale = false;
                item.Isrequired = false;
                item.Bordercolor = Colors.White;

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
                            isVisible = true  // Set to true since it's visible in this case
                        });

                        

                    }

                    //find out the selected item
                    

                    var selectedanswer = item.AnswerOptions.Where(x => x.answerid == userquestionanswer.answer[0].answerid).FirstOrDefault();

                    var selectedIndex = item.AnswerOptions.IndexOf(selectedanswer);

                    item.AnswerOptions[selectedIndex].selectedss = true;

                    if (!string.IsNullOrEmpty(userquestionanswer.answer[0].answervalue))
                    {
                        item.freetextentry = userquestionanswer.answer[0].answervalue;
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
                            answertitle = answer.answertitle,
                            isVisible = true  // Set to true since it's visible in this case
                        });
                    }



                    foreach (var it in userquestionanswer.answer)
                    {
                        var selectedanswer = item.AnswerOptions.Where(x => x.answerid == it.answerid).FirstOrDefault();

                        var selectedIndex = item.AnswerOptions.IndexOf(selectedanswer);

                        item.AnswerOptions[selectedIndex].selectedms = true;

                        if (!string.IsNullOrEmpty(it.answervalue))
                        {
                            item.freetextentry = it.answervalue;
                        }

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
                        var convertnum = Convert.ToInt32(userquestionanswer.answer[0].answervalue);

                        item.slidervalue = convertnum;
                    }

                }



            }

            if (completeduserquestionnaire)
            {
                completedquestionnaire.IsVisible = true;
                completedquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;

            }
            else
            {
                mainquestionnaire.IsVisible = true;

                mainquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;
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

    async void populatequestionnaire()
    {
        try
        {
            
            foreach(var item in questionnairefromlist.questionanswerjsonlist)
            {
                item.Addfreetext = false;
                item.Doublenumentry = false;
                item.Numericentry = false;
                item.Sliderscale = false;
                item.Isrequired = false;
                item.Bordercolor = Colors.White;

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
                            isVisible = true  // Set to true since it's visible in this case
                        });

                    }


                    if(item.questionanswers.Any(x => x.answeroptions == "specifyfreetext"))
                    {
                        item.Addfreetext = true;
                        item.Addfreetextopacity = 0.5;
                        item.Addfreetextenabled = false;
                    }


                }
                else if(item.questiontype == "multipleselection")
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
                else if(item.questiontype == "freetext")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Addfreetext = true;
                    item.Addfreetextenabled = true;
                    item.Addfreetextopacity = 1;
                }
                else if(item.questiontype == "numeric")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Numericentry = true;
                }
                else if(item.questiontype == "doublenumeric")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    item.Doublenumentry = true;
                }
                else if(item.questiontype == "scale110singleselection")
                {
                    item.answerid = item.questionanswers[0].answerid;
                    
                    item.Sliderscale = true;

                    item.answertitle = item.questionanswers[0].answertitle;
                }
            }

            if (completeduserquestionnaire)
            {
                completedquestionnaire.IsVisible = true;
                completedquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;

            }
            else
            {
                mainquestionnaire.IsVisible = true;

                mainquestionnaire.ItemsSource = questionnairefromlist.questionanswerjsonlist;
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

            if(e.IsChecked == true)
            {
                var item = (ExtendedRadioButton)sender;

                if (item != null)
                {
                    var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();


                    if(item.IDRecord == "specifyfreetext")
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

            getitem.Bordercolor = Colors.White;
            getitem.Isrequired = false;
            

            if (e.IsChecked == true)
            {
                if (!getitem.Selectedansweridlist.Contains(item.IDValue))
                {
                    getitem.Selectedansweridlist.Add(item.IDValue);
                }

                if (item != null)
                {
               
                    if (item.IDRecord == "specifyfreetext")
                    {
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

            if (e.Value == true)
            {
                var item = (ExtendedRadioButton)sender;

                if (item != null)
                {
                    var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

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

                        getitem.Hasanswered = false;
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
                                var getansweroptions = item.questionanswers.Where(x => x.answerid == item.Selectedansweridlist[i]).FirstOrDefault();

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
                                newanswer.answervalue = item.selectedtextvalue;
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

                //   alluserquestionnaires.Add(newitem);

                await Navigation.PushAsync(new AllQuestionnaires(), false);

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
            var item = (ExtendedEditor)sender;

            var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();


            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
              
                getitem.Bordercolor = Colors.White;
                getitem.Isrequired = false;
                getitem.Hasanswered = true;

                getitem.selectedtextvalue = e.NewTextValue;

            }
            else
            {
                getitem.Hasanswered = false;

                getitem.selectedtextvalue = string.Empty;
            }

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

            var item = (ExtendedEntry)sender;

            var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();


            if (!string.IsNullOrEmpty(e.NewTextValue))
            {
                
                getitem.Bordercolor = Colors.White;
                getitem.Isrequired = false;
                getitem.Answerednumericentryone = true;

                getitem.doubleentryone = e.NewTextValue.ToString();

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

            var item = (ExtendedEntry)sender;

            var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();


            if (!string.IsNullOrEmpty(e.NewTextValue))
            {

                getitem.Bordercolor = Colors.White;
                getitem.Isrequired = false;
                getitem.Answerednumericentrytwo = true;
                getitem.doubleentrytwo = e.NewTextValue.ToString();

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
            var item = (ExtendedEntry)sender;

            var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();


            if (!string.IsNullOrEmpty(e.NewTextValue))
            {

                getitem.Bordercolor = Colors.White;
                getitem.Isrequired = false;
                getitem.Hasanswered = true;

                getitem.selectedtextvalue = e.NewTextValue;


            }
            else
            {
                getitem.Hasanswered = false;
            }

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
            var item = (ExtendedSlider)sender;

            var getitem = questionnairefromlist.questionanswerjsonlist.Where(x => x.questionid == item.questionid).FirstOrDefault();

            getitem.selectedtextvalue = e.NewValue.ToString();

            //if (!string.IsNullOrEmpty(e.NewValue))
            // {

            //   getitem.Bordercolor = Colors.White;
            //   getitem.Isrequired = false;
            //   getitem.Hasanswered = true;


            // }
            // else
            // {
            //getitem.Hasanswered = false;
            // }
        }
        catch (Exception Ex)
        {
            
        }
    }
}