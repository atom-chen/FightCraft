using UnityEngine;
using System.Collections;

public class ImpactExAttack : ImpactBase
{

    void Update()
    {
        if (SenderMotion == null || SenderMotion.ActingSkill!= SkillMotion)
        {
            FinishImpact(SenderMotion);
            return;
        }

        Debug.Log("Impact ex attack");
        UpdateInput();
    }


    private ObjMotionSkillBase _ExAttackSkill;
    public int _AttackTimes;
    public float _Damage;

    public override void Init(ObjMotionSkillBase skillMotion, SelectBase selector)
    {
        base.Init(skillMotion, selector);

        string skillPath = "SkillMotion/" + skillMotion.MotionManager._MotionAnimPath + "/AttackEx";
        var skillObj = ResourceManager.Instance.GetInstanceGameObject(skillPath);
        _ExAttackSkill = skillObj.GetComponent<ObjMotionSkillBase>();
        var hitCollider = skillObj.GetComponentInChildren<SelectCollider>();
        if (hitCollider != null)
        {
            while (hitCollider._EventFrame.Count > _AttackTimes)
            {
                hitCollider._EventFrame.RemoveAt(hitCollider._EventFrame.Count - 1);
            }
        }
        _ExAttackSkill.transform.SetParent(skillMotion.transform.parent);
        _ExAttackSkill.Init();
    }

    public override void ActImpact(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActImpact(senderManager, reciverManager);

        this.enabled = true;
    }

    public override void FinishImpact(MotionManager reciverManager)
    {
        base.FinishImpact(reciverManager);

        this.enabled = false;
    }

    private void UpdateInput()
    {
        if (InputManager.Instance.IsKeyHold("j") || InputManager.Instance.IsKeyHold("k"))
        {
            SenderMotion.ActSkill(_ExAttackSkill);
        }
    }

}
