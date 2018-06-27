using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using TimerApp.Control;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    

    class TimerPageViewModel : INotifyPropertyChanged
    {

        public DisplayTimer CurrentTimer { get; set; }
        private string name;
        private string duration;
        private uint repetitions;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public uint Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }
     
        public TimerPageViewModel()
        {
            CurrentTimer = new DisplayTimer();
            CurrentTimer.Duration = new TimeSpan(1, 2, 3);
            CurrentTimer.Name = "Uebung01";
            CurrentTimer.Repetitions = 5;

            Duration = CurrentTimer.Duration.ToString();
            //Duration = secondsToCountdown();
            TimerManager mgr = new TimerManager();

            //mgr.work += 

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
