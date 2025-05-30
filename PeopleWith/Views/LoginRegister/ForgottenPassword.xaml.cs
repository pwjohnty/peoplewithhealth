using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using Mopups.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Maui.Devices;
using Microsoft.Maui.Networking;

namespace PeopleWith;

public partial class ForgottenPassword : ContentPage
{
    public HttpClient Client = new HttpClient();
    //Connectivity Changed 
    public event EventHandler<bool> ConnectivityChanged;
    //Crash Handler
    CrashDetected crashHandler = new CrashDetected();

    async public void NotasyncMethod(Exception Ex)
    {
        try
        {
            await crashHandler.SentryCrashDetected(Ex);
            await Navigation.PushAsync(new ErrorPage("Login"), false);
        }
        catch (Exception ex)
        {
            //Dunno 
        }
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

    public ForgottenPassword()
	{
        try
        {
            InitializeComponent();
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    static Regex ValidEmailRegex = CreateValidEmailRegex();

    private static Regex CreateValidEmailRegex()
    {
        string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }
    internal static bool EmailIsValid(string emailAddress)
    {
        bool isValid = ValidEmailRegex.IsMatch(emailAddress);
        return isValid;
    }

    private void emailentry_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            
        }
        catch (Exception Ex)
        {
            //Leave Empty
        }
    }

    private void EmailVerification_Clicked(object sender, EventArgs e)
    {
        try
        {
            //Connectivity Changed 
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                //Limit No. of Taps 
                EmailVerification.IsEnabled = false;
                handleemailframe();
                EmailVerification.IsEnabled = true;
            }
            else
            {
                var isConnected = accessType == NetworkAccess.Internet;
                ConnectivityChanged?.Invoke(this, isConnected);
            }      
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void handleemailframe()
    {
        try
        {

        bool ValidEmail = false;
        //check email validation
        if (string.IsNullOrEmpty(emailentry.Text))
        {
            emailhelper.ErrorText = "Please enter an email address";
            emailhelper.HasError = true;
            Vibration.Vibrate();
            emailentry.Focus();
            await Task.Delay(3000);
            emailhelper.HasError = false;
            return;
        }

        //trim email address
        emailentry.Text = emailentry.Text.Trim();

        if (!EmailIsValid(emailentry.Text))
        {
            emailhelper.ErrorText = "Please enter a valid email address";
            emailhelper.HasError = true;
            Vibration.Vibrate();
            emailentry.Focus();
            await Task.Delay(3000);
            emailhelper.HasError = false;
            return;
        }

        //check if email is in the db

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var url = APICalls.Checkuseremail + "%27" + emailentry.Text + "%27";

        ConfigureClient(); 

        HttpResponseMessage response = await Client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string content = await response.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<APIUserResponse>(content);
            ObservableCollection<user> users = userResponse.Value;

            if (users.Count == 0)
            {
                emailhelper.ErrorText = "Email does not Exist";
                emailhelper.HasError = true;
                Vibration.Vibrate();
                emailentry.Focus();
                await Task.Delay(3000);
                emailhelper.HasError = false;
                return;
            }
            else
            {
                //Valid Email Address
                //Temporary Check
                await MopupService.Instance.PushAsync(new PopupPageHelper("Email Verification Sent") { });
                    //await Task.Delay(1500);

                    //email generate Update URL
                    //StringContent mail_content = new StringContent(emailentry.Text, System.Text.Encoding.UTF8, "application/json");

                    //var emailresponse = await client.PostAsync("https://portal.peoplewith.com/hub/email-validation.php?uid=" + emailentry.Text, mail_content);
                    HttpClient hTTPClient = new HttpClient();

                    String json = String.Format(@"{{""Email"":""{0}""}}", emailentry.Text);

                    StringContent mail_content = new StringContent(json);

                    var GetResponse = await hTTPClient.PostAsync("https://portal.peoplewith.com/process-password-request.php?email=" + emailentry.Text, mail_content);

                    if (response.IsSuccessStatusCode)
                    {

                        //Successful

                    }


                    await MopupService.Instance.PopAllAsync(false);
                //    await Email.Default.ComposeAsync(message);

                //}
            }
          }
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }

    async private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        try
        {
            Navigation.RemovePage(this);
        }
        catch (Exception Ex)
        {
            NotasyncMethod(Ex);
        }
    }
}