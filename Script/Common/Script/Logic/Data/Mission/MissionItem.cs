﻿using UnityEngine;
using System.Collections;
using Tables;
using System;

public class MissionProcessData
{
    public int MissionProcessInt = 0;
}

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
    private MissionProcessData _MissionProcessData = new MissionProcessData();

    public MissionProcessData MissionProcessData
    {
        get
        {
            return _MissionProcessData;
        }
    }

    [SaveField(3)]
    public MissionState _MissionState = MissionState.Accepted;

    public MissionConditionBase _MissionCondition;

    public void InitMissionItem()
    {
        var conditionType = Type.GetType(MissionRecord.ConditionScript);
        _MissionCondition = Activator.CreateInstance(conditionType) as MissionConditionBase;
        _MissionCondition.InitCondition(MissionProcessData, MissionRecord.ConditionParams);
    }

    public void RefreshMissionState()
    {
        if (_MissionState == MissionState.Accepted && _MissionCondition.IsConditionMet())
        {
            _MissionState = MissionState.Done;
        }
    }

    public void MissionGetAward()
    {
        Debug.Log("MissionGetAward");
        _MissionState = MissionState.Finish;
    }

}
