
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private string name;
        private string duration;
        private uint repetitions;
        
        //DownloadManager multiDownloadManager;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Duration
        {
            get { return duration; }
            set { duration = value; }
        }
        public uint Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }

        public TimerPageViewModel()
        {
            CurrentTimer = new DisplayTimer();
            CurrentTimer.Duration = new TimeSpan(1, 2, 3);
            CurrentTimer.Name = "Uebung01";
            CurrentTimer.Repetitions = 5;

            Duration = CurrentTimer.Duration.ToString();
            currentWorkout = new Workout()
            {
                Playlist = string.Empty,
                Timers = new List<TimerSet>() { new TimerSet() { Name = "test",
                    Timers = new List<AtomicTimer> {
                     new AtomicTimer()
                     {
                        Name = "asd",
                        Repetitions = 1,
                        Duration = new TimeSpan(1,2,2000)
                        },
                        new AtomicTimer()
                        {
                                Name = "asd",
                                Repetitions = 15,
                                Duration = new TimeSpan(2000)
                        }
            } } }

            };

            
        }

        

        public TimerPageViewModel(Workout workout)
        {
            currentWorkout = workout;
            
        }

        internal void StartNextTimer()
        {
            var manager = new TimerManager();
            manager.StartWorkoutAsync(currentWorkout);
            
            manager.ExerciseTimerElapsedEvent += manager_ExerciseTimerElapsedEvent;
            manager.ExerciseTimerFinishedEvent += Manager_ExerciseTimerFinishedEvent;
            manager.SetTimerElapsedEvent += Manager_SetTimerElapsedEvent;
            manager.SetTimerFinishedEvent += Manager_SetTimerFinishedEvent;
            manager.WorkoutTimerElapsedEvent += manager_WorkoutTimerElapsedEvent;            
            manager.WorkoutTimerFinishedEvent += Manager_WorkoutTimerFinishedEvent;
            
            
        }

        private void Manager_SetTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Manager_WorkoutTimerFinishedEvent(object sender, WorkoutFinishedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Manager_SetTimerFinishedEvent(object sender, SetFinishedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void Manager_ExerciseTimerFinishedEvent(object sender, ExerciseFinishedEventArgs e)
        {
            CurrentTimer = new DisplayTimer() { Duration = new TimeSpan(2, 3, 4), Name = "DOENER", Repetitions = 5 };//dernächstetimer im timerset
            //throw new NotImplementedException();
        }

        private void manager_WorkoutTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            
            
            //throw new NotImplementedException();
        }

        private void manager_ExerciseTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            CurrentTimer.Duration.Subtract(new TimeSpan(0,0,1));
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
