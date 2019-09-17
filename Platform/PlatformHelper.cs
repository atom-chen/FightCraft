﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformHelper : MonoBehaviour
{
    #region 
    private static PlatformHelper _Instance;

    public static PlatformHelper Instance
    {
        get
        {
            return _Instance;
        }
    }

    private void Start()
    {
        _Instance = this;
    }
    #endregion

#if UNITY_ANDROID 
    public string PLATFORM_CLASS = "";
    public string CallAndroid(string func, string param)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (string.IsNullOrEmpty(PLATFORM_CLASS))
        {
            PLATFORM_CLASS = Application.identifier + ".PlatformHelper";
        }
        Debug.LogError("OnCallAndroid:" + func + "," + param);
        using (AndroidJavaClass cls = new AndroidJavaClass(PLATFORM_CLASS))
        {   
            string ret = cls.CallStatic<string>("jniCall", func, param);
            return ret;
        }
#endif
        return "0";
    }

    public void OnAndroidCall(string jsonstr)
    {
        Debug.LogError("OnCallResult : " + jsonstr);
        JsonData jsonobj = JsonMapper.ToObject(jsonstr);
        string func = (string)jsonobj["func"];

        if (func.Equals("OnVideoADLoad"))
        {
            OnVideoADLoaded();
        }
        else if (func.Equals("OnVideoReward"))
        {
            OnVideoADReward();
        }
        else if (func.Equals("OnVideoComplete"))
        {
            OnVideoADComplate();
        }
        else if (func.Equals("OnInterADReceive"))
        {
            OnInterADLoaded();
        }
    }

#endif

        #region AD

    public void GetLocationPermission()
    {
        string permissionStr = "android.permission.ACCESS_FINE_LOCATION";
        CallAndroid("RequstePermission", permissionStr);
    }

    public void LoadVideoAD()
    {
        CallAndroid("LoadVideoAD", "");
    }

    public void ShowVideoAD()
    {
        CallAndroid("ShowVideoAD", "");
    }

    public void LoadInterAD()
    {
        CallAndroid("LoadInterstitialAD", "");
    }

    public void ShowInterAD()
    {
        CallAndroid("ShowInterstitialAD", "");
    }

    public void CloseInterAD()
    {
        CallAndroid("CloseInterstitialAD", "");
    }

    public void OnVideoADLoaded()
    {
        //result.text = "OnVideoADLoaded";
    }

    public void OnVideoADReward()
    {
        AdManager.Instance.WatchAdVideoFinish();
    }

    public void OnVideoADComplate()
    {
        //result.text = "OnVideoADComplate";
    }

    public void OnInterADLoaded()
    {
        //result.text = "OnInterADLoaded";
    }

    #endregion



}
