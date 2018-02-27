using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIDailyMissionItem : UIItemSelect
{
    public Image _MissionIcon;
    public Text _MissionDesc;
    public Slider _MissionProcess;
    public Text _MissionProcessText;
    public UIAwardItem _Award;
    public GameObject _BtnGetAward;
    public GameObject _AwardGettedGO;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (MissionItem)hash["InitObj"];
        showItem.RefreshMissionState();
        ShowMissionItem(showItem);
    }

    private void ShowMissionItem(MissionItem showItem)
    {
        _MissionDesc.text = showItem.MissionRecord.Name;
        _MissionProcess.value = showItem._MissionCondition.GetConditionProcess();
        _MissionProcessText.text = showItem._MissionCondition.GetConditionProcessText();

        _BtnGetAward.SetActive(false);
        _AwardGettedGO.SetActive(false);
        if (showItem._MissionState == MissionState.Done)
        {
            _BtnGetAward.SetActive(true);
        }
        else if (showItem._MissionState == MissionState.Finish)
        {
            _AwardGettedGO.SetActive(true);
        }

    }

    


}

