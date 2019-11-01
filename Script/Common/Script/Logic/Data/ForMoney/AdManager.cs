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

    public void PrepareVideo()
    {
        PlatformHelper.Instance.LoadVideoAD();
    }

    public void WatchAdVideo(Action finishCallBack)
    {
        _AdVideoCallBack = finishCallBack;

        if(PlatformHelper.Instance.GetLocationPermission())
            PlatformHelper.Instance.ShowVideoAD();
    }

    public void WatchAdVideoFinish()
    {
        if (_AdVideoCallBack != null)
        {
            _AdVideoCallBack.Invoke();
        }
    }

    #endregion

    #region ad inter

    public float _ShowInterADTime = 3.0f;
    public float _StartInterADTime;
    public bool _IsInterADPrepared = false;

    private bool _InterADExposure = false;
    private int _LoadSceneTimes = 0;
    private int _ShowAdLoadTimes = 1;
    private bool _IsShowInterAD = false;
    public bool IsShowInterAD
    {
        get
        {
            return false;
            return _IsShowInterAD;
        }
    }

    public void PrepareInterAD()
    {
        PlatformHelper.Instance.LoadInterAD();
    }

    public void LoadedInterAD()
    {
        _IsInterADPrepared = true;
        _StartInterADTime = 0;
    }

    public void ShowInterAD()
    {
        _IsInterADPrepared = false;
        _StartInterADTime = Time.time;
        PlatformHelper.Instance.ShowInterAD();
    }

    public bool IsShowInterADFinish()
    {
        if (!_InterADExposure)
            return true;

        if (Time.time - _StartInterADTime > _ShowInterADTime)
        {
            CloseInterAD();
            return true;
        }
        return false;
    }

    public void CloseInterAD()
    {
        PlatformHelper.Instance.CloseInterAD();
    }

    public void OnInterADExposure()
    {
        _InterADExposure = true;
    }

    public void OnInterADClosed()
    {
        _InterADExposure = false;
    }

    public void AddLoadSceneTimes()
    {
        CloseInterAD();
        ++_LoadSceneTimes;
        if (_LoadSceneTimes % _ShowAdLoadTimes == 0)
        {
            LoadedInterAD();
            _IsShowInterAD = true;
        }
        else
        {
            _IsShowInterAD = false;
        }
    }

    #endregion
}
