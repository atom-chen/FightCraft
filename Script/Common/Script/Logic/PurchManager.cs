using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PurchManager
{

    #region 唯一

    private static PurchManager _Instance = null;
    public static PurchManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new PurchManager();
            }
            return _Instance;
        }
    }

    private PurchManager() { }

    #endregion

    #region purch

    private Action _PurchCallback;

    public void Purch(int idx, Action callBack)
    {
        _PurchCallback = callBack;
        //watch movie

        PurchFinish();
    }

    public void PurchFinish()
    {
        if (_PurchCallback != null)
            _PurchCallback.Invoke();
    }

    #endregion

    #region ad

    private Action _WatchADCallback;

    public void WatchAD(Action callBack)
    {
        _WatchADCallback = callBack;
        //watch movie

        WatchADFinish();
    }

    public void WatchADFinish()
    {
        if (_WatchADCallback != null)
            _WatchADCallback.Invoke();
    }

    #endregion
}
