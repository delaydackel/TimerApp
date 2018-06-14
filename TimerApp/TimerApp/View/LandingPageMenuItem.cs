using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimerApp.View
{

    public class LandingPageMenuItem
    {
        public LandingPageMenuItem()
        {
            //TargetType = typeof(LandingPageDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}