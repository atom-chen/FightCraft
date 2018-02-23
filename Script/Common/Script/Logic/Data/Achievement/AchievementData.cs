using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tables;

public class AchievementData : DataPackBase
{

    public void InitAchievement()
    {
        InitAchieveItems();
        InitAchieveData();
    }

    #region achieveitems

    [SaveField(1)]
    public List<AchievementItem> _AchieveItemList;

    public void InitAchieveItems()
    {
        if (_AchieveItemList == null || _AchieveItemList.Count == 0)
        {
            foreach (var achieveRecord in TableReader.Achievement.Records.Values)
            {
                AchievementItem achieveItem = new AchievementItem();
                achieveItem._AchieveDataID = achieveRecord.Id;
                achieveItem._AchieveState = AchieveState.Processing;
            }
        }
    }

    public Dictionary<string, AchievementItem> GetShowAchieveList()
    {
        Dictionary<string, AchievementItem> showList = new Dictionary<string, AchievementItem>();

        foreach (var achieveItem in _AchieveItemList)
        {
            if (achieveItem._AchieveState == AchieveState.Done
                || achieveItem._AchieveState == AchieveState.Processing
                || achieveItem.AchieveTabRecord.ClassFinal)
            {
                if (!showList.ContainsKey(achieveItem.AchieveTabRecord.ClassScript))
                {
                    showList.Add(achieveItem.AchieveTabRecord.ClassScript, achieveItem);
                }
            }
        }

        return showList;
    }

    #endregion

    #region achieve data

    [SaveField(2)]
    private List<int> _AchieveData;

    private List<string> _AchieveScripts = new List<string>();

    public void InitAchieveData()
    {
        foreach (var achieveRecord in TableReader.Achievement.Records.Values)
        {
            if (!_AchieveScripts.Contains(achieveRecord.ClassScript))
            {
                _AchieveScripts.Add(achieveRecord.ClassScript);
            }
        }

        if (_AchieveData == null || _AchieveScripts.Count != _AchieveData.Count)
        {
            _AchieveData = new List<int>();
            for (int i = 0; i < _AchieveScripts.Count; ++i)
            {
                _AchieveData.Add(0);
            }
        }
    }

    public int GetAchieveData(string scriptName)
    {
        int idx = _AchieveScripts.IndexOf(scriptName);
        return _AchieveData[idx];
    }

    public void SetAchieveData(string scriptName, int value)
    {
        int idx = _AchieveScripts.IndexOf(scriptName);
        _AchieveData[idx] = value;
    }

    #endregion

}
