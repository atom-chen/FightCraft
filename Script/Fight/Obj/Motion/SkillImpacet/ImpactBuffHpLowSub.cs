using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffHpLowSub : ImpactBuffCD
{
    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
    }

    public override void UpdateBuff()
    {
        base.UpdateBuff();

        if (_BuffOwner.RoleAttrManager.HPPersent <= _HPRate && !IsInCD())
        {
            ActSubImpacts();
        }
    }

    #region hp rate

    public float _HPRate = 0.5f;

    #endregion

    #region sub impact

    public GameObject _SubImpactGO;

    protected List<ImpactBase> _SubImpacts;

    private void ActSubImpacts()
    {
        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(_BuffSender, _BuffOwner);
        }
    }
    #endregion
}
