using Mopups.Pages;
using Mopups.Services;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using Maui.FreakyControls.Extensions;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.Picker;

namespace PeopleWith;
public partial class DateTimePopup : PopupPage
{
    List<string> datesList = new List<string>();
    List<string> HoursList = new List<string>();
    List<string> MinutesList = new List<string>();
    private TaskCompletionSource<string> ReturnDate;
    private string SetDateTime;

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

    public DateTimePopup(TaskCompletionSource<string> DateToReturn, string dateTimePass)
    {
        InitializeComponent();
        ReturnDate = DateToReturn;
        SetDateTime = dateTimePass;
        //Load all Data 
        LoadData();
    }

    //Load all Picker Data 
    private async void LoadData()
    {
        try
        {
            //string DatePassed = await ReturnDate.Task;
            DateTime today = DateTime.Today;

            for (int i = -30; i <= 30; i++)
            {
                DateTime targetDate = today.AddDays(i);
                if (targetDate == DateTime.Today)
                {
                    datesList.Add("Today");
                }
                else
                {
                    string dateString = targetDate.ToString("dd/MM/yy");
                    datesList.Add(dateString);
                }
            }

            for (int i = 0; i <= 23; i++)
            {
                HoursList.Add(i.ToString("D2"));
            }

            for (int i = 0; i <= 59; i++)
            {
                MinutesList.Add(i.ToString("D2"));
            }

            var HourMin = DateTime.Now.ToString("HH:mm");
            var GetSplit = HourMin.Split(':');
            var Hora = GetSplit[0];
            var Minuto = GetSplit[1];

            var Split = SetDateTime.Split(" ");
            var setDate = Split[0];
            var SplitTime = Split[1].Split(":");
            var setHour = SplitTime[0];
            var setMin = SplitTime[1];

            PickerColumn Date = new PickerColumn();
            Date.ItemsSource = datesList;
            int Check = datesList.IndexOf(setDate);
            if(Check == -1)
            {
                Date.SelectedIndex = datesList.IndexOf("Today");
            }
            else
            {
                Date.SelectedIndex = datesList.IndexOf(setDate);
            }          
            DatePicker.Columns.Add(Date);

            PickerColumn Hours = new PickerColumn();
            Hours.ItemsSource = HoursList;
            Hours.SelectedIndex = HoursList.IndexOf(setHour);
            HourPicker.Columns.Add(Hours);

            PickerColumn Mins = new PickerColumn();
            Mins.ItemsSource = MinutesList;
            Mins.SelectedIndex = MinutesList.IndexOf(setMin);
            MinutePicker.Columns.Add(Mins);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private string GetSelectedDateTime()
    {
        var selectedDate = DatePicker.Columns[0].SelectedItem.ToString();
        var selectedHour = HourPicker.Columns[0].SelectedItem.ToString();
        var selectedMinute = MinutePicker.Columns[0].SelectedItem.ToString();

        // Combine into a formatted string
        return $"{selectedDate},{selectedHour}:{selectedMinute}";
    }

    private async void Closebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            string DateSelected = GetSelectedDateTime();

            // Set the result
            ReturnDate.TrySetResult(DateSelected);

            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}