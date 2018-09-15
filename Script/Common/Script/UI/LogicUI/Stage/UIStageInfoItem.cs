
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIStageInfoItem : UIItemSelect
{
    public Text _StageName;
    public GameObject _LockedGO;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (StageInfoRecord)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(StageInfoRecord showItem)
    {
        _StageName.text = StrDictionary.GetFormatStr(showItem.Name);

        int stageId = int.Parse(showItem.Id);
        if (UIStageSelect.GetMaxStageID() < stageId)
        {
            _LockedGO.SetActive(true);
        }
        else
        {
            _LockedGO.SetActive(false);
        }
    }

    public override void OnItemClick()
    {
        if (_LockedGO.activeSelf)
            return;

        base.OnItemClick();
    }
}

