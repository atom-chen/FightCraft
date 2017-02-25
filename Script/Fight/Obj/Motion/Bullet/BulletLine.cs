using UnityEngine;
using System.Collections;

public class BulletLine : MonoBehaviour
{
    public float lifeTime = 2.0f;
    public float speed;

    private MotionManager _SkillMotion;
    private ImpactBase[] _ImpactList;

    // Use this for initialization
    void Start ()
    {
        GameObject.Destroy(gameObject, lifeTime);

        _SkillMotion = gameObject.GetComponentInParent<MotionManager>();
        _ImpactList = gameObject.GetComponentsInChildren<ImpactBase>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position += transform.forward.normalized * speed * Time.fixedDeltaTime;
	}

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        var motion = other.gameObject.GetComponent<MotionManager>();
        if (motion != null)
        {
            foreach (var impact in _ImpactList)
            {
                impact.ActImpact(_SkillMotion, motion);
            }
        }
    }
}
