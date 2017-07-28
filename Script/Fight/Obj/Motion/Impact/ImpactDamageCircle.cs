using UnityEngine;
using System.Collections;

public class ImpactDamageCircle : ImpactBuff
{
    public float _HitInterval = 0.2f;
    public float _DamageRate = 1;

    private CapsuleCollider _TriggerCollider;
    private float _HitTime;
    private bool _IsHit;

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        this.enabled = true;
        _TriggerCollider = gameObject.AddComponent<CapsuleCollider>();
        _TriggerCollider.radius = 3;
        _TriggerCollider.isTrigger = true;
        _TriggerCollider.gameObject.layer = FightLayerCommon.GetBulletLayer(reciverManager);
        _HitTime = Time.time;
        _IsHit = true;
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        this.enabled = false;
        GameObject.DestroyImmediate(_TriggerCollider);
    }

    void FixedUpdate()
    {
        if (Time.time - _HitInterval > _HitTime)
        {
            _HitTime = Time.time;
            StartCoroutine(CalculateHit());
        }
    }

    IEnumerator CalculateHit()
    {
        _TriggerCollider.enabled = true;

        yield return new WaitForFixedUpdate();

        _TriggerCollider.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        _BuffSender.RoleAttrManager.SendDamageEvent(targetMotion, _DamageRate, _SkillMotion);
    }
}
