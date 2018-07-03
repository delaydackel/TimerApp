using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TimerApp.Model
{
    class DisplayTimer : BindableObject, INotifyPropertyChanged
    {
        private string name;
        private TimeSpan duration;
        private int repetitions;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; OnPropertyChanged(); }
        }
        public int Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }
        public DisplayTimer(AtomicTimer timer) {
            name = timer.Name;
            duration = timer.Duration;
            repetitions = timer.Repetitions;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
