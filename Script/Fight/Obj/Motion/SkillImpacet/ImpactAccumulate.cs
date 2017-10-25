using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAccumulate : ImpactBase
{
    public void Update()
    {
        if (_IsActingImpact)
        {
            if (!InputManager.Instance.IsAnyHold())
            {
                ReleaseAccumulate();
            }
            else if (Time.time - _HoldTime >= _AccumulateTime)
            {
                ReleaseAccumulate();
            }

        }
    }

    private AnimationClip _AccumulateAnim;
    public float _AccumulateTime;
    public float _AccumulateDamage;

    private float _HoldTime;

    public override void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillMotion, selector);

        string animPath = "Animation/" + SkillMotion.MotionManager._MotionAnimPath + "/Act_Skill_Accumulate";
        _AccumulateAnim = ResourceManager.Instance.GetAnimationClip(animPath);
        if (_AccumulateAnim == null)
        {
            Debug.LogError("ImpactAccumulate init anim error: animPath");
            return;
        }

        SkillMotion._NextAnim.Insert(0, _AccumulateAnim);
        SkillMotion._NextEffect.Insert(0, null);
        SenderMotion.AnimationEvent.AddSelectorEvent(_AccumulateAnim, 0, selector._ColliderID);
        SenderMotion.AnimationEvent.AddSelectorFinishEvent(_AccumulateAnim, 2.0f, selector._ColliderID);
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);
        _IsActingImpact = true;
        _HoldTime = Time.time;
    }

    public override void FinishImpact(MotionManager reciverManager)
    {
        //base.RemoveImpact(reciverManager);
    }

    public override void StopImpact()
    {
        base.StopImpact();

        _IsActingImpact = false;
    }

    private void ReleaseAccumulate()
    {
        float accumulateRate = (Time.time - _HoldTime / _AccumulateTime);
        accumulateRate = Mathf.Clamp(accumulateRate, 0, 1);
        _IsActingImpact = false;

        SkillMotion.PlayerNextAnim();
    }
}
