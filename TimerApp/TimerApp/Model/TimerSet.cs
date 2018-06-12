using System;
using System.Collections.Generic;
using System.Text;

namespace TimerApp.Model
{
    class TimerSet
    {
        private List<AtomicTimer> elements;
        private uint repetitions;
        public List<AtomicTimer> Elements
        {
            get { return elements; }
            set { elements = value; }
        }
        public uint Repetitions
        {
            get { return repetitions; }
            set { repetitions = value; }
        }
    }
}
