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

        instance.RefreshUsingItems();
        instance.RefreshPackItems();
        instance.RefreshCoreItems();
        instance.RefreshUsingCoreItems();

    }

    #endregion

    #region element pack

    public List<UIFiveElementItem> _UsingElement = new List<UIFiveElementItem>();
    public UIContainerBase _ElementPack;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        RefreshUsingItems();
        RefreshPackItems();
        RefreshCoreItems();
        RefreshUsingCoreItems();
    }

    public void RefreshUsingItems()
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
    }

    public void RefreshPackItems()
    {
        
        _ElementPack.InitContentItem(FiveElementData.Instance._PackElements._PackItems, ShowTooltipsRight);
    }

    private void ShowTooltipsLeft(object itemObj)
    {
        ItemFiveElement elementItem = itemObj as ItemFiveElement;
        if (elementItem == null || !elementItem.IsVolid())
            return;

        UIFiveElementTooltip.ShowAsynInType(elementItem, TooltipType.Single, new ToolTipFunc[1] { new ToolTipFunc(10016, Extra)});
        //UIGemTooltips.ShowAsynInType(gemItem, TooltipType.GemSuitAttr, new ToolTipFunc[2] { new ToolTipFunc(10008, PunchOff), new ToolTipFunc(10009, LevelUp) });
        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void ShowTooltipsRight(object itemObj)
    {
        ItemFiveElement elementItem = itemObj as ItemFiveElement;
        if (elementItem == null || !elementItem.IsVolid())
            return;

        UIFiveElementTooltip.ShowAsynInType(elementItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10016, Extra), new ToolTipFunc(10014, Decompose) });
        //UIGemTooltips.ShowAsynInType(gemItem, TooltipType.GemSuitAttr, new ToolTipFunc[2] { new ToolTipFunc(10008, PunchOff), new ToolTipFunc(10009, LevelUp) });
        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void Extra(ItemBase itemBase)
    {
        ItemFiveElement itemElement = itemBase as ItemFiveElement;
        if (itemElement != null && itemElement.IsVolid())
        {
            var itemUsing = FiveElementData.Instance._UsingElements[(int)itemElement.FiveElementRecord.EvelemtType];
            UIFiveElementExtra.ShowAsyn(itemUsing);
        }

        ItemFiveElementCore itemElementCore = itemBase as ItemFiveElementCore;
        if (itemElementCore != null)
        {
            var itemUsing = FiveElementData.Instance._UsingElements[(int)itemElementCore.FiveElementRecord.ElementType];
            UIFiveElementExtra.ShowAsyn(itemUsing);
        }
        //RefreshItems();
    }

    private void Decompose(ItemBase itemBase)
    {
        ItemFiveElement itemElement = itemBase as ItemFiveElement;
        if (!itemElement.IsVolid())
            return;

        RefreshPackItems();
    }
    #endregion

    #region core pack

    public List<UIFiveElementItem> _UsingElementCore = new List<UIFiveElementItem>();
    public UIContainerBase _ElementCorePack;

    public void RefreshCoreItems()
    {
        _ElementCorePack.InitContentItem(FiveElementData.Instance._PackCores._PackItems, ShowTooltipsCoreRight);
    }

    public void RefreshUsingCoreItems()
    {
        for (int i = 0; i < _UsingElement.Count; ++i)
        {
            _UsingElementCore[i].ShowItem(FiveElementData.Instance._UsingCores[i]);
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", FiveElementData.Instance._UsingCores[i]);
            hash.Add("DragPack", this);
            _UsingElementCore[i].Show(hash);
            _UsingElementCore[i]._InitInfo = FiveElementData.Instance._UsingCores[i];
            _UsingElementCore[i]._ClickEvent += ShowTooltipsCoreLeft;
        }
    }

    private void ShowTooltipsCoreRight(object itemObj)
    {
        ItemFiveElementCore elementItem = itemObj as ItemFiveElementCore;
        if (elementItem == null || !elementItem.IsVolid())
            return;

        UIFiveElementCoreTooltip.ShowAsynInType(elementItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10003, PutOn), new ToolTipFunc(10014, Decompose) });
        
    }

    private void ShowTooltipsCoreLeft(object itemObj)
    {
        ItemFiveElementCore elementItem = itemObj as ItemFiveElementCore;
        if (elementItem == null || !elementItem.IsVolid())
            return;

        UIFiveElementCoreTooltip.ShowAsynInType(elementItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10016, Extra), new ToolTipFunc(10004, PutOff) });
    }

    private void PutOn(ItemBase itemBase)
    {
        ItemFiveElementCore itemElement = itemBase as ItemFiveElementCore;
        if (!itemElement.IsVolid())
            return;

        FiveElementData.Instance.PutOnElementCore(itemElement);
        RefreshCoreItems();
        RefreshUsingCoreItems();
    }

    private void PutOff(ItemBase itemBase)
    {
        ItemFiveElementCore itemElement = itemBase as ItemFiveElementCore;
        if (!itemElement.IsVolid())
            return;

        FiveElementData.Instance.PutOffElementCore(itemElement);
        RefreshCoreItems();
        RefreshUsingCoreItems();
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

