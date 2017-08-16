using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBase : MonoBehaviour
{
    public int _ColliderID;
    public bool _IsColliderFinish = false;
    public bool _IsRemindSelected = false;
    
    protected MotionManager _ObjMotion;
    protected ObjMotionSkillBase _SkillMotion;
    protected ImpactBase[] _ImpactList;

    public virtual void Init()
    {
        _SkillMotion = gameObject.GetComponentInParent<ObjMotionSkillBase>();
        _ObjMotion = gameObject.GetComponentInParent<MotionManager>();
        _ImpactList = gameObject.GetComponents<ImpactBase>();
        foreach (var impactBase in _ImpactList)
        {
            impactBase.SkillMotion = _SkillMotion;
        }
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
