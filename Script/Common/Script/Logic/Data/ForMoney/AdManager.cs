using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdManager
{
    #region 

    private static AdManager _Instance;

    public static AdManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new AdManager();
            }
            return _Instance;
        }
    }

    #endregion

    #region ad video

    private Action _AdVideoCallBack;

    public void WatchAdVideo(Action finishCallBack)
    {
        //todo
        _AdVideoCallBack = finishCallBack;
        WatchAdVideoFinish();
    }

    public void WatchAdVideoFinish()
    {
        if (_AdVideoCallBack != null)
        {
            _AdVideoCallBack.Invoke();
        }
    }

    #endregion
}
