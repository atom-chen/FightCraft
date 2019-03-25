
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

 
using UnityEngine.EventSystems;
using System;
using Tables;



public class UIBossStageInfoItem : UIStageInfoItem
{
    protected BossStageRecord _ShowBossItem;
    public override void Show(Hashtable hash)
    {
        base.Show();

        var showItem = (BossStageRecord)hash["InitObj"];
        ShowStage(showItem);
    }

    public void ShowStage(BossStageRecord showItem)
    {
        _ShowBossItem = showItem;

        _StageName.text = StrDictionary.GetFormatStr(showItem.Name);

        int stageID = int.Parse(showItem.Id);
        if (RoleData.SelectRole._CombatValue < showItem.Combat)
        {
            _ConditionTips = CommonDefine.GetEnableRedStr(0) + StrDictionary.GetFormatStr(71104, showItem.Combat) + "</color>";
        }
        else if (ActData.Instance._BossStageIdx + 1 < stageID)
        {
            _ConditionTips = StrDictionary.GetFormatStr(71104, showItem.Combat);
        }
        _StageCondition.text = _ConditionTips;

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
            int stageId = int.Parse(_ShowBossItem.Id);
            if (ActData.Instance._BossStageIdx < stageId)
            {
                UIMessageTip.ShowMessageTip(71102);
            }
            else
            {
                UIMessageTip.ShowMessageTip(71101);
            }
            return;
        }

        base.OnItemClick();
    }
}

