﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class SerializeEnemyInfo
{
    public Transform _EnemyTransform;
    public string _EnemyDataID;
}


public class FightSceneAreaBase : MonoBehaviour
{
    public int AreaID { get; set; }

    protected bool _AreaStarted = false;

    public bool AreaStrated
    {
        get
        {
            return _AreaStarted;
        }
    }

    protected bool _AreaFinished = false;
    public bool AreaFinished
    {
        get
        {
            return _AreaFinished;
        }
    }

    public virtual void InitArea()
    { }

    public virtual void StartArea()
    {
        _AreaStarted = true;
        UpdateEnemyAlert();
        InitFightLogic();
        CloseAllDoor();
    }

    public virtual void FinishArea()
    {
        _AreaFinished = true;
        OpenAllDoor();
        _FightSceneLogic.AreaFinish(this);
    }

    public virtual void UpdateArea()
    {
        UpdateEnemyAlert();
    }

    public virtual void MotionDie(MotionManager motion)
    {
        

    }

    #region FightSceneLogic

    public FightSceneLogicPassArea _FightSceneLogic;

    private void InitFightLogic()
    {
        _FightSceneLogic = GetComponentInParent<FightSceneLogicPassArea>();
    }

    #endregion

    #region collider

    public enum TrigType
    {
        TRIG_AUTO,
        TRIG_EVENT,
    }

    protected float _EnemyAlertDistance = 8;
    public GameObject[] _AreaDoors;
    public TrigType _TrigAreaType;

    protected bool _IsEnemyAlert = false;

    private void UpdateEnemyAlert()
    {
        if (_IsEnemyAlert)
            return;

        if (_EnemyAlertDistance <= 0)
        {
            _IsEnemyAlert = true;
            return;
        }
    }

    private void CloseAllDoor()
    {
        foreach (var door in _AreaDoors)
        {
            door.SetActive(true);
        }
    }

    private void OpenAllDoor()
    {
        foreach (var door in _AreaDoors)
        {
            door.SetActive(false);
        }
    }
    #endregion

    public virtual Transform GetAreaTransform()
    {
        return null;
    }

    public virtual List<string> GetAreaMonIDs()
    {
        return null;
    }
}
