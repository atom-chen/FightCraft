using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletBase : SelectCollider
{
    public GameObject _BulletPrefab;
    public Vector3 _InitPositionOffset;

    public override void ColliderStart()
    {
        var bulletObj = GameObject.Instantiate(_BulletPrefab);
        bulletObj.transform.position = transform.position + _InitPositionOffset;
        bulletObj.transform.rotation = transform.rotation;
        bulletObj.SetActive(true);
    }
}
