
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;
using System;
using Tables;

 
public class UIBossStageSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/Stage/UIBossStageSelect", UILayer.PopUI, hash);
    }

    #endregion

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        InitDiffs();
    }

    #region difficult panel

    public UIContainerSelect _DiffContainer;

    private int _SelectedDiff = 0;

    public void InitDiffs()
    {
        int maxDiff = ActData.Instance._BossStageDiff;

        List<int> diffList = new List<int>();
        for (int i = 0; i < maxDiff + 1; ++i)
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

    private BossStageRecord _SelectedStage;

    public void InitStages()
    {
        int bossStageId = ActData.Instance._BossStageIdx;
        if (bossStageId == 0)
        {
            _SelectedStage = TableReader.BossStage.BossDiffRecords[_SelectedDiff][0];
        }
        else
        {
            _SelectedStage = TableReader.BossStage.GetRecord(bossStageId.ToString());
        }
        _StageContainer.InitSelectContent(TableReader.BossStage.BossDiffRecords[_SelectedDiff], 
            new List<BossStageRecord>() { _SelectedStage }, OnSelectStage);
    }

    private void OnSelectStage(object stageObj)
    {
        BossStageRecord stageInfo = stageObj as BossStageRecord;
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

    private void SetStageInfo(BossStageRecord stage)
    {
        _StageName.text = stage.Name;
        _StageLevel.text = stage.Level.ToString();
        _StageDesc.text = stage.Desc;
    }

    public void OnEnterStage()
    {
        ActData.Instance.StartStage(_SelectedDiff, int.Parse(_SelectedStage.Id), STAGE_TYPE.BOSS);
        LogicManager.Instance.EnterFight(Tables.TableReader.StageInfo.GetRecord("100"));
    }

    #endregion
}

