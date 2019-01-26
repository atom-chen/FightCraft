using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FightManager : InstanceBase<FightManager>
{

    // Use this for initialization
    void Start ()
    {
        SetInstance(this);
        _InitProcess = 0;

        StartCoroutine(InitFightManager());
    }

    IEnumerator InitFightManager()
    {
        InitResourcePool();
        _InitProcess = 0.2f;
        yield return new WaitForFixedUpdate();

        InitScene();
        _InitProcess = 0.2f;

        InitMainRole();
        _InitProcess = 0.4f;

        InitCamera();
        _InitProcess = 0.7f;

        UIDamagePanel.ShowAsyn();
        AimTargetPanel.ShowAsyn();

        InitMonsterPrefab();
        _InitProcess = 1f;
        yield return new WaitForFixedUpdate();
    }

    #region Init

    public CameraFollow _CameraFollow;
    private int _ActingRegion;
    private List<GameObject> _SceneSPObj = new List<GameObject>();

    private float _InitProcess = 0;
    public float InitProcess
    {
        get
        {
            return _InitProcess;
        }
    }

    private void InitCamera()
    {
        GameObject cameraRoot = new GameObject("CameraRoot");

        Camera sceneCamera = Camera.main;
        if (sceneCamera == null)
        {
            GameObject go = new GameObject("Camera");
            sceneCamera = go.AddComponent<Camera>();
            go.tag = "MainCamera";
        }
        cameraRoot.transform.position = sceneCamera.transform.position;
        cameraRoot.transform.rotation = sceneCamera.transform.rotation;

        sceneCamera.transform.SetParent(cameraRoot.transform);
        sceneCamera.transform.localPosition = Vector3.zero;
        sceneCamera.transform.localRotation = Quaternion.Euler(Vector3.zero);
        sceneCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));
        //cameraRoot.AddComponent<AudioListener>();

        var subUICamera = ResourceManager.Instance.GetInstanceGameObject("Common/SubUICamera");
        subUICamera.transform.SetParent(sceneCamera.transform);
        subUICamera.transform.localPosition = Vector3.zero;
        subUICamera.transform.localRotation = Quaternion.Euler(Vector3.zero);

        _ActingRegion = 0;
        _CameraFollow = cameraRoot.AddComponent<CameraFollow>();
        _CameraFollow._FollowObj = _MainChatMotion.gameObject;
        _CameraFollow._Distance = LogicManager.Instance.EnterStageInfo.CameraOffset[_ActingRegion];

        var globalEffect = cameraRoot.AddComponent<GlobalEffect>();
        var inputManager = cameraRoot.AddComponent<InputManager>();
        var aimManager = cameraRoot.AddComponent<AimTarget>();
        inputManager._InputMotion = _MainChatMotion;

        for (int i = 0; i < LogicManager.Instance.EnterStageInfo.ValidScenePath.Count; ++i)
        {
            var spGO = GameObject.Find(LogicManager.Instance.EnterStageInfo.ValidScenePath[i] + "_SP");
            _SceneSPObj.Add(spGO);

            if (i > 0)
            {
                _SceneSPObj[i].SetActive(false);
            }
        }
        if (_SceneSPObj.Count > 0)
        {
            _SceneSPObj[0].SetActive(true);
        }


        _FightLevel = ActData.Instance.GetStageLevel();
    }

    private void InitResourcePool()
    {
        gameObject.AddComponent<ResourcePool>();
    }

    private void InitMonsterPrefab()
    {
        var fightLogic = _FightScene as FightSceneLogicPassArea;
        if (fightLogic == null)
            return;

        ResourcePool.Instance.InitMonsterBase(fightLogic.GetLogicMonIDs());
    }

    #endregion

    #region Objects

    public int _FightLevel;

    public int GetEliteMonsterRate()
    {
        return GameDataValue.GetMaxRate();
    }

    private MotionManager _MainChatMotion;
    public MotionManager MainChatMotion
    {
        get
        {
            return _MainChatMotion;
        }  
    }

    private void InitMainRole()
    {
        string mainBaseName = PlayerDataPack.Instance._SelectedRole.MainBaseName;
        string modelName = PlayerDataPack.Instance._SelectedRole.ModelName;
        string weaponName = PlayerDataPack.Instance._SelectedRole.GetWeaponModelName();

        var mainBase = ResourceManager.Instance.GetInstanceGameObject("ModelBase/" + mainBaseName);
        _MainChatMotion = mainBase.GetComponent<MotionManager>();

        _MainChatMotion.SetPosition(_FightScene._MainCharBornPos.position);
        _MainChatMotion.SetRotate(_FightScene._MainCharBornPos.rotation.eulerAngles);
        mainBase.tag = "Player";
        //_MainChatMotion.InitRoleAttr();

        var model = ResourceManager.Instance.GetInstanceGameObject("Model/" + modelName);
        model.transform.SetParent(mainBase.transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var weapon = ResourceManager.Instance.GetInstanceGameObject("Model/" + weaponName);
        var weaponTrans = model.transform.Find("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/righthand/rightweapon");
        var weaponTransChild = weaponTrans.GetComponentsInChildren<Transform>();
        for (int i = weapon.transform.childCount - 1; i >= 0; --i)
        {
            weapon.transform.GetChild(i).SetParent(weaponTrans.parent);
        }
        foreach (var oldWeaponChild in weaponTransChild)
        {
            GameObject.Destroy(oldWeaponChild.gameObject);
        }
        GameObject.Destroy(weapon.gameObject);

        //PlayerDataPack.Instance._SelectedRole.InitExAttrs();
        var motionTran = mainBase.transform.Find("Motion");

        GlobalBuffData.Instance.ActBuffInFight();
        UITestEquip.ActBuffInFight();
        SummonSkill.Instance.InitSummonMotions();
        List<string> skillMotions = SkillData.Instance.GetRoleSkills();

        foreach (var skillMotion in skillMotions)
        {
            var motionObj = ResourceManager.Instance.GetInstanceGameObject("SkillMotion/" + PlayerDataPack.Instance._SelectedRole.MotionFold + "/" + skillMotion);
            if (motionObj != null)
            {
                motionObj.transform.SetParent(motionTran);
                motionObj.transform.localPosition = Vector3.zero;
                motionObj.SetActive(true);
                var skillBase = motionObj.GetComponent<ObjMotionSkillBase>();
                if (skillBase == null)
                    continue;

                SetSkillElement(skillMotion, skillBase);
            }
        }

        foreach (var impact in RoleData.SelectRole._BaseAttr._ExAttr)
        {
            if (impact is RoleAttrImpactAddSkill)
            {
                var skillBase = (impact as RoleAttrImpactAddSkill).GetSkillBase();
                skillBase.transform.SetParent(motionTran);
                skillBase.transform.localPosition = Vector3.zero;
                skillBase.gameObject.SetActive(true);
            }
        }

        _MainChatMotion.InitMotion();
        FightLayerCommon.SetPlayerLayer(_MainChatMotion);
        //UIHPPanel.ShowHPItem(_MainChatMotion);

        GameCore.Instance.EventController.RegisteEvent( EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, RoleLevelUp);
    }

    private void SetSkillElement(string skillName, ObjMotionSkillBase skillBase)
    {
        

    }

    #endregion

    #region scene obj

    List<MotionManager> _MonMotion = new List<MotionManager>();

    private int _SceneEnemyCnt = 0;
    public int SceneEnemyCnt
    {
        get
        {
            return _SceneEnemyCnt;
        }
    }

    public MotionManager InitEnemy(string monsterID, Vector3 pos, Vector3 rot, bool isElite = false)
    {
        Tables.MonsterBaseRecord monsterBase = Tables.TableReader.MonsterBase.GetRecord(monsterID);
        if (isElite)
        {
            monsterBase = Tables.TableReader.MonsterBase.GetGroupElite(monsterBase);
        }
        if (monsterBase == null)
            return null;

        var mainBase = ResourcePool.Instance.GetIdleMotion(monsterBase);
        mainBase.SetPosition(pos);
        mainBase.SetRotate(rot);
        
        mainBase.InitRoleAttr(monsterBase);
        mainBase.InitMotion();
        FightLayerCommon.SetEnemyLayer(mainBase);

        AI_Base aiBase = mainBase.GetComponent<AI_Base>();
        aiBase.SetCombatLevel(10);

        _MonMotion.Add(mainBase);

        ++_SceneEnemyCnt;

        return mainBase;
    }

    public void ObjDisapear(MotionManager objMotion)
    {
        _MonMotion.Remove(objMotion);

        ResourcePool.Instance.RecvIldeMotion(objMotion);
        
        --_SceneEnemyCnt;
    }

    public void ObjCorpse(MotionManager objMotion)
    {
        _FightScene.MotionDie(objMotion);
        
        --_SceneEnemyCnt;

        if (objMotion.MonsterBase != null)
        {
            Hashtable hash = new Hashtable();
            hash.Add("MonsterInfo", objMotion);
            GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_KILL_MONSTER, this, hash);
        }
    }

    public void KillAllMotion()
    {
        foreach (var motion in _MonMotion)
        {
            if(!motion.IsMotionDie)
                motion.MotionDie();
        }
    }


    #endregion

    #region scene

    private FightSceneLogicBase _FightScene;

    private void InitScene()
    {
        var sceneGO = ResourcePool.Instance.CreateFightSceneObj("FightSceneLogic/" + LogicManager.Instance.EnterStageInfo.FightLogicPath);
        sceneGO.SetActive(true);
        _FightScene = sceneGO.GetComponent<FightSceneLogicBase>();
        if (_FightScene is FightSceneLogicRandomArea)
        { }
        else
        {
            var areaGroups = GameObject.FindObjectsOfType<AreaGroup>();
            foreach (var area in areaGroups)
            {
                area.gameObject.SetActive(false);
            }
        }
        StartCoroutine(StartSceneLogic());
    }

    private IEnumerator StartSceneLogic()
    {
        yield return new WaitForFixedUpdate();
        _FightScene.StartLogic();
    }

    public void OnObjDie()
    {

    }

    public void StagePass()
    {
        ActData.Instance.PassStage(LogicManager.Instance.EnterStageInfo.StageType);
    }

    public void LogicFinish(bool isWin)
    {
        Debug.Log("LogicFinish");
        LogicManager.Instance.ExitFight();

        Hashtable hash = new Hashtable();
        hash.Add("IsWin", isWin);
        GameCore.Instance.EventController.PushEvent(EVENT_TYPE.EVENT_LOGIC_EXIT_STAGE, this, hash);

        GameCore.Instance.EventController.UnRegisteEvent(EVENT_TYPE.EVENT_LOGIC_ROLE_LEVEL_UP, RoleLevelUp);

        GlobalBuffData.Instance.DeactBuffOutFight();
    }



    #endregion

    #region combo

    private int _Combo = 0;
    public int Combo
    {
        get
        {
            return _Combo;
        }
    }

    #endregion

    #region region teleport

    public void TeleportToNextRegion(Transform destTrans, bool transScene = true)
    {
        FightManager.Instance.MainChatMotion.SetPosition(destTrans.position);
        FightManager.Instance.MainChatMotion.SetRotate(destTrans.rotation.eulerAngles);
        _SceneSPObj[_ActingRegion].SetActive(false);
        if (transScene)
        {
            ++_ActingRegion;
        }
        _SceneSPObj[_ActingRegion].SetActive(true);
        _CameraFollow._Distance = LogicManager.Instance.EnterStageInfo.CameraOffset[_ActingRegion];

        //var effectPrefab = ResourceManager.Instance.GetEffect("Born2");
        //var effectSingle = effectPrefab.GetComponent<EffectSingle>();
        //var effectInstance = FightManager.Instance.MainChatMotion.PlayDynamicEffect(effectSingle);
        //effectInstance.transform.position = destTrans.position;


        if (_FightScene is FightSceneLogicPassArea)
        {
            (_FightScene as FightSceneLogicPassArea).StartNextArea();
        }
    }

    public void RoleLevelUp(object sender, Hashtable arg)
    {
        var effectPrefab = ResourceManager.Instance.GetEffect("Born2");
        var effectSingle = effectPrefab.GetComponent<EffectSingle>();
        var effectInstance = FightManager.Instance.MainChatMotion.PlayDynamicEffect(effectSingle);
        effectInstance.transform.position = FightManager.Instance.MainChatMotion.transform.position;

    }

    public void MoveToNewScene(string sceneName)
    {
        var sceneCnt = SceneManager.sceneCount;
        for (int i = 0; i < sceneCnt; ++i)
        {
            var sceneInfo = SceneManager.GetSceneAt(i);
            if (sceneInfo.name == sceneName)
            {
                SceneManager.MoveGameObjectToScene(_CameraFollow.gameObject, sceneInfo);
                SceneManager.MoveGameObjectToScene(MainChatMotion.gameObject, sceneInfo);
                SceneManager.MoveGameObjectToScene(gameObject, sceneInfo);
                //SceneManager.MoveGameObjectToScene(_FightScene.gameObject, sceneInfo);

                SceneManager.SetActiveScene(sceneInfo);
            }
        }
    }

    #endregion
}
