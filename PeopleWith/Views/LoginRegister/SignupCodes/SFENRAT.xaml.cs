//using Android.Graphics;
//using CoreText;
using PeopleWith;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Color = Microsoft.Maui.Graphics.Color;

namespace PeopleWith;

public partial class SFENRAT : ContentPage
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
    public userdiagnosis userdiag = new userdiagnosis();
    string surgery;

    ObservableCollection<symptom> symptomchipselectedlist = new ObservableCollection<symptom>();
    ObservableCollection<usersymptom> symptomstoadd = new ObservableCollection<usersymptom>();

    ObservableCollection<medication> filteredmedicationlist = new ObservableCollection<medication>();
    ObservableCollection<medication> additionalfilteredmedicationlist = new ObservableCollection<medication>();

    ObservableCollection<medication> medicationchipselectedlist = new ObservableCollection<medication>();
    ObservableCollection<usermedication> medicationstoadd = new ObservableCollection<usermedication>();
    ObservableCollection<userresponse> userresponselist = new ObservableCollection<userresponse>();
    question ctquestion;
    question commprefquestion;
    List<string> commprefaddedlist = new List<string>();

    public consent additonalconsent = new consent();

    user newuser;
    signupcode signupcodeinfo;
    public SFENRAT()
	{
		InitializeComponent();


		primaryconditionlist.Add("ACC");
		primaryconditionlist.Add("PPGL");

		pclist.ItemsSource = primaryconditionlist;

	}

    public SFENRAT(user userp, ObservableCollection<symptom> symtpomsp, ObservableCollection<medication> medicationsp, signupcode signupcodeinfop, double progressp, ObservableCollection<question> requestions, ObservableCollection<answer> reganswers, consent addtionalcon)
    {
        InitializeComponent();

        topprogress.SetProgress(progressp + 5, 0);

        primaryconditionlist.Add("Adrenal Cortical Cancer (ACC)");
        primaryconditionlist.Add("Phaeo Para Syndromes (Paraganglioma or Phaeochromocytoma (PPGL))");

        pclist.ItemsSource = primaryconditionlist;

        newuser = userp;
        allsymptomlist = symtpomsp;
        allmedicationlist = medicationsp;
        signupcodeinfo = signupcodeinfop;

        additonalconsent = addtionalcon;

        var splitlist = signupcodeinfo.symptoms.Split(',').ToList();

        foreach(var item in splitlist)
        {
            var symptom = allsymptomlist.Where(x => x.symptomid == item).SingleOrDefault();

            if(symptom != null)
            {
                filteredsymptomlist.Add(symptom);
            }


        }

        symptomchiplistnrat.ItemsSource = filteredsymptomlist;
        additionlsymlist.ItemsSource = allsymptomlist;

        additionalsymptomchiplistnrat.ItemsSource = additionalfilteredsymptomlist;


        var splitmedlist = signupcodeinfo.medications.Split(',').ToList();

        foreach(var item in splitmedlist)
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


        ctquestion = requestions.Where(x => x.title.Contains("Clinical Trials")).SingleOrDefault();

        if(ctquestion != null)
        {

            ctnamequestion.Text = ctquestion.title;
            ctquestiondes.Text = ctquestion.directions;

            var getanswers = reganswers.Where(x => x.questionid == ctquestion.questionid).ToList();

            ctlist.ItemsSource = getanswers;
        }

        commprefquestion = requestions.Where(x => x.title.Contains("Communication Preferences")).SingleOrDefault();

        if (commprefquestion != null)
        {

            commtitlequestion.Text = commprefquestion.title;
            commprefquestiondes.Text = commprefquestion.directions;

            var getanswers = reganswers.Where(x => x.questionid == commprefquestion.questionid).ToList();
            getanswers = getanswers.OrderBy(x => Convert.ToInt32(x.order)).ToList();

            compreflist.ItemsSource = getanswers;
        }

    }


    private void Button_Clicked(object sender, EventArgs e)
    {
		//yes to surgery button
		try
		{
            surgery = "Yes";

			yesbtn.BorderColor = Colors.Transparent;
			yesbtn.Background = Color.FromArgb("#BFDBF7");
			yesbtn.TextColor = Color.FromArgb("#03192");
			yesbtn.FontFamily = "HankenGroteskBold";

			doslbl.IsVisible = true;
			dateofsurgeryEntry.IsVisible = true;
			dosgrid.IsVisible = true;
			dosfloat.IsVisible = true;


            notbtn.BorderColor = Colors.LightGray;
			notbtn.Background = Colors.Transparent;
			notbtn.TextColor = Colors.Gray;
			notbtn.FontFamily = "HankenGroteskRegular";

        }
		catch(Exception ex)
		{

		}
    }

    private void notbtn_Clicked(object sender, EventArgs e)
    {
		//no to surgery button
		try
		{
            surgery = "No";

            notbtn.BorderColor = Colors.Transparent;
            notbtn.Background = Color.FromArgb("#BFDBF7");
            notbtn.TextColor = Color.FromArgb("#03192");
            notbtn.FontFamily = "HankenGroteskBold";

            doslbl.IsVisible = false;
            dateofsurgeryEntry.IsVisible = false;
            dosgrid.IsVisible = false;
			dosfloat.IsVisible = false;

            yesbtn.BorderColor = Colors.LightGray;
            yesbtn.Background = Colors.Transparent;
            yesbtn.TextColor = Colors.Gray;
            yesbtn.FontFamily = "HankenGroteskRegular";
        }
		catch(Exception ex)
		{

		}
    }

    private void dateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (isEditing)
                return;

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
                dateEntry.IsEnabled = false;
                dateEntry.IsEnabled = true;
            
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
                        dateEntry.TextColor = Color.FromArgb("#031926"); // Valid date
                        validdob = true;

                   
                    }
                    else
                    {
                        dateEntry.TextColor = Colors.Red; // Invalid date range
                        validdob = false;
                        ageentry.Text = string.Empty;
                    }
                }
                else
                {
                    dateEntry.TextColor = Colors.Red; // Invalid date
                    validdob = false;
                    ageentry.Text = string.Empty;
                }
            }
            else
            {
                dateEntry.TextColor = Color.FromArgb("#031926"); // Intermediate input
                validdob = false;
                ageentry.Text = string.Empty;
            }

            dateEntry.Text = input;

            // Adjust cursor position
            dateEntry.CursorPosition = input.Length;

            isEditing = false;

            if(validdob)
            {
                DateTime dateofdiag = DateTime.Parse(dateEntry.Text);
                DateTime dob = DateTime.Parse(newuser.dateofbirth);
                // DateTime diagnosisDate = diagdatepicker.Date;

                if (dateofdiag < dob)
                {
                    // throw new ArgumentOutOfRangeException("The calculated DateTime is out of range.");
                    ageentry.Text = "Invalid age";
                    return;
                }

                // Calculate the age
                TimeSpan timeSpan = dateofdiag - dob;
                DateTime ageDateTime = DateTime.MinValue + timeSpan;

                // Check for potential overflow
              
                
                    int age = ageDateTime.Year - 1;

                    if (age >= 0)
                    {
                        ageentry.Text = age.ToString();
                    }
                    else
                    {
                        ageentry.Text = "Invalid age"; // Or handle it as per your requirement
                    }
                
            }
        }
        catch (Exception ex)
        {

        }
    }

    private void symptomchiplistnrat_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var item = e.AddedItem as symptom;

            if(symptomchipselectedlist.Contains(item))
            {
                symptomchipselectedlist.Remove(item);
            }
            else
            {
                symptomchipselectedlist.Add(item);
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //symptom search entry

            if(string.IsNullOrEmpty(e.NewTextValue))
            {
                additionlsymlist.IsVisible = false;
                searchsymsentry.IsEnabled = false;
                searchsymsentry.IsEnabled = true;

            }
            else
            {

                var collectionone = allsymptomlist.Where(x => x.title.ToLowerInvariant().StartsWith(e.NewTextValue.ToLowerInvariant()));
                var count = collectionone.Count();
                if (count == 0)
                {
                    additionlsymlist.IsVisible = false;
        
                }
                else
                {
                    // emptyframe.IsVisible = false;
                    // resultsframe.IsVisible = true;
                    additionlsymlist.ItemsSource = collectionone;
                    additionlsymlist.HeightRequest = 400;
                    additionlsymlist.IsVisible = true;
         
                }

            }

        }
        catch(Exception ex)
        {

        }
    }

    private void additionlsymlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //additional symptom list tapped

            var item = e.DataItem as symptom;

            searchsymsentry.IsEnabled = false;
            searchsymsentry.IsEnabled = true;

            // Convert the selected item to a ChipItem
            if (additionalfilteredsymptomlist.Contains(item))
            {
                additionalfilteredsymptomlist.Remove(item);
                symptomchipselectedlist.Remove(item);
            }
            else
            {

                additionalfilteredsymptomlist.Add(item);
                symptomchipselectedlist.Add(item);
            }

            if(additionalfilteredsymptomlist.Count == 0)
            {
                addlbl1.IsVisible = false;
                additionalsymptomchiplistnrat.IsVisible = false;
            }
            else
            {
                addlbl1.IsVisible = true;
                additionalsymptomchiplistnrat.IsVisible = true;
            }

           



        }
        catch(Exception ex)
        {

        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if(e.Value)
            {
                dateEntry.IsEnabled = false;
                dateEntry.Text = string.Empty;
                ageentry.Text = string.Empty;
                dateEntry.Opacity = 0.3;
                ageentry.Opacity = 0.3;
            }
            else
            {
                dateEntry.IsEnabled = true;
                dateEntry.Opacity = 1;
                ageentry.Opacity = 0.3;
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void dateofsurgeryEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (isEditingsurgery)
                return;

            isEditingsurgery = true;

            string input = e.NewTextValue;

            // Remove any non-numeric characters except '/'
            input = new string(input.Where(c => char.IsDigit(c) || c == '/').ToArray());

            // Remove existing slashes to reformat correctly
            input = input.Replace("/", string.Empty);

            // Limit the input to a maximum of 8 numeric characters (DDMMYYYY)
            if (input.Length >= 8)
            {
                input = input.Substring(0, 8);
                dateofsurgeryEntry.IsEnabled = false;
                dateofsurgeryEntry.IsEnabled = true;
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
                        dateofsurgeryEntry.TextColor = Color.FromArgb("#031926"); // Valid date
                        validdobsurgery = true;
                    }
                    else
                    {
                        dateofsurgeryEntry.TextColor = Colors.Red; // Invalid date range
                        validdobsurgery = false;
                    }
                }
                else
                {
                    dateofsurgeryEntry.TextColor = Colors.Red; // Invalid date
                    validdobsurgery = false;
                }
            }
            else
            {
                dateofsurgeryEntry.TextColor = Color.FromArgb("#031926"); // Intermediate input
                validdobsurgery = false;
            }

            dateofsurgeryEntry.Text = input;

            // Adjust cursor position
            dateofsurgeryEntry.CursorPosition = input.Length;

            isEditingsurgery = false;
        }
        catch (Exception ex)
        {

        }
    }

    private void CheckBox_CheckedChanged_1(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (e.Value)
            {
                dateofsurgeryEntry.IsEnabled = false;
                dateofsurgeryEntry.Opacity = 0.3;
            }
            else
            {
                dateofsurgeryEntry.IsEnabled = true;
                dateofsurgeryEntry.Opacity = 1;
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
            //next button
            if(primaryconframe.IsVisible == true)
            {
                Handleprimaryconframe();
            }
            else if(dateofdiagframe.IsVisible == true)
            {
                Handledateofdiagframe();
            }
            else if(symptomsframe.IsVisible == true)
            {
                Handlesymptomsframe();
            }
            else if(medicationsframe.IsVisible == true)
            {
                Handlemedsframe();
            }
            else if(ctframe.IsVisible == true)
            {
                Handlectframe();
            }
            else if(comprefframe.IsVisible == true)
            {
                Handlecomprefframe();
            }

        }
        catch(Exception ex)
        {

        }
    }

    async void Handleprimaryconframe()
    {
        try
        {
            if(pclist.SelectedItems.Count == 0)
            {
                Vibration.Vibrate();
                return;
            }

            primaryconframe.IsVisible = false;
            dateofdiagframe.IsVisible = true;
            UpdateProgress();
        }
        catch(Exception ex)
        {

        }
    }

    async void Handledateofdiagframe()
    {
        try
        {

            //check if unknown date is selected
            if(diagdatecheckbox.IsChecked)
            {
                //unknown date
            }
            else
            {
                //check if date is valid
                if(dateEntry.TextColor != Colors.Red)
                {

                    if (ageentry.Text == "Invalid age")
                    {
                        dateEntry.Focus();
                        Vibration.Vibrate();
                        return;
                    }

                    userdiag.dateofdiagnosis = dateEntry.Text;
                }
                else
                {
                    dateEntry.Focus();
                    Vibration.Vibrate();
                    return;
                }
            }

  

            if(string.IsNullOrEmpty(surgery))
            {
                Vibration.Vibrate();
                return;
            }
            else
            {
                var agestring = "";

                if(string.IsNullOrEmpty(ageentry.Text))
                {
                    agestring = "Unknown";
                }
                else
                {
                    agestring = ageentry.Text;
                }

                if(surgery == "Yes")
                {
                    if(datesurgerycheckbox.IsChecked)
                    {
                        userdiag.additionalparameters = agestring + "|" + surgery + "|" + "Unknown";
                    }
                    else
                    {
                        if(dateofsurgeryEntry.TextColor  != Colors.Red)
                        {
                            userdiag.additionalparameters = agestring + "|" + surgery + "|" + dateofsurgeryEntry.Text;
                        }
                        else
                        {
                            dateofsurgeryEntry.Focus();
                            Vibration.Vibrate();
                            return;
                        }
                    }


                    
                }
                else
                {
                    //no clicked
                    //continue to next page 
                    userdiag.userid = newuser.userid;
                    userdiag.additionalparameters = agestring + "|" + surgery + "|" + "Unknown";

                }
            }

            dateofdiagframe.IsVisible = false;
            symptomsframe.IsVisible = true;
            skipbtn.IsVisible = true;
            UpdateProgress();


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

            foreach(var item in symptomchipselectedlist)
            {
                var neewitem = new usersymptom();
                neewitem.symptomid = item.symptomid;
                neewitem.symptomtitle = item.title;

                var sf = new symptomfeedback();
                sf.timestamp = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                Random random = new Random();
                int num = random.Next(10000000, 1000000000);
                sf.symptomfeedbackid = num.ToString();
                sf.intensity = "50";
                sf.notes = null;
                sf.triggers = null;
                sf.interventions = null;
                sf.duration = null;

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

            foreach(var item in medicationchipselectedlist)
            {

                var newitem = new usermedication();
                newitem.status = "Pending";
                newitem.medicationid = item.medicationid;
                newitem.medicationtitle = item.title;
                newitem.userid = newuser.userid;

                medicationstoadd.Add(newitem);


            }

            medicationsframe.IsVisible = false;
            ctframe.IsVisible = true;
            UpdateProgress();


        }
        catch(Exception ex)
        {

        }
    }

    async void Handlectframe()
    {
        try
        {
            if(ctlist.SelectedItem == null)
            {
                Vibration.Vibrate();
                return;
            }

            var item = ctlist.SelectedItem as answer;

            //add the question
            var response = new userresponse();
            response.questionid = ctquestion.questionid;
            response.answerid = item.answerid;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response.userid = newuser.userid;
            userresponselist.Add(response);

            ctframe.IsVisible = false;
            comprefframe.IsVisible = true;
            UpdateProgress();

        }
        catch(Exception ex)
        {

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

            //add the question
            var response = new userresponse();
            response.questionid = commprefquestion.questionid;
            var getallanswers = string.Join(",", commprefaddedlist);
            response.answerid = getallanswers;
            response.responsedate = DateTime.Now.ToString("dd/MM/yyyy");
            response.userid = newuser.userid;
            userresponselist.Add(response);


            UpdateProgress();

            await Navigation.PushAsync(new RegisterFinalPage(newuser, topprogress.Progress, userresponselist, additonalconsent, symptomstoadd, medicationstoadd, userdiag), false);

        }
        catch (Exception ex)
        {

        }
    }
    private void pclist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as string;

            userdiag.diagnosistitle = item;

            if(item.Contains("ACC"))
            {
                newuser.registrycondition = "ACC";
                userdiag.diagnosisid = "38AC079A-ECF4-473A-82A1-932EB839EA42";
            }
            else
            {
                newuser.registrycondition = "PPGL";
                userdiag.diagnosisid = "4561A0CB-7536-4712-9DF9-CE0E4FA5BEC9";
            }
        }
        catch(Exception ex)
        {

        }
    }

    async void UpdateProgress()
    {
        try
        {

            topprogress.Progress = topprogress.Progress += 5;


        }
        catch (Exception ex)
        {

        }

    }

    private void searchmedentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //medication search entry

            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                additionlmedlist.IsVisible = false;
                searchmedentry.IsEnabled = false;
                searchmedentry.IsEnabled = true;

            }
            else
            {

                var collectionone = allmedicationlist.Where(x => x.title.ToLowerInvariant().StartsWith(e.NewTextValue.ToLowerInvariant()));
                var count = collectionone.Count();
                if (count == 0)
                {
                    additionlmedlist.IsVisible = false;

                }
                else
                {
                    // emptyframe.IsVisible = false;
                    // resultsframe.IsVisible = true;
                    additionlmedlist.ItemsSource = collectionone;
                    additionlmedlist.HeightRequest = 400;
                    additionlmedlist.IsVisible = true;

                }

            }

        }
        catch (Exception ex)
        {

        }
    }

    private void additionlmedlist_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //additional symptom list tapped

            var item = e.DataItem as medication;

            searchmedentry.IsEnabled = false;
            searchmedentry.IsEnabled = true;

            // Convert the selected item to a ChipItem
            if (additionalfilteredmedicationlist.Contains(item))
            {
                additionalfilteredmedicationlist.Remove(item);
                medicationchipselectedlist.Remove(item);
            }
            else
            {

                additionalfilteredmedicationlist.Add(item);
                medicationchipselectedlist.Add(item);
            }

            if (additionalfilteredmedicationlist.Count == 0)
            {
                addmedlbl1.IsVisible = false;
                additionalmedicationchiplistnrat.IsVisible = false;
            }
            else
            {
                addmedlbl1.IsVisible = true;
                additionalmedicationchiplistnrat.IsVisible = true;
            }





        }
        catch (Exception ex)
        {

        }
    }

    private void medicationchiplistnrat_SelectionChanged(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var item = e.AddedItem as medication;

            if (medicationchipselectedlist.Contains(item))
            {
                medicationchipselectedlist.Remove(item);
            }
            else
            {
                medicationchipselectedlist.Add(item);
            }
        }
        catch(Exception ex)
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
        catch(Exception ex)
        {

        }
    }

    async void BackProgress()
    {
        try
        {
            topprogress.Progress = topprogress.Progress -= 5;
        }
        catch(Exception ex)
        {

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
                ctframe.IsVisible = true;
               
            }
            else if(ctframe.IsVisible == true)
            {
                ctframe.IsVisible = false;
                medicationsframe.IsVisible = true;
               
            }
            else if(medicationsframe.IsVisible == true)
            {
                medicationsframe.IsVisible = false;
                symptomsframe.IsVisible = true;
              
            }
            else if(symptomsframe.IsVisible == true)
            {
                symptomsframe.IsVisible = false;
                dateofdiagframe.IsVisible = true;
               
            }
            else if(dateofdiagframe.IsVisible == true)
            {
                dateofdiagframe.IsVisible = false;
                primaryconframe.IsVisible = true;

            }
            else if(primaryconframe.IsVisible == true)
            {
                MessagingCenter.Send<object>(this, "RemoveProgress");
                Navigation.RemovePage(this);
                return;
            }

            BackProgress();

        }
        catch(Exception ex)
        {

        }
    }
}