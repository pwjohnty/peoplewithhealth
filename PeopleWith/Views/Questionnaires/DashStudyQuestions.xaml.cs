using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Controls.Platform;
using Mopups.Services;
using Microsoft.Maui.ApplicationModel;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Syncfusion.Maui.Graphics.Internals;
using CommunityToolkit.Mvvm.Messaging;

namespace PeopleWith;

public partial class DashStudyQuestions : ContentPage
{
    //Take Questionnaire 
    public ObservableCollection<registryDataInputs> QuestionsPassed = new ObservableCollection<registryDataInputs>();
    public ObservableCollection<registryDataInputs> SelectedDashQuestions = new ObservableCollection<registryDataInputs>();
    public ObservableCollection<registryDataInputs> RetrieveDashQuestions = new ObservableCollection<registryDataInputs>();
    public ObservableCollection<registryDataInputs> SelectedQuestion  = new ObservableCollection<registryDataInputs>();
    public ObservableCollection<registryData> QuestionAnswers = new ObservableCollection<registryData>();
    public ObservableCollection<registryData> AnswersPassed = new ObservableCollection<registryData>();
    public int Order = 1;
    public int MaxOrder;
    bool IsEdit = false;
    bool IsNavigating = false; 
    APICalls database = new APICalls();

    //View Responses
    public ObservableCollection<registryData> UserResponses = new ObservableCollection<registryData>();
   
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

    protected override void OnDisappearing()
    {
#if ANDROID

        base.OnDisappearing();

        DashQuestionsCollection.ItemsSource = null;
        SelectedQuestion.Clear();
        IsNavigating = false;
#endif

    }

    public DashStudyQuestions(ObservableCollection<registryData> ResponsesPassed)
    {
        try
        {
            //View Responses 
            InitializeComponent();
            UserResponses = ResponsesPassed;
            SelectedDashQuestions.Clear();
            SelectedQuestion.Clear();
            QuestionsPassed.Clear();
            ViewResponses.IsVisible = true;
            DataStack.IsVisible = false;


            if (UserResponses.Count > 0)
            {
                previouslist.ItemsSource = UserResponses;
                previouslist.IsVisible = true;
                nodatastack.IsVisible = false;
            }
            else
            {
                previouslist.IsVisible = false;
                nodatastack.IsVisible = true;
            }
        }
        catch(Exception Ex)
        {
            NotasyncMethod(Ex);
        }     
    }

    public DashStudyQuestions(ObservableCollection<registryDataInputs> DashQuestionsPassed, ObservableCollection<registryData> PassedAnwers)
    {
        try
        {
            //Take Questionnaire 
            InitializeComponent();
            SelectedDashQuestions.Clear();
            SelectedQuestion.Clear();
            QuestionsPassed.Clear();
            QuestionsPassed = DashQuestionsPassed;
            ViewResponses.IsVisible = false;
            AnswersPassed = PassedAnwers; 


            foreach (var data in DashQuestionsPassed)
            {
                var New = new registryDataInputs
                {
                    id = data.id,
                    quesorder = data.quesorder,
                    type = data.type, 
                    apporder = data.apporder,
                    values = data.values,
                    dataTab = data.dataTab,
                    dataInputs = data.dataInputs, 
                    label = data.label,
                    labelDirections = data.labelDirections,
                    upperLimit = data.upperLimit,
                    lowerLimit = data.lowerLimit, 
                    itemOrder = data.itemOrder,
                    inputGroup = data.inputGroup,
                    trigger = data.trigger,
                };

                SelectedDashQuestions.Add(New);
            }

            foreach (var items in SelectedDashQuestions)
            {
                //Set all initially to false
                items.Dropdown = false;
                items.Multiple = false;
                items.Weight = false;
                items.WeightYear = false;
                items.Number = false;
                items.MultipleDate = false;
                items.DateDate = false;
                items.Date = false;
                items.WeightEntry = false;
                items.WeightYearEntry = false;
                items.DropDownwOther = false;

                //Add quesorder
                if (!string.IsNullOrEmpty(items.apporder))
                {
                    items.quesorder = Int32.Parse(items.apporder);
                }

                var SetBool = new Dictionary<string, Action>
                {
                    { "dropdown", () => items.Dropdown = true },
                    { "multiple", () => items.Multiple = true },
                    { "weight", () => items.Weight = true },
                    { "weight,year", () => items.WeightYear = true },
                    { "number", () => items.Number = true },
                    { "multiple,date", () => items.MultipleDate = true },
                    { "date,date", () => items.DateDate = true },
                    { "date", () => items.Date = true },

                };

                if (SetBool.TryGetValue(items.type.ToLower(), out var setter))
                {
                    setter();
                }

                //Split Values 
                if (!string.IsNullOrEmpty(items.values))
                {
                    if (items.values.Contains(","))
                    {
                        items.ValueInputs = items.values
                            .Split(',').Select(value => new CheckBoxOption
                            {
                                Text = value.Trim(),
                                questionid = items.id,
                                ItemSelected = false
                            }).ToList();
                    }

                    if (items.type == "dropdown" && items.values.Contains("Other (Specify)"))
                    {
                        items.type = "dropdownwother";
                        items.Dropdown = false;
                        items.DropDownwOther = true;
                    }
                }

                items.TextInput = new TextOption { TextValue = String.Empty, questionid = items.id };
                items.UpdateLabel = String.Empty;

            }
           // SelectedDashQuestions = new ObservableCollection<registryDataInputs>(QuestionsPassed);
            MaxOrder = SelectedDashQuestions.Count();


            foreach (var item in SelectedDashQuestions)
            {

                var CheckOption = new CheckBoxOption();

                if (item.ValueInputs != null)
                {
                    bool Check = item.ValueInputs.Any(x => x.Text == "Other (Specify)");

                    if (Check)
                    {
                        CheckOption = item.ValueInputs.Where(x => x.Text == "Other (Specify)").FirstOrDefault();

                        if (CheckOption != null)
                        {
                            // Remove it from the current position
                            item.ValueInputs.Remove(CheckOption);

                            // Add it to the end
                            item.ValueInputs.Add(CheckOption);
                        }
                    }
                }
            }

            titlelbl.Text = SelectedDashQuestions[0].dataTab;
            sublabel.IsVisible = true;
            RetrieveDashQuestions = SelectedDashQuestions; 
            SetItemSource("+");

        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void SetItemSource(string MassoMenos)
    {
        try
        {
    
                var QuestionSelected = SelectedDashQuestions
          .Where(x => x.quesorder == Order)
          .ToList();
                if (QuestionSelected.Count > 0)
                {
                    SelectedQuestion = new ObservableCollection<registryDataInputs>(QuestionSelected);

                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        DashQuestionsCollection.ItemsSource = null;
                        DashQuestionsCollection.ItemsSource = SelectedQuestion;

                        //Update Nav Btn's
                        Updatebtns();
                    });
                }
                else
                {
                    int Navigation = MassoMenos == "+" ? Order + 1 : MassoMenos == "-" ? Order - 1 : Order;
                    var NextQuestion = SelectedDashQuestions
              .Where(x => x.quesorder == Navigation)
              .ToList();
                SelectedQuestion = new ObservableCollection<registryDataInputs>(NextQuestion);
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    DashQuestionsCollection.ItemsSource = null;
                    DashQuestionsCollection.ItemsSource = SelectedQuestion;

                    Order = Navigation;

                    //Update Nav Btn's
                    Updatebtns();

                });

            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async void Updatebtns()
    {

        try
        {
            backbtn.IsVisible = Order > 1;
            Nextbtn.IsVisible = Order < MaxOrder;
            submitbtn.IsVisible = Order == MaxOrder;
            IsEdit = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void backbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true; 
            //Update Item 
            if (Order > 0)
            {
                if (Nextbtn.Opacity == 0.2)
                {
                    Nextbtn.Opacity = 1;
                }

                Order--;
                SetItemSource("-");
                IsNavigating = false;
            }
            else
            {
                Updatebtns();
                IsNavigating = false;
            }
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private void Nextbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (IsNavigating) return;
            IsNavigating = true;
            //Update Item  
            if (Order < MaxOrder)
            {
                if(Nextbtn.Opacity == 0.2)
                {
                    Vibration.Vibrate();
                    return;
                }

                //Reset Nextbtn.opacity 
                var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == SelectedQuestion[0].id).FirstOrDefault();

                if (CurrentQuestion.type == "number")
                {
                    if (CurrentQuestion.NumberColour != null)
                    {
                        if (CurrentQuestion.NumberColour.ToHex() == "#FF0000")
                        {
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                            IsNavigating = false;
                            return;
                        }
                    }
                }
                else if (CurrentQuestion.type == "date")
                {
                    if (CurrentQuestion.DateColour != null)
                    {
                        if (CurrentQuestion.DateColour.ToHex() == "#FF0000")
                        {
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                            IsNavigating = false;
                            return;
                        }
                    }
                }
                else if (CurrentQuestion.type == "date,date")
                {
                    if (CurrentQuestion.DateStartColour != null && CurrentQuestion.DateEndColour != null)
                    {
                        if (CurrentQuestion.DateStartColour.ToHex() == "#FF0000" || CurrentQuestion.DateEndColour.ToHex() == "#FF0000")
                        {
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                            IsNavigating = false;
                            return;
                        }
                    }
                }
                else if (CurrentQuestion.type == "multiple,date")
                {
                    //CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().SetDateColour = Color.FromArgb("#031926");
                    bool CheckValue = CurrentQuestion.ValueInputs.Any(x => x.SetDateColour == Color.FromArgb("#FF0000"));
                    if(CheckValue)
                    {
                        Nextbtn.Opacity = 0.2;
                        Vibration.Vibrate();
                        IsNavigating = false;
                        return;
                    }
                }


                Order++;
                SetItemSource("+");
                IsNavigating = false;
            }
            else
            {
                Updatebtns();
                IsNavigating = false;
            }
        }
        catch (Exception Ex)
        {
            IsNavigating = false;
            NotasyncMethod(Ex);
        }
    }

    private async void submitbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                loadingstack.IsVisible = true;
                DataStack.IsVisible = false;

                string userid = Preferences.Default.Get("userid", "Unknown");
                var signupcode = Helpers.Settings.SignUp;

                foreach (var item in SelectedDashQuestions)
                {
                    //Create Answer items 
                    var AddItem = new registryData
                    {
                        id = Guid.NewGuid().ToString().ToUpper(),
                        inputid = item.id,
                        userid = userid,
                        advertid = signupcode,
                        condition = "Weight Management",
                        inputConditionCategory = "CORE01",
                        currentDataPoint = true, 

                        //Data For Responses
                        Title = item.dataTab,
                        Question = item.label

                    };
                    QuestionAnswers.Add(AddItem);
                }

                //update QuestionAnswers
                foreach (var items in SelectedDashQuestions)
                {
 
                    //Get Question item
                    var UpdateQuestion = QuestionAnswers.Where(x => x.inputid == items.id).FirstOrDefault();
                    //Check Type 
                    if (UpdateQuestion == null) continue; 

                    //items to Add
                      var Missing = "Unknown";
                      var tilde = "~";

                    if (items.type == "dropdown")
                    {
                        //Wont update if none true
                        if (items.ValueInputs.Any(x => x.ItemSelected))
                        {
                            //Should only be one instance
                            UpdateQuestion.inputValue = items.ValueInputs.First(x => x.ItemSelected).Text;
                        }
                    }
                    else if (items.type == "dropdownwother")
                    {
                        //Wont update if none true
                        if (items.ValueInputs.Any(x => x.ItemSelected))
                        {
                            //Should only be one instance
                            UpdateQuestion.inputValue = items.ValueInputs.First(x => x.ItemSelected).Text;

                            if (UpdateQuestion.inputValue.Contains("Other (Specify)"))
                            {
                                if (!string.IsNullOrEmpty(items.FreeTextEntry))
                                {
                                    UpdateQuestion.notes = items.FreeTextEntry;
                                }
                            }
                        }
                    }
                    else if (items.type == "multiple")
                    {
                        var selectedList = new List<string>();
                        var stringValue = string.Empty;
                        selectedList = items.ValueInputs.Where(x => x.ItemSelected).Select(x => x.Text).ToList();

                        //No items Selected 
                        if (selectedList.Count == 0) continue;

                        //Else update selected Values
                        stringValue = string.Join(",", selectedList);
                        UpdateQuestion.inputValue = stringValue;

                        //Check if specify notes visible 
                        if (stringValue.Contains("Other (Specify)"))
                        {
                            if (!string.IsNullOrEmpty(items.FreeTextEntry))
                            {
                                UpdateQuestion.notes = items.FreeTextEntry;
                            }
                        }
                    }
                    else if (items.type == "number")
                    {
                        if (!string.IsNullOrEmpty(items.NumberEntry))
                        {
                            UpdateQuestion.inputValue = items.NumberEntry;
                        }
                    }
                    else if (items.type == "weight")
                    {
                        //Wont update if none true
                        if (items.ValueInputs.Any(x => x.ItemSelected))
                        {
                            var SelectedItem = items.ValueInputs.First(x => x.ItemSelected).Text;
                            //Should only be one instance
                            if (SelectedItem == "Unknown")
                            {
                                //just upload unknown 
                                UpdateQuestion.inputValue = SelectedItem;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(items.WeightEntryText))
                                {
                                    items.WeightEntryText = Missing;
                                }

                                UpdateQuestion.inputValue = $"{items.WeightEntryText} {SelectedItem}";
                            }
                        }
                    }
                    else if (items.type == "weight,year")
                    {
                        if (items.ValueInputs.Any(x => x.ItemSelected))
                        {
                            var SelectedItem = items.ValueInputs.First(x => x.ItemSelected).Text;
                            //Should only be one instance
                            if (SelectedItem == "Unknown")
                            {
                                //just upload unknown 
                                UpdateQuestion.inputValue = SelectedItem;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(items.WeightYearOne))
                                {
                                    items.WeightYearOne = Missing;
                                }
                                if (string.IsNullOrEmpty(items.WeightYearTwo))
                                {
                                    items.WeightYearTwo = Missing;
                                }

                                UpdateQuestion.inputValue = $"{items.WeightYearOne} {SelectedItem}{tilde}{items.WeightYearTwo}";
                            }
                        }
                    }
                    else if (items.type == "date")
                    {
                        if (!string.IsNullOrEmpty(items.DateEntry))
                        {
                            //Single Date (Should already be validated) Change to Selected Format
                            var ChangeDate = DateTime.Parse(items.NumberEntry).ToString("yyyy-MM-dd");
                            UpdateQuestion.inputValue = ChangeDate;
                        }
                    }
                    else if (items.type == "date,date")
                    {
                        //Check initally Both items contain a value
                        if (!string.IsNullOrEmpty(items.DateDateStart) || !string.IsNullOrEmpty(items.DateDateEnd))
                        {
                            if (string.IsNullOrEmpty(items.DateDateStart))
                            {
                                items.DateDateStart = Missing;
                            }
                            else
                            {
                                items.DateDateStart = DateTime.Parse(items.DateDateStart).ToString("yyyy-MM-dd");
                            }
                            if (string.IsNullOrEmpty(items.DateDateEnd))
                            {
                                items.DateDateStart = Missing;
                            }
                            else
                            {
                                items.DateDateEnd = DateTime.Parse(items.DateDateEnd).ToString("yyyy-MM-dd");
                            }

                              UpdateQuestion.inputValue = $"{items.DateDateStart}*{items.DateDateEnd}";

                        }
                    }
                    else if (items.type == "multiple,date")
                    {
                        //If nothing selected ignore 
                        var selectedList = items.ValueInputs
                            .Where(x => x.ItemSelected)
                            .Select(x =>
                            {
                                string formattedDate;

                                if (string.IsNullOrWhiteSpace(x.DateValue))
                                {
                                    formattedDate = "Unknown";
                                }
                                else if (DateTime.TryParse(x.DateValue, out var parsedDate))
                                {
                                    formattedDate = parsedDate.ToString("yyyy-MM-dd");
                                }
                                else
                                {
                                    formattedDate = "Unknown"; // fallback if parsing fails
                                }

                                return $"{x.Text}~{formattedDate}";
                            })
                            .ToList();

                        // No items selected, skip
                        if (selectedList.Count == 0) continue;

                        // Join final string
                        var stringValue = string.Join(",", selectedList);
                        UpdateQuestion.inputValue = stringValue;

                        if (stringValue.Contains("Other (Specify)"))
                        {
                            if (!string.IsNullOrEmpty(items.FreeTextEntry))
                            {
                                UpdateQuestion.notes = items.FreeTextEntry;
                            }
                        }
                    }
                }

                await database.PostDashQuestionnaire(QuestionAnswers);

                //Update QuestionAnswers
                foreach(var item in QuestionAnswers)
                {
                    //Update Date 
                    item.createdAt = DateTime.Now;
                    item.DateConverted = DateTime.Now.ToString("dd/MM/yy");

                    if (!String.IsNullOrEmpty(item.notes))
                    {
                        item.HasNotes = true; 
                    }
                }

                if(QuestionAnswers.Count > 0)
                {
                    foreach(var item in QuestionAnswers)
                    {
                        AnswersPassed.Add(item);
                    }       
                }

                WeakReferenceMessenger.Default.Send(new UpdateDashAnswers(AnswersPassed));




                //Clear items source 
                //DashQuestionsCollection.ItemsSource = null;
                //SelectedQuestion.Clear();
                //IsNavigating = false;

                loadingstack.IsVisible = false;
                await MopupService.Instance.PushAsync(new PopupPageHelper("Information Uploaded") { });
                await Task.Delay(1500);
                await MopupService.Instance.PopAllAsync(false);
                Navigation.RemovePage(this);
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

    private void MultipleSelection_CheckedChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {

            var selectedCheckbox = (ExtendedCheckbox)sender;
            var selectedValue = selectedCheckbox.questionid;
            var selectedText = selectedCheckbox.Text.ToString();

            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();
            //var GetAnswer = QuestionAnswers.Where(x => x.inputid == selectedValue).FirstOrDefault();

            var SelectedList = new List<string>();
            var Setstring = string.Empty;

            //if (GetAnswer.inputValue != null)
            //{
            //    var CheckString = GetAnswer.inputValue;

            //    if (CheckString.Contains(","))
            //    {
            //        SelectedList = CheckString.Split(',').ToList();

            //        if (SelectedList.Contains(selectedText))
            //        {
            //            SelectedList.Remove(selectedText);
            //        }
            //        else
            //        {
            //            SelectedList.Add(selectedText);
            //        }

            //        // Not Needed
            //        //Setstring = SelectedList.Count > 0 ? string.Join(",", SelectedList) : null;
            //    }
            //    else
            //    {
            //        if (CheckString == selectedText)
            //        {
            //            //Setstring = null;
            //        }
            //        else
            //        {
            //            SelectedList.Add(CheckString);
            //            SelectedList.Add(selectedText);
            //            //Setstring = string.Join(",", SelectedList);
            //        }
            //    }
            //}
            //else
            //{
            //    Setstring = selectedText;

            //}

            //update Answer
            //UpdateAnswer(selectedValue, "InputValue", Setstring);

            //Check If Selected Item OtherSpecify is check/Unchecked 
            if (CurrentQuestion != null)
            {
                if (string.IsNullOrEmpty(selectedText)) return; 

                if(selectedText == "Other (Specify)")
                {
                    //Set Visibilty based on Items Selection true/true  false/false
                    CurrentQuestion.SpecifyText = CurrentQuestion.ValueInputs.Where(x => x.Text == selectedText).FirstOrDefault().ItemSelected; 
                }
            }                 
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void SpecifyText_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            var TextValue = (ExtendedEditor)sender;
#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();
            //var GetAnswer = QuestionAnswers.Where(x => x.inputid == selectedValue).FirstOrDefault();

            if (string.IsNullOrEmpty(selectedValue))  return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.FreeTextEntry = String.Empty;
                //UpdateAnswer(selectedValue, "Notes", null);
            }
            else
            {

                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(TextValue.Text.ToString()))
                {
                    var selectedText = TextValue.Text.ToString();
                    CurrentQuestion.FreeTextEntry = selectedText;
                    //UpdateAnswer(selectedValue, "Notes", selectedText);
                }
            }         
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private void DashQuestionsCollection_SizeChanged(object sender, EventArgs e)
    {
        try
        {
            var viewCell = sender as View;
            if (viewCell != null)
            {
                double itemHeight = viewCell.Height;
                //Comment out for now 
                //DashQuestionsCollection.HeightRequest = itemHeight + 20;
            }
        }
        catch (Exception Ex)
        {

        }
    }

    private void WeightSelection_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (IsEdit) return;
            IsEdit = true;
            var selectedCheckbox = (ExtendedRadioButton)sender;
            var selectedValue = selectedCheckbox.questionid;
            string selectedText = selectedCheckbox?.Content?.ToString();

            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (CurrentQuestion.type != "weight")
            {
                IsEdit = false;
                return;
            }

            //Remove All items
            foreach (var items in CurrentQuestion.ValueInputs)
            {
                if (!string.IsNullOrEmpty(selectedText))
                {
                    if (items.Text != selectedText)
                    {
                        if (items.ItemSelected == true)
                        {
                            items.ItemSelected = false;
                        }
                    }
                }        
            }

            if (!string.IsNullOrEmpty(selectedText))
            {
                if (selectedText != "Unknown")
                {
                    //Update Overall Questions
                    if (CurrentQuestion.type == "weight")
                    {
                        CurrentQuestion.WeightEntry = true;
                        CurrentQuestion.UpdateLabel = selectedText;

                    }
                    //Don't update until Text added

                }
                else
                {
                    //Unknown Selected
                    CurrentQuestion.WeightEntry = false;

                    if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                    {
                        //UpdateAnswer(selectedValue, "InputValue", selectedText);
                    }
                }
            }
            
            IsEdit = false;
        }
        catch (Exception Ex)
        {
            IsEdit = false;
            NotasyncMethod(Ex);
        }
    }

    private void EntryWeight_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var Selectedweight = TextValue.IDValue;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (string.IsNullOrEmpty(Selectedweight)) return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.WeightEntryText = String.Empty; 
                //UpdateAnswer(selectedValue, "InputValue", null);
            }
            else
            {
                var selectedText = TextValue.Text.ToString() + " " + TextValue.IDValue.ToString();

                if (!String.IsNullOrEmpty(TextValue.Text.ToString()))
                {
                    CurrentQuestion.WeightEntryText = TextValue.Text.ToString();
                }
                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    //UpdateAnswer(selectedValue, "InputValue", selectedText);
                }
            }            
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private void WeightYear_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (IsEdit) return;
            IsEdit = true;
            var selectedCheckbox = (ExtendedRadioButton)sender;
            var selectedValue = selectedCheckbox.questionid;
            string selectedText = selectedCheckbox?.Content?.ToString();

            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (CurrentQuestion.type != "weight,year")
            {
                IsEdit = false;
                return; 
            }

            //Remove All items
            foreach (var items in CurrentQuestion.ValueInputs)
            {
                if (!string.IsNullOrEmpty(selectedText))
                {
                    if (items.Text != selectedText)
                    {
                        if (items.ItemSelected == true)
                        {
                            items.ItemSelected = false;
                        }
                    }
                }         
            }

            if (!string.IsNullOrEmpty(selectedText))
            {
                if (selectedText != "Unknown")
                {
                    //Update Overall Questions
                    if (CurrentQuestion.type == "weight,year")
                    {
                        CurrentQuestion.WeightYearEntry = true;
                        CurrentQuestion.UpdateLabel = selectedText;
                    }
                    //Don't update until Text added
                }
                else
                {
                    //Unknown Selected
                    CurrentQuestion.WeightYearEntry = false;

                    if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                    {
                        //UpdateAnswer(selectedValue, "InputValue", selectedText);

                    }
                }
            }
            
            IsEdit = false;
        }
        catch (Exception Ex)
        {
            IsEdit = false;
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedEntryYear_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.WeightYearTwo = String.Empty;
                //UpdateAnswer(selectedValue, "DateTime", null);
            }
            else
            {             
                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(TextValue.Text.ToString()))
                {
                    var selectedText = TextValue.Text.ToString();
                    CurrentQuestion.WeightYearTwo = selectedText;
                    //UpdateAnswer(selectedValue, "DateTime", selectedText);
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void ExtendedEntryWeight_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif
            var selectedValue = TextValue.questionid;
            var Selectedweight = TextValue.IDValue;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (string.IsNullOrEmpty(Selectedweight)) return;
         
            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.WeightYearOne = String.Empty; 
                //UpdateAnswer(selectedValue, "InputValue" , null);
            }
            else
            {
                var selectedText = TextValue.Text.ToString() + " " + TextValue.IDValue;

                if (!String.IsNullOrEmpty(TextValue.Text.ToString()))
                {
                    CurrentQuestion.WeightYearOne = TextValue.Text.ToString();
                }
                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    //UpdateAnswer(selectedValue, "InputValue", selectedText);
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void date_TextChanged(object sender, Syncfusion.Maui.Inputs.MaskedEntryValueChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedMaskedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (CurrentQuestion.type != "date")
            {
                return;
            }

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.DateEntry = String.Empty;
                //UpdateAnswer(selectedValue, "InputValue", null);
            }
            else
            {
                var selectedText = TextValue.Text.ToString();

                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    if (selectedText.Contains("_"))
                    {
                        CurrentQuestion.DateColour = Color.FromArgb("#031926"); 
                        Nextbtn.Opacity = 1;
                    }
                    else
                    {
                        //Complete Date. Check Valid 

                        if (DateTime.TryParseExact(selectedText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        {
                            // Check if the year is reasonable and the date isn't in the future
                            if (parsedDate.Year >= 1900 && parsedDate <= DateTime.Now)
                            {
                                //Single Date 
                                CurrentQuestion.DateEntry = selectedText;
                                //UpdateAnswer(selectedValue, "InputValue", parsedDate.ToString("yyyy-MM-dd"));
                                CurrentQuestion.DateColour = Color.FromArgb("#031926"); // Valid
                                Nextbtn.Opacity = 1;
                            }
                            else
                            {
                                CurrentQuestion.DateColour = Color.FromArgb("#FF0000");  // Invalid 
                                Nextbtn.Opacity = 0.2;
                                Vibration.Vibrate();
                            }
                        }
                        else
                        {
                            CurrentQuestion.DateColour = Color.FromArgb("#FF0000");  // Invalid 
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private void number_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.NumberEntry = String.Empty; 
                //UpdateAnswer(selectedValue, "InputValue", null);
            }
            else
            {
                var selectedText = TextValue.Text.ToString();
                
                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    int Numvalue = Int32.Parse(selectedText); 
                    int UpperLimit = Int32.Parse(CurrentQuestion.upperLimit);
                    int LowerLimit = Int32.Parse(CurrentQuestion.lowerLimit);
                    //Check Upper/lowerlimit 
                    if(Numvalue <= UpperLimit && Numvalue >= LowerLimit)
                    {
                        //Success
                        CurrentQuestion.NumberEntry = selectedText;
                        CurrentQuestion.NumberColour = Color.FromArgb("#031926");
                        //UpdateAnswer(selectedValue, "InputValue", selectedText);
                        Nextbtn.Opacity = 1;
                    }
                    else
                    {
                        //failure
                        CurrentQuestion.NumberColour = Color.FromArgb("#FF0000");
                        Nextbtn.Opacity = 0.2; 
                    }
                    
                  
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }

    private void SingleSelection_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (IsEdit) return;
            IsEdit = true;
            var selectedCheckbox = (ExtendedRadioButton)sender;
            var selectedValue = selectedCheckbox.questionid;
            string selectedText = selectedCheckbox?.Content?.ToString();

            if (string.IsNullOrEmpty(selectedText))
            {
                IsEdit = false;
                return;
            }

            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (CurrentQuestion.type != "dropdown")
            {
                IsEdit = false;
                return;
            }

            //Remove All items
            foreach (var items in CurrentQuestion.ValueInputs)
            {
                if (!String.IsNullOrEmpty(selectedText))
                {
                    if (items.Text != selectedText)
                    {
                        items.ItemSelected = false;
                    }
                }
            }
            //Check item exisits 
            var NextOrder = Order + 1;
            bool Exisits = SelectedDashQuestions.Any(x => x.quesorder == NextOrder);

            if (Exisits)
            {
                var NextQuestion = SelectedDashQuestions.Where(x => x.quesorder == NextOrder).FirstOrDefault();

                if (NextQuestion != null && selectedText != "Yes")
                {
                    MainThread.BeginInvokeOnMainThread(() => SelectedDashQuestions.Remove(NextQuestion));
                }
            }
            else
            {
                var NextQuestion = RetrieveDashQuestions.Where(x => x.quesorder == NextOrder).FirstOrDefault();
                if (NextQuestion != null && selectedText == "Yes")
                {
                    //Add it back
                    if (NextQuestion.type == "dropdownwother")
                    {
                        NextQuestion.Dropdown = false;
                        NextQuestion.DropDownwOther = true;
                    }

                    SelectedDashQuestions.Add(NextQuestion);
                    MainThread.BeginInvokeOnMainThread(() => SelectedDashQuestions.Add(NextQuestion));
                }
            }

            //update buttons if last item in collection 
            int maxOrderInCollection = SelectedDashQuestions.Max(x => x.quesorder);

            if (Order == maxOrderInCollection)
            {
                Nextbtn.IsVisible = false;
                submitbtn.IsVisible = true;
            }
            else
            {
                Nextbtn.IsVisible = true;
                submitbtn.IsVisible = false;
            }

            //check orinigal item not updating 

            var SelecteOriginal = QuestionsPassed.Where(x=> x.id == CurrentQuestion.id).FirstOrDefault();

            //// bool Exisits = SelectedDashQuestions.Where(x => x.quesorder == NextOrder).FirstOrDefault();
            ////Check not null, then procced 
            //if (!string.IsNullOrEmpty(CurrentQuestion.trigger))
            //{
            //    if (NextQuestion != null)
            //    {
            //        //Add item back in if Removed 
            //        if (selectedText == "Yes")
            //        {

            //        }
            //        else
            //        {
            //            //Remove items from list 
            //            //Trigger && Inputgroup 
            //            //Ignore if null
            //        }

            //        string[] parts = NextQuestionDI.Split('|');
            //        var CheckTrigger = parts[1];
            //    }
            //}
            //Get Selected item 

            IsEdit = false;
        }
        catch (Exception Ex)
        {
            IsEdit = false;
            NotasyncMethod(Ex);
        }
    }

    private void dropdownWother_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (IsEdit) return;
            IsEdit = true;
            var selectedCheckbox = (ExtendedRadioButton)sender;
            var selectedValue = selectedCheckbox.questionid;
            string selectedText = selectedCheckbox?.Content?.ToString();

            if (string.IsNullOrEmpty(selectedText))
            {
                IsEdit = false;
                return;
            }

            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (CurrentQuestion.type != "dropdownwother")
            {
                IsEdit = false;
                return;
            }

            //Remove All items
            foreach (var items in CurrentQuestion.ValueInputs)
            {
                if (!String.IsNullOrEmpty(selectedText))
                {
                    if (items.Text != selectedText)
                    {
                        items.ItemSelected = false;
                    }
                }
            }


            if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
            {
                //UpdateAnswer(selectedValue, "InputValue", selectedText);
            }

            //Show Freetext 
            bool SetFreeText = selectedText?.Contains("Other (Specify)") == true;
            if (CurrentQuestion != null)
            {
                CurrentQuestion.SpecifyText = SetFreeText;
            }

            IsEdit = false;
        }
        catch (Exception Ex)
        {
            IsEdit = false;
            NotasyncMethod(Ex);
        }
    }

    private void datedatestart_ValueChanged(object sender, Syncfusion.Maui.Inputs.MaskedEntryValueChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedMaskedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (CurrentQuestion.type != "date,date")
            {
                IsEdit = false;
                return;
            }

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.DateDateStart = String.Empty;
                CurrentQuestion.DateColour = Color.FromArgb("#031926");
                //UpdateAnswer(selectedValue, "InputValue", null);
            }
            else
            {
                var selectedText = TextValue.Text.ToString();

                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    if (selectedText.Contains("_"))
                    {
                        CurrentQuestion.DateStartColour = Color.FromArgb("#031926");
                        Nextbtn.Opacity = 1;
                    }
                    else
                    {
                        //Complete Date. Check Valid 

                        if (DateTime.TryParseExact(selectedText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        {
                            // Check if the year is reasonable and the date isn't in the future
                            if (parsedDate.Year >= 1900 && parsedDate <= DateTime.Now)
                            {
                                //Single Date 
                                CurrentQuestion.DateDateStart = selectedText;
                                //UpdateAnswer(selectedValue, "InputValue", parsedDate.ToString("yyyy-MM-dd"));
                                CurrentQuestion.DateStartColour = Color.FromArgb("#031926"); // Valid
                                Nextbtn.Opacity = 1;
                            }
                            else
                            {
                                CurrentQuestion.DateStartColour = Color.FromArgb("#FF0000");  // Invalid 
                                Nextbtn.Opacity = 0.2;
                                Vibration.Vibrate();
                            }
                        }
                        else
                        {
                            CurrentQuestion.DateStartColour = Color.FromArgb("#FF0000");  // Invalid 
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private void datedateend_ValueChanged(object sender, Syncfusion.Maui.Inputs.MaskedEntryValueChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedMaskedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();

            if (CurrentQuestion.type != "date,date")
            {
                return;
            }

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                //Clear value
                CurrentQuestion.DateDateEnd = String.Empty;
                CurrentQuestion.DateEndColour = Color.FromArgb("#031926");
                //UpdateAnswer(selectedValue, "InputValue", null);
            }
            else
            {
                var selectedText = TextValue.Text.ToString();

                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    if (selectedText.Contains("_"))
                    {
                        CurrentQuestion.DateEndColour = Color.FromArgb("#031926");
                        Nextbtn.Opacity = 1;
                    }
                    else
                    {
                        //Complete Date. Check Valid 

                        if (DateTime.TryParseExact(selectedText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        {
                            // Check if the year is reasonable and the date isn't in the future
                            if (parsedDate.Year >= 1900 && parsedDate <= DateTime.Now)
                            {
                                //Single Date 
                                CurrentQuestion.DateDateEnd = selectedText;
                                //UpdateAnswer(selectedValue, "StringJoin", parsedDate.ToString("yyyy-MM-dd"));
                                CurrentQuestion.DateEndColour = Color.FromArgb("#031926"); // Valid
                                Nextbtn.Opacity = 1;
                            }
                            else
                            {
                                CurrentQuestion.DateEndColour = Color.FromArgb("#FF0000");  // Invalid 
                                Nextbtn.Opacity = 0.2;
                                Vibration.Vibrate();
                            }
                        }
                        else
                        {
                            CurrentQuestion.DateEndColour = Color.FromArgb("#FF0000");  // Invalid 
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            //NotasyncMethod(Ex);
        }
    }

    private void multipledate_StateChanged(object sender, Syncfusion.Maui.Buttons.StateChangedEventArgs e)
    {
        try
        {
            var selectedCheckbox = (ExtendedCheckbox)sender;
            var selectedValue = selectedCheckbox.questionid;
            var CheckBoxText = selectedCheckbox.IDValue;
            var selectedText = selectedCheckbox.Text.ToString();

            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();
            //var GetAnswer = QuestionAnswers.Where(x => x.inputid == selectedValue).FirstOrDefault();

            var SelectedList = new List<string>();
            var Setstring = string.Empty;

            //if (GetAnswer.inputValue != null)
            //{
            //    var CheckString = GetAnswer.inputValue;

            //    if (CheckString.Contains(","))
            //    {
            //        SelectedList = CheckString.Split(',').ToList();

            //        if (SelectedList.Contains(selectedText))
            //        {
            //            SelectedList.Remove(selectedText);
            //        }
            //        else
            //        {
            //            SelectedList.Add(selectedText);
            //        }

            //        Setstring = SelectedList.Count > 0 ? string.Join(",", SelectedList) : null;
            //    }
            //    else
            //    {
            //        if (CheckString == selectedText)
            //        {
            //            Setstring = null;
            //        }
            //        else
            //        {
            //            SelectedList.Add(CheckString);
            //            SelectedList.Add(selectedText);
            //            Setstring = string.Join(",", SelectedList);
            //        }
            //    }
            //}
            //else
            //{
            //    Setstring = selectedText;

            //}

            //update Answer
            //UpdateAnswer(selectedValue, "InputValue", Setstring);

            //Show Freetext 

            if (CurrentQuestion != null)
            {
                if (string.IsNullOrEmpty(selectedText)) return;

                if (selectedText == "Other (Specify)")
                {
                    //Set Visibilty based on Items Selection true/true  false/false
                    CurrentQuestion.SpecifyText = CurrentQuestion.ValueInputs.Where(x => x.Text == selectedText).FirstOrDefault().ItemSelected;
                }
            }

            //bool SetFreeText = Setstring?.Contains("Other (Specify)") == true;
            //if (CurrentQuestion != null)
            //{
            //    CurrentQuestion.SpecifyText = SetFreeText;
            //}
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }

    }

    private void multipledate_ValueChanged(object sender, Syncfusion.Maui.Inputs.MaskedEntryValueChangedEventArgs e)
    {
        try
        {
            var TextValue = (ExtendedMaskedEntry)sender;

#if ANDROID
                var handler = TextValue.Handler as Microsoft.Maui.Handlers.EntryHandler;
                var editText = handler?.PlatformView as AndroidX.AppCompat.Widget.AppCompatEditText;
                if (editText != null)
                {
                    editText.EmojiCompatEnabled = false;
                    editText.SetTextKeepState(TextValue.Text);
                }
#endif

            var selectedValue = TextValue.questionid;
            var CheckBoxText = TextValue.IDValue;
            var CurrentQuestion = SelectedDashQuestions.Where(x => x.id == selectedValue).FirstOrDefault();


            if (CurrentQuestion.type != "multiple,date")
            {
                return;
            }

            if (string.IsNullOrEmpty(selectedValue)) return;

            if (String.IsNullOrEmpty(TextValue.Text.ToString()))
            {
                CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().DateValue = String.Empty;
                CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().SetDateColour = Color.FromArgb("#031926");
                Nextbtn.Opacity = 1;
            }
            else
            {
                var selectedText = TextValue.Text.ToString();

                if (!string.IsNullOrEmpty(selectedValue) && !string.IsNullOrEmpty(selectedText))
                {
                    if (selectedText.Contains("_"))
                    {
                        CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().SetDateColour = Color.FromArgb("#031926");
                        Nextbtn.Opacity = 1;

                    }
                    else
                    {
                        //Complete Date. Check Valid 

                        if (DateTime.TryParseExact(selectedText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
                        {
                            // Check if the year is reasonable and the date isn't in the future
                            if (parsedDate.Year >= 1900 && parsedDate <= DateTime.Now)
                            {
                                //Single Date 
                                CurrentQuestion.DateEntry = selectedText;
                                CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().DateValue = selectedText;
                                CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().SetDateColour = Color.FromArgb("#031926");
                                Nextbtn.Opacity = 1;
                            }
                            else
                            {
                                //CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().DateValue = selectedText;
                                CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().SetDateColour = Color.FromArgb("#FF0000"); ;
                                Nextbtn.Opacity = 0.2;
                                Vibration.Vibrate();
                            }
                        }
                        else
                        {
                            //CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().DateValue = selectedText;
                            CurrentQuestion.ValueInputs.Where(x => x.Text == CheckBoxText).FirstOrDefault().SetDateColour = Color.FromArgb("#FF0000"); ;
                            Nextbtn.Opacity = 0.2;
                            Vibration.Vibrate();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
}