using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleHCP : ContentPage
{
    public ObservableCollection<hcp> AllUserHCPs = new ObservableCollection<hcp>();
    public ObservableCollection<appointment> AllAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> UpcomingAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> HistoricalAppointments = new ObservableCollection<appointment>();
    public ObservableCollection<appointment> HCPAppointments = new ObservableCollection<appointment>();
    hcp HCPSelected = new hcp();
    public List<hcp> ContactDetailsList = new List<hcp>();
    APICalls database = new APICalls();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public SingleHCP(hcp SelectedHCP, ObservableCollection<hcp> AllHCPsPassed)
	{
        try
        {
            InitializeComponent();
            HCPSelected = SelectedHCP;
            AllUserHCPs = AllHCPsPassed;

            Namelbl.Text = HCPSelected.fullname;
            Rolelbl.Text = HCPSelected.role;
            locationlbl.Text = HCPSelected.locationname;

            PopulateContactDetails();
            GetAppointments();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public void PopulateContactDetails()
    {
        try
        {
            //Address
            var Address = new hcp();
            Address.ContactTitle = "Address";
            if (string.IsNullOrEmpty(HCPSelected.addresslineone))
            {
                Address.ContactItem = "--";
            }
            else
            {
                Address.ContactItem = HCPSelected.addresslineone;
            }
            ContactDetailsList.Add(Address);

            //Town/City
            var Town = new hcp();
            Town.ContactTitle = "Town/City";
            if (string.IsNullOrEmpty(HCPSelected.towncity))
            {
                Town.ContactItem = "--";
            }
            else
            {
                Town.ContactItem = HCPSelected.towncity;
            }
            ContactDetailsList.Add(Town);

            //Country
            var Country = new hcp();
            Country.ContactTitle = "Country";
            if (string.IsNullOrEmpty(HCPSelected.country))
            {
                Country.ContactItem = "--";
            }
            else
            {
                Country.ContactItem = HCPSelected.country;
            }
            ContactDetailsList.Add(Country);
       
            //County
            var County = new hcp();
            County.ContactTitle = "County";
            if (string.IsNullOrEmpty(HCPSelected.county))
            {
                County.ContactItem = "--";
            }
            else
            {
                County.ContactItem = HCPSelected.county;
            }
            ContactDetailsList.Add(County);

            //postcode 
            var postcode = new hcp();
            postcode.ContactTitle = "Postcode";
            if (string.IsNullOrEmpty(HCPSelected.postcode))
            {
                postcode.ContactItem = "--";
            }
            else
            {
                postcode.ContactItem = HCPSelected.postcode;
            }
            ContactDetailsList.Add(postcode);

            //Telephone  
            var Telephone = new hcp();
            Telephone.ContactTitle = "Telephone";
            if (string.IsNullOrEmpty(HCPSelected.telephone))
            {
                Telephone.ContactItem = "--";
            }
            else
            {
                Telephone.ContactItem = HCPSelected.telephone;
            }
            ContactDetailsList.Add(Telephone);

            //Email  
            var Email = new hcp();
            Email.ContactTitle = "Email";
            if (string.IsNullOrEmpty(HCPSelected.email))
            {
                Email.ContactItem = "--";
            }
            else
            {
                Email.ContactItem = HCPSelected.email;
            }
            ContactDetailsList.Add(Email);

            HCPContactDetails.ItemsSource = ContactDetailsList;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public async void GetAppointments()
    {
        try
        {
            AllAppointments = await database.GetUserAppointment();

           foreach(var item in AllAppointments)
            {
                if(HCPSelected.hcpid == item.hcpid)
                {
                    HCPAppointments.Add(item);
                }
            }

           if(HCPAppointments.Count > 0)
            {
                foreach(var item in HCPAppointments)
                {
                    item.datetimeConverted = DateTime.Parse(item.datetime);

                    if(item.datetimeConverted > DateTime.Now)
                    {
                        UpcomingAppointments.Add(item);
                    }
                    else
                    {
                        HistoricalAppointments.Add(item);
                    }
                }

                if(UpcomingAppointments.Count > 0)
                {
                    Upcominglbl.IsVisible = true;
                    UpcomingListview.IsVisible = true;
                    UpcomingListview.ItemsSource = UpcomingAppointments.OrderBy(X => X.datetimeConverted);
                }
                if(HistoricalAppointments.Count > 0)
                {
                    Historicalbl.IsVisible = true;
                    HistoricalListview.IsVisible = true;
                    HistoricalListview.ItemsSource = HistoricalAppointments.OrderBy(X => X.datetimeConverted);
                }

                showalllbl2.Text = "Tap to view all Appointments With this HCP";
                showallbtn.Text = "View Appointments";

            }
            else
            {

                showalllbl2.Text = "Tap to add an appointment with this HCP";
                showallbtn.Text = "Add Appointment";
                //NoAppointmentsStack.IsVisible = true; 
            }
           
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        //Edit HCP
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                Editbtn.IsEnabled = false;
                bool IsEdit = true;
                await Navigation.PushAsync(new AddHCPs(AllUserHCPs, IsEdit, HCPSelected), false);
                Editbtn.IsEnabled = true;
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

    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                Deletebtn.IsEnabled = false;
                var Result = await DisplayAlert("Delete HCP", "Are you sure you would like to delete this HCP? Once Deleted it cannot be retrieved.", "Accept", "Decline");
                if (Result)
                {
                    //Accept
                    HCPSelected.deleted = true;
                    await database.DeleteUserHCP(HCPSelected);
                    AllUserHCPs.Remove(HCPSelected);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("HCP Deleted") { });
                    await Task.Delay(1500);

                    await Navigation.PushAsync(new HCPs(AllUserHCPs), false);

                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is HCPs);
                    if (pageToRemoves != null)
                    {

                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);

                    await MopupService.Instance.PopAllAsync(false);
                }
                else
                {
                    //Decline 
                    Deletebtn.IsEnabled = true;
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

    async private void AppointmentsListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                var SelectedAppoint = e.DataItem as appointment;
                await Navigation.PushAsync(new AppointmentFeedback(SelectedAppoint), false);
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

    async private void HistoricalListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                var SelectedAppoint = e.DataItem as appointment;
                await Navigation.PushAsync(new AppointmentFeedback(SelectedAppoint), false);
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

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                NoAppointFrame.IsEnabled = false;
                await Navigation.PushAsync(new AddAppointment(HCPSelected, AllUserHCPs), false);
                NoAppointFrame.IsEnabled = true;
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

    private async void showallbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                showallbtn.IsEnabled = false;
                if(showallbtn.Text == "Add Appointment")
                {
                    await Navigation.PushAsync(new AddAppointment(HCPSelected, AllUserHCPs), false);
                }
                else
                {
                    await Navigation.PushAsync(new AllAppointments(), false); 
                }
               
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
}