﻿using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// 游戏核心
/// </summary>
public class GameCore : MonoBehaviour
{
    #region 固有

    public void Awake()
    {
        DontDestroyOnLoad(this);
        _Instance = this;
    }

    public void Start()
    {
        Tables.TableReader.ReadTables();
        UILogin.ShowAsyn();
    }

    public void Update()
    {
        if ((Application.platform == RuntimePlatform.Android
        || Application.platform == RuntimePlatform.WindowsPlayer
        || Application.platform == RuntimePlatform.WindowsEditor) && (Input.GetKeyDown(KeyCode.Escape)))
        {
            LogicManager.Instance.QuitGame();
            Debug.Log("save data");
        }

#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.C))
        {
            LogicManager.Instance.CleanUpSave();
        }

#endif
    }

    void OnApplicationQuit()
    {
        LogicManager.Instance.QuitGame();
    }
    #endregion

    #region 唯一

    private static GameCore _Instance = null;
    public static GameCore Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    #region 管理者

    /// <summary>
    /// 主UI画布
    /// </summary>
    [SerializeField]
    private UIManager _UIManager;
    public UIManager UIManager { get { return _UIManager; } }

    #endregion

    #region 

    public int _StrVersion = 0;

    #endregion

}

