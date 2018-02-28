using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class AchievementData : DataPackBase
{

    #region 唯一

    private static AchievementData _Instance = null;
    public static AchievementData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new AchievementData();
            }
            return _Instance;
        }
    }

    private AchievementData()
    {
        _SaveFileName = "AchievementData";
    }

    #endregion

    #region 

    [SaveField(1)]
    public List<MissionItem> _MissionItems;

    public void InitMissionData()
    {
        List<MissionRecord> achieveMission = new List<MissionRecord>();
        foreach (var missionTab in TableReader.Mission.Records)
        {
            if (missionTab.Value.Achieve == 1)
            {
                achieveMission.Add(missionTab.Value);
            }
        }

        if (_MissionItems == null || achieveMission.Count != _MissionItems.Count)
        {
            _MissionItems = new List<MissionItem>();
            foreach (var achieve in achieveMission)
            {
                MissionItem missionItem = new MissionItem(achieve);
                _MissionItems.Add(missionItem);
            }
            SaveClass(true);
        }

        foreach (var mission in _MissionItems)
        {
            mission.InitMissionItem();
        }
    }
    

    #endregion

}
