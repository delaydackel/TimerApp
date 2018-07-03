using System;
using System.Collections.Generic;
using System.Text;
using TimerApp.Model;

namespace TimerApp.Control
{
    public class AppCore
    {
        private  static List<Workout> workouts;
        private static string currentWorkoutId;
        private static Workout currentWorkout;
        public static string CurrentWorkoutId { get { return currentWorkoutId; } set { currentWorkoutId = value; } }
        public static List<Workout> Workouts { get { return workouts; } set { workouts = value; } }
        public static Workout CurrentWorkout { get { return currentWorkout; } set { currentWorkout = value; } }
        private AppCore()
        {
            CurrentWorkoutId = ""; //könnte man in einstellungen speichern, welches zuletzt gewählt wurde
            Workouts = new List<Workout>();
            CurrentWorkout = new Workout();
            LoadData();
        }
        public static AppCore StartAppCore()
        {
            return new AppCore();
        }

        private static void LoadData()
        {
            try
            {
                //hole alle workouts aus DB
                var dbMgr = new DatabaseManager();
                var wo = dbMgr.LoadWorkouts();
                if (wo != null)
                {
                    Workouts = wo;
                    CurrentWorkout = wo[0];
                }
                else
                {
                    Workouts = new List<Workout>()
                    {
                        new Workout()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Playlist = string.Empty,
                            Timers = new List<TimerSet>() {
                                new TimerSet(){
                                    SetId = Guid.NewGuid().ToString(),
                                    Name = "neues Set",
                                    Repetitions = 1,
                                    Timers = new List<AtomicTimer>()
                                    {
                                        new AtomicTimer(){Name = "Timer",Repetitions = 1, Duration = new TimeSpan(0,0,1)}
                                    }
                                }

                            },
                        Name = "Workout"

                    } };

                }
                CurrentWorkout = Workouts[0];
                
                //throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
