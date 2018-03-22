using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Base : MonoBehaviour
{
    public MotionManager _TargetMotion;

    bool _Init = false;

    protected MotionManager _SelfMotion;
    protected bool _AIAwake = false;
    public bool AIWake
    {
        get
        {
            return _AIAwake;
        }
        set
        {
            _AIAwake = value;
        }
    }

    void Start()
    {
        StartCoroutine(InitDelay());
        AIManager.Instance.RegistAI(this);
    }

    void OnDisable()
    {
        AIManager.Instance.RemoveAI(this);
    }

    void FixedUpdate()
    {
        if (!_Init)
            return;

        if (_SelfMotion == null || _SelfMotion.IsMotionDie)
            return;

        AIUpdate();
    }

    private IEnumerator InitDelay()
    {
        yield return new WaitForFixedUpdate();
        Init();
    }

    protected virtual void Init()
    {
        _Init = true;
        _SelfMotion = GetComponent<MotionManager>();

        if (_TargetMotion == null)
        {
            var mainPlayer = SelectTargetCommon.GetMainPlayer();
            if (mainPlayer != null)
            {
                _TargetMotion = mainPlayer;
            }
        }

        ModifyInitSkill();
        InitSkillInfos();
    }

    protected virtual void AIUpdate()
    {

    }

    public virtual void OnStateChange(StateBase orgState, StateBase newState)
    {
        MoveState(orgState, newState);
    }

    #region combatLevel

    protected int _CombatLevel = 1;

    public void SetCombatLevel(int level)
    {
        _CombatLevel = level;
    }

    protected virtual void ModifyInitSkill()
    {
        if (_AISkills.Count == 0)
            return;

        if (_CombatLevel == 1)
            return;


    }

    #endregion

    #region skill

    [System.Serializable]
    public class AI_Skill_Info
    {
        public ObjMotionSkillBase SkillBase;
        public float SkillRange;
        public float SkillInterval;
        public float ReadyTime = 0;

        public float LastUseSkillTime { get; set; }

        public bool IsSkillCD()
        {
            return (Time.time - LastUseSkillTime > SkillInterval);
        }
    }
    public List<AI_Skill_Info> _AISkills;

    public void InitSkillGoes(MotionManager mainMotion)
    {
        GameObject motionObj = new GameObject("Motion");
        motionObj.transform.SetParent(mainMotion.transform);
        motionObj.transform.localPosition = Vector3.zero;
        motionObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        foreach (var skillInfo in _AISkills)
        {
            var skillBase = GameObject.Instantiate(skillInfo.SkillBase);
            skillBase.transform.SetParent(motionObj.transform);
            skillInfo.SkillBase = skillBase;
        }
    }

    protected void InitSkillInfos()
    {

    }

    protected void SetSkillCD(AI_Skill_Info skillInfo, float cdTime)
    {
        skillInfo.LastUseSkillTime = Time.time;
    }

    protected virtual void StartSkill(AI_Skill_Info skillInfo, bool isIgnoreCD = false)
    {
        if (!skillInfo.SkillBase.IsCanActSkill())
            return;

        _SelfMotion.transform.LookAt(_TargetMotion.transform.position);
        _SelfMotion.ActSkill(skillInfo.SkillBase);
        SetSkillCD(skillInfo, skillInfo.SkillInterval);
    }

    protected virtual bool StartSkill()
    {
        if (!IsRandomActSkill())
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        for (int i = _AISkills.Count - 1; i >= 0; --i)
        {
            if (!_AISkills[i].IsSkillCD())
                continue;

            if (_AISkills[i].SkillRange < dis)
                continue;

            StartSkill(_AISkills[i]);
            return true;

        }

        return false;
    }

    protected bool IsRandomActSkill()
    {
        var actRandom = Random.Range(0, 10000);
        if (_SelfMotion.RoleAttrManager.Level * 50 >= actRandom)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Move radius

    public float _MoveRadius = 0;
    public float _NormalRadius = 0;

    protected void MoveState(StateBase orgState, StateBase newState)
    {

        if (_MoveRadius <= 0 || _NormalRadius <= 0)
            return;

        if (_MoveRadius == _NormalRadius)
            return;

        if (_SelfMotion == null)
            return;

        if (_SelfMotion.NavAgent == null)
            return;

        if (newState is StateMove)
        {
            _SelfMotion.NavAgent.radius = _MoveRadius;
        }
        else
        {
            _SelfMotion.NavAgent.radius = _NormalRadius;
        }
    }

    #endregion

}
