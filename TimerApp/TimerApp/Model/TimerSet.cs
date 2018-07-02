using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace TimerApp.Model
{
    public class TimerSet: BindableObject, INotifyPropertyChanged
    {
        //public TimerSet() : base()
        //{
        //    this.Add("Name", string.Empty);
        //    this.Add("Timers", new List<AtomicTimer>());
        //    this.Add("Repetitions", 0);
        //}
        private string name;
        private string id;
        private List<AtomicTimer> timers;
        private int repetitions;
        public string Name { get { return name; } set { name = value; OnPropertyChanged(); } }
        public List<AtomicTimer> Timers { get { return timers; } set { timers = value; OnPropertyChanged(); } }
        public int Repetitions { get { return repetitions; }set { repetitions = value; OnPropertyChanged(); } }
        public string SetId { get { return id; } set { id = value; OnPropertyChanged(); } }
        public TimerSet() : base()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        private ICommand increaseRepetitionsCommand;
        public ICommand IncreaseRepetitionsCommand
        {
            get
            {
                if (increaseRepetitionsCommand == null)
                {
                    increaseRepetitionsCommand = new Command(() => Repetitions++);
                }
                return increaseRepetitionsCommand;
            }
        }
        private ICommand decreaseRepetitionsCommand;
        public ICommand DecreaseRepetitionsCommand
        {
            get
            {
                if (decreaseRepetitionsCommand == null)
                {
                    decreaseRepetitionsCommand = new Command(() => {
                        if (Repetitions>0)
                        {
                            Repetitions--;
                        }
                    });
                }
                return decreaseRepetitionsCommand;
            }
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
