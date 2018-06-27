﻿using System;
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
            set { repetitions = value; OnPropertyChanged(); }
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
        public AtomicTimer():base()
        {
            //this.Add("Name",string.Empty);
            //this.Add("Duration", new TimeSpan(0));
            //this.Add("Repetitions", 0);
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
