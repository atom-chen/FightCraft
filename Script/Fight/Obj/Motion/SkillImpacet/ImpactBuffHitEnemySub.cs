using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffHitEnemySub : ImpactBuffSub
{
    public int _Rate;
    public bool _IsNeedTarget = false;
    
    public override void HitEnemy()
    {
        if (_IsNeedTarget)
            return;

        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            Debug.Log("GameRandom.IsInRate:" + _Rate);
            ActSubImpacts();
            SetCD();
        }
    }

    public override void HitEnemy(ImpactHit hitImpact, List<MotionManager> hittedMotions)
    {
        Debug.Log("HitEnemy:" + hittedMotions.Count);
        if (!_IsNeedTarget)
            return;

        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            int randomIdx = Random.Range(0, hittedMotions.Count);
            Debug.Log("GameRandom.IsInRate:" + _Rate);
            ActSubImpacts(_BuffOwner, hittedMotions[randomIdx]);
            SetCD();
        }
    }

}
