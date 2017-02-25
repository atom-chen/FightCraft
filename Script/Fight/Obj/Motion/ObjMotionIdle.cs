using UnityEngine;
using System.Collections;

public class ObjMotionIdle : ObjMotionBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

    }

    public override void PlayMotion(object go, Hashtable eventArgs)
    {
        MotionIdle();
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
        base.InitEvent();

        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_FINISH, PlayMotion);
    }
    #endregion

    public void MotionIdle()
    {

        _MotionManager.PlayAnimation(_AnimationClip);

    }

}
