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
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
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

            adddatepicker.Date = DateTime.Now;
            adddatepicker.MaximumDate = DateTime.Now;
            addtimepicker.Time = DateTime.Now.TimeOfDay;

            if (itempassed.Type == "Medication")
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#e5f9f4");
                unitentryframeDD.BackgroundColor = Color.FromArgb("#e5f9f4");
                SubmitBtn.BackgroundColor = Colors.Teal;

            }
            else
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#f9f4e5");
                unitentryframeDD.BackgroundColor = Color.FromArgb("#f9f4e5");
                SubmitBtn.BackgroundColor = Color.FromArgb("#ac5735");
            }

            lblentryunit.Text = newitem.dosageunit;
            itemname.Text = newitem.DisplayName;
            if (!string.IsNullOrEmpty(newitem.Name))
            {
                Nameitem.Text = newitem.Name;
                Nameitem.IsVisible = true;
            }

            if(newitem.dosageunit.Contains(" "))
            {
                newitem.DoubleDosage = true; 
            }

            if (newitem.DoubleDosage == true)
            {
                unitentryframe.IsVisible = false;
                unitentryframeDD.IsVisible = true;

                var SplitUnit = newitem.dosageunit.Split(' ');
                lblUnitOne.Text = SplitUnit[0] + " " + SplitUnit[1];
                lblUnittwo.Text =  SplitUnit[2];
            }
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

            adddatepicker.Date = DateTime.Now;
            adddatepicker.MaximumDate = DateTime.Now;
            addtimepicker.Time = DateTime.Now.TimeOfDay;

            if (itempassed.Type == "Medication")
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#e5f9f4");
                unitentryframeDD.BackgroundColor = Color.FromArgb("#e5f9f4");
                SubmitBtn.BackgroundColor = Colors.Teal;

            }
            else
            {
                unitentryframe.BackgroundColor = Color.FromArgb("#f9f4e5");
                unitentryframeDD.BackgroundColor = Color.FromArgb("#f9f4e5");
                SubmitBtn.BackgroundColor = Color.FromArgb("#ac5735");
            }

            lblentryunit.Text = newitem.dosageunit;
            itemname.Text = newitem.DisplayName;
            if (!string.IsNullOrEmpty(newitem.Name))
            {
                Nameitem.Text = newitem.Name;
                Nameitem.IsVisible = true; 
            }

            if (newitem.dosageunit.Contains(" "))
            {
                newitem.DoubleDosage = true;
            }

            if(newitem.DoubleDosage == true)
            {
                unitentryframe.IsVisible = false;
                unitentryframeDD.IsVisible = true;

                var SplitUnit = newitem.dosageunit.Split(' ');
                lblUnitOne.Text = SplitUnit[0];
                lblUnittwo.Text = SplitUnit[1] + " " + SplitUnit[2]; 

            }
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

                if (newitem.DoubleDosage == true)
                {
                    if (string.IsNullOrEmpty(unitentryOne.Text) || string.IsNullOrEmpty(unitentrytwo.Text))
                    {
                        if (string.IsNullOrEmpty(unitentryOne.Text))
                        {
                            unitentryOne.Focus(); 
                        }
                        else if (string.IsNullOrEmpty(unitentrytwo.Text))
                        {
                            unitentrytwo.Focus(); 
                        }
                        SubmitBtn.IsEnabled = true;
                        Vibration.Vibrate();
                        return; 
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(unitentry.Text))
                    {
                        unitentry.Focus();
                        SubmitBtn.IsEnabled = true;
                        Vibration.Vibrate();
                        return; 
                    }
                }

                  

                        if (newitem.Type == "Medication")
                        {

                            var newfeedback = new MedSuppFeedback();
                            Random random = new Random();
                            int randomNumber = random.Next(100000, 100000001);

                            newfeedback.id = randomNumber.ToString();
                            if(newitem.DoubleDosage == true)
                            {
                                newfeedback.Recorded = unitentryOne.Text + "|" + unitentrytwo.Text;
                            }
                            else
                            {
                                newfeedback.Recorded = unitentry.Text;
                            }
                            
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
                            if (newitem.DoubleDosage == true)
                            {
                                newfeedback.Recorded = unitentryOne.Text + "|" + unitentrytwo.Text;
                            }
                            else
                            {
                                newfeedback.Recorded = unitentry.Text;
                            }

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
                        if(pageToRemoves.Count == 6)
                        {
                            if (ii == 0)
                            {
                            }
                            else if (ii == 3 || ii == 4)
                            {
                                Navigation.RemovePage(page);
                            }
                            else
                            {
                                //Navigation.RemovePage(page);
                            }
                            ii++;
                        }
                        else
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