using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TimerApp.Control;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    class WorkoutCreationPageViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<Workout> workouts;
        public ObservableCollection<Workout> WorkoutsCollection { get { return workouts; }set { workouts = value;OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public WorkoutCreationPageViewModel()
        {
            WorkoutsCollection = new ObservableCollection<Workout>();
       
            
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void LoadWorkouts()
        {
            WorkoutsCollection.Clear();// = new ObservableCollection<Workout>();
            foreach (var item in (AppCore.Workouts))
            {
                WorkoutsCollection.Add(item);
            }
        }

        internal void SaveWorkouts()
        {
            var currentWorkouts = new List<Workout>();
            foreach (var item in WorkoutsCollection)
            {
                currentWorkouts.Add(item);
            }
            AppCore.Workouts = currentWorkouts;
            var dbMgr = new DatabaseManager();
            dbMgr.SaveWorkouts(AppCore.Workouts);
        }

        internal void AddWorkout()
        {
            WorkoutsCollection.Add(new Workout());
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Playlist = string.Empty,
            //    Timers = new List<TimerSet>() {
            //        new TimerSet(){
            //            SetId = Guid.NewGuid().ToString(),
            //            Name = "neues Set",
            //            Repetitions = 1,
            //            Timers = new List<AtomicTimer>()
            //            {
            //                new AtomicTimer(){Name = "Timer",Repetitions = 1, Duration = new TimeSpan(0,0,1)}
            //            }
            //        }

            //                },
            //    Name = "Workout"
            //});
        }
    }
}
