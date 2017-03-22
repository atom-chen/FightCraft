using UnityEngine;
using System.Collections;

public class BulletDiaoChanBingCi : BulletBase
{
    //public Vector3 _StartSpeed;
    public GameObject _EffectAlert;
    public GameObject _SubEffect;
    public Collider _Trigger;
    public float _AlertTime = 0.6f;
    public float _ExplodeTime = 1f;
    public float _LifeTime = 1f;

    public override void Init(MotionManager senderMotion)
    {
        base.Init(senderMotion);

        _EffectAlert.SetActive(true);
        _SubEffect.SetActive(false);
        _Trigger.enabled = false;
    }

    public IEnumerator StartHit()
    {
        yield return new WaitForSeconds(_AlertTime);

        _EffectAlert.SetActive(false);
        _SubEffect.SetActive(true);
        _Trigger.enabled = true;

        yield return new WaitForSeconds(_ExplodeTime);

        //explode

        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
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
