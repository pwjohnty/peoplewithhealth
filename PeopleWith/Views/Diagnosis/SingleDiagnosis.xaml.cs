using Microsoft.Azure.NotificationHubs;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class SingleDiagnosis : ContentPage
{
    public ObservableCollection<userdiagnosis> AllUserDiagnosis = new ObservableCollection<userdiagnosis>();
    public ObservableCollection<userdiagnosis> DiagnosisPassed = new ObservableCollection<userdiagnosis>();
    diagnosis SelectedDiagnosis = new diagnosis(); 
    string DateofDiagnosis;
    bool isEditing;
    bool validdob;
    string WebpageLink;
    public string WeborPdf;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.CrashDetectedSend(Ex);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }
    public SingleDiagnosis()
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

    public SingleDiagnosis(ObservableCollection<userdiagnosis> DiagPassed, ObservableCollection<userdiagnosis> AllDiagnosis)
    {
        try
        {
            InitializeComponent();
            DiagnosisPassed = DiagPassed;
            AllUserDiagnosis = AllDiagnosis;

            if (DiagnosisPassed[0].status == "Pending")
            {
                //Add Diagnosis Date 
                NavigationPage.SetHasNavigationBar(this, false);
                EditBtn.IsEnabled = false;
                DiagnosisSingle.IsVisible = false;
                dateofBirth.IsVisible = true;
                Diagnosislbl.Text = DiagnosisPassed[0].diagnosistitle;
                EditBtn.IsEnabled = true;
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
                DiagnosisTitle.Text = DiagnosisPassed[0].diagnosistitle;
                var date = DateTime.Parse(DiagnosisPassed[0].dateofdiagnosis);
                DiagnosisDate.Text = "Diagnosed on: " + date.ToString("dd MMMM yyyy");
                DateofDiagnosis = DiagnosisPassed[0].dateofdiagnosis;
                DateEntry.Text = date.ToString("dd/MM/yyyy");

                loadMedInformation();

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async void loadMedInformation()
    {
        try
        {
            SelectedDiagnosis.Diagnosisid = DiagnosisPassed[0].diagnosisid;
            APICalls Database = new APICalls();
            var GetDiagInfo = await Database.GetAsyncSingleDiagnosis(SelectedDiagnosis);
            SelectedDiagnosis = GetDiagInfo;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void UpdateBtn_Clicked(object sender, EventArgs e)
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
                    DateEntry.Unfocus();
                    dateofBirth.IsVisible = false;

                    AndroidBtn.IsVisible = false;
                    IOSBtn.IsVisible = false; 

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
            NotasyncMethod(Ex);
        }
    }

    async private void Deletebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                Deletebtn.IsEnabled = false;
                bool Delete = await DisplayAlert("Delete Diagnosis", "Are you sure you would like the delete this Diagnosis? Once deleted it cannot be retrieved", "Delete", "Cancel");
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

                    Deletebtn.IsEnabled = true;
                    return;
                    
                }
                else
                {
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
            NotasyncMethod(Ex);
        }
    }

    private void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                NavigationPage.SetHasNavigationBar(this, false);
                EditBtn.IsEnabled = false;
                DiagnosisSingle.IsVisible = false;
                dateofBirth.IsVisible = true;
                Diagnosislbl.Text = DiagnosisPassed[0].diagnosistitle;
                EditBtn.IsEnabled = true;
                if(DeviceInfo.Current.Platform == DevicePlatform.Android)
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
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            if(SelectedDiagnosis.Diagnosisinformation != null)
            {
                var title = DiagnosisPassed[0].diagnosistitle;
                await Navigation.PushAsync(new DiagnosisInfo(SelectedDiagnosis, title), false); 

            }
            else
            {
                await DisplayAlert("Diagnosis Information", "No Information is saved against this Diagnosis", "Close");

            }
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
                        AddBtn.IsEnabled = true;
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

    async private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        try
        {
                     
            if(WeborPdf == "Web") 
            {
                DiagnosisSingle.IsVisible = false;
                WebViewerStack.IsVisible = true;
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
                var pdflink = WebpageLink;
                await Browser.OpenAsync(pdflink, new BrowserLaunchOptions
                {
                    LaunchMode = BrowserLaunchMode.SystemPreferred,
                    TitleMode = BrowserTitleMode.Hide
                });
            }
      
            EditBtn.IsEnabled = false;
            if (DeviceInfo.Current.Platform == DevicePlatform.Android)
            {
                WebView.HeightRequest = 700;
            }
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
                DiagnosisSingle.IsVisible = true;
                dateofBirth.IsVisible = false;
                AndroidBtn.IsVisible = false;
                IOSBtn.IsVisible = false; 
                Title = null;
                NavigationPage.SetHasNavigationBar(this, true); 
            }
            //else if (WebViewerStack.IsVisible == true)
            //{
            //    WebViewerStack.IsVisible = false;
            //    DiagnosisSingle.IsVisible = true;
            //    EditBtn.IsEnabled = true;
            //    AndroidBtn.IsVisible = false;
            //    IOSBtn.IsVisible = false;
            //    NavigationPage.SetHasNavigationBar(this, true);
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}