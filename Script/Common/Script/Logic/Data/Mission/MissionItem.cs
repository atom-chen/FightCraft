using UnityEngine;
using System.Collections;
using Tables;
using System;

public enum MissionState
{
    None,
    Accepted,
    Done,
    Finish, //getted award
}

public class MissionItem : SaveItemBase
{
    public MissionItem()
    {

    }

    public MissionItem(string missionDataID)
    {
        _MissionDataID = missionDataID;
    }

    public MissionItem(MissionRecord missionData)
    {
        _MissionDataID = missionData.Id;
    }

    [SaveField(1)]
    private string _MissionDataID;
    public string MissionDataID
    {
        get
        {
            return _MissionDataID;
        }
    }

    private MissionRecord _MissionRecord;
    public MissionRecord MissionRecord
    {
        get
        {
            if (_MissionRecord == null)
            {
                _MissionRecord = TableReader.Mission.GetRecord(_MissionDataID);
            }
            return _MissionRecord;
        }
    }

    [SaveField(2)]
    private int _MissionProcessData = 0;

    public int MissionProcessData
    {
        get
        {
            return _MissionProcessData;
        }
        set
        {
            _MissionProcessData = value;
        }
    }

    [SaveField(3)]
    public MissionState _MissionState = MissionState.Accepted;

    public MissionConditionBase _MissionCondition;

    public void InitMissionItem()
    {
        var conditionType = Type.GetType(MissionRecord.ConditionScript);
        if (conditionType == null)
        {
            Debug.LogError("MissionRecord.ConditionScript type error:" + MissionRecord.ConditionScript);
            return;
        }
        _MissionCondition = Activator.CreateInstance(conditionType) as MissionConditionBase;
        _MissionCondition.InitCondition(this, MissionRecord);

        RefreshMissionState();
    }

    public void RefreshMissionState()
    {
        if (_MissionState == MissionState.Accepted && _MissionCondition.IsConditionMet())
        {
            _MissionState = MissionState.Done;

            if (MissionRecord.Achieve == 2)
            {
                GuideMissionData.Instance.ComplateMission();
            }
        }
        else if (_MissionState == MissionState.Finish)
        {
            if (MissionRecord.Achieve == 2)
            {
                GuideMissionData.Instance.ComplateMission();
            }
        }
        else if(_MissionState == MissionState.Done)
        {
            _MissionState = MissionState.Finish;
            if (MissionRecord.Achieve == 2)
            {
                GuideMissionData.Instance.ComplateMission();
            }
        }
    }

    public bool MissionGetAward(bool showTips = false)
    {
        if (MissionRecord.AwardType == 0)
        {
            PlayerDataPack.Instance.AddGold(MissionRecord.AwardNum);
            if (showTips)
            {
                string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(1000002), 1));
                UIMessageTip.ShowMessageTip(strTips);
            }
        }
        else if (MissionRecord.AwardType == 1)
        {
            PlayerDataPack.Instance.AddDiamond(MissionRecord.AwardNum);
            if (showTips)
            {
                string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(1000003), 1));
                UIMessageTip.ShowMessageTip(strTips);
            }
        }
        else if (MissionRecord.AwardType == 2)
        {
            BackBagPack.Instance.PageItems.AddItem(MissionRecord.AwardSubType.ToString(), MissionRecord.AwardNum);
            var itemRecord = Tables.TableReader.CommonItem.GetRecord(MissionRecord.AwardSubType.ToString());
            if (showTips)
            {
                string strTips = Tables.StrDictionary.GetFormatStr(2300088, string.Format("{0} * {1}", Tables.StrDictionary.GetFormatStr(itemRecord.NameStrDict), 1));
                UIMessageTip.ShowMessageTip(strTips);
            }
        }

        _MissionState = MissionState.Finish;

        return true;
    }

}

