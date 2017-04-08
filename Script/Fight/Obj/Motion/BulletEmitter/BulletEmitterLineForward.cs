using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BulletEmitterLineForward : BulletEmitterBase
{

    public float _EmitterTime;
    public float _EmitterLength;
    public float _EmitterNum;

    private float _Interval;
    private float _BulletDistance;
    private float _SendedCnt;

    private Vector3 _OriginPos;
    private Vector3 _OriginDirect;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _Interval = _EmitterTime / (_EmitterNum + 1);
        _BulletDistance = _EmitterLength / (_EmitterNum + 1);
        _SendedCnt = 0;

        _OriginPos = senderManager.transform.position;
        _OriginDirect = senderManager.transform.forward;
        //var bullet = InitBulletGO<BulletBase>();
        StartCoroutine(InitBullet());
    }

    private IEnumerator InitBullet()
    {
        var bullet = InitBulletGO<BulletBase>();
        bullet.transform.position = _OriginPos + (_SendedCnt + 1) * _BulletDistance * _OriginDirect;
        ++_SendedCnt;

        if (_SendedCnt < _EmitterNum)
        {
            yield return new WaitForSeconds(_Interval);
            StartCoroutine(InitBullet());
        }
    }
}
