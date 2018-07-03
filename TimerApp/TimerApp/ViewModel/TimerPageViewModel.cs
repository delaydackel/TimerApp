using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Timers;
using TimerApp.Control;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    

    class TimerPageViewModel : INotifyPropertyChanged
    {
        private Workout currentWorkout;
        public DisplayTimer CurrentTimer { get; set; }
        private TimerManager manager = new TimerManager();
        private string currentTimerName;
        private string duration;
        private int repetitions;
        private int exerciseIndex = 0;
        private int currentExerciseRepetitions = 0;
        private int currentSetRepetitions = 0;
        private List<AtomicTimer> allTimers = new List<AtomicTimer>();
        private List<TimerSet> allSets = new List<TimerSet>();


        public string CurrentTimerName
        {
            get { return CurrentTimer.Name; }
            set { CurrentTimer.Name = value; OnPropertyChanged(); }
        }
        public string Duration
        {
            get { return CurrentTimer.Duration.ToString(); }
            set { duration = value; OnPropertyChanged(); }
        }
        public int Repetitions
        {
            get { return CurrentTimer.Repetitions; }
            set { repetitions = value; OnPropertyChanged(); }
        }

        public TimerPageViewModel()
        {
            //CurrentTimer = new DisplayTimer();
            //CurrentTimer.Duration = new TimeSpan(1, 2, 3);
            //CurrentTimer.Name = "Uebung01";
            //CurrentTimer.Repetitions = 5;

            //Duration = CurrentTimer.Duration.ToString();
            currentWorkout = AppCore.Workouts.First();
            var ct = currentWorkout.Timers.First().Timers.First();
            CurrentTimer = new DisplayTimer(ct);
            //Duration = CurrentTimer.Duration.ToString();

            //dummywerte weil zuerst ein finished event fliegt
            //allSets.Add(new TimerSet());
            //allTimers.Add(new AtomicTimer());
            foreach (var set in currentWorkout.Timers)
            {
                allSets.Add(set);
                foreach (var timer in set.Timers)
                {
                    allTimers.Add(timer);                 
                }
            }
            manager.ExerciseTimerElapsedEvent += manager_ExerciseTimerElapsedEvent;
            manager.ExerciseTimerFinishedEvent += Manager_ExerciseTimerFinishedEvent;
            manager.SetTimerElapsedEvent += Manager_SetTimerElapsedEvent;
            manager.SetTimerFinishedEvent += Manager_SetTimerFinishedEvent;
            manager.WorkoutTimerElapsedEvent += manager_WorkoutTimerElapsedEvent;
            manager.WorkoutTimerFinishedEvent += Manager_WorkoutTimerFinishedEvent;
        }
        public TimerPageViewModel(Workout workout)
        {
            currentWorkout = workout;
            
        }

        internal void StartNextTimer()
        {
            //var manager = new TimerManager();


        }


        public bool isRunning;

        internal void PauseTimer()
        {
            if(isRunning)
            {
                isRunning = false;

                manager.PauseTimer();
            }
        }

        internal void StartTimer()
        {
            if(!isRunning)
            {
                isRunning = true;

                manager.StartWorkoutAsync(currentWorkout);
            }

        }

        private void Manager_WorkoutTimerFinishedEvent(object sender, WorkoutFinishedEventArgs e)
        {
            manager.StopAllTimers();
            // throw new NotImplementedException();
        }

        private void Manager_SetTimerFinishedEvent(object sender, SetFinishedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Manager_SetTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            currentSetRepetitions++;
        }

        private void Manager_ExerciseTimerFinishedEvent(object sender, ExerciseFinishedEventArgs e)
        {
            //var nextDisplayTimer = new DisplayTimer(allTimers[exerciseIndex]);
            
            
         
            while (exerciseIndex <= allTimers.Count())
            {
              
                CurrentTimer = new DisplayTimer(allTimers[exerciseIndex]);
                Duration = CurrentTimer.Duration.ToString();
                CurrentTimerName = CurrentTimer.Name;
                //var blub = allTimers[0];
                //CurrentTimer.Duration = blub.Duration;
                //CurrentTimer.Repetitions = blub.Repetitions;
                //allTimers.RemoveAt(0);
                currentExerciseRepetitions++;
                exerciseIndex++;
            }

        }

        private void manager_WorkoutTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {

           
            //throw new NotImplementedException();
        }

        private void manager_ExerciseTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            CurrentTimer.Duration = CurrentTimer.Duration.Subtract(new TimeSpan(0,0,1));
            Duration = CurrentTimer.Duration.ToString();
           
            //throw new NotImplementedException();
        }




        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
