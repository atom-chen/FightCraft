using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterBase : ImpactBase
{
    public BulletBase _BulletPrefab;
    public Vector3 _EmitterOffset;

    protected MotionManager _SenderManager;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);
        _SenderManager = senderManager;
    }

    protected T InitBulletGO<T>() where T:class
    {
        var bulletObj = ResourcePool.Instance.GetIdleBullet(_BulletPrefab);
        bulletObj.transform.SetParent(ResourcePool.Instance.transform);
        bulletObj.transform.position = _SenderManager.transform.position + _EmitterOffset;
        bulletObj.transform.rotation = _SenderManager.transform.rotation;
        bulletObj.gameObject.SetActive(true);
        bulletObj.Init(_SenderManager);
        return bulletObj as T;
    }

    protected List<T> InitBulletGO<T>(int bulletCnt) where T :class
    {
        List<T> bullets = new List<T>();
        for (int i = 0; i < bulletCnt; ++i)
        {
            var bullet = InitBulletGO<T>();
            bullets.Add(bullet);
        }

        return bullets;
    }
}
