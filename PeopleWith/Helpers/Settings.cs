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


        private const string UserIDKey = "id";
        private static readonly string UserKeyDefault = string.Empty;
        public static string UserKey
        {
            get => Preferences.Get(UserIDKey, UserKeyDefault);
            set => Preferences.Set(UserIDKey, value);
        }

        
        private const string UserTitleKey = "usertitle";
        private static readonly string UserTitleDefault = string.Empty;
        public static string UserTitle
        {
            get => Preferences.Get(UserTitleKey, UserTitleDefault);
            set => Preferences.Set(UserTitleKey, value);
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


        private const string DiagKey = "diagpopup";
        private static readonly string DiagDefault = string.Empty;
        public static string DiagPopup
        {
            get => Preferences.Get(DiagKey, DiagDefault);
            set => Preferences.Set(DiagPopup, value);
        }


        private const string LowerAgeKey = "loweragekey";
        private static readonly string LowerAgeDefault = string.Empty;
        public static string LowerAgeBracket
        {
            get => Preferences.Get(LowerAgeKey, LowerAgeDefault);
            set => Preferences.Set(LowerAgeBracket, value);
        }


        private const string UpperAgeKey = "upperagekey";
        private static readonly string UpperAgeDefault = string.Empty;
        public static string UpperAgeBracket
        {
            get => Preferences.Get(UpperAgeKey, UpperAgeDefault);
            set => Preferences.Set(UpperAgeBracket, value);
        }


        private const string MostRecentDiagnosisKey = "mostrecentdiagkey";
        private static readonly string MostRecentDiagnosisDefault = string.Empty;
        public static string MostRecentDiagnosisID
        {
            get => Preferences.Get(MostRecentDiagnosisKey, MostRecentDiagnosisDefault);
            set => Preferences.Set(MostRecentDiagnosisID, value);
        }


        private const string PasswordHash = "userpasswordhash";
        private static readonly string PasswordHashDefault = string.Empty;
        public static string UserPasswordHash
        {
            get => Preferences.Get(PasswordKey, PasswordDefault);
            set => Preferences.Set(UserPasswordHash, value);
        }


        private const string RotationSetting = "rotationsetting";
        private static readonly string RotationSettingDefault = string.Empty;
        public static string RotationSettingDash
        {
            get => Preferences.Get(RotationSetting, RotationSettingDefault);
            set => Preferences.Set(RotationSettingDash, value);
        }


        private const string UserPreference = "userpreferences";
        private static readonly string UserPreferenceDefault = string.Empty;
        public static string userpreferences
        {
            get => Preferences.Get(UserPreference, UserPreferenceDefault);
            set => Preferences.Set(userpreferences, value);
        }


        private const string UserAdvertID = "advertID";
        private static readonly string UserAdvertIDSettingDefault = string.Empty;
        public static string AdvertID
        {
            get => Preferences.Get(UserAdvertID, UserAdvertIDSettingDefault);
            set => Preferences.Set(UserAdvertID, value);
        }


        private const string DashSettings = "dashsettings";
        private static readonly string DashSettingsSettingDefault = string.Empty;
        public static string Dashsettings
        {
            get => Preferences.Get(DashSettings, DashSettingsSettingDefault);
            set => Preferences.Set(DashSettings, value);
        }


        private const string CancelMedicationNotifications = "cancelmedicationnotifications";
        private static readonly string CancelMedicationNotificationsDefault = string.Empty;
        public static string CancelNotificationsMedications
        {
            get => Preferences.Get(CancelMedicationNotifications, CancelMedicationNotificationsDefault);
            set => Preferences.Set(CancelNotificationsMedications, value);
        }


        private const string CancelSupplementNotifications = "cancelsupplementnotifications";
        private static readonly string CancelSupplementNotificationsDefault = string.Empty;
        public static string CancelNotificationsSupplements
        {
            get => Preferences.Get(CancelSupplementNotifications, CancelSupplementNotificationsDefault);
            set => Preferences.Set(CancelNotificationsSupplements, value);
        }


        private const string CancelAppointmentNotifications = "cancelappointmentnotifications";
        private static readonly string CancelAppointmentNotificationsDefault = string.Empty;
        public static string CancelNotificationsAppointmentts
        {
            get => Preferences.Get(CancelAppointmentNotifications, CancelAppointmentNotificationsDefault);
            set => Preferences.Set(CancelNotificationsAppointmentts, value);
        }


        private const string CancelSymptomNotifications = "cancelsymptomnotifications";
        private static readonly string CancelSymptomNotificationsDefault = string.Empty;
        public static string CancelNotificationsSymptoms
        {
            get => Preferences.Get(CancelSymptomNotifications, CancelSymptomNotificationsDefault);
            set => Preferences.Set(CancelNotificationsSymptoms, value);
        }


        private const string CancelPushNotifications = "cancelnotificationssetting";
        private static readonly string CancelPushNotificationsDefault = string.Empty;
        public static string CancelNotificationsPush
        {
            get => Preferences.Get(CancelPushNotifications, CancelPushNotificationsDefault);
            set => Preferences.Set(CancelNotificationsPush, value);
        }

        //Not Found 
        private const string CancelNotificationsSetting = "cancelnotificationssetting";
        private static readonly string CancelNotificationsSettingDefault = string.Empty;

        private const string CancelAppNotificationsSetting = "cancelappnotificationssetting";
        private static readonly string CancelAppNotificationsSettingDefault = string.Empty;


        private const string CancelNotificationsAppointments = "cancelnotificationsappointments";
        private static readonly string CancelNotificationsAppointmentsDefault = string.Empty;
        public static string CancelAppointNotifications
        {
            get => Preferences.Get(CancelNotificationsAppointments, CancelNotificationsAppointmentsDefault);
            set => Preferences.Set(CancelAppointNotifications, value);
        }


        private const string OverdueFirstDaySetting = "overduefirstdaysetting";
        private static readonly string OverdueFirstDaySettingDefault = string.Empty;
        public static string OverdueFirstDay
        {
            get => Preferences.Get(OverdueFirstDaySetting, OverdueFirstDaySettingDefault);
            set => Preferences.Set(OverdueFirstDay, value);
        }

        private const string HasDiagnosisSetting = "hasdiagnosissetting";
        private static readonly string HasDiagnosisSettingDefault = string.Empty;
        public static string HasDiagnosis
        {
            get => Preferences.Get(HasDiagnosisSetting, HasDiagnosisSettingDefault);
            set => Preferences.Set(HasDiagnosis, value);
        }


        private const string HasSymptomsSetting = "hassymptomssetting";
        private static readonly string HasSymptomsSettingDefault = string.Empty;
        public static string HasSymptoms
        {
            get => Preferences.Get(HasSymptomsSetting, HasSymptomsSettingDefault);
            set => Preferences.Set(HasSymptoms, value);
        }


        private const string UserNickname = "usernickname";
        private static readonly string HasUserNicknameDefault = string.Empty;
        public static string UserNickName
        {
            get => Preferences.Get(UserNickName, HasUserNicknameDefault);
            set => Preferences.Set(UserNickName, value);
        }


        private const string DictionaryJSON = "dictionaryjson";
        private static readonly string DictionaryJSONDefault = string.Empty;
        public static string Dictionaryjson
        {
            get => Preferences.Get(Dictionaryjson, DictionaryJSONDefault);
            set => Preferences.Set(Dictionaryjson, value);
        }


        private const string CancelAllNotificationsSetting = "cancelallnotificationssetting";
        private static readonly string CancelAllNotificationsSettingDefault = string.Empty;
        public static string CancelNotifications
        {
            get => Preferences.Get(CancelAllNotificationsSetting, CancelAllNotificationsSettingDefault);
            set => Preferences.Set(CancelNotifications, value);
        }


        private const string TurnOffSingleNotificationsSetting = "turnoffsinglenotificationssetting";
        private static readonly string TurnOffSingleNotificationsSettingDefault = string.Empty;
        public static string TurnOffSingleNotifications
        {
            get => Preferences.Get(TurnOffSingleNotificationsSetting, TurnOffSingleNotificationsSettingDefault);
            set => Preferences.Set(TurnOffSingleNotifications, value);
        }


        private const string DarkModeSetting = "darkmodesetting";
        private static readonly string DarkModeSettingDefault = string.Empty;
        public static string DarkMode
        {
            get => Preferences.Get(DarkModeSetting, DarkModeSettingDefault);
            set => Preferences.Set(DarkMode, value);
        }


        private const string Announcmentids = "announcementids";
        private static readonly string AnnouncmentidsSettingDefault = string.Empty;
        public static string Announcment
        {
            get => Preferences.Get(Announcmentids, AnnouncmentidsSettingDefault);
            set => Preferences.Set(Announcment, value);
        }


        private const string HeightKey = "height";
        private static readonly string HeightDefault = string.Empty;
        public static string Height
        {
            get => Preferences.Get(HeightKey, HeightDefault);
            set => Preferences.Set(Height, value);
        }


        private const string WeightKey = "weight";
        private static readonly string WeightDefault = string.Empty;
        public static string Weight
        {
            get => Preferences.Get(WeightKey, WeightDefault);
            set => Preferences.Set(Weight, value);
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

        private const string Autosym = "autosym";
        private static readonly string AutosymSettingDefault = string.Empty;
        public static string autosym
        {
            get => Preferences.Get(Autosym, AutosymSettingDefault);
            set => Preferences.Set(autosym, value);
        }


        private const string SignUpKey = "signupcode";
        private static readonly string SignUpKeySettingDefault = string.Empty;
        public static string SignUp
        {
            get => Preferences.Get(SignUpKey, SignUpKeySettingDefault);
            set => Preferences.Set(SignUp, value);
        }


        private const string SymptomSwipeKey = "symptomswipe";
        private static readonly string SymptomSwipeKeySettingDefault = string.Empty;
        public static string SymptomSwipe
        {
            get => Preferences.Get(SymptomSwipeKey, SymptomSwipeKeySettingDefault);
            set => Preferences.Set(SymptomSwipe, value);
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


        public const string UsersIDKey = "userid";
        private static readonly string UsersIDDefault = string.Empty;
        public static string UsersID
        {
            get => Preferences.Get(UsersIDKey, UsersIDDefault);
            set => Preferences.Set(UsersID, value);
        }


        private const string AudioKey = "audio";
        private static readonly string AudioDefault = string.Empty;
        public static string Audio
        {
            get => Preferences.Get(AudioKey, AudioDefault);
            set => Preferences.Set(Audio, value);
        }


        private const string VideoKey = "video";
        private static readonly string VideoDefault = string.Empty;
        public static string Video
        {
            get => Preferences.Get(VideoKey, VideoDefault);
            set => Preferences.Set(Video, value);
        }


        private const string WrittenKey = "written";
        private static readonly string WrittenDefault = string.Empty;
        public static string Written
        {
            get => Preferences.Get(WrittenKey, WrittenDefault);
            set => Preferences.Set(Written, value);
        }


        private const string PodcastKey = "podcasts";
        private static readonly string PodcastDefault = string.Empty;
        public static string Podcasts
        {
            get => Preferences.Get(PodcastKey, PodcastDefault);
            set => Preferences.Set(Podcasts, value);
        }


        private const string ArticlesKey = "articles";
        private static readonly string ArticlesDefault = string.Empty;
        public static string Articles
        {
            get => Preferences.Get(ArticlesKey, ArticlesDefault);
            set => Preferences.Set(Articles, value);
        }


        private const string ImagesKey = "images";
        private static readonly string ImagesDefault = string.Empty;
        public static string Images
        {
            get => Preferences.Get(ImagesKey, ImagesDefault);
            set => Preferences.Set(Images, value);
        }


        private const string PresentationsKey = "presentations";
        private static readonly string PresentationsDefault = string.Empty;
        public static string Presentations
        {
            get => Preferences.Get(PresentationsKey, PresentationsDefault);
            set => Preferences.Set(Presentations, value);
        }


        private const string EmailsKey = "emails";
        private static readonly string EmailsDefault = string.Empty;
        public static string Emails
        {
            get => Preferences.Get(EmailsKey, EmailsDefault);
            set => Preferences.Set(Emails, value);
        }


        private const string DigitalMeetingKey = "digitalmeetings";
        private static readonly string DigitalMeetingDefault = string.Empty;
        public static string DigitalMeetings
        {
            get => Preferences.Get(DigitalMeetingKey, DigitalMeetingDefault);
            set => Preferences.Set(DigitalMeetings, value);
        }


        public const string NotesKey = "notes";
        private static readonly string NotesDefault = string.Empty;
        public static string Notes
        {
            get => Preferences.Get(NotesKey, NotesDefault);
            set => Preferences.Set(Notes, value);
        }


        // Diagnosis Feelings Question

        public const string UserssIDKey = "userid";
        private static readonly string UserssIDDefault = string.Empty;
        public static string UserssID
        {
            get => Preferences.Get(UserssIDKey, UserssIDDefault);
            set => Preferences.Set(UserssID, value);
        }


        public const string NoEffectKey = "noeffect";
        private static readonly string NoEffectDefault = string.Empty;
        public static string NoEffect
        {
            get => Preferences.Get(NoEffectKey, NoEffectDefault);
            set => Preferences.Set(NoEffect, value);
        }


        public const string SmallEffectKey = "smalleffect";
        private static readonly string SmallEffectDefault = string.Empty;
        public static string SmallEffect
        {
            get => Preferences.Get(SmallEffectKey, SmallEffectDefault);
            set => Preferences.Set(SmallEffect, value);
        }


        public const string ModerateEffectKey = "moderateeffect";
        private static readonly string ModerateEffectDefault = string.Empty;
        public static string ModerateEffect
        {
            get => Preferences.Get(ModerateEffectKey, ModerateEffectDefault);
            set => Preferences.Set(ModerateEffect, value);
        }


        public const string LargeEffectKey = "largeeffect";
        private static readonly string LargeEffectDefault = string.Empty;
        public static string LargeEffect
        {
            get => Preferences.Get(LargeEffectKey, LargeEffectDefault);
            set => Preferences.Set(LargeEffect, value);
        }


        public const string SevereEffectKey = "severeeffect";
        private static readonly string SevereEffectDefault = string.Empty;
        public static string SevereEffect
        {
            get => Preferences.Get(SevereEffectKey, SevereEffectDefault);
            set => Preferences.Set(SevereEffect, value);
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
