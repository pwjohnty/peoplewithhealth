//using Android.Graphics;
using System.Collections.ObjectModel;
using Color = Microsoft.Maui.Graphics.Color;
using Mopups.Services;

namespace PeopleWith;

public partial class AddAsRequiredDosage : ContentPage
{
    APICalls aPICalls = new APICalls();
    MedtimesDosages newitem = new MedtimesDosages();
    usermedication usermed = new usermedication();
    usersupplement usersupp = new usersupplement();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }


    public AddAsRequiredDosage()
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

    public AddAsRequiredDosage(MedtimesDosages itempassed)
    {
        try
        {
            InitializeComponent();

            newitem = itempassed;

            addtimepicker.Time = DateTime.Now.TimeOfDay;

            if (itempassed.Type == "Medication")
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#e5f9f4");
                SubmitBtn.BackgroundColor = Colors.Teal;

            }
            else
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#f9f4e5");
                SubmitBtn.BackgroundColor = Color.FromArgb("#ac5735");
            }


            lblentryunit.Text = newitem.dosageunit;
            itemname.Text = newitem.Name;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }  
    }

    public AddAsRequiredDosage(MedtimesDosages itempassed, usermedication usermedpassed)
    {
        try
        {
            InitializeComponent();

            newitem = itempassed;
            usermed = usermedpassed;

            addtimepicker.Time = DateTime.Now.TimeOfDay;

            if (itempassed.Type == "Medication")
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#e5f9f4");
                SubmitBtn.BackgroundColor = Colors.Teal;

            }
            else
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#f9f4e5");
                SubmitBtn.BackgroundColor = Color.FromArgb("#ac5735");
            }

            lblentryunit.Text = newitem.dosageunit;
            itemname.Text = newitem.Name;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }      
    }

    public AddAsRequiredDosage(MedtimesDosages itempassed, usersupplement usersupppassed)
    {
        try
        {
            InitializeComponent();

            newitem = itempassed;
            usersupp = usersupppassed;

            addtimepicker.Time = DateTime.Now.TimeOfDay;

            if (itempassed.Type == "Medication")
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#e5f9f4");
                SubmitBtn.BackgroundColor = Colors.Teal;

            }
            else
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#f9f4e5");
                SubmitBtn.BackgroundColor = Color.FromArgb("#ac5735");
            }


            lblentryunit.Text = newitem.dosageunit;
            itemname.Text = newitem.Name;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }   
    }

    private async void SubmitBtn_Clicked(object sender, EventArgs e)
    {
		try
		{
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                SubmitBtn.IsEnabled = false;
                if (string.IsNullOrEmpty(unitentry.Text))
                {
                    unitentry.Focus();
                    Vibration.Vibrate();
                }
                else
                {
                    if (newitem.Type == "Medication")
                    {

                        var newfeedback = new MedSuppFeedback();
                        Random random = new Random();
                        int randomNumber = random.Next(100000, 100000001);

                        newfeedback.id = randomNumber.ToString();
                        newfeedback.Recorded = unitentry.Text;


                        var dt = adddatepicker.Date + addtimepicker.Time;
                        newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");

                        if (usermed.feedback == null || !usermed.feedback.Any())
                        {
                            //feedback is null initalize before hand 
                            usermed.feedback = new ObservableCollection<MedSuppFeedback>();
                        }

                        usermed.feedback.Add(newfeedback);
                        await aPICalls.UpdateMedicationFeedbackAsync(usermed);


                    }
                    else
                    {
                        var newfeedback = new MedSuppFeedback();
                        Random random = new Random();
                        int randomNumber = random.Next(100000, 100000001);

                        newfeedback.id = randomNumber.ToString();
                        newfeedback.Recorded = unitentry.Text;


                        var dt = adddatepicker.Date + addtimepicker.Time;
                        newfeedback.datetime = dt.ToString("HH:mm, dd/MM/yyyy");

                        if (usersupp.feedback == null || !usersupp.feedback.Any())
                        {
                            //feedback is null initalize before hand 
                            usersupp.feedback = new ObservableCollection<MedSuppFeedback>();
                        }

                        usersupp.feedback.Add(newfeedback);
                        await aPICalls.UpdateSupplementFeedbackAsync(usersupp);
                    }

                    SubmitBtn.IsEnabled = true;
                    var type = newitem.Type + " Feedback Updated";

                    await MopupService.Instance.PushAsync(new PopupPageHelper(type) { });
                    await Task.Delay(1000);

                    await Navigation.PushAsync(new MainSchedule());


                    await MopupService.Instance.PopAllAsync(false);

                    var pageToRemoves = Navigation.NavigationStack.ToList();

                    int ii = 0;

                    foreach (var page in pageToRemoves)
                    {
                        if (ii == 0)
                        {
                        }
                        else if (ii == 1 || ii == 2)
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
}