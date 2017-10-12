using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffIntervalSub : ImpactBuff
{

    public GameObject _SubImpactGO;
    public float _Interval = 1;

    private CapsuleCollider _Collider;
    private List<ImpactBase> _SubImpacts;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
        StartCoroutine(Interval());
    }

    private IEnumerator Interval()
    {
        yield return new WaitForSeconds(_Interval);
        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(_BuffSender, _BuffOwner);
        }
        
        StartCoroutine(Interval());
    }
    
}
