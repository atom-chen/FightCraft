using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightGuide : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        NormalGuideUpdate();
        DeBuffGuideUpdate();
        BuffGuideUpdate();
    }

    #region normal skill

    public bool _IsShowNormalSkillGuide = true;
    private int _GuideStep = 1;

    public void StartSkillGuide(int idx)
    {
        _IsShowNormalSkillGuide = true;
        _GuideStep = idx;
    }

    public void NormalGuideUpdate()
    {
        if (!_IsShowNormalSkillGuide)
            return;

        if (FightManager.Instance.MainChatMotion.ActingSkill == null)
            return;

        if (FightManager.Instance.MainChatMotion.ActingSkill.GetSkillType() == ObjMotionSkillBase.CharSkillType.NormalAttack)
        {
            ObjMotionSkillAttack normalAttack = FightManager.Instance.MainChatMotion.ActingSkill as ObjMotionSkillAttack;
            if (normalAttack.CurStep == _GuideStep && normalAttack.CanNextInput)
            {
                GlobalEffect.Instance.Pause(1);
            }
        }

    }

    #endregion

    #region debuff skill

    public bool _IsShowDebuffSkillGuide = true;

    public void StartDebuffGuide(int idx)
    {
        _IsShowDebuffSkillGuide = true;
        _GuideStep = idx;
    }

    public void DeBuffGuideUpdate()
    {
        if (!_IsShowDebuffSkillGuide)
            return;

        if (FightManager.Instance.MainChatMotion.ActingSkill == null)
            return;

        if (FightManager.Instance.MainChatMotion.ActingSkill.GetSkillType() == ObjMotionSkillBase.CharSkillType.NormalAttack)
        {
            ObjMotionSkillAttack normalAttack = FightManager.Instance.MainChatMotion.ActingSkill as ObjMotionSkillAttack;
            if (normalAttack.CurStep == _GuideStep && normalAttack.CanNextInput)
            {
                //tip buff skill
            }
        }
        else if (FightManager.Instance.MainChatMotion.ActingSkill.GetSkillType() == ObjMotionSkillBase.CharSkillType.DeBuff)
        {
            if (FightManager.Instance.MainChatMotion.ActingSkill.CanNextInput)
            {
                //tip buff skill
            }
        }
    }

    #endregion

    #region buff

    public bool _IsShowBuffGuide = true;

    public void StartBuffGuide(int idx)
    {
        _IsShowBuffGuide = true;
        _GuideStep = idx;
    }

    public void BuffGuideUpdate()
    {
        if (!_IsShowBuffGuide)
            return;

        if (FightManager.Instance.MainChatMotion.ActingSkill == null)
            return;

        if (FightManager.Instance.MainChatMotion.ActingSkill.GetSkillType() == ObjMotionSkillBase.CharSkillType.Skill)
        {
            var normalSkill = FightManager.Instance.MainChatMotion.ActingSkill;
            if (normalSkill.CanNextInput)
            {
                //tip buff skill
            }
        }
        else if (FightManager.Instance.MainChatMotion.ActingSkill.GetSkillType() == ObjMotionSkillBase.CharSkillType.DeBuff)
        {
            if (FightManager.Instance.MainChatMotion.ActingSkill.CanNextInput)
            {
                //tip buff skill
            }
        }
    }

    #endregion
}
