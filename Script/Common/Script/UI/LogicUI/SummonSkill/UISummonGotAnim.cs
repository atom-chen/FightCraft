using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class UISummonGotAnim : UIBase
{

    #region static funs

    public static void ShowAsyn(List<SummonMotionData> summonDatas, Action finishCallBack)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonDatas", summonDatas);
        hash.Add("FinishCallBack", finishCallBack);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UISummonGotAnim, UILayer.Sub2PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillPack>(UIConfig.UISummonGotAnim);
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region pack

    public UIContainerBase _SummonItemContainer;

    private List<SummonMotionData> _SummonDatas;
    private Action _FinishCallBack;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SummonDatas = (List<SummonMotionData>)hash["SummonDatas"];
        _FinishCallBack = (Action)hash["FinishCallBack"];
        ShowItemPack();
    }

    public override void Hide()
    {
        base.Hide();

        if (_FinishCallBack != null)
        {
            _FinishCallBack.Invoke();
            _FinishCallBack = null;
        }

    }

    private void ShowItemPack()
    {   
        _SummonItemContainer.InitContentItem(_SummonDatas);
    }

    #endregion


}

