
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
    public Text _StageCondition;

    protected string _ConditionTips;
    protected StageInfoRecord _ShowItem;

    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (StageInfoRecord)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(StageInfoRecord showItem)
    {
        _ShowItem = showItem;
        _StageName.text = StrDictionary.GetFormatStr(showItem.Name);

        int stageId = int.Parse(showItem.Id);

        _ConditionTips = "";

        int stageLevel = GameDataValue.GetStageLevel(UIStageSelect.GetSelectedDiff(), stageId, STAGE_TYPE.NORMAL);
        if (showItem.StageType == STAGE_TYPE.ACTIVITY)
        {
            stageLevel = RoleData.SelectRole.TotalLevel;
        }
        else
        {
            if (RoleData.SelectRole.TotalLevel + 10 < stageLevel)
            {
                _ConditionTips = CommonDefine.GetEnableRedStr(0) + StrDictionary.GetFormatStr(71103, stageLevel) + "</color>";
            }
            else if (UIStageSelect.GetMaxStageID() < stageId)
            {
                _ConditionTips = StrDictionary.GetFormatStr(71103, stageLevel);
            }
            _StageCondition.text = _ConditionTips;
        }

        if (string.IsNullOrEmpty(_ConditionTips))
        {
            _LockedGO.SetActive(false);
        }
        else
        {
            _LockedGO.SetActive(true);
        }

    }

    public override void OnItemClick()
    {
        if (_LockedGO.activeSelf)
        {
            int stageId = int.Parse(_ShowItem.Id);
            if (UIStageSelect.GetMaxStageID() < stageId)
            {
                UIMessageTip.ShowMessageTip(71102);
            }
            else
            {
                UIMessageTip.ShowMessageTip(71100);
            }
            return;
        }

        base.OnItemClick();
    }
}

