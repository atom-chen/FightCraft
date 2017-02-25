using UnityEngine;
using System.Collections;

public class ObjMotionMove : ObjMotionBase
{
    #region override

    public override void InitMotion(MotionManager manager)
    {
        base.InitMotion(manager);

        _NavAgent = manager.gameObject.GetComponent<NavMeshAgent>();
        _MotionPriority = 10;
    }

    public override void PlayMotion(object go, Hashtable eventArgs)
    {
        base.PlayMotion(go, eventArgs);

        Vector2 direct = (Vector2)eventArgs["Direct"];
        MoveDirect(direct);
    }

    public override bool ActiveInput(InputManager inputManager)
    {
        if (!base.IsCanActiveMotion())
            return false;
        if (inputManager._Axis != Vector2.zero)
        {
            //Hashtable hash = new Hashtable();
            //hash.Add("Direct", inputManager._Axis);
            //_MotionManager._EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_MOVE, _MotionManager, hash);
            _MotionManager.MotionStart(this);
            MoveDirect(inputManager._Axis);
            return true;
        }
        return false;
    }

    public override bool ContinueInput(InputManager inputManager)
    {
        base.ContinueInput(inputManager);

        if (inputManager._Axis != Vector2.zero)
        {
            //Hashtable hash = new Hashtable();
            //hash.Add("Direct", inputManager._Axis);
            //_MotionManager._EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_MOTION_MOVE, _MotionManager, hash);
            MoveDirect(inputManager._Axis);
        }
        else
        {

            _NavAgent.Stop();
            _NavAgent.ResetPath();
            _MotionManager.MotionFinish(this);
        }
        return true;
    }

    protected override void InitEvent()
    {
        base.InitEvent();

        _MotionManager._EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_MOTION_MOVE, PlayMotion);
    }

    public override void StopMotion()
    {
        base.StopMotion();

        _NavAgent.SetDestination(_NavAgent.transform.position);
    }

    public override bool IsCanActiveMotion()
    {
        if (_MotionManager.CurMotion == this)
            return true;
        return base.IsCanActiveMotion();
    }
    #endregion

    private NavMeshAgent _NavAgent;

    public void MoveDirect(Vector2 direct)
    {
        Vector3 derectV3 = new Vector3(direct.x, 0, direct.y);

        MoveDirect(derectV3);
    }

    public void MoveDirect(Vector3 derectV3)
    {
        Vector3 destPoint = transform.position + derectV3.normalized * Time.deltaTime * _NavAgent.speed *  _MotionManager._RoleAttrManager.MoveSpeed;
        _MotionManager.MotionStart(this);
        _MotionManager.PlayAnimation(_AnimationClip, _MotionManager._RoleAttrManager.MoveSpeed);
        _MotionManager.transform.rotation = Quaternion.LookRotation(derectV3);

        //Debug.Log("Trans Position BF:" + _NavAgent.transform.position);
        //_NavAgent.Move(derectV3.normalized * Time.deltaTime * _NavAgent.speed);
        //Debug.Log("Trans Position AF:" + _NavAgent.transform.position);

        NavMeshHit navHit = new NavMeshHit();
        if (!NavMesh.SamplePosition(destPoint, out navHit, 5, NavMesh.AllAreas))
        {
            return;
        }
        _NavAgent.Warp(navHit.position);
    }

}
