using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameUI;

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
    {
        if (motion.RoleAttrManager.MotionType == MotionType.MainChar)
        {
            LogicFinish(false);
            return;
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
        //FightManager.Instance.LogicFinish(true);
        UIFightFinish.ShowAsyn(isWin);
    }
}
