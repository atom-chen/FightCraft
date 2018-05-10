using UnityEngine;
using System.Collections;

public class AI_StrengthHeroBase : AI_HeroBase
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

    public float Stage2SuperTime = 20;

    protected bool _Stage2Started = false;

    private ImpactBuff[] _Strtage2Buff;
    public ImpactBuff[] StrStage2Buff
    {
        get
        {
            if (_Strtage2Buff == null)
            {
                var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/StrBuff");
                _Strtage2Buff = buffGO.GetComponents<ImpactBuff>();
            }
            return _Strtage2Buff;
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
        for (int i = 0; i < StrStage2Buff.Length; ++i)
        {
            StrStage2Buff[i].ActBuffInstance(_SelfMotion, _SelfMotion, Stage2SuperTime);
        }
    }

    #endregion
}

