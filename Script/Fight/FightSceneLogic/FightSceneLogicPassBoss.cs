using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
 

public class FightSceneLogicPassBoss : FightSceneLogicPassArea
{

    public List<Transform> _PlayerTeleportPoses;

    public override void StartLogic()
    {
        var bossStage = TableReader.BossStage.GetRecord(ActData.Instance._ProcessStageIdx.ToString());
        for (int i = 0; i < _FightArea.Count; ++i)
        {
            if (_FightArea[i] is FightSceneAreaKBossWithFish)
            {
                (_FightArea[i] as FightSceneAreaKBossWithFish)._BossMotionID = bossStage.BossID[i].Id.ToString();
            }
        }

        base.StartLogic();
    }

    public override void AreaStart(FightSceneAreaBase startArea)
    {
        base.AreaStart(startArea);
        _IsTeleporting = false;
    }

    public override void AreaFinish(FightSceneAreaBase finishArea)
    {
        if (_RunningIdx + 1 < _PlayerTeleportPoses.Count)
        {
            _IsTeleporting = true;
            return;
        }
        else
        {
            base.AreaFinish(finishArea);
        }
    }

    public override void StartNextArea()
    {
        base.StartNextArea();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        UpdateTeleport();
    }

    #region telepor

    public static float _TeleDistance = 3;
    public static float _TeleProcessTime = 1;

    private bool _Teleporting = false;
    private float _StartingTime = 0;

    private bool _IsTeleporting = false;

    private void UpdateTeleport()
    {
        if (!_IsTeleporting)
            return;

        var timeDelta = Time.time - _StartingTime;
        FightManager.Instance.MainChatMotion._SkillProcessing = timeDelta / _TeleProcessTime;
        if (FightManager.Instance.MainChatMotion._SkillProcessing >= 1)
        {
            FightManager.Instance.TeleportToNextRegion(_PlayerTeleportPoses[_RunningIdx + 1], true);
            FightManager.Instance.MainChatMotion._SkillProcessing = 0;
            _Teleporting = false;
        }
    }

    #endregion
}
