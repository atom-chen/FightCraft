using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Tables;

public class UIGemSuitPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Gem/UIGemSuitPack", UILayer.SubPopUI, hash);
    }

    public static void RefreshBagItems()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>("LogicUI/Gem/UIGemSuitPack");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public UIGemSuitAttr _GemSuitAttr;

    public UIContainerSelect _GemSuitContainer;

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowPackItems();
    }

    private void ShowPackItems()
    {
        var suitTabs = Tables.TableReader.GemSet.Records.Values;
        _GemSuitContainer.InitSelectContent(suitTabs, new List<GemSetRecord>() { GemSuit.Instance.ActSet }, SuitSelect);
    }

    private void ClearSuitInfo()
    {
        _GemSuitAttr.ClearSuitInfo();
    }

    private void SuitSelect(object suitObj)
    {
        GemSetRecord gemSet = suitObj as GemSetRecord;
        if (gemSet == null)
            return;

        _GemSuitAttr.SuitSelect(gemSet);
    }
}

