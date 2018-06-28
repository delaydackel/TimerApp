using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimerApp.Model;

namespace TimerApp.ViewModel
{
    class TimerSetCreationPageViewModel
    {
        private List<TimerSet> timerSets;

        public string SetId { get; internal set; }
        public TimerSetCreationPageViewModel()
        {
            timerSets = new List<TimerSet>();
        }
        public TimerSetCreationPageViewModel(string workoutId):this()
        {

        }

        public void LoadTimerSets(string workoutId)
        {
            var dbMgr = new DatabaseManager();

            //TODO timerSets = dbMgr.readEntry(workoutId);
            if (timerSets.Count() == 0)
            {
                timerSets.Add(new TimerSet() { Name = "Item", Repetitions = 1, Timers = new List<AtomicTimer>() });
            }

        }
        public void SaveTimerSets(string name)
        {

        }
    }
}
