using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectTargetCommon
{

    public static MotionManager GetNearMotion(Vector3 startPosition, List<MotionManager> excludeMotions)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        float minDistance = 0;
        MotionManager nearMotion = null;
        foreach (var motion in motions)
        {
            if (excludeMotions.Contains(motion))
                continue;

            float distance = Vector3.Distance(startPosition, motion.transform.position);
            if (nearMotion == null || distance < minDistance)
            {
                nearMotion = motion;
                minDistance = distance;
            }
        }

        return nearMotion;
    }

    public static List<MotionManager> GetNearMotions(MotionManager selfMotion)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<MotionManager> nearMotions = new List<MotionManager>();
        foreach (var motion in motions)
        {
            if (motion == selfMotion)
                continue;

            nearMotions.Add(motion);
        }

        return nearMotions;
    }
}
