
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

public class UIStageSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Stage/UIStageSelect", UILayer.PopUI, hash);
    }

    public static int GetMaxStageID()
    {
        var instance = GameCore.Instance.UIManager.GetUIInstance<UIStageSelect>("LogicUI/Stage/UIStageSelect");
        if (instance == null)
            return -1;

        return instance.MaxStageId;
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitDiffs();
    }

    #region difficult panel

    public UIContainerSelect _DiffContainer;

    private int _SelectedDiff;

    public void InitDiffs()
    {
        int maxDiff = ActData.Instance._NormalStageDiff;
        int maxStage = TableReader.StageInfo.GetMaxNormalStageID();
        if (ActData.Instance._NormalStageIdx == 0 || ActData.Instance._NormalStageIdx == maxStage)
        {
            ++maxDiff;
        }

        List<int> diffList = new List<int>();
        for (int i = 1; i < maxDiff + 1; ++i)
        {
            diffList.Add(i);
        }
        _DiffContainer.InitSelectContent(diffList, new List<int>() { maxDiff }, OnDiffSelect);
    }

    public void OnDiffSelect(object diffObj)
    {
        _SelectedDiff = (int)diffObj;
        InitStages();
    }
    
    #endregion

    #region stagePanel

    public UIContainerSelect _StageContainer;

    private int _MaxStageId = 0;
    public int MaxStageId
    {
        get
        {
            return _MaxStageId;
        }
    }

    private int _NormalMinID = 1;
    private int _NormalMaxID = 21;
    private int _RandomMinID = 21;
    private int _RandomMaxID = 41;

    private StageInfoRecord _SelectedStage;

    public void InitStages()
    {
        int maxStage = ActData.Instance._NormalStageIdx;
        if (_SelectedDiff < ActData.Instance._NormalStageDiff)
        {
            maxStage = TableReader.StageInfo.GetMaxNormalStageID();
            if (maxStage < _RandomMaxID)
            {
                maxStage = maxStage - _RandomMinID;
            }
            else
            {
                maxStage = maxStage - _NormalMinID;
            }
        }
        else if (_SelectedDiff > ActData.Instance._NormalStageDiff)
        {
            maxStage = 0;
        }
        else
        {
            ++maxStage;
            maxStage = Mathf.Clamp(maxStage, 0, 19);
        }

        List<StageInfoRecord> stageList = new List<StageInfoRecord>();
        if (_SelectedDiff == 1)
        {
            for (int i = _NormalMinID; i < _NormalMaxID; ++i)
            {
                stageList.Add(TableReader.StageInfo.GetRecord(i.ToString()));
            }
        }
        else
        {
            for (int i = _RandomMinID; i < _RandomMaxID; ++i)
            {
                stageList.Add(TableReader.StageInfo.GetRecord(i.ToString()));
            }
        }
        _MaxStageId = int.Parse(stageList[maxStage].Id);
        //_MaxStageId = 2;
        _StageContainer.InitSelectContent(stageList, new List<StageInfoRecord>() { stageList[maxStage] }, OnSelectStage);
    }

    private void OnSelectStage(object stageObj)
    {
        StageInfoRecord stageInfo = stageObj as StageInfoRecord;
        if (stageInfo == null)
            return;

        _SelectedStage = stageInfo;

        SetStageInfo(_SelectedStage);
    }

    #endregion

    #region stageInfo

    public Text _StageName;
    public Text _StageLevel;
    public Text _StageDesc;

    private void SetStageInfo(StageInfoRecord stage)
    {
        _StageName.text = stage.Name;
        _StageLevel.text = "1";
        _StageDesc.text = stage.Desc;
    }

    public void OnEnterStage()
    {
        ActData.Instance.StartStage(1, int.Parse(_SelectedStage.Id), STAGE_TYPE.NORMAL);
        LogicManager.Instance.EnterFight(_SelectedStage);
    }

    #endregion
}

