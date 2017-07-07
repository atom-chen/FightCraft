using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBase : MonoBehaviour
{
    public int _ColliderID;
    public bool _IsColliderFinish = false;
    public bool _IsRemindSelected = false;
    
    protected MotionManager _SkillMotion;
    protected ImpactBase[] _ImpactList;

    public virtual void Init()
    {
        _SkillMotion = gameObject.GetComponentInParent<MotionManager>();
        _ImpactList = gameObject.GetComponents<ImpactBase>();
    }

    public virtual void ResetSkillRange()
    { }
    

    public virtual void ColliderStart()
    {
        gameObject.SetActive(true);

        
        if (!_IsColliderFinish)
        {
            StartCoroutine(AutoFinish());
        }
    }

    public IEnumerator AutoFinish()
    {
        yield return new WaitForFixedUpdate();

        ColliderFinish();
    }

    public virtual void ColliderFinish()
    {
        //gameObject.SetActive(false);
    }

}
