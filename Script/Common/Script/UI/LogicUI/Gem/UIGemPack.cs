using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIGemPack : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Gem/UIGemPack", UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIGemPack>("LogicUI/Gem/UIGemPack");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.Refresh();
    }

    #endregion

    #region 

    public UIContainerBase _GemPack;

    public UITagPanel _TagPanel;
    public UIGemPackPunch _PunchPanel;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _TagPanel.ShowPage(0);
        _GemPack.InitContentItem(GemData.Instance.PackGemDatas._PackItems, OnPackItemClick);
    }

    public void Refresh()
    {
        _GemPack.RefreshItems();
    }

    private void OnPackItemClick(object gemItemObj)
    {
        ItemGem gemItem = gemItemObj as ItemGem;
        if (gemItem == null)
            return;

        int showingPage = _TagPanel.GetShowingPage();
        if (showingPage == 0)
        {
            _PunchPanel.ShowGemTooltipsRight(gemItem);
        }
    }

    #endregion

    #region 

    public void OnBtnSort()
    {
        GemData.Instance.PackGemDatas.SortStack();
        GemData.Instance.PackGemDatas.SortEmpty();
        _GemPack.RefreshItems();
    }

    #endregion


}

