using Microsoft.Maui.Controls;
using Mopups.Services;
using System.Collections.ObjectModel;
namespace PeopleWith;
public partial class UpdateAllSymptoms : ContentPage
{
    //public ObservableCollection<symptomupdate> SymptomUpdatelist = new ObservableCollection<symptomupdate>();
    public ObservableCollection<usersymptom> SymptomUpdateNewlist = new ObservableCollection<usersymptom>();
    public ObservableCollection<usersymptom> UserSymptomsPassed = new ObservableCollection<usersymptom>();
   // public symptomupdate itemtoremove;
    string SelectedTime;
    string SelectedDate;
    string previous;
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();

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


    public UpdateAllSymptoms()
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
    public UpdateAllSymptoms(ObservableCollection<usersymptom> AllUserSymptoms)
    {
        try
        {
            InitializeComponent();
            UserSymptomsPassed = AllUserSymptoms;

            foreach (var item in UserSymptomsPassed)
            {
                item.Enabled = true;
                item.Opacity = 1;
                item.Slidervalue = Convert.ToDouble(item.CurrentIntensity);
                item.SlidervalueUA = Convert.ToDouble(item.CurrentIntensity);
                item.CurrentIntensityUA = item.CurrentIntensity;
                var dt = DateTime.Parse(item.feedback[item.feedback.Count - 1].timestamp).ToString("dd MMM, HH:mm");
                item.DateUpdatedAll = "Last updated: " + dt;
            }
            Datelbl.Text = DateTime.Now.ToString("dd MMM");
            Timelbl.Text = DateTime.Now.ToString("HH:mm");
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            adddatepicker.Date = DateTime.Now;
            adddatepicker.MaximumDate = DateTime.Now;

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                SymptomUpdateLV.HeightRequest = UserSymptomsPassed.Count * 80;
            }
            var sortedSymptoms = UserSymptomsPassed.OrderByDescending(f => DateTime.Parse(f.LastUpdatedTime)).ToList();
            SymptomUpdateLV.ItemsSource = sortedSymptoms;
            //change visual state on page load, as button is not updating;
            UpdateBtn.IsEnabled = true;
           // UpdateBtn.IsEnabled = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }

    public UpdateAllSymptoms(ObservableCollection<usersymptom> AllUserSymptoms, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            UserSymptomsPassed = AllUserSymptoms;
            userfeedbacklistpassed = userfeedbacklist;

            foreach (var item in UserSymptomsPassed)
            {
                item.Enabled = true;
                item.Opacity = 1;
                item.Slidervalue = Convert.ToDouble(item.CurrentIntensity);
                item.SlidervalueUA = Convert.ToDouble(item.CurrentIntensity);
                item.CurrentIntensityUA = item.CurrentIntensity;
                var dt = DateTime.Parse(item.feedback[item.feedback.Count - 1].timestamp).ToString("dd MMM, HH:mm");
                item.DateUpdatedAll = "Last updated: " + dt;
            }
            Datelbl.Text = DateTime.Now.ToString("dd MMM");
            Timelbl.Text = DateTime.Now.ToString("HH:mm");
            addtimepicker.Time = DateTime.Now.TimeOfDay;

            if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                SymptomUpdateLV.HeightRequest = UserSymptomsPassed.Count * 90;
            }
            var sortedSymptoms = UserSymptomsPassed.OrderByDescending(f => DateTime.Parse(f.LastUpdated)).ToList();
            SymptomUpdateLV.ItemsSource = UserSymptomsPassed;
            //change visual state on page load, as button is not updating;
            UpdateBtn.IsEnabled = true;
           // UpdateBtn.IsEnabled = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        try
        {
            var sfSlide = sender as Syncfusion.Maui.Sliders.SfSlider;
            if (sfSlide != null)
            {
                // Round the slider's value
                var roundedValue = Math.Round(sfSlide.Value);

                // Update the slider's value to the rounded one if necessary
                sfSlide.Value = roundedValue;

                // Set the rounded value in your model
                var selectedItem = sfSlide.BindingContext as usersymptom;
                if (selectedItem != null)
                {
                    selectedItem.CurrentIntensityUA = roundedValue.ToString();
                }
            }

        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            await Navigation.PopAsync();
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
 

            // Check Internet Connectivity
            if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
            {
                UpdateBtn.IsEnabled = false;

                // Prepare Feedback Data
                var updates = new List<feedbackdata>();
                var symptomUpdates = new List<symptom>();

                Parallel.ForEach(UserSymptomsPassed.Where(s => s.Enabled), symptom =>
                {
                    var items = new symptomfeedback
                    {
                        timestamp = $"{adddatepicker.Date:dd/MM/yyyy} {addtimepicker.Time:hh\\:mm}",
                        symptomfeedbackid = Guid.NewGuid().ToString().ToUpper(),
                        intensity = symptom.SlidervalueUA.ToString(),
                    };
                    lock (symptom.feedback)
                    {
                        symptom.feedback.Add(items);
                    }
                    lock (SymptomUpdateNewlist)
                    {
                        SymptomUpdateNewlist.Add(symptom);
                    }

                    updates.Add(new feedbackdata
                    {
                        value = symptom.SlidervalueUA.ToString(),
                        datetime = items.timestamp,
                        action = "update",
                        label = symptom.symptomtitle.TrimEnd(),
                        id = items.symptomfeedbackid
                    });
                });

                // Add Feedback Data to User List
                if (userfeedbacklistpassed.symptomfeedbacklist == null)
                {
                    userfeedbacklistpassed.symptomfeedbacklist = new ObservableCollection<feedbackdata>();
                }

                foreach (var update in updates)
                {
                    userfeedbacklistpassed.symptomfeedbacklist.Add(update);
                }

                // Serialize Feedback Data Once
                userfeedbacklistpassed.symptomfeedback = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.symptomfeedbacklist);

                // Show Loading Screen
                var popup = new PopupPageHelper("Updating...");

                // Show the Loading Popup
                await MopupService.Instance.PushAsync(popup);
                

                // Perform API Calls Concurrently
                var database = new APICalls();
                await Task.WhenAll(
                    database.UpdateAll(SymptomUpdateNewlist),
                    database.UserfeedbackUpdateSymptomData(userfeedbacklistpassed)
                );

                popup.UpdateMessage("Symptoms Updated");
                await Task.Delay(1000);
                //await MopupService.Instance.PopAllAsync(false);
                //await MopupService.Instance.PushAsync(new PopupPageHelper("Symptoms Updated"));
                // Navigate to Updated Symptoms Page
                await Navigation.PushAsync(new AllSymptoms(UserSymptomsPassed, userfeedbacklistpassed));
                // Dismiss Loading Screen
                await MopupService.Instance.PopAllAsync(false);

             

                // Remove Pages from Navigation Stack
                var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllSymptoms);
                if (pageToRemove != null)
                {
                    Navigation.RemovePage(pageToRemove);
                }
                Navigation.RemovePage(this);
                UpdateBtn.IsEnabled = true;

            }
            else
            {
                ConnectivityChanged?.Invoke(this, false);
            }
        }
        catch (Exception ex)
        {
            UpdateBtn.IsEnabled = true;
            NotasyncMethod(ex);
        }
    }
    private void SelectDatePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.DatePickerSelectionChangedEventArgs e)
    {
        try
        {
            var Date = e.NewValue;
            Datelbl.Text = Date.ToString("dd MMM");
            var split = Date.ToString().Split(' ');
            SelectedDate = split[0];
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TimePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.TimePickerSelectionChangedEventArgs e)
    {
        try
        {
            if (e.NewValue > DateTime.Now.TimeOfDay)
            {
                int secondsToVibrate = Random.Shared.Next(1, 1);
                TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
                Vibration.Default.Vibrate(vibrationLength);
                return;
            }
            else
            {
                var Time = e.NewValue.ToString();
                var split = Time.Split(':');
                Timelbl.Text = split[0] + ":" + split[1];
                var getTime = split[0] + ":" + split[1] + ":" + "00";
                SelectedTime = getTime;
            }
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
       
    }
    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Schedulepopup.IsOpen = true;
            Mainstack.Opacity = 0.2;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        try
        {
            Timepopup.IsOpen = true;
            Mainstack.Opacity = 0.2;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TimePicker_Closed(object sender, EventArgs e)
    {
        try
        {
            Mainstack.Opacity = 1;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void Schedulepopup_Closed(object sender, EventArgs e)
    {
        try
        {
            Mainstack.Opacity = 1;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TapGestureRecognizer_Tapped_3(object sender, TappedEventArgs e)
    {
        try
        {
            var frame = sender as Grid;
            var symptom = frame?.BindingContext as usersymptom;
            if (symptom == null)
                //return;
            foreach (var item in UserSymptomsPassed)
            {
                if (item.id == symptom.id)
                {
                    if (!item.Enabled)
                    {
                        item.Opacity = 1;
                        item.Enabled = true;
                    }
                    else
                    {
                        item.Opacity = 0.5;
                        item.Enabled = false;
                    }
                }
            }
            bool anyEnabled = UserSymptomsPassed.Any(item => item.Enabled);
            UpdateBtn.IsEnabled = anyEnabled;
            SymptomUpdateLV.ItemsSource = null;
            SymptomUpdateLV.ItemsSource = UserSymptomsPassed;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    async private void SelectAllBtn_Clicked(object sender, EventArgs e)
    {
        //if (SelectAllBtn.Text == "Select All")
        //{
        //    foreach (var item in SymptomUpdatelist)
        //    {
        //        item.Opacity = 1;
        //        item.Enabled = true;

        //    }
        //    SelectAllBtn.Text = "UnSelect All";
        //}
        //else
        //{
        //    foreach (var item in SymptomUpdatelist)
        //    {
        //        item.Opacity = 0.5;
        //        item.Enabled = false;
        //    }
        //    SelectAllBtn.Text = "Select All";
        //}
        //bool anyEnabled = SymptomUpdatelist.Any(item => item.Enabled);
        //UpdateBtn.IsEnabled = anyEnabled;
        //SymptomUpdateLV.ItemsSource = null;
        //SymptomUpdateLV.ItemsSource = SymptomUpdatelist;
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                SelectAll.IsEnabled = false;

                this.ToolbarItems.Clear();

                ToolbarItem itemm = new ToolbarItem
                {
                    Text = "UnSelect All"

                };
                itemm.Clicked += OnItemClicked;

                this.ToolbarItems.Add(itemm);
                SymptomUpdateLV.SelectAll();
                foreach (var item in UserSymptomsPassed)
                {
                    item.Enabled = true;
                }
                UpdateBtn.IsEnabled = true;
                SelectAll.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }

        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void OnItemClicked(object sender, EventArgs e)
    {
        try
        {
            //this.ToolbarItems.Clear();

            //ToolbarItem itemm = new ToolbarItem
            //{
            //    Text = "Select All"

            //};
            //itemm.Clicked += ToolbarItem_Clicked;
            //this.ToolbarItems.Add(itemm);
            //SymptomUpdateLV.SelectedItems.Clear();
            //foreach (var item in UserSymptomsPassed)
            //{
            //    item.Enabled = false;
            //}
            //UpdateBtn.IsEnabled = false;
        
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SymptomUpdateLV_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            
            //var item = e.DataItem as usersymptom;

            //if(item != null)
            //{
            //    if(item.Enabled)
            //    {
            //        item.Enabled = false;
            //    }
            //    else
            //    {
            //        item.Enabled = true;
            //    }
            //}

            //bool hasEnabledItem = UserSymptomsPassed.Any(item => item.Enabled == true);
            //if (hasEnabledItem)
            //{
            //    UpdateBtn.IsEnabled = true;
            //}
            //else
            //{
            //    UpdateBtn.IsEnabled = false;
            //}

        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }
}