using UnityEngine;
using System.Collections;

public class AI_IntHeroBase : AI_HeroBase
{
    protected override void Init()
    {
        base.Init();
        Debug.Log("init AI_StrengthHeroBase");
        InitAttackBlock();
    }

    #region super armor

    private void InitAttackBlock()
    {
        for (int i = 0; i < _AISkills.Count; ++i)
        {
            InitSuperArmorSkill(_AISkills[i].SkillBase);
        }
    }

    #endregion

    #region stage 2

    public float Stage2BuffTime = 60;

    protected bool _Stage2Started = false;

    private ImpactBuff[] _IntStage2Buff;
    public ImpactBuff[] IntStage2Buff
    {
        get
        {
            if (_IntStage2Buff == null)
            {
                var buffGO = GameBase.ResourceManager.Instance.GetGameObject("SkillMotion/IntShieldBuff");
                _IntStage2Buff = buffGO.GetComponents<ImpactBuff>();
            }
            return _IntStage2Buff;
        }
    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_SelfMotion.RoleAttrManager.HPPersent < 0.5f)
        {
            StartStage2();
        }
    }

    protected virtual void StartStage2()
    {
        if (_Stage2Started)
            return;

        _Stage2Started = true;
        for (int i = 0; i < IntStage2Buff.Length; ++i)
        {
            IntStage2Buff[i].ActBuffInstance(_SelfMotion, _SelfMotion, Stage2BuffTime);
        }

        for (int i = 0; i < _AISkills.Count; ++i)
        {
            _AISkills[i].SkillInterval *= 0.5f;
            //_AISkills[i].LastUseSkillTime = 0;
        }
    }

    #endregion
}

