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
    CrashDetected crashHandler = new CrashDetected();

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

            Medicationname.Text = MedSelected.supplementtitle;
            if (MedSelected.frequency.Contains("|"))
            {
                var freq = MedSelected.frequency.Split('|');
                if (freq[0] == "As Required")
                {
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
                    lblvalue.Text = freq[1];
                }
                else
                {
                    lblvalue.Text = MedSelected.schedule[0].Dosage;
                    freqSplit = MedSelected.frequency.Split('|');

                    foreach (var item in Schedule)
                    {
                        if (freqSplit[0] == "Weekly" || freqSplit[0] == "Weekly ")
                        {
                            item.Times = "1 " + MedSelected.preparation;
                            item.Type = item.Day;
                        }
                        else
                        {
                            item.Times = "1 " + MedSelected.preparation;
                            item.Type = freqSplit[0];
                        }

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

            lblunit.Text = MedSelected.unit;
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

        }

    }

    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new MainSchedule(), false);
        }
        catch
        {
        }
    }

    async private void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ShowAllSupplement(MedSelected), false);
        }
        catch
        {
        }
    }

    async private void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        //Delete Medication 
        try
        {
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
                return;
            }



        }
        catch (Exception Ex)
        {
            //await crashHandler.CrashDetectedSend(Ex);
        }
    }

    async private void EditMed_Clicked(object sender, EventArgs e)
    {
        try
        {
            MedSelected.EditMedSection = "Details";
            await Navigation.PushAsync(new AddSupplement(UserMedications, MedSelected), false);

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
        }

    }
}