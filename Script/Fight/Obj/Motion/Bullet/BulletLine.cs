using UnityEngine;
using System.Collections;

public class BulletLine : BulletBase
{
    public float _LifeTime = 2.0f;
    public float _Speed = 10;

    // Use this for initialization
    public override void Init(MotionManager senderMotion)
    {
        base.Init(senderMotion);
    }

    private IEnumerator FinishDelay()
    {
        yield return new WaitForSeconds(_LifeTime);

        BulletFinish();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position += transform.forward.normalized * _Speed * Time.fixedDeltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter:" + other.ToString());
        var targetMotion = other.GetComponentInParent<MotionManager>();
        if (targetMotion == null)
            return;

        BulletHit(targetMotion);
        BulletFinish();
    }
}
