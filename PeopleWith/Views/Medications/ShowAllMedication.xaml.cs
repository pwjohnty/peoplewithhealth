using System.Collections.ObjectModel;

namespace PeopleWith;

public partial class ShowAllMedication : ContentPage
{
	usermedication MedSelected = new usermedication();
	ObservableCollection<usermedication> MedicationList = new ObservableCollection<usermedication>();
    ObservableCollection<usermedication> MedicationNotRecordedList = new ObservableCollection<usermedication>();
    Color SetColour; 

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

						foreach(var item in MedSelected.feedback)
						{

							var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
							if (item.Recorded == "Taken")
							{
								 SetColour = Colors.ForestGreen;
							}
							else
							{
                                 SetColour = Colors.IndianRed;
                            }
							var NewMed = new usermedication();
							NewMed.MedDateTime = RecordTime;
						    NewMed.Colour = SetColour;
                            NewMed.Action = item.Recorded;
							NewMed.id = item.id; 
						    MedicationList.Add(NewMed);

                        }

						foreach(var item in MedicationList)
						{
							foreach(var x in MedSelected.schedule)
							{
								if(item.id == x.id.ToString())
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
				else if(Freq[0] == "Daily")
			    {
                    //Populate with Schedule and Medications Not Taken 
                    nodatastack.IsVisible = false;
                    datastack.IsVisible = true;

                    foreach (var item in MedSelected.feedback)
                    {

                        var RecordTime = DateTime.Parse(item.datetime).ToString("dd MMMM yy, HH:mm");
                        if (item.Recorded == "Taken")
                        {
                            SetColour = Colors.Green;
                        }
                        else
                        {
                            SetColour = Colors.Red;
                        }
                        var NewMed = new usermedication();
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

                            var newItem = new usermedication
                            {
                                id = item.id.ToString(),
                                MedDateTime = scheduleDateTime.ToString("dd MMMM yy, HH:mm"),
                                Dosage = item.Dosage,
                                unit = item.dosageunit, 
                                Colour = Colors.Brown,
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
                    UserMedicationSchedule.HeightRequest = sortedList.Count * 140;



                }
                else if (Freq[0] == "Weekly" || Freq[0] == "Weekly ")
                {
                    //Populate with Schedule and Medications Not Taken 

                }

                else if (Freq[0] == "Days Intercal")
                {
                    //Populate with Schedule and Medications Not Taken 

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
}