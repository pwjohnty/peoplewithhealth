using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace PeopleWith
{
    public class CalendarOverlayViewModel : INotifyPropertyChanged
    {
        private bool _isCalendarPopupVisible;
        private DateTime _selectedDate;

        public bool IsCalendarPopupVisible
        {
            get => _isCalendarPopupVisible;
            set
            {
                _isCalendarPopupVisible = value;
                OnPropertyChanged();
            }
        }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowCalendarPopupCommand { get; }
        public ICommand HideCalendarPopupCommand { get; }
        public ICommand ConfirmDateCommand { get; }

        public CalendarOverlayViewModel()
        {
            SelectedDate = DateTime.Today;

            ShowCalendarPopupCommand = new Command(() => IsCalendarPopupVisible = true);
            HideCalendarPopupCommand = new Command(() => IsCalendarPopupVisible = false);
            ConfirmDateCommand = new Command(() =>
            {
                IsCalendarPopupVisible = false;
                // Optionally trigger an event or data update
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
