using UnityEngine;
using System.Collections;

public class BulletEmitterRandomSplit : BulletEmitterBase
{
    public int _BulletCnt = 5;
    public int _MaxRange = 8;
    public float _SplitRange = 0.5f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        for (int i = 0; i < _BulletCnt; ++i)
        {
            EmitBullet();
        }
    }

    private void EmitBullet()
    {
        var bullet = InitBulletGO<BulletBase>();
        int splitCnt = (int)(_MaxRange / _SplitRange);
        var randomX = Random.Range(-splitCnt, splitCnt);
        var randomY = Random.Range(-splitCnt, splitCnt);

        Vector3 destPos = new Vector3(randomX * _SplitRange, 0, randomY * _SplitRange);
        bullet.transform.position = _SenderManager.transform.position + destPos;
        Debug.Log("bullet position:" + bullet.transform.position);
    }
}
