using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 

public class FightManager : InstanceBase<FightManager>
{

    // Use this for initialization
    void Start ()
    {
        SetInstance(this);
        InitResourcePool();
        InitScene();
        InitMainRole();
        InitCamera();
    }

    #region Init

    private CameraFollow _CameraFollow;
    private int _ActingRegion;
    private List<GameObject> _SceneSPObj = new List<GameObject>();

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
        cameraRoot.AddComponent<AudioListener>();

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
        _SceneSPObj[0].SetActive(true);
    }

    private void InitResourcePool()
    {
        gameObject.AddComponent<ResourcePool>();
    }

    #endregion

    #region Objects

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
        _MainChatMotion.InitRoleAttr(null);

        var model = ResourceManager.Instance.GetInstanceGameObject("Model/" + modelName);
        model.transform.SetParent(mainBase.transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var weapon = ResourceManager.Instance.GetInstanceGameObject("Model/" + weaponName);
        var weaponTrans = model.transform.FindChild("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/righthand/rightweapon");
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
        var motionTran = mainBase.transform.FindChild("Motion");

        List<string> skillMotions = RoleData.SelectRole.GetRoleSkills();

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

        _MainChatMotion.InitMotion();
        FightLayerCommon.SetPlayerLayer(_MainChatMotion);
        UIHPPanel.ShowHPItem(_MainChatMotion);
    }

    private void SetSkillElement(string skillName, ObjMotionSkillBase skillBase)
    {
        

    }

    #endregion

    #region scene obj

    private int _SceneEnemyCnt = 0;
    public int SceneEnemyCnt
    {
        get
        {
            return _SceneEnemyCnt;
        }
    }

    public MotionManager InitEnemy(string monsterID, Vector3 pos, Vector3 rot)
    {
        var monsterBase = Tables.TableReader.MonsterBase.GetRecord(monsterID);
        if (monsterBase == null)
            return null;

        var mainBase = ResourcePool.Instance.GetIdleMotion(monsterBase);
        mainBase.SetPosition(pos);
        mainBase.SetRotate(rot);

        mainBase.InitRoleAttr(monsterBase);
        mainBase.InitMotion();
        FightLayerCommon.SetEnemyLayer(mainBase);

        UIHPPanel.ShowHPItem(mainBase);
        AI_Base aiBase = mainBase.GetComponent<AI_Base>();
        aiBase.SetCombatLevel(10);

        ++_SceneEnemyCnt;

        return mainBase;
    }

    public void ObjDisapear(MotionManager objMotion)
    {
        ResourcePool.Instance.RecvIldeMotion(objMotion);

        --_SceneEnemyCnt;
    }

    public void ObjCorpse(MotionManager objMotion)
    {
        _FightScene.MotionDie(objMotion);
        
        --_SceneEnemyCnt;
    }

    #endregion

    #region scene

    private FightSceneLogicBase _FightScene;

    private void InitScene()
    {
        var sceneGO = ResourceManager.Instance.GetInstanceGameObject("FightSceneLogic/" + LogicManager.Instance.EnterStageInfo.FightLogicPath);
        sceneGO.SetActive(true);
        _FightScene = sceneGO.GetComponent<FightSceneLogicBase>();
        StartCoroutine(StartSceneLogic());
    }

    private IEnumerator StartSceneLogic()
    {
        yield return new WaitForSeconds(2);
        _FightScene.StartLogic();
    }

    public void OnObjDie()
    {

    }

    public void LogicFinish(bool isWin)
    {
        Debug.Log("LogicFinish");
        LogicManager.Instance.ExitFight();
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

    public void TeleportToNextRegion(Transform destTrans)
    {
        FightManager.Instance.MainChatMotion.SetPosition(destTrans.position);
        FightManager.Instance.MainChatMotion.SetRotate(destTrans.rotation.eulerAngles);
        _SceneSPObj[_ActingRegion].SetActive(false);
        ++_ActingRegion;
        _SceneSPObj[_ActingRegion].SetActive(true);
        _CameraFollow._Distance = LogicManager.Instance.EnterStageInfo.CameraOffset[_ActingRegion];

        var effectPrefab = ResourceManager.Instance.GetEffect("Born2");
        var effectSingle = GameObject.Instantiate(effectPrefab).GetComponent<EffectSingle>();
        effectSingle.transform.position = destTrans.position;
        effectSingle.Play();

        if (_FightScene is FightSceneLogicPassArea)
        {
            (_FightScene as FightSceneLogicPassArea).StartNextArea();
        }
    }

    #endregion
}
