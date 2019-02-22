
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;



public class UIElementCoreCondition : UIItemSelect
{
    public Text _Desc;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (string)hash["InitObj"];

        ShowAttr(showItem);
    }

    public void ShowAttr(string itemElement)
    {

        _Desc.text = itemElement;
    }


}

