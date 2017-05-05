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
    }

    protected virtual void UpdateLogic()
    {

    }

    public virtual void MotionDie(MotionManager motion)
    { }

    #endregion

    protected void LogicFinish()
    {
        StartCoroutine(ExitFightLogic());
    }

    private IEnumerator ExitFightLogic()
    {
        yield return new WaitForSeconds(3);
        FightManager.Instance.LogicFinish(true);
    }
}
