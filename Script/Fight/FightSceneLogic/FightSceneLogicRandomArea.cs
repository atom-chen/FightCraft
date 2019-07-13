using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightSceneLogicRandomArea : FightSceneLogicBase
{

    protected AreaGroup _ActingGroup;
    protected int _RunningIdx;

    public override void StartLogic()
    {
        base.StartLogic();

        InitPos();

        InitMons();
        //InitAreas();
    }

    protected override void UpdateLogic()
    {
        base.UpdateLogic();

        for (int i = 0; i < _ActingGroup._FightAreas.Count; ++i)
        {
            if (i == MainCharPosIdxInFightArea)
                continue;

            if (!_ActingGroup._FightAreas[i].AreaStrated)
            {
                //float dis = Vector3.Distance(FightManager.Instance.MainChatMotion.transform.position, area.transform.position);
                float dis = AI_Base.GetPathLength(FightManager.Instance.MainChatMotion.transform.position, _ActingGroup._FightAreas[i].transform.position);

                if (dis < 25)
                {
                    _ActingGroup._FightAreas[i].InitArea();
                }
            }
        }
    }

    #region motion die

    public static int _FindBossKillCnt = 120;
    private int _KillCnt = 0;
    private bool _IsChangeToBoss = false;
    public bool IsChangeToBoss
    {
        get
        {
            return _IsChangeToBoss;
        }
    }

    public override void MotionDie(MotionManager motion)
    {
        ++_KillCnt;
        if (_KillCnt >= _FindBossKillCnt)
        {
            if (!_IsChangeToBoss)
            {
                _IsChangeToBoss = true;
                CreateTeleport();
            }
        }

        if (motion.MonsterBase.MotionType == Tables.MOTION_TYPE.Hero)
        {
            LogicFinish(true);
        }
    }

    private void CreateTeleport()
    {
        var teleportGO = ResourceManager.Instance.GetGameObject("Common/TeleportRed");
        var teleportInstance = GameObject.Instantiate(teleportGO);
        teleportInstance.transform.position = FightManager.Instance.MainChatMotion.transform.position;
        var gate = teleportInstance.AddComponent<AreaGateRandom>();
        gate.RandomLogic = this;
    }

    #endregion

    #region 

    #endregion

    #region 

    public int MainChatPosIdx { get; set; }
    public List<int> NormalMonster { get; set; }
    public int MagicMonster { get; set; }
    public int BossID { get; set; }

    private int MainCharPosIdxInFightArea;
    private List<string> _ExcludeScene;
    private string _CurActScene;

    private void InitPos()
    {
        _ActingGroup = GameObject.Find("RandomAreas").GetComponent<AreaGroup>();
        //_ActingGroup.gameObject.SetActive(false);
        for (int i = 0; i < _ActingGroup._FightAreas.Count; ++i)
        {
            _ActingGroup._FightAreas[i].RandomLogic = this;
            _ActingGroup._FightAreas[i].AreaID = i;
            _ActingGroup._FightAreas[i].gameObject.SetActive(true);
        }

        var areaGates = _ActingGroup.GetComponentsInChildren<AreaGateRandom>();
        for (int i = 0; i < areaGates.Length; ++i)
        {
            areaGates[i].RandomLogic = this;
            areaGates[i].gameObject.SetActive(true);
        }

        MainChatPosIdx = Random.Range(0,  _ActingGroup._TeleAreas.Count);
        //MainChatPosIdx = 0;
        _MainCharBornPos = _ActingGroup._TeleAreas[MainChatPosIdx].transform;
        MainCharPosIdxInFightArea = _ActingGroup._FightAreas.IndexOf(_ActingGroup._TeleAreas[MainChatPosIdx]);
        _ActingGroup._TeleAreas[MainChatPosIdx].gameObject.SetActive(false);
        if (FightManager.Instance.MainChatMotion != null)
        {
            FightManager.Instance.MainChatMotion.SetPosition(_MainCharBornPos.position);
            FightManager.Instance.MainChatMotion.SetRotate(_MainCharBornPos.rotation.eulerAngles);
        }
        

        if (FightManager.Instance._CameraFollow != null)
        {
            float cameraDis = Mathf.Sqrt(FightManager.Instance._CameraFollow._Distance.x * FightManager.Instance._CameraFollow._Distance.x + FightManager.Instance._CameraFollow._Distance.z * FightManager.Instance._CameraFollow._Distance.z);
            int randomRadio = Random.Range(0, 360 / 15 + 1);
            int radio = randomRadio * 15;
            float x = cameraDis * Mathf.Sin(radio / Mathf.Rad2Deg);
            float y = cameraDis * Mathf.Cos(radio / Mathf.Rad2Deg);
            FightManager.Instance._CameraFollow._Distance = new Vector3(x, FightManager.Instance._CameraFollow._Distance.y, y);
        }

        if (_ExcludeScene == null)
        {
            _ExcludeScene = new List<string>();
            _ExcludeScene.Add(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            _CurActScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        else
        {
            _CurActScene = _ExcludeScene[_ExcludeScene.Count - 1];
        }

        _ActingGroup._LightGO.SetActive(true);
    }

    private void InitAreas()
    {
        for (int i = 0; i < _ActingGroup._FightAreas.Count; ++i)
        {
            if (i != MainCharPosIdxInFightArea)
            //if (i == 1)
            {
                _ActingGroup._FightAreas[i].InitArea();
            }
        }
    }

    private void InitMons()
    {
        //if (IsChangeToBoss)
        //{
        //    int bossID = LogicManager.Instance.EnterStageInfo.ExParam[1];
        //    NormalMonster = new List<int>() { bossID };
        //}
        //else
        {
            NormalMonster = GetRandomNormalMon();
            MagicMonster = GetRandomMagicMon();

            if (FightSceneAreaRandom.IsDiffBossElite(ActData.Instance.GetNormalDiff()))
            {
                int bossID = LogicManager.Instance.EnterStageInfo.ExParam[2];
                BossID = bossID;
            }
            else
            {
                int bossID = LogicManager.Instance.EnterStageInfo.ExParam[1];
                BossID = bossID;
            }
        }
    }

    public string GetNextScene()
    {
        string nextScene = "";
        if (_IsChangeToBoss)
        {
            nextScene = GetBossScene(_ExcludeScene);
        }
        else
        {
            nextScene = GetRandomScene(_ExcludeScene);
        }
        _ExcludeScene.Add(nextScene);
        return nextScene;
    }

    public void TeleportToNext()
    {
        for (int i = 0; i < _ActingGroup._FightAreas.Count; ++i)
        {
            _ActingGroup._FightAreas[i].ClearAllEnemy();
        }
        _ActingGroup._LightGO.SetActive(false);
        _ActingGroup.gameObject.SetActive(false);

        var oldSceneName = _CurActScene;

        InitPos();

        FightManager.Instance.MoveToNewScene(_CurActScene);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(oldSceneName);

        InitAreas();
    }

    #endregion

    #region random random res

    public static List<string> _ShaMoScene = new List<string>() {
        "Stage_ShaMo_01",
        "Stage_ShaMo_02",
        "Stage_ShaMo_05",
        "Stage_ShaMo_06",
        "Stage_ShaMo_07",
        "Stage_ShaMo_08",
        "Stage_ShaMo_09",
        "Stage_ShaMo_10",
        "Stage_ShaMo_11",
        "Stage_ShaMo_12",
        "Stage_ShaMo_15",
    };

    public static List<string> _CaoYuanScene = new List<string>() {
        "Stage_CaoYuan_01",
        "Stage_CaoYuan_03",
        "Stage_CaoYuan_04",
        "Stage_CaoYuan_06",
        "Stage_CaoYuan_07",
    };

    public static List<string> _BingYuanScene = new List<string>() {
        "Stage_BingYuan_01",
        "Stage_BingYuan_02",
        "Stage_BingYuan_04",
        "Stage_BingYuan_05",
    };

    public static List<string> _DiChengScene = new List<string>() {
        "Stage_DiXiaCheng_01",
        "Stage_DiXiaCheng_02",
        "Stage_DiXiaCheng_03",
        "Stage_DiXiaCheng_04",
        "Stage_DiXiaCheng_05",
    };

    public static List<string> _BossScene = new List<string>()
    {
        "Stage_ShaMo_Boss",
        "Stage_CaoYuan_Boss",
        "Stage_BingYuan_Boss",
        "Stage_DiXiaCheng_Boss"
    };

    public static List<int> _NormalMon = new List<int>() { 21,23,25,27,29,33,37,48,50};
    public static List<int> _MagicMon = new List<int>() { 31, 39 };
    public static List<int> _QiLin = new List<int>() { 43, 44,45 };
    public static List<int> _QiLinEx = new List<int>() { 210, 211, 212 };
    public static List<int> _BossType = new List<int>()
    {
        1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20
    };
    public static List<int> _ExBossType = new List<int>()
    {
        101,102,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120
    };

    public static string GetRandomScene(List<string> excludeScene = null)
    {
        //if (excludeScene == null)
        //{
        //    return "Stage_DiXiaCheng_04";
        //}
        //else
        //{
        //    return "Stage_DiXiaCheng_03";
        //}
        int type = 0;
        if (excludeScene != null && excludeScene.Count > 0)
        {
            if (_ShaMoScene.Contains(excludeScene[0]))
                type = 1;
            if (_CaoYuanScene.Contains(excludeScene[0]))
                type = 2;
            if (_BingYuanScene.Contains(excludeScene[0]))
                type = 3;
            if (_DiChengScene.Contains(excludeScene[0]))
                type = 4;
        }
        else
        {
            type = Random.Range(1, 5);
            //type = 1;
        }

        return GetRandomScene(type, excludeScene);
    }

    public static string GetRandomScene(int type, List<string> excludeScene = null)
    {
        List<string> includeScenes = new List<string>();
        List<string> baseScene = null;
        switch (type)
        {
            case 1:
                baseScene = _ShaMoScene;
                break;
            case 2:
                baseScene = _CaoYuanScene;
                break;
            case 3:
                baseScene = _BingYuanScene;
                break;
            case 4:
                baseScene = _DiChengScene;
                break;
        }

        if (excludeScene != null)
        {
            if (excludeScene.Count == baseScene.Count)
            {
                excludeScene.Clear();
            }

            foreach (var scene in baseScene)
            {
                if (!excludeScene.Contains(scene))
                {
                    includeScenes.Add(scene);
                }
            }
        }
        else
        {
            foreach (var scene in baseScene)
            {
                includeScenes.Add(scene);
            }
        }

        int idx = Random.Range(0, includeScenes.Count);
        return includeScenes[idx];
    }

    public static string GetBossScene(List<string> excludeScene)
    {
        int type = 0;
        if (_ShaMoScene.Contains(excludeScene[0]))
            type = 0;
        if (_CaoYuanScene.Contains(excludeScene[0]))
            type = 1;
        if (_BingYuanScene.Contains(excludeScene[0]))
            type = 2;
        if (_DiChengScene.Contains(excludeScene[0]))
            type = 3;

        return _BossScene[type];
    }

    

    public static List<int> GetRandomNormalMon()
    {
        int randomCnt = GameRandom.GetRandomLevel(20, 50, 30);
        var monIds = GameRandom.GetIndependentRandoms(0, _NormalMon.Count, randomCnt + 1);
        List<int> randomNormals = new List<int>();
        foreach (var monIdx in monIds)
        {
            randomNormals.Add(_NormalMon[monIdx]);
        }

        return randomNormals;
    }

    public static int GetRandomBoss()
    {
        int idx = Random.Range(0, _BossType.Count);
        return _BossType[idx];
    }

    public static int GetRandomMagicMon()
    {
        int randomIdx = Random.Range(0, _MagicMon.Count);
        return _MagicMon[randomIdx];
    }

    #endregion
}
