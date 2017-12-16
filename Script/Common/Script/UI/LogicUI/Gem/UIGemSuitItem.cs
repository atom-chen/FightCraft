using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;


public class UIGemSuitItem : UIItemBase
{
    public Text _SuitName;
    public UIContainerBase _SuitGems;
    public Button _BtnApply;

    private Tables.GemSetRecord _GemSetTab;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        var gemInfo = (GemSetRecord)hash["InitObj"];
        ShowGem(gemInfo);
    }

    public override void Refresh()
    {
        base.Refresh();

        ShowGem(_GemSetTab);
    }


    public void ShowGem(GemSetRecord gemSet)
    {
        _GemSetTab = gemSet;

        _SuitName.text = gemSet.Name;

        Hashtable hash = new Hashtable();
        hash.Add("MinLevel", gemSet.MinGemLv);
        hash.Add("IsClearGem", gemSet.IsEnableDefault);
        _SuitGems.InitContentItem(gemSet.Gems, null, hash);

        if (GemSuit.Instance.IsGemSetCanUse(gemSet))
        {
            _BtnApply.interactable = true;
        }
        else
        {
            _BtnApply.interactable = false;
        }
    }

    public void IsGemSetCanUse()
    {

    }

}

