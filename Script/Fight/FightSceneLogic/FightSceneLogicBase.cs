using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 

public class FightSceneLogicBase : MonoBehaviour
{
    public Transform _MainCharBornPos;

    private bool _IsStart = false;

    void FixedUpdate()
    {
        if (!_IsStart)
            return;

        UpdateLogic();
    }

    #region 

    public virtual void StartLogic()
    {
        _IsStart = true;
        StartTimmer();
    }

    protected virtual void UpdateLogic()
    {

    }

    public virtual void MotionDie(MotionManager motion)
    {
        if (motion.RoleAttrManager.MotionType == MotionType.MainChar)
        {
            LogicFinish(false);
            return;
        }
    }

    #endregion

    #region timmer

    public int _LogicTimmer;

    private float _StartTime = 0;
    private bool _IsRunTimmer = false;

    public void StartTimmer()
    {
        _IsRunTimmer = true;
        _LogicTimmer = 0;
        InvokeRepeating("TimmerUpdate", 0, 1);
    }

    public void StopTimmer()
    {
        _IsRunTimmer = false;
    }

    public void TimmerUpdate()
    {
        if (_IsRunTimmer)
        {
            ++_LogicTimmer;
        }
    }


    #endregion

    protected void LogicFinish(bool isWin)
    {
        StartCoroutine(ExitFightLogic(isWin));
    }

    private IEnumerator ExitFightLogic(bool isWin)
    {
        yield return new WaitForSeconds(3);
        if (isWin)
        {
            FightManager.Instance.StagePass();
        }
        UIFightFinish.ShowAsyn(isWin);
    }
}
