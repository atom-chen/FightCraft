
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UILegendaryEquipItem : UIItemBase
{

    public Text _Name;
    public UIBackPackItem _BackpackItem;
    public ItemEquip _EquipItem;

    public EquipItemRecord _LegendaryRecord;
    private UIBase _DragPanel;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (EquipItemRecord)hash["InitObj"];
        var dragPack = (UIBase)hash["DragPack"];
        ShowItem(showItem, dragPack);
    }

    private void ShowItem(EquipItemRecord legendaryEquip, UIBase dragPanel)
    {
        _LegendaryRecord = legendaryEquip;
        _DragPanel = dragPanel;
        _Name.text = legendaryEquip.Name;
        _BackpackItem.ShowItem(LegendaryData.Instance._LegendaryEquipDict[legendaryEquip]);

        _EquipItem = LegendaryData.Instance._LegendaryEquipDict[legendaryEquip];

        Hashtable hash = new Hashtable();
        hash.Add("InitObj", LegendaryData.Instance._LegendaryEquipDict[legendaryEquip]);
        hash.Add("DragPack", dragPanel);
        _BackpackItem.Show(hash);
        _BackpackItem._InitInfo = LegendaryData.Instance._LegendaryEquipDict[legendaryEquip];
        _BackpackItem._ClickEvent += OnEquipItemClick;
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowItem(_LegendaryRecord, _DragPanel);
    }

    public void OnEquipItemClick(object equipItem)
    {
        OnItemClick();
    }
}

