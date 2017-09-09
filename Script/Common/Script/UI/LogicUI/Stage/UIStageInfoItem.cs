
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UIStageInfoItem : UIItemSelect
{
    public Text _StageName;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (StageInfoRecord)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(StageInfoRecord showItem)
    {
        _StageName.text = showItem.Name;
    }


}

