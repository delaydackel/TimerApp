using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TimerApp.Model
{
    public class AtomicTimer :BindableObject, INotifyPropertyChanged
    {
        private string name;
        private int repetitions;
        private TimeSpan duration;
        private bool isSelected;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value;
                OnPropertyChanged();
                OnPropertyChanged("DurationText");
            }
        }
        public string DurationText
        {
            get { return duration.ToString(); }
           
        }
        public int Repetitions
        {
            get { return repetitions; }
            set
            {
               // if (repetitions>1)
                {
                    repetitions = value; OnPropertyChanged();
                }
            }
        }
        public string RepetitionsText
        {
            get { return repetitions.ToString(); }
    
        }

        public bool IsSelected {
            get { return isSelected; }
            set { isSelected = value; OnPropertyChanged(); }
        }

        //public KeyCollection keys;
        public AtomicTimer()
        {
            Name = "Timer";
            Repetitions = 1;
            Duration = new TimeSpan(0, 0, 1);
            IsSelected = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        //void OnKeyAddedRemovedUpdated(string key)
        //{
        //    PropertyChanged(this, new PropertyChangedEventArgs("Item[" + key + "]");
        //}

        //    private TimeSpan duration;
        //    private string name;
        //    public string Name
        //    {
        //        get { return name; }
        //        set { name = value; }
        //    }
        //    public TimeSpan Duration
        //    {
        //        get { return duration; }
        //        set { duration = value; }
        //    }


    }
}
