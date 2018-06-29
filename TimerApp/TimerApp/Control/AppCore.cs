using System;
using System.Collections.Generic;
using System.Text;
using TimerApp.Model;

namespace TimerApp.Control
{
    static class AppCore
    {
        private  static List<Workout> workouts;
        private static string currentWorkoutId;
        private static Workout currentWorkout;
        public static string CurrentWorkoutId { get { return currentWorkoutId; } set { currentWorkoutId = value; } }
        public static List<Workout> Workouts { get { return workouts; } set { workouts = value; } }
        public static Workout CurrentWorkout { get { return currentWorkout; } set { currentWorkout = value; } }
        static AppCore()
        {
            CurrentWorkoutId = ""; //könnte man in einstellungen speichern, welches zuletzt gewählt wurde
            Workouts = new List<Workout>();
            CurrentWorkout = new Workout();
            LoadData();
        }

        private static void LoadData()
        {
            //hole alle workouts aus DB
            //throw new NotImplementedException();
        }
    }
}
