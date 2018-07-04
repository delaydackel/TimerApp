using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TimerApp.Model;

namespace TimerApp.Control
{
    class TimerManager : ITimerManager
    {
          ConcurrentBag<CancellationTokenSource> activeSources;
        ConcurrentBag<Task> tasks;
        //ConcurrentBag<CancellationTokenSource> activeSources;
        public event ExerciseTimerElapsedHandler ExerciseTimerElapsedEvent;
        public event SetTimerElapsedHandler SetTimerElapsedEvent;
        public event WorkoutTimerElapsedHandler WorkoutTimerElapsedEvent;
        public event ExerciseTimerFinishedHandler ExerciseTimerFinishedEvent;
        public event SetTimerFinishedHandler SetTimerFinishedEvent;
        public event WorkoutTimerFinishedHandler WorkoutTimerFinishedEvent;



        private TimerState _timerState = TimerState.STOPPED;
        private System.Timers.Timer exerciseTimer = new System.Timers.Timer();
        private System.Timers.Timer setTimer = new System.Timers.Timer();
        private System.Timers.Timer workoutTimer = new System.Timers.Timer();
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

            this.activeSources = new ConcurrentBag<CancellationTokenSource>();
            tasks = new ConcurrentBag<Task>();

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

                CancellationTokenSource src = new CancellationTokenSource();
               

                activeSources.Add(src);

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
                CancellationToken ct = src.Token;
                var exerciseTask = Task.Run(() => {
                    RunExercise(ct);
                },ct);
                
                var setTask = Task.Run(() =>
                {
                    while (_timerState == TimerState.RUNNING)
                    {
                        while (setTimeSpans.Count > 0)
                        {
                            if (ct.IsCancellationRequested)
                            {
                                ct.ThrowIfCancellationRequested();
                            }
                            setTimer.Start();
                            Task.Delay(setTimeSpans[0]).Wait();
                            setTimeSpans.RemoveAt(0);
                            OnSetTimerFinishedEvent(this, new SetFinishedEventArgs(true));
                        }
                    }                    
                    setTimer.Stop();
                },ct);

                //TimeSpan setSpan = GetSetSpan();
                //int workoutSeconds = workoutSpan.TotalSeconds;

                var workoutTask = Task.Run(() =>
                {
                    while (_timerState == TimerState.RUNNING)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            ct.ThrowIfCancellationRequested();
                        }
                        workoutTimer.Start();
                        Task.Delay(workoutSpan).Wait();
                        OnWorkoutTimerFinishedEvent(this, new WorkoutFinishedEventArgs(true));
                    }                    
                    workoutTimer.Stop();
                },ct);
                _timerState = TimerState.RUNNING;
                //workoutTask.ContinueWith(t => { int x = 0; });
                //exerciseTask.ContinueWith(t => { int x = 0; });
                //setTask.ContinueWith(t => { int x = 0; });
                //Task.WaitAll(new[] { exerciseTask, setTask, workoutTask});
                //Task.Run(() => exerciseTask);
                //Task.Run(() => workoutTask);
                //Task.Run(() => setTask);
                //exerciseTask.Start();
                //setTask.Start();               
                //workoutTask.Start();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RunExercise( CancellationToken ct)
        {
            while (_timerState == TimerState.RUNNING)
            {
                while (exerciseTimeSpans.Count > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    exerciseTimer.Start();
                    Task.Delay(exerciseTimeSpans[0]).Wait();
                    exerciseTimeSpans.RemoveAt(0);
                    OnExerciseTimerFinishedEvent(this, new ExerciseFinishedEventArgs(true));
                }
            }
            exerciseTimer.Stop();
        }

        internal void StopAllTimers()
        {
            PauseTimer();
            this.workoutTimer.Stop();
            this.setTimer.Stop();
            this.exerciseTimer.Stop();
          //  throw new NotImplementedException();
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
                try
                {
                    foreach (CancellationTokenSource src in activeSources)
                    {
                        src.Cancel();
                    }
                    Task.WaitAll(tasks.ToArray());
                }
                catch (AggregateException ae)
                {
                    if (!(ae.InnerException.GetType() == typeof(TaskCanceledException)))
                    {
                        throw ae;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    tasks = new ConcurrentBag<Task>();
                    activeSources = new ConcurrentBag<CancellationTokenSource>();
                }
                exerciseTimer.Stop();
                setTimer.Stop();
                workoutTimer.Stop();
            }            
        }

        public void SkipCurrentExercise()
        {
            throw new NotImplementedException();
        }
        public async Task StartExerciseTimer(CancellationToken ct)
        {

        }
    }
}
