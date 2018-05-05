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

    public void InitSetting()
    {
        _Shadow.isOn = GlobalValPack.Instance.IsShowShadow;
        _Volumn.value = GlobalValPack.Instance.Volume;
    }

    public void OnTrigShadow(bool isTrig)
    {
        GlobalValPack.Instance.IsShowShadow = isTrig;
    }

    public void OnSlider()
    {
        GlobalValPack.Instance.Volume = _Volumn.value;
    }

    #endregion
}

