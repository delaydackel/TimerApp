using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TimerApp.Model
{
    class TimerSet:Dictionary<string, object>, INotifyPropertyChanged
    {
        public TimerSet() : base()
        {
            this.Add("Name", string.Empty);
            this.Add("Timers", new List<AtomicTimer>());
            this.Add("Repetitions", 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        //private List<AtomicTimer> elements;
        //private uint repetitions;
        //public List<AtomicTimer> Elements
        //{
        //    get { return elements; }
        //    set { elements = value; }
        //}
        //public uint Repetitions
        //{
        //    get { return repetitions; }
        //    set { repetitions = value; }
        //}
    }
}
