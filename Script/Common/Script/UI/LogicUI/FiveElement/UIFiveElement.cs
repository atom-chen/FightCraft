using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIFiveElement : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/FiveElement/UIFiveElement", UILayer.PopUI, hash);
    }

    public static void RefreshPack()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIFiveElement>("LogicUI/FiveElement/UIFiveElement");
        if (instance == null)
            return;

        if (!instance.isActiveAndEnabled)
            return;

        instance.RefreshItems();
    }

    #endregion

    #region 

    public List<UIFiveElementItem> _UsingElement = new List<UIFiveElementItem>();
    public UIContainerBase _ElementPack;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshItems();
    }

    public void RefreshItems()
    {
        for (int i = 0; i < _UsingElement.Count; ++i)
        {
            _UsingElement[i].ShowItem(FiveElementData.Instance._UsingElements[i]);
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", FiveElementData.Instance._UsingElements[i]);
            hash.Add("DragPack", this);
            _UsingElement[i].Show(hash);
            _UsingElement[i]._InitInfo = FiveElementData.Instance._UsingElements[i];
            _UsingElement[i]._ClickEvent += ShowTooltipsLeft;
        }
        _ElementPack.InitContentItem(FiveElementData.Instance._PackElements, ShowTooltipsRight);
    }

    private void ShowTooltipsLeft(object itemObj)
    {
        ItemFiveElement elementItem = itemObj as ItemFiveElement;
        if (elementItem == null || !elementItem.IsVolid())
            return;

        UIFiveElementTooltip.ShowAsynInType(elementItem, TooltipType.Single, new ToolTipFunc[1] { new ToolTipFunc(10016, LevelUp) });
        //UIGemTooltips.ShowAsynInType(gemItem, TooltipType.GemSuitAttr, new ToolTipFunc[2] { new ToolTipFunc(10008, PunchOff), new ToolTipFunc(10009, LevelUp) });
        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void ShowTooltipsRight(object itemObj)
    {
        ItemFiveElement elementItem = itemObj as ItemFiveElement;
        if (elementItem == null || !elementItem.IsVolid())
            return;

        UIFiveElementTooltip.ShowAsynInType(elementItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10016, LevelUp), new ToolTipFunc(10005, Sell) });
        //UIGemTooltips.ShowAsynInType(gemItem, TooltipType.GemSuitAttr, new ToolTipFunc[2] { new ToolTipFunc(10008, PunchOff), new ToolTipFunc(10009, LevelUp) });
        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void PutOn(ItemBase itemBase)
    {
        ItemFiveElement itemElement = itemBase as ItemFiveElement;
        if (!itemElement.IsVolid())
            return;

        FiveElementData.Instance.PutOnElement(itemElement);
        RefreshItems();
    }

    private void PutOff(ItemBase itemBase)
    {
        ItemFiveElement itemElement = itemBase as ItemFiveElement;
        if (!itemElement.IsVolid())
            return;

        FiveElementData.Instance.PutOffElement(itemElement);
        RefreshItems();
    }

    private void LevelUp(ItemBase itemBase)
    {
        ItemFiveElement itemElement = itemBase as ItemFiveElement;
        if (!itemElement.IsVolid())
            return;

        var itemUsing = FiveElementData.Instance._UsingElements[(int)itemElement.FiveElementRecord.EvelemtType];

        UIFiveElementExtra.ShowAsyn(itemUsing);
        //RefreshItems();
    }

    private void Sell(ItemBase itemBase)
    {
        ItemFiveElement itemElement = itemBase as ItemFiveElement;
        if (!itemElement.IsVolid())
            return;

        RefreshItems();
    }
    #endregion

    #region 

    public void OnBtnExtra()
    {
        UIFiveElementExtra.ShowAsyn(null);
    }

    public void OnBtnValueAttr()
    {
        UIFiveElementValueTip.ShowAsyn();
    }

    #endregion

}

