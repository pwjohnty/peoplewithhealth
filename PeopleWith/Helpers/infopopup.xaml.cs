using Mopups.Pages;
using Mopups.Services;

namespace PeopleWith;

public partial class Infopopup : PopupPage
{
	public Infopopup()
	{
		InitializeComponent();
	}

    public Infopopup(string message)
    {
        try
        {
            InitializeComponent();

            if (message == "symptom")
            {
                syminfo.IsVisible = true;
                titlelbl.Text = "What is a Symptom?"; 
            }
            else if (message == "med")
            {
                medinfo.IsVisible = true;
                titlelbl.Text = "What is a Medication?";
            }
            else if (message == "supp")
            {
                suppinfo.IsVisible = true;
                titlelbl.Text = "What is a Supplement?";
            }
            else if (message == "diag")
            {
                diaginfo.IsVisible = true;
                titlelbl.Text = "What is a Diagnosis?";
            }
            else if (message == "measure")
            {
                measurementinfo.IsVisible = true;
                titlelbl.Text = "What is a Measurement?";
            }
            else if (message == "question")
            {
                questioninfo.IsVisible = true;
                titlelbl.Text = "What is a Questionnaire?";
            }
            else if (message == "mood")
            {
                moodinfo.IsVisible = true;
                titlelbl.Text = "What is a Mood?";
            }
            else if (message == "allergy")
            {
                allergyinfo.IsVisible = true;
                titlelbl.Text = "What is an Allergy?";
            }
            else if (message == "hcp")
            {
                hcpinfo.IsVisible = true;
                titlelbl.Text = "What is a HCP?";
            }
        }
        catch (Exception Ex)
        {

        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {

        }
    }
}