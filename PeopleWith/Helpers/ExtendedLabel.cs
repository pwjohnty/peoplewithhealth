using System;

namespace PeopleWith
{
    /// <summary>
    /// ExtendedLabel Class to Extend Label Class with Dosage and Time properties
    /// </summary>
    public class ExtendedLabel : Label
    {
        // Properties for Dosage and Time
        public string Dosage { get; set; }
        public string Time { get; set; }

        // BindableProperty for Dosage
        public static readonly BindableProperty DosageProperty = BindableProperty.Create(
            propertyName: nameof(Dosage),
            returnType: typeof(string),
            declaringType: typeof(ExtendedLabel),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: DosagePropertyChanged);

        // BindableProperty for Time
        public static readonly BindableProperty TimeProperty = BindableProperty.Create(
            propertyName: nameof(Time),
            returnType: typeof(string),
            declaringType: typeof(ExtendedLabel),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TimePropertyChanged);

        // Property change callback for Dosage
        private static void DosagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedLabel)bindable;
            control.Dosage = newValue.ToString();
            // Update the Label text or do any other required action here
            control.Text = $"Dosage: {control.Dosage}"; // Example: Setting Label text to show dosage
        }

        // Property change callback for Time
        private static void TimePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedLabel)bindable;
            control.Time = newValue.ToString();
            // Update the Label text or do any other required action here
            control.Text += $", Time: {control.Time}"; // Example: Appending time to the Label text
        }
    }
}