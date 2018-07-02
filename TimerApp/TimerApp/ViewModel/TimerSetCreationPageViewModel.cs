using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TimerApp.Model;
using Xamarin.Forms;

namespace TimerApp.ViewModel
{
    class TimerSetCreationPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TimerSet> timerSets;
        public ObservableCollection<TimerSet> TimerSets { get { return timerSets; } set { timerSets = value;OnPropertyChanged(); } }
        public string SetId { get; internal set; }
        public TimerSetCreationPageViewModel()
        {
            timerSets = new ObservableCollection<TimerSet>();
            LoadTimerSets(string.Empty);
        }
        public TimerSetCreationPageViewModel(string workoutId):this()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public void LoadTimerSets(string workoutId)
        {
            var dbMgr = new DatabaseManager();

            //TODO timerSets = dbMgr.readEntry(workoutId);
            if (timerSets.Count() == 0 || string.Equals(workoutId, string.Empty))
            {
                timerSets.Add(new TimerSet() { Name = "Item", Repetitions = 1, Timers = new List<AtomicTimer>() });
            }

        }
        public void SaveTimerSets(string name, string workoutId)
        {

        }

        internal void AddTimerSet()
        {
            TimerSets.Add(new TimerSet() { Name = "Neues Set",
                Repetitions = 1,
                Timers = new List<AtomicTimer>() { new AtomicTimer() } });
            //throw new NotImplementedException();
        }

        internal void IncreaseRepetitions(TimerSet timerSet)
        {
            //TimerSets[TimerSets.IndexOf(timerSet)].Repetitions++;
            //throw new NotImplementedException();
        }

        internal void DecreaseRepetitions(TimerSet timerSet)
        {
            if (timerSet.Repetitions > 0)
            {
                TimerSets[TimerSets.IndexOf(timerSet)].Repetitions--;
            }
        }
   
    }
}
