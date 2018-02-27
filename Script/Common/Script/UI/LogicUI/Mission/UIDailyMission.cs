
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIDailyMission : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Mission/UIDailyMission", UILayer.PopUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitContainer();
    }

    #region 

    public UIContainerBase _MissionContainer;
    public UIDailyMissionItem _TotalMissionItem;

    private void InitContainer()
    {
        _MissionContainer.InitContentItem(MissionData.Instance._MissionItems);
    }

    #endregion

}

