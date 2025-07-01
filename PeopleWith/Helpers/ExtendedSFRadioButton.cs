using System;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Buttons; // Use .NET MAUI namespace

namespace PeopleWith
{
    /// <summary>
    /// ExtendedRadioButton Class to Extend SfRadioButton for MAUI
    /// </summary>
    public class ExtendedSFRadioButton : SfRadioButton
    {
        public string IDValue { get; set; }
        public string IDRecord { get; set; }
        public string questionid { get; set; }

        // BindableProperty for IDValue
        public static readonly BindableProperty IDValueProperty = BindableProperty.Create(
            propertyName: nameof(IDValue),
            returnType: typeof(string),
            declaringType: typeof(ExtendedSFRadioButton),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnIDValueChanged);

        // BindableProperty for IDRecord
        public static readonly BindableProperty IDRecordProperty = BindableProperty.Create(
            propertyName: nameof(IDRecord),
            returnType: typeof(string),
            declaringType: typeof(ExtendedSFRadioButton),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnIDRecordChanged);

        // BindableProperty for questionid
        public static readonly BindableProperty questionidProperty = BindableProperty.Create(
            propertyName: nameof(questionid),
            returnType: typeof(string),
            declaringType: typeof(ExtendedSFRadioButton),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnQuestionIDChanged);

        // Property change handler for IDValue
        private static void OnIDValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedSFRadioButton)bindable;
            control.IDValue = newValue?.ToString();
        }

        // Property change handler for IDRecord
        private static void OnIDRecordChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedSFRadioButton)bindable;
            control.IDRecord = newValue?.ToString();
        }

        // Property change handler for questionid
        private static void OnQuestionIDChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedSFRadioButton)bindable;
            control.questionid = newValue?.ToString();
        }
    }
}
