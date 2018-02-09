
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UIBossStageInfoItem : UIItemSelect
{
    public Text _StageName;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (BossStageRecord)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(BossStageRecord showItem)
    {
        _StageName.text = showItem.Name;
    }


}

