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
    public UpdateAllSymptoms(ObservableCollection<usersymptom> AllUserSymptoms)
    {
        InitializeComponent();
        UserSymptomsPassed = AllUserSymptoms;

        foreach(var item in UserSymptomsPassed)
        {
            item.Enabled = false;
            item.Opacity = 0.5;
            item.Slidervalue = Convert.ToDouble(item.CurrentIntensity);
            item.SlidervalueUA = Convert.ToDouble(item.CurrentIntensity);
            item.CurrentIntensityUA = item.CurrentIntensity;
            var dt = DateTime.Parse(item.feedback[item.feedback.Count - 1].timestamp).ToString("dd MMM, HH:mm");
            item.DateUpdatedAll = "Last updated: " + dt;
        }
      //  Schedulepopup.BindingContext = new DatePickerViewModel();
        Datelbl.Text = DateTime.Now.ToString("dd MMM");
        Timelbl.Text = DateTime.Now.ToString("HH:mm");
        addtimepicker.Time = DateTime.Now.TimeOfDay;
       // UpdateBtn.IsEnabled = false;
        //foreach (var item in UserSymptomsPassed)
        //{
        //   var NewUpdate = new symptomupdate();
        //    NewUpdate.Id = item.id;
        //    NewUpdate.Opacity = 0.5;
        //    NewUpdate.Enabled = false;
        //    NewUpdate.LastUpdate = item.LastUpdated;
        //    NewUpdate.SliderValue = Int32.Parse(item.CurrentIntensity);
        //    NewUpdate.SymptomName = item.symptomtitle;
        //    SymptomUpdatelist.Add(NewUpdate);
        //}

        if (DeviceInfo.Platform == DevicePlatform.iOS)
        {
            SymptomUpdateLV.HeightRequest = UserSymptomsPassed.Count * 100;
        }
        var sortedSymptoms = UserSymptomsPassed.OrderByDescending(f => DateTime.Parse(f.LastUpdated)).ToList();
        //SymptomUpdatelist.Clear();
        //foreach (var symptom in sortedSymptoms)
        //{
        //    SymptomUpdatelist.Add(symptom);
        //}
        SymptomUpdateLV.ItemsSource = UserSymptomsPassed;
    }
    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        try
        {
            var sfslide = sender as Syncfusion.Maui.Sliders.SfSlider;
            var slidvalue = Math.Round(sfslide.Value);
            var selectedItem = sfslide.BindingContext as usersymptom;
            selectedItem.CurrentIntensityUA = slidvalue.ToString();

        }
        catch(Exception ex)
        {

        }
    }
    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
    async private void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
          
                    foreach (var symptom in UserSymptomsPassed)
                    {

                        if (symptom.Enabled == true)
                        {
                            
                            
                                var items = new symptomfeedback();
                    SelectedDate = adddatepicker.Date.ToString("dd/MM/yyyy");
                    SelectedTime = addtimepicker.Time.ToString(@"hh\:mm");
                    //if (string.IsNullOrEmpty(SelectedDate) || SelectedDate == null)
                    //            {
                    //                var Date = DateTime.Now;
                    //                SelectedDate = Date.ToString("dd /MM/yyyy");
                    //            }
                    //            else
                    //            {
                    //               // var Date = DateTime.Parse(adddatepicker.Date);
                    //                SelectedDate = adddatepicker.Date.ToString("dd/MM/yyyy");
                    //            }
                    //if (string.IsNullOrEmpty(SelectedTime) || SelectedTime == null)
                    //            {
                    //                var time = DateTime.Now;
                    //                SelectedTime = time.ToString("HH:mm");
                    //            }
                    //            else
                    //            {
                    //               // var time = DateTime.Parse(addtimepicker.Time);
                    //                SelectedTime = addtimepicker.Time.ToString("HH:mm");
                    //            }
            
                                items.timestamp = SelectedDate + " " + SelectedTime;
                                Guid newUUID = Guid.NewGuid();
                                items.symptomfeedbackid = newUUID.ToString().ToUpper();
                                items.intensity = symptom.Slidervalue.ToString();
                                items.notes = null;
                                items.triggers = null;
                                items.interventions = null;
                                items.duration = null;
                                symptom.feedback.Add(items);
                                SymptomUpdateNewlist.Add(symptom);
                            
                        }

                    
                
            }
            //Need Loading Screen
            APICalls database = new APICalls();
            await database.UpdateAll(SymptomUpdateNewlist);

            await MopupService.Instance.PushAsync(new PopupPageHelper("Symptoms Updated") { });
            await Task.Delay(1500);

            await Navigation.PushAsync(new AllSymptoms(UserSymptomsPassed));

            await MopupService.Instance.PopAllAsync(false);

            var pageToRemoveAllSymptoms = Navigation.NavigationStack.FirstOrDefault(x => x is AllSymptoms);
            if (pageToRemoveAllSymptoms != null)
            {
                Navigation.RemovePage(pageToRemoveAllSymptoms);
            }
            Navigation.RemovePage(this);
        }
        catch(Exception ex)
        {

        }
    }
    private void SelectDatePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.DatePickerSelectionChangedEventArgs e)
    {
        var Date = e.NewValue;
        Datelbl.Text = Date.ToString("dd MMM");
        var split = Date.ToString().Split(' ');
        SelectedDate = split[0];
    }
    private void TimePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.TimePickerSelectionChangedEventArgs e)
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
    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        Schedulepopup.IsOpen = true;
        Mainstack.Opacity = 0.2;
    }
    private void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
    {
        Timepopup.IsOpen = true;
        Mainstack.Opacity = 0.2;
    }
    private void TimePicker_Closed(object sender, EventArgs e)
    {
        Mainstack.Opacity = 1;
    }
    private void Schedulepopup_Closed(object sender, EventArgs e)
    {
        Mainstack.Opacity = 1;
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
        catch(Exception ex)
        {

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
            SymptomUpdateLV.SelectAll();
            foreach(var item in UserSymptomsPassed)
            {
                item.Enabled = true;
            }
            UpdateBtn.IsEnabled = true;
        }
        catch(Exception ex)
        {

        }
    }

    private void SymptomUpdateLV_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var item = e.DataItem as usersymptom;

            if(item != null)
            {
                if(item.Enabled)
                {
                    item.Enabled = false;
                }
                else
                {
                    item.Enabled = true;
                }
            }

        }
        catch(Exception ex)
        {

        }
    }
}