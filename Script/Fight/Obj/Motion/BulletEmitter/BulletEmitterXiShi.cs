using UnityEngine;
using System.Collections;

public class BulletEmitterXiShi : BulletEmitterBase
{
    public int _BulletCnt = 8;
    public float _Interval = 0.1f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        gameObject.SetActive(true);
        StartCoroutine(EmitBullet());
    }

    private IEnumerator EmitBullet()
    {
        for (int i = 0; i < _BulletCnt; ++i)
        {
            var bullet = InitBulletGO<BulletXiShiFeng>();
            float angle = i * (360.0f / _BulletCnt) / Mathf.Rad2Deg;
            float sinAngle = Mathf.Sin(angle);
            //if (i > _BulletCnt * 0.5f)
            //{
            //    sinAngle = -sinAngle;
            //}

            Vector3 direct = new Vector3(sinAngle, 0, Mathf.Cos(angle));

            bullet.SetDirect(direct);
            yield return new WaitForSeconds(_Interval);
        }
    }
}
