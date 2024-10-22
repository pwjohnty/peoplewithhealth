//using Android.Views;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleMedication : ContentPage
{
    ObservableCollection<usermedication> UserMedications = new ObservableCollection<usermedication>();
    ObservableCollection<MedtimesDosages> Schedule = new ObservableCollection<MedtimesDosages>();
    usermedication MedSelected = new usermedication();
    string[] freqSplit;
    CrashDetected crashHandler = new CrashDetected();

    //public SingleMedication()
    //{
    //	InitializeComponent();
    //}

    public SingleMedication( ObservableCollection<usermedication> AllUserMedications, usermedication Selectedmed)
    {
        try
        {
            InitializeComponent();
            UserMedications = AllUserMedications;
            MedSelected = Selectedmed;
            Schedule = MedSelected.schedule; 

            Medicationname.Text = MedSelected.medicationtitle;
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
                    lblvalue.Text = MedSelected.feedback.Count().ToString() + " Taken";
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
                                item.Type = day[2];
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
              lblunit.Text = MedSelected.unit;
            //}
            
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
            await Navigation.PushAsync(new ShowAllMedication(MedSelected), false); 
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
            bool Result = await DisplayAlert("Delete Medication", "Are you sure you would like to Delete this Medicaiton, it cannot be retrieved once Deleted", "Delete", "Cancel");
            if (Result)
            {
                //Delete
                MedSelected.deleted = true; 

                APICalls database = new APICalls();
                await database.DeleteMedication(MedSelected);

                //Symptom Deleted Message
                await MopupService.Instance.PushAsync(new PopupPageHelper("Medication Deleted") { });
                await Task.Delay(1500);


                await MopupService.Instance.PopAllAsync(false);


                UserMedications.Remove(MedSelected); 

                await Navigation.PushAsync(new AllMedications(UserMedications));
                var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(p => p is AllMedications);
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
            await Navigation.PushAsync(new AddMedication(UserMedications, MedSelected), false);

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