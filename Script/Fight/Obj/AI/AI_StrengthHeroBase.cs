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
        SuperArmorPrefab.ActBuffInstance(_SelfMotion, _SelfMotion, Stage2SuperTime);
    }

    #endregion
}

