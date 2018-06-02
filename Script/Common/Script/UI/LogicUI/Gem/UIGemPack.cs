using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIGemPack : UIBase, IDragablePack
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

        instance.RefreshItems();
    }

    #endregion

    #region 

    public UIContainerBase _GemContainer;
    public UIContainerBase _MaterialContainer;

    public UIGemItem[] _GemPack;

    private int _SelectGemSlot = -1;

    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowPackItems();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void ShowPackItems()
    {
        Hashtable exHash = new Hashtable();
        exHash.Add("DragPack", this);

        _GemContainer.InitContentItem(GemData.Instance._GemContainer, ShowGemTooltipsRight, exHash);

        List<ItemBase> matItems = new List<ItemBase>();
        foreach (var matData in GemData._GemMaterialDataIDs)
        {
            var matItemInPack = BackBagPack.Instance.GetItem(matData);
            if (matItemInPack == null)
            {
                matItems.Add(new ItemBase(matData));
            }
            else
            {
                matItems.Add(matItemInPack);
            }
        }
        _MaterialContainer.InitContentItem(matItems, ShowMaterialTooltips, exHash);
        for (int i = 0; i < _GemPack.Length; ++i)
        {
            Hashtable hash = new Hashtable();
            hash.Add("InitObj", GemData.Instance._EquipedGems[i]);
            hash.Add("DragPack", this);
            _GemPack[i].Show(hash);
            _GemPack[i]._InitInfo = GemData.Instance._EquipedGems[i];
            _GemPack[i]._ClickEvent += ShowGemTooltipsLeft;
        }
        //_BackPack.Show(null);
    }

    public void RefreshItems()
    {
        _GemContainer.RefreshItems();
        _MaterialContainer.RefreshItems();
        for (int i = 0; i < _GemPack.Length; ++i)
        {
            _GemPack[i].ShowGem(GemData.Instance._EquipedGems[i]);
        }
        //_EquipContainer.RefreshItems();
        //_BackPack.RefreshItems();
    }

    private void ShowGemTooltipsLeft(object equipObj)
    {
        for (int i = 0; i < _GemPack.Length; ++i)
        {
            if (_GemPack[i]._InitInfo == equipObj)
            {
                _GemPack[i].Selected();
                _SelectGemSlot = i;
            }
            else
            {
                _GemPack[i].UnSelected();
            }
        }
        ItemGem gemItem = equipObj as ItemGem;
        if (gemItem == null || !gemItem.IsVolid())
            return;

        UIGemTooltips.ShowAsynInType(gemItem, TooltipType.GemSuitAttr, new ToolTipFunc[2] { new ToolTipFunc(10008, PunchOff), new ToolTipFunc(10009, LevelUp) });
        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void ShowGemTooltipsRight(object equipObj)
    {
        ItemGem gemItem = equipObj as ItemGem;
        if (gemItem == null || !gemItem.IsVolid())
            return;

        if (gemItem.ItemStackNum == 0)
        {
            UIGemTooltips.ShowAsynInType(gemItem, TooltipType.Single, new ToolTipFunc[1] { new ToolTipFunc(10012, LevelUp) });
        }
        else
        {
            UIGemTooltips.ShowAsynInType(gemItem, TooltipType.Single, new ToolTipFunc[2] { new ToolTipFunc(10007, PunchOn), new ToolTipFunc(10009, LevelUp) });
        }
        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }

    private void ShowMaterialTooltips(object equipObj)
    {
        ItemEquip equipItem = equipObj as ItemEquip;
        if (equipItem == null || !equipItem.IsVolid())
            return;

        //UIEquipTooltips.ShowAsyn(equipItem, ToolTipsShowType.ShowInEquipPack);
    }
    #endregion

    #region 

    private void ExchangeGems(ItemGem itemGem1, ItemGem itemGem2)
    {
        GemData.Instance.ExchangeGem(itemGem1, itemGem2);
        RefreshItems();
    }

    private void PunchOn(ItemGem itemGem, int idx)
    {
        GemData.Instance.PutOnGem(itemGem, idx);
        RefreshItems();
    }

    private void PunchOn(ItemBase itemBase)
    {
        ItemGem itemGem = itemBase as ItemGem;
        if (!itemGem.IsVolid())
            return;

        if (_SelectGemSlot >= 0)
        {
            PunchOn(itemGem, _SelectGemSlot);
        }
        else
        {
            var idx = GemData.Instance.GetPutOnIdx();
            PunchOn(itemGem, idx);
        }
    }

    private void PunchOff(ItemBase itemBase)
    {
        ItemGem itemGem = itemBase as ItemGem;
        GemData.Instance.PutOff(itemGem);
        RefreshItems();
    }

    private void LevelUp(ItemBase itemBase)
    {
        ItemGem itemGem = itemBase as ItemGem;
        if (GemData.Instance.GemLevelUp(itemGem))
        {
            RefreshItems();
        }
    }

    private void ItemRefresh(object sender, Hashtable args)
    {
        RefreshItems();
    }

    public bool IsCanDragItem(UIDragableItemBase dragItem)
    {
        if (!dragItem.ShowedItem.IsVolid())
            return false;

        var gemRecord = Tables.TableReader.GemTable.GetRecord(dragItem.ShowedItem.ItemDataID);
        if (gemRecord == null)
            return false;

        return true;
    }

    public bool IsCanDropItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        //if (dragItem._DragPackBase == dropItem._DragPackBase)
        //    return false;

        //
        //if (GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem)
        //    && GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
        //    return false;

        //gem collect cant change
        if (!GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem)
            && !GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
            return false;

        if (dragItem.ShowedItem.ItemStackNum < 1)
            return false;

        return true;
    }

    public void OnDragItem(UIDragableItemBase dragItem, UIDragableItemBase dropItem)
    {
        if (GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem)
            && GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
        {
            ExchangeGems(dragItem.ShowedItem as ItemGem, dropItem.ShowedItem as ItemGem);
        }
        else if (GemData.Instance._EquipedGems.Contains(dragItem.ShowedItem as ItemGem))
        {
            PunchOff(dragItem.ShowedItem as ItemGem);
        }
        else if (GemData.Instance._EquipedGems.Contains(dropItem.ShowedItem as ItemGem))
        {
            int idx = GemData.Instance._EquipedGems.IndexOf(dropItem.ShowedItem as ItemGem);
            PunchOn(dragItem.ShowedItem as ItemGem, idx);
        }
    }
    #endregion

    #region 

    public void OnBtnGemSuit()
    {
        UIGemSuitPack.ShowAsyn();
    }

    #endregion

}

