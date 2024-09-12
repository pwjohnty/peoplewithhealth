using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PeopleWith;

public partial class ProfileSection : ContentPage
{
    public List<user> UserDetailsList = new List<user>();
    public List<user> OtherDetailsList = new List<user>();
    public List<string> seconditemlist = new List<string>();
    public List<string> thirditemlist = new List<string>();

    //public ProfileSection()
    //{
    //	InitializeComponent();
    //}

    public ProfileSection()
    {
        try
        {
            InitializeComponent();

            //Icon and user's Name 
            var FirstName = Helpers.Settings.FirstName;
            var Surname = Helpers.Settings.Surname;

            GetHealthDetails();
            GetOtherDetails();


        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }
    private void GetHealthDetails()
    {
        try
        {
            //Name 
            var Name = new user();
            Name.SettingsTitle = "Name";

            var fullname = Helpers.Settings.FirstName + " " + Helpers.Settings.Surname;

            if (fullname == " ")
            {
                Name.SettingsItem = "--";
            }
            else
            {
                Name.SettingsItem = fullname;
            }

            //Email

            UserDetailsList.Add(Name);

            var Email = new user();
            Email.SettingsTitle = "Email";
            Email.SettingsItem = Helpers.Settings.Email;

            UserDetailsList.Add(Email);

            //Date of Birth
            var DOB = new user();
            DOB.SettingsTitle = "Date of Birth";

            if (Helpers.Settings.Age.Contains("00:00:00"))
            {
                var n = Helpers.Settings.Age;
                var nn = n.Replace("00:00:00", string.Empty);
                DOB.SettingsItem = nn;
            }
            else
            {
                DOB.SettingsItem = Helpers.Settings.Age;
            }
            UserDetailsList.Add(DOB);

            //Gender
            var Gender = new user();
            Gender.SettingsTitle = "Gender";
            Gender.SettingsItem = Helpers.Settings.Gender;

            UserDetailsList.Add(Gender);

            //Ethnicity
            var Ethnicity = new user();
            Ethnicity.SettingsTitle = "Ethnicity";
            Ethnicity.SettingsItem = Helpers.Settings.Ethnicity;

            UserDetailsList.Add(Ethnicity);

            //PhoneNumber
            var PhoneNumber = new user();
            PhoneNumber.SettingsTitle = "Phone Number";


            if (string.IsNullOrEmpty(Helpers.Settings.PhoneNumber))
            {
                PhoneNumber.SettingsItem = "--";
            }
            else
            {
                PhoneNumber.SettingsItem = Helpers.Settings.PhoneNumber;
            }



            UserDetailsList.Add(PhoneNumber);

            //Town/City
            var newuser444 = new user();
            newuser444.SettingsTitle = "Town/City";

            if (string.IsNullOrEmpty(Helpers.Settings.Town))
            {
                newuser444.SettingsItem = "--";
            }
            else
            {
                newuser444.SettingsItem = Helpers.Settings.Town;
            }


            UserDetailsList.Add(newuser444);

            //Height
            //var newuser3 = new user();
            //newuser3.SettingsTitle = "Height";

            //if (string.IsNullOrEmpty(Helpers.Settings.Height))
            //{
            //    newuser3.SettingsItem = "--";
            //}
            //else
            //{
            //    newuser3.SettingsItem = Helpers.Settings.Height;
            //}


            //UserDetailsList.Add(newuser3);

            //Weight
            //var newuser4 = new user();
            //newuser4.SettingsTitle = "Weight";


            //if (string.IsNullOrEmpty(Helpers.Settings.Weight))
            //{
            //    newuser4.SettingsItem = "--";
            //}
            //else
            //{
            //    newuser4.SettingsItem = Helpers.Settings.Weight;
            //}


            //UserDetailsList.Add(newuser4);

            UserDetails.ItemsSource = UserDetailsList;
        }
        catch (Exception Ex)
        {
            //Add Crash log 
        }
    }

    private void GetOtherDetails()
    {
        try
        {
        }
        catch (Exception Ex)
        {

        }
    }

    private void UserDetails_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }

    private void Settings_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }

    private void OtherDetails_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {

    }
}