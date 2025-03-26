
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls.Platform;
using Mopups.Services;


namespace PeopleWith;

public partial class DashQuestionnaire : ContentPage
{
    public ObservableCollection<registryDataInputs> FilteredQuestions = new ObservableCollection<registryDataInputs>();
    public ObservableCollection<registryDataInputs> AllQuestions = new ObservableCollection<registryDataInputs>();
    public List<string> MHq1 = new List<string>();
    public List<string> MHq2 = new List<string>();

    public string MC1yesanswerss;
    public List<string> MCq1 = new List<string>();
    public string MC2 = "";
    public string MC3 = "";
    public string MC31 = "";
    public string MC32 = "";
    public string MC33 = "";
    public string MC34 = "";
    public string MC35 = "";

    public List<string> Tq1 = new List<string>();
    public List<string> Tq2 = new List<string>();
    public List<string> Tq3 = new List<string>();
    public string Tq4 = "";
    public List<string> Tq41 = new List<string>();
    public string Tq5 = "";
    public string Tq6 = "";
    public string Tq8 = "";
    public string Tq81 = "";
    public List<string> Tq82 = new List<string>();
    public string Tq10 = "";
    public string Tq101 = "";
    public string Tq11 = "";
    public string Tq112 = "";
    public string Tq12 = "";
    public string Tq121 = "";
    public string Tq122 = "";
    public string Tq13 = "";
    public string Tq131 = "";
    public string Tq132 = "";
    public HttpClient Client = new HttpClient();

    public DashQuestionnaire()
	{
		InitializeComponent();
	}

    private void ConfigureClient()
    {
        try
        {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
            Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
        }
        catch (Exception Ex)
        {
            //Empty
        }
    }


    public DashQuestionnaire(string input)
    {
        InitializeComponent();

        getquestions(input);

        if (input == "Treatment")
        {
            titlelbl.Text = "Treatment Information";
        }
        else
        {

            titlelbl.Text = input;
        }

    }

    public DashQuestionnaire(string input, string list)
    {
        InitializeComponent();

        titlelbl.Text = input;

        sublabel.IsVisible = false;

        getquestionsandresponses();



    }

    async void getquestionsandresponses()
    {
        try
        {
            APICalls database = new APICalls();

            var getquestions = database.GetWHfromregistrydatainput("WHA101");


            var questionslist = await getquestions;


            foreach (var item in questionslist)
            {

                if (item != null)
                {
                    if (item.patientAccess == true)
                    {
                        AllQuestions.Add(item);
                        
                    }


                }


            }



            var getresponses = await database.GetUserResponses(Helpers.Settings.UserKey);

            var correctresponses = new ObservableCollection<userresponse>();

            foreach(var item in getresponses)
            {

                if(!string.IsNullOrEmpty(item.userquestionnaireid))
                {
                    if(item.userquestionnaireid == "Medical History" || item.userquestionnaireid == "Menstural Cycle" || item.userquestionnaireid == "Treatment Information")
                    {
                        correctresponses.Add(item);
                    }



                }


            }



            foreach (var item in correctresponses)
            {

                var getquestion = AllQuestions.Where(x => x.id == item.questionid).FirstOrDefault();

                if (getquestion != null)
                {

                    item.score = getquestion.label;


                    item.answerid = item.answerid + " " + item.notes;

                }

            }


            correctresponses = new ObservableCollection<userresponse>(
     correctresponses
         .OrderByDescending(x => DateTime.Parse(x.responsedate)) // Order by date first
         .GroupBy(x => x.userquestionnaireid) // Group by userquestionnaireid
         .SelectMany(group => group) // Flatten the grouped result back into a list
 );

            loadingstack.IsVisible = false;
            previousstack.IsVisible = true;
            titlelbl.IsVisible = true;
            previouslist.ItemsSource = correctresponses;
            previouslist.HeightRequest = correctresponses.Count * 100; 


            

        }
        catch(Exception ex)
        {

        }
    }


    async void getquestions(string input)
    {
        try
        {

            APICalls database = new APICalls();

            var getquestions = database.GetWHfromregistrydatainput("WHA101");


            var questionslist = await getquestions;


            foreach(var item in questionslist)
            {

                if(item != null)
                {
                    if(item.patientAccess == true)
                    {
                        if(item.dataTab == input)
                        {
                            //add item 
                            if(!string.IsNullOrEmpty(item.apporder))
                            {
                                FilteredQuestions.Add(item);
                            }
                          
                            AllQuestions.Add(item);
                        }
                    }


                }


            }


            var filteredList = FilteredQuestions
         .Where(x => !x.apporder.StartsWith("q", StringComparison.OrdinalIgnoreCase)) // Remove "If"
         .OrderBy(x => Convert.ToInt32(x.apporder))
        // .GroupBy(x => x.apporder)
         //.Select(g => g.First())
         .ToList();


            ///  var orderedlist = FilteredQuestions.OrderBy(x => Convert.ToInt32(x.apporder)).ToList();

            // mainquestionnaire.ItemsSource = filteredList;

            if (input == "Menstural Cycle")
            {
                loadingstack.IsVisible = false;
                titlelbl.IsVisible = true;
                sublabel.IsVisible = true;
                mcstack.IsVisible = true;
                //get all questions 1 details

                q1lbl.Text = filteredList[0].label;
                q1directions.Text = filteredList[0].labelDirections;
                q1answers.IsVisible = true;
                //var listof1 = filteredList[0].values.Split(',');

                // q1answers.ItemsSource = listof1;

                q2lbl.Text = filteredList[1].label;
                q2directions.Text = filteredList[1].labelDirections;
                q2answers.IsVisible = true;

                q3lbl.Text = filteredList[2].label;
                q3directions.Text = filteredList[2].labelDirections;
                q3answers.IsVisible = true;

                q4lbl.Text = filteredList[3].label;
                q4directions.Text = filteredList[3].labelDirections;
                q4entrytext.IsVisible = true;
                q4entrytextborder.IsVisible = true;

                q5lbl.Text = filteredList[4].label;
                q5directions.Text = filteredList[4].labelDirections;
                datePicker.IsVisible = true;

            }
            else if(input == "Treatment")
            {
                loadingstack.IsVisible = false;
                titlelbl.IsVisible = true;
                sublabel.IsVisible = true;
                treatmentstack.IsVisible = true;

                t1lbl.Text = filteredList[0].label;
                t1directions.Text= filteredList[0].labelDirections;

                var listof1 = filteredList[0].values.Split(',');

                 t1answers.ItemsSource = listof1;

                t1answers.IsVisible = true;


                t2lbl.Text = filteredList[1].label;
                t2directions.Text = filteredList[1].labelDirections;

                var listof2 = filteredList[1].values.Split(',');

                t2answers.ItemsSource = listof1;

                t2answers.IsVisible = true;

                t3lbl.Text = filteredList[2].label;
                t3directions.Text = filteredList[2].labelDirections;

                var listof3 = filteredList[2].values.Split(',');

                t3answers.ItemsSource = listof3;

                t3answers.IsVisible = true;

                t4lbl.Text = filteredList[3].label;
                t4directions.Text = filteredList[3].labelDirections;

                //var listof3 = filteredList[2].values.Split(',');

                //t3answers.ItemsSource = listof1;

                t4answers.IsVisible = true;

                t5lbl.Text = filteredList[4].label;
                t5directions.Text = filteredList[4].labelDirections;

                //var listof3 = filteredList[2].values.Split(',');

                //t3answers.ItemsSource = listof1;

                t5answers.IsVisible = true;

                t6lbl.Text = filteredList[5].label;
                t6directions.Text = filteredList[5].labelDirections;

                var listof6 = filteredList[5].values.Split(',');

                t6answersyes.ItemsSource = listof6;
                t6answersyes.IsVisible = true;

                t7lbl.Text = filteredList[6].label;
                t7directions.Text = filteredList[6].labelDirections;


                t8lbl.Text = filteredList[7].label;
                t8directions.Text = filteredList[7].labelDirections;

                var listof8 = filteredList[7].values.Split(',');

                t8answers.ItemsSource = listof8;

                t8answers.IsVisible = true;

                t9lbl.Text = filteredList[8].label;
                t9directions.Text = filteredList[8].labelDirections;


                t10lbl.Text = filteredList[9].label;
                t10directions.Text = filteredList[9].labelDirections;

                var listof10 = filteredList[9].values.Split(',');

                t10answers.ItemsSource = listof10;

                t10answers.IsVisible = true;


                t11lbl.Text = filteredList[10].label;
                t11directions.Text = filteredList[10].labelDirections;

                var listof11 = filteredList[10].values.Split(',');

                t11answers.ItemsSource = listof11;

                t11answers.IsVisible = true;


                t12lbl.Text = filteredList[11].label;
                t12directions.Text = filteredList[11].labelDirections;

                var listof12 = filteredList[11].values.Split(',');

                t12answers.ItemsSource = listof12;

                t12answers.IsVisible = true;

                t13lbl.Text = filteredList[12].label;
                t13directions.Text = filteredList[12].labelDirections;

                var listof13 = filteredList[12].values.Split(',');

                t13answers.ItemsSource = listof13;

                t13answers.IsVisible = true;

            }
            else if(input == "Medical History")
            {
                loadingstack.IsVisible = false;
                titlelbl.IsVisible = true;
                sublabel.IsVisible = true;
                familyhistorystack.IsVisible = true;


                f1lbl.Text = filteredList[0].label;
                f1directions.Text = filteredList[0].labelDirections;

                var listof1 = filteredList[0].values.Split(',');

                f1answers.ItemsSource = listof1;

                f1answers.IsVisible = true;


                f2lbl.Text = filteredList[1].label;
                f2directions.Text = filteredList[1].labelDirections;

                var listof2 = filteredList[1].values.Split(',');

                f2answers.ItemsSource = listof2;

                f2answers.IsVisible = true;


                f3lbl.Text = filteredList[2].label;
                f3directions.Text = filteredList[2].labelDirections;

            }


            // submitbtn.IsVisible = true;
            nextbtn.IsVisible = true;

        }
        catch (Exception ex)
        {

            var s = ex.StackTrace.ToString();

        }
    }

    private async void mainquestionnaire_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        try
        {

            var item = e.Item as registryDataInputs;

            var filteredList = FilteredQuestions
          .Where(x => x.itemOrder == item.itemOrder)  // Use "==" for comparison
                                                      //.OrderBy(x => Convert.ToInt32(x.apporder))  // Uncomment if needed
         // .GroupBy(x => x.itemOrder)
          //.Select(g => g.First())  // Uncomment if you only want the first item from each group
          .ToList();

            await Navigation.PushAsync(new SingleQuestion(filteredList), false);
        }
        catch(Exception ex)
        {

        }
    }

    private async void ExtendedRadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            //no q1
            if (e.Value)
            {
                q1yeslbl1.IsVisible = false;
                q1yesdirections1.IsVisible = false;
                q1yes1answers.IsVisible = false;
                q1yeslbl2.IsVisible = false;
                q1yesdirections2.IsVisible = false;
                q1yes2answers.IsVisible = false;

                var getquestion = AllQuestions.Where(x => x.apporder == "q1no").FirstOrDefault();

                if (getquestion != null)
                {
                    q1nolbl.Text = getquestion.label;
                    qno1directions.Text = getquestion.labelDirections;

                    q1nolbl.IsVisible = true;
                    qno1directions.IsVisible = true;
                    q1noanswers.IsVisible = true;
                }
            }
            else
            {
                q1nolbl.IsVisible = false;
                qno1directions.IsVisible = false;
                q1noanswers.IsVisible = false;
            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            //yes q1
            if (e.Value)
            {

                q1nolbl.IsVisible = false;
                qno1directions.IsVisible = false;
                q1noanswers.IsVisible = false;



                var getquestion = AllQuestions.Where(x => x.apporder.Contains("q1yes")).ToList();

                if (getquestion != null)
                {
                    var getquestion1 = getquestion.Where(x => x.apporder == "q1yes1").FirstOrDefault();

                    if (getquestion1 != null)
                    {

                        q1yeslbl1.Text = getquestion1.label;
                        q1yesdirections1.Text = getquestion1.labelDirections;

                        var split = getquestion1.values.Split(',');

                        if(split != null)
                        {
                            q1yes1answers.ItemsSource = split;
                        }

                    }

                    var getquestion2 = getquestion.Where(x => x.apporder == "q1yes2").FirstOrDefault();

                    if (getquestion2 != null)
                    {

                        q1yeslbl2.Text = getquestion2.label;
                        q1yesdirections2.Text = getquestion2.labelDirections;

                        var splitanswers = getquestion2.values.Split(',');

                        if (splitanswers != null)
                        {
                            q1yes2answers.ItemsSource = splitanswers;
                        }
                    }


                    q1yeslbl1.IsVisible = true;
                    q1yesdirections1.IsVisible = true;
                    q1yes1answers.IsVisible = true;
                    q1yeslbl2.IsVisible = true;
                    q1yesdirections2.IsVisible = true;
                    q1yes2answers.IsVisible = true;
                    
                }



            }
            else
            {

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_2(object sender, CheckedChangedEventArgs e)
    {
        //q2 yes

        try
        {
            if (e.Value)
            {

                q2othertext.Text = string.Empty;

                var getquestion = AllQuestions.Where(x => x.apporder == "q2yes").FirstOrDefault();

                if (getquestion != null)
                {

                    q2lblyes.Text = getquestion.label;
                    q2directionsyes.Text = getquestion.labelDirections;


                    var split = getquestion.values.Split(',');

                    q2bothanswers.ItemsSource = split;

                }

                q2lblyes.IsVisible = true;
                q2directionsyes.IsVisible = true;
                q2bothanswers.IsVisible = true;
                q2othertext.IsVisible = false;
                q2othertextborder.IsVisible = false;

      

            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_3(object sender, CheckedChangedEventArgs e)
    {
        //q2 no

        try
        {
            if (e.Value)
            {
                q2othertext.Text = string.Empty;

                var getquestion = AllQuestions.Where(x => x.apporder == "q2no").FirstOrDefault();

                if (getquestion != null)
                {

                    q2lblyes.Text = getquestion.label;
                    q2directionsyes.Text = getquestion.labelDirections;


                    var split = getquestion.values.Split(',');

                    q2bothanswers.ItemsSource = split;

                }

                q2lblyes.IsVisible = true;
                q2directionsyes.IsVisible = true;
                q2bothanswers.IsVisible = true;
                q2othertext.IsVisible = false;
                q2othertextborder.IsVisible = false;
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_4(object sender, CheckedChangedEventArgs e)
    {

        //q2 other

        try
        {
            if (e.Value)
            {

                q2othertext.Text = string.Empty;

                q2lblyes.IsVisible = false;
                q2directionsyes.IsVisible = false;
                q2bothanswers.IsVisible = false;

                q2othertext.IsVisible = true;
                q2othertextborder.IsVisible = true;

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_5(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            //q2 both answers single selection 
            q2othertext.Text = string.Empty;

            var item = (ExtendedRadioButton)sender;

            if(item != null)
            {
                var text = item.Content as string;

                MC2 = text;

                if(text.Contains("Other"))
                {
                    q2othertext.IsVisible = true;
                    q2othertextborder.IsVisible = true;
                }
                else
                {
                    q2othertext.IsVisible = false;
                    q2othertextborder.IsVisible = false;
                }
            }



        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_6(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            //q3 yes

            if (e.Value)
            {

                q3lblno1.IsVisible = false;
                q3directionsno1.IsVisible = false;
                q3answersno1.IsVisible = false;

                q3lblno2.IsVisible = false;
                q3directionsno2.IsVisible = false;
                q3answersno2.IsVisible = false;

                q3lblno3.IsVisible = false;
                q3directionsno3.IsVisible = false;
                q3answersno3.IsVisible = false;

                q3lblno4.IsVisible = false;
                q3directionsno4.IsVisible = false;
                q3answersno4.IsVisible = false;

                q3lblno5.IsVisible = false;
                q3directionsno5.IsVisible = false;
                q3answersno5.IsVisible = false;

                q3lblno6.IsVisible = false;
                q3directionsno6.IsVisible = false;
                q3answersno6.IsVisible = false;

                var getquestion = AllQuestions.Where(x => x.apporder == "q3yes").FirstOrDefault();

                if (getquestion != null)
                {

                    q3lblyes.Text = getquestion.label;
                    q3directionsyes.Text = getquestion.labelDirections;


                 

                }

                q3lblyes.IsVisible = true;
                q3directionsyes.IsVisible = true;
                q3answersyes.IsVisible = true;
               
                //hide no answers

            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_7(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            //q3 no

            if (e.Value)
            {

                q3lblyes.IsVisible = false;
                q3directionsyes.IsVisible = false;
                q3answersyes.IsVisible = false;


                var getquestion1 = AllQuestions.Where(x => x.apporder == "q3no1").FirstOrDefault();

                if (getquestion1 != null)
                {

                    q3lblno1.Text = getquestion1.label;
                    q3directionsno1.Text = getquestion1.labelDirections;

                    var split = getquestion1.values.Split(',');

                    q3answersno1.ItemsSource = split;


                }

                var getquestion2 = AllQuestions.Where(x => x.apporder == "q3no2").FirstOrDefault();

                if (getquestion2 != null)
                {

                    q3lblno2.Text = getquestion2.label;
                    q3directionsno2.Text = getquestion2.labelDirections;

                    var split = getquestion2.values.Split(',');

                    q3answersno2.ItemsSource = split;


                }

                var getquestion3 = AllQuestions.Where(x => x.apporder == "q3no3").FirstOrDefault();

                if (getquestion3 != null)
                {

                    q3lblno3.Text = getquestion3.label;
                    q3directionsno3.Text = getquestion3.labelDirections;

                    var split = getquestion3.values.Split(',');

                    q3answersno3.ItemsSource = split;


                }

                var getquestion4 = AllQuestions.Where(x => x.apporder == "q3no4").FirstOrDefault();

                if (getquestion4 != null)
                {

                    q3lblno4.Text = getquestion4.label;
                    q3directionsno4.Text = getquestion4.labelDirections;

                    var split = getquestion4.values.Split(',');

                    q3answersno4.ItemsSource = split;


                }


                q3lblno1.IsVisible = true;
                q3directionsno1.IsVisible = true;
                q3answersno1.IsVisible = true;

                q3lblno2.IsVisible = true;
                q3directionsno2.IsVisible = true;
                q3answersno2.IsVisible = true;

                q3lblno3.IsVisible = true;
                q3directionsno3.IsVisible = true;
                q3answersno3.IsVisible = true;

                q3lblno4.IsVisible = true;
                q3directionsno4.IsVisible = true;
                q3answersno4.IsVisible = true;


            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_8(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                MC33 = text;

                if (text.Contains("Yes"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q3no5").FirstOrDefault();

                    if (getquestion4 != null)
                    {

                        q3lblno5.Text = getquestion4.label;
                        q3directionsno5.Text = getquestion4.labelDirections;

                        var split = getquestion4.values.Split(',');

                        q3answersno5.ItemsSource = split;

                        q3lblno5.IsVisible = true;
                        q3directionsno5.IsVisible = true;
                        q3answersno5.IsVisible = true;


                    }
                }
                else
                {
                    q3lblno5.IsVisible = false;
                    q3directionsno5.IsVisible = false;
                    q3answersno5.IsVisible = false;
                }
            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_9(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                MC34 = text;

                if (text.Contains("Predictable"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q3no6").FirstOrDefault();

                    if (getquestion4 != null)
                    {

                        q3lblno6.Text = getquestion4.label;
                        q3directionsno6.Text = getquestion4.labelDirections;

                        var split = getquestion4.values.Split(',');

                        q3answersno6.ItemsSource = split;

                        q3lblno6.IsVisible = true;
                        q3directionsno6.IsVisible = true;
                        q3answersno6.IsVisible = true;


                    }
                }
                else
                {
                    q3lblno6.IsVisible = false;
                    q3directionsno6.IsVisible = false;
                    q3answersno6.IsVisible = false;
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //treatment question one 

            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {
                    if (!Tq1.Contains(text))
                    {
                        Tq1.Add(text);
                    }


                    if (text.Contains("Other"))
                    {
                        t1textentry.IsVisible = true;
                        t1textborder.IsVisible = true;
                    }
                    else
                    {
                        if (text.Contains("Testosterone"))
                        {
                            tq3border.IsVisible = true;
                        }

                        // t1textentry.IsVisible = false;
                        //t1textborder.IsVisible = false;
                    }

                }
                else
                {
                    Tq1.Remove(text);

                    if (text.Contains("Other"))
                    {
                        t1textentry.IsVisible = false;
                        t1textborder.IsVisible = false;
                    }

                    if (!Tq1.Any(s => s.Contains("Testosterone")) && !Tq2.Any(s => s.Contains("Testosterone")))
                    {
                        tq3border.IsVisible = false;
                    }
                }

            }



        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_1(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //treatment question two

            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {

                    if (!Tq2.Contains(text))
                    {
                        Tq2.Add(text);
                    }

                    if (text.Contains("Other"))
                    {
                        t2textentry.IsVisible = true;
                        t2textborder.IsVisible = true;

                       

                    }
                    else
                    {

                        if(text.Contains("Testosterone"))
                        {
                            tq3border.IsVisible = true;
                        }

                      //  t2textentry.IsVisible = false;
                       // t2textborder.IsVisible = false;
                    }

                }
                else
                {

                    Tq2.Remove(text);

                    if (text.Contains("Other"))
                    {

                        t2textentry.IsVisible = false;
                        t2textborder.IsVisible = false;
                    }

                    if (!Tq1.Any(s => s.Contains("Testosterone")) && !Tq2.Any(s => s.Contains("Testosterone")))
                    {
                        tq3border.IsVisible = false;
                    }
                }

            }



        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_2(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //treatment question three 

            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {

                    if (!Tq3.Contains(text))
                    {
                        Tq3.Add(text);
                    }

                    if (text.Contains("Other"))
                    {
                        t3textentry.IsVisible = true;
                        t3textborder.IsVisible = true;
                    }
                    else
                    {
                        //t3textentry.IsVisible = false;
                        //t3textborder.IsVisible = false;
                    } 

                }
                else
                {

                    Tq3.Remove(text);

                    if (text.Contains("Other"))
                    {
                        t3textentry.IsVisible = false;
                        t3textborder.IsVisible = false;
                    }
                }

            }



        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_10(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                Tq4 = item.Content.ToString();

                t4textborder.IsVisible = true;
                t4textentry.IsVisible = true;
                t4yes.IsVisible = false;
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_11(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                Tq4 = item.Content.ToString();

                t4textborder.IsVisible = false;
                t4textentry.IsVisible = false;
                t4yes.IsVisible = false;
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_12(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            t4textborder.IsVisible = false;
            t4textentry.IsVisible = false;

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {

                //question 4 yes 
                if (e.Value)
                {
                    Tq4 = item.Content.ToString();

                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q4yes").FirstOrDefault();

                    t4lblyes.Text = getquestion4.label;
                    t4directionsyes.Text = getquestion4.labelDirections;

                    var split = getquestion4.values.Split(',');

                    t4answersyes.ItemsSource = split;

                    t4yes.IsVisible = true;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_3(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {

            //q4 yes 

            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {
                    if (!Tq41.Contains(text))
                    {
                        Tq41.Add(text);
                    }

                    if (text.Contains("Other"))
                    {
                        t4textborderyes.IsVisible = true;
                        t4textentryyes.IsVisible = true;
                    }
                    else
                    {
                        // f1textborder.IsVisible = false;
                        // f1textentry.IsVisible = false;
                    }

                }
                else
                {
                    Tq41.Remove(text);

                    if (text.Contains("Other"))
                    {
                        t4textborderyes.IsVisible = false;
                        t4textentryyes.IsVisible = false;
                    }
                }

            }



        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_13(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                //q5 yes
                if (e.Value)
                {

                    Tq5 = text;

                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q5yes").FirstOrDefault();

                    t5lblyes.Text = getquestion4.label;
                    t5directionsyes.Text = getquestion4.labelDirections;

                    t5lblyes.IsVisible = true;
                    t5directionsyes.IsVisible = true;
                    t5yesentry.IsVisible = true;
                    t5textborderyes.IsVisible = true;

                }
            }

        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_14(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                if (e.Value)
                {

                    Tq5 = text;

                    t5lblyes.IsVisible = false;
                    t5directionsyes.IsVisible = false;
                    t5textborderyes.IsVisible = false;
                    t5yesentry.IsVisible = false;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_15(object sender, CheckedChangedEventArgs e)
    {
        try
        {
        
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq6 = text;

                if (text.Contains("Other"))
                {
                    t6yesentry.IsVisible = true;
                    t6textborderyes.IsVisible = true;
                }
                else
                {
                    t6yesentry.IsVisible = false;
                    t6textborderyes.IsVisible = false;
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_16(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq8 = text;

                if (text.Contains("Yes"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q8yes1").FirstOrDefault();

                    t8lblyes1.Text = getquestion4.label;
                    t8directionsyes1.Text = getquestion4.labelDirections;

                    var split = getquestion4.values.Split(',');

                    t8answersyes1.ItemsSource = split;

                    t8lblyes1.IsVisible = true;
                    t8directionsyes1.IsVisible = true;
                    t8answersyes1.IsVisible = true;

                    var getquestion5 = AllQuestions.Where(x => x.apporder == "q8yes2").FirstOrDefault();

                    t8lblyes2.Text = getquestion5.label;
                    t8directionsyes2.Text = getquestion5.labelDirections;

                    var split2 = getquestion5.values.Split(',');

                    t8answersyes2.ItemsSource = split2;

                    t8lblyes2.IsVisible = true;
                    t8directionsyes2.IsVisible = true;
                    t8answersyes2.IsVisible = true;
                }
                else
                {
                    t8lblyes1.IsVisible = false;
                    t8directionsyes1.IsVisible = false;
                    t8answersyes1.IsVisible = false;
                    t8textborderyes.IsVisible = false;
                    t8yesentry.IsVisible = false;
                    t8lblyes2.IsVisible = false;
                    t8directionsyes2.IsVisible = false;
                    t8answersyes2.IsVisible = false;
                    t8textborder2.IsVisible = false;
                    t8textentry2.IsVisible = false;
                }
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_17(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq81 = text;

                if (text.Contains("Other"))
                {

                    t8textborderyes.IsVisible = true;
                    t8yesentry.IsVisible = true;

                }
                else
                {
                    t8textborderyes.IsVisible = false;
                    t8yesentry.IsVisible = false;
                }
            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_4(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedCheckbox)sender;


            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {

                    if (!Tq82.Contains(text))
                    {
                        Tq82.Add(text);
                    }

                    if (text.Contains("Other"))
                    {
                        t8textentry2.IsVisible = true;
                        t8textborder2.IsVisible = true;
                    }
                    else
                    {
                        //t3textentry.IsVisible = false;
                        //t3textborder.IsVisible = false;
                    }

                }
                else
                {

                    Tq82.Remove(text);

                    if (text.Contains("Other"))
                    {
                        t8textentry2.IsVisible = false;
                        t8textborder2.IsVisible = false;
                    }
                }

            }

       
        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_18(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq10 = text;

                if (text.Contains("Yes"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q10yes").FirstOrDefault();

                    t10lblyes1.Text = getquestion4.label;
                    t10directionsyes1.Text = getquestion4.labelDirections;

                    var split = getquestion4.values.Split(',');

                    t10answersyes1.ItemsSource = split;

                    t10lblyes1.IsVisible = true;
                    t10directionsyes1.IsVisible = true;
                    t10answersyes1.IsVisible = true;

                }
                else
                {

                    t10lblyes1.IsVisible = false;
                    t10directionsyes1.IsVisible = false;
                    t10answersyes1.IsVisible = false;
                    t10textborderyes.IsVisible = false;
                    t10yesentry.IsVisible = false;
                }

            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_5(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {
                    
                    
                        Tq101 = (text);
                    

                    if (text.Contains("Other"))
                    {
                        t10yesentry.IsVisible = true;
                        t10textborderyes.IsVisible = true;
                    }
                    else
                    {
                        // f1textborder.IsVisible = false;
                        // f1textentry.IsVisible = false;
                    }

                }
                else
                {
                    

                    if (text.Contains("Other"))
                    {
                        t10yesentry.IsVisible = false;
                        t10textborderyes.IsVisible = false;
                    }
                }

            }


           
        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_19(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq11 = text;

                if (text.Contains("Yes"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q11yes1").FirstOrDefault();

                    t11lblyes1.Text = getquestion4.label;
                    t11directionsyes1.Text = getquestion4.labelDirections;

                    t11lblyes1.IsVisible = true;
                    t11directionsyes1.IsVisible = true;
                    t11textborderyes.IsVisible = true;
                    t11yesentry.IsVisible = true;

                    var getquestion5 = AllQuestions.Where(x => x.apporder == "q11yes2").FirstOrDefault();

                    t11lblyes2.Text = getquestion5.label;
                    t11directionsyes2.Text = getquestion5.labelDirections;

                    var split = getquestion5.values.Split(',');

                    t11answersyes2.ItemsSource = split;

                    t11lblyes2.IsVisible = true;
                    t11directionsyes2.IsVisible = true;
                    t11answersyes2.IsVisible = true;


                }
                else
                {
                    t11lblyes1.IsVisible = false;
                    t11directionsyes1.IsVisible = false;
                    t11textborderyes.IsVisible = false;
                    t11yesentry.IsVisible = false;


                    t11lblyes2.IsVisible = false;
                    t11directionsyes2.IsVisible = false;
                    t11answersyes2.IsVisible = false;

                    t11textborderyes2.IsVisible = false;
                    t11yesentry2.IsVisible = false;
                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_20(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq112 = text;

                if (text.Contains("Other"))
                {
                    t11textborderyes2.IsVisible = true;
                    t11yesentry2.IsVisible = true;
                }
                else
                {
                    t11textborderyes2.IsVisible = false;
                    t11yesentry2.IsVisible = false;
                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_21(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq12 = text;

                if (text.Contains("Yes"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q12yes1").FirstOrDefault();

                    t12lblyes1.Text = getquestion4.label;
                    t12directionsyes1.Text = getquestion4.labelDirections;

                    var split1 = getquestion4.values.Split(',');

                    t12answersyes1.ItemsSource = split1;

                    t12lblyes1.IsVisible = true;
                    t12directionsyes1.IsVisible = true;
                    t12answersyes1.IsVisible = true;

                    var getquestion5 = AllQuestions.Where(x => x.apporder == "q12yes2").FirstOrDefault();

                    t12lblyes2.Text = getquestion5.label;
                    t12directionsyes2.Text = getquestion5.labelDirections;

                    var split = getquestion5.values.Split(',');

                    t12answersyes2.ItemsSource = split;

                    t12lblyes2.IsVisible = true;
                    t12directionsyes2.IsVisible = true;
                    t12answersyes2.IsVisible = true;


                }
                else
                {
                    t12lblyes1.IsVisible = false;
                    t12directionsyes1.IsVisible = false;
                    t12answersyes1.IsVisible = false;

                    t12textborderyes.IsVisible = false;
                    t12yesentry.IsVisible = false;


                    t12lblyes2.IsVisible = false;
                    t12directionsyes2.IsVisible = false;
                    t12answersyes2.IsVisible = false;

                    t12textborderyes2.IsVisible = false;
                    t12yesentry2.IsVisible = false;


                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_22(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq121 = text;

                if (text.Contains("Other"))
                {
                    t12textborderyes.IsVisible = true;
                    t12yesentry.IsVisible = true;
                }
                else
                {
                    t12textborderyes.IsVisible = false;
                    t12yesentry.IsVisible = false;
                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_23(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq122 = text;

                if (text.Contains("Other"))
                {
                    t12textborderyes2.IsVisible = true;
                    t12yesentry2.IsVisible = true;
                }
                else
                {
                    t12textborderyes2.IsVisible = false;
                    t12yesentry2.IsVisible = false;
                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_24(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq13 = text;

                if (text.Contains("Yes"))
                {
                    var getquestion4 = AllQuestions.Where(x => x.apporder == "q13yes1").FirstOrDefault();

                    t13lblyes1.Text = getquestion4.label;
                    t13directionsyes1.Text = getquestion4.labelDirections;

                    var split1 = getquestion4.values.Split(',');

                    t13answersyes1.ItemsSource = split1;

                    t13lblyes1.IsVisible = true;
                    t13directionsyes1.IsVisible = true;
                    t13answersyes1.IsVisible = true;

                    var getquestion5 = AllQuestions.Where(x => x.apporder == "q13yes2").FirstOrDefault();

                    t13lblyes2.Text = getquestion5.label;
                    t13directionsyes2.Text = getquestion5.labelDirections;

                    var split = getquestion5.values.Split(',');

                    t13answersyes2.ItemsSource = split;

                    t13lblyes2.IsVisible = true;
                    t13directionsyes2.IsVisible = true;
                    t13answersyes2.IsVisible = true;


                }
                else
                {
                    t13lblyes1.IsVisible = false;
                    t13directionsyes1.IsVisible = false;
                    t13answersyes1.IsVisible = false;

                    t13textborderyes.IsVisible = false;
                    t13yesentry.IsVisible = false;


                    t13lblyes2.IsVisible = false;
                    t13directionsyes2.IsVisible = false;
                    t13answersyes2.IsVisible = false;

                    t13textborderyes2.IsVisible = false;
                    t13yesentry2.IsVisible = false;


                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_25(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq131 = text;

                if (text.Contains("Other"))
                {
                    t13textborderyes.IsVisible = true;
                    t13yesentry.IsVisible = true;
                }
                else
                {
                    t13textborderyes.IsVisible = false;
                    t13yesentry.IsVisible = false;
                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_26(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {
                var text = item.Content as string;

                Tq132 = text;

                if (text.Contains("Other"))
                {
                    t13textborderyes2.IsVisible = true;
                    t13yesentry2.IsVisible = true;
                }
                else
                {
                    t13textborderyes2.IsVisible = false;
                    t13yesentry2.IsVisible = false;
                }

            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_6(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {

            //fm1
            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {
                    if (!MHq1.Contains(text))
                    {
                        MHq1.Add(text);
                    }

                    if (text.Contains("Other"))
                    {
                        f1textborder.IsVisible = true;
                        f1textentry.IsVisible = true;
                    }
                    else
                    {
                       // f1textborder.IsVisible = false;
                       // f1textentry.IsVisible = false;
                    }

                }
                else
                {
                    MHq1.Remove(text);

                    if (text.Contains("Other"))
                    {
                        f1textborder.IsVisible = false;
                        f1textentry.IsVisible = false;
                    }
                }

            }



        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_7(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {

            //fm2
            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {
                    if (!MHq2.Contains(text))
                    {
                        MHq2.Add(text);
                    }

                    if (text.Contains("Other"))
                    {
                        f2textborder.IsVisible = true;
                        f2textentry.IsVisible = true;
                    }
                    else
                    {
                        // f1textborder.IsVisible = false;
                        // f1textentry.IsVisible = false;
                    }

                }
                else
                {
                    MHq2.Remove(text);

                    if (text.Contains("Other"))
                    {
                        f2textborder.IsVisible = false;
                        f2textentry.IsVisible = false;
                    }
                }

            }



        }
        catch (Exception ex)
        {

        }
    }

    private async void submitbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            submitbtn.IsEnabled = false;
            NavigationPage.SetHasNavigationBar(this, false);

            prevbtn.IsVisible = false;

            titlelbl.IsVisible = false;
            sublabel.IsVisible = false;
            mcstack.IsVisible = false;
            treatmentstack.IsVisible = false;
            familyhistorystack.IsVisible = false;
            submitbtn.IsVisible = false;
            loadingstack.IsVisible = true;
            loadinglbl.IsVisible = true;


            var newuserresponse = new ObservableCollection<userresponse>();

            if(titlelbl.Text == "Medical History")
            {

                //q1
                string q1 = string.Join(", ", MHq1);

                var newq1 = new userresponse();

                newq1.userid = Helpers.Settings.UserKey;
                newq1.questionid = "88BEEED5-9BF4-498E-AE8B-A4381A81D340";
                newq1.answerid = q1;
                newq1.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                if(f1textentry.IsVisible)
                {
                    newq1.notes = f1textentry.Text;
                }

                newuserresponse.Add(newq1);


                //q2
                string q2 = string.Join(", ", MHq2);

                var newq2 = new userresponse();

                newq2.userid = Helpers.Settings.UserKey;
                newq2.questionid = "51EFD39E-A7EA-4F79-9172-8CF45FB9C8B1";
                newq2.answerid = q2;
                newq2.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                if (f2textentry.IsVisible)
                {
                    newq2.notes = f2textentry.Text;
                }

                newuserresponse.Add(newq2);

                //q3

                var newq3 = new userresponse();

                newq3.userid = Helpers.Settings.UserKey;
                newq3.questionid = "09E75651-7EAA-449B-909B-D82F7C393735";
                newq3.notes = f3textentry.Text;
                newq3.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

             
                newuserresponse.Add(newq3);



            }
            else if(titlelbl.Text == "Treatment Information")
            {

                //q1
                string q1 = string.Join(", ", Tq1);

                var newq1 = new userresponse();

                newq1.userid = Helpers.Settings.UserKey;
                newq1.questionid = "BFDE419F-DBB2-40C9-825E-36B38E9DF069";
                newq1.answerid = q1;
                newq1.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                if (t1textentry.IsVisible)
                {
                    newq1.notes = t1textentry.Text;
                }

                newuserresponse.Add(newq1);


                //q2
                string q2 = string.Join(", ", Tq2);

                var newq2 = new userresponse();

                newq2.userid = Helpers.Settings.UserKey;
                newq2.questionid = "4D31C5AE-F012-450C-A406-F9AC0FA3BFFA";
                newq2.answerid = q2;
                newq2.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                if (t2textentry.IsVisible)
                {
                    newq2.notes = t2textentry.Text;
                }

                newuserresponse.Add(newq2);


                //q3
                if(tq3border.IsVisible == true)
                {
                    string q3 = string.Join(", ", Tq3);

                    var newq3 = new userresponse();

                    newq3.userid = Helpers.Settings.UserKey;
                    newq3.questionid = "F47EB393-E480-422B-A438-3C8A456258E6";
                    newq3.answerid = q3;
                    newq3.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (t3textentry.IsVisible)
                    {
                        newq3.notes = t3textentry.Text;
                    }

                    newuserresponse.Add(newq3);
                }


                //q4

                var newq4 = new userresponse();

                newq4.userid = Helpers.Settings.UserKey;
                newq4.questionid = "8649296F-5339-4400-9B46-E21A97BCFB34";
                newq4.answerid = Tq4;
                newq4.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                if (t4textentry.IsVisible)
                {
                    newq4.notes =   t4textentry.Text;
                }

                newuserresponse.Add(newq4);

                if(Tq4.Contains("Yes"))
                {

                    string q41 = string.Join(", ", Tq41);

                    var newq41 = new userresponse();

                    newq41.userid = Helpers.Settings.UserKey;
                    newq41.questionid = "309C66FB-575E-4C8C-A07A-417782CFF19A";
                    newq41.answerid = q41;
                    newq41.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (t4textentryyes.IsVisible)
                    {
                        newq41.notes = t4textentryyes.Text;
                    }

                    newuserresponse.Add(newq41);



                }


                //q5

                var newq5 = new userresponse();

                newq5.userid = Helpers.Settings.UserKey;
                newq5.questionid = "99AE5F9A-2D74-47F0-AD20-5C795A579273";
                newq5.answerid = Tq5;
                newq5.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq5);


                if(t5lblyes.IsVisible)
                {
                    var newq51 = new userresponse();

                    newq51.userid = Helpers.Settings.UserKey;
                    newq51.questionid = "65C1D251-A249-48D6-8229-00AF9FBA15DC";
                    newq51.notes = t5yesentry.Text;
                    newq51.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    newuserresponse.Add(newq51);
                }


                //q6

                var newq6 = new userresponse();

                newq6.userid = Helpers.Settings.UserKey;
                newq6.questionid = "C8A5CBE9-E285-4E45-B987-43F6BB09376E";
                newq6.answerid = Tq6;
                newq6.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                if(t6yesentry.IsVisible)
                {
                    newq6.notes = t6yesentry.Text;
                }

                newuserresponse.Add(newq6);


                //q7

                var newq7 = new userresponse();

                newq7.userid = Helpers.Settings.UserKey;
                newq7.questionid = "34FBB5D9-3A0F-497D-9338-082989EBB9AC";
                newq7.notes = t7entry.Text;
                newq7.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

              

                newuserresponse.Add(newq7);


                //q8

                var newq8 = new userresponse();

                newq8.userid = Helpers.Settings.UserKey;
                newq8.questionid = "844FAFCE-9746-4E14-B244-5D97467496F3";
                newq8.answerid = Tq8;
                newq8.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq8);


                if(Tq8.Contains("Yes"))
                {

                    var newq81 = new userresponse();

                    newq81.userid = Helpers.Settings.UserKey;
                    newq81.questionid = "4B51F6F4-2601-434F-9E9E-12D7DCFADE11";
                    newq81.answerid = Tq81;
                    newq81.responsedate = DateTime.Now.ToString("dd/MM/yyyy");


                    if(t8yesentry.IsVisible)
                    {
                        newq81.notes = t8yesentry.Text;
                    }


                    newuserresponse.Add(newq81);


                    string q82 = string.Join(", ", Tq82);

                    var newq82 = new userresponse();

                    newq82.userid = Helpers.Settings.UserKey;
                    newq82.questionid = "62D3A8F6-2B86-494B-987B-481FFFAC6EAA";
                    newq82.answerid = q82;
                    newq82.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (t8textentry2.IsVisible)
                    {
                        newq82.notes = t8textentry2.Text;
                    }

                    newuserresponse.Add(newq82);



                }



                //q9


                var newq9 = new userresponse();

                newq9.userid = Helpers.Settings.UserKey;
                newq9.questionid = "86B8ECA7-8FD4-4589-BBFA-B7CC2B35DF31";
                newq9.notes = t9entry.Text;
                newq9.responsedate = DateTime.Now.ToString("dd/MM/yyyy");


                newuserresponse.Add(newq9);


                //q10

                var newq10 = new userresponse();

                newq10.userid = Helpers.Settings.UserKey;
                newq10.questionid = "8395BED5-5421-4A45-84DA-D748404BDE22";
                newq10.answerid = Tq10;
                newq10.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq10);


                if(t10lblyes1.IsVisible)
                {

                    var newq101 = new userresponse();

                    newq101.userid = Helpers.Settings.UserKey;
                    newq101.questionid = "8395BED5-5421-4A45-84DA-D748404BDE22";
                   /// string q101 = string.Join(", ", Tq101);
                    newq101.answerid = Tq101;
                    newq101.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if(t10yesentry.IsVisible)
                    {
                        newq101.notes = t10yesentry.Text;
                    }


                    newuserresponse.Add(newq101);

                    //add private question

                    if(t10lblyes2.IsVisible)
                    {
                        var newq102 = new userresponse();

                        newq102.userid = Helpers.Settings.UserKey;
                        newq102.questionid = "00358573-DCE8-47AB-81A4-89AEFE9EAA51";
                        /// string q101 = string.Join(", ", Tq101);
                        newq102.notes = t10yesentry2.Text.ToString();
                        newq102.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                        
                        newuserresponse.Add(newq102);
                    }



                }


                //q11

                var newq11 = new userresponse();

                newq11.userid = Helpers.Settings.UserKey;
                newq11.questionid = "876FA1EA-6E2C-4A1F-B013-2BFBB0E61840";
                newq11.answerid = Tq11;
                newq11.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq11);


                if(t11lblyes1.IsVisible)
                {

                    var newq111 = new userresponse();

                    newq111.userid = Helpers.Settings.UserKey;
                    newq111.questionid = "898B0361-5068-4326-8A0D-940CDC40CEAC";
                    newq111.notes = t11yesentry.Text;
                    newq111.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    newuserresponse.Add(newq111);



                    var newq112 = new userresponse();

                    newq112.userid = Helpers.Settings.UserKey;
                    newq112.questionid = "B1607095-0723-4398-8A0C-1DCBD7E391CE";
                    newq112.answerid = Tq112;
                    newq112.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if(t11yesentry2.IsVisible)
                    {
                        newq112.notes = t11yesentry2.Text;
                    }

                    newuserresponse.Add(newq112);


                }



                //q12

                var newq12 = new userresponse();

                newq12.userid = Helpers.Settings.UserKey;
                newq12.questionid = "34FE80CB-FC86-4862-A52D-68F100412340";
                newq12.answerid = Tq12;
                newq12.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq12);

                if(t12lblyes1.IsVisible)
                {
                    var newq121 = new userresponse();

                    newq121.userid = Helpers.Settings.UserKey;
                    newq121.questionid = "3432B7AE-9CED-4086-84EC-0445566FFEE0";
                    newq121.answerid = Tq121;
                    newq121.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if(t12yesentry.IsVisible)
                    {
                        newq121.notes = t12yesentry.Text;
                    }


                    newuserresponse.Add(newq121);


                    var newq122 = new userresponse();

                    newq122.userid = Helpers.Settings.UserKey;
                    newq122.questionid = "F9227960-839D-41B6-B02D-94FBB44A9A6E";
                    newq122.answerid = Tq122;
                    newq122.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (t12yesentry2.IsVisible)
                    {
                        newq122.notes = t12yesentry2.Text;
                    }


                    newuserresponse.Add(newq122);



                }




                //q13


                var newq13 = new userresponse();

                newq13.userid = Helpers.Settings.UserKey;
                newq13.questionid = "5940CC61-40DE-4422-9724-AF45F6C80AEE";
                newq13.answerid = Tq13;
                newq13.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq13);


                if(t13lblyes1.IsVisible)
                {

                    var newq131 = new userresponse();

                    newq131.userid = Helpers.Settings.UserKey;
                    newq131.questionid = "51E9F071-71C3-4F26-ABAC-EAE07E9627D6";
                    newq131.answerid = Tq131;
                    newq131.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (t13yesentry.IsVisible)
                    {
                        newq131.notes = t13yesentry.Text;
                    }


                    newuserresponse.Add(newq131);



                    var newq132 = new userresponse();

                    newq132.userid = Helpers.Settings.UserKey;
                    newq132.questionid = "FF832A5D-8896-4F8E-8119-2D952955C487";
                    newq132.answerid = Tq132;
                    newq132.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    if (t13yesentry2.IsVisible)
                    {
                        newq132.notes = t13yesentry2.Text;
                    }

                    newuserresponse.Add(newq132);



                }




            }
            else if(titlelbl.Text == "Menstural Cycle")
            {

                //q1
                var newq1 = new userresponse();

                newq1.userid = Helpers.Settings.UserKey;
                newq1.questionid = "8FECF64C-20A0-49AC-9F1D-C4E823CE77C3";
                if (mc1ssyes.IsChecked)
                {
                    newq1.answerid = "Yes";
                }
                else if (mc1ssno.IsChecked)
                {
                    newq1.answerid = "No";
                }
                newq1.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq1);


                if(mc1ssyes.IsChecked)
                {
                    //yes

                    var newq11 = new userresponse();

                    newq11.userid = Helpers.Settings.UserKey;
                    newq11.questionid = "4F7A5C8C-99F5-4ADE-B74D-21DE068F41FC";
                    newq11.answerid = MC1yesanswerss;
                    newq11.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    newuserresponse.Add(newq11);


                    string q1 = string.Join(", ", MCq1);

                    var newq111 = new userresponse();

                    newq111.userid = Helpers.Settings.UserKey;
                    newq111.questionid = "F44E6E7A-F450-4C88-819F-D0299E93C490";
                    newq111.answerid = q1;
                    newq111.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    newuserresponse.Add(newq111);

                }
                else
                {
                    //no

                    var newq12 = new userresponse();

                    newq12.userid = Helpers.Settings.UserKey;
                    newq12.questionid = "E90618A6-91DD-4AD9-B2E6-683EE1214477";
                    if (mcno1yesss.IsChecked)
                    {
                        newq12.answerid = "Yes";
                    }
                    else if (mcno1noss.IsChecked)
                    {
                        newq12.answerid = "No";
                    }

                    newq12.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                    newuserresponse.Add(newq12);

                }



                //q2

                var newq2 = new userresponse();

                newq2.userid = Helpers.Settings.UserKey;
                newq2.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                newq2.questionid = "78CC8663-F1DD-4A1B-BB96-FDB247E4DE19";

                if(ssmcq2yes.IsChecked)
                {
                    newq2.answerid = "Yes";

                    var newq21 = new userresponse();

                    newq21.userid = Helpers.Settings.UserKey;
                    newq21.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq21.questionid = "7A9672F1-D4C0-4905-ACA2-7C7ACB3E7C37";

                    if(q2othertext.IsVisible)
                    {
                        newq21.notes = q2othertext.Text;
                    }
                    else
                    {
                        newq21.answerid = MC2;
                    }

                    newuserresponse.Add(newq21);

                }
                else if(ssmcq2no.IsChecked)
                {
                    newq2.answerid = "No";

                    var newq21 = new userresponse();

                    newq21.userid = Helpers.Settings.UserKey;
                    newq21.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq21.questionid = "F014484C-A77E-46B7-B4D6-8B2474E8898F";

                    if (q2othertext.IsVisible)
                    {
                        newq21.notes = q2othertext.Text;
                    }
                    else
                    {
                        newq21.answerid = MC2;
                    }

                    newuserresponse.Add(newq21);

                }
                else if(ssmcq2other.IsChecked)
                {
                    newq2.notes = q2othertext.Text;
                }

                newuserresponse.Add(newq2);


                //q3
                var newq3 = new userresponse();

                newq3.userid = Helpers.Settings.UserKey;
                newq3.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                newq3.questionid = "29CBB1DA-173A-4754-93D1-A818BF20B000";

                if(mc3ssyes.IsChecked)
                {
                    newq3.answerid = "Yes";

                    var newq31 = new userresponse();

                    newq31.userid = Helpers.Settings.UserKey;
                    newq31.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq31.questionid = "EC0D498C-B2B0-4C8A-B1F0-0857AC4F37BF";

                    if(mc3ssyes1.IsChecked)
                    {
                        newq31.answerid = "Yes";
                    }
                    else if(mc3ssno1.IsChecked)
                    {
                        newq31.answerid = "No";

                    }

                    newuserresponse.Add(newq31);

                }
                else if(mc3ssno.IsChecked)
                {
                    newq3.answerid = "No";

                    var newq31 = new userresponse();

                    newq31.userid = Helpers.Settings.UserKey;
                    newq31.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq31.questionid = "0C568C5F-3304-4B6C-91C5-ADE35B110428";
                    newq31.answerid = MC3;

                    newuserresponse.Add(newq31);


                    var newq32 = new userresponse();

                    newq32.userid = Helpers.Settings.UserKey;
                    newq32.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq32.questionid = "B2E8B070-E6A5-4AC6-AB9B-3982C2FED551";
                    newq32.answerid = MC31;

                    newuserresponse.Add(newq32);


                    var newq33 = new userresponse();

                    newq33.userid = Helpers.Settings.UserKey;
                    newq33.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq33.questionid = "6765E59A-948B-4D52-A1F8-6AB21AB09376";
                    newq33.answerid = MC32;

                    newuserresponse.Add(newq33);


                    var newq34 = new userresponse();

                    newq34.userid = Helpers.Settings.UserKey;
                    newq34.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                    newq34.questionid = "6E536DCD-7A39-41DC-9115-BB9F5167E98B";
                    newq34.answerid = MC33;

                    newuserresponse.Add(newq34);


                    if(MC33.Contains("Yes"))
                    {


                        var newq35 = new userresponse();

                        newq35.userid = Helpers.Settings.UserKey;
                        newq35.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                        newq35.questionid = "CFD3C523-C967-4A17-9AE3-09211107000A";
                        newq35.answerid = MC34;

                        newuserresponse.Add(newq35);



                        if(MC34.Contains("Predictable"))
                        {
                            var newq36 = new userresponse();

                            newq36.userid = Helpers.Settings.UserKey;
                            newq36.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
                            newq36.questionid = "3CEC7451-0204-4D81-9BEE-5FF57C88B6DB";
                            newq36.answerid = MC35;

                            newuserresponse.Add(newq36);
                        }

                    }
                    

                }

                newuserresponse.Add(newq3);


                //q4 
                var newq4 = new userresponse();

                newq4.userid = Helpers.Settings.UserKey;
                newq4.questionid = "733DF4FB-6C01-42F0-AABB-5ABA16346300";
                newq4.notes = q4entrytext.Text.ToString();
                newq4.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq4);

                //q5
                var newq5 = new userresponse();

                newq5.userid = Helpers.Settings.UserKey;
                newq5.questionid = "B3DA18BA-5E22-4DB8-B527-9C7AFA451F8A";
                newq5.notes = datePicker.Date.ToString("dd/MM/yyyy");
                newq5.responsedate = DateTime.Now.ToString("dd/MM/yyyy");

                newuserresponse.Add(newq5);

            }


            //upload the user
            var serializerOptions = new JsonSerializerOptions
            {
                //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                //  WriteIndented = true
            };

            if (newuserresponse != null || newuserresponse.Count != 0)
            {
                //add the inital questions and answers as user reponses
                for (int i = 0; i < newuserresponse.Count; i++)
                {

                    if (string.IsNullOrEmpty(newuserresponse[i].answerid) && string.IsNullOrEmpty(newuserresponse[i].notes))
                    {

                    }
                    else
                    {
                        newuserresponse[i].userquestionnaireid = titlelbl.Text;

                        if (!string.IsNullOrEmpty(newuserresponse[i].notes))
                        {
                            newuserresponse[i].notes = newuserresponse[i].notes.Trim();
                        }

                        //  newuserresponse[i].id = "123";
                        Uri uri = new Uri(string.Format("https://pwapi.peoplewith.com/api/userresponse", string.Empty));
                        // var url = APICalls.InsertUserResponse;
                        string jsonn = System.Text.Json.JsonSerializer.Serialize<userresponse>(newuserresponse[i], serializerOptions);
                        StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                        ConfigureClient();
                        var response = await Client.PostAsync(uri, contenttt);

                        if (response.IsSuccessStatusCode)
                        {

                           


                        }
                        else
                        {
                            string errorcontent = await response.Content.ReadAsStringAsync();
                            var s = errorcontent;
                        }
                    }
                }


                loadingstack.IsVisible = false;
                await MopupService.Instance.PushAsync(new PopupPageHelper("Information Uploaded") { });
                await Task.Delay(1500);
                await MopupService.Instance.PopAllAsync(false);
                Navigation.RemovePage(this);

            }


         
          //  submitbtn.IsEnabled = true;



        }
        catch(Exception ex)
        {

            Navigation.RemovePage(this);

        }
    }

    private void ExtendedRadioButton_CheckedChanged_27(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            //mc one yes ss

            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {


                if (e.Value)
                {
                    MC1yesanswerss = item.Content.ToString();
                }
            }


        }
        catch(Exception ex)
        {

        }
    }

    private void ExtendedCheckbox_StateChanged_8(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            //mc1yes2
            var item = (ExtendedCheckbox)sender;

            if (item != null)
            {
                var text = item.Text.ToString();

                if (e.IsChecked == true)
                {
                    if (!MCq1.Contains(text))
                    {
                        MCq1.Add(text);
                    }

           

                }
                else
                {
                    MCq1.Remove(text);

           
                }

            }

        }
        catch (Exception ex)
        {
        }
    }

    private void ExtendedRadioButton_CheckedChanged_28(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            //mc three
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {


                if (e.Value)
                {
                    MC3 = item.Content.ToString();
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_29(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            //mc three
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {


                if (e.Value)
                {
                    MC31 = item.Content.ToString();
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_30(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            //mc three
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {


                if (e.Value)
                {
                    MC32 = item.Content.ToString();
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_31(object sender, CheckedChangedEventArgs e)
    {
        try
        {

            //mc three
            var item = (ExtendedRadioButton)sender;

            if (item != null)
            {


                if (e.Value)
                {
                    MC35 = item.Content.ToString();
                }
            }


        }
        catch (Exception ex)
        {

        }
    }

    private void ExtendedRadioButton_CheckedChanged_32(object sender, CheckedChangedEventArgs e)
    {
        try 
        {
            
        var item = (ExtendedRadioButton)sender;

        if (item != null)
        {
            var text = item.Content as string;

             Tq101 = text;

            if (text.Contains("Private"))
            {
                var getquestion4 = AllQuestions.Where(x => x.apporder == "q10yes2").FirstOrDefault();

                    t10lblyes2.Text = getquestion4.label;
                t11directionsyes1.Text = getquestion4.labelDirections;

                    t10lblyes2.IsVisible = true;
                    t10directionsyes2.IsVisible = true;
                    t10textborderyes2.IsVisible = true;
                    t10yesentry2.IsVisible = true;


            }
            else if(text.Contains("Other"))
                {
                    t10textborderyes.IsVisible = true;
                    t10yesentry.IsVisible = true;
                }
            else
            {
                    t10lblyes2.IsVisible = false;
                    t10directionsyes2.IsVisible = false;
                    t10textborderyes2.IsVisible = false;
                    t10yesentry2.IsVisible = false;

                    t10textborderyes.IsVisible = false;
                    t10yesentry.IsVisible = false;



                }

        }


    }
        catch (Exception ex)
        {

        }
    }

    private void nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            if (mcstack.IsVisible)
            {

                if (mcsection1.IsVisible)
                {
                    mcsection1.IsVisible = false;
                    mcsection2.IsVisible = true;
                    prevbtn.IsVisible = true;

                }
                else if (mcsection2.IsVisible)
                {
                    mcsection2.IsVisible = false;
                    mcsection3.IsVisible = true;
                  //  nextbtn.IsVisible = false;
                    //submitbtn.IsVisible = true;
                }
                else if(mcsection3.IsVisible)
                {
                    mcsection3.IsVisible = false;
                    mcsection4.IsVisible = true;
                }
                else if(mcsection4.IsVisible)
                {
                    mcsection4.IsVisible = false;
                    mcsection5.IsVisible = true;
                    nextbtn.IsVisible = false;
                    submitbtn.IsVisible = true;
                }
               


            }
            else if (treatmentstack.IsVisible)
            {
          
                if(tsection1.IsVisible)
                {
                    prevbtn.IsVisible = true;
                    tsection1.IsVisible = false;
                    tsection2.IsVisible = true;
                }
                else if (tsection2.IsVisible)
                {

                    tsection2.IsVisible = false;
                    tsection3.IsVisible = true;
                }
                else if(tsection3.IsVisible)
                {
                    tsection3.IsVisible = false;
                    tsection4.IsVisible = true;
                }
                else if (tsection4.IsVisible)
                {
                    tsection4.IsVisible = false;
                    tsection5.IsVisible = true;
                }
                else if (tsection5.IsVisible)
                {
                    tsection5.IsVisible = false;
                    tsection6.IsVisible = true;
                }
                else if (tsection6.IsVisible)
                {
                    tsection6.IsVisible = false;
                    tsection7.IsVisible = true;
                }
                else if (tsection7.IsVisible)
                {
                    tsection7.IsVisible = false;
                    tsection8.IsVisible = true;
                }
                else if (tsection8.IsVisible)
                {
                    tsection8.IsVisible = false;
                    tsection9.IsVisible = true;
                }
                else if (tsection9.IsVisible)
                {
                    tsection9.IsVisible = false;
                    tsection10.IsVisible = true;
                }
                else if (tsection10.IsVisible)
                {
                    tsection10.IsVisible = false;
                    tsection11.IsVisible = true;
                    nextbtn.IsVisible = false;
                    submitbtn.IsVisible = true;
                }

            }
            else if (familyhistorystack.IsVisible)
            {

                if (fhsection1.IsVisible)
                {
                    fhsection1.IsVisible = false;
                    fhsection2.IsVisible = true;
                    prevbtn.IsVisible = true;

                }
                else if(fhsection2.IsVisible)
                {
                    fhsection2.IsVisible = false;
                    fhsection3.IsVisible = true;
                    nextbtn.IsVisible = false;
                    submitbtn.IsVisible = true;
                }
             
            }



        }
        catch(Exception ex)
        {

        }
    }

    private void prevbtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            if (mcstack.IsVisible)
            {
                if(mcsection5.IsVisible)
                {
                    submitbtn.IsVisible = false;
                    mcsection5.IsVisible = false;
                    mcsection4.IsVisible = true;
                    nextbtn.IsVisible = true;
                }
                else if(mcsection4.IsVisible)
                {
                    mcsection4.IsVisible = false;
                    mcsection3.IsVisible = true;
                }
                else if(mcsection3.IsVisible)
                {
                    mcsection3.IsVisible = false;
                    mcsection2.IsVisible = true;

                }
                else if(mcsection2.IsVisible)
                {
                    mcsection2.IsVisible = false;
                    mcsection1.IsVisible = true;
                    prevbtn.IsVisible = false;
                }

            }
            else if(treatmentstack.IsVisible)
            {
                if (tsection11.IsVisible)
                {
                    tsection11.IsVisible = false;
                    tsection10.IsVisible = true;
                    nextbtn.IsVisible = true;
                    submitbtn.IsVisible = false;
                }
                else if (tsection10.IsVisible)
                {
                    tsection10.IsVisible = false;
                    tsection9.IsVisible = true;
                }
                else if (tsection9.IsVisible)
                {
                    tsection9.IsVisible = false;
                    tsection8.IsVisible = true;
                }
                else if (tsection8.IsVisible)
                {
                    tsection8.IsVisible = false;
                    tsection7.IsVisible = true;
                }
                else if (tsection7.IsVisible)
                {
                    tsection7.IsVisible = false;
                    tsection6.IsVisible = true;
                }
                else if (tsection6.IsVisible)
                {
                    tsection6.IsVisible = false;
                    tsection5.IsVisible = true;
                }
                else if (tsection5.IsVisible)
                {
                    tsection5.IsVisible = false;
                    tsection4.IsVisible = true;
                }
                else if (tsection4.IsVisible)
                {
                    tsection4.IsVisible = false;
                    tsection3.IsVisible = true;
                }
                else if (tsection3.IsVisible)
                {
                    tsection3.IsVisible = false;
                    tsection2.IsVisible = true;
                }
                else if (tsection2.IsVisible)
                {
                    tsection2.IsVisible = false;
                    tsection1.IsVisible = true;
                    prevbtn.IsVisible = false;

                }
            }
            else if(familyhistorystack.IsVisible)
            {
                if (fhsection3.IsVisible)
                {
                    fhsection3.IsVisible = false;
                    fhsection2.IsVisible = true;
                    prevbtn.IsVisible = true;
                    nextbtn.IsVisible = true;
                    submitbtn.IsVisible = false;

                }
                else if (fhsection2.IsVisible)
                {
                    fhsection2.IsVisible = false;
                    fhsection1.IsVisible = true;
                    nextbtn.IsVisible = true;
                    prevbtn.IsVisible = false;
                }
            }

        }
        catch(Exception ex)
        {

        }
    }
}