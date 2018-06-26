using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using TimerApp.Model;

namespace TimerApp.Control
{
    class TimerManager : ITimerManager
    {
        private TimerState _timerState = TimerState.STOPPED;
        private Timer exerciseTimer = new Timer();
        private Timer workoutTimer = new Timer();
        public TimerManager()
        {
            exerciseTimer.Interval = 1000;
            workoutTimer.Interval = 1000;
            exerciseTimer.Elapsed += ExerciseTimerElapsedEvent;
            workoutTimer.Elapsed += WorkoutTimer_Elapsed;

        }

        private void WorkoutTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ExerciseTimerElapsedEvent(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void StartWorkout(Workout workout)
        {
            throw new NotImplementedException();
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
