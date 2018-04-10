using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletBase : MonoBehaviour
{
    public bool _IsBulletHitLie;
    public int _BornAudio;
    protected ImpactBase[] _ImpactList;
    protected MotionManager _SkillMotion;
    public MotionManager SkillMotion
    {
        get
        {
            return _SkillMotion;
        }
    }

    protected BulletEmitterBase _EmitterBase;


    public virtual void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        _SkillMotion = senderMotion;
        _ImpactList = gameObject.GetComponents<ImpactBase>();
        _EmitterBase = emitterBase;

        if (_BornAudio > 0)
        {
            _SkillMotion.PlayAudio(ResourcePool.Instance._CommonAudio[_BornAudio]);
        }
    }

    protected virtual void BulletHit(MotionManager hitMotion)
    {
        foreach (var impact in _ImpactList)
        {
            impact.ActImpact(_SkillMotion, hitMotion);
        }
    }

    protected virtual void BulletFinish()
    {
        ResourcePool.Instance.RecvIldeBullet(this);
    }
}
