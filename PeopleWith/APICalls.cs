//using Android.Net.Wifi.Aware;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.DataSource.Extensions;
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
        public const string Checkuseremail = "https://pwdevapi.peoplewith.com/api/user?$filter=email%20eq%20";
        public const string CheckuserPassword = "https://pwdevapi.peoplewith.com/api/user?$filter=password%20eq%20";

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
        public const string InsertUserSymptoms = "https://pwdevapi.peoplewith.com/api/usersymptom/";
        public const string InsertUserMedications = "https://pwdevapi.peoplewith.com/api/usermedication/";
        public const string InsertUserDiagnosis = "https://pwdevapi.peoplewith.com/api/userdiagnosis/";
        public const string InsertUserMeasurements = "https://pwdevapi.peoplewith.com/api/usermeasurement/";
        public const string usersymptoms = "https://pwdevapi.peoplewith.com/api/usersymptom";
        //CrashLog  
        public const string CrashLog = "https://pwdevapi.peoplewith.com/api/crashlog";

        //Allergies  
        public const string Allergies = "https://pwdevapi.peoplewith.com/api/allergy";


        //Symptoms
        public const string GetSymptoms = "https://pwdevapi.peoplewith.com/api/symptom?$select=symptomid,title";

        //Medications 
        public const string usermedications = "https://pwdevapi.peoplewith.com/api/usermedication";
        public const string GetMedications = "https://pwdevapi.peoplewith.com/api/medication?$select=medicationid,title";

        //Supplements
        public const string usersupplements = "https://pwdevapi.peoplewith.com/api/usersupplement";
        public const string InsertUserSupplements = "https://pwdevapi.peoplewith.com/api/usersupplement";
        public const string GetSupplements = "https://pwdevapi.peoplewith.com/api/supplement?$select=supplementid,title";

        //User Allergies  
        public const string UserAllergies = "https://pwdevapi.peoplewith.com/api/userallergy";

        //Diagnosis 
        public const string Diagnosis = "https://pwdevapi.peoplewith.com/api/diagnosis";

        //UserDiagnosis 
        public const string UserDiagnosis = "https://pwdevapi.peoplewith.com/api/userdiagnosis";
        // Mood  
        public const string UserMood = "https://pwdevapi.peoplewith.com/api/usermood";

        //HCPS
        public const string UserHCPs = "https://pwdevapi.peoplewith.com/api/hcp";

        //Appointment
        public const string Appointments = "https://pwdevapi.peoplewith.com/api/appointment";

        //Videos 
        public const string Videos = "https://pwdevapi.peoplewith.com/api/video";
        //Videos engagement
        public const string VideosEngage = "https://pwdevapi.peoplewith.com/api/videoengagementdata";
        //User Consent 
        public const string UserConsent = "https://pwdevapi.peoplewith.com/api/userconsent";

        public const string UserQuestionnaires = "https://pwdevapi.peoplewith.com/api/userquestionnaire";

        public const string UserFeedback = "https://pwdevapi.peoplewith.com/api/userfeedback";

        public const string PrivPolicy = "https://pwdevapi.peoplewith.com/api/privacypolicy";

        public const string signupcode = "https://pwdevapi.peoplewith.com/api/signupcode";

        //Get User Details 
        public async Task<ObservableCollection<user>> GetuserDetails()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var url = $"https://pwdevapi.peoplewith.com/api/user/userid/{USERID}";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APIUserResponse>(contentconsent);
                    var consent = userResponseconsent.Value;

                    return new ObservableCollection<user>(consent);

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

        //Update User Details 


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


        public async Task<measurement> GetMeasurementsInfo(measurement Getinfo)
        {
            try
            {
                var id = Getinfo.measurementid; 
                var url = $"https://pwdevapi.peoplewith.com/api/measurement/measurementid/{id}";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseMeasurement>(contentconsent);
                    var consent = userResponseconsent.Value;
                    Getinfo.measurementinformation = consent[0].measurementinformation; 
                    return Getinfo;

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
                ObservableCollection<usermeasurement> itemstoremove = new ObservableCollection<usermeasurement>();
                string userid = Preferences.Default.Get("userid", "Unknown");

                var url = "https://pwdevapi.peoplewith.com/api/usermeasurement?$filter=userid%20eq%20" + "%27" + userid + "%27";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserMeasurement>(contentconsent);
                    var consent = userResponseconsent.Value;

                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            itemstoremove.Add(item);
                        }
                    }
                    foreach (var i in itemstoremove)
                    {
                        consent.Remove(i);
                    }

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

        //Delete User Measurement 

        public async Task DeleteUserMeasurements(ObservableCollection<usermeasurement> deletelistpassed)
        {
            try
            {
                for (int i = 0; i < deletelistpassed.Count; i++)
                {
                    var url = $"https://pwdevapi.peoplewith.com/api/usermeasurement/id/{deletelistpassed[i].id}";
                    string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = deletelistpassed[i].deleted });
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
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        //Delete User Symptom 
        public async Task DeleteSymptom(ObservableCollection<usersymptom> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/usersymptom/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
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
                            if (feedback.action == "deleted")
                            {

                            }
                            else
                            {
                                newUserSymptom.feedback.Add(feedback);
                            }
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
                HttpResponseMessage response;

                // Choose POST or PATCH based on whether the ID is null or empty
                if (string.IsNullOrEmpty(usermedpassed.id))
                {
                    response = await client.PostAsync(url, contenttts);
                }
                else
                {
                    response = await client.PatchAsync(url, contenttts);
                }

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


        public async Task<usersupplement> PostSupplementAsync(usersupplement usermedpassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://pwdevapi.peoplewith.com/api/usersupplement";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usersupplement>(usermedpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                // Choose POST or PATCH based on whether the ID is null or empty
                if (string.IsNullOrEmpty(usermedpassed.id))
                {
                    response = await client.PostAsync(url, contenttts);
                }
                else
                {
                    response = await client.PatchAsync(url, contenttts);
                }

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

        public async Task UpdateEditMedication(usermedication usermedpassed)
        {
            try
            {
                // HttpClient client = new HttpClient();
                var id = usermedpassed.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermedication/{id}";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usermedication>(usermedpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                StringContent content = new StringContent(jsonns, Encoding.UTF8, "application/json");
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var responsee = await client.SendAsync(request);
                    if (!responsee.IsSuccessStatusCode)
                    {
                        var errorResponse = await responsee.Content.ReadAsStringAsync();
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
                            deleted = rawSymptom.deleted,
                            //feedback = rawSymptom.feedback,
                            details = rawSymptom.details,
                            formulation = rawSymptom.formulation,
                            preparation = rawSymptom.preparation,
                            unit = rawSymptom.unit,
                            schedule = new ObservableCollection<MedtimesDosages>(),
                            medicationquestions = rawSymptom.medicationquestions
                        };
                        if (rawSymptom.schedule == null)
                        {

                        }
                        else
                        {
                            var feedbackSymptoms = JsonConvert.DeserializeObject<List<MedtimesDosages>>(rawSymptom.schedule);
                            // Add only the relevant feedback to this usersymptom

                            int Index = 0;

                            foreach (var feedback in feedbackSymptoms)
                            {
                                newUserSymptom.schedule.Add(feedback);
                                var dosage = feedback.Dosage;
                                var Updatetime = DateTime.Parse(feedback.time).ToString("HH:mm");
                                feedback.time = Updatetime; 
                                var time = feedback.time;
                              
                                List<string> days = new List<string>();

                                var getfreq = newUserSymptom.frequency.Split('|');

                                //weekly Days
                                if (getfreq[1].Contains(","))
                                {
                                    days = getfreq[1].Split(',').ToList();
                                    int GetCount = feedbackSymptoms.Count() / days.Count;
                                    var duplicatedDays = Enumerable.Repeat(days, GetCount).SelectMany(x => x).ToList();
                                    var uniqueDays = duplicatedDays.Distinct().ToList();
                                    days = uniqueDays.SelectMany(day => duplicatedDays.Where(d => d == day)).ToList();
                                }

                                //Daily
                                if (getfreq[0] == "Daily" || getfreq[0] == "Days Interval")
                                {
                                    var DosageTime = time + "|" + dosage;
                                    newUserSymptom.TimeDosage.Add(DosageTime);
                                }
                                //Weekly
                                else if (getfreq[0] == "Weekly" || getfreq[0] == "Weekly ")
                                {
                                    string DosageTime = String.Empty;
                                    if (getfreq[1].Contains(","))
                                    {
                                        DosageTime = time + "|" + dosage + "|" + days[Index];

                                    }
                                    else
                                    {
                                        var day = getfreq[1];
                                        DosageTime = time + "|" + dosage + "|" + day;
                                    }

                                    newUserSymptom.TimeDosage.Add(DosageTime);
                                    Index = Index + 1;
                                }

                            }
                        }

                        if (rawSymptom.feedback == null)
                        {

                        }
                        else
                        {
                            var medfeedback = JsonConvert.DeserializeObject<List<MedSuppFeedback>>(rawSymptom.feedback);

                            if (newUserSymptom.feedback == null)
                            {
                                newUserSymptom.feedback = new ObservableCollection<MedSuppFeedback>();
                            }
                            foreach (var feedback in medfeedback)
                            {
                                newUserSymptom.feedback.Add(feedback);
                            }
                        }

                        if (newUserSymptom.deleted == true)
                        {
                            //Ignore
                        }
                        else
                        {
                            userSymptomsList.Add(newUserSymptom);
                        }

                    }
                }
                return new ObservableCollection<usermedication>(userSymptomsList);

            }
            catch (Exception ex)
            {
                return new ObservableCollection<usermedication>();
            }
        }

        //Delete Usermedication 
        public async Task DeleteMedication(usermedication Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermedication/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }


        public async Task DeleteSupplement(usersupplement Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usersupplement/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //Update Medicaiton Details 
        public async Task UpdateMedicationDetails(usermedication Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermedication/id/{id}";

                var payload = new
                {
                    preparation = Updatefeedback.preparation,
                    formulation = Updatefeedback.formulation,
                    unit = Updatefeedback.unit,
                    startdate = Updatefeedback.startdate,
                    enddate = Updatefeedback.enddate,
                    frequency = Updatefeedback.frequency,
                    schedule = Updatefeedback.schedule,
                    status = Updatefeedback.status,
                    details = Updatefeedback.details,
                    medicationquestions = Updatefeedback.medicationquestions
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task UpdateSupplementDetails(usersupplement Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usersupplement/id/{id}";

                var payload = new
                {
                    preparation = Updatefeedback.preparation,
                    formulation = Updatefeedback.formulation,
                    unit = Updatefeedback.unit,
                    startdate = Updatefeedback.startdate,
                    enddate = Updatefeedback.enddate,
                    frequency = Updatefeedback.frequency,
                    schedule = Updatefeedback.schedule,
                    status = Updatefeedback.status,
                    details = Updatefeedback.details,
                    supplementquestions = Updatefeedback.supplementquestions
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task UpdateCompletedMedication(usermedication updatedmed)
        {
            try
            {
                string id = updatedmed.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermedication/id/{id}";

                var payload = new
                {
                    status = updatedmed.status
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task UpdateCompletedSupplement(usersupplement updatedmed)
        {
            try
            {
                string id = updatedmed.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usersupplement/id/{id}";

                var payload = new
                {
                    status = updatedmed.status

                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //Update Medicaiton schedule unit 
        //public async Task UpdateScheduleUnit(usermedication Updatefeedback)
        //{
        //    try
        //    {
        //        string id = Updatefeedback.id;
        //        var url = $"https://pwdevapi.peoplewith.com/api/usermedication/id/{id}";

        //        string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
        //        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        //        using (var client = new HttpClient())
        //        {
        //            var request = new HttpRequestMessage(HttpMethod.Patch, url)
        //            {
        //                Content = content
        //            };

        //            var response = await client.SendAsync(request);

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                var errorResponse = await response.Content.ReadAsStringAsync();
        //            }
        //        }

        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        return;
        //    }
        //}


        //Update medication feedback Data
        public async Task UpdateMedicationFeedbackAsync(usermedication Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermedication/id/{id}";
                var feedbacks = Updatefeedback.feedback;
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

        public async Task UpdateSupplementFeedbackAsync(usersupplement Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/usersupplement/id/{id}";
                var feedbacks = Updatefeedback.feedback;
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



        //supplments
        public async Task<ObservableCollection<usersupplement>> GetUserSupplementsAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                //var url = $"https://pwdevapi.peoplewith.com/api/usersupplement/userid/{USERID}";
                string urlWithQuery = $"{usersupplements}?$filter=userid eq '{userid}'";
                //string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{USERID}'&$select=id,userid,symptomid,feedback,symptomtitle";
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
                            deleted = rawSymptom.deleted,
                            //feedback = rawSymptom.feedback,
                            details = rawSymptom.details,
                            formulation = rawSymptom.formulation,
                            preparation = rawSymptom.preparation,
                            unit = rawSymptom.unit,
                            schedule = new ObservableCollection<MedtimesDosages>(),
                            supplementquestions = rawSymptom.supplementquestions
                        };
                        // Deserialize the feedback string into the FeedbackList
                        if (rawSymptom.schedule == null || rawSymptom.schedule == "[]" || string.IsNullOrEmpty(rawSymptom.schedule))
                        {

                        }
                        else
                        {
                            var feedbackSymptoms = JsonConvert.DeserializeObject<List<MedtimesDosages>>(rawSymptom.schedule);
                            // Add only the relevant feedback to this usersymptom

                            int Index = 0;

                                foreach (var feedback in feedbackSymptoms)
                                {
                                    newUserSymptom.schedule.Add(feedback);
                                    var dosage = feedback.Dosage;
                                    var Updatetime = DateTime.Parse(feedback.time).ToString("HH:mm");
                                    feedback.time = Updatetime;
                                    var time = feedback.time;
                                    List<string> days = new List<string>();

                                    var getfreq = newUserSymptom.frequency.Split('|');

                                    //weekly Days
                                    if (getfreq[1].Contains(","))
                                    {
                                        days = getfreq[1].Split(',').ToList();
                                        int GetCount = feedbackSymptoms.Count() / days.Count;
                                        var duplicatedDays = Enumerable.Repeat(days, GetCount).SelectMany(x => x).ToList();
                                        var uniqueDays = duplicatedDays.Distinct().ToList();
                                        days = uniqueDays.SelectMany(day => duplicatedDays.Where(d => d == day)).ToList();
                                    }

                                    //Daily
                                    if (getfreq[0] == "Daily" || getfreq[0] == "Days Interval")
                                    {
                                        var DosageTime = time + "|" + dosage;
                                        newUserSymptom.TimeDosage.Add(DosageTime);
                                    }
                                    //Weekly
                                    else if (getfreq[0] == "Weekly" || getfreq[0] == "Weekly ")
                                    {
                                        string DosageTime = String.Empty;
                                        if (getfreq[1].Contains(","))
                                        {
                                            DosageTime = time + "|" + dosage + "|" + days[Index];

                                        }
                                        else
                                        {
                                            var day = getfreq[1];
                                            DosageTime = time + "|" + dosage + "|" + day;
                                        }

                                        newUserSymptom.TimeDosage.Add(DosageTime);
                                        Index = Index + 1;
                                    }

                                }
                            
                        }

                        if (rawSymptom.feedback == null)
                        {

                        }
                        else
                        {
                            var medfeedback = JsonConvert.DeserializeObject<List<MedSuppFeedback>>(rawSymptom.feedback);

                            if (newUserSymptom.feedback == null)
                            {
                                newUserSymptom.feedback = new ObservableCollection<MedSuppFeedback>();
                            }
                            foreach (var feedback in medfeedback)
                            {
                                newUserSymptom.feedback.Add(feedback);
                            }
                        }

                        if (newUserSymptom.deleted == true)
                        {
                            //Ignore
                        }
                        else
                        {
                            userSymptomsList.Add(newUserSymptom);
                        }

                    }
                }
                return new ObservableCollection<usersupplement>(userSymptomsList);
            }
            catch (Exception ex)
            {
                return new ObservableCollection<usersupplement>();
            }
        }



        //Add Crashlog Item 
        public async Task<crashlog> InsertCrashLog(crashlog item)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = APICalls.CrashLog;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<crashlog>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, contenttt);

                if (response.IsSuccessStatusCode)
                {
                    //Nothing to return
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
                //Error Occured on Crashlog 
            }
        }

        //Get All Allergies Data 

        public class ApiAllergies
        {
            public ObservableCollection<allergies> Value { get; set; }
        }

        public async Task<ObservableCollection<allergies>> GetAsyncAllergies()
        {
            try
            {
                HttpClient client = new HttpClient();
                var URl = APICalls.Allergies;
                HttpResponseMessage response = await client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiAllergies>(data);
                ObservableCollection<allergies> users = userResponse.Value;
                return new ObservableCollection<allergies>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<allergies>();
            }
        }


        //Get Single Allergy Informaiton 
        public async Task<allergies> GetAsyncSingleAllergy(allergies GetInfo)
        {
            try
            {
                var id = GetInfo.Allergyid;
                HttpClient client = new HttpClient();

                var URl = $"https://pwdevapi.peoplewith.com/api/allergy/allergyid/{id}";

                HttpResponseMessage response = await client.GetAsync(URl);

                if (response.IsSuccessStatusCode)
                {
                    string contentconsent = await response.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiAllergies>(contentconsent);
                    var consent = userResponseconsent.Value;
                    GetInfo.Allergyinformation = consent[0].Allergyinformation;
                    return GetInfo;

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



        //Post UserAllergies Data  
        public async Task<ObservableCollection<userallergies>> PostUserAllergiesAsync(ObservableCollection<userallergies> AllergyPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urls = APICalls.UserAllergies;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userallergies>(AllergyPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urls, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();
                    var createdAt = jsonResponse["value"]?[0]?["createdAt"]?.ToString();
                    AllergyPassed[0].id = id;
                    var AllergyDate = DateTime.Parse(createdAt);
                    AllergyPassed[0].createdAt = AllergyDate.ToString("dd/MM/yyyy");
                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                }


                return new ObservableCollection<userallergies>(AllergyPassed);
            }

            catch (Exception ex)
            {
                return new ObservableCollection<userallergies>();
            }
        }

        //Get UserAllergies  

        public class GetUserAllergies
        {
            public ObservableCollection<userallergies> Value { get; set; }
        }

        public async Task<ObservableCollection<userallergies>> GetUserAllergiesAsync(string USERID)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlWithQuery = $"{UserAllergies}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<GetUserAllergies>(data);
                ObservableCollection<userallergies> users = userResponse.Value;
                foreach (var item in userResponse.Value)
                {
                    string dateString = item.createdAt;
                    DateTime parsedDate = DateTime.Parse(dateString, null, System.Globalization.DateTimeStyles.RoundtripKind);
                    string formattedDate = parsedDate.ToString("dd/MM/yyyy");
                    item.createdAt = formattedDate;
                }

                return new ObservableCollection<userallergies>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<userallergies>();
            }

        }


        //Delete UserAllergy  

        public async Task DeleteUserAllergy(ObservableCollection<userallergies> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/userallergy/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //Get All Diagnosis Data 

        public class ApiDiagnosis
        {
            public ObservableCollection<diagnosis> Value { get; set; }
        }
        public async Task<ObservableCollection<diagnosis>> GetAsyncDiagnosis()
        {
            try
            {
                HttpClient client = new HttpClient();
                var URl = "https://pwdevapi.peoplewith.com/api/diagnosis";
                HttpResponseMessage response = await client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiDiagnosis>(data);
                ObservableCollection<diagnosis> users = userResponse.Value;
                return new ObservableCollection<diagnosis>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<diagnosis>();
            }
        }

        //public class ApiSingleDiagnosis
        //{
        //    public List<diagnosis> Value { get; set; }
        //}

        //Get Single Diagnosis Information
        public async Task<diagnosis> GetAsyncSingleDiagnosis(diagnosis GetInfo)
        {
            try
            {
                var id = GetInfo.Diagnosisid;
                HttpClient client = new HttpClient();

                // Use string id interpolation to insert Diagid
                var URl = $"https://pwdevapi.peoplewith.com/api/diagnosis/diagnosisid/{id}";

                HttpResponseMessage response = await client.GetAsync(URl);

                if (response.IsSuccessStatusCode)
                {
                    string contentconsent = await response.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiDiagnosis>(contentconsent);
                    var consent = userResponseconsent.Value;
                    GetInfo.Diagnosisinformation = consent[0].Diagnosisinformation;
                    return GetInfo;

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

        //Post UserDiagnosis Data  
        public async Task<ObservableCollection<userdiagnosis>> PostUserDiagnosisAsync(ObservableCollection<userdiagnosis> UserDiagnosisPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urls = APICalls.UserDiagnosis;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userdiagnosis>(UserDiagnosisPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urls, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();
                    var createdAt = jsonResponse["value"]?[0]?["createdAt"]?.ToString();
                    UserDiagnosisPassed[0].id = id;
                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                }


                return new ObservableCollection<userdiagnosis>(UserDiagnosisPassed);
            }

            catch (Exception ex)
            {
                return new ObservableCollection<userdiagnosis>();
            }
        }

        //Get UserDiagnosis  

        public class GetUserDiagnosis
        {
            public ObservableCollection<userdiagnosis> Value { get; set; }
        }

        public async Task<ObservableCollection<userdiagnosis>> GetUserDiagnosisAsync(string USERID)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlWithQuery = $"{UserDiagnosis}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<GetUserDiagnosis>(data);
                ObservableCollection<userdiagnosis> users = userResponse.Value;
                return new ObservableCollection<userdiagnosis>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<userdiagnosis>();
            }

        }


        //Delete UserDiagnosis  

        public async Task DeleteDiagnosis(ObservableCollection<userdiagnosis> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/userdiagnosis/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //Update Diagnosis Date 
        public async Task PutDiagnosisAsync(ObservableCollection<userdiagnosis> Updatefeedback)
        {
            try
            {

                var id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/userdiagnosis/id/{id}";
                var feedbacks = Updatefeedback[0].dateofdiagnosis;

                string json = System.Text.Json.JsonSerializer.Serialize(new { dateofdiagnosis = feedbacks });
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
                        Console.WriteLine("Successfully updated Diagnosis Date");
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }




        //Supplements 

        //Update UserSupplements in DB 


        public class SingleUserSupplement
        {
            public ObservableCollection<rawusersupplement> Value { get; set; }
        }



        //Get All User Supplements
        //public async Task<ObservableCollection<usersupplement>> GetUserSupplementsAsync()
        //{
        //    try
        //    {


        //        HttpClient client = new HttpClient();
        //        string userid = Preferences.Default.Get("userid", "Unknown");
        //        string urlWithQuery = $"{usersupplements}?$filter=userid eq '{userid}'";
        //        HttpResponseMessage response = await client.GetAsync(urlWithQuery);
        //        string data = await response.Content.ReadAsStringAsync();
        //        // Deserialize the response into a generic structure
        //        var rawResponse = JsonConvert.DeserializeObject<SingleUserSupplement>(data);
        //        var userSymptomsList = new List<usersupplement>();
        //        if (rawResponse?.Value != null)
        //        {
        //            foreach (var rawSymptom in rawResponse.Value)
        //            {
        //                var newUserSymptom = new usersupplement
        //                {
        //                    id = rawSymptom.id,
        //                    userid = rawSymptom.userid,
        //                    supplementid = rawSymptom.supplementid,
        //                    supplementtitle = rawSymptom.supplementtitle,
        //                    startdate = rawSymptom.startdate,
        //                    enddate = rawSymptom.enddate,
        //                    frequency = rawSymptom.frequency,
        //                    diagnosis = rawSymptom.diagnosis,
        //                    status = rawSymptom.status,
        //                    feedback = rawSymptom.feedback,
        //                    //details = rawSymptom.details,
        //                    formulation = rawSymptom.formulation,
        //                    preparation = rawSymptom.preparation,
        //                    unit = rawSymptom.unit,
        //                    schedule = new ObservableCollection<MedtimesDosages>()

        //                };
        //                // Deserialize the feedback string into the FeedbackList
        //                var feedbackSymptoms = JsonConvert.DeserializeObject<List<MedtimesDosages>>(rawSymptom.schedule);
        //                // Add only the relevant feedback to this usersymptom

        //                foreach (var feedback in feedbackSymptoms)
        //                {
        //                    newUserSymptom.schedule.Add(feedback);
        //                }
        //                userSymptomsList.Add(newUserSymptom);
        //                //userSymptomsList[0].dosage

        //            }
        //        }
        //        return new ObservableCollection<usersupplement>(userSymptomsList);
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ObservableCollection<usersupplement>();
        //    }
        //}




        //Get User Mood 
        public class GetUserMood
        {
            public ObservableCollection<usermood> Value { get; set; }
        }

        public async Task<ObservableCollection<usermood>> GetUserMoodsAsync(string USERID)
        {
            try
            {
                HttpClient client = new HttpClient();
                string urlWithQuery = $"{UserMood}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<GetUserMood>(data);
                ObservableCollection<usermood> users = userResponse.Value;
                foreach (var item in userResponse.Value)
                {
                    if (item.title != null)
                    {
                        string GetSource = item.title.ToLower();
                        item.source = GetSource + ".png";
                    }

                }

                return new ObservableCollection<usermood>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<usermood>();
            }

        }


        //Post UserMood Data 
        public async Task<ObservableCollection<usermood>> PostUserMoodAsync(ObservableCollection<usermood> MoodPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urls = APICalls.UserMood;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usermood>(MoodPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urls, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();
                    var createdAt = jsonResponse["value"]?[0]?["createdAt"]?.ToString();
                    MoodPassed[0].id = id;
                    foreach (var item in MoodPassed)
                    {
                        string GetSource = item.title.ToLower();
                        item.source = GetSource + ".png";
                    }

                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                }


                return new ObservableCollection<usermood>(MoodPassed);
            }

            catch (Exception ex)
            {
                return new ObservableCollection<usermood>();
            }
        }



        //Delete UserMood Data 

        public async Task DeleteUserMood(ObservableCollection<usermood> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermood/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }



        //Update User Mood 
        public async Task PutMoodAsync(ObservableCollection<usermood> Updatefeedback)
        {
            try
            {

                var id = Updatefeedback[0].id;
                var url = $"https://pwdevapi.peoplewith.com/api/usermood/id/{id}";
                var feedbacks = Updatefeedback[0];

                //Change the following   
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    title = feedbacks.title,
                    datetime = feedbacks.datetime,
                    notes = feedbacks.notes
                });
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
                        Console.WriteLine("Successfully updated Mood");
                    }
                }
            }

            catch (Exception ex)
            {

            }
        }


        //Get All User HCP'S
        public async Task<ObservableCollection<hcp>> GetUserHCP()
        {
            try
            {
                ObservableCollection<hcp> itemstoremove = new ObservableCollection<hcp>();
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{UserHCPs}?$filter=userid eq '{userid}'";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponeHCP>(contentconsent);
                    var consent = userResponseconsent.Value;

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            itemstoremove.Add(item);
                        }
                    }

                    foreach (var item in itemstoremove)
                    {
                        consent.Remove(item);
                    }

                    return new ObservableCollection<hcp>(consent);

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


        //Add User HCP's
        public async Task<ObservableCollection<hcp>> PostUserHCPAsync(ObservableCollection<hcp> HCPPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urls = APICalls.UserHCPs;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<hcp>(HCPPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(urls, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var id = jsonResponse["value"]?[0]?["hcpid"]?.ToString();
                    HCPPassed[0].hcpid = id;

                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                }


                return new ObservableCollection<hcp>(HCPPassed);
            }

            catch (Exception ex)
            {
                return new ObservableCollection<hcp>();
            }
        }

        //Update HCP Exisiting Item 
        public async Task UpdateHCPItem(hcp Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.hcpid;
                var url = $"https://pwdevapi.peoplewith.com/api/hcp/hcpid/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(Updatefeedback);
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }




        //Delete User HCP
        public async Task DeleteUserHCP(hcp Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.hcpid;
                var url = $"https://pwdevapi.peoplewith.com/api/hcp/hcpid/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }


        //Get All User Appointments 

        public async Task<ObservableCollection<appointment>> GetUserAppointment()
        {
            try
            {
                ObservableCollection<appointment> itemstoremove = new ObservableCollection<appointment>();
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{Appointments}?$filter=userid eq '{userid}'";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    // Add Feedback Converter
                    var settings = new JsonSerializerSettings();
                    settings.Converters.Add(new AppointmentFeedbackConverter());
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponeAppointment>(contentconsent, settings);
                    var consent = userResponseconsent.Value;

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            itemstoremove.Add(item);
                        }
                    }

                    foreach (var item in itemstoremove)
                    {
                        consent.Remove(item);
                    }

                    return new ObservableCollection<appointment>(consent);

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

        //Update User Appointment

        public async Task UpdateAppointmentItem(appointment Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/appointment/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(Updatefeedback);
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //Delete User Appointment 

        public async Task DeleteUserAppointment(appointment Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/appointment/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
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
                }
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        //Add User Appointment
        public async Task<ObservableCollection<appointment>> PostUserAppointmentAsync(ObservableCollection<appointment> AppointmentPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var urls = APICalls.Appointments;

                string jsonns = System.Text.Json.JsonSerializer.Serialize<appointment>(AppointmentPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(urls, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var jsonResponse = JObject.Parse(responseContent);
                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();
                    AppointmentPassed[0].id = id;

                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                }


                return new ObservableCollection<appointment>(AppointmentPassed);
            }

            catch (Exception ex)
            {
                return new ObservableCollection<appointment>();
            }
        }


        //Get All User Videos
        public async Task<ObservableCollection<videos>> GetAllVideos()
        {
            try
            {
                ObservableCollection<videos> itemstoremove = new ObservableCollection<videos>();
                var urls = APICalls.Videos;
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urls);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseVideos>(contentconsent);
                    var consent = userResponseconsent.Value;

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            itemstoremove.Add(item);
                        }

                        item.dateandlength = item.dateadded + " " + "Length: " + item.lenght;
                        item.thumbnail = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.thumbnail;
                        item.filename = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.filename;

                        if (item.subtitle.Length > 122)
                        {
                            var SubString = item.subtitle.Substring(0, 122) + "...";
                            item.subtitleshort = SubString;
                        }
                        else
                        {
                            item.subtitleshort = item.subtitle;
                        }
                    }

                    foreach (var item in itemstoremove)
                    {
                        consent.Remove(item);
                    }

                    return new ObservableCollection<videos>(consent);

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


        public async Task<ObservableCollection<videos>> GetAllHelpVideos()
        {
            try
            {
                ObservableCollection<videos> itemstoremove = new ObservableCollection<videos>();
                var urls = APICalls.Videos;
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urls);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseVideos>(contentconsent);
                    var consent = userResponseconsent.Value;

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            itemstoremove.Add(item);
                        }
                        else if(!string.IsNullOrEmpty(item.referral))
                        {
                            if (Helpers.Settings.SignUp != item.referral)
                            {
                                itemstoremove.Add(item);
                            }
                        }
                        

                        item.dateandlength = item.dateadded + " " + "Length: " + item.lenght;
                        item.thumbnail = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.thumbnail;
                        item.filename = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.filename;

                        if (item.subtitle.Length > 122)
                        {
                            var SubString = item.subtitle.Substring(0, 122) + "...";
                            item.subtitleshort = SubString;
                        }
                        else
                        {
                            item.subtitleshort = item.subtitle;
                        }
                    }

                    foreach (var item in itemstoremove)
                    {
                        consent.Remove(item);
                    }

                    return new ObservableCollection<videos>(consent);

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

        public async Task<ObservableCollection<videos>> GetAllVideoswithsignupcode()
        {
            try
            {
                ObservableCollection<videos> itemstoremove = new ObservableCollection<videos>();
                //var urls = APICalls.Videos;
               // var signupcode = Helpers.Settings.SignUp;
                var signupcode = Helpers.Settings.SignUp;
                string urlWithQuery = $"{Videos}?$filter=referral eq '{signupcode}'";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseVideos>(contentconsent);
                    var consent = userResponseconsent.Value;

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            itemstoremove.Add(item);
                        }

                        item.dateandlength = item.dateadded + " " + "Length: " + item.lenght;
                        item.thumbnail = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.thumbnail;
                        item.filename = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.filename;

                        if (item.subtitle.Length > 122)
                        {
                            var SubString = item.subtitle.Substring(0, 122) + "...";
                            item.subtitleshort = SubString;
                        }
                        else
                        {
                            item.subtitleshort = item.subtitle;
                        }
                    }

                    foreach (var item in itemstoremove)
                    {
                        consent.Remove(item);
                    }

                    return new ObservableCollection<videos>(consent);

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

        //Update Video Engagement 
        public async Task<videoengage> PostEngagementAsync(videoengage PassedEngagement)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = APICalls.VideosEngage;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<videoengage>(PassedEngagement);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    //Do Nothing 
                    return null;
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


        //User Consent Post Data  

        public async Task<userconsent> PostUserConsentAsync(userconsent ConsentPassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = APICalls.UserConsent;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userconsent>(ConsentPassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string 
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return null;


                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //questionnaire

        public async Task<ObservableCollection<questionnaire>> GetQuestionnaires()
        {
            try
            {
                var url = "https://pwdevapi.peoplewith.com/api/questionnaire/";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseQuestionnaire>(contentconsent);
                    var consent = userResponseconsent.Value;

                    // var questionAnswers = JsonConvert.DeserializeObject<ObservableCollection<questionanswerinfo>>();
                    var newcollection = new ObservableCollection<questionnaire>();

                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            //Ignore
                       
                        }
                        else
                        {
                        



                                try
                                {
                                    // Attempt to deserialize as an array
                                    item.questionanswerjsonlist = JsonConvert.DeserializeObject<ObservableCollection<questionanswerinfo>>(item.questionanswerjson);
                                }
                                catch (JsonSerializationException)
                                {
                                    // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                    var singleItem = JsonConvert.DeserializeObject<questionanswerinfo>(item.questionanswerjson);
                                    item.questionanswerjsonlist = new ObservableCollection<questionanswerinfo> { singleItem };
                                }

                            var usersignupcode = Helpers.Settings.SignUp;

                            if (string.IsNullOrEmpty(item.signupcodeid))
                            {
                                newcollection.Add(item);
                            }
                            else if(string.IsNullOrEmpty(Helpers.Settings.SignUp))
                            {
                                //ignore it
                            }
                            else if(item.signupcodeid.Contains(usersignupcode))
                            {
                                newcollection.Add(item);
                            }
                            else
                            {
                                //ignore it
                            }


                        }
                    }

                    return new ObservableCollection<questionnaire>(newcollection);

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


        public async Task<userquestionnaire> PostUserQuestionnaire(userquestionnaire userquestionnairepassed)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = "https://pwdevapi.peoplewith.com/api/userquestionnaire";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userquestionnaire>(userquestionnairepassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["userquestionnaireid"]?.ToString();

                    userquestionnairepassed.userquestionnaireid = id;
                    // Return the inserted item
                    return userquestionnairepassed;

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

        public async Task<ObservableCollection<userquestionnaire>> GetUserQuestionnaires()
        {
            try
            {
                ObservableCollection<userquestionnaire> itemstoremove = new ObservableCollection<userquestionnaire>();
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{UserQuestionnaires}?$filter=userid eq '{userid}'";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    // Add Feedback Converter
                  //  var settings = new JsonSerializerSettings();
                  //  settings.Converters.Add(new AppointmentFeedbackConverter());
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserQuestionnaire>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<userquestionnaire>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {
                            
                        }
                        else
                        {
                            try
                            {
                                // Attempt to deserialize as an array
                                item.questionanswerjsonlist = JsonConvert.DeserializeObject<ObservableCollection<userquestionnaireresponse>>(item.questionanswerjson);
                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<userquestionnaireresponse>(item.questionanswerjson);
                                item.questionanswerjsonlist = new ObservableCollection<userquestionnaireresponse> { singleItem };
                            }
                        }

                        newcollection.Add(item);
                    }

              

                    return new ObservableCollection<userquestionnaire>(newcollection);

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


        public async Task<ObservableCollection<userfeedback>> GetUserFeedback()
        {
            try
            {
                ObservableCollection<userfeedback> itemstoremove = new ObservableCollection<userfeedback>();
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{UserFeedback}?$filter=userid eq '{userid}'";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    // Add Feedback Converter
                    //  var settings = new JsonSerializerSettings();
                    //  settings.Converters.Add(new AppointmentFeedbackConverter());
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserFeedback>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<userfeedback>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {

                        ////item.moodfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.moodfeedback);
                        //if (!string.IsNullOrEmpty(item.medicationfeedback))
                        //{
                        //    try
                        //    {
                        //        // Attempt to deserialize as an array
                        //        item.medicationfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.medicationfeedback);

                        //    }
                        //    catch (JsonSerializationException)
                        //    {
                        //        // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //        var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.medicationfeedback);
                        //        item.medicationfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //    }
                        //}

                        //if(!string.IsNullOrEmpty(item.supplementfeedback))
                        //try
                        //{
                        //    // Attempt to deserialize as an array
                        //    item.supplementfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.supplementfeedback);
                        //}
                        //catch (JsonSerializationException)
                        //{
                        //    // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //    var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.supplementfeedback);
                        //    item.supplementfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //}

                        if (!string.IsNullOrEmpty(item.symptomfeedback))
                        {

                            try
                            {
                                // Attempt to deserialize as an array
                                if(item.symptomfeedback == "[]")
                                {
                                    
                                }
                                else
                                {
                                    item.symptomfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.symptomfeedback);
                                }
                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.symptomfeedback);
                                item.symptomfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                            }
                        }


                        if (!string.IsNullOrEmpty(item.measurementfeedback))
                        {
                            try
                            {
                                // Attempt to deserialize as an array
                                item.measurementfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.measurementfeedback);
                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.measurementfeedback);
                                item.measurementfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                            }
                        }

                        if (!string.IsNullOrEmpty(item.moodfeedback))
                        {

                            try
                            {
                                // Attempt to deserialize as an array
                                item.moodfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.moodfeedback);
                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.moodfeedback);
                                item.moodfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                            }
                        }



                        newcollection.Add(item);
                    }



                    return new ObservableCollection<userfeedback>(newcollection);

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

        public async Task<ObservableCollection<privacypolicy>> GetAsyncPrivacyPolicy()
        {
            try
            {
                HttpClient client = new HttpClient();
                var URl = APICalls.PrivPolicy;
                HttpResponseMessage response = await client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiPrivPolicy>(data);
                ObservableCollection<privacypolicy> users = userResponse.Value;

                ObservableCollection<privacypolicy> itemstoremove = new ObservableCollection<privacypolicy>();

                foreach (var item in users)
                {
                    if(item.deleted == true)
                    {
                        itemstoremove.Add(item); 
                    }
                }
                foreach(var item in itemstoremove)
                {
                    users.Remove(item);
                }

                return new ObservableCollection<privacypolicy>(users.Take(Range.All));
            }
            catch (Exception ex)
            {
                return new ObservableCollection<privacypolicy>();
            }
        }

        public async Task<ObservableCollection<signupcode>> GetUserSignUpCodeInfo(string signupcodepassed)
        {
            try
            {
                ObservableCollection<signupcode> itemstoremove = new ObservableCollection<signupcode>();
                var userid = Helpers.Settings.SignUp;
                string urlWithQuery = $"{signupcode}?$filter=signupcodeid eq '{signupcodepassed}'";
                HttpClient client = new HttpClient();
                HttpResponseMessage responseconsent = await client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    // Add Feedback Converter
                    //  var settings = new JsonSerializerSettings();
                    //  settings.Converters.Add(new AppointmentFeedbackConverter());
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseSignUpCode>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<signupcode>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {

                        //item.moodfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.moodfeedback);

                        try
                        {
                            // Attempt to deserialize as an array
                            item.signupcodeinfolist = JsonConvert.DeserializeObject<ObservableCollection<signupcodeinformation>>(item.signupcodeinformation);

                        }
                        catch (JsonSerializationException)
                        {
                            // If the JSON is a single object, deserialize it as such and wrap it in a collection
                            var singleItem = JsonConvert.DeserializeObject<signupcodeinformation>(item.signupcodeinformation);
                            item.signupcodeinfolist = new ObservableCollection<signupcodeinformation> { singleItem };
                        }

                     

                        newcollection.Add(item);
                    }



                    return new ObservableCollection<signupcode>(newcollection);

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

        public async Task UserfeedbackUpdateSymptomData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.symptomfeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { symptomfeedback = feedbacks });
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

        public async Task UserfeedbackUpdateMeasurementData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.measurementfeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { measurementfeedback = feedbacks });
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

        public async Task UserfeedbackUpdateMoodData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwdevapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.moodfeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { moodfeedback = feedbacks });
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
        public async Task<userfeedback> InsertUserFeedback(userfeedback item)
        {
            try
            {
                HttpClient client = new HttpClient();
                var url = APICalls.UserFeedback;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<userfeedback>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, contenttt);

                if (response.IsSuccessStatusCode)
                {
                    //Nothing to return
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
                //Error Occured on Crashlog 
            }
        }

        public async Task UpdateUser(user updateduserdetails)
        {
            try
            {
                string id = updateduserdetails.userid;
                var url = $"https://pwdevapi.peoplewith.com/api/user/userid/{id}";

                var payload = new
                {
                    userpin = updateduserdetails.userpin,
                    usermigrated = updateduserdetails.usermigrated,
                    pushnotifications = updateduserdetails.pushnotifications,
                    emailnotifications = updateduserdetails.emailnotifications
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
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
                }

                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }


    }

}
