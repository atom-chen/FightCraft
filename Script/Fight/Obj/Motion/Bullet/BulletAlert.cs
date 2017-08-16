﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAlert : BulletBase
{

    #region static

    private static GameObject _AlertPrefab;

    public static void ShowAlert(Transform bullet, float time, float range)
    {
        if (_AlertPrefab == null)
        {
            _AlertPrefab = GameBase.ResourceManager.Instance.GetInstanceGameObject("Bullet/Bullets/AlertBase");
        }
        var alertGO = ResourcePool.Instance.GetIdleUIItem<BulletAlert>(_AlertPrefab);
        alertGO.transform.SetParent(bullet);
        alertGO.transform.localPosition = Vector3.zero;
        alertGO.GetComponentInChildren<ParticleSystem>().startSize = range;
        alertGO.StartCoroutine(alertGO.HidePrefab(time));
    }

    #endregion

    private IEnumerator HidePrefab(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResourcePool.Instance.RecvIldeUIItem(this.gameObject);
    }
}
