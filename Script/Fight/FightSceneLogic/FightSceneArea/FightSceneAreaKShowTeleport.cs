﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneAreaKShowTeleport : FightSceneAreaBase
{
    public override void StartArea()
    {
        base.StartArea();

        _Teleport.SetActive(true);
    }
    
    #region teleport

    public GameObject _Teleport;

    #endregion
}
