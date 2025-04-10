using Maui.FreakyControls.Extensions;
using Mopups.Services;
using Syncfusion.Maui.Core.Carousel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Networking;
namespace PeopleWith;
public partial class ShowAllSymptomData : ContentPage
{
    ObservableCollection<usersymptom> SymptomPassed = new ObservableCollection<usersymptom>();
    ObservableCollection<usersymptom> AllSymptomsData = new ObservableCollection<usersymptom>();
    ObservableCollection<symptomfeedback> SymptomFeedback = new ObservableCollection<symptomfeedback>();
    ObservableCollection<symptomfeedback> Feedbacksymptom = new ObservableCollection<symptomfeedback>();
    ObservableCollection<symptomfeedback> itemstoremove = new ObservableCollection<symptomfeedback>();
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

    protected override void OnDisappearing()
    {
        try
        {
            //Clear the Toolbar item on Back Pressed and reset to Original
            //deletelbl.IsVisible = false;
            //// Set DeleteCheck and DeleteSelected to false for all items in SymptomFeedback
            //foreach (var item in SymptomFeedback)
            //{
            //    item.DeleteCheck = false;
            //    item.DeleteSelected = false;
            //}
            //AllDataLV.ItemsSource = SymptomFeedback;
            //AllDataLV.HeightRequest = SymptomFeedback.Count * 160;

            //this.ToolbarItems.Clear();

            //ToolbarItem itemm = new ToolbarItem
            //{
            //    Text = "Edit"

            //};

            //itemm.Clicked += EditBtn_Clicked;

            //// "this" refers to a Page object
            //this.ToolbarItems.Add(itemm);

            //EditBtn.IsEnabled = true;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    public ShowAllSymptomData()
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
    public ShowAllSymptomData(ObservableCollection<usersymptom> PassedSymptom, ObservableCollection<symptomfeedback> PassedSymptomFeedback, ObservableCollection<usersymptom> AllSymptoms, userfeedback userfeedbacklist)
    {
        try
        {
            InitializeComponent();
            SymptomPassed = PassedSymptom;
            Feedbacksymptom = PassedSymptomFeedback;
            SymptomFeedback = Feedbacksymptom;
            AllSymptomsData = AllSymptoms;
            ShowAllTitle.Text = SymptomPassed[0].Shorttitle;
            userfeedbacklistpassed = userfeedbacklist;
            ShowAllloading.IsVisible = true; 
            foreach (var item in SymptomFeedback)
            {
                item.OtherBool = false;
                item.DeleteCheck = false;
                item.ImageAttached = false;
                item.DeleteSelected = false;
                var time = DateTime.Parse(item.timestamp);
                var format = time.ToString("HH:mm, dd/MM/yy");
                item.formattedDateTime = format;
                if (!string.IsNullOrEmpty(item.triggers))
                {
                    item.triggerorIntervention = "Trigger";
                    item.TriggerBool = true;
                }
                else if (!string.IsNullOrEmpty(item.interventions))
                {
                    item.triggerorIntervention = "Intervention";
                    item.InterventionBool = true;
                }
                else
                {
                    item.triggerorIntervention = "Trigger/Intervention";
                    item.OtherBool = true;
                }
                if (item.duration == "00 Hours 00 Minutes" || item.duration == "No Duration" || item.duration == null || item.duration == "--" || string.IsNullOrEmpty(item.duration))
                {
                    item.formattedduration = "No Duration";
                }
                else
                {
                    var Replaceitem = item.duration.Replace(" Hours", "").Replace(" Minutes", "");
                    var SplitReplace = Replaceitem.Split(" ");
                    var NewDuration = $"{SplitReplace[0]}h{SplitReplace[1]}m";
                    item.formattedduration = NewDuration;
                }
                if (item.notes == "--" || item.notes == null || string.IsNullOrEmpty(item.notes))
                {
                    item.notes = "No Notes";
                }

                if (!string.IsNullOrEmpty(item.symptomimage))
                {
                    item.ImageAttached = true;
                    var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/symptomimages/{item.symptomimage}?t={DateTime.UtcNow.Ticks}";
                    item.symptomimageurl = imagestring;
                }
            }

            var orderlist = SymptomFeedback.OrderByDescending(x => DateTime.Parse(x.timestamp)).ToList();

            //Show Hide Based 
            bool CheckForTrue = orderlist.Any(x => x.ImageAttached == true);
            ShowImageHeader.IsVisible = CheckForTrue;
            GalleryBtn.IsVisible = CheckForTrue;

           foreach(var item in orderlist)
            {
                if (CheckForTrue)
                {
                    item.ImageShowAll = true;
                    item.NormalShowAll = false;
                }
                else
                {
                    item.ImageShowAll = false;
                    item.NormalShowAll = true;
                }
            }

            AllDataLV.ItemsSource = orderlist;
            Task.Delay(2000);
            ShowAllloading.IsVisible = false;
            //AllDataLV.HeightRequest = SymptomFeedback.Count * 120; 
        }
        catch (Exception Ex)
        {
            ShowAllloading.IsVisible = false;
            NotasyncMethod(Ex);
        }
    }
    async private void EditBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Edit Clicked 
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                EditBtn.IsEnabled = false;

               // deletelbl.IsVisible = true;
                foreach (var item in SymptomFeedback)
                {
                    if (item.symptomfeedbackid == SymptomFeedback[0].symptomfeedbackid)
                    {
                        item.DeleteCheck = false;
                        item.DeleteSelected = false;
                    }
                    else
                    {
                        //Set All others to true 
                        item.DeleteCheck = true;
                        item.DeleteSelected = false;
                    }
                }
                AllDataLV.ItemsSource = SymptomFeedback;
                AllDataLV.HeightRequest = SymptomFeedback.Count * 160;

                this.ToolbarItems.Clear();

                ToolbarItem itemm = new ToolbarItem
                {
                    Text = "Delete"

                };

                itemm.Clicked += OnItemClicked;

                // "this" refers to a Page object
                this.ToolbarItems.Add(itemm);

                EditBtn.IsEnabled = true;


              

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

    private async void OnItemClicked(object sender, EventArgs e)
    {
        //Delete Clicked 
        try
        {
            //Old Code 
            //Bool To Check if Any items have Delete Selected
            bool hasDateSelected = SymptomFeedback.Any(item => item.DeleteSelected);
            if (!hasDateSelected)
            {
                //Bool to Show Check Box visible/Not Visible
                bool DeleteVisible = SymptomFeedback.Any(item => item.DeleteCheck);

                if (DeleteVisible)
                {
                    //Nothing Selected Return to orignial View 
                   // deletelbl.IsVisible = false;
                    // Set DeleteCheck and DeleteSelected to false for all items in SymptomFeedback
                    foreach (var item in SymptomFeedback)
                    {
                        item.DeleteCheck = false;
                        item.DeleteSelected = false;
                    }
                    AllDataLV.ItemsSource = SymptomFeedback;
                    AllDataLV.HeightRequest = SymptomFeedback.Count * 160;

                    this.ToolbarItems.Clear();

                    ToolbarItem itemm = new ToolbarItem
                    {
                        Text = "Edit"

                    };

                    itemm.Clicked += EditBtn_Clicked;

                    // "this" refers to a Page object
                    this.ToolbarItems.Add(itemm);

                    EditBtn.IsEnabled = true;
                    return;
                }
            }

            else
            {
                bool DeleteMsg = await DisplayAlert("Delete Selected Records", "Are you Sure you Would like to Delete the following Records?, Once Deleted they cannot be retrieved", "Delete", "Cancel");
                if (DeleteMsg)
                {
                    //Accept
                    foreach (var item in SymptomPassed)
                    {
                        foreach (var x in item.feedback)
                        {
                            for (int i = 0; i < SymptomFeedback.Count; i++)
                            {
                                if (SymptomFeedback[i].symptomfeedbackid == x.symptomfeedbackid)
                                {
                                    if (SymptomFeedback[i].DeleteSelected == true)
                                    {
                                        itemstoremove.Add(x);
                                    }
                                }
                            }
                        }
                        foreach (var p in itemstoremove)
                        {
                            item.feedback.Remove(p);
                        }
                    }
                    foreach (var item in AllSymptomsData)
                    {
                        if (item.id == SymptomPassed[0].id)
                        {
                            var feedbackToRemove = new List<symptomfeedback>();
                            foreach (var x in item.feedback)
                            {
                                if (itemstoremove.Contains(x))
                                {
                                    feedbackToRemove.Add(x);
                                }
                            }
                            foreach (var feedback in feedbackToRemove)
                            {
                                item.feedback.Remove(feedback);
                            }
                        }
                    }
                    //API CALL
                    APICalls database = new APICalls();
                    var userid = Helpers.Settings.UserKey;
                    await database.PutSymptomAsync(SymptomPassed);

                    this.ToolbarItems.Clear();

                    ToolbarItem itemm = new ToolbarItem
                    {
                        Text = "Edit"

                    };

                    itemm.Clicked += EditBtn_Clicked;

                    // "this" refers to a Page object
                    this.ToolbarItems.Add(itemm);

                    EditBtn.IsEnabled = true;
                    //Navigate Back to AllSymptoms
                    await Navigation.PushAsync(new AllSymptoms(AllSymptomsData, userfeedbacklistpassed));
                    var pageToRemove = Navigation.NavigationStack.FirstOrDefault(x => x is AllSymptoms);
                    var pageToRemoves = Navigation.NavigationStack.FirstOrDefault(x => x is SingleSymptom);

                    if (pageToRemove != null)
                    {
                        Navigation.RemovePage(pageToRemove);
                    }
                    if (pageToRemoves != null)
                    {
                        Navigation.RemovePage(pageToRemoves);
                    }
                    Navigation.RemovePage(this);
                }
                else
                {
                    //Decline 
                    EditBtn.IsEnabled = true;
                    return;
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
    async private void AllDataLV_ItemTapped(object sender, Syncfusion.Maui.ListView.ItemTappedEventArgs e)
    {
        try
        {
            var Symptom = e.DataItem as symptomfeedback;
            bool DeleteVisible = SymptomFeedback.Any(f => f.DeleteCheck);
            if (DeleteVisible == true)
            {
                foreach (var item in SymptomFeedback)
                {
                    if (SymptomFeedback[0].symptomfeedbackid == Symptom.symptomfeedbackid)
                    {
                        await DisplayAlert("Inital Feedback", "This Feedback cannot be Edited or Deleted", "Close");
                        return;
                    }
                    if (item.symptomfeedbackid == Symptom.symptomfeedbackid)
                    {
                        if (item.DeleteSelected == true)
                        {
                            item.DeleteSelected = false;
                        }
                        else
                        {
                            item.DeleteSelected = true;
                        }

                    }
                }
            }
            else
            {
                if (SymptomFeedback[0].symptomfeedbackid == Symptom.symptomfeedbackid)
                {
                    await DisplayAlert("Inital Feedback", "This Feedback cannot be Edited or Deleted", "Close");
                    return;
                }
                else
                {
                    await Navigation.PushAsync(new UpdateSingleSymptom(SymptomPassed, Symptom.symptomfeedbackid, AllSymptomsData, userfeedbacklistpassed, "editpage"));
                    return;
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void AllDataLV_ItemLongPress(object sender, Syncfusion.Maui.ListView.ItemLongPressEventArgs e)
    {
        try
        {
            var Symptom = e.DataItem as symptomfeedback;
            if(Symptom != null)
            {
                if (!String.IsNullOrEmpty(Symptom.symptomimage))
                {
                    var imagestring = $"https://peoplewithappiamges.blob.core.windows.net/symptomimages/{Symptom.symptomimage}?t={DateTime.UtcNow.Ticks}";
                    string rotate = string.Empty;
                    await MopupService.Instance.PushAsync(new imagePopUp(imagestring, rotate) { });

                }
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
           if(SymptomFeedback != null )
            {
                var orderlist = SymptomFeedback.OrderByDescending(x => DateTime.Parse(x.timestamp)).ToObservable();
                await Navigation.PushAsync(new SymptomGallery(orderlist, SymptomPassed[0].Shorttitle), false);
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}