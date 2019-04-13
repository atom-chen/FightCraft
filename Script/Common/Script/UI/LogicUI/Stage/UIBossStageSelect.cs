
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
        //int maxDiff = 9;

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

        UnSelectAct();
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
    public Text _Condition;
    public UICurrencyItem _UITicket;
    public Text _CostTicket;
    public Text _ActTicketDesc;

    private void SetStageInfo(BossStageRecord stage)
    {
        int stageID = int.Parse(stage.Id);
        int stageLevel = GameDataValue.GetStageLevel(_SelectedDiff, stageID, STAGE_TYPE.NORMAL);
        _StageName.text = StrDictionary.GetFormatStr(stage.Name);
        _StageLevel.text = stageLevel.ToString();
        _StageDesc.text = StrDictionary.GetFormatStr(stage.Desc);
        _UITicket.ShowOwnCurrency(ActData._BOSS_TICKET);
        _CostTicket.text = "/1";
        _ActTicketDesc.text = "";
        _Condition.text = "";

    }

    public void OnEnterStage()
    {
        if (_ActSelected.activeSelf)
        {
            if (ActData.Instance.IsCanStartAct(_UsingTicket))
            {
                ActData.Instance.StartStage(_SelectedDiff, int.Parse(_SelectedStage.Id), STAGE_TYPE.ACTIVITY);
                LogicManager.Instance.EnterFight(_SelectedActStage);
            }
        }
        else
        {
            if (ActData.Instance.IsCanStartBossStage())
            {
                ActData.Instance.StartStage(_SelectedDiff, int.Parse(_SelectedStage.Id), STAGE_TYPE.BOSS);
                LogicManager.Instance.EnterFight(Tables.TableReader.StageInfo.GetRecord("100"));
            }
            else
            {
                UIMessageTip.ShowMessageTip(71106);
            }
        }
    }

    #endregion

    #region act 

    public GameObject _ActSelected;
    public GameObject _StagePanel;
    public GameObject _ActPanel;
    public UIContainerSelect _ActStageContainer;

    private static List<string> _StageRecord = new List<string>() { "200", "201" };
    private bool _InitAct = false;
    private StageInfoRecord _SelectedActStage;
    private int _UsingTicket;

    public void OnBtnActDiff()
    {
        if (_ActSelected.activeSelf)
            return;

        _ActSelected.SetActive(true);
        _DiffContainer.SetSelect(null);
        SelectAct();
    }

    public void SelectAct()
    {
        _StagePanel.SetActive(false);
        _ActPanel.SetActive(true);
        if (_InitAct)
            return;
        _InitAct = true;

        List<StageInfoRecord> stageRecords = new List<StageInfoRecord>();
        for (int i = 0; i < _StageRecord.Count; ++i)
        {
            var stageRecord = TableReader.StageInfo.GetRecord(_StageRecord[i]);
            stageRecords.Add(stageRecord);
        }
        _ActStageContainer.InitSelectContent(stageRecords, new List<StageInfoRecord>() { stageRecords[0] }, OnSelectActStage, null);
    }

    public void UnSelectAct()
    {
        _StagePanel.SetActive(true);
        _ActPanel.SetActive(false);
        if (_ActSelected.activeSelf)
        {
            _ActSelected.SetActive(false);
        }
    }

    private void OnSelectActStage(object stageObj)
    {
        StageInfoRecord stageInfo = stageObj as StageInfoRecord;
        if (stageInfo == null)
            return;

        _SelectedActStage = stageInfo;

        SetStageInfo(_SelectedStage);
        SetActTicketDesc();
    }

    public void AddActUsingTicket()
    {
        ++_UsingTicket;
        SetActTicketDesc();
    }

    public void DecActUsingTicket()
    {
        --_UsingTicket;
        _UsingTicket = Mathf.Max(0, _UsingTicket);
        SetActTicketDesc();
    }

    private void SetActTicketDesc()
    {
        int Ownvalue = BackBagPack.Instance.PageItems.GetItemCnt(ActData._ACT_TICKET);
        _UITicket.ShowCurrency(ActData._ACT_TICKET, Ownvalue);
        _UsingTicket = Mathf.Min(_UsingTicket, Ownvalue, ActData._MAX_ACT_USING_TICKET);
        _CostTicket.text = "/" + _UsingTicket.ToString();
        _ActTicketDesc.text = StrDictionary.GetFormatStr(71105, CommonDefine.GetQualityItemName(ActData._ACT_TICKET, true), _UsingTicket, ActData._DropRates[_UsingTicket]);
    }

    #endregion
}

