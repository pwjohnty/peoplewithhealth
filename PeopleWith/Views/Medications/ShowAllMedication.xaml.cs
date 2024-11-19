using CommunityToolkit.Maui.Core.Extensions;
using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class ShowAllMedication : ContentPage
{
	usermedication MedSelected = new usermedication();
	ObservableCollection<usermedication> MedicationList = new ObservableCollection<usermedication>();
    ObservableCollection<usermedication> TakenMedicationList = new ObservableCollection<usermedication>();
    ObservableCollection<usermedication> NotTakenMedicationList = new ObservableCollection<usermedication>();
    ObservableCollection<usermedication> NotRecordedMedicationList = new ObservableCollection<usermedication>();
    ObservableCollection<usermedication> MedicationNotRecordedList = new ObservableCollection<usermedication>();
    Color SetColour;
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


    public ShowAllMedication(usermedication SelectedMed)
	{
		try
		{
			InitializeComponent();
			MedSelected = SelectedMed;
			MedicationName.Text = MedSelected.medicationtitle;

            PopulateListView(); 
			
		}
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void PopulateListView()
	{
		try
		{
			if (MedSelected.frequency.Contains("|"))
			{
				var Freq = MedSelected.frequency.Split('|');

				//As Required Medication 
				if (Freq[0] == "As Required")
				{
					//Only Populate With Feedback items 
					if(MedSelected.feedback == null)
					{
						nodatastack.IsVisible = true;
						datastack.IsVisible = false; 

                    }
					else
					{
                        nodatastack.IsVisible = false;
                        datastack.IsVisible = true;
                        NotTakenFrame.Background = Colors.White;
                        NotTakenFrame.BorderColor = Colors.White;
                        NotTakenFrame.Opacity = 1;
                        NotTakenFrame.IsEnabled = false;
                        NotRecordedFrame.Background = Colors.White;
                        NotRecordedFrame.BorderColor = Colors.White;
                        NotRecordedFrame.Opacity = 1;
                        NotRecordedFrame.IsEnabled = false;


                        foreach (var item in MedSelected.feedback)
						{

							var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");

							var NewMed = new usermedication();
							NewMed.MedDateTime = RecordTime;
						    NewMed.Colour = Colors.LightGreen;
                            NewMed.Action = "Taken";
							NewMed.id = item.id;
                            NewMed.Dosage = item.Recorded; 
						    MedicationList.Add(NewMed);

                        }

						foreach(var item in MedicationList)
						{								
						    item.unit = MedSelected.unit; 
						}


				    var sortedlist = MedicationList.OrderBy(t => t.MedDateTime);
					UserMedicationSchedule.ItemsSource = sortedlist;
                        //Filtered List 
                        TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();

                    }
				}
				else if(Freq[0] == "Daily")
			    {
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;

                    DateTime? enddate = null; 
                    if (!string.IsNullOrEmpty(MedSelected.enddate))
                    {
                        enddate = DateTime.Parse(MedSelected.enddate);
                    }

                    if (MedSelected.feedback == null)
                    {

                    }
                    else
                    {

                        foreach (var item in MedSelected.feedback)
                        {

                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            var CurrentTime = DateTime.Parse(item.datetime);
                            if (enddate == null || CurrentTime <= enddate.Value)
                            {
                                if (item.Recorded == "Taken")
                                {
                                    SetColour = Colors.LightGreen;
                                }
                                else
                                {
                                    SetColour = Color.FromArgb("#ff6666");
                                }
                                var NewMed = new usermedication();
                                NewMed.MedDateTime = RecordTime;
                                NewMed.Colour = SetColour;
                                NewMed.Action = item.Recorded;
                                NewMed.id = item.id;
                                MedicationList.Add(NewMed);

                            }
                        }
                    }
                    foreach (var item in MedicationList)
                    {
                        foreach (var x in MedSelected.schedule)
                        {
                            if (item.id == x.id.ToString())
                            {
                                item.Dosage = x.Dosage;
                                item.unit = x.dosageunit;
                            }
                        }
                    }

					var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;

                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(1))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);

                            if (enddate != null && scheduleDateTime > enddate.Value)
                            {
                                continue;
                            }

                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue; 
                            }

                            var newItem = new usermedication
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit, 
                                Colour = Colors.LightGray,
                                Action = "Not Recorded",
                          };

                            MedicationNotRecordedList.Add(newItem);
                        }
                    }

                    foreach(var item in MedicationNotRecordedList)
                    {
                        if (!MedicationList.Any(m => m.MedDateTime == item.MedDateTime))
                        {
                            MedicationList.Add(item);
                        }
                    }

                    var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                    UserMedicationSchedule.ItemsSource = sortedList;
                    UserMedicationSchedule.HeightRequest = sortedList.Count * 120;

                    //Filtered List 
                    TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                    NotTakenMedicationList = MedicationList.Where(s => s.Action.Equals("Not Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                    NotRecordedMedicationList = MedicationList.Where(s => s.Action.Equals("Not Recorded", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();

                }
                else if (Freq[0] == "Weekly" || Freq[0] == "Weekly ")
                {
                    DateTime? enddate = null;
                    //Populate with Schedule and Medications Not Taken 
                    if (MedSelected.feedback == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                       
                        foreach (var item in MedSelected.feedback)
                        {
                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                          
                            if (!string.IsNullOrEmpty(MedSelected.enddate))
                            {
                                enddate = DateTime.Parse(MedSelected.enddate);
                            }

                            var CurrentTime = DateTime.Parse(item.datetime);
                            if (enddate == null || CurrentTime <= enddate.Value)
                            {
                                if (item.Recorded == "Taken")
                                {
                                    SetColour = Colors.LightGreen;
                                }
                                else
                                {
                                    SetColour = Color.FromArgb("#ff6666");
                                }
                                var NewMed = new usermedication();
                                NewMed.MedDateTime = RecordTime;
                                NewMed.Colour = SetColour;
                                NewMed.Action = item.Recorded;
                                NewMed.id = item.id;
                                MedicationList.Add(NewMed);
                            }
                        }
                        foreach (var item in MedicationList)
                        {
                            foreach (var x in MedSelected.schedule)
                            {
                                if (item.id == x.id.ToString())
                                {
                                    item.Dosage = x.Dosage;
                                    item.unit = x.dosageunit;
                                }
                            }
                        }
                    }
                    var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;

                    //add the days to med schedule

                    if (Freq[1].Contains(','))
                    {
                        var i = 0;
                        foreach (var item in MedSelected.schedule)
                        {
                            var GetDay = MedSelected.TimeDosage[i].Split('|');
                            item.Day = GetDay[2];
                            i++;
                        }

                    }
                    else
                    {

                        foreach (var item in MedSelected.schedule)
                        {
                            item.Day = Freq[1];
                        }
                    }

                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(1))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            var Day = ParseDay(item.Day);
                            if (date.DayOfWeek != Day)
                            {
                                continue;
                            }
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);
                            if (enddate != null && scheduleDateTime > enddate.Value)
                            {
                                continue;
                            }
                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }
                            var newItem = new usermedication
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit,
                                Colour = Colors.LightGray,
                                Action = "Not Recorded",
                            };
                            MedicationNotRecordedList.Add(newItem);
                        }
                    }
                    foreach (var item in MedicationNotRecordedList)
                    {
                        if (!MedicationList.Any(m => m.MedDateTime == item.MedDateTime))
                        {
                            MedicationList.Add(item);
                        }
                    }
                    if (MedicationList.Count == 0)
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;
                    }
                    else
                    {
                        var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                        UserMedicationSchedule.ItemsSource = sortedList;
                        UserMedicationSchedule.HeightRequest = sortedList.Count * 120;

                        //Filtered List 
                        TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                        NotTakenMedicationList = MedicationList.Where(s => s.Action.Equals("Not Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                        NotRecordedMedicationList = MedicationList.Where(s => s.Action.Equals("Not Recorded", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                    }
                }
                else if (Freq[0] == "Days Interval")
                {
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;

                    DateTime? enddate = null;
                    if (!string.IsNullOrEmpty(MedSelected.enddate))
                    {
                        enddate = DateTime.Parse(MedSelected.enddate);
                    }

                    if (MedSelected.feedback == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        foreach (var item in MedSelected.feedback)
                        {
                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            var CurrentTime = DateTime.Parse(item.datetime);
                            if (enddate == null || CurrentTime <= enddate.Value)
                            {
                                if (item.Recorded == "Taken")
                                {
                                    SetColour = Colors.LightGreen;
                                }
                                else
                                {
                                    SetColour = Color.FromArgb("#ff6666");
                                }
                                var NewMed = new usermedication();
                                NewMed.MedDateTime = RecordTime;
                                NewMed.Colour = SetColour;
                                NewMed.Action = item.Recorded;
                                NewMed.id = item.id;
                                MedicationList.Add(NewMed);
                            }
                        }
                        foreach (var item in MedicationList)
                        {
                            foreach (var x in MedSelected.schedule)
                            {
                                if (item.id == x.id.ToString())
                                {
                                    item.Dosage = x.Dosage;
                                    item.unit = x.dosageunit;
                                }
                            }
                        }
                    }
                    var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;
                    var freq = MedSelected.frequency.Split('|');
                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(Int32.Parse(freq[1])))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);
                            if (enddate != null && scheduleDateTime > enddate.Value)
                            {
                                continue;
                            }
                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }
                            var newItem = new usermedication
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit,
                                Colour = Colors.LightGray,
                                Action = "Not Recorded",
                            };
                            MedicationNotRecordedList.Add(newItem);
                        }
                    }
                    foreach (var item in MedicationNotRecordedList)
                    {
                        if (!MedicationList.Any(m => m.MedDateTime == item.MedDateTime))
                        {
                            MedicationList.Add(item);
                        }
                    }
                    if (MedicationList.Count == 0)
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;
                    }
                    else
                    {
                        var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                        UserMedicationSchedule.ItemsSource = sortedList;
                        UserMedicationSchedule.HeightRequest = sortedList.Count * 120;

                        //Filtered List 
                        TakenMedicationList = MedicationList.Where(s => s.Action.Equals("Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                        NotTakenMedicationList = MedicationList.Where(s => s.Action.Equals("Not Taken", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();
                        NotRecordedMedicationList = MedicationList.Where(s => s.Action.Equals("Not Recorded", StringComparison.OrdinalIgnoreCase)).OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToObservableCollection();

                    }
                }  
            }
            else
            {
                //Other As Required 
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    DayOfWeek ParseDay(string day)
    {
        return day switch
        {
            "Mon" => DayOfWeek.Monday,
            "Tues" => DayOfWeek.Tuesday,
            "Tue" => DayOfWeek.Tuesday,
            "Wed" => DayOfWeek.Wednesday,
            "Thurs" => DayOfWeek.Thursday,
            "Fri" => DayOfWeek.Friday,
            "Sat" => DayOfWeek.Saturday,
            "Sun" => DayOfWeek.Sunday,
            _ => throw new ArgumentException("Invalid day format")
        };
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //var SelectedFrame = (sender) as Frame;
        try
        {
            var SF = (TappedEventArgs)e;
            string parameter = (string)SF.Parameter;
            AllFrame.Opacity = 0;
            TakenFrame.Opacity = 0;
            if (NotTakenFrame.IsEnabled != false)
            {
                NotTakenFrame.Opacity = 0;
            }
            if (NotTakenFrame.IsEnabled != false)
            {
                NotRecordedFrame.Opacity = 0;
            }
            var Check = string.Empty; 
            if (parameter == "All")
            {
                AllFrame.Opacity = 0.2;

                UserMedicationSchedule.IsVisible = true;
                noFilterstack.IsVisible = false;

                var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                UserMedicationSchedule.ItemsSource = sortedList;
                UserMedicationSchedule.HeightRequest = sortedList.Count * 120;
               
            }
            else if (parameter == "Taken")
            {
                TakenFrame.Opacity = 0.2;

                //Only Show Taken Items 
                NoDatalbl.Text = "No Records Contain Taken";
               
                if (TakenMedicationList.Count > 0)
                {
                    UserMedicationSchedule.IsVisible = true;
                    noFilterstack.IsVisible = false;

                    var sortedList = TakenMedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                    UserMedicationSchedule.ItemsSource = sortedList;
                    UserMedicationSchedule.HeightRequest = sortedList.Count * 120;
                }
                else
                {
                    UserMedicationSchedule.IsVisible = false;
                    noFilterstack.IsVisible = true;
                }
            }
            else if (parameter == "NotTaken")
            {
                if (NotTakenFrame.BackgroundColor != Colors.White)
                {
                    NotTakenFrame.Opacity = 0.2;

                    //Only Show NotTaken Items 
                    NoDatalbl.Text = "No Records Contain Not Taken";

                    if (NotTakenMedicationList.Count > 0)
                    {
                        UserMedicationSchedule.IsVisible = true;
                        noFilterstack.IsVisible = false;

                        var sortedList = NotTakenMedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                        UserMedicationSchedule.ItemsSource = sortedList;
                        UserMedicationSchedule.HeightRequest = sortedList.Count * 120;
                    }
                    else
                    {
                        UserMedicationSchedule.IsVisible = false;
                        noFilterstack.IsVisible = true;
                    }
                }
            }
            else if (parameter == "NotRecorded")
            {
                if (NotRecordedFrame.BackgroundColor != Colors.White)
                {
                    NotRecordedFrame.Opacity = 0.2;
                    //Only Show NotRecorded Items 
                    NoDatalbl.Text = "No Records Contain Not Recorded";

                    if (NotRecordedMedicationList.Count > 0)
                    {
                        UserMedicationSchedule.IsVisible = true;
                        noFilterstack.IsVisible = false;

                        var sortedList = NotRecordedMedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                        UserMedicationSchedule.ItemsSource = sortedList;
                        UserMedicationSchedule.HeightRequest = sortedList.Count * 120;
                    }
                    else
                    {
                        UserMedicationSchedule.IsVisible = false;
                        noFilterstack.IsVisible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}