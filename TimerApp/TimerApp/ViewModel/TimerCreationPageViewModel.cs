using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    class TimerCreationPageViewModel:INotifyPropertyChanged
    {
        private ObservableCollection<AtomicTimer> timerList;
        public ObservableCollection<TimerApp.Model.AtomicTimer> TimerList
        {
            get { return timerList; }
            set { timerList = value; OnPropertyChanged(); }
        }

        public TimerCreationPageViewModel()
        {
            timerList = new ObservableCollection<AtomicTimer>();
            var testTimer = new AtomicTimer();
            testTimer.Name = "asd";
            testTimer.Repetitions = 15;
            testTimer.Duration = new TimeSpan(2000);
            var testTimer2 = new AtomicTimer();
            testTimer2.Name = "asd";
            testTimer2.Repetitions = 15;
            testTimer2.Duration = new TimeSpan(2000);
            var testTimer3 = new AtomicTimer();
            testTimer3.Name = "asd";
            testTimer3.Repetitions = 15;
            testTimer3.Duration = new TimeSpan(2000);
            //testTimer["Name"] = "asd";
            //testTimer["Duration"] = new TimeSpan(2000);
            //testTimer["Repetitions"] = 15;
            TimerList.Add(testTimer);
            TimerList.Add(testTimer2);
            TimerList.Add(testTimer3);
            var blub = new TimerSet();
            blub.Timers = new List<AtomicTimer> { testTimer,testTimer2,testTimer3};
            


        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void AddTimer()
        {
            TimerList.Add(new AtomicTimer() { Name = "Timer",Repetitions=1});
         //   throw new NotImplementedException();
        }
    }
}
