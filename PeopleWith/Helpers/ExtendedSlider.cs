﻿using System;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Sliders;

namespace PeopleWith
{
    /// <summary>
    /// Extended Editor Class to Enhance the Editor Control
    /// </summary>
    public class ExtendedSlider : SfSlider
    {
        public string IDValue { get; set; } // Name property for the entry
        public string IDRecord { get; set; }
        public string questionid { get; set; } // Updated to lowercase

        // Bindable properties
        public static readonly BindableProperty IDValueProperty = BindableProperty.Create(
            nameof(IDValue), // Using nameof for property name
            typeof(string),
            typeof(ExtendedSlider),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: IDValuePropertyChanged);

        public static readonly BindableProperty IDRecordProperty = BindableProperty.Create(
            nameof(IDRecord), // Using nameof for property name
            typeof(string),
            typeof(ExtendedSlider),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: IDRecordPropertyChanged);

        public static readonly BindableProperty questionidProperty = BindableProperty.Create(
            nameof(questionid), // Updated to lowercase
            typeof(string),
            typeof(ExtendedSlider),
            string.Empty,
            BindingMode.TwoWay,
            propertyChanged: QuestionIdPropertyChanged);

        // Property changed handlers
        private static void IDValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedSlider)bindable;
            control.IDValue = newValue?.ToString(); // Use null-conditional operator
        }

        private static void IDRecordPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedSlider)bindable;
            control.IDRecord = newValue?.ToString(); // Use null-conditional operator
        }

        private static void QuestionIdPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ExtendedSlider)bindable;
            control.questionid = newValue?.ToString(); // Updated to lowercase
        }
    }
}
