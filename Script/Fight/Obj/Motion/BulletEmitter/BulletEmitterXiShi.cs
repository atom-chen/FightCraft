using UnityEngine;
using System.Collections;

public class BulletEmitterXiShi : BulletEmitterBase
{
    public int _BulletCnt = 8;
    public float _RotSpeed = 60.0f;

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        gameObject.SetActive(true);
        EmitBullet();
    }

    private void EmitBullet()
    {
        for (int i = 0; i < _BulletCnt; ++i)
        {
            SendBullet(i);
        }
    }

    private void SendBullet(int idx)
    {
        var bullet = InitBulletGO<BulletBase>();
        float angle = idx * (360 / _BulletCnt) - 360 * 0.5f;

        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, _SenderManager.transform.rotation.eulerAngles.y + angle, 0));
        bullet.transform.SetParent(transform);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y + Time.fixedDeltaTime * _RotSpeed, 0));
    }
}
