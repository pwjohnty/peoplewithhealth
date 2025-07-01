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
using Microsoft.Maui.Storage;
using System.Net;
using System.Text.RegularExpressions;
//using Windows.System;
//using static Android.Gms.Common.Apis.Api;

namespace PeopleWith
{
    public class APICalls
    {
        //add the names of the api followed with the url connection
        public const string Checkuseremail = "https://pwapi.peoplewith.com/api/user?$filter=email%20eq%20";
        public const string CheckuserPassword = "https://pwapi.peoplewith.com/api/user?$filter=password%20eq%20";

        //Crash
        public const string AddCrash = "https://pwapi.peoplewith.com/api/crashlog";

        //Sign-Up Code
        public const string CheckSignUpCode = "https://pwapi.peoplewith.com/api/signupcode?$filter=signupcodeid%20eq%20";
        public const string Checksignupregquestions = "https://pwapi.peoplewith.com/api/question?$filter=signupcodereferral%20eq%20";
        public const string Checksignupreganswers = "https://pwapi.peoplewith.com/api/answer?$filter=signupcodereferral%20eq%20";
        public const string CheckConsentforsignupcode = "https://pwapi.peoplewith.com/api/consent?$filter=signupcodeid%20eq%20";
        public const string CheckConsent = "https://pwapi.peoplewith.com/api/consent";

        //User
        public const string InsertUser = "https://pwapi.peoplewith.com/api/user/";
        public const string InsertUserResponse = "https://pwapi.peoplewith.com/api/userresponse/";
        public const string InsertUserSymptoms = "https://pwapi.peoplewith.com/api/usersymptom/";
        public const string InsertUserMedications = "https://pwapi.peoplewith.com/api/usermedication/";
        public const string InsertUserDiagnosis = "https://pwapi.peoplewith.com/api/userdiagnosis/";
        public const string InsertUserMeasurements = "https://pwapi.peoplewith.com/api/usermeasurement/";
        public const string usersymptoms = "https://pwapi.peoplewith.com/api/usersymptom";
        public const string InsertUserDiet = "https://pwapi.peoplewith.com/api/userdiet/";
        //CrashLog  
        public const string CrashLog = "https://pwapi.peoplewith.com/api/crashlog";

        //Allergies  
        public const string Allergies = "https://pwapi.peoplewith.com/api/allergy";


        //Symptoms
        public const string GetSymptoms = "https://pwapi.peoplewith.com/api/symptom?$select=symptomid,title";

        //Medications 
        public const string usermedications = "https://pwapi.peoplewith.com/api/usermedication";
        public const string GetMedications = "https://pwapi.peoplewith.com/api/medication?$select=medicationid,title,status";

        //Supplements
        public const string usersupplements = "https://pwapi.peoplewith.com/api/usersupplement";
        public const string InsertUserSupplements = "https://pwapi.peoplewith.com/api/usersupplement";
        public const string GetSupplements = "https://pwapi.peoplewith.com/api/supplement?$select=supplementid,title,status";

        //User Allergies  
        public const string UserAllergies = "https://pwapi.peoplewith.com/api/userallergy";

        //Diagnosis 
        public const string Diagnosis = "https://pwapi.peoplewith.com/api/diagnosis";

        //UserDiagnosis 
        public const string UserDiagnosis = "https://pwapi.peoplewith.com/api/userdiagnosis";
        // Mood  
        public const string UserMood = "https://pwapi.peoplewith.com/api/usermood";

        //HCPS
        public const string UserHCPs = "https://pwapi.peoplewith.com/api/hcp";

        //Appointment
        public const string Appointments = "https://pwapi.peoplewith.com/api/appointment";

        //Videos 
        public const string Videos = "https://pwapi.peoplewith.com/api/video";
        //Videos engagement
        public const string VideosEngage = "https://pwapi.peoplewith.com/api/videoengagementdata";
        //User Consent 
        public const string UserConsent = "https://pwapi.peoplewith.com/api/userconsent";

        public const string UserQuestionnaires = "https://pwapi.peoplewith.com/api/userquestionnaire";

        public const string UserFeedback = "https://pwapi.peoplewith.com/api/userfeedback";

        public const string PrivPolicy = "https://pwapi.peoplewith.com/api/privacypolicy";

        public const string signupcode = "https://pwapi.peoplewith.com/api/signupcode";

        public const string postcode = "https://pwapi.peoplewith.com/api/postcode";

        //Diet 
        public const string GetDiet = "https://pwapi.peoplewith.com/api/diet";
        public const string GetUserDiet = "https://pwapi.peoplewith.com/api/userdiet";

        //Investigation 
        public const string GetInvestigation = "https://pwapi.peoplewith.com/api/investigation";
        public const string GetUserInvestigation = "https://pwapi.peoplewith.com/api/userinvestigation";

        public const string registrydatainputs = "https://pwapi.peoplewith.com/api/registryDataInputs";

        //Exercise
        public const string GetExercise = "https://pwapi.peoplewith.com/api/exercise";
        //public const string GetUserInvestigation = "https://pwapi.peoplewith.com/api/userinvestigation";

        //Actiivty 
        public const string GetActivity = "https://pwapi.peoplewith.com/api/dailyactivity";
        public const string GetUserActivity = "https://pwapi.peoplewith.com/api/userdailyactivity";


        //Fitness
        public const string GetUserFitness = "https://pwapi.peoplewith.com/api/userfitnessdata";

        //registryDataInputs
        public const string GetDashQuestionnaire = "https://pwapi.peoplewith.com/api/registryDataInputs";

        //registryData
        public const string DashQuestionAnswers = "https://pwapi.peoplewith.com/api/registryData";

        //Authentication Test
        public const string GetAuth = "https://pwapicontainer.thankfulground-b43b4106.ukwest.azurecontainerapps.io/api/registryConfig";

        public HttpClient Client = new HttpClient();
        CrashDetected crashHandler = new CrashDetected();

        async public Task NotasyncMethod(Exception Ex)
        {
            try
            {
                await crashHandler.SentryCrashDetected(Ex);
                //await Navigation.PushAsync(new ErrorPage("Dashboard"), false);
            }
            catch (Exception ex)
            {
                //Dunno 
            }
        }

        async public void IntentionalCrash()
        {
            //Intentional Crash Data (Remove) 
            //HttpResponseMessage response = await Client.GetAsync("http://10.255.255.1");
            //HttpResponseMessage response = await Client.GetAsync("http://localhost:9999");
            //HttpResponseMessage response = await Client.GetAsync("http://example.invalid");

            //Client.Timeout = TimeSpan.FromMilliseconds(1);
            //HttpResponseMessage response = await Client.GetAsync("https://google.com");
            //HttpResponseMessage response = await Client.GetAsync("https://expired.badssl.com/");
            //HttpResponseMessage response = await Client.GetAsync("https://self-signed.badssl.com/");
            //HttpResponseMessage response = await Client.GetAsync("https://httpstat.us/404");
            //HttpResponseMessage response = await Client.SendAsync(null);
        }


        private void ConfigureClient()
        {
            try
            {
                Client = new HttpClient();
                Client.DefaultRequestHeaders.Add("X-MS-CLIENT-PRINCIPAL", "eyAgCiAgImlkZW50aXR5UHJvdmlkZXIiOiAidGVzdCIsCiAgInVzZXJJZCI6ICIxMjM0NSIsCiAgInVzZXJEZXRhaWxzIjogImpvaG5AY29udG9zby5jb20iLAogICJ1c2VyUm9sZXMiOiBbIjFFMzNDMEFDLTMzOTMtNEMzNC04MzRBLURFNUZEQkNCQjNDQyJdCn0=");
                Client.DefaultRequestHeaders.Add("X-MS-API-ROLE", "1E33C0AC-3393-4C34-834A-DE5FDBCBB3CC");
            }
            catch (Exception Ex)
            {
                //Empty
            }
        }


        //Get User Details 
        public async Task<ObservableCollection<user>> GetuserDetails()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var url = $"https://pwapi.peoplewith.com/api/user/userid/{USERID}";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

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

            catch (Exception ex) when (
            ex is HttpRequestException || 
            ex is WebException ||
            ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<consent> GetConsentAsync()
        {
            try
            {
                var signupcode = Helpers.Settings.SignUp;
                string urlWithQuery = $"{CheckConsent}?$filter=signupcodeid eq '{signupcode}'";

                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string content = await responseconsent.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponseConsent>(content);
                    var consentItem = apiResponse?.Value
                        .FirstOrDefault(c => c.area.Equals("All", StringComparison.OrdinalIgnoreCase));

                    return consentItem;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is WebException ||
            ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }




        //Update User Details 


        public async Task<ObservableCollection<measurement>> GetMeasurements()
        {
            try
            {
                var url = "https://pwapi.peoplewith.com/api/measurement?$select=measurementid,measurementname,units";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

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
            catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is WebException ||
            ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<measurement> GetMeasurementsInfo(measurement Getinfo)
        {
            try
            {
                var id = Getinfo.measurementid;
                var url = $"https://pwapi.peoplewith.com/api/measurement/measurementid/{id}";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

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
            catch (Exception ex) when (
           ex is HttpRequestException ||
           ex is WebException ||
           ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<ObservableCollection<usermeasurement>> GetUserMeasurements()
        {
            try
            {
                ObservableCollection<usermeasurement> itemstoremove = new ObservableCollection<usermeasurement>();
                string userid = Preferences.Default.Get("userid", "Unknown");

                var url = "https://pwapi.peoplewith.com/api/usermeasurement?$filter=userid%20eq%20" + "%27" + userid + "%27";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserMeasurement>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var FilterMeasure = consent?.Where(item => !item.deleted).ToObservableCollection() ?? new ObservableCollection<usermeasurement>();
                    return new ObservableCollection<usermeasurement>(FilterMeasure);
                    //old
                    //foreach (var item in consent)
                    //{
                    //    if (item.deleted == true)
                    //    {
                    //        itemstoremove.Add(item);
                    //    }
                    //}
                    //foreach (var i in itemstoremove)
                    //{
                    //    consent.Remove(i);
                    //}
                    //return new ObservableCollection<usermeasurement>(consent);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<usermeasurement> InsertUsermeasurement(usermeasurement item)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.InsertUserMeasurements;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<usermeasurement>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PostAsync(url, contenttt);

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
           catch (Exception ex) when (
           ex is HttpRequestException ||
           ex is WebException ||
           ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //Delete User Measurement 

        public async Task DeleteUserMeasurements(ObservableCollection<usermeasurement> deletelistpassed)
        {
            try
            {
                for (int i = 0; i < deletelistpassed.Count; i++)
                {
                    var url = $"https://pwapi.peoplewith.com/api/usermeasurement/id/{deletelistpassed[i].id}";
                    string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = deletelistpassed[i].deleted });
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    ConfigureClient();
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Patch, url)
                        {
                            Content = content
                        };

                        var response = await Client.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is WebException ||
            ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);              
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);         
            }
        }

        public async Task DeleteSingleMeasurement(usermeasurement SingleMeasure)
        {
            try
            {
                var id = SingleMeasure.id;
                var url = $"https://pwapi.peoplewith.com/api/usermeasurement/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = SingleMeasure.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is WebException ||
            ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }

        //Update Single Measurement
        public async Task UpdateSingleMeasurement(usermeasurement SingleMeasure)
        {
            try
            {
                var id = SingleMeasure.id;
                var url = $"https://pwapi.peoplewith.com/api/usermeasurement/id/{id}";
                //Update the only three possible fields to change
                string json = System.Text.Json.JsonSerializer.Serialize(new { inputdatetime = SingleMeasure.inputdatetime, value = SingleMeasure.value, inputmethod = SingleMeasure.inputmethod });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception ex) when (
          ex is HttpRequestException ||
          ex is WebException ||
          ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //Delete User Symptom 
        public async Task DeleteSymptom(ObservableCollection<usersymptom> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwapi.peoplewith.com/api/usersymptom/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
           ex is HttpRequestException ||
           ex is WebException ||
           ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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


                ConfigureClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                //var url = $"https://pwapi.peoplewith.com/api/usersymptom/userid/{USERID}";
                string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{userid}'";
                //string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{USERID}'&$select=id,userid,symptomid,feedback,symptomtitle";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
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
            catch (Exception ex) when (
          ex is HttpRequestException ||
          ex is WebException ||
          ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usersymptom>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usersymptom>();
            }
        }

        public async Task<ObservableCollection<usersymptom>> GetUserSymptoms()
        {
            try
            {
                string userid = Preferences.Default.Get("userid", "Unknown");

                var url = "https://pwapi.peoplewith.com/api/usersymptom?$filter=userid%20eq%20" + "%27" + userid + "%27";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

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
            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        public async Task<ObservableCollection<symptom>> GetSymptomSearchAsync()
        {
            try
            {
                var url = "https://pwapi.peoplewith.com/api/symptom?$select=symptomid,title,status,classification";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(url);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiResponseSymptom>(data);
                ObservableCollection<symptom> users = userResponse.Value;
                return new ObservableCollection<symptom>(users.Take(Range.All));
            }
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        public async Task<bool> GetSymptonsInput(string symptomID)
        {
            try
            {
                //var id = CheckSymptom.symptomid;
                var id = symptomID;
                var url = $"https://pwapi.peoplewith.com/api/symptom/symptomid/{id}";
                var fullurl = $"{url}?$select=imageinput";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(fullurl);
                string data = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(data);
                bool imageInput = jsonObject["value"]?[0]?["imageinput"]?.Value<bool>() ?? false;
                return imageInput;

            }
            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return true;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return true;
            }
        }

        public async Task<bool> GetSymptonsInputbyName(string SymptomName)
        {
            try
            {
                //Search By Name (No SymptomId in UserFeedback) 
                var Name = SymptomName;
                var url = $"https://pwapi.peoplewith.com/api/symptom/";
                string urlWithQuery = $"{url}?$filter=title eq '{Name}'";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);

                string data = await response.Content.ReadAsStringAsync();

                var jsonObject = JObject.Parse(data);
                bool imageInput = jsonObject["value"]?[0]?["imageinput"]?.Value<bool>() ?? false;
                return imageInput;

            }
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return true;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return true;
            }
        }

        public async Task<usersymptom> PostSymptomAsync(usersymptom usersymptomsPassed)
        {
            try
            {
                ConfigureClient();
                var url = "https://pwapi.peoplewith.com/api/usersymptom";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usersymptom>(usersymptomsPassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
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
            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //Update Symptomfeedback Data
        public async Task PutSymptomAsync(ObservableCollection<usersymptom> Updatefeedback)
        {
            try
            {
                var id = Updatefeedback[0].id;
                var url = $"https://pwapi.peoplewith.com/api/usersymptom/id/{id}";
                var feedbacks = Updatefeedback[0].feedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                    var url = $"https://pwapi.peoplewith.com/api/usersymptom/id/{id}";
                    var feedbacks = Updatefeedback[i].feedback;
                    string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks });
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    ConfigureClient();
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Patch, url)
                        {
                            Content = content
                        };
                        var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        public async Task<ObservableCollection<interventiontrigger>> GetAsyncInterventionTrigger()
        {
            try
            {

                var URl = "https://pwapi.peoplewith.com/api/interventiontrigger?$select=title,category,type";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiInterventionTrigger>(data);
                ObservableCollection<interventiontrigger> users = userResponse.Value;
                return new ObservableCollection<interventiontrigger>(users.Take(Range.All));
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<interventiontrigger>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<interventiontrigger>();
            }
        }

        //Get All Medication Preperation
        public async Task<ObservableCollection<preparation>> GetMedPreparation()
        {
            try
            {
                var url = "https://pwapi.peoplewith.com/api/preparation/";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

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
            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //Update UserMedication in DB 
        public async Task<usermedication> PostMedicationAsync(usermedication usermedpassed)
        {
            try
            {
                ConfigureClient();
                var url = "https://pwapi.peoplewith.com/api/usermedication";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usermedication>(usermedpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                // Choose POST or PATCH based on whether the ID is null or empty
                if (string.IsNullOrEmpty(usermedpassed.id))
                {
                    response = await Client.PostAsync(url, contenttts);
                }
                else
                {
                    response = await Client.PatchAsync(url, contenttts);
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
            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<usersupplement> PostSupplementAsync(usersupplement usermedpassed)
        {
            try
            {
                ConfigureClient();
                var url = "https://pwapi.peoplewith.com/api/usersupplement";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usersupplement>(usermedpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                // Choose POST or PATCH based on whether the ID is null or empty
                if (string.IsNullOrEmpty(usermedpassed.id))
                {
                    response = await Client.PostAsync(url, contenttts);
                }
                else
                {
                    response = await Client.PatchAsync(url, contenttts);
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
            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        public async Task UpdateEditMedication(usermedication usermedpassed)
        {
            try
            {
                // HttpClient client = new HttpClient();
                var id = usermedpassed.id;
                var url = $"https://pwapi.peoplewith.com/api/usermedication/{id}";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usermedication>(usermedpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                StringContent content = new StringContent(jsonns, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var responsee = await Client.SendAsync(request);
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
            catch (Exception ex) when (
          ex is HttpRequestException ||
          ex is WebException ||
          ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                //var url = $"https://pwapi.peoplewith.com/api/usermedication/userid/{USERID}";
                string urlWithQuery = $"{usermedications}?$filter=userid eq '{userid}'";
                //string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{USERID}'&$select=id,userid,symptomid,feedback,symptomtitle";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
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
                            medicationquestions = rawSymptom.medicationquestions,
                            groupscheduleid = rawSymptom.groupscheduleid
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
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usermedication>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usermedication>();
            }
        }

        //Delete Usermedication 
        public async Task DeleteMedication(usermedication Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/usermedication/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        public async Task DeletePendingMedications(ObservableCollection<usermedication> deletelistpassed)
        {
            try
            {
                for (int i = 0; i < deletelistpassed.Count; i++)
                {
                    var url = $"https://pwapi.peoplewith.com/api/usermedication/id/{deletelistpassed[i].id}";
                    string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = deletelistpassed[i].deleted });
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    ConfigureClient();
                    using (var client = new HttpClient())
                    {
                        var request = new HttpRequestMessage(HttpMethod.Patch, url)
                        {
                            Content = content
                        };

                        var response = await Client.SendAsync(request);

                        if (!response.IsSuccessStatusCode)
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        public async Task DeleteSupplement(usersupplement Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/usersupplement/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        //Update Medicaiton Details 
        public async Task UpdateMedicationDetails(usermedication Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/usermedication/id/{id}";

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
                    medicationquestions = Updatefeedback.medicationquestions,
                    groupscheduleid = Updatefeedback.groupscheduleid
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        public async Task UpdateSupplementDetails(usersupplement Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/usersupplement/id/{id}";

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
                    supplementquestions = Updatefeedback.supplementquestions,
                    //groupscheduleid = Updatefeedback.groupscheduleid
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        public async Task UpdateCompletedMedication(usermedication updatedmed)
        {
            try
            {
                string id = updatedmed.id;
                var url = $"https://pwapi.peoplewith.com/api/usermedication/id/{id}";

                var payload = new
                {
                    status = updatedmed.status
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        public async Task UpdateCompletedSupplement(usersupplement updatedmed)
        {
            try
            {
                string id = updatedmed.id;
                var url = $"https://pwapi.peoplewith.com/api/usersupplement/id/{id}";

                var payload = new
                {
                    status = updatedmed.status

                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        //Update Medicaiton schedule unit 
        //public async Task UpdateScheduleUnit(usermedication Updatefeedback)
        //{
        //    try
        //    {
        //        string id = Updatefeedback.id;
        //        var url = $"https://pwapi.peoplewith.com/api/usermedication/id/{id}";

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
                var url = $"https://pwapi.peoplewith.com/api/usermedication/id/{id}";
                var feedbacks = Updatefeedback.feedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }

        public async Task UpdateSupplementFeedbackAsync(usersupplement Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/usersupplement/id/{id}";
                var feedbacks = Updatefeedback.feedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }



        //supplments
        public async Task<ObservableCollection<usersupplement>> GetUserSupplementsAsync()
        {
            try
            {
                ConfigureClient();
                string userid = Preferences.Default.Get("userid", "Unknown");
                //var url = $"https://pwapi.peoplewith.com/api/usersupplement/userid/{USERID}";
                string urlWithQuery = $"{usersupplements}?$filter=userid eq '{userid}'";
                //string urlWithQuery = $"{usersymptoms}?$filter=userid eq '{USERID}'&$select=id,userid,symptomid,feedback,symptomtitle";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
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
                            supplementquestions = rawSymptom.supplementquestions,
                            groupscheduleid = rawSymptom.groupscheduleid
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

                            //Stops issue of schedule causing crash on As Required
                            if (!newUserSymptom.frequency.Contains("As Required"))
                            {
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
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usersupplement>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usersupplement>();
            }
        }



        //Add Crashlog Item 
        public async Task<crashlog> InsertCrashLog(crashlog item)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.CrashLog;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<crashlog>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PostAsync(url, contenttt);

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
            catch (Exception ex) when (
     ex is HttpRequestException ||
     ex is WebException ||
     ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
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
                ConfigureClient();
                var URl = APICalls.Allergies;
                HttpResponseMessage response = await Client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiAllergies>(data);
                ObservableCollection<allergies> users = userResponse.Value;
                return new ObservableCollection<allergies>(users.Take(Range.All));
            }
            catch (Exception ex) when (
   ex is HttpRequestException ||
   ex is WebException ||
   ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<allergies>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<allergies>();
            }
        }


        //Get Single Allergy Informaiton 
        public async Task<allergies> GetAsyncSingleAllergy(allergies GetInfo)
        {
            try
            {
                var id = GetInfo.Allergyid;
                ConfigureClient();

                var URl = $"https://pwapi.peoplewith.com/api/allergy/allergyid/{id}";

                HttpResponseMessage response = await Client.GetAsync(URl);

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
            catch (Exception ex) when (
 ex is HttpRequestException ||
 ex is WebException ||
 ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }



        //Post UserAllergies Data  
        public async Task<ObservableCollection<userallergies>> PostUserAllergiesAsync(ObservableCollection<userallergies> AllergyPassed)
        {
            try
            {
                ConfigureClient();
                var urls = APICalls.UserAllergies;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userallergies>(AllergyPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(urls, contenttts);
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
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userallergies>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                string urlWithQuery = $"{UserAllergies}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
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
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userallergies>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userallergies>();
            }
        }


        //Delete UserAllergy  

        public async Task DeleteUserAllergy(ObservableCollection<userallergies> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwapi.peoplewith.com/api/userallergy/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
            ex is HttpRequestException ||
            ex is WebException ||
            ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                var URl = "https://pwapi.peoplewith.com/api/diagnosis";
                HttpResponseMessage response = await Client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiDiagnosis>(data);
                ObservableCollection<diagnosis> users = userResponse.Value;
                return new ObservableCollection<diagnosis>(users.Take(Range.All));
            }
            catch (Exception ex) when (
           ex is HttpRequestException ||
           ex is WebException ||
           ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<diagnosis>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();

                // Use string id interpolation to insert Diagid
                var URl = $"https://pwapi.peoplewith.com/api/diagnosis/diagnosisid/{id}";

                HttpResponseMessage response = await Client.GetAsync(URl);

                if (response.IsSuccessStatusCode)
                {
                    string contentconsent = await response.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiDiagnosis>(contentconsent);
                    var consent = userResponseconsent.Value;
                    GetInfo.Title = consent[0].Title;
                    return GetInfo;

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex) when (
          ex is HttpRequestException ||
          ex is WebException ||
          ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }



        //Post UserDiagnosis Data  
        public async Task<ObservableCollection<userdiagnosis>> PostUserDiagnosisAsync(ObservableCollection<userdiagnosis> UserDiagnosisPassed)
        {
            try
            {
                ConfigureClient();
                var urls = APICalls.UserDiagnosis;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userdiagnosis>(UserDiagnosisPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(urls, contenttts);
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
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userdiagnosis>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                string urlWithQuery = $"{UserDiagnosis}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<GetUserDiagnosis>(data);
                ObservableCollection<userdiagnosis> users = userResponse.Value;
                return new ObservableCollection<userdiagnosis>(users.Take(Range.All));
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userdiagnosis>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userdiagnosis>();
            }
        }


        //Delete UserDiagnosis  

        public async Task DeleteDiagnosis(ObservableCollection<userdiagnosis> Updatefeedback)
        {
            try
            {
                string id = Updatefeedback[0].id;
                var url = $"https://pwapi.peoplewith.com/api/userdiagnosis/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback[0].deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        //Update Diagnosis Date 
        public async Task PutDiagnosisAsync(ObservableCollection<userdiagnosis> Updatefeedback)
        {
            try
            {

                var id = Updatefeedback[0].id;
                var url = $"https://pwapi.peoplewith.com/api/userdiagnosis/id/{id}";
                var feedbacks = Updatefeedback[0].dateofdiagnosis;

                string json = System.Text.Json.JsonSerializer.Serialize(new { dateofdiagnosis = feedbacks });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

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

            catch (Exception ex) when (
    ex is HttpRequestException ||
    ex is WebException ||
    ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                string urlWithQuery = $"{UserMood}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
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
                var FilterMood = users?.Where(item => !item.deleted).ToList() ?? new List<usermood>();
                return new ObservableCollection<usermood>(FilterMood);
            }
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usermood>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usermood>();
            }
        }


        //Post UserMood Data 
        public async Task<ObservableCollection<usermood>> PostUserMoodAsync(ObservableCollection<usermood> MoodPassed)
        {
            try
            {
                ConfigureClient();
                var urls = APICalls.UserMood;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<usermood>(MoodPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(urls, contenttts);
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
           catch (Exception ex) when (
           ex is HttpRequestException ||
           ex is WebException ||
           ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usermood>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<usermood>();
            }
        }



        //Delete UserMood Data 

        public async Task DeleteUserMood(usermood Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/usermood/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
          ex is HttpRequestException ||
          ex is WebException ||
          ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }



        //Update User Mood 
        public async Task PutMoodAsync(ObservableCollection<usermood> Updatefeedback)
        {
            try
            {

                var id = Updatefeedback[0].id;
                var url = $"https://pwapi.peoplewith.com/api/usermood/id/{id}";
                var feedbacks = Updatefeedback[0];

                //Change the following   
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    title = feedbacks.title,
                    datetime = feedbacks.datetime,
                    notes = feedbacks.notes
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

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

            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

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

            catch (Exception ex) when (
         ex is HttpRequestException ||
         ex is WebException ||
         ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //Add User HCP's
        public async Task<ObservableCollection<hcp>> PostUserHCPAsync(ObservableCollection<hcp> HCPPassed)
        {
            try
            {
                ConfigureClient();
                var urls = APICalls.UserHCPs;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<hcp>(HCPPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(urls, contenttts);
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
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<hcp>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<hcp>();
            }
        }

        //Update HCP Exisiting Item 
        public async Task UpdateHCPItem(hcp Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.hcpid;
                var url = $"https://pwapi.peoplewith.com/api/hcp/hcpid/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(Updatefeedback);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }




        //Delete User HCP
        public async Task DeleteUserHCP(hcp Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.hcpid;
                var url = $"https://pwapi.peoplewith.com/api/hcp/hcpid/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

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
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //Update User Appointment

        public async Task UpdateAppointmentItem(appointment Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/appointment/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(Updatefeedback);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
        ex is HttpRequestException ||
        ex is WebException ||
        ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        //Delete User Appointment 

        public async Task DeleteUserAppointment(appointment Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/appointment/id/{id}";
                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }
                return;
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        //Add User Appointment
        public async Task<ObservableCollection<appointment>> PostUserAppointmentAsync(ObservableCollection<appointment> AppointmentPassed)
        {
            try
            {
                ConfigureClient();
                var urls = APICalls.Appointments;

                string jsonns = System.Text.Json.JsonSerializer.Serialize<appointment>(AppointmentPassed[0]);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                HttpResponseMessage response;

                // Choose POST or PATCH based on whether the ID is null or empty
                if (string.IsNullOrEmpty(AppointmentPassed[0].id))
                {
                    response = await Client.PostAsync(urls, contenttts);
                }
                else
                {
                    string id = AppointmentPassed[0].id;
                    var url = $"https://pwapi.peoplewith.com/api/appointment/id/{id}";

                    //string json = System.Text.Json.JsonSerializer.Serialize<appointment>(AppointmentPassed[0]);
                    //StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    response = await Client.PatchAsync(url, contenttts);

                    //using (var clients = new HttpClient())
                    //{
                    //    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    //    {
                    //        Content = content
                    //    };

                    //    var responsse = await client.SendAsync(request);

                    //    if (!responsse.IsSuccessStatusCode)
                    //    {
                    //        var errorResponsse = await responsse.Content.ReadAsStringAsync();
                    //    }
                    //}
                    //return new ObservableCollection<appointment>(AppointmentPassed);
                }


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
            catch (Exception ex) when (
     ex is HttpRequestException ||
     ex is WebException ||
     ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<appointment>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<appointment>();
            }
        }



        //Get All User Videos
        public async Task<ObservableCollection<videos>> GetAllVideos(string ReturnType)
        {
            try
            {

                ObservableCollection<videos> itemstoremove = new ObservableCollection<videos>();
                var urls = APICalls.Videos;
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urls);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseVideos>(contentconsent);
                    var consent = userResponseconsent.Value;

                    //Remove All Deleted Items 
                    var signupid = Helpers.Settings.SignUp;
                    foreach (var item in consent)
                    {
                        //Return All items Including SignupCode 
                        if (ReturnType == "All")
                        {
                            if (item.deleted == true)
                            {
                                itemstoremove.Add(item);
                            }
                        }
                        //Just Return SignupCode Items 
                        else
                        {
                            bool Check = (!string.IsNullOrEmpty(item.referral) && item.referral.Contains(signupid)) ||
                                (!string.IsNullOrEmpty(item.signupcodeid) && item.signupcodeid.Contains(signupid));

                            if (item.deleted == true)
                            {
                                itemstoremove.Add(item);
                            }
                            if (!Check)
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
            catch (Exception ex) when (
    ex is HttpRequestException ||
    ex is WebException ||
    ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<ObservableCollection<videos>> GetAllHelpVideos()
        {
            try
            {
                ObservableCollection<videos> itemstoremove = new ObservableCollection<videos>();
                var urls = APICalls.Videos;
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urls);

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
                        else if (!string.IsNullOrEmpty(item.referral))
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
            catch (Exception ex) when (
    ex is HttpRequestException ||
    ex is WebException ||
    ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //public async Task<ObservableCollection<videos>> GetAllVideoswithsignupcode()
        //{
        //    try
        //    {
        //        ObservableCollection<videos> itemstoremove = new ObservableCollection<videos>();
        //        var urls = APICalls.Videos;
        //        ConfigureClient();
        //        HttpResponseMessage responseconsent = await Client.GetAsync(urls);
        //        //var urls = APICalls.Videos;
        //        // var signupcode = Helpers.Settings.SignUp;
        //        //var signupcode = Helpers.Settings.SignUp;
        //        //string urlWithQuery = $"{Videos}?$filter=referral eq '{signupcode}'";
        //        //string encodedSignupCode = Uri.EscapeDataString(signupcode);
        //        //public const string Videos = "https://pwapi.peoplewith.com/api/video";
        //        //string urlWithQuery = $"{Videos}?$filter=contains(referral, '{signupcode}') or contains(signupcodeid, '{signupcode}')";
        //        //string urlWithQuery = $"{Videos}?$filter=referral eq '{encodedSignupCode}' or contains(signupcodeid, '{encodedSignupCode}')";
        //        //string urlWithQuery = $"{Videos}?$filter=referral eq '{signupcode}' and contains(signupcodeid, '{signupcode}')";
        //        //ConfigureClient();
        //        //HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

        //        if (responseconsent.IsSuccessStatusCode)
        //        {
        //            string contentconsent = await responseconsent.Content.ReadAsStringAsync();
        //            var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseVideos>(contentconsent);
        //            var consent = userResponseconsent.Value;
        //            var signupid = Helpers.Settings.SignUp; 
        //            //Remove All Deleted Items 
        //            foreach (var item in consent)
        //            {
        //                bool Check = (!string.IsNullOrEmpty(item.referral) && item.referral.Contains(signupid)) ||
        //                    (!string.IsNullOrEmpty(item.signupcodeid) && item.signupcodeid.Contains(signupid));

        //                if (item.deleted == true)
        //                {
        //                    itemstoremove.Add(item);
        //                }
        //                if (!Check)
        //                {
        //                    itemstoremove.Add(item);
        //                }

        //                item.dateandlength = item.dateadded + " " + "Length: " + item.lenght;
        //                item.thumbnail = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.thumbnail;
        //                item.filename = "https://peoplewithappiamges.blob.core.windows.net/appimages/appimages/" + item.filename;

        //                if (item.subtitle.Length > 122)
        //                {
        //                    var SubString = item.subtitle.Substring(0, 122) + "...";
        //                    item.subtitleshort = SubString;
        //                }
        //                else
        //                {
        //                    item.subtitleshort = item.subtitle;
        //                }
        //            }

        //            foreach (var item in itemstoremove)
        //            {
        //                consent.Remove(item);
        //            }

        //            return new ObservableCollection<videos>(consent);

        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //Update Video Engagement 
        public async Task<videoengage> PostEngagementAsync(videoengage PassedEngagement)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.VideosEngage;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<videoengage>(PassedEngagement);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
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
            catch (Exception ex) when (
    ex is HttpRequestException ||
    ex is WebException ||
    ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //User Consent Post Data  

        public async Task<userconsent> PostUserConsentAsync(userconsent ConsentPassed)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.UserConsent;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userconsent>(ConsentPassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
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
            catch (Exception ex) when (
    ex is HttpRequestException ||
    ex is WebException ||
    ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //questionnaire

        public async Task<ObservableCollection<questionnaire>> GetQuestionnaires()
        {
            try
            {
                var url = "https://pwapi.peoplewith.com/api/questionnaire/";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(url);

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
                            else if (string.IsNullOrEmpty(Helpers.Settings.SignUp))
                            {
                                //ignore it
                            }
                            else if (item.signupcodeid.Contains(usersignupcode))
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
            catch (Exception ex) when (
    ex is HttpRequestException ||
    ex is WebException ||
    ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }




        public async Task<ObservableCollection<questionnaire>> GetSingleQuestionnaire(string questionnaireid)
        {
            try
            {
                var id = questionnaireid;
                var url = "https://pwapi.peoplewith.com/api/questionnaire/";
                //HttpClient client = new HttpClient();
                //HttpResponseMessage responseconsent = await client.GetAsync(url);

                string urlWithQuery = $"{url}?$filter=questionnaireid eq '{id}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

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
                            else if (string.IsNullOrEmpty(Helpers.Settings.SignUp))
                            {
                                //ignore it
                            }
                            else if (item.signupcodeid.Contains(usersignupcode))
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
            catch (Exception ex) when (
     ex is HttpRequestException ||
     ex is WebException ||
     ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<userquestionnaire> PostUserQuestionnaire(userquestionnaire userquestionnairepassed)
        {
            try
            {
                ConfigureClient();
                var url = "https://pwapi.peoplewith.com/api/userquestionnaire";
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userquestionnaire>(userquestionnairepassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
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
            catch (Exception ex) when (
  ex is HttpRequestException ||
  ex is WebException ||
  ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

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
                            continue;
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
            catch (Exception ex) when (
  ex is HttpRequestException ||
  ex is WebException ||
  ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
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
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

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
                        //{x 
                        //    // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //    var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.supplementfeedback);
                        //    item.supplementfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //}

                        if (!string.IsNullOrEmpty(item.symptomfeedback))
                        {

                            try
                            {
                                // Attempt to deserialize as an array
                                if (item.symptomfeedback == "[]")
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
                                if (item.measurementfeedback == "[]")
                                {

                                }
                                else
                                {
                                    // Attempt to deserialize as an array
                                    item.measurementfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.measurementfeedback);
                                }

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
                                if (item.moodfeedback == "[]")
                                {

                                }
                                else
                                {
                                    // Attempt to deserialize as an array
                                    item.moodfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.moodfeedback);
                                }
                                // Attempt to deserialize as an array

                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.moodfeedback);
                                item.moodfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                            }
                        }

                        if (!string.IsNullOrEmpty(item.initialquestionnairefeedback))
                        {

                            try
                            {
                                // Attempt to deserialize as an array
                                if (item.initialquestionnairefeedback == "[]")
                                {

                                }
                                else
                                {
                                    // Attempt to deserialize as an array
                                    item.initialquestionnairefeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.initialquestionnairefeedback);
                                }

                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.initialquestionnairefeedback);
                                item.initialquestionnairefeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
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
            catch (Exception ex) when (
   ex is HttpRequestException ||
   ex is WebException ||
   ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        public async Task<ObservableCollection<privacypolicy>> GetAsyncPrivacyPolicy()
        {
            try
            {
                ConfigureClient();
                var URl = APICalls.PrivPolicy;
                HttpResponseMessage response = await Client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiPrivPolicy>(data);
                ObservableCollection<privacypolicy> users = userResponse.Value;

                ObservableCollection<privacypolicy> itemstoremove = new ObservableCollection<privacypolicy>();

                foreach (var item in users)
                {
                    if (item.deleted == true)
                    {
                        itemstoremove.Add(item);
                    }
                }
                foreach (var item in itemstoremove)
                {
                    users.Remove(item);
                }

                return new ObservableCollection<privacypolicy>(users.Take(Range.All));
            }
            catch (Exception ex) when (
  ex is HttpRequestException ||
  ex is WebException ||
  ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<privacypolicy>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<privacypolicy>();
            }
        }

        public async Task<ObservableCollection<signupcode>> GetUserSignUpCodeInfo(string signupcodepassed)
        {
            try
            {
                //Ensure SignupCode Is not Null When Passed
                if (String.IsNullOrEmpty(signupcodepassed)) { signupcodepassed = Helpers.Settings.SignUp; }
                var SignupPathway = "https://pwapi.peoplewith.com/api/signupcode";
                string urlWithQuery = $"{SignupPathway}?$filter=signupcodeid eq '{signupcodepassed}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);
                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseSignUpCode>(contentconsent);
                    var consent = userResponseconsent.Value;
                    var newcollection = new ObservableCollection<signupcode>();
                    foreach (var item in consent)
                    {
                        try
                        {
                            if (!string.IsNullOrWhiteSpace(item.signupcodeinformation))
                            {
                                item.signupcodeinfolist = JsonConvert.DeserializeObject<ObservableCollection<signupcodeinformation>>(item.signupcodeinformation);
                            }
                            else
                            {
                                item.signupcodeinfolist = new ObservableCollection<signupcodeinformation>();
                            }
                        }
                        catch (JsonSerializationException)
                        {
                            var singleItem = JsonConvert.DeserializeObject<signupcodeinformation>(item.signupcodeinformation);
                            item.signupcodeinfolist = new ObservableCollection<signupcodeinformation> { singleItem };
                        }
                        newcollection.Add(item);

                    }
                    return new ObservableCollection<signupcode>(newcollection);
                }
                else
                {
                    return new ObservableCollection<signupcode>();
                }
            }
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<signupcode>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<signupcode>();
            }
        }
        public async Task UserfeedbackUpdateSymptomData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.symptomfeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { symptomfeedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }

        public async Task UserfeedbackUpdateMeasurementData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.measurementfeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { measurementfeedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
                ex is HttpRequestException ||
                ex is WebException ||
                ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }

        public async Task UserfeedbackUpdateMoodData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.moodfeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { moodfeedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }

        public async Task UserfeedbackUpdateQuestionnaireData(userfeedback Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userfeedback/id/{id}";
                var feedbacks = Updatefeedback.initialquestionnairefeedback;
                string json = System.Text.Json.JsonSerializer.Serialize(new { initialquestionnairefeedback = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
               ex is HttpRequestException ||
               ex is WebException ||
               ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }
        public async Task<userfeedback> InsertUserFeedback(userfeedback item)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.UserFeedback;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<userfeedback>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PostAsync(url, contenttt);

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
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        public async Task UpdateUser(user updateduserdetails)
        {
            try
            {
                string id = updateduserdetails.userid;
                var url = $"https://pwapi.peoplewith.com/api/user/userid/{id}";

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
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
             ex is HttpRequestException ||
             ex is WebException ||
             ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }

        public async Task<ObservableCollection<postcode>> GetAsyncPostcode()
        {
            try
            {
                ConfigureClient();
                var URl = APICalls.postcode;
                HttpResponseMessage response = await Client.GetAsync(URl);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiResponsePostcode>(data);
                ObservableCollection<postcode> users = userResponse.Value;
                return new ObservableCollection<postcode>(users.Take(Range.All));
            }
            catch (Exception ex) when (
           ex is HttpRequestException ||
           ex is WebException ||
           ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<postcode>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<postcode>();
            }
        }

        public async Task<ObservableCollection<registryDataInputs>> GetWHfromregistrydatainput(string signupcode)
        {
            try
            {


                ObservableCollection<registryDataInputs> itemstoremove = new ObservableCollection<registryDataInputs>();
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{registrydatainputs}?$filter=dataInputs eq '{signupcode}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseregistyDataInputs>(contentconsent);
                    var consent = userResponseconsent.Value;

                    // var questionAnswers = JsonConvert.DeserializeObject<ObservableCollection<questionanswerinfo>>();
                    var newcollection = new ObservableCollection<questionnaire>();

                    //Remove All Deleted Items 
                    //foreach (var item in consent)
                    //{
                    //    if (item.deleted == true)
                    //    {
                    //        itemstoremove.Add(item);
                    //    }
                    //}

                    //foreach (var item in itemstoremove)
                    //{
                    //    consent.Remove(item);
                    //}

                    return new ObservableCollection<registryDataInputs>(consent);

                }
                else
                {
                    string errorcontent = await responseconsent.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
            }
            catch (Exception ex) when (
          ex is HttpRequestException ||
          ex is WebException ||
          ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<registryDataInputs>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<registryDataInputs>();
            }
        }


        public class GetRegInputs
        {
            public ObservableCollection<registryDataInputs> Value { get; set; }
        }


        public async Task<ObservableCollection<userresponse>> GetUserResponses(string USERID)
        {
            try
            {
                ConfigureClient();
                string urlWithQuery = $"{InsertUserResponse}?$filter=userid eq '{USERID}'";
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiResponseUserResponse>(data);
                ObservableCollection<userresponse> users = userResponse.Value;
                foreach (var item in userResponse.Value)
                {


                }

                return new ObservableCollection<userresponse>(users.Take(Range.All));
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userresponse>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userresponse>();
            }
        }




        //Get Diet Information

        public async Task<ObservableCollection<diet>> GetDietDetails()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var URl = APICalls.GetDiet;
                //string urlWithQuery = $"{GetDiet}?$filter=userid eq '{USERID}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(URl);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APIDietResponse>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<diet>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {

                        }
                        else
                        {
                            var GetString = item.grouping;
                            if (!String.IsNullOrEmpty(GetString))
                            {
                                if (GetString.Contains("Diets"))
                                {
                                    GetString = GetString.Replace(" Diets", "");
                                }

                                if (GetString.Contains("and"))
                                {
                                    GetString = GetString.Replace(" and ", "/");
                                }

                                if (GetString.Contains("Nutrition"))
                                {
                                    GetString = GetString.Replace(" Nutrition", "");
                                }

                                item.ShortGroup = GetString;
                            }
                            newcollection.Add(item);
                        }
                    }

                    return new ObservableCollection<diet>(newcollection);

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<ObservableCollection<userdiet>> GetUserDietAsync()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var URl = APICalls.GetUserDiet;
                string urlWithQuery = $"{URl}?$filter=userid eq '{USERID}'";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();

                // Deserialize the response into a generic structure
                var rawResponse = JsonConvert.DeserializeObject<SingleUserDiet>(data);
                var UserDietList = new List<userdiet>();

                if (rawResponse?.Value != null)
                {
                    foreach (var rawUserdiet in rawResponse.Value)
                    {
                        var NewUserDiet = new userdiet
                        {
                            id = rawUserdiet.id,
                            createdAt = rawUserdiet.createdAt,
                            userid = rawUserdiet.userid,
                            dietid = rawUserdiet.dietid,
                            diettitle = rawUserdiet.diettitle,
                            deleted = rawUserdiet.deleted,
                            datestarted = rawUserdiet.datestarted,
                            dateended = rawUserdiet.dateended,
                            notes = new ObservableCollection<userNotesFeedback>()
                        };

                        // Deserialize notes only if not null or empty
                        if (!string.IsNullOrEmpty(rawUserdiet.notes))
                        {
                            var feedbackUserDiet = JsonConvert.DeserializeObject<List<userNotesFeedback>>(rawUserdiet.notes);

                            if (feedbackUserDiet != null)
                            {
                                foreach (var feedback in feedbackUserDiet)
                                {
                                    if (feedback.deleted != "deleted")
                                    {
                                        NewUserDiet.notes.Add(feedback);
                                    }
                                }
                            }
                        }

                        // Always add NewUserDiet, even if notes are null or empty
                        UserDietList.Add(NewUserDiet);
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return new ObservableCollection<userdiet>();
                }

                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<APIUserDietResponse>(content);

                var filteredDiets = UserDietList?.Where(item => !item.deleted).ToList() ?? new List<userdiet>();

                return new ObservableCollection<userdiet>(filteredDiets);

            }
            catch (Exception ex) when (
   ex is HttpRequestException ||
   ex is WebException ||
   ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userdiet>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userdiet>();
            }
        }


        public async Task<userdiet> PostUserDiet(userdiet userdietpassed)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.GetUserDiet;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userdiet>(userdietpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    userdietpassed.id = id;
                    // Return the inserted item
                    return userdietpassed;

                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
                // return null;
            }
            catch (Exception ex) when (
  ex is HttpRequestException ||
  ex is WebException ||
  ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //Add Diet Notes 

        public async Task AddDietNotesAsymc(userdiet Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userdiet/id/{id}";
                var feedbacks = Updatefeedback.notes;
                string json = System.Text.Json.JsonSerializer.Serialize(new { notes = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //Update User Diet
        public async Task UpdateUserDiet(userdiet Updatefeedback)
        {
            try
            {

                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userdiet/id/{id}";
                var feedbacks = Updatefeedback;

                //Change the following   
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    datestarted = feedbacks.datestarted,
                    dateended = feedbacks.dateended,
                    notes = feedbacks.notes
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine("Successfully updated userDiet");
                    }
                }
            }
            catch (Exception ex) when (
      ex is HttpRequestException ||
      ex is WebException ||
      ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //Delete UserDiet  
        public async Task DeleteUserDiet(userdiet Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userdiet/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //Get Investigation Information
        public async Task<ObservableCollection<investigation>> GetInvestigationDetails()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var URl = APICalls.GetInvestigation;
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(URl);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APInvestigationResponse>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<investigation>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {

                        }
                        else
                        {
                            //Shorten Filter Tabs for long winded items
                            var GetString = item.grouping;
                            if (!String.IsNullOrEmpty(GetString))
                            {

                                if (GetString.Contains("and"))
                                {
                                    GetString = GetString.Replace(" and ", "/");
                                }

                                item.ShortGroup = GetString;
                            }
                            else
                            {
                                item.ShortGroup = "Other";
                            }
                            newcollection.Add(item);
                        }
                    }

                    return new ObservableCollection<investigation>(newcollection);

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //GetUserInvestigation  
        public async Task<ObservableCollection<userinvestigation>> GetUserInvestigationAsync()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var URl = APICalls.GetUserInvestigation;
                string urlWithQuery = $"{URl}?$filter=userid eq '{USERID}'";
                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);
                string data = await response.Content.ReadAsStringAsync();

                // Deserialize the response into a generic structure
                var rawResponse = JsonConvert.DeserializeObject<SingleUserINvestigation>(data);
                var UserInvestList = new List<userinvestigation>();

                if (rawResponse?.Value != null)
                {
                    foreach (var rawUserInvest in rawResponse.Value)
                    {
                        var NewUserInvest = new userinvestigation
                        {
                            id = rawUserInvest.id,
                            createdAt = rawUserInvest.createdAt,
                            userid = rawUserInvest.userid,
                            deleted = rawUserInvest.deleted,
                            investigationid = rawUserInvest.investigationid,
                            investigationname = rawUserInvest.investigationname,
                            value = rawUserInvest.value,
                            status = rawUserInvest.status,
                            investigationdate = rawUserInvest.investigationdate,
                            investigationdocument = rawUserInvest.investigationdocument,
                            investigationimage = rawUserInvest.investigationimage,
                            notes = new ObservableCollection<notesuserfeedback>()
                        };

                        // Deserialize notes only if not null or empty
                        if (!string.IsNullOrEmpty(rawUserInvest.notes))
                        {
                            var feedbackUserInvest = JsonConvert.DeserializeObject<List<notesuserfeedback>>(rawUserInvest.notes);

                            if (feedbackUserInvest != null)
                            {
                                foreach (var feedback in feedbackUserInvest)
                                {
                                    if (feedback.deleted != "deleted")
                                    {
                                        NewUserInvest.notes.Add(feedback);
                                    }
                                }
                            }
                        }

                        // Always add NewUserDiet, even if notes are null or empty
                        UserInvestList.Add(NewUserInvest);
                    }
                }

                if (!response.IsSuccessStatusCode)
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return new ObservableCollection<userinvestigation>();
                }

                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<APIUserInvestigationResponse>(content);

                var filteredDiets = UserInvestList?.Where(item => !item.deleted).ToList() ?? new List<userinvestigation>();

                foreach (var item in filteredDiets)
                {
                    if (!string.IsNullOrEmpty(item.investigationdate))
                    {
                        var splititem = item.investigationdate.Split(' ');
                        item.datewotime = splititem[0];
                    }
                }

                return new ObservableCollection<userinvestigation>(filteredDiets);

            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userinvestigation>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<userinvestigation>();
            }
        }


        //Post UserInvestigation
        public async Task<userinvestigation> PostUserInvestigation(userinvestigation userInvestpassed)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.GetUserInvestigation;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userinvestigation>(userInvestpassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    userInvestpassed.id = id;
                    // Return the inserted item
                    return userInvestpassed;

                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
                // return null;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //Add Investigation Notes 
        public async Task AddInvestigationNotesAsync(userinvestigation Updatefeedback)
        {
            try
            {
                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userinvestigation/id/{id}";
                var feedbacks = Updatefeedback.notes;
                string json = System.Text.Json.JsonSerializer.Serialize(new { notes = feedbacks });
                //string json = System.Text.Json.JsonSerializer.Serialize(new { feedback = feedbacks }, serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    //works with patch
                    //var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };
                    var response = await Client.SendAsync(request);
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
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //Update User Investigation
        public async Task UpdateUserInvestigation(userinvestigation Updatefeedback)
        {
            try
            {

                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userinvestigation/id/{id}";
                var feedbacks = Updatefeedback;

                //Change the following  (To be defined)  
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    investigationdate = feedbacks.investigationdate,
                    notes = feedbacks.notes
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine("Successfully updated userDiet");
                    }
                }
            }

            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }

        //Delete UserInvestigation 
        public async Task DeleteUserInvestigation(userinvestigation Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userinvestigation/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }


        // Get Daily Activity information
        public async Task<ObservableCollection<dailyactivity>> GetActivityDetails()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var URl = APICalls.GetActivity;
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(URl);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APIActivityResponse>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<dailyactivity>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted) continue; // Skip deleted items immediately

                        string getString = item.grouping?.Trim() ?? string.Empty;

                        var replacements = new Dictionary<string, string>
                        {
                            { "Combat & Martial Arts", "Martial Arts" },
                            { "Team Sports", "Team Sports" },
                            { "Swimming", "Swimming" },
                            { "Endurance & Cardio Activities", "Endurance" },
                            { "Outdoor & Adventure", "Outdoor" },
                            { "Individual Sports", "Individual Sports" },
                            { "Additional Functional & Cognitive Assessments", "Cognitive" },
                            { "Mind, Body & Flexibility", "MindBody" },
                            { "Instrumental Activities of Daily Living (IADLs)", "Daily Living" },
                            { "Strength & Resistance Training", "Strength Training" },
                            { "Basic Activities of Daily Living (ADLs)", "Basic Activities" },
                            { "Motor & Extreme Sports", "Extreme Sports" },
                            { "Winter Sports", "Winter Sports" },
                            { "Equestrian", "Equestrian" },
                            { "Recreational & Alternative", "Recreational" },
                            { "Open Water Sports", "Water Sports" }
                        };

                        if (replacements.TryGetValue(getString, out string newString))
                        {
                            item.ShortGroup = newString;
                            item.Source = newString.Replace(" ", "").ToLower() + ".png";
                        }
                        else
                        {
                            item.ShortGroup = getString;

                        }

                        newcollection.Add(item);
                    }

                    return new ObservableCollection<dailyactivity>(newcollection);

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //Post userdailyActivity
        public async Task<userdailyactivity> PostUserActiivty(userdailyactivity ActivityPassed)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.GetUserActivity;
                string jsonns = System.Text.Json.JsonSerializer.Serialize<userdailyactivity>(ActivityPassed);
                StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(url, contenttts);
                var errorResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(responseContent);

                    var id = jsonResponse["value"]?[0]?["id"]?.ToString();

                    ActivityPassed.id = id;
                    // Return the inserted item
                    return ActivityPassed;

                }
                else
                {
                    string errorcontent = await response.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
                // return null;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }



        public async Task<ObservableCollection<userdailyactivity>> GetUserActivityAsync()
        {
            try
            {
                var userId = Helpers.Settings.UserKey;
                string urlWithQuery = $"{GetUserActivity}?$filter=userid eq '{userId}'";

                ConfigureClient();
                HttpResponseMessage response = await Client.GetAsync(urlWithQuery);

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string content = await response.Content.ReadAsStringAsync();
                var userResponse = JsonConvert.DeserializeObject<ApiResponseUserActivity>(content);
                var consent = userResponse?.Value ?? new ObservableCollection<userdailyactivity>();

                var newCollection = new ObservableCollection<userdailyactivity>();

                foreach (var item in consent)
                {
                    // Deserialize `activityfrequency`
                    //if (!string.IsNullOrEmpty(item.activityfrequency))
                    //{
                    //    try
                    //    {
                    //        if (item.activityfrequency != "[]")
                    //        {
                    //            item.activityfrequencylist = JsonConvert.DeserializeObject<ObservableCollection<activefrequency>>(item.activityfrequency);
                    //        }
                    //    }
                    //    catch (JsonSerializationException)
                    //    {
                    //        var singleItem = JsonConvert.DeserializeObject<activefrequency>(item.activityfrequency);
                    //        item.activityfrequencylist = new ObservableCollection<activefrequency> { singleItem };
                    //    }
                    //}

                    // Deserialize `activitysymptoms`
                    if (!string.IsNullOrEmpty(item.activitysymptoms))
                    {
                        try
                        {
                            if (item.activitysymptoms != "[]")
                            {
                                item.ActivitySymptomsList = JsonConvert.DeserializeObject<ObservableCollection<ActivitySymptoms>>(item.activitysymptoms);
                            }
                        }
                        catch (JsonSerializationException)
                        {
                            var singleItem = JsonConvert.DeserializeObject<ActivitySymptoms>(item.activitysymptoms);
                            item.ActivitySymptomsList = new ObservableCollection<ActivitySymptoms> { singleItem };
                        }
                    }

                    // Deserialize `feedback`
                    if (!string.IsNullOrEmpty(item.feedback))
                    {
                        try
                        {
                            if (item.feedback != "[]")
                            {
                                item.ActivityFeedbackList = JsonConvert.DeserializeObject<ActivityFeedback>(item.feedback);
                            }
                        }
                        catch (JsonSerializationException)
                        {
                            var singleItem = JsonConvert.DeserializeObject<ActivityFeedback>(item.feedback);
                            item.ActivityFeedbackList = singleItem;
                        }
                    }

                    newCollection.Add(item);
                }
                //remove Deleted from List
                var UserActivity = newCollection?.Where(item => !item.deleted).ToObservableCollection() ?? new ObservableCollection<userdailyactivity>();

                return UserActivity;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //Update Activity Feedback 
        public async Task UpdateUserActivity(userdailyactivity Updatefeedback)
        {
            try
            {

                var id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userdailyactivity/id/{id}";
                var feedbacks = Updatefeedback;

                //Change the following  (To be defined)  
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    feedback = feedbacks.feedback,
                    notes = feedbacks.notes
                });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {

                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine("Successfully updated userDiet");
                    }
                }
            }

            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //Delete User Activity
        public async Task DeleteUserActivity(userdailyactivity Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userdailyactivity/id/{id}";

                string json = System.Text.Json.JsonSerializer.Serialize(new { deleted = Updatefeedback.deleted });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }



        // ( Come Back Too) 
        // Get Exercise information
        public async Task<ObservableCollection<exercise>> GetExerciseDetails()
        {
            try
            {
                var USERID = Helpers.Settings.UserKey;
                var URl = APICalls.GetExercise;
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(URl);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<APIExerciseResponse>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<exercise>();

                    //Remove All Deleted Items 
                    foreach (var item in consent)
                    {
                        if (item.deleted == true)
                        {

                        }
                        else
                        {
                            var GetString = item.grouping;
                            if (!String.IsNullOrEmpty(GetString))
                            {
                                if (GetString.Contains("Basic"))
                                {
                                    GetString = GetString.Replace("Basic ", "");
                                }

                                if (GetString.Contains("&"))
                                {
                                    GetString = GetString.Replace(" & ", "/");
                                }

                                item.ShortGroup = GetString;
                            }
                            newcollection.Add(item);
                        }
                    }

                    return new ObservableCollection<exercise>(newcollection);

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        //Edit Activity Details 
        public async Task UpdateActivityDetails(userdailyactivity Updatefeedback)
        {
            try
            {
                string id = Updatefeedback.id;
                var url = $"https://pwapi.peoplewith.com/api/userdailyactivity/id/{id}";

                var payload = new
                {
                    activityid = Updatefeedback.activityid,
                    activitytitle = Updatefeedback.activitytitle,
                    startdate = Updatefeedback.startdate,
                    notes = Updatefeedback.notes,
                    feedback = Updatefeedback.feedback
                };

                // Serialize the object into JSON
                string json = System.Text.Json.JsonSerializer.Serialize(payload);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                ConfigureClient();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Patch, url)
                    {
                        Content = content
                    };

                    var response = await Client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                    }
                }

                return;
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return;
            }
        }


        public async Task<ObservableCollection<userfitnessdata>> GetUserFitnessData()
        {
            try
            {
                ObservableCollection<userfitnessdata> itemstoremove = new ObservableCollection<userfitnessdata>();
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{GetUserFitness}?$filter=userid eq '{userid}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    // Add Feedback Converter
                    //  var settings = new JsonSerializerSettings();
                    //  settings.Converters.Add(new AppointmentFeedbackConverter());
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseUserFitness>(contentconsent);
                    var consent = userResponseconsent.Value;

                    var newcollection = new ObservableCollection<userfitnessdata>();

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
                        //{x 
                        //    // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //    var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.supplementfeedback);
                        //    item.supplementfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //}

                        if (!string.IsNullOrEmpty(item.stepfeedback))
                        {

                            try
                            {
                                // Attempt to deserialize as an array
                                if (item.stepfeedback == "[]")
                                {

                                }
                                else
                                {
                                    item.stepfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<fitnessfeedback>>(item.stepfeedback);
                                }
                            }
                            catch (JsonSerializationException)
                            {
                                // If the JSON is a single object, deserialize it as such and wrap it in a collection
                                var singleItem = JsonConvert.DeserializeObject<fitnessfeedback>(item.stepfeedback);
                                item.stepfeedbacklist = new ObservableCollection<fitnessfeedback> { singleItem };
                            }
                        }


                        //if (!string.IsNullOrEmpty(item.measurementfeedback))
                        //{
                        //    try
                        //    {

                        //        // Attempt to deserialize as an array
                        //        if (item.measurementfeedback == "[]")
                        //        {

                        //        }
                        //        else
                        //        {
                        //            // Attempt to deserialize as an array
                        //            item.measurementfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.measurementfeedback);
                        //        }

                        //    }
                        //    catch (JsonSerializationException)
                        //    {
                        //        // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //        var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.measurementfeedback);
                        //        item.measurementfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //    }
                        //}

                        //if (!string.IsNullOrEmpty(item.moodfeedback))
                        //{

                        //    try
                        //    {
                        //        // Attempt to deserialize as an array
                        //        if (item.moodfeedback == "[]")
                        //        {

                        //        }
                        //        else
                        //        {
                        //            // Attempt to deserialize as an array
                        //            item.moodfeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.moodfeedback);
                        //        }
                        //        // Attempt to deserialize as an array

                        //    }
                        //    catch (JsonSerializationException)
                        //    {
                        //        // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //        var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.moodfeedback);
                        //        item.moodfeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //    }
                        //}

                        //if (!string.IsNullOrEmpty(item.initialquestionnairefeedback))
                        //{

                        //    try
                        //    {
                        //        // Attempt to deserialize as an array
                        //        if (item.initialquestionnairefeedback == "[]")
                        //        {

                        //        }
                        //        else
                        //        {
                        //            // Attempt to deserialize as an array
                        //            item.initialquestionnairefeedbacklist = JsonConvert.DeserializeObject<ObservableCollection<feedbackdata>>(item.initialquestionnairefeedback);
                        //        }

                        //    }
                        //    catch (JsonSerializationException)
                        //    {
                        //        // If the JSON is a single object, deserialize it as such and wrap it in a collection
                        //        var singleItem = JsonConvert.DeserializeObject<feedbackdata>(item.initialquestionnairefeedback);
                        //        item.initialquestionnairefeedbacklist = new ObservableCollection<feedbackdata> { singleItem };
                        //    }
                        //}



                        newcollection.Add(item);
                    }



                    return new ObservableCollection<userfitnessdata>(newcollection);

                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }


        public async Task<userfitnessdata> InsertUserFitness(userfitnessdata item)
        {
            try
            {
                ConfigureClient();
                var url = APICalls.GetUserFitness;
                string jsonn = System.Text.Json.JsonSerializer.Serialize<userfitnessdata>(item);
                StringContent contenttt = new StringContent(jsonn, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await Client.PostAsync(url, contenttt);

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
            catch (Exception ex) when (
 ex is HttpRequestException ||
 ex is WebException ||
 ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }

        //Get DashQuestionnaire Data 
        public async Task<ObservableCollection<registryDataInputs>> GetDashQuestions()
        {
            try
            {
                var userid = Helpers.Settings.UserKey;
                var signupcode = Helpers.Settings.SignUp;
                if (string.IsNullOrEmpty(signupcode)) return null;

                //Change only for SFECORE00
                if (signupcode.Contains("SFECORE"))
                {
                    signupcode = "CORE01";
                }

                string urlWithQuery = $"{GetDashQuestionnaire}?$filter=dataInputs eq '{signupcode}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);
                var newcollection = new ObservableCollection<registryDataInputs>();

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseregistyDataInputs>(contentconsent);
                    var consent = userResponseconsent.Value;

                    newcollection = new ObservableCollection<registryDataInputs>(consent.Where(x => x.deleted != true && x.apporder != null));
                    //var GroupedData = newcollection.GroupBy(s => s.dataTab).ToObservableCollection();
                    return new ObservableCollection<registryDataInputs>(newcollection);
                }
                else
                {
                    string errorcontent = await responseconsent.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }

            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<registryDataInputs>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<registryDataInputs>();
            }
        }

        //PostDashQuestionnaire 
        public async Task PostDashQuestionnaire(ObservableCollection<registryData> RegistryAnswers)
        {
            try
            {
                for (int i = 0; i < RegistryAnswers.Count; i++)
                {
                    var urls = APICalls.DashQuestionAnswers;
                    ConfigureClient();
                    string jsonns = System.Text.Json.JsonSerializer.Serialize<registryData>(RegistryAnswers[i]);
                    StringContent contenttts = new StringContent(jsonns, Encoding.UTF8, "application/json");
                    var response = await Client.PostAsync(urls, contenttts);
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        //Success
                        string responseContent = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        //Failed
                        string errorcontent = await response.Content.ReadAsStringAsync();
                        var s = errorcontent;
                    }
                }
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
            }
        }


        //GetDashQuestioAnswers 
        public async Task<ObservableCollection<registryData>> GetDashQuestionAnswers()
        {
            try
            {
                var userid = Helpers.Settings.UserKey;
                string urlWithQuery = $"{DashQuestionAnswers}?$filter=userid eq '{userid}'";
                ConfigureClient();
                HttpResponseMessage responseconsent = await Client.GetAsync(urlWithQuery);
                var newcollection = new ObservableCollection<registryData>();

                if (responseconsent.IsSuccessStatusCode)
                {
                    string contentconsent = await responseconsent.Content.ReadAsStringAsync();
                    var userResponseconsent = JsonConvert.DeserializeObject<ApiResponseregistyData>(contentconsent);
                    var consent = userResponseconsent.Value;
                    newcollection = new ObservableCollection<registryData>(consent.Where(x => x.deleted != true));

                    return new ObservableCollection<registryData>(newcollection);

                }
                else
                {
                    string errorcontent = await responseconsent.Content.ReadAsStringAsync();
                    var s = errorcontent;
                    return null;
                }
            }
            catch (Exception ex) when (
ex is HttpRequestException ||
ex is WebException ||
ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<registryData>();
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return new ObservableCollection<registryData>();
            }
        }

        public async Task<user> UpdateUserID(user UpdateUser)
        {
            try
            {
                string url = "https://portal.peoplewith.com/migration/sql-update-user.php?uid=" + UpdateUser.userid + "&sid=" + UpdateUser.signupcodeid;
                //Old 
                //string url = "https://core.peoplewith.com/sql-update-user.php?uid=" + UpdateUser.userid + "&sid=" + UpdateUser.signupcodeid;
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        // Send a GET request to the URL
                        HttpResponseMessage response = await client.GetAsync(url);

                        // Check if the response is successful
                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            // Check if content is not empty
                            if (!string.IsNullOrEmpty(content))
                            {
                                var match = Regex.Match(content, @"newUserID:\s*'([^']*)'");
                                if (match.Success)
                                {
                                    string newUserId = match.Groups[1].Value;
                                    UpdateUser.userid = newUserId;
                                    return UpdateUser;
                                }
                            }
                            else
                            {
                                Console.WriteLine("No content returned from the URL.");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Failed to retrieve content. Status code: {response.StatusCode}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occurred: " + ex.Message);
                    }
                }

                return null;
            }
            catch (Exception ex) when (
       ex is HttpRequestException ||
       ex is WebException ||
       ex is TaskCanceledException)
            {
                await NotasyncMethod(ex);
                return null;
            }
            catch (Exception ex)
            {
                await NotasyncMethod(ex);
                return null;
            }
        }
    }
}
