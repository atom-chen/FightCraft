﻿using UnityEngine;
using System.Collections;

public class BaseMotionManager : MonoBehaviour
{
    public const int IDLE_PRIOR = 0;
    public const int MOVE_PRIOR = 10;
    public const int HIT_PRIOR = 1000;
    public const int FLY_PRIOR = 1001;
    public const int RISE_PRIOR = 999;
    public const int DIE_PRIOR = 2000;

    protected MotionManager _MotionManager;

    public void Init()
    {
        _MotionManager = gameObject.GetComponent<MotionManager>();
        _NavAgent = gameObject.GetComponent<NavMeshAgent>();

        _MotionManager.AddAnimationEndEvent(_HitAnim);
        _FlyBody = _MotionManager.AnimationEvent.gameObject;

        _MotionManager.InitAnimation(_IdleAnim);
        _MotionManager.InitAnimation(_MoveAnim);
        _MotionManager.InitAnimation(_HitAnim);
        _MotionManager.InitAnimation(_FlyAnim);
        _MotionManager.InitAnimation(_RiseAnim);

        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, HitEvent);
        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, FlyEvent);
    }

    public void HitEvent(object go, Hashtable eventArgs)
    {
        if (_MotionManager.MotionPrior <= HIT_PRIOR)
        {
            float hitTime = -1;
            int hitEffect = 0;
            if (eventArgs.ContainsKey("HitTime"))
            {
                hitTime = (float)eventArgs["HitTime"];
            }

            if (eventArgs.ContainsKey("HitEffect"))
            {
                hitEffect = (int)eventArgs["HitEffect"];
            }
            MotionManager impactSender = go as MotionManager;
            MotionHit(hitTime, hitEffect, impactSender);
        }
        else if (_MotionManager.MotionPrior == FLY_PRIOR)
        {
            if ((GameBase.EVENT_TYPE)eventArgs["EVENT_TYPE"] == GameBase.EVENT_TYPE.EVENT_MOTION_HIT)
            {
                eventArgs.Add("StopEvent", true);
            }
            var hitTime = (float)eventArgs["HitTime"];

            int hitEffect = 0;
            if (eventArgs.ContainsKey("HitEffect"))
            {
                hitEffect = (int)eventArgs["HitEffect"];
            }
            MotionManager impactSender = go as MotionManager;
            MotionFlyStay(hitTime, hitEffect, impactSender);
        }
    }

    public void FlyEvent(object go, Hashtable eventArgs)
    {
        float flyHeight = (float)eventArgs["FlyHeight"];
        int hitEffect = 0;
        if (eventArgs.ContainsKey("HitEffect"))
        {
            hitEffect = (int)eventArgs["HitEffect"];
        }
        MotionManager impactSender = go as MotionManager;
        MotionFly(flyHeight, hitEffect, impactSender);
    }

    public void DispatchAnimEvent(string funcName, object param)
    {
        switch (_MotionManager.MotionPrior)
        {
            case HIT_PRIOR:
                DispatchHitEvent(funcName, param);
                break;
        }
    }

    void Update()
    {
        UpdateMove();
        FlyUpdate();
    }

    #region idle

    public AnimationClip _IdleAnim;

    public bool CanMotionIdle()
    {
        if (_MotionManager.MotionPrior != MOVE_PRIOR && _MotionManager.MotionPrior > IDLE_PRIOR)
            return false;

        if (_MotionManager.ActingSkill != null)
            return false;

        return true;
    }

    public void MotionIdle()
    {
        _MotionManager.MotionPrior = IDLE_PRIOR;
        _MotionManager.PlayAnimation(_IdleAnim);
    }

    #endregion

    #region move

    public AnimationClip _MoveAnim;

    private NavMeshAgent _NavAgent;

    public bool CanMotionMove()
    {
        if (_MotionManager.MotionPrior > MOVE_PRIOR)
            return false;

        if (_MotionManager.ActingSkill != null)
            return false;

        return true;
    }

    public void MoveDirect(Vector2 direct)
    {

        Vector3 derectV3 = new Vector3(direct.x, 0, direct.y);

        MoveDirect(derectV3);
    }

    public void MoveDirect(Vector3 derectV3)
    {
        _MotionManager.MotionPrior = MOVE_PRIOR;
        Vector3 destPoint = transform.position + derectV3.normalized * Time.deltaTime * _NavAgent.speed * _MotionManager._RoleAttrManager.MoveSpeed;
        _MotionManager.PlayAnimation(_MoveAnim, _MotionManager._RoleAttrManager.MoveSpeed);
        _MotionManager.transform.rotation = Quaternion.LookRotation(derectV3);

        //NavMeshHit navHit = new NavMeshHit();
        //if (!NavMesh.SamplePosition(destPoint, out navHit, 5, NavMesh.AllAreas))
        //{
        //    return;
        //}
        _NavAgent.Warp(destPoint);
    }

    #endregion

    #region push

    private Vector3 _TargetVec;
    private float _LastTime;
    private float _Speed;

    public void SetRotate(Vector3 rotate)
    {
        transform.rotation = Quaternion.LookRotation(rotate);
    }

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

    #region hit

    public AnimationClip _HitAnim;

    private float _StopKeyFrameTime = 0.0f;

    private void DispatchHitEvent(string funcName, object param)
    {
        switch (funcName)
        {
            case AnimationEvent.KEY_FRAME:
                HitKeyframe(param);
                break;
            case AnimationEvent.ANIMATION_END:
                HitEnd();
                break;
        }
    }

    public void MotionHit(float hitTime, int hitEffect, MotionManager impactSender)
    {
        PlayHitEffect(impactSender, hitEffect);

        if (hitTime > _HitAnim.length)
        {
            _StopKeyFrameTime = hitTime - _HitAnim.length;
        }
        else
        {
            _StopKeyFrameTime = 0;
        }
        _MotionManager.MotionPrior = HIT_PRIOR;
        _MotionManager.RePlayAnimation(_HitAnim, 1);

    }

    public void HitKeyframe(object param)
    {
        if (_StopKeyFrameTime > 0)
        {
            _MotionManager.PauseAnimation(_HitAnim);
            StartCoroutine(ComsumeAnim());
        }
    }

    public IEnumerator ComsumeAnim()
    {
        yield return new WaitForSeconds(_StopKeyFrameTime);
        _MotionManager.ResumeAnimation(_HitAnim);
    }

    public void HitEnd()
    {
        _MotionManager.MotionPrior = IDLE_PRIOR;
    }

    protected void PlayHitEffect(MotionManager impactSender, int effectIdx)
    {
        if (ResourcePool.Instance._CommonHitEffect.Length > effectIdx && effectIdx >= 0)
        {
            _MotionManager.PlayDynamicEffect(ResourcePool.Instance._CommonHitEffect[effectIdx]);
        }
    }

    #endregion

    #region fly

    private const float _UpSpeed = 20;
    private const float _DownSpeed = 15;
    private const float _LieTimeStatic = 1;

    public AnimationClip _FlyAnim;

    private GameObject _FlyBody;
    private float _FlyHeight = 0;
    private float _StayTime = 0;
    private float _LieTime = 0;

    public void MotionFly(float flyHeight, int effectID, MotionManager impactSender)
    {
        PlayHitEffect(impactSender, effectID);

        _MotionManager.MotionPrior = FLY_PRIOR;
        _MotionManager.RePlayAnimation(_FlyAnim, 1);

        _FlyHeight = flyHeight;
    }

    public void MotionFlyStay(float time, int effectID, MotionManager impactSender)
    {
        PlayHitEffect(impactSender, effectID);

        _MotionManager.MotionPrior = FLY_PRIOR;
        _MotionManager.RePlayAnimation(_FlyAnim, 1);

        _StayTime = time;
    }

    public void FlyUpdate()
    {
        if (_StayTime > 0)
        {
            _FlyHeight = 0f;
            _StayTime -= Time.fixedDeltaTime;
        }
        else if (_FlyHeight > 0)
        {
            _FlyBody.transform.localPosition += _UpSpeed * Time.fixedDeltaTime * Vector3.up;

            if (_FlyBody.transform.localPosition.y > _FlyHeight)
            {
                _FlyBody.transform.localPosition = new Vector3(0, _FlyHeight, 0);
                _FlyHeight = 0;
            }
        }
        else if (_FlyBody.transform.localPosition.y > 0)
        {
            _FlyBody.transform.localPosition -= _DownSpeed * Time.fixedDeltaTime * Vector3.up;
            if (_FlyBody.transform.localPosition.y < 0)
            {
                _FlyBody.transform.localPosition = Vector3.zero;
            }
            _LieTime = _LieTimeStatic;
        }
        else if (_LieTime > 0)
        {
            _LieTime -= Time.fixedDeltaTime;
            if (_LieTime <= 0)
            {
                MotionRise();
            }
        }
    }

    #endregion

    #region rise

    public AnimationClip _RiseAnim;

    private void MotionRise()
    {
        _MotionManager.MotionPrior = RISE_PRIOR;
        _MotionManager.PlayAnimation(_RiseAnim);
    }

    #endregion
}