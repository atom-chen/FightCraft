using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class ActData : DataPackBase
{
    #region 单例

    private static ActData _Instance;
    public static ActData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new ActData();
            }
            return _Instance;
        }
    }

    private ActData()
    {
        _SaveFileName = "ActData";
    }

    #endregion

    #region 

    public static int LEVEL_LIMIT = 10;

    public void InitActData()
    {
        if (_NormalStageIdx <= 0)
            _NormalStageIdx = 0;

        if (_BossStageIdx <= 0)
            _BossStageIdx = 0;
    }

    public int _ProcessStageIdx;
    public STAGE_TYPE _StageMode;

    public void StartStage(int stageIdx, Tables.STAGE_TYPE stageMode)
    {
        _ProcessStageIdx = stageIdx;
        _StageMode = stageMode;
    }

    public int GetStageLevel()
    {
        return GameDataValue.GetStageLevel(_ProcessStageIdx, _StageMode);
    }

    public void PassStage(Tables.STAGE_TYPE stageMode)
    {
        switch (stageMode)
        {
            case STAGE_TYPE.NORMAL:
                SetPassNormalStage(_ProcessStageIdx);
                break;
            case STAGE_TYPE.BOSS:
                SetPassBossStage(_ProcessStageIdx);
                break;
        }

        Hashtable hash = new Hashtable();
        hash.Add("StageType", stageMode);
        hash.Add("StageIdx", _ProcessStageIdx);
        if (stageMode == STAGE_TYPE.NORMAL)
        {
            hash.Add("StageDiff", GetNormalDiff());
        }
        else
        {
            hash.Add("StageDiff", GetBossDiff());
        }
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_PASS_STAGE, this, hash);
    }

    #endregion

    #region normal 

    [SaveField(1)]
    public int _NormalStageIdx = 0;

    public static int _MAX_NORMAL_DIFF = 9;
    public static int _CIRCLE_STAGE_COUNT = 20;

    public void SetPassNormalStage(int stageIdx)
    {
        if (_NormalStageIdx >= stageIdx)
            return;
        _NormalStageIdx = stageIdx;

        SaveClass(false);
    }

    public void GetNextNormalStateId()
    {

    }

    public int GetNormalDiff()
    {
        return _NormalStageIdx / _MAX_NORMAL_DIFF + 1;
    }

    #endregion

    #region boss

    [SaveField(2)]
    public int _BossStageIdx = 0;

    public static int _MAX_BOSS_DIFF = 9;
    public static string _BOSS_TICKET = "1600000";

    public void SetPassBossStage(int stageIdx)
    {
        if (_BossStageIdx >= stageIdx)
            return;
        
        _BossStageIdx = stageIdx;

        SaveClass(false);
    }

    public bool IsCanStartBossStage()
    {
        return BackBagPack.Instance.PageItems.DecItem(ActData._BOSS_TICKET, 1);
    }

    public bool IsBossStageLock(BossStageRecord bossStageRecord)
    {
        int stageID = int.Parse(bossStageRecord.Id);
        if (stageID > _BossStageIdx + 2)
        {
            return false;
        }

        if (RoleData.SelectRole._RoleLevel < bossStageRecord.Level)
        {
            return false;
        }

        if (RoleData.SelectRole._CombatValue < bossStageRecord.Combat)
            return false;

        return true;
    }

    public int GetBossDiff()
    {
        return _BossStageIdx / _MAX_NORMAL_DIFF + 1;
    }

    #endregion

    #region act

    public static string _ACT_TICKET = "1600001";
    public static int _MAX_ACT_USING_TICKET = 5;
    public static int _MAX_START_ACT_LEVEL = 20;
    public static List<float> _DropRates = new List<float>() { 1, 1.95f, 2.85f, 3.7f, 4.5f, 5.25f };

    public int _ActConsumeTickets = 0;

    public bool IsCanStartAct(int consumeTickets)
    {
        if (BackBagPack.Instance.PageItems.DecItem(ActData._ACT_TICKET, consumeTickets))
        {
            _ActConsumeTickets = consumeTickets;
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}
