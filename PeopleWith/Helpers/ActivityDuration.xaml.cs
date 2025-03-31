using Mopups.Pages;
using Mopups.Services;
using Syncfusion.Maui.Calendar;
using System.Globalization;
using System.Text.RegularExpressions;
using Syncfusion.Maui.Picker;

namespace PeopleWith;

public partial class ActivityDuration : PopupPage
{
    List<string> datesList = new List<string>();
    List<string> HoursList = new List<string>();
    List<string> MinutesList = new List<string>();
    private TaskCompletionSource<string> ReturnDuration;
    private string SetDuration;


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

    public ActivityDuration(TaskCompletionSource<string> DurationtoReturn, string Duration)
    {
        InitializeComponent();
        ReturnDuration = DurationtoReturn;
        SetDuration = Duration;
        //Load all Data 
        LoadData();
    }

    //Load all Picker Data 
    private async void LoadData()
    {
        try
        {

            for (int i = 0; i <= 23; i++)
            {
                HoursList.Add(i.ToString("D2"));
            }

            for (int i = 0; i <= 59; i++)
            {
                MinutesList.Add(i.ToString("D2"));
            }

            var Split = SetDuration.Split(" ");
            var setHour = Split[0];
            var setMin = Split[1];

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

    private string GetSelectedDuration()
    {
        var selectedHour = HourPicker.Columns[0].SelectedItem.ToString();
        var selectedMinute = MinutePicker.Columns[0].SelectedItem.ToString();

        // Combine into a formatted string
        return $"{selectedHour}h {selectedMinute}m";
    }

    private async void Closebtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            string SelectedDuration = GetSelectedDuration();

            // Set the result
            ReturnDuration.TrySetResult(SelectedDuration);

            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Reset_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Reset Selected index 
            HourPicker.Columns[0].SelectedIndex = HoursList.IndexOf("00");
            MinutePicker.Columns[0].SelectedIndex = MinutesList.IndexOf("00");
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void IncrementPickcer_Clicked(object sender, EventArgs e)
    {
        try
        {
            var selectedHour = HourPicker.Columns[0].SelectedItem.ToString();
            var selectedMinute = MinutePicker.Columns[0].SelectedItem.ToString();

            int hours = int.TryParse(selectedHour, out int h) ? h : 00;
            int minutes = int.TryParse(selectedMinute, out int m) ? m : 00;

            var GetCommand = (sender) as Button;
            var StringTime = GetCommand.CommandParameter.ToString();
            int GetMins = Int32.Parse(StringTime);
            minutes += GetMins;

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
            var UpdatedHours = hours.ToString("D2");  
            var UpdatedMins = minutes.ToString("D2");

            HourPicker.Columns[0].SelectedIndex = HoursList.IndexOf(UpdatedHours);
            MinutePicker.Columns[0].SelectedIndex = MinutesList.IndexOf(UpdatedMins);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}