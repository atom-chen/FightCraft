using UnityEngine;
using System.Collections;

public class AI_IntHeroBase : AI_HeroBase
{
    protected override void Init()
    {
        base.Init();
        Debug.Log("init AI_StrengthHeroBase");
        InitSkills();
    }

    #region super armor

    private void InitSkills()
    {
        for (int i = 0; i < _AISkills.Count; ++i)
        {
            InitSuperArmorSkill(_AISkills[i].SkillBase);
            InitReadySkillSpeed(_AISkills[i]);
        }
    }

    #endregion

    #region stage 2

    public float Stage2BuffTime = 20;
    public float Stage2HP = 0;
    public float Stage2BuffCD = 180;

    protected float _Stage2Started = 0;

    private ImpactBuff[] _IntStage2Buff;
    public ImpactBuff[] IntStage2Buff
    {
        get
        {
            if (_IntStage2Buff == null)
            {
                var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/IntShieldBuff");
                _IntStage2Buff = buffGO.GetComponents<ImpactBuff>();
            }
            return _IntStage2Buff;
        }
    }

    protected override void AIUpdate()
    {
        base.AIUpdate();

        if (_SelfMotion.RoleAttrManager.HPPersent < Stage2HP)
        {
            StartStage2();
        }
    }

    protected virtual void StartStage2()
    {
        if (Time.time -  _Stage2Started < Stage2BuffCD)
            return;

        _Stage2Started = Time.time;
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

