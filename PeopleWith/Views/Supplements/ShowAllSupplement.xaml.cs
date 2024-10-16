using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class ShowAllSupplement : ContentPage
{
    usersupplement MedSelected = new usersupplement();
    ObservableCollection<usersupplement> MedicationList = new ObservableCollection<usersupplement>();
    ObservableCollection<usersupplement> MedicationNotRecordedList = new ObservableCollection<usersupplement>();
    Color SetColour;

    public ShowAllSupplement(usersupplement SelectedMed)
    {
        try
        {
            InitializeComponent();
            MedSelected = SelectedMed;
            MedicationName.Text = MedSelected.supplementtitle;

            PopulateListView();

        }
        catch (Exception Ex)
        {

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
                    if (MedSelected.feedback == null)
                    {
                        nodatastack.IsVisible = true;
                        datastack.IsVisible = false;

                    }
                    else
                    {
                        nodatastack.IsVisible = false;
                        datastack.IsVisible = true;

                        foreach (var item in MedSelected.feedback)
                        {

                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            if (item.Recorded == "Taken")
                            {
                                SetColour = Colors.LightGreen;
                            }
                            else
                            {
                                SetColour = Color.FromArgb("#ff6666");
                            }
                            var NewMed = new usersupplement();
                            NewMed.MedDateTime = RecordTime;
                            NewMed.Colour = SetColour;
                            NewMed.Action = item.Recorded;
                            NewMed.id = item.id;
                            MedicationList.Add(NewMed);

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


                        var sortedlist = MedicationList.OrderBy(t => t.MedDateTime);
                        UserMedicationSchedule.ItemsSource = sortedlist;

                    }
                }
                else if (Freq[0] == "Daily")
                {
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;

                    if (MedSelected.feedback == null)
                    {

                    }
                    else
                    {

                        foreach (var item in MedSelected.feedback)
                        {

                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            if (item.Recorded == "Taken")
                            {
                                SetColour = Colors.LightGreen;
                            }
                            else
                            {
                                SetColour = Color.FromArgb("#ff6666");
                            }
                            var NewMed = new usersupplement();
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

                    var startdate = DateTime.Parse(MedSelected.startdate);
                    DateTime currentDateTime = DateTime.Now;

                    for (DateTime date = startdate.Date; date <= currentDateTime.Date; date = date.AddDays(1))
                    {
                        foreach (var item in MedSelected.schedule)
                        {
                            DateTime scheduleDateTime = DateTime.Parse(date.ToString("yyyy-MM-dd") + " " + item.time);

                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }

                            var newItem = new usersupplement
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

                    var sortedList = MedicationList.OrderByDescending(m => DateTime.Parse(m.MedDateTime)).ToList();
                    UserMedicationSchedule.ItemsSource = sortedList;
                    UserMedicationSchedule.HeightRequest = sortedList.Count * 140;



                }
                else if (Freq[0] == "Weekly" || Freq[0] == "Weekly ")
                {
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
                            if (item.Recorded == "Taken")
                            {
                                SetColour = Colors.LightGreen;
                            }
                            else
                            {
                                SetColour = Color.FromArgb("#ff6666");
                            }
                            var NewMed = new usersupplement();
                            NewMed.MedDateTime = RecordTime;
                            NewMed.Colour = SetColour;
                            NewMed.Action = item.Recorded;
                            NewMed.id = item.id;
                            MedicationList.Add(NewMed);
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

                        var splitdays = Freq[1].Split(',').ToList();

                        var i = 0;

                        var ii = 0;

                        var numoftimes = Convert.ToInt32(Freq[2]);

                        var num = numoftimes - 1;

                        foreach (var item in MedSelected.schedule)
                        {

                            if (ii == num)
                            {
                                item.Day = splitdays[i];
                                i++;
                                ii++;
                            }
                            else
                            {
                                item.Day = splitdays[i];
                                ii++;
                            }

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
                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }
                            var newItem = new usersupplement
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
                        UserMedicationSchedule.HeightRequest = sortedList.Count * 140;
                    }
                }
                else if (Freq[0] == "Days Interval")
                {
                    //Populate with Schedule and Medications Not Taken 
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;
                    if (MedSelected.feedback == null)
                    {
                        //Do Nothing
                    }
                    else
                    {
                        foreach (var item in MedSelected.feedback)
                        {
                            var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                            if (item.Recorded == "Taken")
                            {
                                SetColour = Colors.LightGreen;
                            }
                            else
                            {
                                SetColour = Color.FromArgb("#ff6666");
                            }
                            var NewMed = new usersupplement();
                            NewMed.MedDateTime = RecordTime;
                            NewMed.Colour = SetColour;
                            NewMed.Action = item.Recorded;
                            NewMed.id = item.id;
                            MedicationList.Add(NewMed);
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
                            if (date == currentDateTime.Date && scheduleDateTime.TimeOfDay > currentDateTime.TimeOfDay)
                            {
                                continue;
                            }
                            var newItem = new usersupplement
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
                        UserMedicationSchedule.HeightRequest = sortedList.Count * 140;
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
}