using System;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Inputs;

namespace PeopleWith
{
    /// <summary>
    /// Extended Editor Class to Enhance the Editor Control
    /// </summary>
    public class ExtendedMaskedEntry : SfMaskedEntry
    {
        public string IDValue { get; set; } // Name property for the entry
        public string IDRecord { get; set; }
        public string questionid { get; set; } // Updated to lowercase

        public string TextValue
        {
            get => (string)GetValue(TextValueProperty);
            set => SetValue(TextValueProperty, value);
        }

        private static void OnTextValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedMaskedEntry)bindable;
            var newText = newValue?.ToString();

            if (control.Value?.ToString() != newText)
            {
                control.Value = newText;
            }
        }

        // Bindable properties
        public static readonly BindableProperty IDValueProperty = BindableProperty.Create(
            nameof(IDValue), // Using nameof for property name
            typeof(string),
            typeof(ExtendedMaskedEntry),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: IDValuePropertyChanged);

        public static readonly BindableProperty IDRecordProperty = BindableProperty.Create(
            nameof(IDRecord), // Using nameof for property name
            typeof(string),
            typeof(ExtendedMaskedEntry),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: IDRecordPropertyChanged);

        public static readonly BindableProperty questionidProperty = BindableProperty.Create(
            nameof(questionid), // Updated to lowercase
            typeof(string),
            typeof(ExtendedMaskedEntry),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: QuestionIdPropertyChanged);


        public static readonly BindableProperty TextValueProperty = BindableProperty.Create(
           nameof(TextValue),
           typeof(string),
           typeof(ExtendedMaskedEntry),
           string.Empty,
           BindingMode.TwoWay,
           propertyChanged: OnTextValueChanged);



        public ExtendedMaskedEntry()
        {
            this.ValueChanged += (s, e) =>
            {
                // Update the bound property when the value changes
                if (TextValue != Value?.ToString())
                {
                    TextValue = Value?.ToString();
                }
            };
        }

        // Property changed handlers
        private static void IDValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedMaskedEntry)bindable;
            control.IDValue = newValue?.ToString(); // Use null-conditional operator
        }

        private static void IDRecordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedMaskedEntry)bindable;
            control.IDRecord = newValue?.ToString(); // Use null-conditional operator
        }

        private static void QuestionIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedMaskedEntry)bindable;
            control.questionid = newValue?.ToString(); // Updated to lowercase
        }

        private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedMaskedEntry)bindable;
            control.TextValue = newValue?.ToString();
        }
    }
}
