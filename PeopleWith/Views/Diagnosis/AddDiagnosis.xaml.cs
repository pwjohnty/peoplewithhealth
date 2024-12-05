using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq.Expressions;
using Mopups.Services;

namespace PeopleWith;

public partial class AddDiagnosis : ContentPage
{
    public ObservableCollection<diagnosis> DiagnosisList = new ObservableCollection<diagnosis>();
    public ObservableCollection<diagnosis> itemstoremove = new ObservableCollection<diagnosis>();
    public ObservableCollection<diagnosis> FilterResults = new ObservableCollection<diagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosistoAdd = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosisPassed = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosisAdded = new ObservableCollection<userdiagnosis>();
    string DateofDiagnosis = null;
    string diagnosisid;
    string diagnosistitle;
    bool isEditing;
    bool validdob;
    DateTime DateOBirth; 
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

    public AddDiagnosis()
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

    public AddDiagnosis(ObservableCollection<userdiagnosis> PassedDiagnosis)
    {
        try
        {
            InitializeComponent();
            GetBCCall();
            DiagnosisPassed = PassedDiagnosis;
            //viewModel = BindingContext as PopupViewModel;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async public void GetBCCall()
    {
        try
        {
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();  
            Diagnosisloading.IsVisible = true;

            APICalls database = new APICalls();
            DiagnosisList = await database.GetAsyncDiagnosis();
            //stopWatch.Stop();
            //TimeSpan ts = stopWatch.Elapsed;
            //await DisplayAlert("Time", ts.ToString(), "Close");

            //Remove Pending and inactive symptoms 
            foreach (var item in DiagnosisList)
            {
                if (item.Status != "active")
                {
                    itemstoremove.Add(item);
                }

            }

            //Get Symptoms user already added to remove from listview
            var DiagnosisAdded = new ObservableCollection<diagnosis>(DiagnosisList.Where(s => DiagnosisPassed.Any(x => x.diagnosistitle == s.Title)));

            foreach (var item in DiagnosisAdded)
            {
                itemstoremove.Add(item);
            }

            //Remove items from list
            foreach (var item in itemstoremove)
            {
                DiagnosisList.Remove(item);
            }

            FilterResults = DiagnosisList;

            DiagnosisListview.ItemsSource = DiagnosisList.OrderBy(s => s.Title);
            var count = DiagnosisList.Count.ToString();

            //Results inital count
            Results.Text = "Results" + " (" + count + ")";

            Diagnosisloading.IsVisible = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = searchbar.Text.ToString();
            var filteredSymptoms = new ObservableCollection<diagnosis>(FilterResults.Where(s => s.Title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.Title);

            DiagnosisListview.ItemsSource = filteredSymptoms;

            var count = filteredSymptoms.Count().ToString();

            Results.Text = "Results" + " (" + count + ")";
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    async private void DiagnosisListview_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                if (searchbar.IsSoftInputShowing())
                    await searchbar.HideSoftInputAsync(System.Threading.CancellationToken.None);
                var Diagnos = e.DataItem as diagnosis;
                var Diagtitle = Diagnos.Title.ToString();
                //popup.HeaderTitle = Diagnos.Title.ToString();
                //popup.IsOpen = true;
                //bool result = await viewModel.ShowPopupAsync();
                bool result = await DisplayAlert(Diagtitle, "Would you like to add this Diagnosis?", "Accept", "Decline");

                if (result)
                {
                    InitalDiagnosisAdd.IsVisible = false;
                    dateofBirth.IsVisible = true;
                    Diagnosislbl.Text = Diagnos.Title;
                    diagnosistitle = Diagnos.Title;
                    foreach (var item in DiagnosisList)
                    {
                        if (item.Title == diagnosistitle)
                        {
                            diagnosisid = item.Diagnosisid;
                        }
                    }
                    NavigationPage.SetHasNavigationBar(this, false);
                    if (DeviceInfo.Current.Platform == DevicePlatform.Android)
                    {
                        AndroidBtn.IsVisible = true;
                    }
                    else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
                    {
                        IOSBtn.IsVisible = true;
                    }

                }
                else
                {
                    // Declined
                    DiagnosisListview.SelectedItems.Clear();
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

    async private void AddBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            DateTime DOB;
            if (DateTime.TryParse(DateofDiagnosis, out DOB))
            {
                if (validdob == false)
                {
                    DateEntry.Focus();
                    EntryError.IsVisible = true;
                    EntryError.Text = "Diagnosis Date is Not a Valid Date, Enter a Valid Date";
                    await Task.Delay(5000);
                    EntryError.Text = null;
                    EntryError.IsVisible = false;
                    return; 

                }
                if (DOB.Date <= DateTime.Now.Date)
                {
                    DateOBirth = DOB; 
                    HandleDiagnosisAdd(); 
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
                if(diagdatecheckbox.IsChecked == true)
                {
                    HandleDiagnosisAdd();
                }
                else
                {
                    DateEntry.Focus();
                    EntryError.IsVisible = true;
                    EntryError.Text = "Diagnosis Date is Not a Valid Date, Enter a Valid Date";
                    await Task.Delay(5000);
                    EntryError.Text = null;
                    EntryError.IsVisible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void HandleDiagnosisAdd()
    {
        try
        {
            DateEntry.Unfocus();
            dateofBirth.IsVisible = false;
            AndroidBtn.IsVisible = false;
            IOSBtn.IsVisible = false;
            NavigationPage.SetHasNavigationBar(this, false);
            if(diagdatecheckbox.IsChecked != true)
            {
                DateofDiagnosis = DateOBirth.ToString("dd/MM/yyyy");
            }
            var Userid = Helpers.Settings.UserKey;
            var NewDiagnosis = new userdiagnosis();
            NewDiagnosis.userid = Userid;
            NewDiagnosis.diagnosisid = diagnosisid;
            NewDiagnosis.dateofdiagnosis = DateofDiagnosis;
            NewDiagnosis.diagnosistitle = diagnosistitle;
            NewDiagnosis.additionalparameters = null;
            NewDiagnosis.notes = null;
            DiagnosistoAdd.Add(NewDiagnosis);

            APICalls database = new APICalls();
            DiagnosisAdded = await database.PostUserDiagnosisAsync(DiagnosistoAdd);
            await MopupService.Instance.PushAsync(new PopupPageHelper("Diagnosis Added") { });
            await Task.Delay(1500);
            foreach (var item in DiagnosisAdded)
            {
                DiagnosisPassed.Add(item);
            }
            await MopupService.Instance.PopAllAsync(false);
            await Navigation.PushAsync(new AllDiagnosis(DiagnosisPassed));
            var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is AllDiagnosis);
            if (pageToRemoves != null)
            {
                Navigation.RemovePage(pageToRemoves);
            }
            Navigation.RemovePage(this);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    //private void SfMaskedEntry_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    try
    //    {
    //        //SfMaskedEntry maskedEntry = sender as SfMaskedEntry;
    //        //var DOB = maskedEntry.Text;
    //        //if (!DOB.Contains("_"))
    //        //{
    //        //    AddBtn.IsEnabled = true;
    //        //}
    //        //else
    //        //{
    //        //    AddBtn.IsEnabled = false;
    //        //}
    //        //DateofDiagnosis = DOB;
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
            NotasyncMethod(Ex);
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
            NotasyncMethod(Ex);
        }
    }

    async private void BackButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (dateofBirth.IsVisible == true)
            {
                AndroidBtn.IsVisible = false;
                IOSBtn.IsVisible = false;
                NavigationPage.SetHasNavigationBar(this, true);
                InitalDiagnosisAdd.IsVisible = true;
                dateofBirth.IsVisible = false;
                Title = null;
                DiagnosisListview.SelectedItem = null;
                searchbar.Focus();
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void diagdatecheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            DateEntry.Text = string.Empty;
            DateofDiagnosis = null;
            validdob = true; 
        }
        catch (Exception Ex)
        {

        }
    }
}