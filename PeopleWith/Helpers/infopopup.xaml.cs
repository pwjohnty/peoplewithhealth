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
                Boxone.Background = Color.FromArgb("#fff7ea");
                Boxone.BackgroundColor = Color.FromArgb("#fff7ea");
                Boxtwo.Background = Color.FromArgb("#fff7ea");
                Boxtwo.BackgroundColor = Color.FromArgb("#fff7ea");
                Closebtn.BackgroundColor = Color.FromArgb("#fff7ea");
            }
            else if (message == "med")
            {
                medinfo.IsVisible = true;
                titlelbl.Text = "What is a Medication?";
                Boxone.Background = Color.FromArgb("#e5f9f4");
                Boxone.BackgroundColor = Color.FromArgb("#e5f9f4");
                Boxtwo.Background = Color.FromArgb("#e5f9f4");
                Boxtwo.BackgroundColor = Color.FromArgb("#e5f9f4");
                Closebtn.BackgroundColor = Color.FromArgb("#e5f9f4");
            }
            else if (message == "supp")
            {
                suppinfo.IsVisible = true;
                titlelbl.Text = "What is a Supplement?";
                Boxone.Background = Color.FromArgb("#f9f4e5");
                Boxone.BackgroundColor = Color.FromArgb("#f9f4e5");
                Boxtwo.Background = Color.FromArgb("#f9f4e5");
                Boxtwo.BackgroundColor = Color.FromArgb("#f9f4e5");
                Closebtn.BackgroundColor = Color.FromArgb("#f9f4e5");
            }
            else if (message == "diag")
            {
                diaginfo.IsVisible = true;
                titlelbl.Text = "What is a Diagnosis?";
                Boxone.Background = Color.FromArgb("#E6E6FA");
                Boxone.BackgroundColor = Color.FromArgb("#E6E6FA");
                Boxtwo.Background = Color.FromArgb("#E6E6FA");
                Boxtwo.BackgroundColor = Color.FromArgb("#E6E6FA");
                Closebtn.BackgroundColor = Color.FromArgb("#E6E6FA");
            }
            else if (message == "measure")
            {
                measurementinfo.IsVisible = true;
                titlelbl.Text = "What is a Measurement?";
                Boxone.Background = Color.FromArgb("#e5f0fb");
                Boxone.BackgroundColor = Color.FromArgb("#e5f0fb");
                Boxtwo.Background = Color.FromArgb("#e5f0fb");
                Boxtwo.BackgroundColor = Color.FromArgb("#e5f0fb");
                Closebtn.BackgroundColor = Color.FromArgb("#e5f0fb");
            }
            else if (message == "question")
            {
                questioninfo.IsVisible = true;
                titlelbl.Text = "What is a Questionnaire?";
                Boxone.Background = Color.FromArgb("#fff9ec");
                Boxone.BackgroundColor = Color.FromArgb("#fff9ec");
                Boxtwo.Background = Color.FromArgb("#fff9ec");
                Boxtwo.BackgroundColor = Color.FromArgb("#fff9ec");
                Closebtn.BackgroundColor = Color.FromArgb("#fff9ec");
            }
            else if (message == "mood")
            {
                moodinfo.IsVisible = true;
                titlelbl.Text = "What is a Mood?";
                Boxone.Background = Color.FromArgb("#FFF8DC");
                Boxone.BackgroundColor = Color.FromArgb("#FFF8DC");
                Boxtwo.Background = Color.FromArgb("#FFF8DC");
                Boxtwo.BackgroundColor = Color.FromArgb("#FFF8DC");
                Closebtn.BackgroundColor = Color.FromArgb("#FFF8DC");
            }
            else if (message == "allergy")
            {
                allergyinfo.IsVisible = true;
                titlelbl.Text = "What is an Allergy?";
                Boxone.Background = Color.FromArgb("#FFF5EE");
                Boxone.BackgroundColor = Color.FromArgb("#FFF5EE");
                Boxtwo.Background = Color.FromArgb("#FFF5EE");
                Boxtwo.BackgroundColor = Color.FromArgb("#FFF5EE");
                Closebtn.BackgroundColor = Color.FromArgb("#FFF5EE");
            }
            else if (message == "hcp")
            {
                hcpinfo.IsVisible = true;
                titlelbl.Text = "What is a HCP?";
                Boxone.Background = Color.FromArgb("#CBC3E3");
                Boxone.BackgroundColor = Color.FromArgb("#CBC3E3");
                Boxtwo.Background = Color.FromArgb("#CBC3E3");
                Boxtwo.BackgroundColor = Color.FromArgb("#CBC3E3");
                Closebtn.BackgroundColor = Color.FromArgb("#CBC3E3");
            }
        }
        catch (Exception Ex)
        {

        }
    }

    //async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    //{
    //    try
    //    {
    //        await MopupService.Instance.PopAsync();
    //    }
    //    catch (Exception Ex)
    //    {

    //    }
    //}

    async private void Closebtn_Clicked(object sender, EventArgs e)
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