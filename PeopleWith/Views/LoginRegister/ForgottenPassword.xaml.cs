using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;
using Mopups.Services;
using System.Security.Cryptography;
using System.Text;

namespace PeopleWith;

public partial class ForgottenPassword : ContentPage
{
    HttpClient client = new HttpClient();
    public ForgottenPassword()
	{
		InitializeComponent();
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

    }

    private void EmailVerification_Clicked(object sender, EventArgs e)
    {
        try
        {
            handleemailframe();
        }
        catch (Exception Ex)
        {

        }
    }

    async private void handleemailframe()
    {
        bool ValidEmail = false;
        //check email validation
        if (string.IsNullOrEmpty(emailentry.Text))
        {
            emailhelper.ErrorText = "Please enter an email address";
            emailhelper.HasError = true;
            Vibration.Vibrate();
            emailentry.Focus();
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
            return;
        }

        //check if email is in the db

        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        var url = APICalls.Checkuseremail + "%27" + emailentry.Text + "%27";

        HttpResponseMessage response = await client.GetAsync(url);

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
                return;
            }
            else
            {
                //Valid Email Address
                //Temporary Check
                await MopupService.Instance.PushAsync(new PopupPageHelper("Email Verification Sent") { });
                await Task.Delay(1500);

                //email generate Update URL
                //StringContent mail_content = new StringContent(newuser.email, System.Text.Encoding.UTF8, "application/json");

                //var emailresponse = await client.PostAsync("https://peoplewithwebapp.azurewebsites.net/hub/email-validation.php?uid=" + newuser.email, mail_content);

                //if (Email.Default.IsComposeSupported)
                //{

                //    string subject = "";
                //    string body = "Userid: " + Helpers.Settings.UserKey;
                //    string[] recipients = new[] { "chris.johnston@peoplewith.com" };

                //    var message = new EmailMessage
                //    {
                //        Subject = subject,
                //        Body = body,
                //        BodyFormat = EmailBodyFormat.PlainText,
                //        To = new List<string>(recipients)
                //    };
                //    await MopupService.Instance.PopAllAsync(false);
                //    await Email.Default.ComposeAsync(message);

                //}
            }
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
           
        }
    }
}