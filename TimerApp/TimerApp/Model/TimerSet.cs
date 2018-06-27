using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace TimerApp.Model
{
    class TimerSet: INotifyPropertyChanged
    {
        //public TimerSet() : base()
        //{
        //    this.Add("Name", string.Empty);
        //    this.Add("Timers", new List<AtomicTimer>());
        //    this.Add("Repetitions", 0);
        //}
        private string name;
        private List<AtomicTimer> timers;
        private int repetitions;
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        public List<AtomicTimer> Timers { get { return timers; } set { timers = value; OnPropertyChanged(); } }
        public int Repetitions { get { return repetitions; }set { repetitions = value; OnPropertyChanged(); } }
        public TimerSet() : base()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
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
