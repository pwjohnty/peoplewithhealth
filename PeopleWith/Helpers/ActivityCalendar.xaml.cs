using Mopups.Pages;
using Mopups.Services;
using Syncfusion.Maui.Calendar;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PeopleWith;

public partial class ActivityCalendar : PopupPage
{
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();
    private bool isediting = false;
    private TaskCompletionSource<string> ReturnDate;

    public ActivityCalendar(TaskCompletionSource<string> DateToReturn)
    {
        InitializeComponent();
        ReturnDate = DateToReturn;
        //Set items 
        var Date = DateTime.Now.ToString("dd MMM yyyy");
        Datelbl.Text = Date;
        DateEntry.Text = DateTime.Now.ToString("dd/MM/yyyy");
        Calendar.MaximumDate = DateTime.Now.AddMonths(1);

        CalendarTextStyle textStyle = new CalendarTextStyle
        {
            TextColor = Color.FromArgb("#991B1B")
        };

        // Assign to Calendar after initialization
        Calendar.MonthView.TodayTextStyle = textStyle;
        Calendar.YearView.TodayTextStyle = textStyle;

    }
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

    private async void Cancelbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Close Clicked
            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void Okbtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Ensure the label exists and has a value
            string selectedDate = Datelbl.Text ?? string.Empty;

            // Set the result
            ReturnDate.TrySetResult(selectedDate);

            // Close Popup
            await MopupService.Instance.PopAsync();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            if(CalendarView.IsVisible == true)
            {
                EntryView.IsVisible = true;
                CalendarView.IsVisible = false;

                Dateimg.Source = "selectcalendar.png";
            }
            else
            {
                EntryView.IsVisible = false;
                CalendarView.IsVisible = true;

                Dateimg.Source = "compose.png";
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private async void DateEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (isediting)
                return;

            isediting = true;

            string input = e.NewTextValue;

            // Remove any non-numeric characters except '/'
            input = new string(input.Where(c => char.IsDigit(c) || c == '/').ToArray());

            // Remove existing slashes to reformat correctly
            input = input.Replace("/", string.Empty);

            // Limit the input to a maximum of 8 numeric characters (DDMMYYYY)
            if (input.Length > 8)
                input = input.Substring(0, 8);

            // Insert slashes at the appropriate positions
            if (input.Length > 2)
                input = input.Insert(2, "/");

            if (input.Length > 5)
                input = input.Insert(5, "/");

            // Validate the entered date when fully typed (10 characters)
            if (input.Length == 10 && DateTime.TryParseExact(input, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime date))
            {
                DateTime today = DateTime.Today;
                DateTime maxValidDate = today.AddMonths(1);
                DateTime minValidDate = today.AddYears(-50);

                if (date < minValidDate)
                {
                    // Invalid Date
                    DateInput.Stroke = Colors.Red;
                    DateInput.HasError = true;
                    DateInput.ErrorText = "Invalid date. Cannot go back more than 50 years";
                    Datelbl.Text = "Entered Date";

                    // Reset error state after 5 seconds
                    await Task.Delay(5000);
                    DateInput.HasError = false;
                    DateInput.Stroke = Color.FromArgb("#fce9d9");
                }
                else if (date > maxValidDate)
                {
                    // Invalid Date
                    DateInput.Stroke = Colors.Red;
                    DateInput.HasError = true;
                    DateInput.ErrorText = "Invalid date. Cannot go forward more than a month";
                    Datelbl.Text = "Entered Date";

                    // Reset error state after 5 seconds
                    await Task.Delay(5000);
                    DateInput.HasError = false;
                    DateInput.Stroke = Color.FromArgb("#fce9d9");
                }
                else
                {
                    // Valid Date
                    Datelbl.Text = date.ToString("dd MMM yyyy");
                    DateInput.HasError = false;
                    DateInput.Stroke = Color.FromArgb("#fce9d9");
                }
            }
            else if (input.Length == 10)
            {
                // Invalid format
                Datelbl.Text = "Enter Date";
                DateInput.Stroke = Colors.Red;
                DateInput.HasError = true;
                DateInput.ErrorText = "Invalid date format. Use DD/MM/YYYY.";

                await Task.Delay(5000);
                DateInput.HasError = false;
                DateInput.Stroke = Color.FromArgb("#fce9d9");
            }

            // Assign formatted text back without triggering loop
            if (DateEntry.Text != input)
            {
                DateEntry.Text = input;
            }

            // Adjust cursor position to prevent jumpiness
            DateEntry.CursorPosition = input.Length;

            isediting = false;
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Calendar_SelectionChanged(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    {
        try
        {
            if (e.NewValue != null)
            {
                DateTime selectedDate = (DateTime)e.NewValue;
                Datelbl.Text = selectedDate.ToString("dd MMM yyyy"); 
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    private void Calendar_ViewChanged(object sender, CalendarViewChangedEventArgs e)
    {
        try
        {
            if (e.NewView == Syncfusion.Maui.Calendar.CalendarView.Century)
            {
                Calendar.View = Syncfusion.Maui.Calendar.CalendarView.Decade; 
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}
