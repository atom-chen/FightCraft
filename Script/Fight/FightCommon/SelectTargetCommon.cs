using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectTargetCommon
{
    public static MotionManager GetMainPlayer()
    {
        if (FightManager.Instance != null)
        {
            return FightManager.Instance.MainChatMotion;
        }
        else
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            return player.GetComponent<MotionManager>();
        }

        return null;
    }

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

    public static List<MotionManager> GetNearMotions(MotionManager selfMotion, float length = 100)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<MotionManager> nearMotions = new List<MotionManager>();
        foreach (var motion in motions)
        {
            if (motion == selfMotion)
                continue;

            float distance = Vector3.Distance(selfMotion.transform.position, motion.transform.position);
            if (distance > length)
                continue;

            nearMotions.Add(motion);
        }

        return nearMotions;
    }

    public static List<MotionManager> GetFrontMotions(MotionManager selfMotion, float length, float angle, bool isSortByType)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<MotionManager> nearMotions = new List<MotionManager>();
        foreach (var motion in motions)
        {
            if (motion == selfMotion)
                continue;

            float distance = Vector3.Distance(selfMotion.transform.position, motion.transform.position);
            if (distance > length)
                continue;

            float targetAngle = Vector3.Angle(motion.transform.position - selfMotion.transform.position, selfMotion.transform.forward);
            if (targetAngle > angle)
                continue;

            nearMotions.Add(motion);
        }

        if (isSortByType)
        {
            nearMotions.Sort((motion1, motion2) =>
            {
                if ((int)motion1.RoleAttrManager.MotionType < (int)motion2.RoleAttrManager.MotionType)
                    return 1;
                else if ((int)motion1.RoleAttrManager.MotionType > (int)motion2.RoleAttrManager.MotionType)
                    return -1;
                else
                    return 0;
            });
        }

        return nearMotions;
    }

    public static List<MotionManager> GetFrontMotions(MotionManager selfMotion, float length)
    {
        var motions = GameObject.FindObjectsOfType<MotionManager>();

        List<MotionManager> nearMotions = GetFrontMotions(selfMotion, length, 30, false);

        return nearMotions;
    }
}
