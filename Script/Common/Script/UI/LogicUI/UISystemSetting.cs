using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UISystemSetting : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/UISystemSetting", UILayer.BaseUI, hash);
    }

    #endregion


    #region setting

    public Toggle _Shadow;
    public Slider _Volumn;
    public Toggle _AimTarget;

    public void InitSetting()
    {
        _Shadow.isOn = GlobalValPack.Instance.IsShowShadow;
        _Volumn.value = GlobalValPack.Instance.Volume;
        _AimTarget.isOn = GlobalValPack.Instance.IsRotToAnimTarget;
    }

    public void OnTrigShadow(bool isTrig)
    {
        GlobalValPack.Instance.IsShowShadow = isTrig;
    }

    public void OnSlider()
    {
        GlobalValPack.Instance.Volume = _Volumn.value;
    }

    public void OnTrigAimTarget(bool isTrig)
    {
        GlobalValPack.Instance.IsRotToAnimTarget = isTrig;
    }

    #endregion
}

