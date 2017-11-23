using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffHitEnemySub : ImpactBuffSub
{
    public int _Rate;
    
    public override void HitEnemy()
    {
        GameRandom.IsInRate(_Rate);
        if (!IsInCD())
        {
            if (!GameRandom.IsInRate(_Rate))
                return;

            ActSubImpacts();
            SetCD();
        }
    }
    
}
