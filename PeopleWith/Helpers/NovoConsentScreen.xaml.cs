using Mopups.Pages;
using Mopups.Services;

namespace PeopleWith;

public partial class NovoConsentScreen : PopupPage
{
    consent NovoConsent = new consent();
    public string GetArea;
    userfeedback userfeedbacklist = new userfeedback(); 
    public NovoConsentScreen(consent PassedConsent, string Area, userfeedback Userfeedbackpassed)
    {
        InitializeComponent();
        NovoConsent = PassedConsent;
        GetArea = Area;
        userfeedbacklist = Userfeedbackpassed;
        var Cleanbody = String.Empty; 
        if (!String.IsNullOrEmpty(NovoConsent.subtitle))
        {
            Cleanbody = NovoConsent.subtitle.Replace("[AREA]", Area);
        }
        title.Text = NovoConsent.title;
        body.Text = Cleanbody;
        var CleanString = String.Empty;
        if (!String.IsNullOrEmpty(NovoConsent.content))
        {
            CleanString = NovoConsent.content.Replace("IE24SX00005", "");
        }  
        Subtext.Text = CleanString;
        exitid.Text = NovoConsent.exitid;

    }

    private async void Agree_Clicked(object sender, EventArgs e)
    {

        //navigate to Intened Page
        if (GetArea == "Medications")
        {
            Preferences.Default.Set("NovoMeds", false);
            await Navigation.PushAsync(new AllMedications(), false);
        }
        else if (GetArea == "Symptoms")
        {
            Preferences.Default.Set("NovoSyms", false);
            await Navigation.PushAsync(new AllSymptoms(userfeedbacklist), false);
        }
        else if (GetArea == "Supplements")
        {
            Preferences.Default.Set("NovoSupps", false);
            await Navigation.PushAsync(new AllSupplements(), false);
        }
        else if (GetArea == "Measurements")
        {
            Preferences.Default.Set("NovoMeas", false);
            await Navigation.PushAsync(new MeasurementsPage(userfeedbacklist), false);
        }
        else if (GetArea == "Diagnosis")
        {
            Preferences.Default.Set("NovoDiag", false);
            await Navigation.PushAsync(new AllDiagnosis(), false);
        }
        else if (GetArea == "Mood")
        {
            Preferences.Default.Set("NovoMood", false);
            await Navigation.PushAsync(new AllMood(userfeedbacklist), false);
        }
        else if (GetArea == "Appointments")
        {
            Preferences.Default.Set("NovoAppt", false);
            await Navigation.PushAsync(new AllAppointments(), false);
        }
        else if (GetArea == "HCPs")
        {
            Preferences.Default.Set("NovoHcp", false);
            await Navigation.PushAsync(new HCPs(), false);
        }
        else if (GetArea == "Questionnaires")
        {
            Preferences.Default.Set("NovoQues", false);
            await Navigation.PushAsync(new AllQuestionnaires(), false);
        }
        else if (GetArea == "Allergens")
        {
            Preferences.Default.Set("NovoAllerg", false);
            await Navigation.PushAsync(new AllAllergies(), false);
        }
        else if (GetArea == "Health Report")
        {
            Preferences.Default.Set("NovoHeRep", false);
            await Navigation.PushAsync(new HealthReport(), false);
        }
        else if (GetArea == "Schedule")
        {
            Preferences.Default.Set("NovoSched", false);
            await Navigation.PushAsync(new MainSchedule(), false);
        }
        else if (GetArea == "Food Diary")
        {
            Preferences.Default.Set("NovoFood", false);
            //Change When added
            //await Navigation.PushAsync(new MainSchedule(), false);
        }
        else if (GetArea == "Diet")
        {
            Preferences.Default.Set("NovoDiet", false);
            await Navigation.PushAsync(new AllDiet(), false);
        }
        else if (GetArea == "Investigations")
        {
            Preferences.Default.Set("NovoInvest", false);
            await Navigation.PushAsync(new AllInvestigations(), false);
        }
        else if (GetArea == "Daily Activity")
        {
            Preferences.Default.Set("NovoActivity", false);
            await Navigation.PushAsync(new ActivitySchedule(), false);
        }


        await MopupService.Instance.RemovePageAsync(this);
    }

    private async void Disagree_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.RemovePageAsync(this);
    }

}