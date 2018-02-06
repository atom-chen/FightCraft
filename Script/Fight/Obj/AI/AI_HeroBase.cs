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

    protected override void AIUpdate()
    {
        base.AIUpdate();

        RiseUpdate();
        UpdateCriticalAI();
    }

    #region rise

    private ImpactBase _RiseBoom;

    private float _RiseTime = 1f;
    private bool _IsRiseEvent = false;

    private void InitRise()
    {
        var riseBoom = ResourceManager.Instance.GetInstanceGameObject("SkillMotion/CommonImpact/RiseBoomSkill");
        var motionTrans = transform.FindChild("Motion");
        riseBoom.transform.SetParent(motionTrans);
        riseBoom.transform.localPosition = Vector3.zero;
        riseBoom.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _RiseBoom = riseBoom.GetComponent<ImpactBase>();
        
    }

    private void RiseUpdate()
    {
        if (_SelfMotion._ActionState == _SelfMotion._StateRise)
        {
            if (!_IsRiseEvent)
            {
                _IsRiseEvent = true;
                _RiseBoom.ActImpact(_SelfMotion, _SelfMotion);
            }
        }
        else
        {
            if (_IsRiseEvent)
            {
                _IsRiseEvent = false;
            }
        }
    }

    #endregion

    #region super armor

    private ImpactBuff _SuperArmorPrefab;
    public ImpactBuff SuperArmorPrefab
    {
        get
        {
            if (_SuperArmorPrefab == null)
            {
                var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/SuperArmor");
                _SuperArmorPrefab = buffGO.GetComponent<ImpactBuff>();
            }
            return _SuperArmorPrefab;
        }
    }

    private ImpactBuffSuperArmor _BuffInstance;

    protected void InitSuperArmorSkill(ObjMotionSkillBase objMotionSkill)
    {
        if (objMotionSkill._NextAnim.Count == 0)
            return;

        float attackConlliderTime = _SelfMotion.AnimationEvent.GetAnimFirstColliderEventTime(objMotionSkill._NextAnim[0]);
        if (attackConlliderTime < 0)
            return;

        Debug.Log("first collider time:"+ objMotionSkill.name + ":" + attackConlliderTime);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], 0, AttackStart);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], attackConlliderTime + 0.05f, AttackCollider);
        _SelfMotion.AnimationEvent.AddEvent(objMotionSkill._NextAnim[0], objMotionSkill._NextAnim[0].length, AttackCollider);
    }

    private void AttackStart()
    {
        _BuffInstance = SuperArmorPrefab.ActBuffInstance(_SelfMotion, _SelfMotion) as ImpactBuffSuperArmor;

    }

    private void AttackCollider()
    {
        if (_BuffInstance != null)
        {
            _BuffInstance.RemoveBuff(_SelfMotion);
        }
    }

    #endregion

    #region critical AI

    //if target start use skill, AI use skill
    private void UpdateCriticalAI()
    {
        if (_SelfMotion._ActionState != _SelfMotion._StateIdle)
            return;

        if (_TargetMotion.ActingSkill == null)
            return;

        AI_Skill_Info aiSkill = _AISkills[0];
        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].IsSkillCD())
            {
                aiSkill = _AISkills[i];
                break;
            }
        }
        StartSkill(aiSkill);
    }

    #endregion
}
