using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletMuTong : BulletSummon
{
    public float _DelayTime = 1;
    public AnimationClip _HitAnimation;
    public BulletEmitterBase _SubBulletEmitter;

    private Animation _Animation;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        gameObject.layer = FightLayerCommon.EVIL;

        _Animation = gameObject.GetComponentInChildren<Animation>();
        if (_Animation != null && _HitAnimation != null)
        {
            _Animation.AddClip(_HitAnimation, _HitAnimation.name);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());

        var collider = gameObject.GetComponent<Collider>();
        collider.enabled = false;

        if (_Animation != null && _HitAnimation != null)
        {
            _Animation.Play(_HitAnimation.name);
        }

        _SubBulletEmitter.ActImpact(_SkillMotion, _SkillMotion);
        Invoke("BulletFinish", 1.5f);
    }
}
