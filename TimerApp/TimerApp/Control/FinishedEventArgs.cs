using System;

namespace TimerApp.Control
{
    public class FinishedEventArgs : EventArgs
    {
        private bool finished;
        public FinishedEventArgs(bool p_finished)
        {
            this.finished = p_finished;
        }
        public bool Finished
        {
            get { return finished; }
        }
    }
}
