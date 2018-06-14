using System;
using System.Collections.Generic;
using System.Text;

namespace TimerApp.Model
{
    class DisplayTimer
    {
        private string name;
        private TimeSpan duration;
        private uint repetitions;
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
        public uint Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }
    }
}
