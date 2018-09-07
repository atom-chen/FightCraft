using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class AI_Base : MonoBehaviour
{
    public MotionManager _TargetMotion;

    bool _Init = false;

    public MotionManager _SelfMotion;
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
    public int GroupID;

    void Start()
    {
        StartCoroutine(InitDelay());
        AIManager.Instance.RegistAI(this);

        //StartCoroutine(AIFixedUpdate());
    }

    void OnDisable()
    {
        AIManager.Instance.RemoveAI(this);
    }

    //private IEnumerator AIFixedUpdate()
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.1f);

    //        if (!_Init)
    //            continue;

    //        if (_SelfMotion == null || _SelfMotion.IsMotionDie)
    //        {
    //            yield break;
    //        }

    //        AIUpdate();
    //    }
    //}

    void FixedUpdate()
    {

        if (!_Init)
            return;

        if (_SelfMotion == null || _SelfMotion.IsMotionDie)
        {
            return;
        }

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

        _BornPos = transform.position;
    }

    protected virtual void AIUpdate()
    {
        HpItemUpdate();
    }

    public virtual void OnStateChange(StateBase orgState, StateBase newState)
    {
        MoveState(orgState, newState);
        HitProtectStateChange(newState);
    }

    public virtual void OnBeHit(ImpactHit impactHit)
    {
        OnHitProtect(impactHit);
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

    private float _ComondSkillCD = 0;
    private float _LastUseSkillTime = 0;

    public void InitSkillGoes(MotionManager mainMotion)
    {
        GameObject motionObj = new GameObject("Motion");
        motionObj.transform.SetParent(mainMotion.transform);
        motionObj.transform.localPosition = Vector3.zero;
        motionObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
        for(int i = 0; i< _AISkills.Count; ++i)
        {
            var skillInfo = _AISkills[i];
            var skillBase = GameObject.Instantiate(skillInfo.SkillBase);
            skillBase.transform.SetParent(motionObj.transform);
            skillInfo.SkillBase = skillBase;
            if (i == 0)
            {
                skillInfo.LastUseSkillTime = -1;
            }
            else
            {
                skillInfo.LastUseSkillTime = Time.time - skillInfo.SkillInterval * 0.6f;
            }
        }
        
    }

    protected void InitSkillInfos()
    {

    }

    protected void SetSkillCD(AI_Skill_Info skillInfo, float cdTime)
    {
        skillInfo.LastUseSkillTime = Time.time;
        _LastUseSkillTime = Time.time;
    }

    protected bool IsCommonCD()
    {
        //if (Time.time - _LastUseSkillTime < _ComondSkillCD)
        //{
        //    return true;
        //}
        //return false;
        return true;
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

            //if (!IsCommonCD())
            //    continue;

            if (_AISkills[i].SkillRange < dis)
                continue;

            StartSkill(_AISkills[i]);
            return true;

        }

        return false;
    }

    int _ActValue = -1;
    float _AtkRate = -1;
    protected bool IsRandomActSkill()
    {
        
        var actRandom = Random.Range(0, 10000);

        if (_ActValue < 0)
        {
            int aiLevel = _SelfMotion.RoleAttrManager.Level / 10;
            aiLevel = Mathf.Clamp(aiLevel, 1, 10);

            if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Normal)
            {
                _ActValue = aiLevel * 60 + 1000;
            }
            else if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Elite)
            {
                _ActValue = 10000;
            }
            else if (_SelfMotion.RoleAttrManager.MotionType == Tables.MOTION_TYPE.Hero)
            {
                _ActValue = 10000;
            }
            if (_AtkRate < 0)
            {
                if (_SelfMotion.MonsterBase != null)
                    _AtkRate = _SelfMotion.MonsterBase.AtkRate;
                else
                    _AtkRate = 1;
            }
            _ActValue = (int)(_ActValue * _AtkRate);
            _ActValue = (int)(_ActValue * Time.fixedDeltaTime);

            foreach (var skillInfo in _AISkills)
            {
                if (_SelfMotion.RoleAttrManager.Level < 100)
                {
                    skillInfo.SkillInterval += 2 * (100 - _SelfMotion.RoleAttrManager.Level) / 100;
                    _ComondSkillCD = 2 * (100 - _SelfMotion.RoleAttrManager.Level);
                }
            }
        }

        if (_ActValue >= actRandom)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Move radius

    private float _MoveRadius = 0;
    private float _HitRadius = 0.1f;

    protected void MoveState(StateBase orgState, StateBase newState)
    {

        if (_HitRadius <= 0)
            return;

        if (_MoveRadius == _HitRadius)
            return;

        if (_SelfMotion == null)
            return;

        if (_SelfMotion.NavAgent == null)
            return;

        if (_MoveRadius <= 0)
        {
            _MoveRadius = _SelfMotion.NavAgent.radius;
        }

        if (newState is StateHit 
            || newState is StateFly
            || newState is StateLie
            || newState is StateCatch
            || newState is StateRise)
        {
            _SelfMotion.NavAgent.radius = _HitRadius;
        }
        else
        {
            _SelfMotion.NavAgent.radius = _MoveRadius;
        }
    }

    #endregion

    #region move

    public float _AlertRange = 7;
    public float _CloseRange = 2;
    public float _HuntRange = 10;
    public float _ReHuntRange = 5;

    protected float _CloseInterval = 0.5f;
    protected Vector3 _BornPos;
    protected bool _IsReturnToBornPos = false;

    private float _CloseWait;

    public bool IsActMove()
    {
        if (_SelfMotion._ActionState != _SelfMotion._StateIdle
            && _SelfMotion._ActionState != _SelfMotion._StateMove)
        {
            _IsReturnToBornPos = false;
        }

        if (!(_SelfMotion._ActionState is StateIdle || _SelfMotion._ActionState is StateMove))
        {
            return false;
        }

        //float distance = Vector3.Distance(transform.position, _TargetMotion.transform.position);
        //float bornDis = Vector3.Distance(transform.position, _BornPos);
        var pathToTarget = GetPath(transform.position, _TargetMotion.transform.position);
        var pathToBorn = GetPath(transform.position, _BornPos);
        if (pathToTarget == null || pathToBorn == null)
            return false;

        float distance = GetPathLength(pathToTarget);
        float bornDis = GetPathLength(pathToBorn);

        if (_CloseWait > 0)
        {
            _CloseWait -= Time.deltaTime;
            return false;
        }

        //alert hunt
        if (!_IsReturnToBornPos)
        {
            //too far
            if (bornDis > _HuntRange)
            {
                _IsReturnToBornPos = true;
                _SelfMotion.StartMoveState(_BornPos);
                return true;
            }
            //close enough
            else if (distance < _CloseRange)
            {
                _SelfMotion.StopMoveState();
                _CloseWait = _CloseInterval;
                return false;
            }
            else //hunt
            {
                _SelfMotion.StartMoveState(_TargetMotion.transform.position);
                return true;
            }
        }
        else
        {
            //rehunt
            if (distance < _ReHuntRange)
            {
                //rehunt in back
                if (bornDis < _HuntRange * 0.5f)
                {
                    _IsReturnToBornPos = false;
                    _SelfMotion.StartMoveState(_TargetMotion.transform.position);
                    return true;
                }
                else
                {
                    _SelfMotion.StartMoveState(_BornPos);
                    return true;
                }
            }
            else if (bornDis < 1.0f) //back close
            {
                _IsReturnToBornPos = false;
                _CloseWait = _CloseInterval;
                _SelfMotion.StopMoveState();
                _AIAwake = false;
                return false;
            }
            else
            {
                _SelfMotion.StartMoveState(_BornPos);
                return true;
            }
        }
        
    }

    public static NavMeshPath GetPath(Vector3 fromPos, Vector3 toPos)
    {
        NavMeshPath path = new NavMeshPath();

        if (NavMesh.CalculatePath(fromPos, toPos, NavMesh.AllAreas, path) == false)
            return null;

        return path;
    }

    public static float GetPathLength(NavMeshPath path)
    {
        float lng = 0.0f;

        if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                lng += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }

        return lng;
    }
    #endregion

    #region show hp item

    protected bool _IsShowHP = false;

    protected virtual void HpItemUpdate()
    {
        if (_IsShowHP)
            return;

        //if (_SelfMotion._ActionState != _SelfMotion._StateIdle
        //    && _SelfMotion._ActionState != _SelfMotion._StateMove)
        {
            _IsShowHP = true;
            UIHPPanel.ShowHPItem(_SelfMotion);
        }
    }

    #endregion

    #region hit protect

    public class HitSkillInfo
    {
        public ObjMotionSkillBase SkillBase;
        public int SkillActTimes;
        public int HitTimes;
    }

    protected ImpactBuff _HitProtectedPrefab;
    public ImpactBuff HitProtectedPrefab
    {
        get
        {
            if (_HitProtectedPrefab == null)
            {
                var buffGO = ResourceManager.Instance.GetGameObject("SkillMotion/CommonImpact/HitProtectedBuff");
                _HitProtectedPrefab = buffGO.GetComponent<ImpactBuff>();
            }
            return _HitProtectedPrefab;
        }
    }

    protected Dictionary<ObjMotionSkillBase, HitSkillInfo> _HitDict = new Dictionary<ObjMotionSkillBase, HitSkillInfo>();
    protected const int _ReleaseSkillTimes = 2;
    protected const int _ReleaseBuffTimes = 3;

    private void OnHitProtect(ImpactHit impactHit)
    {
        if (impactHit == null)
            return;

        if (impactHit.SkillMotion is ObjMotionSkillAttack)
            return;

        if (impactHit.SkillMotion == null)
            return;

        if (!_HitDict.ContainsKey(impactHit.SkillMotion))
        {
            _HitDict.Add(impactHit.SkillMotion, new HitSkillInfo() { SkillBase = impactHit.SkillMotion , SkillActTimes= -1, HitTimes=0});
        }

        if (_HitDict[impactHit.SkillMotion].SkillActTimes != impactHit.SkillMotion._SkillActTimes)
        {
            _HitDict[impactHit.SkillMotion].SkillActTimes = impactHit.SkillMotion._SkillActTimes;
            ++_HitDict[impactHit.SkillMotion].HitTimes;
            if (impactHit.SkillMotion is ObjMotionSkillBuff)
            {
                if (_HitDict[impactHit.SkillMotion].HitTimes > _ReleaseBuffTimes)
                {
                    ReleaseHit();
                }
            }
            else
            {
                if (_HitDict[impactHit.SkillMotion].HitTimes > _ReleaseSkillTimes)
                {
                    ReleaseHit();
                }
            }
        }
        
    }

    private void ReleaseHit()
    {
        Debug.Log("ReleaseHit ");
        ImpactFlyAway impact = new ImpactFlyAway();
        impact._FlyHeight = 1;
        impact._Time = 0.5f;
        impact._Speed = 10;
        impact._DamageRate = 0;
        impact.ActImpact(_SelfMotion, _SelfMotion);
        HitProtectedPrefab.ActBuffInstance(_SelfMotion, _SelfMotion);
    }

    private void HitProtectStateChange(StateBase newState)
    {
        if (newState is StateFly
            || newState is StateCatch
            || newState is StateHit
            || newState is StateLie)
            return;

        _HitDict.Clear();
        Debug.Log("HitProtectStateChange " + newState);
    }

    #endregion
}


