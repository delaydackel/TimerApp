using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TimerApp.Model
{
    class Workout : INotifyPropertyChanged
    {
        private List<TimerSet> timers;// = new List<TimerSet>();
        private string playlist;


        public List<TimerSet> Timers
        {
            get { return timers; }
            set { timers = value; }
        }
        public string Playlist
        {
            get { return playlist; }
            set { playlist = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
