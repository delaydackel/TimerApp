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
        public event SetTimerElapsedHandler SetTimerElapsedEvent;
        public event WorkoutTimerElapsedHandler WorkoutTimerElapsedEvent;
        public event ExerciseTimerFinishedHandler ExerciseTimerFinishedEvent;
        public event SetTimerFinishedHandler SetTimerFinishedEvent;
        public event WorkoutTimerFinishedHandler WorkoutTimerFinishedEvent;

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
        public delegate void SetTimerElapsedHandler(object sender, ElapsedEventArgs e);
        protected void OnSetTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            SetTimerElapsedEvent(this, e);
        }
        public delegate void ExerciseTimerElapsedHandler(object sender, ElapsedEventArgs e);

        protected void OnWorkoutTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            WorkoutTimerElapsedEvent(this, e);
        }
        public delegate void WorkoutTimerElapsedHandler(object sender, ElapsedEventArgs e);


        protected void OnExerciseTimerFinishedEvent(object sender, ExerciseFinishedEventArgs e)
        {
            ExerciseTimerFinishedEvent(this, e);
        }
        public delegate void ExerciseTimerFinishedHandler(object sender, ExerciseFinishedEventArgs e);
        public delegate void SetTimerFinishedHandler(object sender, SetFinishedEventArgs e);
        protected void OnSetTimerFinishedEvent(object sender, SetFinishedEventArgs e)
        {
            SetTimerFinishedEvent(this, e);
        }


        protected void OnWorkoutTimerFinishedEvent(object sender, WorkoutFinishedEventArgs e)
        {
            WorkoutTimerFinishedEvent(this, e);
        }
        public delegate void WorkoutTimerFinishedHandler(object sender, WorkoutFinishedEventArgs e);




        public void StartWorkout(Workout workout)
        {
            this.currentWorkout = workout;
            TimeSpan workoutSpan= GetWorkoutSpan(workout);

            
        }

        private TimeSpan GetWorkoutSpan(Workout workout)
        {
            TimeSpan rvalue = new TimeSpan();
            foreach (var set in currentWorkout.Timers)
            {
                foreach (var timer in set.Timers)
                {
                    rvalue.Add(timer.Duration);
                }
            }
            return rvalue;

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
