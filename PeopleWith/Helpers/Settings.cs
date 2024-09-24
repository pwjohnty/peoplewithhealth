using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace PeopleWith.Helpers
{
    public static class Settings
    {
        public static bool? biometrics
        {
            get
            {
                if (Preferences.ContainsKey(nameof(biometrics)))
                {
                    // Get the value or return true if the value is null
                    bool? value = Preferences.Get(nameof(biometrics), true);
                    return value ?? true; 
                }
                else
                {
                    return true; // Return true if the key doesn't exist
                }
            }
            set
            {
                if (value.HasValue)
                {
                    Preferences.Set(nameof(biometrics), value.Value); 
                }
                else
                {
                    Preferences.Set(nameof(biometrics), true); // If value is null, set it to true
                }
            }
        }
        public static bool FirstRun
        {
            get => Preferences.Get(nameof(FirstRun), true);
            set => Preferences.Set(nameof(FirstRun), value);
        }
        public static bool LaunchVideo
        {
            get => Preferences.Get(nameof(LaunchVideo), true);
            set => Preferences.Set(nameof(LaunchVideo), value);
        }
        public static bool IsActive
        {
            get => Preferences.Get(nameof(IsActive), true);
            set => Preferences.Set(nameof(IsActive), false);
        }


        private const string UserIDKey = "userid";
        private static readonly string UserKeyDefault = string.Empty;
        public static string UserKey
        {
            get => Preferences.Get(UserIDKey, UserKeyDefault);
            set => Preferences.Set(UserIDKey, value);
        }


        private const string FirstNameKey = "firstname";
        private static readonly string FirstNameDefault = string.Empty;
        public static string FirstName
        {
            get => Preferences.Get(FirstNameKey, FirstNameDefault);
            set => Preferences.Set(FirstNameKey, value);
        }


        private const string SurnameKey = "surname";
        private static readonly string SurnameDefault = string.Empty;
        public static string Surname
        {
            get => Preferences.Get(SurnameKey, SurnameDefault);
            set => Preferences.Set(SurnameKey, value);
        }


        private const string GenderKey = "gender";
        private static readonly string GenderDefault = string.Empty;
        public static string Gender
        {
            get => Preferences.Get(GenderKey, GenderDefault);
            set => Preferences.Set(GenderKey, value);
        }


        private const string EmailKey = "email";
        private static readonly string EmailDefault = string.Empty;
        public static string Email
        {
            get => Preferences.Get(EmailKey, EmailDefault);
            set => Preferences.Set(EmailKey, value);
        }


        private const string PasswordKey = "password";
        private static readonly string PasswordDefault = string.Empty;
        public static string Password
        {
            get => Preferences.Get(PasswordKey, PasswordDefault);
            set => Preferences.Set(PasswordKey, value);
        }


        private const string AgeKey = "age";
        private static readonly string AgeDefault = string.Empty;
        public static string Age
        {
            get => Preferences.Get(AgeKey, AgeDefault);
            set => Preferences.Set(AgeKey, value);
        }


        private const string EthnicityKey = "ethnicity";
        private static readonly string EthnicityDefault = string.Empty;
        public static string Ethnicity
        {
            get => Preferences.Get(EthnicityKey, EthnicityDefault);
            set => Preferences.Set(EthnicityKey, value);
        }


        private const string AddressLineOneKey = "addresslineone";
        private static readonly string AddressLineOneDefault = string.Empty;
        public static string AddressLineOne
        {
            get => Preferences.Get(AddressLineOneKey, AddressLineOneDefault);
            set => Preferences.Set(AddressLineOneKey, value);
        }


        private const string AddressLineTwoKey = "addresslinetwo";
        private static readonly string AddressLineTwoDefault = string.Empty;
        public static string AddressLineTwo
        {
            get => Preferences.Get(AddressLineTwoKey, AddressLineTwoDefault);
            set => Preferences.Set(AddressLineTwoKey, value);
        }


        private const string PostcodeKey = "postcode";
        private static readonly string PostcodeDefault = string.Empty;
        public static string Postcode
        {
            get => Preferences.Get(PostcodeKey, PostcodeDefault);
            set => Preferences.Set(Postcode, value);
        }

        private const string TownKey = "town";
        private static readonly string TownDefault = string.Empty;
        public static string Town
        {
            get => Preferences.Get(TownKey, TownDefault);
            set => Preferences.Set(Town, value);
        }


        private const string CityKey = "city";
        private static readonly string CityDefault = string.Empty;
        public static string City
        {
            get => Preferences.Get(CityKey, CityDefault);
            set => Preferences.Set(City, value);
        }


        private const string PhoneNumberKey = "phonenumber";
        private static readonly string PhoneNumberDefault = string.Empty;
        public static string PhoneNumber
        {
            get => Preferences.Get(PhoneNumberKey, PhoneNumberDefault);
            set => Preferences.Set(PhoneNumber, value);
        }


        private const string WalkThroughKey = "walkthrough";
        private static readonly string WalkThroughDefault = string.Empty;
        public static string WalkThrough
        {
            get => Preferences.Get(WalkThroughKey, WalkThroughDefault);
            set => Preferences.Set(WalkThrough, value);
        }


        private const string PasswordHash = "userpasswordhash";
        private static readonly string PasswordHashDefault = string.Empty;
        public static string UserPasswordHash
        {
            get => Preferences.Get(PasswordKey, PasswordDefault);
            set => Preferences.Set(UserPasswordHash, value);
        }


        private const string UserPreference = "userpreferences";
        private static readonly string UserPreferenceDefault = string.Empty;
        public static string userpreferences
        {
            get => Preferences.Get(UserPreference, UserPreferenceDefault);
            set => Preferences.Set(userpreferences, value);
        }


        private const string Announcmentids = "announcementids";
        private static readonly string AnnouncmentidsSettingDefault = string.Empty;
        public static string Announcment
        {
            get => Preferences.Get(Announcmentids, AnnouncmentidsSettingDefault);
            set => Preferences.Set(Announcment, value);
        }



        private const string DOBKey = "dob";
        private static readonly string DOBDefault = string.Empty;
        public static string DOB
        {
            get => Preferences.Get(DOBKey, DOBDefault);
            set => Preferences.Set(DOB, value);
        }


        private const string UpdateKey = "update";
        private static readonly string UpdateSettingDefault = string.Empty;
        public static string update
        {
            get => Preferences.Get(UpdateKey, UpdateSettingDefault);
            set => Preferences.Set(update, value);
        }


        private const string SignUpKey = "signupcode";
        private static readonly string SignUpKeySettingDefault = string.Empty;
        public static string SignUp
        {
            get => Preferences.Get(SignUpKey, SignUpKeySettingDefault);
            set => Preferences.Set(SignUp, value);
        }

        private const string PinCodeKey = "pincode";
        private static readonly string PinCodeKeySettingDefault = string.Empty;
        public static string PinCode
        {
            get => Preferences.Get(PinCodeKey, PinCodeKeySettingDefault);
            set => Preferences.Set(PinCode, value);
        }

        private const string LaunchVideoKey = "launchvideo";
        private static readonly string LaunchVideKeySettingDefault = string.Empty;
        public static string launchvideo
        {
            get => Preferences.Get(LaunchVideoKey, LaunchVideKeySettingDefault);
            set => Preferences.Set(launchvideo, value);
        }


        private const string applicationcount = "appcount";
        private static readonly string applicationcountDefault = string.Empty;
        public static string appcount
        {
            get => Preferences.Get(applicationcount, applicationcountDefault);
            set => Preferences.Set(appcount, value);
        }


        private const string AppusingKey = "appusing";
        private static readonly string AppusingKeyDefault = string.Empty;
        public static string Appusing
        {
            get => Preferences.Get(AppusingKey, AppusingKeyDefault);
            set => Preferences.Set(Appusing, value);
        }


        private const string NewdashupdateKey = "newdashupdate";
        private static readonly string NewdashupdateDefault = string.Empty;
        public static string Newdashupdate
        {
            get => Preferences.Get(NewdashupdateKey, NewdashupdateDefault);
            set => Preferences.Set(Newdashupdate, value);
        }


        private const string ClinicalTrialKey = "clinicaltrial";
        private static readonly string ClinicalTrialDefault = string.Empty;
        public static string Clinicaltrial
        {
            get => Preferences.Get(ClinicalTrialKey, ClinicalTrialDefault);
            set => Preferences.Set(Clinicaltrial, value);
        }


        private const string MedNotificationsKey = "mednotifications";
        private static readonly string MedNotificationsDefault = string.Empty;
        public static string MedNotifications
        {
            get => Preferences.Get(MedNotificationsKey, MedNotificationsDefault);
            set => Preferences.Set(MedNotifications, value);
        }

        private const string SuppNotificationsKey = "suppnotifications";
        private static readonly string SuppNotificationsDefault = string.Empty;
        public static string SuppNotifications
        {
            get => Preferences.Get(SuppNotificationsKey, SuppNotificationsDefault);
            set => Preferences.Set(SuppNotifications, value);
        }


        private const string AppointNotificationsKey = "appointnotifications";
        private static readonly string AppointNotificationsDefault = string.Empty;
        public static string AppointNotifications
        {
            get => Preferences.Get(AppointNotificationsKey, AppointNotificationsDefault);
            set => Preferences.Set(AppointNotifications, value);
        }


        private const string HasAdditionalConsentKey = "additionalconsent";
        private static readonly string HasAdditionalConsentDefault = string.Empty;
        public static string AdditionalConsent
        {
            get => Preferences.Get(HasAdditionalConsentKey, HasAdditionalConsentDefault);
            set => Preferences.Set(AdditionalConsent, value);
        }


        private const string createdatdatekey = "createdat";
        private static readonly string createdatdateDefault = string.Empty;
        public static string CreatedAt
        {
            get => Preferences.Get(createdatdatekey, createdatdateDefault);
            set => Preferences.Set(CreatedAt, value);
        }


        private const string createdatdateonlykey = "createdatdateonly";
        private static readonly string createdatdateonlyDefault = string.Empty;
        public static string CreatedAtDateOnly
        {
            get => Preferences.Get(createdatdateonlykey, createdatdateonlyDefault);
            set => Preferences.Set(CreatedAtDateOnly, value);
        }


        private const string usergpidkey = "usergpid";
        private static readonly string usergpidDefault = string.Empty;
        public static string Usergpid
        {
            get => Preferences.Get(usergpidkey, usergpidDefault);
            set => Preferences.Set(Usergpid, value);
        }

        private const string TokenKey = "token";
        private static readonly string TokenDefault = string.Empty;
        public static string Token
        {
            get => Preferences.Get(TokenKey, TokenDefault);
            set => Preferences.Set(TokenKey, value);
        }
        private const string DeviceIDKey = "deviceid";
        private static readonly string DeviceIDDefault = string.Empty;
        public static string DeviceID
        {
            get => Preferences.Get(DeviceIDKey, DeviceIDDefault);
            set => Preferences.Set(DeviceIDKey, value);
        }

    }
}
