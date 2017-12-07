
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;

public class ToolTipFunc
{
    public ToolTipFunc(int strIDX, CallBackFunc func)
    {
        _FuncName = StrDictionary.GetFormatStr(strIDX);
        _Func = func;
    }
    public string _FuncName;
    public delegate void CallBackFunc(ItemBase itemBase);
    public CallBackFunc _Func;
}

public class UIItemTooltips : UIBase
{

    #region static funs

    public static void ShowAsyn(ItemBase itembase, params ToolTipFunc[] funcs)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ItemBase", itembase);
        hash.Add("ToolTipFun", funcs);
        GameCore.Instance.UIManager.ShowUI("LogicUI/BagPack/UIItemTooltips", UILayer.MessageUI, hash);
    }

    public static void HideAsyn()
    {
        UIManager.Instance.HideUI("LogicUI/BagPack/UIItemTooltips");
    }

    #endregion

    #region 

    public UIItemInfo _UIItemInfo;

    public GameObject _BtnPanel;

    public Button[] _BtnGO;
    public Text[] _BtnText;

    #endregion

    #region 

    protected ItemBase _ShowItem;
    protected ToolTipFunc[] _ShowFuncs;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        _ShowItem = hash["ItemBase"] as ItemBase;
        ToolTipFunc[] showType = (ToolTipFunc[])hash["ToolTipFun"];
        ShowTips(_ShowItem);
        ShowFuncs(showType);
    }

    protected virtual void ShowFuncs(ToolTipFunc[] funcs)
    {
        _ShowFuncs = funcs;
        if (funcs.Length == 0)
        {
            SetGOActive(_BtnPanel, false);
        }
        else
        {
            SetGOActive(_BtnPanel, true);
            for (int i = 0; i < _BtnGO.Length; ++i)
            {
                if (i < funcs.Length)
                {
                    SetGOActive(_BtnGO[i], true);
                    _BtnText[i].text = funcs[i]._FuncName;
                }
                else
                {
                    SetGOActive(_BtnGO[i], false);
                }
            }
        }
    }

    private void ShowTips(ItemBase itemBase)
    {
        if (itemBase == null)
        {
            _ShowItem = null;
            return;
        }
        _ShowItem = itemBase;

        _UIItemInfo.ShowTips(_ShowItem);
    }

    #endregion

    #region operate

    public void OnBtnFunc(int idx)
    {
        _ShowFuncs[idx]._Func.Invoke(_ShowItem);
        Hide();
    }

    #endregion

}

