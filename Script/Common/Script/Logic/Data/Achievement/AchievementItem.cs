using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public enum AchieveState
{
    None,
    Processing,
    Done, //not get award;
    Finish,
}

public class AchievementItem : SaveItemBase
{
    [SaveField(1)]
    public string _AchieveDataID;

    public Tables.AchievementRecord AchieveTabRecord
    {
        get
        {
            return TableReader.Achievement.GetRecord(_AchieveDataID);
        }
    }

    [SaveField(2)]
    public AchieveState _AchieveState;

    #region 

    #endregion
}
