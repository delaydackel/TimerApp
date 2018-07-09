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
        private TimerManager manager;
        private string currentTimerName;
        private string duration;
        private int repetitions;
        private int exerciseIndex;
        private int setIndex;
        private int currentExerciseRepetitions = 0;
        private int currentSetRepetitions = 0;
        private List<AtomicTimer> allTimers = new List<AtomicTimer>();
        private List<TimerSet> allSets = new List<TimerSet>();
        private string setDuration;
        private string workoutDuration;
        private TimeSpan remainingSetSpan;
        private TimeSpan remainingWorkoutSpan;


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
        public string SetDuration
        {
            get { return remainingSetSpan.ToString(); }
            set { setDuration = value; OnPropertyChanged(); }
        }
        public string WorkoutDuration
        {
            get { return remainingWorkoutSpan.ToString(); }
            set { workoutDuration = value; OnPropertyChanged(); }
        }
        public string SetName
        {
            get { return setName; }
            set { setName = value; OnPropertyChanged(); }
        }
        public string WorkoutName
        {
            get { return workoutName; }
            set { workoutName = value; OnPropertyChanged(); }
        }
        public TimeSpan RemainingWorkoutSpan
        {
            get { return remainingWorkoutSpan; }
            set { remainingWorkoutSpan = value; OnPropertyChanged();OnPropertyChanged("WorkoutDuration"); }
        }
        public  TimeSpan RemainingSetSpan
        {
            get { return remainingSetSpan; }
            set { remainingSetSpan = value; OnPropertyChanged();OnPropertyChanged("SetDuration"); }
        }
        public TimerPageViewModel()
        {
            exerciseIndex = 0;
            setIndex = 0;
            currentWorkout = AppCore.CurrentWorkout;//AppCore.Workouts.First();
            WorkoutName = currentWorkout.Name;
            setName = currentWorkout.Timers.First().Name;
            var ct = currentWorkout.Timers.First().Timers.First();
            
            CurrentTimer = new DisplayTimer(ct);
            //Duration = CurrentTimer.Duration.ToString();
            manager = new TimerManager();
            
            remainingWorkoutSpan = new TimeSpan();
            remainingWorkoutSpan =currentWorkout.Duration;
            workoutDuration = remainingWorkoutSpan.ToString();
            remainingSetSpan = new TimeSpan();
            remainingSetSpan = currentWorkout.Timers.First().Duration;//manager.GetSetSpan();
            setDuration = remainingSetSpan.ToString();
            //dummywerte weil zuerst ein finished event fliegt
            //allSets.Add(new TimerSet());
            //allTimers.Add(new AtomicTimer());
            foreach (var set in currentWorkout.Timers)
            {
                for (int i = 0; i < set.Repetitions; i++)
                {
                    allSets.Add(set);
                    foreach (var timer in set.Timers)
                    {
                        for (int j = 0; j < timer.Repetitions; j++)
                        {
                            allTimers.Add(timer);
                        }
                    }
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
        private string workoutName;
        private string setName;

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
                //manager = new TimerManager();
                isRunning = true;
                exerciseIndex = 0;
                manager.StartWorkoutAsync();
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
#if DEBUG
            Console.WriteLine( "exercise finished " + allTimers[exerciseIndex].Name);
#endif
            if (exerciseIndex <= allTimers.Count())
            {
#if DEBUG
                Console.WriteLine("number "+exerciseIndex.ToString());
#endif
       
                //var blub = allTimers[0];
                //CurrentTimer.Duration = blub.Duration;
                //CurrentTimer.Repetitions = blub.Repetitions;
                //allTimers.RemoveAt(0);            
               
            }

        }

        private void manager_WorkoutTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
#if DEBUG
            Console.WriteLine("workout ticked ");
#endif
            //modify currently displayed exercise timer
            CurrentTimer.Duration = CurrentTimer.Duration.Subtract(new TimeSpan(0, 0, 1));

            if (CurrentTimer.Duration != new TimeSpan(0, 0, -1))
            {
                Duration = CurrentTimer.Duration.ToString();
            }
                
            else// (CurrentTimer.Duration == new TimeSpan(0,0,-1))
            {
                exerciseIndex++;
                CurrentTimer = new DisplayTimer(allTimers[exerciseIndex]);
                Duration = CurrentTimer.Duration.ToString();
                CurrentTimerName = CurrentTimer.Name;
            }
            //modify currently displayed set timer
            if (RemainingSetSpan != new TimeSpan(0, 0, -1))
            {
                RemainingSetSpan = RemainingSetSpan.Subtract(new TimeSpan(0, 0, 1));
            }                
            else// (remainingSetSpan == new TimeSpan(0, 0, -1))
            {
                setIndex++;
                RemainingSetSpan = allSets[setIndex].Duration;//manager.GetSetSpan(allSets[setIndex]);
                SetName = allSets[setIndex].Name;
            }
            //modify currently displayed workout timer

            if(RemainingWorkoutSpan != new TimeSpan(0))
            {
                RemainingWorkoutSpan = RemainingWorkoutSpan.Subtract(new TimeSpan(0, 0, 1));
            }            
        }

        private void manager_ExerciseTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
    
           
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
