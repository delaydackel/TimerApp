using System;
using System.Collections.Generic;
using System.Text;

namespace TimerApp.Control
{
    interface IAppService
    {
        void createTimer();
        void removeTimer();
        void saveTimer();
        void loadTimer();
    }

}
