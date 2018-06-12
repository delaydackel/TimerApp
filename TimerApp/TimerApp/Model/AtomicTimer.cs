using System;
using System.Collections.Generic;
using System.Text;

namespace TimerApp.Model
{
    class AtomicTimer
    {
        private TimeSpan duration;
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public TimeSpan Duration
        {
            get { return duration; }
            set { duration = value; }
        }
    }
}
