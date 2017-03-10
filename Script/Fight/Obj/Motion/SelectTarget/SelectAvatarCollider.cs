using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectAvatarCollider : SelectBase
{
    public int _AvatarCnt = 3;

    private List<Collider> _TrigCollider = new List<Collider>();
    private int _HitCnt = 0;
    private Collider[] _Colliders;

    public override void Init()
    {
        base.Init();

        _Colliders = GetComponentsInChildren<Collider>();
    }

    public override void ColliderFinish()
    {
        base.ColliderFinish();

        _TrigCollider.Clear();
    }

    public override void ColliderStart()
    {
        gameObject.SetActive(true);

        foreach (var collider in _Colliders)
        {
            collider.enabled = true;
        }

        StartCoroutine(AvatarHit());

        if (!_IsColliderFinish)
        {
            StartCoroutine(ColliderDisable());
        }
    }

    public IEnumerator ColliderDisable()
    {
        yield return new WaitForFixedUpdate();

        foreach (var collider in _Colliders)
        {
            collider.enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        
        if (!_TrigCollider.Contains(other))
        {
            _TrigCollider.Add(other);
            
            //var motion = other.gameObject.GetComponentInParent<MotionManager>();
            //if (motion != null)
            //{
            //    AvatarHit(motion);
            //}
        }
        _HitCnt = 1;
    }

    IEnumerator AvatarHit()
    {
        yield return new WaitForSeconds(0.03f);
        Debug.Log("AvatarHit:" + Time.time);
        foreach (var target in _TrigCollider)
        {
            var motion = target.gameObject.GetComponentInParent<MotionManager>();
            if (motion != null)
            {
                AvatarHit(motion);
            }
        }
        ++_HitCnt;

        if (_HitCnt < _AvatarCnt)
        {
            StartCoroutine(AvatarHit());
        }
        else
        {
            StartCoroutine(ActHit());
        }
    }

    void AvatarHit(MotionManager targetMotion)
    {
        //foreach (var impact in _ImpactList)
        //{
        //    if(impact is ImpactHit)
        //        impact.ActImpact(_SkillMotion, targetMotion);
        //}

        if (_SkillMotion._IsRoleHit)
        {
            GlobalEffect.Instance.Pause(0.05f);
        }
    }

    IEnumerator ActHit()
    {
        yield return new WaitForSeconds(0.03f);

        foreach (var target in _TrigCollider)
        {
            var motion = target.gameObject.GetComponentInParent<MotionManager>();
            if (motion != null)
            {
                foreach (var impact in _ImpactList)
                {
                    impact.ActImpact(_SkillMotion, motion);
                }
            }
        }

        if (_SkillMotion._IsRoleHit)
        {
            GlobalEffect.Instance.Pause(0.05f);
        }

        ColliderFinish();
    }

}
