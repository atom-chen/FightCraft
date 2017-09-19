using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectBase : MonoBehaviour
{
    public int _ColliderID;
    public bool _IsColliderFinish = false;
    public bool _IsRemindSelected = false;
    public AnimationClip _EventAnim;
    public List<int> _EventFrame;
    
    protected MotionManager _ObjMotion;
    protected ObjMotionSkillBase _SkillMotion;
    protected ImpactBase[] _ImpactList;

    public virtual void Init(RoleAttrManager.SkillAttr skillAttr)
    {
        _SkillMotion = gameObject.GetComponentInParent<ObjMotionSkillBase>();
        _ObjMotion = gameObject.GetComponentInParent<MotionManager>();
        _ImpactList = gameObject.GetComponents<ImpactBase>();
        foreach (var impactBase in _ImpactList)
        {
            impactBase.Init(skillAttr, _SkillMotion, this);
        }
    }

    public virtual void RegisterEvent()
    {
        for (int i = 0; i < _EventFrame.Count; ++i)
        {
            var anim = _ObjMotion.Animation.GetClip(_EventAnim.name);
            _ObjMotion.AnimationEvent.AddSelectorEvent(anim, _EventFrame[i], _ColliderID);
        }
    }

    public virtual void ResetSelector()
    {
        _ObjMotion.AnimationEvent.RemoveSelectorEvent(_EventAnim, _ColliderID);
    }
    

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

    public virtual void ColliderStop()
    {
        foreach (var impact in _ImpactList)
        {
            impact.StopImpact();
        }
    }

}
