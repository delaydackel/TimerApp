using System;
using System.Timers;

namespace TimerApp.Control
{
    public class FinishedEventArgs : EventArgs
    {
        public DateTime SignalTime { get; set; }
    }
}
