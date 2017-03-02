using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MotionManager : MonoBehaviour
{
    void Start()
    {
        InitMotions();
    }

    void FixedUpdate()
    {
        UpdateMove();
    }

    #region motion

    private ObjMotionBase[] _MotionList;
    private int _MotionCount;

    private ObjMotionBase _CurMotion;
    public ObjMotionBase CurMotion
    {
        get
        {
            return _CurMotion;
        }
    }

    private void InitMotions()
    {
        _MotionList = gameObject.GetComponentsInChildren<ObjMotionBase>();
        _MotionCount = _MotionList.Length;

        for (int i = 0; i< _MotionCount; ++i)
        {
            _MotionList[i].InitMotion(this);
        }
    }

    public void MotionInput(InputManager input)
    {
        if (_CurMotion != null)
        {
            _CurMotion.ContinueInput(input);
        }

        for (int i = 0; i < _MotionCount; ++i)
        {
            _MotionList[i].ActiveInput(input);
        }
    }

    public void MotionStart(ObjMotionBase motion)
    {
        if (_CurMotion != null && _CurMotion != motion)
        {
            _CurMotion.StopMotion();
        }
        _CurMotion = motion;
    }

    public void MotionFinish(ObjMotionBase motion)
    {
        if (motion == _CurMotion)
        {
            _CurMotion.StopMotion();
            _CurMotion = null;
            _EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FINISH, this, new Hashtable());
        }
    }

    public void NotifyAnimEvent(string function, object param)
    {
        if (_CurMotion != null)
        {
            _CurMotion.AnimEvent(function, param);
        }
    }
    #endregion

    #region Animation

    public Animation _Animaton;
    public bool _IsRoleHit;

    public void InitAnimation(AnimationClip animClip)
    {
        _Animaton.AddClip(animClip, animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip)
    {
        _Animaton[animClip.name].speed = _RoleAttrManager.SkillSpeed;
        _Animaton.Play(animClip.name);
    }

    public void PlayAnimation(AnimationClip animClip, float speed)
    {
        _Animaton[animClip.name].speed = speed;
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip, float speed)
    {
        _Animaton[animClip.name].speed = speed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    public void RePlayAnimation(AnimationClip animClip)
    {
        _Animaton[animClip.name].speed = _RoleAttrManager.SkillSpeed;
        _Animaton.Stop();
        _Animaton.Play(animClip.name);
    }

    float _OrgSpeed = 1;
    public void PauseAnimation(AnimationClip animClip)
    {
        _OrgSpeed = _Animaton[animClip.name].speed;
        _Animaton[animClip.name].speed = 0;
    }

    public void ResumeAnimation(AnimationClip animClip)
    {
        if (_Animaton.IsPlaying(animClip.name))
        {
            _Animaton[animClip.name].speed = _OrgSpeed;
        }
    }

    #endregion

    #region event

    public GameBase.EventController _EventController;

    #endregion

    #region roleAttr

    public RoleAttrManager _RoleAttrManager;

    #endregion

    #region buff

    List<ImpactBuff> _ImpactBuffs = new List<ImpactBuff>();

    public void AddBuff(ImpactBuff buff)
    {
        _ImpactBuffs.Add(buff);
    }

    public void RemoveBuff(ImpactBuff buff)
    {
        buff.RemoveBuff();
        _ImpactBuffs.Remove(buff);
    }

    #endregion

    #region effect

    private Dictionary<string, Transform> _BindTransform = new Dictionary<string, Transform>();

    public void PlaySkillEffect(EffectController effect)
    {
        effect.PlayEffect(_RoleAttrManager.SkillSpeed);
    }

    public void StopSkillEffect(EffectController effect)
    {
        effect.HideEffect();
    }

    public void PlayDynamicEffect(EffectController effect)
    {
        if (!_BindTransform.ContainsKey(effect._BindPos))
        {
            var bindTran = transform.FindChild(_Animaton.name + "/" + effect._BindPos);
            _BindTransform.Add(effect._BindPos, bindTran);
        }

        var idleEffect = EffectController.GetIdleEffect(effect);
        idleEffect.transform.SetParent(_BindTransform[effect._BindPos]);
        idleEffect.transform.localPosition = Vector3.zero;
        idleEffect.transform.localRotation = Quaternion.Euler(Vector3.zero);
        idleEffect._EffectLastTime = effect._EffectLastTime;
        idleEffect.PlayEffect();
        StartCoroutine(StopDynamicEffect(idleEffect));
    }

    public IEnumerator StopDynamicEffect(EffectController effct)
    {
        yield return new WaitForSeconds( effct._EffectLastTime);
        effct.HideEffect();
        EffectController.RecvIldeEffect(effct);
    }

    #endregion

    #region move

    private NavMeshAgent _NavAgent;
    private Vector3 _TargetVec;
    private float _LastTime;
    private float _Speed;

    public void SetMove(Vector3 moveVec, float lastTime)
    {
        if (_NavAgent == null)
        {
            _NavAgent = GetComponent<NavMeshAgent>();
        }
        
        _TargetVec += moveVec;
        _LastTime = lastTime;
        _Speed = _TargetVec.magnitude / _LastTime;
    }

    public void UpdateMove()
    {
        if (_TargetVec == Vector3.zero)
            return;

        Vector3 moveVec = _TargetVec.normalized * _Speed * Time.fixedDeltaTime;

        _TargetVec -= moveVec;
        _LastTime -= Time.fixedDeltaTime;
        if (_LastTime < 0)
        {
            _LastTime = 0;
            _TargetVec = Vector3.zero;
        }

        _NavAgent.Warp(transform.position + moveVec);
    }

    #endregion
}
