using UnityEngine;
using System.Collections;

public class AI_DexHeroBase : AI_HeroBase
{
    protected override void Init()
    {
        base.Init();
        Debug.Log("init AI_StrengthHeroBase");
        InitAttackBlock();
        InitBlockSkill();
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

    public float Stage2BuffTime = 20;

    protected bool _Stage2Started = false;

    private ImpactBuff[] _DexStage2Buff;
    public ImpactBuff[] DexStage2Buff
    {
        get
        {
            if (_DexStage2Buff == null)
            {
                var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/DexAccelateBuff");
                _DexStage2Buff = buffGO.GetComponents<ImpactBuff>();
            }
            return _DexStage2Buff;
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
        for (int i = 0; i < DexStage2Buff.Length; ++i)
        {
            DexStage2Buff[i].ActBuffInstance(_SelfMotion, _SelfMotion, Stage2BuffTime);
        }
    }

    #endregion

    #region 

    public AnimationClip _BlockAnim;
    public int _AfterBlockSkill = -1;
    private ObjMotionSkillBase _SkillBlock;
    private float _BlockCD = 10;
    private float _LastBlockTime;

    protected int _NextSkillIdx = -1;

    private void InitBlockSkill()
    {
        var blockSkill = ResourceManager.Instance.GetInstanceGameObject("SkillMotion/BlockSkill");
        var motionTrans = _SelfMotion.transform.FindChild("Motion");
        blockSkill.transform.SetParent(motionTrans);
        _SkillBlock = blockSkill.GetComponent<ObjMotionSkillBlock>();
        _SkillBlock._Anim = _BlockAnim;
        _SkillBlock.Init();

        _LastBlockTime = -_BlockCD;
        _SelfMotion.EventController.RegisteEvent(EVENT_TYPE.EVENT_MOTION_HIT, HitEvent);
    }

    private void HitEvent(object sender, Hashtable eventArgs)
    {
        Debug.Log("HitEvent");
        if (Time.time - _LastBlockTime > _BlockCD)
        {
            _LastBlockTime = Time.time;

            _SkillBlock.ActSkill();

            _NextSkillIdx = _AfterBlockSkill;
        }
    }

    #endregion
}

