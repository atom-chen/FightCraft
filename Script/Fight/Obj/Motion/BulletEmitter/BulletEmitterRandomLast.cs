using UnityEngine;
using System.Collections;

public class BulletEmitterRandomLast : BulletEmitterElement
{
    public int _RoundBulletCnt = 5;
    public int _RoundCount = 1;
    public float _RoundInterval = 0.1f;
    public int _MaxRange = 8;

    private int _RountIdx;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        _RountIdx = 0;
        gameObject.SetActive(true);
        StartCoroutine(EmitBullet());
    }

    private IEnumerator EmitBullet()
    {
        for (int i = 0; i < _RoundBulletCnt; ++i)
        {
            var bullet = InitBulletGO<BulletBase>();
            float randX = Random.Range(-_MaxRange, _MaxRange);
            float randZ = Random.Range(-_MaxRange, _MaxRange);
            //float angle = Random.Range(0, 360);
            //angle = angle / Mathf.Rad2Deg;
            //Vector3 direct = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
            //float range = Random.Range(0, _MaxRange);
            //if (i > _BulletCnt * 0.5f)
            //{
            //    sinAngle = -sinAngle;
            //}

            bullet.transform.position = transform.position + _EmitterOffset + new Vector3(randX, 0, randZ);
        }

        ++_RountIdx;
        if (_RountIdx >= _RoundCount)
        {
            yield break;
        }
        else
        {
            yield return new WaitForSeconds(_RoundInterval);
            StartCoroutine(EmitBullet());
        }
    }
}
