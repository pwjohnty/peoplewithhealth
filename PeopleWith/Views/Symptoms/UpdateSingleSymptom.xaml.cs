//using Java.Time.Temporal;
using Mopups.Services;
using Syncfusion.Maui.DataSource.Extensions;
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
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    string triggorinterstring;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    userfeedback userfeedbacklistpassed = new userfeedback();
    bool edit;
    bool dashupdate;

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


    public UpdateSingleSymptom()
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
    public UpdateSingleSymptom(ObservableCollection<usersymptom> SymptomPassed, String AddEdit, ObservableCollection<usersymptom> AllSymptomData, userfeedback userfeedbacklist , string editpage)
    {
        try
        {

            //edit data page

            InitializeComponent();
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            userfeedbacklistpassed = userfeedbacklist;
            gettriggersandinterventions();
            PassedSymptom = SymptomPassed;
            EditAdd = AddEdit;
            AllSymptomDataPassed = AllSymptomData;
            TItlelbl.Text = PassedSymptom[0].symptomtitle;
            DeleteBtn.IsVisible = true;
            edit = true;

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

                                hoursentry.Text = hours;
                                minsentry.Text = Seconds;
                                   // TimeSpan DurationPick = TimeSpan.Parse(GetDuration);
                                   // DurationPicker.SelectedTime = DurationPick;                              
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

    public UpdateSingleSymptom(ObservableCollection<usersymptom> SymptomPassed, String AddEdit, ObservableCollection<usersymptom> AllSymptomData, userfeedback userfeedbacklist)
    {
        try
        {
            //add data page

            InitializeComponent();
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            userfeedbacklistpassed = userfeedbacklist;
            gettriggersandinterventions();
            PassedSymptom = SymptomPassed;
            EditAdd = AddEdit;
            AllSymptomDataPassed = AllSymptomData;
            TItlelbl.Text = PassedSymptom[0].symptomtitle;

            UpdateBtn.Text = "Add";

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

    public UpdateSingleSymptom(userfeedback userfeedbacklist, string symptomtitle, string symptomscore)
    {
        try
        {
            //add data page

            InitializeComponent();
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            addtimepicker.Time = DateTime.Now.TimeOfDay;
            userfeedbacklistpassed = userfeedbacklist;
            gettriggersandinterventions();
          //  PassedSymptom = SymptomPassed;
            EditAdd = "Add";
          //  AllSymptomDataPassed = AllSymptomData;
            TItlelbl.Text = symptomtitle;
            SymptomSlider.Value = Convert.ToInt32(symptomscore);
            currentint = Convert.ToInt32(symptomscore);

            UpdateBtn.Text = "Add";
            dashupdate = true;

 
            updateResultCount();


            //get the user symptom details
            getUserSymptoms();

            //Set Both Stacks to False on Load 
            TriggersStack.IsVisible = false;
            InterventionsStack.IsVisible = false;

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void getUserSymptoms()
    {
        try
        {
            APICalls aPICalls = new APICalls();

            var getSymptomsTask = aPICalls.GetUserSymptomAsync();

            //var delayTask = Task.Delay(500);

            //if (await Task.WhenAny(getSymptomsTask, delayTask) == delayTask)
            //{
            // await MopupService.Instance.PushAsync(new GettingReady("Loading Symptoms") { });
            //}

            var AllUserSymptoms = await getSymptomsTask;


            foreach (var item in AllUserSymptoms)
            {

                if (item.symptomtitle == TItlelbl.Text)
                {
                    PassedSymptom.Add(item);
                }


            }



        }
        catch (Exception Ex)
        {

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

                                var newcollection = Triggers.OrderBy(p => p.title).ToList();

                                // TriggersLV.SelectedItem = x.triggers;

                                var sl = Triggers.Where(m => m.title == x.triggers).FirstOrDefault();

                                if (sl != null)
                                {
                                    // Remove the item from the collection
                                    newcollection.Remove(sl);

                                    // Insert the item at the first position
                                    newcollection.Insert(0, sl);

                                }

                                //int index = Triggers.IndexOf(sl);
                                TriggersLV.ItemsSource = newcollection;

                                TriggersLV.SelectedItems.Add(sl);

                               // int index = TriggersLV.DataSource.DisplayItems.IndexOf(sl);
                                // Programmatic scrolling based on the item index.
                               // TriggersLV.ItemsLayout.ScrollToRowIndex(index, Microsoft.Maui.Controls.ScrollToPosition.MakeVisible, false);

                                //Add Chips
                                //string[] itemsArray = x.triggers.Split(',');
                                //foreach (string A in itemsArray)
                                //{
                                //    Triggerss.Add(A);
                                //}
                                //foreach (var items in Triggerss)
                                //{
                                //    foreach (var p in Triggers)
                                //    {
                                //        if (p.title == items)
                                //        {
                                //            TriggerChips.Add(p);
                                //            TriggersAdded.Add(p);
                                //        }
                                //    }
                                //}
                                //foreach (var q in TriggerChips)
                                //{
                                //    Triggers.Remove(q);
                                //}
                                //TriggChips.ItemsSource = TriggerChips;
                                //TriggChips.DisplayMemberPath = "title";
                            }
                            if (string.IsNullOrEmpty(x.interventions) || x.interventions == null)
                            {
                                //No Intervention Data 
                            }
                            else
                            {
                                InterventionsStack.IsVisible = true;

                                var newcollection = Interventions.OrderBy(p => p.title).ToList();


                                var sl = Interventions.Where(m => m.title == x.interventions).FirstOrDefault();

                                //  int index = Interventions.IndexOf(sl);

                                if (sl != null)
                                {
                                    // Remove the item from the collection
                                    newcollection.Remove(sl);

                                    // Insert the item at the first position
                                    newcollection.Insert(0, sl);

                                }

                                InterventionLV.ItemsSource = newcollection;


                                InterventionLV.SelectedItems.Add(sl);

                              //  int index = InterventionLV.DataSource.DisplayItems.IndexOf(sl);
                                // Programmatic scrolling based on the item index.
                                //InterventionLV.ItemsLayout.ScrollToRowIndex(index, Microsoft.Maui.Controls.ScrollToPosition.MakeVisible, false);


                                //Add Chips
                                //string[] itemsArray = x.interventions.Split(',');
                                //foreach (string A in itemsArray)
                                //{
                                //    Intervention.Add(A);
                                //}
                                //foreach (var items in Intervention)
                                //{
                                //    foreach (var p in Interventions)
                                //    {
                                //        if (p.title == items)
                                //        {
                                //            InterventionChips.Add(p);
                                //            InterventionsAdded.Add(p);
                                //        }
                                //    }
                                //}
                                //foreach (var q in InterventionChips)
                                //{
                                //    Interventions.Remove(q);
                                //}
                                //IntervChips.ItemsSource = InterventionChips;
                                //IntervChips.DisplayMemberPath = "title";
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

            if(filteredTriggers.Count() == 0)
            {
                trigList.IsVisible = false;
                NoResults.IsVisible = true; 
            }
            else
            {
                trigList.IsVisible = true;
                NoResults.IsVisible = false;
            }
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

            if (filteredInterventions.Count() == 0)
            {
                IntList.IsVisible = false;
                NoResults.IsVisible = true;
            }
            else
            {
                IntList.IsVisible = true;
                NoResults.IsVisible = false;
            }
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

            updateResultCount();
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
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                UpdateBtn.IsEnabled = false;
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
                    items.triggers = triggorinterstring;
                    //Triggerss.Clear();
                    //foreach (var itemx in TriggerChips)
                    //{
                    //    var itemtoadd = itemx.title.ToString();
                    //    Triggerss.Add(itemtoadd);
                    //}
                    //if (Triggerss.Count == 0)
                    //{
                    //    items.triggers = null;
                    //}
                    //else
                    //{
                    //    List<string> NoDuplicates = Triggerss.Distinct().ToList();
                    //    string trig = string.Join(",", NoDuplicates);
                    //    items.triggers = trig;
                    //}
                }
                else if (triggORInter == "Intervention")
                {
                    items.interventions = triggorinterstring;
                    //Intervention.Clear();
                    //foreach (var item in InterventionChips)
                    //{
                    //    var itemtoadd = item.title.ToString();
                    //    Intervention.Add(itemtoadd);
                    //}
                    //if (Intervention.Count == 0)
                    //{
                    //    items.interventions = null;
                    //}
                    //else
                    //{
                    //    List<string> NoDuplicates = Intervention.Distinct().ToList();
                    //    string Inter = string.Join(",", NoDuplicates);
                    //    items.interventions = Inter;
                    //}
                }


                if(string.IsNullOrEmpty(hoursentry.Text) && string.IsNullOrEmpty(minsentry.Text))
                {
                    items.duration = null;
                }
                else
                {
                    var hours = "";
                    var mins = "";

                    if(string.IsNullOrEmpty(hoursentry.Text))
                    {
                        hours = "00";
                    }
                    else
                    {
                        hours = hoursentry.Text;
                    }

                    if (string.IsNullOrEmpty(minsentry.Text))
                    {
                        mins = "00";
                    }
                    else
                    {
                        mins = minsentry.Text;
                    }

                    var timestring = hours + " Hours " + mins + " Minutes";
                    items.duration = timestring;
                }


               // items.duration = Duration;
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
                                i.action = "update";
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

            var newsym = new feedbackdata();
            newsym.value = SliderValue;
            newsym.datetime = items.timestamp;
            newsym.action = "update";
            newsym.label = PassedSymptom[0].symptomtitle;
            newsym.id = items.symptomfeedbackid;

                var remove = userfeedbacklistpassed.symptomfeedbacklist.FirstOrDefault(x => x.id == items.symptomfeedbackid);

                if(remove != null)
                {
                    userfeedbacklistpassed.symptomfeedbacklist.Remove(remove);
                }

            userfeedbacklistpassed.symptomfeedbacklist.Add(newsym);

            string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.symptomfeedbacklist);
            userfeedbacklistpassed.symptomfeedback = newsymJson;


            await database.UserfeedbackUpdateSymptomData(userfeedbacklistpassed);

                if(DeleteBtn.IsVisible)
                {
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Data Updated") { });
                }
                else
                {
                    await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Data Added") { });
                }
                //   await Navigation.PushAsync(new SingleSymptom(PassedSymptom, AllSymptomDataPassed));

                await Task.Delay(1000);
               if(dashupdate)
                {
                    Navigation.RemovePage(this);
                    await MopupService.Instance.PopAllAsync(false);
                    return;

                }
               
            await Navigation.PushAsync(new AllSymptoms(AllSymptomDataPassed, userfeedbacklistpassed));
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
            //foreach (var item in Interventions)
            //{
            //    if (title == item.title)
            //    {
            //        InterventionChips.Add(item);
            //        InterventionsAdded.Add(item);
            //    }
            //}
            //var matchingItem = InterventionChips.FirstOrDefault(item => item.title == title);
            //Interventions.Remove(matchingItem);
            //IntervChips.ItemsSource = InterventionChips;
            //IntervChips.DisplayMemberPath = "title";
            //updateResultCount();
            triggorinterstring = title;
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
            //foreach (var item in Triggers)
            //{
            //    if (title == item.title)
            //    {
            //        TriggerChips.Add(item);
            //        TriggersAdded.Add(item);
            //    }
            //}
            //var matchingItem = TriggerChips.FirstOrDefault(item => item.title == title);
            //Triggers.Remove(matchingItem);
            //TriggChips.ItemsSource = TriggerChips;
            //TriggChips.DisplayMemberPath = "title";
            // updateResultCount();
            triggorinterstring = title;

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

    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            //hours entry text changed
            var entry = sender as Entry;
            if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;

            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }
        }
        catch(Exception ex)
        {

        }
    }

    private void Entry_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        try
        {
            var entry = sender as Entry;
            if (entry == null) return;

            // Get the new text value
            var newText = e.NewTextValue;

            // Ensure the text is numeric and limit to 2 digits
            if (!string.IsNullOrEmpty(newText))
            {
                // Remove non-numeric characters
                newText = new string(newText.Where(char.IsDigit).ToArray());

                // Limit to 2 characters
                if (newText.Length > 2)
                {
                    newText = newText.Substring(0, 2);
                }

                // Validate the value is within the range (0-59)
                if (int.TryParse(newText, out int minutes))
                {
                    if (minutes > 59)
                    {
                        newText = "00"; // Set to max value
                    }
                }
            }

            // Set the corrected text back to the entry
            if (entry.Text != newText)
            {
                entry.Text = newText;
            }
        }
        catch(Exception ex)
        {
         
        }
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        try
        {

            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 15 minutes
            minutes += 15;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch(Exception ex)
        {

        }
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        try
        {

            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 15 minutes
            minutes += 30;

            // Handle overflow into hours
            if (minutes >= 60)
            {
                minutes -= 60;
                hours += 1;
            }

            // Handle hour overflow (optional, if you want to wrap hours to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0;
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void Button_Clicked_3(object sender, EventArgs e)
    {
        try
        {

            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 60 minutes (equivalent to adding 1 hour)
            hours += 1;

            // Handle hour overflow (optional, wrap to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0; // Reset to 0 if over 23 (for 24-hour format)
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private void Button_Clicked_4(object sender, EventArgs e)
    {
        try
        {

            // Parse the current hour and minute values
            int hours = int.TryParse(hoursentry.Text, out int h) ? h : 0;
            int minutes = int.TryParse(minsentry.Text, out int m) ? m : 0;

            // Add 1 hour and 30 minutes
            minutes += 30;
            hours += 1;

            // Handle minute overflow
            if (minutes >= 60)
            {
                minutes -= 60; // Adjust minutes
                hours += 1;    // Increment hours for overflow
            }

            // Handle hour overflow (optional, wrap to a 24-hour format)
            if (hours >= 24)
            {
                hours = 0; // Reset to 0 if over 23 (for 24-hour format)
            }

            // Update the Entries with the new values
            hoursentry.Text = hours.ToString("D2");   // Ensure 2-digit format
            minsentry.Text = minutes.ToString("D2");
        }
        catch (Exception ex)
        {

        }
    }

    private async void DeleteBtn_Clicked(object sender, EventArgs e)
    {
        try
        {

            var userresponse = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this symptom feedback ?", "Cancel", "Yes");

            if (!userresponse)
            {


                foreach (var item in PassedSymptom)
                {
                    foreach (var i in item.feedback)
                    {
                        if (i.symptomfeedbackid == feedbackid)
                        {
                            i.action = "deleted";
                        }
                    }
                }
            


            APICalls database = new APICalls();
            await database.PutSymptomAsync(PassedSymptom);


            var remove = userfeedbacklistpassed.symptomfeedbacklist.FirstOrDefault(x => x.id == EditAdd);

                if (remove != null)
                {
                    remove.action = "deleted";
                }


                string newsymJson = System.Text.Json.JsonSerializer.Serialize(userfeedbacklistpassed.symptomfeedbacklist);
                userfeedbacklistpassed.symptomfeedback = newsymJson;

                
                await database.UserfeedbackUpdateSymptomData(userfeedbacklistpassed);

                //remove it from local collection

                var symptomToRemove = AllSymptomDataPassed.FirstOrDefault(x => x.id == PassedSymptom[0].id);
                if (symptomToRemove != null)
                {
                    AllSymptomDataPassed.Remove(symptomToRemove);
                }

                // Add the updated symptom
                AllSymptomDataPassed.Add(PassedSymptom[0]);


                await MopupService.Instance.PushAsync(new PopupPageHelper("Symptom Feedback Deleted") { });
                
                //   await Navigation.PushAsync(new SingleSymptom(PassedSymptom, AllSymptomDataPassed));

                await Task.Delay(1000);
                await Navigation.PushAsync(new AllSymptoms(AllSymptomDataPassed, userfeedbacklistpassed));
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


        }
        catch(Exception ex)
        {

        }
    }
}