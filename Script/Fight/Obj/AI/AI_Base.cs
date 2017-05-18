using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI_Base : MonoBehaviour
{
    public MotionManager _TargetMotion;

    bool _Init = false;

    protected MotionManager _SelfMotion;

    void Start()
    {
        StartCoroutine(InitDelay());
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
        InitSkills();
    }

    protected virtual void AIUpdate()
    {

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

        if (_SelfMotion.RoleAttrManager.MotionType == MotionType.Normal)
        {
            int randomLockTime = Random.Range((int)(_AISkills[0].SkillInterval * _CombatLevel * 0.5f), (int)(_AISkills[0].SkillInterval * _CombatLevel * 2));
            StartCoroutine(StartLockSkill(_AISkills[0], randomLockTime));
            _AISkills[0].SkillInterval = _AISkills[0].SkillInterval * _CombatLevel;
            _AISkills[0].StartCD = false;
        }
        else if (_SelfMotion.RoleAttrManager.MotionType == MotionType.Elite)
        {
            _AISkills[0].SkillInterval = _AISkills[0].SkillInterval * _CombatLevel * 0.5f;
            _AISkills[0].StartCD = true;

            for (int i = 1; i < _AISkills.Count; ++i)
            {   
                int randomLockTime = Random.Range((int)(_AISkills[i].SkillInterval * _CombatLevel * 0.5f), (int)(_AISkills[i].SkillInterval * _CombatLevel * 2));
                StartCoroutine(StartLockSkill(_AISkills[i], randomLockTime));
                _AISkills[i].SkillInterval = _AISkills[i].SkillInterval * _CombatLevel * 0.5f;
                _AISkills[i].StartCD = false;
            }
        }
    }

    protected IEnumerator StartLockSkill(AI_Skill_Info lockSkill, float lockTime)
    {
        lockSkill.StartLock = true;
        yield return new WaitForSeconds(lockTime);
        lockSkill.StartLock = false;
        UnLockSkill(lockSkill);
    }


    #endregion

    #region skill

    [System.Serializable]
    public class AI_Skill_Info
    {
        public ObjMotionSkillBase SkillBase;
        public float SkillRange;
        public float SkillInterval;
        public bool StartCD = false;
        public bool StartLock = false;

        public float LastUseSkillTime { get; set; }
    }
    public List<AI_Skill_Info> _AISkills;
    private List<AI_Skill_Info> _CDSkills = new List<AI_Skill_Info>();

    protected void InitSkills()
    {
        _CDSkills.Clear();
        foreach (var skillInfo in _AISkills)
        {
            if (skillInfo.StartLock)
            {
                continue;
            }
            _CDSkills.Add(skillInfo);
            if (skillInfo.StartCD)
            {
                SetSkillCD(skillInfo, skillInfo.SkillInterval);
            }
        }
    }

    protected void SetSkillCD(AI_Skill_Info skillInfo, float cdTime)
    {
        if (skillInfo.SkillInterval <= 0)
            return;

        _CDSkills.Remove(skillInfo);
        StartCoroutine(SkillCD(skillInfo));
    }

    protected IEnumerator SkillCD(AI_Skill_Info skillInfo)
    {
        yield return new WaitForSeconds(skillInfo.SkillInterval);

        UnLockSkill(skillInfo);
    }

    protected void UnLockSkill(AI_Skill_Info skillInfo)
    {
        int skillPrior = _AISkills.IndexOf(skillInfo);
        int insertIdx = -1;
        for (int i = 0; i < _CDSkills.Count; ++i)
        {
            int cdSkillPrior = _AISkills.IndexOf(_CDSkills[i]);
            if (skillPrior < cdSkillPrior)
            {
                insertIdx = i;
                break;
            }
        }
        if (insertIdx >= 0)
        {
            _CDSkills.Insert(insertIdx, skillInfo);
        }
        else
        {
            _CDSkills.Add(skillInfo);
        }
    }

    protected void StartSkill(AI_Skill_Info skillInfo, bool isIgnoreCD = false)
    {
        if (!skillInfo.SkillBase.IsCanActSkill())
            return;

        _SelfMotion.transform.LookAt(_TargetMotion.transform.position);
        _SelfMotion.ActSkill(skillInfo.SkillBase);
        SetSkillCD(skillInfo, skillInfo.SkillInterval);
    }

    protected bool StartSkill()
    {
        if (_CDSkills.Count == 0)
            return false;

        float dis = Vector3.Distance(_SelfMotion.transform.position, _TargetMotion.transform.position);

        for (int i = _CDSkills.Count - 1; i >= 0; --i)
        {
            if (_CDSkills[i].SkillRange > dis)
            {
                StartSkill(_CDSkills[i]);
                return true;
            }
        }

        return false;
    }
    #endregion

}
