//using Android.Net.Wifi.Aware;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.Maui.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using Windows.System;
//using static Android.Gms.Common.Apis.Api;

namespace PeopleWith
{
    public class APICalls
    {
        //add the names of the api followed with the url connection
       
        //Crash
        public const string AddCrash = "https://pwdevapi.peoplewith.com/api/crashlog";

        //Sign-Up Code
        public const string CheckSignUpCode = "https://pwdevapi.peoplewith.com/api/signupcode?$filter=signupcodeid%20eq%20";
        public const string Checksignupregquestions = "https://pwdevapi.peoplewith.com/api/question?$filter=signupcodereferral%20eq%20";
        public const string Checksignupreganswers = "https://pwdevapi.peoplewith.com/api/answer?$filter=signupcodereferral%20eq%20";
        public const string CheckConsentforsignupcode = "https://pwdevapi.peoplewith.com/api/consent?$filter=signupcodeid%20eq%20";
      
        //User
        public const string InsertUser = "https://pwdevapi.peoplewith.com/api/user/";
        public const string InsertUserResponse = "https://pwdevapi.peoplewith.com/api/userresponse/";
        public const string Checkuseremail = "https://pwdevapi.peoplewith.com/api/user?$filter=email%20eq%20";

        //Diagnosis
        public const string InsertUserDiagnosis = "https://pwdevapi.peoplewith.com/api/userdiagnosis/";

        //Measurements
        public const string InsertUserMeasurements = "https://pwdevapi.peoplewith.com/api/usermeasurement/";

        //Symptoms
        public const string usersymptoms = "https://pwdevapi.peoplewith.com/api/usersymptom";
        public const string GetSymptoms = "https://pwdevapi.peoplewith.com/api/symptom?$select=symptomid,title";
        public const string InsertUserSymptoms = "https://pwdevapi.peoplewith.com/api/usersymptom/";

        //Medications 
        public const string usermedications = "https://pwdevapi.peoplewith.com/api/usermedication";
        public const string InsertUserMedications = "https://pwdevapi.peoplewith.com/api/usermedication/";
        public const string GetMedications = "https://pwdevapi.peoplewith.com/api/medication?$select=medicationid,title";

        //Supplements
        public const string usersupplements = "https://pwdevapi.peoplewith.com/api/usersupplement";
        public const string InsertUserSupplements = "https://pwdevapi.peoplewith.com/api/usersupplement";
        public const string GetSupplements = "https://pwdevapi.peoplewith.com/api/supplement?$select=supplementid,title";



        public async Task<ObservableCollection<measurement>> GetMeasurements()
        {
            try
            {
                var url = "https://pwdevapi.peoplewith.com/api/measurement?$select=measurementid,measurementname,units";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseMeasurement>(contentconsent);
                    var consent = userResponseconsent.Value;

                    return new ObservableCollection<measurement>(consent);

                }
                else
                {
                    return null;
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<ObservableCollection<usermeasurement>> GetUserMeasurements()
        {
            try
            {
                string userid = Preferences.Default.Get("userid", "Unknown");

                var url = "https://pwdevapi.peoplewith.com/api/usermeasurement?$filter=userid%20eq%20" + "%27" + userid + "%27";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserMeasurement>(contentconsent);
                    var consent = userResponseconsent.Value;

                    return new ObservableCollection<usermeasurement>(consent);

                }
                else
                {
                    return null;
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public async Task<usermeasurement> InsertUsermeasurement(usermeasurement item)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = APICalls.InsertUserMeasurements;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<usermeasurement>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, contenttt);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    item.id = id;
                    // Return the inserted item
                    return item;
                }
                else
                {
                    return null;
                    // Optionally handle the scenario where the API call was not successful
                    //throw new Exception("Failed to insert user measurement: " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                return null;
                // Handle exceptions
                //throw new Exception("An error occurred while inserting user measurement: " + ex.Message);
            }
        }

        public async Task DeleteUserMeasurements(ObservableCollection<usermeasurement> deletelistpassed)
        {
            try
            {
                //string userid = Preferences.Default.Get("userid", "Unknown");
                for (int i = 0; i < deletelistpassed.Count; i++)
                {


                    var url = $"https://pwdevapi.peoplewith.com/api/usermeasurement/id/{deletelistpassed[i].id}";
                    HttpClient client = new HttpClient();
                    HttpResponseMessage responseconsent = await client.DeleteAsync(url);

                    if (responseconsent.IsSuccessStatusCode)
                    {
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        public class SingleUserSymptom
        {
            public ObservableCollection<rawusersymptom> Value { get; set; }
        }

        public async Task<ObservableCollection<usersymptom>> GetUserSymptomAsync()
        {
            try
            {


                HttpClient client = new HttpClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                //var url = $"https://pwdevapi.peoplewith.com/api/usersymptom/userid/{USERID}";
                string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{userid}'";
                //string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{USERID}'&$select=id,userid,symptomid,feedback,symptomtitle";
                HttpResponseMessage response = await client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                // Deserialize the response into a generic structure
                var rawResponse = JsonConvert.DeserializeObject<SingleUserSymptom>(data);
                var userSymptomsList = new List<usersymptom>();
                if (rawResponse?.Value != null)
                {
                    foreach (var rawSymptom in rawResponse.Value)
                    {
                        var newUserSymptom = new usersymptom
                        {
                            id = rawSymptom.id,
                            userid = rawSymptom.userid,
                            symptomid = rawSymptom.symptomid,
                            symptomtitle = rawSymptom.symptomtitle,
                            deleted = rawSymptom.deleted,
                            feedback = new ObservableCollection<symptomfeedback>()
                        };
                        // Deserialize the feedback string into the FeedbackList
                        var feedbackSymptoms = JsonConvert.DeserializeObject<List<symptomfeedback>>(rawSymptom.feedback);
                        // Add only the relevant feedback to this usersymptom

                        foreach (var feedback in feedbackSymptoms)
                        {
                            newUserSymptom.feedback.Add(feedback);
                        }
                        userSymptomsList.Add(newUserSymptom);
                    }
                }
                return new ObservableCollection<usersymptom>(userSymptomsList);
            }
            catch (Exception ex)
            {
                return new ObservableCollection<usersymptom>();
            }
        }

        public async Task<ObservableCollection<usersymptom>> GetUserSymptoms()
        {
            try
            {
                string userid = Preferences.Default.Get("userid", "Unknown");

                var url = "https://pwdevapi.peoplewith.com/api/usersymptom?$filter=userid%20eq%20" + "%27" + userid + "%27";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserSymptom>(contentconsent);
                    var consent = userResponseconsent.Value;

                    return new ObservableCollection<usersymptom>(consent);

                }
                else
                {
                    return null;
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ObservableCollection<symptom>> GetSymptomSearchAsync()
        {
            try
            {
                var url = "https://pwdevapi.peoplewith.com/api/symptom?$select=symptomid,title,status,classification";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiResponseSymptom>(data);
                ObservableCollection<symptom> users = userResponse.Value;
                return new ObservableCollection<symptom>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<usersymptom> PostSymptomAsync(usersymptom usersymptomsPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://pwdevapi.peoplewith.com/api/usersymptom";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usersymptom>(usersymptomsPassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    usersymptomsPassed.id = id;
                    // Return the inserted item
                    return usersymptomsPassed;


                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
                // return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Update Symptomfeedback Data
        public async Task PutSymptomAsync(ObservableCollection<usersymptom> Updatefeedback)
        {
            try
            {
                var id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/usersymptom/id/{id}";
                var feedbacks = Updatefeedback[0].feedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await client.SendAsync(request);
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine("Successfully updated feedback");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        //Update All Page 
        public async Task UpdateAll(ObservableCollection<usersymptom> Updatefeedback)
        {
            try
            {
                for (int i = 0; i < Updatefeedback.Count; i++)
                {
                    var id = Updatefeedback[i].id;
                    var url = $"https://pwdevapi.peoplewith.com/api/usersymptom/id/{id}";
                    var feedbacks = Updatefeedback[i].feedback;
                    string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks });
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Patch, url)
                        {
                            Content = content
                        };
                        var response = await client.SendAsync(request);
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            Console.WriteLine("Successfully updated feedback");
                        }
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }


        public async Task<ObservableCollection<interventiontrigger>> GetAsyncInterventionTrigger()
        {
            try
            {

                var URl = "https://pwdevapi.peoplewith.com/api/interventiontrigger?$select=title,category,type";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiInterventionTrigger>(data);
                ObservableCollection<interventiontrigger> users = userResponse.Value;
                return new ObservableCollection<interventiontrigger>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<interventiontrigger>();
            }
        }

        //Get All Medication Preperation
        public async Task<ObservableCollection<preparation>> GetMedPreparation()
        {
            try
            {
                var url = "https://pwdevapi.peoplewith.com/api/preparation/";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponsePrepartion>(contentconsent);
                    var consent = userResponseconsent.Value;

                    return new ObservableCollection<preparation>(consent);

                }
                else
                {
                    var errorResponse = await responseconsent.Content.ReadAsStringAsync();
                    return null;
                }



            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Update UserMedication in DB 
        public async Task<usermedication> PostMedicationAsync(usermedication usermedpassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://pwdevapi.peoplewith.com/api/usermedication";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usermedication>(usermedpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    usermedpassed.id = id;
                    // Return the inserted item
                    return usermedpassed;


                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
                // return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public class SingleUserMedication
        {
            public ObservableCollection<rawusermedication> Value { get; set; }
        }



        //Get All User Medications 
        public async Task<ObservableCollection<usermedication>> GetUserMedicationsAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                //var url = $"https://pwdevapi.peoplewith.com/api/usermedication/userid/{USERID}";
                string urlWithQuery = $"{usermedications}?$filter=userid eq '{userid}'";
                //string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{USERID}'&$select=id,userid,symptomid,feedback,symptomtitle";
                HttpResponseMessage response = await client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                // Deserialize the response into a generic structure
                var rawResponse = JsonConvert.DeserializeObject<SingleUserMedication>(data);
                var userSymptomsList = new List<usermedication>();
                if (rawResponse?.Value != null)
                {
                    foreach (var rawSymptom in rawResponse.Value)
                    {
                        var newUserSymptom = new usermedication
                        {
                            id = rawSymptom.id,
                            userid = rawSymptom.userid,
                            medicationid = rawSymptom.medicationid,
                            medicationtitle = rawSymptom.medicationtitle,
                            startdate = rawSymptom.startdate,
                            enddate = rawSymptom.enddate,
                            frequency = rawSymptom.frequency,
                            diagnosis = rawSymptom.diagnosis,
                            status = rawSymptom.status,
                            feedback = rawSymptom.feedback,
                            details = rawSymptom.details,
                            formulation = rawSymptom.formulation,
                            preparation = rawSymptom.preparation,
                            unit = rawSymptom.unit,
                            schedule = new ObservableCollection<MedtimesDosages>()

                        };
                        // Deserialize the feedback string into the FeedbackList
                        var feedbackSymptoms = JsonConvert.DeserializeObject<List<MedtimesDosages>>(rawSymptom.schedule);
                        // Add only the relevant feedback to this usersymptom

                        foreach (var feedback in feedbackSymptoms)
                        {
                            newUserSymptom.schedule.Add(feedback);
                            var dosage = feedback.Dosage;
                            var time = feedback.time;
                            //Daily
                            var getfreq = newUserSymptom.frequency.Split('|'); 
                            if (getfreq[0] == "Daily" || getfreq[0] == "Days Interval")
                            {
                                var DosageTime = time + "|" + dosage;
                                newUserSymptom.TimeDosage.Add(DosageTime);
                            }
                            //Weekly
                            else if (getfreq[0] == "Weekly" || getfreq[0] == "Weekly ")
                            {
                                var day = feedback.Day;
                                var DosageTime = time + "|" + dosage + "|" + day;
                                newUserSymptom.TimeDosage.Add(DosageTime);
                            }

                        }
                        userSymptomsList.Add(newUserSymptom);
                    }
                }
                return new ObservableCollection<usermedication>(userSymptomsList);
            }
            catch (Exception ex)
            {
                return new ObservableCollection<usermedication>();
            }
        }


        //Supplements 

        //Update UserSupplements in DB 
        public async Task<usersupplement> PostSupplementAsync(usersupplement usersuppassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://pwdevapi.peoplewith.com/api/usersupplement";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usersupplement>(usersuppassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    usersuppassed.id = id;
                    // Return the inserted item
                    return usersuppassed;


                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
                // return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public class SingleUserSupplement
        {
            public ObservableCollection<rawusersupplement> Value { get; set; }
        }



        //Get All User Medications 
        public async Task<ObservableCollection<usersupplement>> GetUserSupplementsAsync()
        {
            try
            {


                HttpClient client = new HttpClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                string urlWithQuery = $"{usersupplements}?$filter=userid eq '{userid}'";
                HttpResponseMessage response = await client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                // Deserialize the response into a generic structure
                var rawResponse = JsonConvert.DeserializeObject<SingleUserSupplement>(data);
                var userSymptomsList = new List<usersupplement>();
                if (rawResponse?.Value != null)
                {
                    foreach (var rawSymptom in rawResponse.Value)
                    {
                        var newUserSymptom = new usersupplement
                        {
                            id = rawSymptom.id,
                            userid = rawSymptom.userid,
                            supplementid = rawSymptom.supplementid,
                            supplementtitle = rawSymptom.supplementtitle,
                            startdate = rawSymptom.startdate,
                            enddate = rawSymptom.enddate,
                            frequency = rawSymptom.frequency,
                            diagnosis = rawSymptom.diagnosis,
                            status = rawSymptom.status,
                            feedback = rawSymptom.feedback,
                            //details = rawSymptom.details,
                            formulation = rawSymptom.formulation,
                            preparation = rawSymptom.preparation,
                            unit = rawSymptom.unit,
                            schedule = new ObservableCollection<MedtimesDosages>()

                        };
                        // Deserialize the feedback string into the FeedbackList
                        var feedbackSymptoms = JsonConvert.DeserializeObject<List<MedtimesDosages>>(rawSymptom.schedule);
                        // Add only the relevant feedback to this usersymptom

                        foreach (var feedback in feedbackSymptoms)
                        {
                            newUserSymptom.schedule.Add(feedback);
                        }
                        userSymptomsList.Add(newUserSymptom);
                        //userSymptomsList[0].dosage

                    }
                }
                return new ObservableCollection<usersupplement>(userSymptomsList);
            }
            catch (Exception ex)
            {
                return new ObservableCollection<usersupplement>();
            }
        }



    }
}
