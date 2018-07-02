using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using TimerApp.Control;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    class TimerCreationPageViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<AtomicTimer> timerList;
        public ObservableCollection<TimerApp.Model.AtomicTimer> TimerList
        {
            get { return timerList; }
            set { timerList = value; OnPropertyChanged(); }
        }

        public string SetId { get; internal set; }

        public TimerCreationPageViewModel()
        {
            timerList = new ObservableCollection<AtomicTimer>();
            //LoadTimers();
            //var testTimer = new AtomicTimer();
            //testTimer.Name = "asd";
            //testTimer.Repetitions = 15;
            //testTimer.Duration = new TimeSpan(2000);
            //var testTimer2 = new AtomicTimer();
            //testTimer2.Name = "asd";
            //testTimer2.Repetitions = 15;
            //testTimer2.Duration = new TimeSpan(2000);
            //var testTimer3 = new AtomicTimer();
            //testTimer3.Name = "asd";
            //testTimer3.Repetitions = 15;
            //testTimer3.Duration = new TimeSpan(2000);
            ////testTimer["Name"] = "asd";
            ////testTimer["Duration"] = new TimeSpan(2000);
            ////testTimer["Repetitions"] = 15;
            //TimerList.Add(testTimer);
            //TimerList.Add(testTimer2);
            //TimerList.Add(testTimer3);
            //var blub = new TimerSet();
            //blub.Timers = new List<AtomicTimer> { testTimer,testTimer2,testTimer3};
        }


        public void LoadTimers()
        {            
            var blub = AppCore.CurrentWorkout.Timers.Where(timer => string.Equals(timer.SetId, this.SetId)).First().Timers;

            foreach (var item in blub)
            {
                TimerList.Add(item);
            }
            //var dbMgr = new DatabaseManager();
            //TimerList = dbMgr.LoadTimerList(SetId);
            //benutze setid um richtgen Datensatz zu ladnen
            //throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void AddTimer()
        {
            TimerList.Add(new AtomicTimer() { Name = "Timer",Repetitions=1});
         //   throw new NotImplementedException();
        }
        internal void SaveWorkouts(string name)
        {
            List<AtomicTimer> currentTimers = TimerList.ToList<AtomicTimer>();
            //var currentSet = ;
            AppCore.CurrentWorkout.Timers.Where(set => string.Equals(this.SetId, set.SetId)).First().Timers = currentTimers;
            AppCore.Workouts.Where(workout => workout.Id == AppCore.CurrentWorkout.Id).First().Timers = AppCore.CurrentWorkout.Timers;
            var dbMgr = new DatabaseManager();
            dbMgr.SaveWorkouts(AppCore.Workouts);//.SaveObsCollOfTimers(TimerList, name, SetId);
            //throw new NotImplementedException();
        }
    }
}
