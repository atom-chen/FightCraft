using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace GameBase
{
    public class TimeController : MonoBehaviour
    {
        void Start()
        {
            var date = System.DateTime.Now;
            Init();
            //InvokeRepeating("TimerInvake", 0.1f, 0.1f);
        }

        void Update()
        {
            CallBackUpdate();
        }

        #region TimeRecord

        //public delegate void TimerDelegate();

        /// <summary>
        /// 事件处理者列表
        /// </summary>        
        private Dictionary<TIMER_TYPE, Timer> _TimerList = new Dictionary<TIMER_TYPE, Timer>();
        //private Dictionary<TIMER_TYPE, Timer> _ActiveHandleList = new Dictionary<TIMER_TYPE, Timer>();

        public void Init()
        {
            Load();
            //InitEventManager();
            //RegisterEvent();            
        }

        public Timer RegisterTimer(TIMER_TYPE type)
        {
            Timer timer;
            if (!_TimerList.ContainsKey(type))
            {
                timer = new Timer();
                timer.TimerType = type;
                _TimerList.Add(type, timer);
            }
            else
            {
                timer = _TimerList[type];
            }

            //timer.CallBack = handle;
            //_TimeEventController.RegisteEvent(((EVENT_TYPE)((int)EVENT_TYPE.EVENT_TIMER_START + (int)type)), handle);
            return timer;
        }

        public void UnRegisterTimer(TIMER_TYPE type)
        {
            if (_TimerList.ContainsKey(type))
            {
                Timer timer = _TimerList[type];
                //_TimeEventController.UnRegisteEvent(((EVENT_TYPE)((int)EVENT_TYPE.EVENT_TIMER_START + (int)type)), timer.CallBack);
                _TimerList.Remove(type);
            }
        }

        public void SetTimer(TIMER_TYPE type, float second, DateTime startTime)
        {
            Timer timer;
            if (_TimerList.ContainsKey(type))
            {
                timer = _TimerList[type];
            }
            else
            {
                timer = RegisterTimer(type);
            }

            timer.LastDate = startTime;
            timer.Delta = TimeSpan.FromSeconds(second);

            //Invoke("TimerInvake", second);
        }

        public void SetTimer(TIMER_TYPE type, float second)
        {
            Timer timer;
            if (_TimerList.ContainsKey(type))
            {
                timer = _TimerList[type];                
            }
            else
            {
                timer = RegisterTimer(type);
            }

            timer.LastDate = System.DateTime.Now;
            timer.Delta = TimeSpan.FromSeconds(second);

            //Invoke("TimerInvake", second);
        }

        public void StopTimer(TIMER_TYPE type)
        {
            if (_TimerList.ContainsKey(type))
            {
                _TimerList[type].LastDate = new DateTime(0);
                _TimerList.Remove(type);
            }
        }

        public System.TimeSpan GetTimeDelay(TIMER_TYPE type)
        {
            if (_TimerList.ContainsKey(type))
            {
                var date = System.DateTime.Now - _TimerList[type].LastDate;
                return date;
            }
            return System.TimeSpan.Zero;
        }

        public System.TimeSpan GetTimeLast(TIMER_TYPE type)
        {
            if (_TimerList.ContainsKey(type))
            {
                var date = System.DateTime.Now - _TimerList[type].LastDate;
                if (date < _TimerList[type].Delta)
                {
                    return _TimerList[type].Delta - date;
                }
                return System.TimeSpan.Zero;
            }
            return System.TimeSpan.Zero;
        }

        public DateTime GetLastDate(TIMER_TYPE type)
        {
            if (_TimerList.ContainsKey(type))
            {
                return _TimerList[type].LastDate;
            }
            return System.DateTime.Now;
        }

        #endregion

        #region SpecilTime

        public bool IsTodayFirstLogin()
        {
            var lastDate = GetLastDate(TIMER_TYPE.TIMER_LOGIN);
            if (lastDate.Day != System.DateTime.Today.Day)
            {
                SetTimer(TIMER_TYPE.TIMER_LOGIN, -1);
                return true;
            }
            return false;
        }

        #endregion

        #region CallBack


        public delegate void TimeCallBackFun();

        public class TimeCallBackInfo
        {
            public TimeCallBackFun _CallBackFun;
            public float _Second;
            public float _StartTime;
        }

        private List<TimeCallBackInfo> _TimeCallBacks = new List<TimeCallBackInfo>();
        private List<TimeCallBackInfo> _TimeToDelete = new List<TimeCallBackInfo>();

        public void SetCallBack(TimeCallBackFun callBack, float seconds)
        {
            _TimeCallBacks.Add(new TimeCallBackInfo() { _CallBackFun = callBack, _Second = seconds, _StartTime = Time.time });

            //Invoke("CallBack", seconds);
        }

        public void CallBackUpdate()
        {
            foreach (var timeInfo in _TimeCallBacks)
            {
                if (Time.time - timeInfo._StartTime >= timeInfo._Second)
                {
                    try
                    {
                        timeInfo._CallBackFun();
                    }
                    catch (Exception e)
                    {
                        LogManager.LogError("TimeCallBackError:" + e.ToString());
                    }
                    _TimeToDelete.Add(timeInfo);
                }
            }

            foreach (var timeInfo in _TimeToDelete)
            {
                _TimeCallBacks.Remove(timeInfo);
            }

            _TimeToDelete.Clear();
        }

        #endregion

        #region save/load

        public void Save()
        {
            string packDataStr = "";
            foreach (var handle in _TimerList)
            {
                packDataStr += handle.Value.GetSaveStr() + ";";
            }
            LocalSave.Save(LocalSaveType.TIMER_PACK, packDataStr);
        }

        public void Load()
        {
            _TimerList.Clear();

            string packDataStr = LocalSave.LoadString(LocalSaveType.TIMER_PACK);
            string[] dataStrs = packDataStr.Split(';');
            foreach (string dataStr in dataStrs)
            {
                Timer timer = Timer.LoadFromStr(dataStr);

                if (timer != null)
                {
                    _TimerList.Add(timer.TimerType, timer);
                }
            }
        }

        public void Clear()
        {
            _TimerList.Clear();
        }
        #endregion
    }
}
