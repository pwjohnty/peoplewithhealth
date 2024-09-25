using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleMedication : ContentPage
{
    ObservableCollection<usermedication> UserMedications = new ObservableCollection<usermedication>();
    ObservableCollection<MedtimesDosages> Schedule = new ObservableCollection<MedtimesDosages>();
    usermedication MedSelected = new usermedication(); 

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
            lblvalue.Text = MedSelected.schedule[0].Dosage; 
            lblunit.Text = MedSelected.unit;
            unitlbl.Text = MedSelected.unit;
            var freqSplit = MedSelected.frequency.Split('|');
            if(freqSplit[0] == "Daily")
            {
                lblfreq.Text = "Every Day";
            }
            else
            {
                lblfreq.Text = freqSplit[0];
            }
            
            //if(freqSplit[1] == "1")
            //{
            //    lbltimes.Text = "Once";
            //}
            //else
            //{
            //    lbltimes.Text = freqSplit[1] + " Times";
            //}
           
            lblStart.Text = MedSelected.startdate;
            if (string.IsNullOrEmpty(MedSelected.enddate))
            {
                lblEnd.Text = "--";
            }
            else
            {
                lblEnd.Text = MedSelected.enddate;
            }

            foreach(var item in Schedule)
            {
                item.Times = "1 " + MedSelected.preparation;
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
            //await Navigation.PushAsync(new Schedule(),false)
        }
        catch 
        { 
        }
    }

    async private void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new ShowAllMedication(), false); 
        }
        catch
        {
        }
    }
}