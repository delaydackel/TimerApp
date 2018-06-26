using System;
using System.Collections.Generic;
using System.Text;
using TimerApp.Model;

namespace TimerApp.Control
{
    interface ITimerManager
    {
        void StartWorkout(Workout workout);
        void PauseTimer();
        void SkipCurrentExercise();
    }
}
