using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletLineHitInterval : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _MoveSpeed = 4;
    public float _HitInterval = 0.1f;
    public float _StayTime = 10;

    private float _LiftTime = 0;
    private float _NextSpeed = 0;
    private float _LastHitTime = 0;
    private Collider _Collider;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _Collider = gameObject.GetComponent<Collider>();
        _NextSpeed = _MoveSpeed;
        _LiftTime = _StayTime;
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * _NextSpeed * Time.fixedDeltaTime;

        if (_LastHitTime + _HitInterval < Time.time)
        {
            StartCoroutine(CalculateHit());
            _LastHitTime = Time.time;
        }

        _LiftTime -= Time.fixedDeltaTime;
        if (_LiftTime < 0)
        {
            BulletFinish();
        }
    }

    IEnumerator CalculateHit()
    {
        _Collider.enabled = true;

        yield return new WaitForFixedUpdate();

        _NextSpeed = _MoveSpeed;
        _Collider.enabled = false;
    }
    
    void OnTriggerEnter(Collider other)
    {
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        if (!_IsBulletHitLie && targetMotion.MotionPrior == BaseMotionManager.LIE_PRIOR)
            return;

        _NextSpeed = 0;
        BulletHit(targetMotion);
    }

}
