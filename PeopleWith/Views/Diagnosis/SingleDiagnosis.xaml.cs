using Microsoft.Azure.NotificationHubs;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleDiagnosis : ContentPage
{
    public ObservableCollection<userdiagnosis> AllUserDiagnosis = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosisPassed = new ObservableCollection<userdiagnosis>();
    string DateofDiagnosis;
    bool isEditing;
    bool validdob;
    protected override bool OnBackButtonPressed()
    {
        try
        {
            if (dateofBirth.IsVisible == true)
            {
                DiagnosisSingle.IsVisible = true;
                dateofBirth.IsVisible = false;
                Title = null;
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception Ex)
        {
            //Add Crash log
            return false;
        }

    }
    public SingleDiagnosis()
    {
        InitializeComponent();
    }

    public SingleDiagnosis(ObservableCollection<userdiagnosis> DiagPassed, ObservableCollection<userdiagnosis> AllDiagnosis)
    {
        try
        {
            InitializeComponent();
            DiagnosisPassed = DiagPassed;
            AllUserDiagnosis = AllDiagnosis;
            DiagnosisTitle.Text = DiagnosisPassed[0].diagnosistitle;
            var date = DateTime.Parse(DiagnosisPassed[0].dateofdiagnosis);
            DiagnosisDate.Text = "Diagnosed on: " + date.ToString("dd MMMM yyyy");
            DateofDiagnosis = DiagnosisPassed[0].dateofdiagnosis;
            DateEntry.Text = date.ToString("dd/MM/yyyy");

        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }

    async private void UpdateBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            DateTime DOB;
            if (DateTime.TryParse(DateofDiagnosis, out DOB))
            {

                if (DOB.Date <= DateTime.Now.Date)
                {
                    DateEntry.Unfocus();
                    dateofBirth.IsVisible = false;

                    NavigationPage.SetHasNavigationBar(this, false);

                    foreach (var item in DiagnosisPassed)
                    {
                        item.dateofdiagnosis = DateofDiagnosis;
                    }


                    APICalls database = new APICalls();
                    await database.PutDiagnosisAsync(DiagnosisPassed);

                    await MopupService.Instance.PushAsync(new PopupPageHelper("Diagnosis Updated") { });
                    await Task.Delay(1500);
                    foreach (var item in AllUserDiagnosis)
                    {
                        if (item.id == DiagnosisPassed[0].id)
                        {
                            item.dateofdiagnosis = DateofDiagnosis;
                        }
                    }
                    await MopupService.Instance.PopAllAsync(false);
                    await Navigation.PushAsync(new AllDiagnosis(AllUserDiagnosis));
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllDiagnosis);
                    if (pageToRemoves != null)
                    {

                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                }
                else if (DOB.Date > DateTime.Now.Date)
                {
                    DateEntry.Focus();
                    EntryError.IsVisible = true;
                    EntryError.Text = "Diagnosis Date is Greater than today's Date, Enter a Valid Date";
                    await Task.Delay(5000);
                    EntryError.Text = null;
                    EntryError.IsVisible = false;

                }
            }
            else
            {
                DateEntry.Focus();
                EntryError.IsVisible = true;
                EntryError.Text = "Diagnosis Date is Not Valid Date, Enter a Valid Date";
                await Task.Delay(5000);
                EntryError.Text = null;
                EntryError.IsVisible = false;

            }
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool Delete = await DisplayAlert("Delete Diagnosis", "Are you sure you would like the delete this Diagnosis?. Once deleted it cannot be retrieved", "Delete", "Cancel");
            if (Delete == true)
            {

                foreach (var item in DiagnosisPassed)
                {
                    item.deleted = true;
                }

                APICalls database = new APICalls();
                await database.DeleteDiagnosis(DiagnosisPassed);

                await MopupService.Instance.PushAsync(new PopupPageHelper("Diagnosis Deleted") { });
                await Task.Delay(1500);

                foreach (var item in AllUserDiagnosis)
                {
                    if (item.id == DiagnosisPassed[0].id)
                    {
                        item.deleted = true;
                    }
                }
                await MopupService.Instance.PopAllAsync(false);
                await Navigation.PushAsync(new AllDiagnosis(AllUserDiagnosis));

                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllDiagnosis);

                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                Navigation.RemovePage(this);

                return;
            }
            else
            {
                return;
            }
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }

    //private void DateEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    try
    //    {
    //        SfMaskedEntry maskedEntry = sender as SfMaskedEntry;

    //        var DOB = maskedEntry.Text;
    //        if (!DOB.Contains("_"))
    //        {
    //            UpdateBtn.IsEnabled = true;
    //        }
    //        else
    //        {
    //            UpdateBtn.IsEnabled = false;
    //        }
    //        DateofDiagnosis = DOB;
    //    }
    //    catch (Exception Ex)
    //    {
    //        //Add Crash log
    //    }
    //}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            DateEntry.Focus();
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }

    private void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            DiagnosisSingle.IsVisible = false;
            dateofBirth.IsVisible = true;
        }
        catch (Exception Ex)
        {
            //Add Crash log
        }
    }

    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {

        try
        {
            //Diagnosis Information Tapped 
        }
        catch (Exception Ex)
        {

        }
    }

    async private void DateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (isEditing)
                return;

            isEditing = true;

            string input = e.NewTextValue;

            // Remove any non-numeric characters except '/'
            input = new string(input.Where(c => char.IsDigit(c) || c == '/').ToArray());

            // Remove existing slashes to reformat correctly
            input = input.Replace("/", string.Empty);

            // Limit the input to a maximum of 8 numeric characters (DDMMYYYY)
            if (input.Length >= 8)
            {
                input = input.Substring(0, 8);
                DateEntry.IsEnabled = false;
                DateEntry.IsEnabled = true;
            }

            // Insert slashes at the appropriate positions
            if (input.Length > 2)
                input = input.Insert(2, "/");

            if (input.Length > 5)
                input = input.Insert(5, "/");

            // Check for valid date parts and set the text color accordingly
            if (input.Length == 10)
            {
                if (DateTime.TryParseExact(input, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
                {
                    // Check if the date is between 1900 and today's year
                    int currentYear = DateTime.Now.Year;
                    if (date.Year >= 1900 && date.Year <= currentYear)
                    {
                        DateEntry.TextColor = Color.FromArgb("#031926"); // Valid date
                        validdob = true;
                    }
                    else
                    {
                        DateEntry.TextColor = Colors.Red; // Invalid date range
                        validdob = false;
                    }
                }
                else
                {
                    DateEntry.TextColor = Colors.Red; // Invalid date
                    validdob = false;
                }
            }
            else
            {
                DateEntry.TextColor = Color.FromArgb("#031926"); // Intermediate input
                validdob = false;
            }

            DateEntry.Text = input;

            // Adjust cursor position
            DateEntry.CursorPosition = input.Length;
            AddBtn.IsEnabled = true;
            isEditing = false;
            DateofDiagnosis = DateEntry.Text;
        }
        catch (Exception Ex)
        {

        }
    }
}