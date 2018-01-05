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
    }

    #region rise

    private ObjMotionSkillBase _RiseBoom;

    private float _RiseTime = 1f;
    private bool _IsRiseEvent = false;

    private void InitRise()
    {
        var riseBoom = ResourceManager.Instance.GetInstanceGameObject("SkillMotion/CommonImpact/RiseBoomSkill");
        var motionTrans = transform.FindChild("Motion");
        riseBoom.transform.SetParent(motionTrans);
        riseBoom.transform.localPosition = Vector3.zero;
        riseBoom.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _RiseBoom = riseBoom.GetComponent<ObjMotionSkillBase>();
        _RiseBoom.Init();
    }

    private void RiseUpdate()
    {
        if (_SelfMotion._ActionState == _SelfMotion._StateRise)
        {
            if (!_IsRiseEvent)
            {
                _IsRiseEvent = true;
                _RiseBoom.ActSkill(null);
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
}
