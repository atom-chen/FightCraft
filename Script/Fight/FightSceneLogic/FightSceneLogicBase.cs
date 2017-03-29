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

    #endregion
}
