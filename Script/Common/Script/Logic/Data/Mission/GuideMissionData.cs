using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class GuideMissionData : DataPackBase
{

    #region 唯一

    private static GuideMissionData _Instance = null;
    public static GuideMissionData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new GuideMissionData();
            }
            return _Instance;
        }
    }

    private GuideMissionData()
    {
        _SaveFileName = "GuideMissionData";
    }

    #endregion

    #region 

    [SaveField(1)]
    private MissionItem _CurMissionItem;

    public MissionItem CurMissionItem
    {
        get
        {
            if (_CurMissionItem == null)
                return null;
            if (string.IsNullOrEmpty(_CurMissionItem.MissionDataID))
            {
                return null;
            }
            return _CurMissionItem;
        }
    }

    public FightGuide _FightGuide;

    public void InitMissionData()
    {
        if (_CurMissionItem == null || string.IsNullOrEmpty( _CurMissionItem.MissionDataID))
        {
            _CurMissionItem = new MissionItem("100000");
            _CurMissionItem.InitMissionItem();
        }
        else
        {
            _CurMissionItem.InitMissionItem();
            if (string.IsNullOrEmpty(_CurMissionItem.MissionDataID))
            {
                _CurMissionItem = new MissionItem("100000");
            }
        }
    }

    public void SetCurMission(string missionID)
    {
        _CurMissionItem = new MissionItem(missionID);
        _CurMissionItem.InitMissionItem();
    }

    public void ComplateMission()
    {
        if (_CurMissionItem._MissionState == MissionState.Done)
        {
            if (_CurMissionItem.MissionGetAward())
            {
                UIMessageTip.ShowMessageTip(50003);
                int missionData = int.Parse(_CurMissionItem.MissionRecord.Id);
                string nextMissionID = (missionData + 1).ToString();
                if (TableReader.Mission.Records.ContainsKey(nextMissionID))
                {
                    _CurMissionItem = new MissionItem((missionData + 1).ToString());
                    _CurMissionItem.InitMissionItem();
                }
                else
                {
                    _CurMissionItem = null;
                }
            }
        }
        else if (_CurMissionItem._MissionState == MissionState.Finish)
        {
            int missionData = int.Parse(_CurMissionItem.MissionRecord.Id);
            string nextMissionID = (missionData + 1).ToString();
            if (TableReader.Mission.Records.ContainsKey(nextMissionID))
            {
                _CurMissionItem = new MissionItem((missionData + 1).ToString());
                _CurMissionItem.InitMissionItem();
            }
            else
            {
                _CurMissionItem = null;
            }
        }

        SaveClass(true);
    }

    #endregion
    

}
