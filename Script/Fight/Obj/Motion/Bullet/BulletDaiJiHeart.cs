using UnityEngine;
using System.Collections;

public class BulletDaiJiHeart : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _GravityModifier;
    public float _TrackAccelate;
    public EffectController _SubEffect;

    private MotionManager _TargetMotion;
    private Vector3 _UpSpeed;
    private Vector3 _TargetPosition;
    private Vector3 _TrackSpeed;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        var target = SelectTargetCommon.GetMainPlayer();
        if (target != null)
        {
            _TargetMotion = target.GetComponent<MotionManager>();
        }
    }

    public void SetInitSpeed(Vector3 initSpeed)
    {
        _UpSpeed = initSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_UpSpeed.y > 0)
        {
            transform.position += _UpSpeed * Time.fixedDeltaTime;
            _UpSpeed.y -= _GravityModifier * 10 * Time.fixedDeltaTime;
            if (_UpSpeed.y <= 0)
            {
                _TargetPosition = _TargetMotion.transform.position;
                var targetDirect = _TargetPosition - transform.position;
                _TrackSpeed = targetDirect.normalized * _UpSpeed.magnitude;
            }
        }
        else
        {
            transform.position += _TrackSpeed * Time.fixedDeltaTime;
            _TrackSpeed += _TrackAccelate * _TrackSpeed.normalized;
            if (transform.position.y < _TargetPosition.y)
            {
                if (_SubEffect != null)
                {
                    ResourcePool.Instance.PlaySceneEffect(_SubEffect, transform.position, Vector3.zero);
                }

                BulletFinish();
            }
        }
    }

    protected override void BulletFinish()
    {
        base.BulletFinish();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);

        if (_SubEffect != null)
        {
            ResourcePool.Instance.PlaySceneEffect(_SubEffect, transform.position, Vector3.zero);
        }

        BulletFinish();
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollierEnter:" + other.ToString());
    }
}
