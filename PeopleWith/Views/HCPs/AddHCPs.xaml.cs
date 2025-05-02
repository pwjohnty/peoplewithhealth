using Mopups.Services;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;

namespace PeopleWith;

public partial class AddHCPs : ContentPage
{
    public ObservableCollection<hcp> AllUserHCPs = new ObservableCollection<hcp>();
    public ObservableCollection<hcp> NEWHCPtoAdd = new ObservableCollection<hcp>();
    hcp SelectedHCP = new hcp();
    static Regex ValidEmailRegex = CreateValidEmailRegex();
    APICalls database = new APICalls();
    bool IsEdit;
    bool AddContact; 
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

    public AddHCPs(ObservableCollection<hcp> AllHCPsPassed)
	{
        try
        {
            InitializeComponent();
            AllUserHCPs = AllHCPsPassed;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    public AddHCPs(ObservableCollection<hcp> AllHCPsPassed, bool Editis, hcp HCPSelected, bool AddContactClick)
    {
        try
        {
            InitializeComponent();
            IsEdit = Editis;
            AllUserHCPs = AllHCPsPassed;
            SelectedHCP = HCPSelected;
            AddContact = AddContactClick; 
            PopulateEdit();
            CheckAddContact(); 

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public void PopulateEdit()
    {
        try
        {
            HCPAdd.Text = "Update HCP";
            AddHCPtitle.Text = "Edit HCP";
            //Should always contain value 
            FirstNameEntry.Text = SelectedHCP.firstname;
            SurNameEntry.Text = SelectedHCP.surname;
            RoleEntry.Text = SelectedHCP.role;
            LocationEntry.Text = SelectedHCP.locationname;

            int i = 0;
            if (!string.IsNullOrEmpty(SelectedHCP.addresslineone))
            {
                addresslineoneEntry.Text = SelectedHCP.addresslineone;
                i = i + 1;
            }
            if (!string.IsNullOrEmpty(SelectedHCP.towncity))
            {
                towncityEntry.Text = SelectedHCP.towncity;
                i = i + 1;
            }
            if (!string.IsNullOrEmpty(SelectedHCP.country))
            {
                countryEntry.Text = SelectedHCP.country;
                i = i + 1;
            }
            if (!string.IsNullOrEmpty(SelectedHCP.county))
            {
                countyEntry.Text = SelectedHCP.county;
                i = i + 1;
            }
            if (!string.IsNullOrEmpty(SelectedHCP.postcode))
            {
                postcodeEntry.Text = SelectedHCP.postcode;
                i = i + 1;
            }
            if (!string.IsNullOrEmpty(SelectedHCP.telephone))
            {
                telephoneEntry.Text = SelectedHCP.telephone;
                i = i + 1;
            }
            if (!string.IsNullOrEmpty(SelectedHCP.email))
            {
                EmailEntry.Text = SelectedHCP.email;
                i = i + 1;
            }
            if (i == 0)
            {
                CheckBox.IsChecked = false;
            }
            else
            {
                CheckBox.IsChecked = true;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public async void CheckAddContact()
    {
        try
        {
            if (AddContact)
            {
                //Set Checkbox
                CheckBox.IsChecked = true;
                await Task.Delay(100); // Wait for the layout to complete
                await AddScrollview.ScrollToAsync(0, AddScrollview.ContentSize.Height, true);

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private static Regex CreateValidEmailRegex()
    {
        string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }
    internal static bool EmailIsValid(string emailAddress)
    {
        bool isValid = ValidEmailRegex.IsMatch(emailAddress);
        return isValid;
    }

    private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(EmailEntry.Text))
            {
                emailhelper.ShowHint = true;
            }
            else
            {
                emailhelper.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void telephoneEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(telephoneEntry.Text))
            {
                telephonehelper.ShowHint = true;
            }
            else
            {
                telephonehelper.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void FirstNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(FirstNameEntry.Text))
            {
                FirstNameTIL.ShowHint = true;
            }
            else
            {
                FirstNameTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SurNameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(SurNameEntry.Text))
            {
                SurNameTIL.ShowHint = true;
            }
            else
            {
                SurNameTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void LocationEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(LocationEntry.Text))
            {
                LocationTIL.ShowHint = true;
            }
            else
            {
                LocationTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void RoleEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(RoleEntry.Text))
            {
                RoleTIL.ShowHint = true;
            }
            else
            {
                RoleTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void addresslineoneEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(addresslineoneEntry.Text))
            {
                AddressTIL.ShowHint = true;
            }
            else
            {
                AddressTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void towncityEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
      try
        {

          if (!String.IsNullOrEmpty(towncityEntry.Text))
          {
              towncityTIL.ShowHint = true;
          }
          else
          {
            towncityTIL.ShowHint = false;
          }
      }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void countryEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(countryEntry.Text))
            {
                countryTIL.ShowHint = true;
            }
            else
            {
                countryTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void countyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(countyEntry.Text))
            {
                countyTIL.ShowHint = true;
            }
            else
            {
                countyTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void postcodeEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(postcodeEntry.Text))
            {
                PostcodeTIL.ShowHint = true;
                postcodeEntry.Text = e.NewTextValue.ToUpper();
            }
            else
            {
                PostcodeTIL.ShowHint = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void HCPAdd_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                HCPAdd.IsEnabled = false;
                //Only Check FirstName, Surname, Role, location
                if (string.IsNullOrEmpty(FirstNameEntry.Text) && string.IsNullOrEmpty(SurNameEntry.Text))
                {
                    FirstNameTIL.ErrorText = "Please enter a Firstname";
                    SurNameTIL.ErrorText = "or a Surname";
                    FirstNameTIL.HasError = true;
                    SurNameTIL.HasError = true;
                    Vibration.Vibrate();
                    FirstNameEntry.Focus();
                    await Task.Delay(2000);
                    FirstNameTIL.HasError = false;
                    SurNameTIL.HasError = false;
                    HCPAdd.IsEnabled = true;
                    return;
                }
                //else if (string.IsNullOrEmpty(SurNameEntry.Text))
                //{
                //    SurNameTIL.ErrorText = "Please enter a Surname";
                //    SurNameTIL.HasError = true;
                //    Vibration.Vibrate();
                //    SurNameEntry.Focus();
                //    await Task.Delay(2000);
                //    SurNameTIL.HasError = false;
                //    HCPAdd.IsEnabled = true;
                //    return;
                //}
               
                else if (string.IsNullOrEmpty(LocationEntry.Text))
                {
                    LocationTIL.ErrorText = "Please enter a Location";
                    LocationTIL.HasError = true;
                    Vibration.Vibrate();
                    LocationEntry.Focus();
                    await Task.Delay(2000);
                    LocationTIL.HasError = false;
                    HCPAdd.IsEnabled = true;
                    return;
                }
                else if (string.IsNullOrEmpty(RoleEntry.Text))
                {
                    RoleTIL.ErrorText = "Please enter a Role";
                    RoleTIL.HasError = true;
                    Vibration.Vibrate();
                    RoleEntry.Focus();
                    await Task.Delay(2000);
                    RoleTIL.HasError = false;
                    HCPAdd.IsEnabled = true;
                    return;
                }

                var NEWHCP = new hcp();
                if (!string.IsNullOrEmpty(FirstNameEntry.Text))
                {
                    NEWHCP.firstname = FirstNameEntry.Text;
                }
                else
                {
                    NEWHCP.firstname = null;
                }
                if (!string.IsNullOrEmpty(SurNameEntry.Text))
                {
                    NEWHCP.surname = SurNameEntry.Text;
                }
                else
                {
                    NEWHCP.surname = null;
                }

                NEWHCP.role = RoleEntry.Text;
                NEWHCP.locationname = LocationEntry.Text;

                //If optional Stack is Visible - Check Valid Email && Telephone 
                if (CheckBox.IsChecked == true)
                {
                    if (!string.IsNullOrEmpty(addresslineoneEntry.Text))
                    {
                        NEWHCP.addresslineone = addresslineoneEntry.Text;
                    }
                    else
                    {
                        NEWHCP.addresslineone = null; 
                    }
                    if (!string.IsNullOrEmpty(towncityEntry.Text))
                    {
                        NEWHCP.towncity = towncityEntry.Text;
                    }
                    else
                    {
                        NEWHCP.towncity = null;
                    }
                    if (!string.IsNullOrEmpty(countryEntry.Text))
                    {
                        NEWHCP.country = countryEntry.Text;
                    }
                    else
                    {
                        NEWHCP.country = null;
                    }
                    if (!string.IsNullOrEmpty(countyEntry.Text))
                    {
                        NEWHCP.county = countyEntry.Text;
                    }
                    else
                    {
                        NEWHCP.county = null;
                    }
                    if (!string.IsNullOrEmpty(postcodeEntry.Text))
                    {
                        NEWHCP.postcode = postcodeEntry.Text;
                    }
                    else
                    {
                        NEWHCP.postcode = null;
                    }
                    if (!string.IsNullOrEmpty(telephoneEntry.Text))
                    {
                        NEWHCP.telephone = telephoneEntry.Text;
                    }
                    else
                    {
                        NEWHCP.telephone = null;
                    }
                    if (!string.IsNullOrEmpty(EmailEntry.Text))
                    {
                        NEWHCP.email = EmailEntry.Text;
                    }
                    else
                    {
                        NEWHCP.email = null;
                    }
                }
                //Update HCP in the DB 
                if (IsEdit == true)
                {
                    SelectedHCP.firstname = NEWHCP.firstname;
                    SelectedHCP.surname = NEWHCP.surname;
                    SelectedHCP.role = NEWHCP.role;
                    SelectedHCP.locationname = NEWHCP.locationname;

                    if (!string.IsNullOrEmpty(NEWHCP.addresslineone))
                    {
                        SelectedHCP.addresslineone = NEWHCP.addresslineone;
                    }
                    else
                    {
                        SelectedHCP.addresslineone = null;
                    }
                    if (!string.IsNullOrEmpty(NEWHCP.towncity))
                    {
                        SelectedHCP.towncity = NEWHCP.towncity;
                    }
                    else
                    {
                        SelectedHCP.addresslineone = null;
                    }
                    if (!string.IsNullOrEmpty(NEWHCP.country))
                    {
                        SelectedHCP.country = NEWHCP.country;
                    }
                    else
                    {
                        SelectedHCP.country = null;
                    }
                    if (!string.IsNullOrEmpty(NEWHCP.county))
                    {
                        SelectedHCP.county = NEWHCP.county;
                    }
                    else
                    {
                        SelectedHCP.county = null;
                    }
                    if (!string.IsNullOrEmpty(NEWHCP.postcode))
                    {
                        SelectedHCP.postcode = NEWHCP.postcode;
                    }
                    else
                    {
                        SelectedHCP.postcode = null;
                    }
                    if (!string.IsNullOrEmpty(NEWHCP.telephone))
                    {
                        SelectedHCP.telephone = NEWHCP.telephone;
                    }
                    else
                    {
                        SelectedHCP.telephone = null;
                    }
                    if (!string.IsNullOrEmpty(NEWHCP.email))
                    {
                        SelectedHCP.email = NEWHCP.email;
                    }
                    else
                    {
                        SelectedHCP.email = null;
                    }
                    await database.UpdateHCPItem(SelectedHCP);

                    foreach (var item in AllUserHCPs)
                    {
                        if (SelectedHCP.hcpid == item.hcpid)
                        {
                            item.firstname = SelectedHCP.firstname;
                            item.surname = SelectedHCP.surname;
                            item.role = SelectedHCP.role;
                            item.locationname = SelectedHCP.locationname;
                            item.addresslineone = SelectedHCP.addresslineone;
                            item.towncity = SelectedHCP.towncity;
                            item.country = SelectedHCP.country;
                            item.county = SelectedHCP.county;
                            item.postcode = SelectedHCP.postcode;
                            item.telephone = SelectedHCP.telephone;
                            item.email = SelectedHCP.email;
                        }
                    }
                    await MopupService.Instance.PushAsync(new PopupPageHelper("HCP Updated") { });
                }
                else
                {
                    NEWHCP.userid = Helpers.Settings.UserKey;
                    NEWHCPtoAdd.Add(NEWHCP);
                    var AddedHCP = await database.PostUserHCPAsync(NEWHCPtoAdd);
                    foreach (var item in AddedHCP)
                    {
                        AllUserHCPs.Add(item);
                    }
                    await MopupService.Instance.PushAsync(new PopupPageHelper("HCP Added") { });
                }


                HCPAdd.IsEnabled = true;
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
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }           
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
           if( e.Value == true)
           {
                addresslineoneStack.IsVisible = true;
                towncity.IsVisible = true;
                country.IsVisible = true;
                county.IsVisible = true;
                postcode.IsVisible = true;
                telephoneStack.IsVisible = true;
                EmailStack.IsVisible = true; 
            }
            else
            {
                addresslineoneStack.IsVisible = false;
                towncity.IsVisible = false;
                country.IsVisible = false;
                county.IsVisible = false;
                postcode.IsVisible = false;
                telephoneStack.IsVisible = false;
                EmailStack.IsVisible = false;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}