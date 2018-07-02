using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TimerApp.Control;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    class WorkoutCreationPageViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<Workout> workouts;
        public ObservableCollection<Workout> WorkoutsCollection { get { return workouts; }set { workouts = value;OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public WorkoutCreationPageViewModel()
        {
            WorkoutsCollection = new ObservableCollection<Workout>();
       
            
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void LoadWorkouts()
        {
            WorkoutsCollection.Clear();// = new ObservableCollection<Workout>();
            foreach (var item in (AppCore.Workouts))
            {
                WorkoutsCollection.Add(item);
            }
        }
    }
}
