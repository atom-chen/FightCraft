using UnityEngine;
using System.Collections;

public class ObjMotionFly : ObjMotionHit
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);
        if (_RiseAnim != null)
            _MotionManager.InitAnimation(_RiseAnim);
        _MotionPriority = 1001; 
    }

    public override void PlayMotion(object go, Hashtable eventArgs)
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

    public void PlayMotionHit(object go, Hashtable eventArgs)
    {
        if (_MotionManager.CurMotion != this)
            return;

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

    public override bool ActiveInput(InputManager inputManager)
    {
        return base.ActiveInput(inputManager);
    }

    public override bool ContinueInput(InputManager inputManager)
    {
        return base.ContinueInput(inputManager);
    }

    protected override void InitEvent()
    {
        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FLY, PlayMotion);
        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_HIT, PlayMotionHit, 10);
    }
    #endregion

    #region unity

    void FixedUpdate()
    {
        FlyUpdate();
    }

    #endregion

    private static float _UpSpeed = 20;
    private static float _DownSpeed = 15;
    private static float _LieTimeStatic = 1;

    public GameObject _FlyBody;
    public AnimationClip _RiseAnim;

    private float _FlyHeight = 0;
    private float _StayTime = 0;
    private float _LieTime = 0;

    public void MotionFly(float flyHeight, int effectID, MotionManager impactSender)
    {
        PlayHitEffect(effectID, impactSender);
        _MotionManager.MotionStart(this);

        _MotionManager.RePlayAnimation(_AnimationClip);

        _FlyHeight = flyHeight;
    }

    public void MotionFlyStay(float time, int effectID, MotionManager impactSender)
    {
        PlayHitEffect(effectID, impactSender);
        _MotionManager.MotionStart(this);

        _MotionManager.RePlayAnimation(_AnimationClip);

        _StayTime = time;
    }

    private void MotionRise()
    {
        _MotionManager.MotionFinish(this);
        _MotionManager.PlayAnimation(_RiseAnim);
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

}
