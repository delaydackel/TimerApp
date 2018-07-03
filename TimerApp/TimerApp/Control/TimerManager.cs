﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
        private Timer setTimer = new Timer();
        private Timer workoutTimer = new Timer();
        private TimeSpan workoutSpan = new TimeSpan();
        private TimeSpan setSpan = new TimeSpan();
        private TimeSpan exerciseSpan = new TimeSpan();
        private List<TimeSpan> setTimeSpans = new List<TimeSpan>();
        private List<TimeSpan> exerciseTimeSpans = new List<TimeSpan>();
        private Workout currentWorkout;
        public TimerManager()
        {
            exerciseTimer.Interval = 1000;
            setTimer.Interval = 1000;
            workoutTimer.Interval = 1000;
            exerciseTimer.Elapsed += OnExerciseTimerElapsedEvent;
            setTimer.Elapsed += OnSetTimerElapsedEvent;
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
            try
            {
                SetTimerFinishedEvent(this, e);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }


        protected void OnWorkoutTimerFinishedEvent(object sender, WorkoutFinishedEventArgs e)
        {
            WorkoutTimerFinishedEvent(this, e);
        }
        public delegate void WorkoutTimerFinishedHandler(object sender, WorkoutFinishedEventArgs e);




        public void StartWorkoutAsync(Workout workout)
        {
            try
            {
                this.currentWorkout = workout;
                TimeSpan workoutSpan = GetWorkoutSpan(workout);
                List<TimeSpan> setSpans = new List<TimeSpan>();
                foreach (var set in workout.Timers)
                {
                    for (int i = 0; i < set.Repetitions; i++)
                    {
                        TimeSpan currentSetSpan = new TimeSpan();
                        foreach (var exercise in set.Timers)
                        {
                            currentSetSpan = currentSetSpan.Add(exercise.Duration);
                            for (int j = 0; j < exercise.Repetitions; j++)
                            {
                                exerciseTimeSpans.Add(exercise.Duration);
                            }                            
                        }
                        setSpans.Add(currentSetSpan);
                        setTimeSpans.Add(currentSetSpan);
                    }                    
                }
                var exerciseTask = new Task(async () => {
                    while(_timerState == TimerState.RUNNING)
                    {
                        while (exerciseTimeSpans.Count > 0)
                        {
                            exerciseTimer.Start();
                            await Task.Delay(exerciseTimeSpans[0]);
                            exerciseTimeSpans.RemoveAt(0);
                            OnExerciseTimerFinishedEvent(this, new ExerciseFinishedEventArgs(true));
                        }
                    }                    
                    exerciseTimer.Stop();
                });
                var setTask = new Task(async () =>
                {
                    while (_timerState == TimerState.RUNNING)
                    {
                        while (setTimeSpans.Count > 0)
                        {
                            setTimer.Start();
                            await Task.Delay(setTimeSpans[0]);
                            setTimeSpans.RemoveAt(0);
                            OnSetTimerFinishedEvent(this, new SetFinishedEventArgs(true));
                        }
                    }                    
                    setTimer.Stop();
                });

                //TimeSpan setSpan = GetSetSpan();
                //int workoutSeconds = workoutSpan.TotalSeconds;

                var workoutTask = new Task(async () =>
                {
                    while (_timerState == TimerState.RUNNING)
                    {
                        await Task.Delay(workoutSpan);
                        OnWorkoutTimerFinishedEvent(this, new WorkoutFinishedEventArgs(true));
                    }                    
                    workoutTimer.Stop();
                });
                _timerState = TimerState.RUNNING;
                exerciseTask.Start();
                setTask.Start();
                workoutTimer.Start();
                workoutTask.Start();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private TimeSpan GetSetSpan(TimerSet set)
        {
            throw new NotImplementedException();
        }

        private TimeSpan GetWorkoutSpan(Workout workout)
        {
            TimeSpan rvalue = new TimeSpan();
            foreach (var set in currentWorkout.Timers)
            {
                foreach (var timer in set.Timers)
                {
                    rvalue=rvalue.Add(timer.Duration);
                }
            }
            return rvalue;

        }

        public void PauseTimer()
        {
            if (_timerState == TimerState.STOPPED)
            {
                _timerState = TimerState.RUNNING;
            }
            else
            {
                _timerState = TimerState.STOPPED;
            }            
        }

        public void SkipCurrentExercise()
        {
            throw new NotImplementedException();
        }
    }
}
