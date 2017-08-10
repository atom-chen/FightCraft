using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_HeroBase : AI_Base
{
    protected override void Init()
    {
        base.Init();
        InitRise();
    }

    #region rise

    private ObjMotionSkillBase _RiseBoom;

    private float _RiseTime = 1f;

    private void InitRise()
    {
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_RISE, RiseEvent);
        _SelfMotion.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_RISE_FINISH, RiseFinishEvent);

        var riseBoom = GameBase.ResourceManager.Instance.GetInstanceGameObject("SkillMotion/RiseBoomSkill");
        var motionTrans = transform.FindChild("Motion");
        riseBoom.transform.SetParent(motionTrans);
        riseBoom.transform.localPosition = Vector3.zero;
        riseBoom.transform.localRotation = Quaternion.Euler(Vector3.zero);
        
        _RiseBoom = riseBoom.GetComponent<ObjMotionSkillBase>();
        _RiseBoom.Init();
    }

    public void RiseEvent(object sender, Hashtable eventArgs)
    {
        Debug.Log("RiseEvent");
        _RiseBoom.ActSkill();
    }

    public void RiseFinishEvent(object sender, Hashtable eventArgs)
    {
        
    }

    #endregion

    #region super armor

    public ImpactBuff _SuperArmorPrefab;

    private ImpactBuffSuperArmor _BuffInstance;

    protected void InitSuperArmorSkill(ObjMotionSkillBase objMotionSkill)
    {
        float attackConlliderTime = _SelfMotion.AnimationEvent.GetAnimFirstColliderEventTime(objMotionSkill._Anim);
        if (attackConlliderTime < 0)
            return;


        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._Anim, 0, AttackStart);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._Anim, attackConlliderTime + 0.05f, AttackCollider);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._Anim, objMotionSkill._Anim.length, AttackCollider);
    }

    private void AttackStart()
    {
        _BuffInstance = _SuperArmorPrefab.ActBuffInstance(_SelfMotion, _SelfMotion) as ImpactBuffSuperArmor;

    }

    private void AttackCollider()
    {
        _BuffInstance.RemoveBuff(_SelfMotion);
    }

    #endregion
}
