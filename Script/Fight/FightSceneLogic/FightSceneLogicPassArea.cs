using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 

public class FightSceneLogicPassArea : FightSceneLogicBase
{

    #region 


    public List<FightSceneAreaBase> _FightArea;

    #endregion

    private FightSceneAreaBase _RunningArea;
    private int _RunningIdx;

    public override void StartLogic()
    {
        base.StartLogic();

        _RunningIdx = -1;

        StartNextArea();

    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        if(_RunningArea != null)
            _RunningArea.UpdateArea();
    }

    public override void MotionDie(MotionManager motion)
    {
        base.MotionDie(motion);

        if (_RunningArea != null)
        {
            _RunningArea.MotionDie(motion);
        }
    }

    public Vector3 GetNextAreaPos()
    {
        FightSceneAreaBase nextArea = _RunningArea;
        if (nextArea == null)
        {
            if (_RunningIdx + 1 < _FightArea.Count)
            {
                nextArea = _FightArea[_RunningIdx + 1];
            }
        }

        if (nextArea is FightSceneAreaKAllEnemy)
        {
            return (nextArea as FightSceneAreaKAllEnemy)._EnemyBornPos[0]._EnemyTransform.position;
        }
        else if (nextArea is FightSceneAreaKEnemyCnt)
        {
            return (nextArea as FightSceneAreaKEnemyCnt)._EnemyBornPos[0].position;
        }
        else if (nextArea is FightSceneAreaKBossWithFish)
        {
            return (nextArea as FightSceneAreaKBossWithFish)._BossBornPos.position;
        }
        else if (nextArea is FightSceneAreaKShowTeleport)
        {
            return (nextArea as FightSceneAreaKShowTeleport)._Teleport.transform.position;
        }
        return Vector3.zero;
    }

    #region 

    public void AreaStart(FightSceneAreaBase startArea)
    {
        _RunningArea = startArea;
        startArea.StartArea();
    }

    public void AreaFinish(FightSceneAreaBase finishArea)
    {
        if (finishArea == _RunningArea)
        {
            _RunningArea = null;
        }
        StartNextArea();
    }

    public void StartNextArea()
    {
        ++_RunningIdx;
        if (_RunningIdx < _FightArea.Count)
        {
            if (_FightArea[_RunningIdx]._TrigAreaType == FightSceneAreaBase.TrigType.TRIG_AUTO)
            {
                AreaStart(_FightArea[_RunningIdx]);
            }

            UIFightWarning.ShowDirectAsyn(FightManager.Instance.MainChatMotion.transform, _FightArea[_RunningIdx].GetAreaTransform());
        }
        else
        {
            LogicFinish(true);
        }
    }



    #endregion
}
