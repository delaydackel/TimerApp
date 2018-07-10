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
        
        //SERIOUSLY?!?!?!
        //Even if the SynchronizingObject property is not null, Elapsed events can occur after the Dispose or Stop method has been called or after the 
        //Enabled property has been set to false, because the signal to raise the Elapsed event is always queued for execution on a thread pool thread. 
        //    One way to resolve this race condition is to set a flag that tells the event handler for the Elapsed event to ignore subsequent events.
        // https://msdn.microsoft.com/de-de/library/system.timers.timer.synchronizingobject(v=vs.110).aspx
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
        private System.Timers.Timer currentExerciseTimer = new System.Timers.Timer();
        private System.Timers.Timer currentSetTimer = new System.Timers.Timer();
        private System.Timers.Timer currentWorkoutTimer = new System.Timers.Timer();
        private TimeSpan workoutSpan = new TimeSpan();
        private TimeSpan setSpan = new TimeSpan();
        private TimeSpan exerciseSpan = new TimeSpan();
        private List<TimeSpan> setTimeSpans = new List<TimeSpan>();
        private List<TimeSpan> exerciseTimeSpans = new List<TimeSpan>();
        private Workout currentWorkout;
        private int currentExerciseIndex;
        private int currentSetIndex;

        public TimerManager()
        {
            currentExerciseIndex = 0;
            currentSetIndex = 0;

            exerciseTimer.Interval = 1000;
            setTimer.Interval = 1000;
            workoutTimer.Interval = 1000;
            exerciseTimer.Elapsed += OnExerciseTimerElapsedEvent;
            setTimer.Elapsed += OnSetTimerElapsedEvent;
            workoutTimer.Elapsed += OnWorkoutTimerElapsedEvent;

            currentExerciseTimer.Elapsed += (sender, e) => OnExerciseTimerFinishedEvent(this, new ExerciseFinishedEventArgs());
            currentSetTimer.Elapsed += (sender, e) => OnSetTimerFinishedEvent(this, new SetFinishedEventArgs());
            currentWorkoutTimer.Elapsed += (sender, e) => OnWorkoutTimerFinishedEvent(this, new WorkoutFinishedEventArgs());

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
            //currentExerciseTimer.Dispose();
            //currentExerciseTimer = new System.Timers.Timer();
      
            ExerciseTimerFinishedEvent(this, e);
            currentExerciseTimer.Elapsed += (s, arg) => OnExerciseTimerFinishedEvent(this, new ExerciseFinishedEventArgs());
            currentExerciseIndex++;
            currentExerciseTimer.Interval = exerciseTimeSpans[currentExerciseIndex].Duration().TotalSeconds * 1000;
            
        }
        public delegate void ExerciseTimerFinishedHandler(object sender, ExerciseFinishedEventArgs e);
        public delegate void SetTimerFinishedHandler(object sender, SetFinishedEventArgs e);
        protected void OnSetTimerFinishedEvent(object sender, SetFinishedEventArgs e)
        {
            try
            {              
                SetTimerFinishedEvent(this, e);

                currentSetIndex++;
                currentSetTimer.Interval = setTimeSpans[currentSetIndex].Duration().TotalSeconds * 1000;
                
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
        public async void StartWorkoutAsync()
        {
            try
            {
                this.currentWorkout = AppCore.CurrentWorkout;
                TimeSpan workoutSpan = GetWorkoutSpan(currentWorkout);
                List<TimeSpan> setSpans = new List<TimeSpan>();

                CancellationTokenSource src = new CancellationTokenSource();
               

                activeSources.Add(src);

                foreach (var set in currentWorkout.Timers)
                {
                    for (int i = 0; i < set.Repetitions; i++)
                    {
                        TimeSpan currentSetSpan = new TimeSpan();
                        foreach (var exercise in set.Timers)
                        {                           
                            for (int j = 0; j < exercise.Repetitions; j++)
                            {
                                currentSetSpan = currentSetSpan.Add(exercise.Duration);
                                //currentSetSpan = currentSetSpan.Add(new TimeSpan(0,0,1));//hax
                                exerciseTimeSpans.Add(exercise.Duration);
                            }                            
                        }
                        setSpans.Add(currentSetSpan);
                        setTimeSpans.Add(currentSetSpan);
                    }                    
                }
                CancellationToken ct = src.Token;
              
                currentSetTimer= new System.Timers.Timer();

            
                currentSetTimer.Elapsed += (sender, e) => OnSetTimerFinishedEvent(this, new SetFinishedEventArgs());
                currentWorkoutTimer.Elapsed += (sender, e) => OnWorkoutTimerFinishedEvent(this, new WorkoutFinishedEventArgs());

                //setze auf ersten timer
                currentExerciseTimer.Interval = exerciseTimeSpans[0].Duration().TotalSeconds * 1000;
                currentSetTimer.Interval = setTimeSpans[0].Duration().TotalSeconds * 1000;
                currentWorkoutTimer.Interval = workoutSpan.Duration().TotalSeconds * 1000;

                StartAllTimers();
                _timerState = TimerState.RUNNING;
                //var exerciseTask = await Task<bool>.Run(() => {
                //    return RunExercise(ct);
                //},ct);

                //var setTask = await Task<bool>.Run(() =>
                //{
                //    while (_timerState == TimerState.RUNNING)
                //    {
                //        while (setTimeSpans.Count > 0)
                //        {
                //            if (ct.IsCancellationRequested)
                //            {
                //                ct.ThrowIfCancellationRequested();
                //            }
                //            //setTimer.Start();
                //            Task.Delay(setTimeSpans[0]).Wait();
                //            setTimeSpans.RemoveAt(0);
                //            OnSetTimerFinishedEvent(this, new SetFinishedEventArgs(true));
                //        }
                //    }                    
                //    setTimer.Stop();
                //    return true;
                //},ct);

                ////TimeSpan setSpan = GetSetSpan();
                ////int workoutSeconds = workoutSpan.TotalSeconds;

                //var workoutTask = await Task<bool>.Run(() =>
                //{
                //    while (_timerState == TimerState.RUNNING)
                //    {
                //        if (ct.IsCancellationRequested)
                //        {
                //            ct.ThrowIfCancellationRequested();
                //        }
                //        //workoutTimer.Start();
                //        Task.Delay(workoutSpan).Wait();
                //        OnWorkoutTimerFinishedEvent(this, new WorkoutFinishedEventArgs(true));
                //    }                    
                //    workoutTimer.Stop();
                //    return true;
                //},ct);
                //exerciseTimer.Start();
                //workoutTimer.Start();
                //setTimer.Start();
                //_timerState = TimerState.RUNNING;
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

        private void StartAllTimers()
        {
            workoutTimer.Start();
            setTimer.Start();
            exerciseTimer.Start();
            currentWorkoutTimer.Start();
            currentSetTimer.Start();
            currentExerciseTimer.Start();
        }

        private bool RunExercise( CancellationToken ct)
        {
            while (_timerState == TimerState.RUNNING)
            {
                while (exerciseTimeSpans.Count > 0)
                {
                    if (ct.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                    }
                    //exerciseTimer.Start();
                    Task.Delay(exerciseTimeSpans[0]).Wait();
                    exerciseTimeSpans.RemoveAt(0);
                    OnExerciseTimerFinishedEvent(this, new ExerciseFinishedEventArgs());
                }
            }
            exerciseTimer.Stop();
            return true;
        }

        internal void StopAllTimers()
        {
            // PauseTimer();
            workoutTimer.Stop();
            setTimer.Stop();
            exerciseTimer.Stop();
            currentWorkoutTimer.Stop();
            currentSetTimer.Stop();
            currentExerciseTimer.Stop();
            workoutTimer.Dispose();
            setTimer.Dispose();
            exerciseTimer.Dispose();
            currentWorkoutTimer.Dispose();
            currentSetTimer.Dispose();
            currentExerciseTimer.Dispose();
            //  throw new NotImplementedException();
        }

        private TimeSpan GetSetSpan(TimerSet set)
        {
            TimeSpan rvalue = new TimeSpan();
           
            for (int i = 0; i < set.Repetitions; i++)
            {
                foreach (var timer in set.Timers)
                {
                    for (int j = 0; j < timer.Repetitions; j++)
                    {
                        rvalue = rvalue.Add(timer.Duration);
                        //rvalue = rvalue.Add(new TimeSpan(0, 0, 1));//hax
                    }
                }
            }
            
            return rvalue;
        }

        private TimeSpan GetWorkoutSpan(Workout workout)
        {
            TimeSpan rvalue = new TimeSpan();
            foreach (var set in currentWorkout.Timers)
            {
                for (int i = 0; i < set.Repetitions; i++)
                {
                    foreach (var timer in set.Timers)
                    {
                        for (int j = 0; j < timer.Repetitions; j++)
                        {
                            rvalue = rvalue.Add(timer.Duration);
                            //rvalue = rvalue.Add(new TimeSpan(0,0,1));//hax
                        }
                        
                    }
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
                StopAllTimers();
                //try
                //{
                //    foreach (CancellationTokenSource src in activeSources)
                //    {
                //        src.Cancel();
                //    }
                //    Task.WaitAll(tasks.ToArray());
                //}
                //catch (AggregateException ae)
                //{
                //    if (!(ae.InnerException.GetType() == typeof(TaskCanceledException)))
                //    {
                //        throw ae;
                //    }
                //}
                //catch (Exception ex)
                //{
                //    throw ex;
                //}
                //finally
                //{
                //    tasks = new ConcurrentBag<Task>();
                //    activeSources = new ConcurrentBag<CancellationTokenSource>();
                //}
                //exerciseTimer.Stop();
                //setTimer.Stop();
                //workoutTimer.Stop();
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
