
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIAchievement : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Achievement/UIAchievement", UILayer.PopUI, hash);
    }

    #endregion

    public override void Init()
    {
        base.Init();

        InitAchieveType();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _Achievetype.ShowDefaultFirst();
    }

    #region 

    public UIContainerBase _AchieveContainer;
    public UISubScollMenu _Achievetype;

    private List<string> _AchieveTypes = new List<string>();
    private void InitAchieveType()
    {
        _AchieveTypes.Clear();

        foreach (var achieveItem in AchievementData.Instance._MissionItems)
        {
            if (!_AchieveTypes.Contains(achieveItem.MissionRecord.Class))
            {
                _AchieveTypes.Add(achieveItem.MissionRecord.Class);
            }
        }

        foreach (var achieveType in _AchieveTypes)
        {
            _Achievetype.PushMenu(achieveType);
        }
    }


    public void ShowAchieveClass(object achieveClassObj)
    {
        string achieveClass = achieveClassObj.ToString();
        Dictionary<string, MissionItem> missionItems = new Dictionary<string, MissionItem>();
        foreach (var achieveItem in AchievementData.Instance._MissionItems)
        {
            if (achieveItem.MissionRecord.Class == achieveClass)
            {
                if (missionItems.ContainsKey(achieveItem.MissionRecord.SubClass))
                {
                    if (missionItems[achieveItem.MissionRecord.SubClass]._MissionState == MissionState.Finish
                        && missionItems[achieveItem.MissionRecord.SubClass].MissionRecord.HardStar < achieveItem.MissionRecord.HardStar)
                    {
                        missionItems.Add(achieveItem.MissionRecord.SubClass, achieveItem);
                    }
                }
                else
                {
                    missionItems.Add(achieveItem.MissionRecord.SubClass, achieveItem);
                }
            }
        }

        _AchieveContainer.InitContentItem(missionItems.Values);
    }

    #endregion

}

