using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameLogic;

public class FightManager : SingleClass<FightManager>
{

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
        cameraFollow._Distance = LogicManager.Instance.EnterStageInfo.CameraOffset;

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

        var mainBase = GameBase.ResourceManager.Instance.GetInstanceGameObject("ModelBase/" + mainBaseName);
        _MainChatMotion = mainBase.GetComponent<MotionManager>();

        _MainChatMotion.SetPosition(_FightScene._MainCharBornPos.position);
        _MainChatMotion.SetRotate(_FightScene._MainCharBornPos.rotation.eulerAngles);
        mainBase.tag = "Player";
        _MainChatMotion.InitRoleAttr(null);

        var model = GameBase.ResourceManager.Instance.GetInstanceGameObject("Model/" + modelName);
        model.transform.SetParent(mainBase.transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var weapon = GameBase.ResourceManager.Instance.GetInstanceGameObject("Model/" + weaponName);
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

        PlayerDataPack.Instance._SelectedRole.InitExAttrs();
        var motionTran = mainBase.transform.FindChild("Motion");
        List<string> skillMotions = new List<string>() { "Attack", "Buff1", "Buff2", "Skill1", "Skill2", "Skill3", "Dush" };
        if (PlayerDataPack.Instance._SelectedRole.Profession == Tables.PROFESSION.BOY_DEFENCE)
        {
            skillMotions.Add("Defence");
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.PRO1_SPECIL_SKILL1))
            {
                skillMotions.Remove("Skill1");
                skillMotions.Add("");
            }
        }
        if (PlayerDataPack.Instance._SelectedRole.Profession == Tables.PROFESSION.GIRL_DOUGE)
        {
            skillMotions.Add("Roll");
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.PRO2_SPECIL_SKILL1))
            {
                skillMotions.Remove("Skill1");
                skillMotions.Add("Skill1Muti");
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.PRO2_SPECIL_SKILL2))
            {
                skillMotions.Remove("Skill2");
                skillMotions.Add("Skill2AvataMuti");
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.PRO2_SPECIL_SKILL3))
            {
                skillMotions.Remove("Skill3");
                skillMotions.Add("Skill3Assembly");
            }
        }

        foreach (var skillMotion in skillMotions)
        {
            var motionObj = GameBase.ResourceManager.Instance.GetInstanceGameObject("SkillMotion/" + mainBaseName + "/" + skillMotion);
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

        GameUI.UIHPPanel.ShowHPItem(_MainChatMotion);
    }

    private void SetSkillElement(string skillName, ObjMotionSkillBase skillBase)
    {
        if (skillName.Contains("Skill1"))
        {
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_FIRE_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Fire);
                skillBase.SetHitEffectElement(ElementType.Fire);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Fire);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_FIRE_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Fire);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Cold);
                skillBase.SetHitEffectElement(ElementType.Cold);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Cold);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Cold);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_LIGHTING_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Lighting);
                skillBase.SetHitEffectElement(ElementType.Lighting);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Lighting);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_LIGHTING_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Lighting);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_WIND_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Wind);
                skillBase.SetHitEffectElement(ElementType.Wind);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Wind);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL1_WIND_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Wind);
            }
        }
        else if (skillName.Contains("Skill2"))
        {
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_FIRE_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Fire);
                skillBase.SetHitEffectElement(ElementType.Fire);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Fire);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_FIRE_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Fire);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Cold);
                skillBase.SetHitEffectElement(ElementType.Cold);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Cold);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Cold);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_LIGHTING_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Lighting);
                skillBase.SetHitEffectElement(ElementType.Lighting);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Lighting);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_LIGHTING_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Lighting);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_WIND_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Wind);
                skillBase.SetHitEffectElement(ElementType.Wind);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Wind);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL2_WIND_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Wind);
            }
        }
        else if (skillName.Contains("Skill3"))
        {
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_FIRE_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Fire);
                skillBase.SetHitEffectElement(ElementType.Fire);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Fire);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_FIRE_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Fire);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Cold);
                skillBase.SetHitEffectElement(ElementType.Cold);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Cold);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Cold);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_LIGHTING_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Lighting);
                skillBase.SetHitEffectElement(ElementType.Lighting);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Lighting);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_LIGHTING_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Lighting);
            }

            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_WIND_DAMAGE))
            {
                skillBase.SetEffectElement(ElementType.Wind);
                skillBase.SetHitEffectElement(ElementType.Wind);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF1)
                || PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_COLD_BUFF2))
            {
                skillBase.SetHitEffectElement(ElementType.Wind);
            }
            if (PlayerDataPack.Instance._SelectedRole._ExAttrs.ContainsKey(Tables.FightAttr.FightAttrType.SKILL3_WIND_SPECIL))
            {
                skillBase.SetImpactElement(ElementType.Wind);
            }
        }

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

    public void InitEnemy(string monsterID, Vector3 pos, Vector3 rot)
    {
        var monsterBase = Tables.TableReader.MonsterBase.GetRecord(monsterID);
        if (monsterBase == null)
            return;

        var mainBase = ResourcePool.Instance.GetIdleMotion(monsterBase.Model);
        mainBase.SetPosition(pos);
        mainBase.SetRotate(rot);
        GameUI.UIHPPanel.ShowHPItem(mainBase);
        AI_Base aiBase = mainBase.GetComponent<AI_Base>();
        aiBase.SetCombatLevel(10);

        mainBase.InitRoleAttr(monsterBase);

        ++_SceneEnemyCnt;
    }

    public void ObjDie(MotionManager objMotion)
    {
        ResourcePool.Instance.RecvIldeMotion(objMotion);
        _FightScene.MotionDie(objMotion);

        --_SceneEnemyCnt;

        MonsterDrop.MonsterDripItems(objMotion);
    }

    #endregion

    #region scene

    private FightSceneLogicBase _FightScene;

    private void InitScene()
    {
        var sceneGO = GameBase.ResourceManager.Instance.GetInstanceGameObject("FightSceneLogic/" + LogicManager.Instance.EnterStageInfo.FightLogicPath);
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
        GameLogic.LogicManager.Instance.ExitFight();
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
}
