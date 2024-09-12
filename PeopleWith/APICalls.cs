//using Android.Net.Wifi.Aware;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public const string AddCrash = "https://pwdevapi.peoplewith.com/api/crashlog";
        public const string CheckSignUpCode = "https://pwdevapi.peoplewith.com/api/signupcode?$filter=signupcodeid%20eq%20";
        public const string Checksignupregquestions = "https://pwdevapi.peoplewith.com/api/question?$filter=signupcodereferral%20eq%20";
        public const string Checksignupreganswers = "https://pwdevapi.peoplewith.com/api/answer?$filter=signupcodereferral%20eq%20";
        public const string CheckConsentforsignupcode = "https://pwdevapi.peoplewith.com/api/consent?$filter=signupcodeid%20eq%20";
        public const string GetSymptoms = "https://pwdevapi.peoplewith.com/api/symptom?$select=symptomid,title";
        public const string GetMedications = "https://pwdevapi.peoplewith.com/api/medication?$select=medicationid,title";
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

        //User Allergies  
        public const string UserAllergies = "https://pwdevapi.peoplewith.com/api/userallergy";

        //Diagnosis 
        public const string Diagnosis = "https://pwdevapi.peoplewith.com/api/diagnosis";

        //UserDiagnosis 
        public const string UserDiagnosis = "https://pwdevapi.peoplewith.com/api/userdiagnosis";
        // Mood  
        public const string UserMood = "https://pwdevapi.peoplewith.com/api/usermood";

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
            catch(Exception ex)
            {
                return null;
            }
        }


        public async Task<ObservableCollection<usermeasurement>> GetUserMeasurements()
        {
            try
            {
                string userid = Preferences.Default.Get("userid", "Unknown");

                var url = "https://pwdevapi.peoplewith.com/api/usermeasurement?$filter=userid%20eq%20" +"%27" + userid + "%27";
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
            catch(Exception ex)
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
            catch(Exception ex)
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
                return null ;
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

        //Post UserDiagnosis Data  
        public async Task<ObservableCollection<userdiagnosis>>PostUserDiagnosisAsync(ObservableCollection<userdiagnosis> UserDiagnosisPassed)
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

    }
}
