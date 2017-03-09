using UnityEngine;
using System.Collections;
using System;

namespace GameBase
{
    public enum TIMER_TYPE
    {
        NONE,
        TIMER_TILI,
        TIMER_LOGIN,
    }

    public class Timer
    {
        public TIMER_TYPE TimerType;
        public System.DateTime LastDate;
        public TimeSpan Delta;
        public EventController.EventDelegate CallBack;
        public bool IsActive;
        public int LastSecond;

        public bool IsTimerNoDate()
        {
            if (LastDate.Ticks == 0)
                return true;
            return false;
        }

        public string GetSaveStr()
        {
            
            TimeSpan timeSpan = System.DateTime.Now - LastDate;
            LastSecond = (int)timeSpan.TotalSeconds;
            string saveStr = ((int)(TimerType)).ToString() + "," + LastDate.ToString() + "," + Delta.ToString() + "," + LastSecond;
            return saveStr;
        }

        public static Timer LoadFromStr(string saveDate)
        {
            Timer timer = new Timer();
            int type;
            int lastSecond;
            string[] valueStrs = saveDate.Split(',');
            if (valueStrs.Length == 4 
                && int.TryParse(valueStrs[0], out type)
                && int.TryParse(valueStrs[3], out lastSecond))
            {
                timer.TimerType = (TIMER_TYPE)type;
                timer.LastDate = System.DateTime.Parse(valueStrs[1]);
                timer.Delta = System.TimeSpan.Parse(valueStrs[2]);
                timer.LastSecond = lastSecond;
                return timer;
            }

            return null;
        }
    }
}
