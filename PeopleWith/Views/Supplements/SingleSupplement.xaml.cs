//using Android.Views;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleSupplement : ContentPage
{
    ObservableCollection<usersupplement> UserMedications = new ObservableCollection<usersupplement>();
    ObservableCollection<MedtimesDosages> Schedule = new ObservableCollection<MedtimesDosages>();
    usersupplement MedSelected = new usersupplement();
    string[] freqSplit;
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


    //public SingleMedication()
    //{
    //	InitializeComponent();
    //}

    public SingleSupplement(ObservableCollection<usersupplement> AllUserMedications, usersupplement Selectedmed)
    {
        try
        {
            InitializeComponent();
            UserMedications = AllUserMedications;
            MedSelected = Selectedmed;
            Schedule = MedSelected.schedule;
            string FreqCheck = String.Empty; 

            Medicationname.Text = MedSelected.supplementtitle;
            if (MedSelected.frequency.Contains("|"))
            {
                var freq = MedSelected.frequency.Split('|');
                if (freq[0] == "As Required")
                {
                    FreqCheck = "As Required";
                    if (Schedule.Count == 0)
                    {
                        MedtimesDosages NewSchedule = new MedtimesDosages();
                        NewSchedule.Times = freq[0];
                        Schedule.Add(NewSchedule);
                    }
                    else
                    {
                        foreach (var item in Schedule)
                        {
                            item.Times = freq[0];
                        }
                    }
                    if (MedSelected.feedback == null)
                    {
                        lblvalue.Text = "0 Recorded";
                        lblvalue.FontSize = 18;
                    }
                    else 
                    {
                        lblvalue.Text = MedSelected.feedback.Count().ToString() + " Recorded";
                        lblvalue.FontSize = 18;
                    }
                    
                }
                else
                {
                    lblvalue.Text = MedSelected.schedule[0].Dosage;
                    freqSplit = MedSelected.frequency.Split('|');
                    int Index = 0;

                    foreach (var item in Schedule)
                    {

                        if (freqSplit[0] == "Weekly" || freqSplit[0] == "Weekly ")
                        {
                            item.Times = "1 " + MedSelected.preparation;
                            var day = MedSelected.TimeDosage[Index].Split('|');

                            if(day.Count() == 4)
                            {
                                //var msg = day[1] + "|" + day[2]; 
                                item.Type = day[3];
                            }
                            else
                            {
                                item.Type = day[2];
                            }

                            
                        }
                        else if (freqSplit[0] == "Days Interval")
                        {
                            item.Times = MedSelected.preparation;
                            //var day = MedSelected.TimeDosage[Index].Split('|');
                            if(freqSplit[1] == "2")
                            {
                                item.Type = "Every other day";
                            }
                            else
                            {
                                item.Type = "Every " + freqSplit[1] + " days";
                            }
                        }
                        else
                        {
                            item.Times = "1 " + MedSelected.preparation;
                            item.Type = freqSplit[0];
                        }
                        Index = Index + 1;

                    }
                }
            }
            else
            {
                if (Schedule.Count == 0)
                {
                    MedtimesDosages NewSchedule = new MedtimesDosages();
                    NewSchedule.Times = MedSelected.frequency;
                    Schedule.Add(NewSchedule);
                }
                else
                {
                    foreach (var item in Schedule)
                    {
                        item.Times = MedSelected.frequency;
                    }
                }
                lblvalue.Text = "N/A";
            }
            //var freq = MedSelected.frequency.Split('|');
            //if (freq[0] == "As Required")
            //{
            //    int Getlength = MedSelected.unit.Length;
            //    string unitText = MedSelected.unit;
            //    if (!unitText.EndsWith("s"))
            //    {
            //        unitText += "s"; 
            //    }
            //    lblunit.Text = unitText;
            //}
            //else
            //{
            //lblunit.Text = MedSelected.unit;
            //}

            if (string.IsNullOrEmpty(FreqCheck))
            {
                lblunit.Text = MedSelected.unit;
            }

            unitlbl.Text = MedSelected.unit;
            lblStart.Text = MedSelected.startdate;
            if (string.IsNullOrEmpty(MedSelected.enddate))
            {
                lblEnd.Text = "--";
            }
            else
            {
                lblEnd.Text = MedSelected.enddate;
            }


            ScheduleTimes.ItemsSource = Schedule;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                ScheduleBtn.IsEnabled = false;
                await Navigation.PushAsync(new MainSchedule(), false);
                ScheduleBtn.IsEnabled = true;
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

    async private void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                showallbtn.IsEnabled = false;
                await Navigation.PushAsync(new ShowAllSupplement(MedSelected), false);
                showallbtn.IsEnabled = true;
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

    async private void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        //Delete Medication 
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                DeleteBtn.IsEnabled = false;
                bool Result = await DisplayAlert("Delete Supplement", "Are you sure you would like to Delete this Supplement, it cannot be retrieved once Deleted", "Delete", "Cancel");
                if (Result)
                {
                    //Delete
                    MedSelected.deleted = true;

                    APICalls database = new APICalls();
                    await database.DeleteSupplement(MedSelected);

                    //Symptom Deleted Message
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Supplement Deleted") { });
                    await Task.Delay(1500);


                    await MopupService.Instance.PopAllAsync(false);

                    DeleteBtn.IsEnabled = true;
                    UserMedications.Remove(MedSelected);

                    await Navigation.PushAsync(new AllSupplements(UserMedications));
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllSupplements);
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                }
                else
                {
                    //Cancel
                    DeleteBtn.IsEnabled = true;
                    return;
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

    async private void EditMed_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                EditMed.IsEnabled = false;
                MedSelected.EditMedSection = "Details";
                await Navigation.PushAsync(new AddSupplement(UserMedications, MedSelected), false);
                EditMed.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }

            //string action = await DisplayActionSheet("Edit Medication", "Cancel", null, "Details", "Schedule");

            //if (action == "Details")
            //{
            //    MedSelected.EditMedSection = "Details"; 
            //    await Navigation.PushAsync(new AddMedication(UserMedications, MedSelected));
            //}
            //else if (action == "Schedule")
            //{
            //    MedSelected.EditMedSection = "Schedule"; 
            //    await Navigation.PushAsync(new AddMedication(UserMedications, MedSelected));
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Add Symptom Info Here
            await DisplayAlert("Supplement Information", "No Information is saved against this Supplement", "Close");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}