using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UISummonGotAnim : UIBase
{

    #region static funs

    public static void ShowAsyn(List<SummonMotionData> summonDatas)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SummonDatas", summonDatas);
        GameCore.Instance.UIManager.ShowUI("LogicUI/SummonSkill/UISummonGotAnim", UILayer.Sub2PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UISummonSkillPack>("LogicUI/SummonSkill/UISummonGotAnim");
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

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _SummonDatas = (List<SummonMotionData>)hash["SummonDatas"];
        ShowItemPack();
    }

    private void ShowItemPack()
    {   
        _SummonItemContainer.InitContentItem(_SummonDatas);
    }

    #endregion


}

