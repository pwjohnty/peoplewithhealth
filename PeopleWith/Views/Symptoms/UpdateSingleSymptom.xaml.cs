using Mopups.Services;
using Syncfusion.Maui.Picker;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
namespace PeopleWith;
public partial class UpdateSingleSymptom : ContentPage
{
    public ObservableCollection<interventiontrigger> Interventions = new ObservableCollection<interventiontrigger>();
    public ObservableCollection<interventiontrigger> InterventionsAdded = new ObservableCollection<interventiontrigger>();
    public ObservableCollection<interventiontrigger> InterventionChips = new ObservableCollection<interventiontrigger>();
    public ObservableCollection<interventiontrigger> Triggers = new ObservableCollection<interventiontrigger>();
    public ObservableCollection<interventiontrigger> TriggersAdded = new ObservableCollection<interventiontrigger>();
    public ObservableCollection<interventiontrigger> TriggerChips = new ObservableCollection<interventiontrigger>();
    public ObservableCollection<usersymptom> PassedSymptom = new ObservableCollection<usersymptom>();
    public ObservableCollection<symptomfeedback> EditFeedback = new ObservableCollection<symptomfeedback>();
    public ObservableCollection<usersymptom> AllSymptomDataPassed = new ObservableCollection<usersymptom>();
    string SelectedDate;
    string SelectedTime;
    string SliderValue;
    string NotesEntry;
    string Duration;
    public List<string> Intervention = new List<string>();
    public List<string> Triggerss = new List<string>();
    int currentint = 0;
    string triggORInter;
    string EditAdd;
    string feedbackid;
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            NotasyncMethod(Ex);
            await crashHandler.CrashDetectedSend(Ex);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
    }

    public UpdateSingleSymptom()
    {
        InitializeComponent();
    }
    public UpdateSingleSymptom(ObservableCollection<usersymptom> SymptomPassed, String AddEdit, ObservableCollection<usersymptom> AllSymptomData)
    {
        try
        {
            InitializeComponent();
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            gettriggersandinterventions();
            PassedSymptom = SymptomPassed;
            EditAdd = AddEdit;
            AllSymptomDataPassed = AllSymptomData;
            TItlelbl.Text = PassedSymptom[0].symptomtitle;
            foreach (var item in SymptomPassed)
            {
                // SymptomTitle.Text = item.symptomtitle;
            }
            if (EditAdd == "Add")
            {
                foreach (var item in SymptomPassed)
                {

                    currentint = Int32.Parse(item.CurrentIntensity);
                    SymptomSlider.Value = Int32.Parse(item.CurrentIntensity);
                }
                Datelbl.Text = DateTime.Now.ToString("dd MMM");
                Timelbl.Text = DateTime.Now.ToString("HH:mm");
                DurationPicker.SelectedTime = new TimeSpan(0, 0, 0);
            }
            else
            {
                foreach (var item in PassedSymptom)
                {
                    foreach (var x in item.feedback)
                    {
                        if (x.symptomfeedbackid == EditAdd)
                        {
                            feedbackid = x.symptomfeedbackid;
                            currentint = Int32.Parse(x.intensity);
                            SymptomSlider.Value = Int32.Parse(x.intensity);
                            DateTime DateAndTime = DateTime.Parse(x.timestamp);
                            addtimepicker.Time = DateAndTime.TimeOfDay;
                            adddatepicker.Date = DateAndTime.Date; 

                            SelectedDate = DateAndTime.ToString("dd/MM/yyyy");
                            SelectedTime = DateAndTime.ToString("HH:mm:ss");
                          
                            if (string.IsNullOrEmpty(x.duration) || x.duration == "No Duration")
                            {
                                DurationPicker.SelectedTime = new TimeSpan(0, 0, 0);
                            }
                            else
                            {
                                    var Duration = x.duration.Split(' ');
                                    var hours = Duration[0];
                                    var Seconds = Duration[2];
                                    string GetDuration = hours + ":" + Seconds + ":00";
                                    TimeSpan DurationPick = TimeSpan.Parse(GetDuration);
                                    DurationPicker.SelectedTime = DurationPick;                              
                            }
                            if (string.IsNullOrEmpty(x.notes) || x.notes == null)
                            {
                                //No Notes
                            }
                            else
                            {
                                Notes.Text = x.notes;
                            }
                        }
                    }
                }
            }
            updateResultCount();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void updateResultCount()
    {
        try
        {
            TriggersLV.ItemsSource = Triggers.OrderBy(m => m.title);
            InterventionLV.ItemsSource = Interventions.OrderBy(p => p.title);
            ResultsTrig.Text = "Results" + " (" + Triggers.Count() + ")";
            ResultsInt.Text = "Results" + " (" + Interventions.Count() + ")";
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }


    public async void gettriggersandinterventions()
    {
        try
        {
            APICalls database = new APICalls();
            ObservableCollection<interventiontrigger> Triggersintervention = await database.GetAsyncInterventionTrigger();
            foreach (var item in Triggersintervention)
            {
                if (item.type == "trigger")
                {
                    Triggers.Add(item);
                }
                else if (item.type == "intervention")
                {
                    Interventions.Add(item);
                }
            }

            if (EditAdd != "Add")
            {
                foreach (var item in PassedSymptom)
                {
                    foreach (var x in item.feedback)
                    {
                        if (x.symptomfeedbackid == EditAdd)
                        {
                            if (string.IsNullOrEmpty(x.triggers) || x.triggers == null)
                            {
                                //No Triggers Data 
                            }
                            else
                            {

                                TriggersStack.IsVisible = true;
                                //Add Chips
                                string[] itemsArray = x.triggers.Split(',');
                                foreach (string A in itemsArray)
                                {
                                    Triggerss.Add(A);
                                }
                                foreach (var items in Triggerss)
                                {
                                    foreach (var p in Triggers)
                                    {
                                        if (p.title == items)
                                        {
                                            TriggerChips.Add(p);
                                            TriggersAdded.Add(p);
                                        }
                                    }
                                }
                                foreach (var q in TriggerChips)
                                {
                                    Triggers.Remove(q);
                                }
                                TriggChips.ItemsSource = TriggerChips;
                                TriggChips.DisplayMemberPath = "title";
                            }
                            if (string.IsNullOrEmpty(x.interventions) || x.interventions == null)
                            {
                                //No Intervention Data 
                            }
                            else
                            {
                                InterventionsStack.IsVisible = true;
                                //Add Chips
                                string[] itemsArray = x.interventions.Split(',');
                                foreach (string A in itemsArray)
                                {
                                    Intervention.Add(A);
                                }
                                foreach (var items in Intervention)
                                {
                                    foreach (var p in Interventions)
                                    {
                                        if (p.title == items)
                                        {
                                            InterventionChips.Add(p);
                                            InterventionsAdded.Add(p);
                                        }
                                    }
                                }
                                foreach (var q in InterventionChips)
                                {
                                    Interventions.Remove(q);
                                }
                                IntervChips.ItemsSource = InterventionChips;
                                IntervChips.DisplayMemberPath = "title";
                            }
                        }
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TriggersSB_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = TriggersSB.Text.ToString();
            var filteredTriggers = new ObservableCollection<interventiontrigger>(Triggers.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
            TriggersLV.ItemsSource = filteredTriggers;
            var count = filteredTriggers.Count().ToString();
            ResultsTrig.Text = "Results" + " (" + count + ")";
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void InterventionSB_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var Characters = InterventionSB.Text.ToString();
            var filteredInterventions = new ObservableCollection<interventiontrigger>(Interventions.Where(s => s.title.Contains(Characters, StringComparison.OrdinalIgnoreCase))).OrderBy(m => m.title);
            InterventionLV.ItemsSource = filteredInterventions;
            var count = filteredInterventions.Count().ToString();
            ResultsInt.Text = "Results" + " (" + count + ")";
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }
    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Schedulepopup.IsOpen = true;
            MainStack.Opacity = 0.2;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
    {
        try
        {
            Timepopup.IsOpen = true;
            MainStack.Opacity = 0.2;
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
            DateTime datetime = DateTime.Parse(Datelbl.Text);
            DateTime SelectedDate = datetime + e.NewValue;
            if (SelectedDate <= DateTime.Now)
            {
                var Time = e.NewValue.ToString();
                var split = Time.Split(':');
                Timelbl.Text = split[0] + ":" + split[1];
                var getTime = split[0] + ":" + split[1] + ":" + "00";
                SelectedTime = getTime;
            }
            else
            {
                int secondsToVibrate = Random.Shared.Next(1, 1);
                TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);
                Vibration.Default.Vibrate(vibrationLength);
                return;
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void DatePicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.DatePickerSelectionChangedEventArgs e)
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
    private void SfSlider_ValueChanged(object sender, Syncfusion.Maui.Sliders.SliderValueChangedEventArgs e)
    {
        try
        {
            var value = Math.Round(e.NewValue);
            if (value < currentint)
            {
                InterventionsStack.IsVisible = true;
                TriggersStack.IsVisible = false;
            }
            else if (value > currentint)
            {
                InterventionsStack.IsVisible = false;
                TriggersStack.IsVisible = true;
            }
            else if (value == currentint)
            {
                InterventionsStack.IsVisible = false;
                TriggersStack.IsVisible = false;
            }
            else if (value == 0)
            {
                InterventionsStack.IsVisible = false;
                TriggersStack.IsVisible = false;
            }
            SliderValue = value.ToString();
            scorelbl.Text = value.ToString();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            SelectedDate = adddatepicker.Date.ToString("dd/MM/yyyy");
            SelectedTime = addtimepicker.Time.ToString();
            if (TriggersStack.IsVisible == true)
            {
                triggORInter = "Trigger";
            }
            else if (InterventionsStack.IsVisible == true)
            {
                triggORInter = "Intervention";
            }
            if (String.IsNullOrEmpty(Notes.Text) || Notes.Text == null)
            {
                NotesEntry = null;
            }
            else
            {
                NotesEntry = Notes.Text.ToString();
            }

            //Update Feedback
            var items = new symptomfeedback();
            if (string.IsNullOrEmpty(SelectedDate) || SelectedDate == null)
            {
                var Date = DateTime.Now;
                SelectedDate = Date.ToString("dd/MM/yyyy");
            }
            if (string.IsNullOrEmpty(SelectedTime) || SelectedTime == null)
            {
                var time = DateTime.Now;
                SelectedTime = time.ToString("HH:mm:ss");
            }
            items.timestamp = SelectedDate + " " + SelectedTime;
            Guid newUUID = Guid.NewGuid();
            if (EditAdd == "Add")
            {
                items.symptomfeedbackid = newUUID.ToString().ToUpper();
            }
            else
            {
                items.symptomfeedbackid = EditAdd;
            }
               
            items.intensity = SliderValue;
            items.notes = NotesEntry;
            if (triggORInter == "Trigger")
            {
                Triggerss.Clear(); 
                foreach (var itemx in TriggerChips)
                {
                    var itemtoadd = itemx.title.ToString();
                    Triggerss.Add(itemtoadd);
                }
                if (Triggerss.Count == 0)
                {
                    items.triggers = null;
                }
                else
                {
                    List<string> NoDuplicates = Triggerss.Distinct().ToList();
                    string trig = string.Join(",", NoDuplicates);
                    items.triggers = trig;
                }
            }
            else if (triggORInter == "Intervention")
            {
                Intervention.Clear();
                foreach (var item in InterventionChips)
                {
                    var itemtoadd = item.title.ToString();
                    Intervention.Add(itemtoadd);
                }
                if (Intervention.Count == 0)
                {
                    items.interventions = null;
                }
                else
                {
                    List<string> NoDuplicates = Intervention.Distinct().ToList();
                    string Inter = string.Join(",", NoDuplicates);
                    items.interventions = Inter;
                }
            }



            items.duration = Duration;
            if (EditAdd == "Add")
            {
                foreach (var item in PassedSymptom)
                {

                    item.feedback.Add(items);
                }
            }
            else
            {
                foreach (var item in PassedSymptom)
                {
                    foreach (var i in item.feedback)
                    {
                        if (i.symptomfeedbackid == feedbackid)
                        {
                            i.intensity = items.intensity;
                            i.notes = items.notes;
                            i.timestamp = items.timestamp;
                            i.interventions = items.interventions;
                            i.triggers = items.triggers;
                            i.duration = items.duration;
                        }
                    }
                }
            }

            APICalls database = new APICalls();
            await database.PutSymptomAsync(PassedSymptom);
            //   await Navigation.PushAsync(new SingleSymptom(PassedSymptom, AllSymptomDataPassed));
            await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Data Added") { });
            await Task.Delay(1500);
            await Navigation.PushAsync(new AllSymptoms(AllSymptomDataPassed));
            await MopupService.Instance.PopAllAsync(false);
            var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleSymptom);
            var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllSymptoms);
         
            if (pageToRemoves != null)
            {
                Navigation.RemovePage(pageToRemoves);
            }
            if (pageToRemove != null)
            {
                Navigation.RemovePage(pageToRemove);
            }

            if (EditAdd != "Add")
            {
                var RemovedPage = Navigation.NavigationStack.FirstOrDefault(x => x is ShowAllSymptomData);
                if (RemovedPage != null)
                {
                    Navigation.RemovePage(RemovedPage);
                } 
            }
            Navigation.RemovePage(this);

        }
        catch (Exception Ex)
        {
            await crashHandler.CrashDetectedSend(Ex); 
        }
    }
    private void DurationPicker_SelectionChanged(object sender, Syncfusion.Maui.Picker.TimePickerSelectionChangedEventArgs e)
    {
        try
        {
            var Time = e.NewValue.ToString();
            var split = Time.Split(':');
            var getTime = split[0] + " Hours " + split[1] + " Minutes";
            Duration = getTime;
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }
    private void InterventionLV_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var title = (e.DataItem as interventiontrigger).title;
            foreach (var item in Interventions)
            {
                if (title == item.title)
                {
                    InterventionChips.Add(item);
                    InterventionsAdded.Add(item);
                }
            }
            var matchingItem = InterventionChips.FirstOrDefault(item => item.title == title);
            Interventions.Remove(matchingItem);
            IntervChips.ItemsSource = InterventionChips;
            IntervChips.DisplayMemberPath = "title";
            updateResultCount();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex); 
        }

    }
    private void IntervChips_ItemRemoved(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var title = (e.RemovedItem as interventiontrigger).title;
            foreach (var item in InterventionsAdded)
            {
                if (title == item.title)
                {
                    Interventions.Add(item);
                }
            }
            var matchingItem = Interventions.FirstOrDefault(item => item.title == title);
            InterventionsAdded.Remove(matchingItem);
            InterventionChips.Remove(matchingItem);
            IntervChips.ItemsSource = InterventionChips;
            IntervChips.DisplayMemberPath = "title";
            updateResultCount();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }
    private void TriggChips_ItemRemoved(object sender, Syncfusion.Maui.Core.Chips.SelectionChangedEventArgs e)
    {
        try
        {
            var title = (e.RemovedItem as interventiontrigger).title;
            foreach (var item in TriggersAdded)
            {
                if (title == item.title)
                {
                    Triggers.Add(item);
                }
            }
            var matchingItem = Triggers.FirstOrDefault(item => item.title == title);
            TriggersAdded.Remove(matchingItem);
            TriggerChips.Remove(matchingItem);
            TriggChips.ItemsSource = TriggerChips;
            TriggChips.DisplayMemberPath = "title";
            updateResultCount();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex); 
        }
    }
    private void TriggersLV_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var title = (e.DataItem as interventiontrigger).title;
            foreach (var item in Triggers)
            {
                if (title == item.title)
                {
                    TriggerChips.Add(item);
                    TriggersAdded.Add(item);
                }
            }
            var matchingItem = TriggerChips.FirstOrDefault(item => item.title == title);
            Triggers.Remove(matchingItem);
            TriggChips.ItemsSource = TriggerChips;
            TriggChips.DisplayMemberPath = "title";
            updateResultCount();
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
    private void Schedulepopup_Closed(object sender, EventArgs e)
    {
        try
        {
            MainStack.Opacity = 1;
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
            MainStack.Opacity = 1;
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
            await Navigation.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}