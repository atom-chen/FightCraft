using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffSub : ImpactBuffCD
{
    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
    }

    #region sub impact

    public GameObject _SubImpactGO;

    protected List<ImpactBase> _SubImpacts;

    protected void ActSubImpacts()
    {
        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(_BuffSender, _BuffOwner);
        }
    }

    protected void ActSubImpacts(MotionManager sender, MotionManager reciver)
    {
        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(sender, reciver);
        }
    }
    #endregion
}
