using CommunityToolkit.Mvvm.Messaging;
using Mopups.Pages;
using Mopups.Services;

namespace PeopleWith;

public partial class Infopopup : PopupPage
{
    string Passed;
    public readonly struct UpdateBiometrics { }
    public Infopopup()
	{
		InitializeComponent();
	}

    public Infopopup(string message)
    {
        try
        {
            InitializeComponent();
            Passed = message; 
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
            else if (message == "appoint")
            {
                appointmentinfo.IsVisible = true;
                titlelbl.Text = "What is an Appointment?";
                Boxone.Background = Color.FromArgb("#ffe4e1");
                Boxone.BackgroundColor = Color.FromArgb("#ffe4e1");
                Boxtwo.Background = Color.FromArgb("#ffe4e1");
                Boxtwo.BackgroundColor = Color.FromArgb("#ffe4e1");
                Closebtn.BackgroundColor = Color.FromArgb("#ffe4e1");
            }
            else if (message == "diet")
            {
                Dietinfo.IsVisible = true;
                titlelbl.Text = "What is a Diet?";
                Boxone.Background = Color.FromArgb("#E8EFD8");
                Boxone.BackgroundColor = Color.FromArgb("#E8EFD8");
                Boxtwo.Background = Color.FromArgb("#E8EFD8");
                Boxtwo.BackgroundColor = Color.FromArgb("#E8EFD8");
                Closebtn.BackgroundColor = Color.FromArgb("#E8EFD8");
            }

            else if (message == "Invest")
            {
                InvestigationInfo.IsVisible = true;
                titlelbl.Text = "What is an Investigation?";
                Boxone.Background = Color.FromArgb("#F5E6E8");
                Boxone.BackgroundColor = Color.FromArgb("#F5E6E8");
                Boxtwo.Background = Color.FromArgb("#F5E6E8");
                Boxtwo.BackgroundColor = Color.FromArgb("#F5E6E8");
                Closebtn.BackgroundColor = Color.FromArgb("#F5E6E8");
            }

            else if (message == "activity")
            {
                DailyActivityInfo.IsVisible = true;
                titlelbl.Text = "What is an Activity?";
                Boxone.Background = Color.FromArgb("#fce9d9");
                Boxone.BackgroundColor = Color.FromArgb("#fce9d9");
                Boxtwo.Background = Color.FromArgb("#fce9d9");
                Boxtwo.BackgroundColor = Color.FromArgb("#fce9d9");
                Closebtn.BackgroundColor = Color.FromArgb("#fce9d9");
            }
            else if (message == "biometrics" || message == "Login" || message == "Reset")
            {
                LoginInfo.IsVisible = true;
                titlelbl.FontSize = 20; 
                titlelbl.Text = "Internet Connection Required";
                Boxone.IsVisible = false;
                Boxtwo.IsVisible = false; 
                Closebtn.BackgroundColor = Color.FromArgb("#031926");
                Closebtn.TextColor = Color.FromArgb("ffffff");
                this.Background = Color.FromArgb("#80808080"); 
                //Overlay.BackgroundColor = Color.FromArgb("#d3d3d3");
                titlelbl.HorizontalOptions = LayoutOptions.Center;
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

            //if(Passed == "biometrics")
            //{
            //    WeakReferenceMessenger.Default.Send(new BiometricsOpacity(1));
            //}
            //else if (Passed == "Login")
            //{
            //    WeakReferenceMessenger.Default.Send(new LoginOpacity(1));
            //}
            //else
            //{
            //    WeakReferenceMessenger.Default.Send(new ResetOpacity(1));
            //}
        }
        catch (Exception Ex)
        {

        }
    }
}