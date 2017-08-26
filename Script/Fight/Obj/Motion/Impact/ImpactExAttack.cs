using UnityEngine;
using System.Collections;

public class ImpactExAttack : ImpactBase
{

    void Update()
    {
        if (SkillMotion.MotionManager.ActingSkill != SkillMotion)
        {
            RemoveImpact(SkillMotion.MotionManager);
            return;
        }

        UpdateInput();
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        this.enabled = true;
    }

    public override void RemoveImpact(MotionManager reciverManager)
    {
        base.RemoveImpact(reciverManager);

        this.enabled = false;
    }

    private void UpdateInput()
    {
        if (InputManager.Instance.IsKeyHold("k"))
        {
            SkillMotion.MotionManager.ActSkill(SkillMotion.MotionManager._SkillMotions["e"]);
        }
    }

}
