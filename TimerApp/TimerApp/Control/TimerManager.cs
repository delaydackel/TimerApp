using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using TimerApp.Model;

namespace TimerApp.Control
{
    class TimerManager : ITimerManager
    {
        //ConcurrentBag<CancellationTokenSource> activeSources;
        public event ExerciseTimerElapsedHandler ExerciseTimerElapsedEvent;
        public event WorkoutTimerElapsedHandler WorkoutTimerElapsedEvent;
  
        private TimerState _timerState = TimerState.STOPPED;
        private Timer exerciseTimer = new Timer();
        private Timer workoutTimer = new Timer();

        private Workout currentWorkout;
        public TimerManager()
        {
            exerciseTimer.Interval = 1000;
            workoutTimer.Interval = 1000;
            exerciseTimer.Elapsed += OnExerciseTimerElapsedEvent;
            workoutTimer.Elapsed += OnWorkoutTimerElapsedEvent;

        }

        protected void OnExerciseTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            ExerciseTimerElapsedEvent(this, e);
        }
        public delegate void ExerciseTimerElapsedHandler(object sender, ElapsedEventArgs e);

        protected void OnWorkoutTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            WorkoutTimerElapsedEvent(this, e);
        }
        public delegate void WorkoutTimerElapsedHandler(object sender, ElapsedEventArgs e);


        public void StartWorkout(Workout workout)
        {
            this.currentWorkout = workout;
        }

        public void PauseTimer()
        {
            throw new NotImplementedException();
        }

        public void SkipCurrentExercise()
        {
            throw new NotImplementedException();
        }
    }
    
}
