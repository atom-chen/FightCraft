using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactBuffIntervalRangeSub : ImpactBuff
{
    public GameObject _SubImpactGO;
    public float _Interval = 1;

    protected CapsuleCollider _Collider;
    protected List<ImpactBase> _SubImpacts;
    public float _Range = 1.5f;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        _Collider = gameObject.AddComponent<CapsuleCollider>();
        _Collider.direction = 1;
        _Collider.radius = _Range;
        _Collider.height = 2;
        _Collider.center = new Vector3(0, 1, 0);
        _Collider.enabled = false;
        _Collider.isTrigger = true;
        gameObject.layer = FightLayerCommon.GetBulletLayer(senderManager);

        _SubImpacts = new List<ImpactBase>(_SubImpactGO.GetComponentsInChildren<ImpactBase>());
        StartCoroutine(Interval());
    }

    private IEnumerator Interval()
    {
        yield return new WaitForSeconds(_Interval);

        _Collider.enabled = true;
        yield return new WaitForFixedUpdate();
        _Collider.enabled = false;

        StartCoroutine(Interval());
    }

    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion.IsMotionDie)
            return;

        foreach (var subImpact in _SubImpacts)
        {
            subImpact.ActImpact(_BuffSender, targetMotion);
        }
        Debug.Log("OnTriggerStay:" + targetMotion.name);
    }

}
