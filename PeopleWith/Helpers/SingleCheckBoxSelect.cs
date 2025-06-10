using System;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Buttons;
using Microsoft.Maui.Controls;
using System.Windows.Input;

namespace PeopleWith
{
    /// <summary>
    /// ExtendedRadioButton Class to Extend SfRadioButton for MAUI
    /// </summary>
    public class SingleCheckSelect : SfCheckBox
    {
        public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(SingleCheckSelect),
            null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(
                nameof(CommandParameter),
                typeof(object),
                typeof(SingleCheckSelect),
                null);

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public SingleCheckSelect()
        {
            StateChanged += OnStateChanged;
        }

        private void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            // only fire when checked
            if (e.IsChecked == true && Command?.CanExecute(CommandParameter) == true)
                Command.Execute(CommandParameter);
        }
    }
}