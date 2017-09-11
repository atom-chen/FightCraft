﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectColliderCircle : SelectCollider
{
    public float _CircleTime = 1.0f;

    private float _AngleSpeed = 36;

    public void Update()
    {
        if (_Collider == null || _Collider.enabled == false)
            return;

        transform.localRotation = Quaternion.Euler(new Vector3(0, _AngleSpeed * Time.deltaTime, 0) + transform.localRotation.eulerAngles);
    }

    public override void ColliderStart()
    {
        _AngleSpeed = -(360 / _CircleTime) * _SkillMotion.SkillBaseSpeed;
        Debug.Log("_AngleSpeed:" + _AngleSpeed);
        base.ColliderStart();
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

   

}
