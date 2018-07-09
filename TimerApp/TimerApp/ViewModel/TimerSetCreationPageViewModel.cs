using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using TimerApp.Control;
using TimerApp.Model;
using Xamarin.Forms;

namespace TimerApp.ViewModel
{
    class TimerSetCreationPageViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TimerSet> timerSets;
        public ObservableCollection<TimerSet> TimerSets { get { return timerSets; } set { timerSets = value;OnPropertyChanged(); } }
       
        public string WorkoutId { get; internal set; }
        public TimerSetCreationPageViewModel()
        {
            timerSets = new ObservableCollection<TimerSet>();
            LoadTimerSets();
        }
        public TimerSetCreationPageViewModel(string workoutId):this()
        {
            WorkoutId = workoutId;            
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


        public void LoadTimerSets()
        {
            //var dbMgr = new DatabaseManager();

            //TODO timerSets = dbMgr.readEntry(workoutId);
            // hier das aktuelle set aus dem appcore.currentworkout ziehen
            TimerSets.Clear();
            foreach (var item in AppCore.CurrentWorkout.Timers)
            {
                TimerSets.Add(item);
            } 
            if (TimerSets.Count() == 0 )
            {
                TimerSets.Add(new TimerSet());
                //SaveTimerSets();
            }

        }
        public void SaveTimerSets()
        {
            var currentTimers = TimerSets.ToList<TimerSet>();
            AppCore.CurrentWorkout.Timers = currentTimers;
            var blub = AppCore.Workouts.Where(wo => wo.Id == this.WorkoutId).First();
            blub =  AppCore.CurrentWorkout;
            var dbMgr = new DatabaseManager();
            dbMgr.SaveWorkouts(AppCore.Workouts);
        }

        internal void AddTimerSet()
        {
            TimerSets.Add(new TimerSet());
            var currentTimers = TimerSets.ToList<TimerSet>();
            AppCore.CurrentWorkout.Timers = currentTimers;
            var blub = AppCore.Workouts.Where(wo => wo.Id == this.WorkoutId).First();
            blub = AppCore.CurrentWorkout;
            //{
            //    SetId = Guid.NewGuid().ToString(),
            //    Name = "neues Set",
            //    Repetitions = 1,
            //    Timers = new List<AtomicTimer>()
            //            {
            //                new AtomicTimer(){Name = "Timer",Repetitions = 1, Duration = new TimeSpan(0,0,1)}
            //            }
            //});
            //throw new NotImplementedException();
        }

     
   
    }
}
