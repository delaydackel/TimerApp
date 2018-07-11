using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TimerApp.Model
{
    public class Workout : BindableObject, INotifyPropertyChanged
    {
        private List<TimerSet> timers;// = new List<TimerSet>();
        private string playlist;
        private string id;
        private string name;
  
        public string Id { get { return id; } set { id = value; } }


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
        public Workout()
        {
            id = Guid.NewGuid().ToString();
            name = "Workout";
            playlist = Guid.NewGuid().ToString();
            timers = new List<TimerSet>() ;
        }
        public Workout(string uid)
        {
            id = uid;
        }       
        public TimeSpan Duration
        {
            get
            {
                TimeSpan rvalue = new TimeSpan(0);
                foreach (var set in this.Timers)
                {
                    for (int i = 0; i < set.Repetitions; i++)
                    {
                        foreach (var timer in set.Timers)
                        {
                            for (int j = 0; j < timer.Repetitions; j++)
                            {
                                rvalue = rvalue.Add(timer.Duration);
                                //rvalue = rvalue.Add(new TimeSpan(0, 0, 1));//hax
                            }

                        }
                    }

                }
                return rvalue;
            }
        }
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
