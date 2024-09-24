using System;
namespace PeopleWith
{
    /// <summary>
    /// Done entry Class to Extend Entry Class
    /// </summary>
	public class ExtendedImage : Image
    {
        // Properties for FeedbackID and UsermedID
        public string FeedbackID { get; set; }
        public string UsermedID { get; set; }

        // BindableProperty for FeedbackID
        public static readonly BindableProperty FeedbackIDProperty = BindableProperty.Create(
            propertyName: nameof(FeedbackID),
            returnType: typeof(string),
            declaringType: typeof(ExtendedImage),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: FeedbackIDPropertyChanged);

        // BindableProperty for UsermedID
        public static readonly BindableProperty UsermedIDProperty = BindableProperty.Create(
            propertyName: nameof(UsermedID),
            returnType: typeof(string),
            declaringType: typeof(ExtendedImage),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: UsermedIDPropertyChanged);

        // Property change callback for FeedbackID
        private static void FeedbackIDPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedImage)bindable;
            control.FeedbackID = newValue.ToString();
        }

        // Property change callback for UsermedID
        private static void UsermedIDPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedImage)bindable;
            control.UsermedID = newValue.ToString();
        }
    }
}