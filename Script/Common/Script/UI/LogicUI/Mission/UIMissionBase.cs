
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIMissionBase : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIMissionBase, UILayer.BaseUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        ShowMissionInfo();
    }

    private void Update()
    {
        MissionUpdate();
    }

    #region 

    public Text _MissionName;
    public Text _Condition;
    public UICommonAwardItem _AwardItem;

    private string _CurShowMissionID = "";
    private float _MissionProcess = 0;
    private string _MissionProcessStr = "";

    private void ShowMissionInfo()
    {
        if (GuideMissionData.Instance.CurMissionItem != null && GuideMissionData.Instance.CurMissionItem._MissionCondition != null)
        {
            _CurShowMissionID = GuideMissionData.Instance.CurMissionItem.MissionRecord.Id;

            var missionRecord = GuideMissionData.Instance.CurMissionItem.MissionRecord;
            //_MissionName.text = StrDictionary.GetFormatStr(missionRecord.NameID.Id);
            //_Condition.text = GuideMissionData.Instance.CurMissionItem._MissionCondition.GetMissionDesc() + " (" + GuideMissionData.Instance.CurMissionItem._MissionCondition.GetConditionProcessText() + ")";
            UpdateMissionProcess();

            if (missionRecord.AwardType == 0)
            {
                _AwardItem.ShowAward(MONEYTYPE.GOLD, missionRecord.AwardNum);
            }
            else if (missionRecord.AwardType == 1)
            {
                _AwardItem.ShowAward(MONEYTYPE.DIAMOND, missionRecord.AwardNum);
            }
            else if (missionRecord.AwardType == 2)
            {
                _AwardItem.ShowAward(missionRecord.AwardSubType.ToString(), missionRecord.AwardNum);
            }
        }
        else
        {
            _MissionName.text = StrDictionary.GetFormatStr(50002);
        }
    }

    public void UpdateMissionProcess()
    {
        var missionRecord = GuideMissionData.Instance.CurMissionItem.MissionRecord;
        _Condition.text = GuideMissionData.Instance.CurMissionItem._MissionCondition.GetMissionDesc() + " (" + GuideMissionData.Instance.CurMissionItem._MissionCondition.GetConditionProcessText() + ")";
        _MissionProcess = GuideMissionData.Instance.CurMissionItem._MissionCondition.GetConditionProcess();
        _MissionProcessStr = GuideMissionData.Instance.CurMissionItem._MissionCondition.GetConditionProcessText();
    }

    private void MissionUpdate()
    {
        if (GuideMissionData.Instance.CurMissionItem != null && GuideMissionData.Instance.CurMissionItem._MissionCondition != null)
        {
            if (!_CurShowMissionID.Equals(GuideMissionData.Instance.CurMissionItem.MissionRecord.Id))
            {
                ShowMissionInfo();
            }
            else if (_MissionProcess != GuideMissionData.Instance.CurMissionItem._MissionCondition.GetConditionProcess())
            {
                UpdateMissionProcess();
            }
            else if (_MissionProcessStr != GuideMissionData.Instance.CurMissionItem._MissionCondition.GetConditionProcessText())
            {
                UpdateMissionProcess();
            }

            if (UIGuideTip.IsUIGuide(GuideMissionData.Instance.CurMissionItem.MissionRecord.GuideID))
            {
                if (UIMainFun.IsUIShow())
                {
                    UIGuideTip.ShowGuideStep(GuideMissionData.Instance.CurMissionItem.MissionRecord.GuideID);
                }
            }
            else if (UIGuideTip.IsFightGuide(GuideMissionData.Instance.CurMissionItem.MissionRecord.GuideID))
            {
                if (UISkillBar.IsUIShow())
                {
                    UIGuideTip.ShowGuideStep(GuideMissionData.Instance.CurMissionItem.MissionRecord.GuideID);
                }
            }
            else
            {
                UIGuideTip.CloseGuide();
            }
        }
        
    }
    #endregion

}

