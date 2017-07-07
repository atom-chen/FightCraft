using UnityEngine;
using System.Collections;

public class BulletXiShiFeng : BulletBase
{
    //public Vector3 _StartSpeed;
    public float _Speed;
    public float _MaxRange;

    private Vector3 _InitPos;
    private float _MoveRange = 0;
    private bool _IsMoveBack = false;
    private Vector3 _MoveDirect;

    public override void Init(MotionManager senderMotion, BulletEmitterBase emitterBase)
    {
        base.Init(senderMotion, emitterBase);

        _InitPos = transform.position;
        _MoveRange = 0;
        _IsMoveBack = false;
    }

    public void SetDirect(Vector3 direct)
    {
        _MoveDirect = direct.normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var deltaDistance = _Speed * Time.fixedDeltaTime;
        if (!_IsMoveBack)
        {
            transform.position += _MoveDirect * deltaDistance;
            _MoveRange += deltaDistance;
            if (_MoveRange >= _MaxRange)
            {
                _IsMoveBack = true;
                _MoveRange = 0;
            }
        }
        else
        {
            transform.position -= _MoveDirect * deltaDistance;
            _MoveRange += deltaDistance;
            if (_MoveRange >= _MaxRange)
            {
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
    }
}
