using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightManager : MonoBehaviour
{
    #region instance

    private FightManager _Instance;

    public FightManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
        _Instance = this;
        InitScene();
        InitMainRole();
        InitResourcePool();
        InitCamera();
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        //LogicUpdate();
    }

    #region Init

    private void InitCamera()
    {
        GameObject cameraRoot = new GameObject("CameraRoot");
        cameraRoot.transform.position = Camera.main.transform.position;
        cameraRoot.transform.rotation = Camera.main.transform.rotation;
        Camera.main.transform.SetParent(cameraRoot.transform);
        Camera.main.transform.localPosition = Vector3.zero;
        Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var cameraFollow = cameraRoot.AddComponent<CameraFollow>();
        cameraFollow._FollowObj = _MainChatMotion.gameObject;
        cameraFollow._Distance = new Vector3(0, 8, -8);

        var globalEffect = cameraRoot.AddComponent<GlobalEffect>();
        var inputManager = cameraRoot.AddComponent<InputManager>();
        inputManager._InputMotion = _MainChatMotion;
    }

    private void InitResourcePool()
    {
        gameObject.AddComponent<ResourcePool>();
    }

    #endregion

    #region Objects

    private MotionManager _MainChatMotion;

    private void InitMainRole()
    {
        string mainBaseName = "MainCharBoy";
        string modelName = "Char_Boy_01_JL_AM";
        string weaponName = "Weapon_HW_01_SM";
        List<string> skillMotions = new List<string>() { "Attack", "Buff1", "Buff2", "Defence", "Dush", "Skill1", "Skill2", "Skill3" };

        var mainBase = GameBase.ResourceManager.Instance.GetInstanceGameObject("ModelBase/" + mainBaseName);
        _MainChatMotion = mainBase.GetComponent<MotionManager>();
        mainBase.transform.position = _FightScene._MainCharBornPos.position;
        mainBase.transform.rotation = _FightScene._MainCharBornPos.rotation;

        var model = GameBase.ResourceManager.Instance.GetInstanceGameObject("Model/" + modelName);
        model.transform.SetParent(mainBase.transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var weapon = GameBase.ResourceManager.Instance.GetInstanceGameObject("Model/" + weaponName);
        var weaponTrans = model.transform.FindChild("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/righthand/rightweapon");
        Debug.Log("weaponTrans:" + weaponTrans.name);
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

        var motionTran = mainBase.transform.FindChild("Motion");
        foreach (var skillMotion in skillMotions)
        {
            var motionObj = GameBase.ResourceManager.Instance.GetInstanceGameObject("SkillMotion/" + mainBaseName + "/" + skillMotion);
            if (motionObj != null)
            {
                motionObj.transform.SetParent(motionTran);
                motionObj.SetActive(true);
            }
        }
    }

    #endregion

    #region scene

    private FightSceneLogic _FightScene;

    private void InitScene()
    {
        var sceneGO = GameObject.Find("FightSceneLogic");
        _FightScene = sceneGO.GetComponent<FightSceneLogic>();
    }

    #endregion
}
