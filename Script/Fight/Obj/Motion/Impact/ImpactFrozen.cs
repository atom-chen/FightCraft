﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImpactFrozen : ImpactBuff
{
    public float _HitAfter = 0.1f;

    private List<MotionManager> _ExcludeMotions = new List<MotionManager>();

    public override void ActBuff(MotionManager senderManager, MotionManager reciverManager)
    {
        base.ActBuff(senderManager, reciverManager);

        StartCoroutine(StopAnimation(reciverManager));
        StartCoroutine(TimeOut(reciverManager));
    }

    private IEnumerator StopAnimation(MotionManager reciverManager)
    {
        yield return new WaitForSeconds(_HitAfter);
        reciverManager.PauseAnimation();
    }

    public override void RemoveBuff(MotionManager reciverManager)
    {
        base.RemoveBuff(reciverManager);

        reciverManager.ResumeAnimation();
    }


}