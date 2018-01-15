
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIGemInfo : UIItemInfo
{

    public UIEquipAttrItem _BaseAttr;

    public void ShowTips(ItemGem itemBase)
    {
        base.ShowTips(itemBase);

        _Level.text = itemBase.Level.ToString();
        _BaseAttr.ShowAttr(itemBase.GemAttr);
    }

}

