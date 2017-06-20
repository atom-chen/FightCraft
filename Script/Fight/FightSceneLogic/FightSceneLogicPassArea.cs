using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameUI;

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

        
    }

    public override void MotionDie(MotionManager motion)
    {
        base.MotionDie(motion);

        if (_RunningArea != null)
        {
            _RunningArea.MotionDie(motion);
        }
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

    private void StartNextArea()
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
