using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    

    class TimerPageViewModel : INotifyPropertyChanged
    {
        public DisplayTimer CurrentTimer { get; set; }

        public TimerPageViewModel()
        {
            CurrentTimer = new DisplayTimer();
            CurrentTimer.Duration = new TimeSpan(10000);
            CurrentTimer.Name = "Uebung01";
            CurrentTimer.Repetitions = 5;
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
