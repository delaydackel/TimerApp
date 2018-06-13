using System;
using System.Collections.Generic;
using System.Text;

namespace TimerApp.Model
{
    public class AtomicTimer : Dictionary<string, object>
    {
        //public KeyCollection keys;
        public AtomicTimer():base()
        {
            this.Add("Name",string.Empty);
            this.Add("Duration", new TimeSpan());
            this.Add("Repetitions", 0);

                
        }

    
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
