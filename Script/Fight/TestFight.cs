using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class TestFight : MonoBehaviour
{
    #region 

    public TestFight _Instance = null;

    #endregion

    // Update is called once per frame
    void Update ()
    {
        //if (_EnemyMotion == null)
        {
            if (!FindEnemy())
            {
                FindNextArea();
            }
        }

        //if (_EnemyMotion != null)
        {
            CloseUpdate();
        }

        UpdatePick();
    }

    void Start()
    {
        InputManager.Instance._EmulateMode = true;
        _NormalAttack = gameObject.GetComponentInChildren<ObjMotionSkillAttack>();
        InitDefence();

        _Instance = this;
    }

    #region find target

    private MotionManager _EnemyMotion;
    private Vector3 _NextAreaPos;

    private bool FindEnemy()
    {
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            float distance = Vector3.Distance(transform.position, _EnemyMotion.transform.position);
            if (distance < 2.0f)
                return true;
        }

        _EnemyMotion = null;
        var motions = GameObject.FindObjectsOfType<MotionManager>();
        float tarDistance = 10;
        foreach (var motion in motions)
        {
            if (motion.RoleAttrManager.MotionType != MOTION_TYPE.MainChar && !motion.IsMotionDie)
            {
                float distance = Vector3.Distance(transform.position, motion.transform.position);
                if (distance < tarDistance)
                {
                    _EnemyMotion = motion;
                    tarDistance = distance;
                }
            }
        }

        return _EnemyMotion != null;
    }

    private bool FindNextArea()
    {
        var fightManager = GameObject.FindObjectOfType<FightSceneLogicPassArea>();
        _NextAreaPos = fightManager.GetNextAreaPos();
        if (_NextAreaPos == Vector3.zero)
        {
            return false;
        }
        return true;
    }

    #endregion

    #region control

    private float _CloseRange = 2.0f;
    private float _SkillRange = 3.0f;
    private int _RandomSkillIdx = 0;
    private ObjMotionSkillAttack _NormalAttack;
    private int _WeaponSkill = -1;

    private void InitWeaponSkill()
    {
        if (_WeaponSkill != -1)
            return;

        var weaponItem = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.WEAPON);
        if (weaponItem == null || !weaponItem.IsVolid())
            return;

        foreach (var exAttrItem in weaponItem.EquipExAttr)
        {
            if (exAttrItem.AttrType != "RoleAttrImpactBaseAttr")
            {
                var attrTab = TableReader.AttrValue.GetRecord(exAttrItem.AttrParams[0].ToString());
                _WeaponSkill = int.Parse(attrTab.StrParam[1]);
            }
        }
    }

    private void CloseUpdate()
    {

        //if (FightManager.Instance.MainChatMotion.ActingSkill != null)
        //{
        //    StartSkill();
        //    return;
        //}

        var destPos = Vector3.zero;
        if (_NextAreaPos != Vector3.zero)
        {
            destPos = _NextAreaPos;
        }
        if (_EnemyMotion != null && !_EnemyMotion.IsMotionDie)
        {
            destPos = _EnemyMotion.transform.position;
        }
        if (destPos == Vector3.zero)
            return;

        float distance = Vector3.Distance(transform.position, destPos);
        if (FightManager.Instance.MainChatMotion._ActionState != FightManager.Instance.MainChatMotion._StateSkill && distance > _CloseRange)
        {
            FightManager.Instance.MainChatMotion.StartMoveState(destPos);
            ReleaseSkill();
        }
        else
        {
            FightManager.Instance.MainChatMotion.StopMoveState();
            
            StartSkill(destPos);
        }
    }

    private bool StartSkill(Vector3 destPos)
    {
        if (_EnemyMotion == null)
            return false;

        if (_EnemyMotion.IsMotionDie)
            return false;

        InitWeaponSkill();

        if (FightManager.Instance.MainChatMotion.ActingSkill == null)
        {
            ++_RandomSkillIdx;
            if (_RandomSkillIdx > 3)
            {
                _RandomSkillIdx = 1;
            }
            //FightManager.Instance.MainChatMotion.ActSkill(FightManager.Instance.MainChatMotion._SkillMotions["j"]);
            transform.LookAt(_EnemyMotion.transform.position);
            InputManager.Instance.SetEmulatePress("j");
        }

        else if (FightManager.Instance.MainChatMotion.ActingSkill == _NormalAttack)
        {
            if (_WeaponSkill > 0)
            {
                if (_NormalAttack.CurStep > 0 && _NormalAttack.CurStep == _WeaponSkill)
                {
                    transform.LookAt(_EnemyMotion.transform.position);
                    InputManager.Instance.SetEmulatePress("k");
                }
            }
            else if (_NormalAttack.CurStep > 0 && _NormalAttack.CurStep == _RandomSkillIdx)
            {
                //if (_NormalAttack.CanNextInput)
                {
                    transform.LookAt(_EnemyMotion.transform.position);
                    InputManager.Instance.SetEmulatePress("k");
                    Debug.Log("emulate key k:" + _RandomSkillIdx);
                    ++_RandomSkillIdx;
                    if (_RandomSkillIdx > 3)
                    {
                        _RandomSkillIdx = 1;
                    }
                }
            }
        }
        return true;
    }

    private void ReleaseSkill()
    {
        InputManager.Instance.ReleasePress();
    }
    #endregion

    #region defence skill



    public void InitDefence()
    {
        GameCore.Instance.EventController.RegisteEvent(EVENT_TYPE.EVENT_LOGIC_SOMEONE_SUPER_ARMOR, SomeOneSuperArmor);
    }

    private void SomeOneSuperArmor(object go, Hashtable eventArgs)
    {
        MotionManager motion = (MotionManager)eventArgs["Motion"];
        if (motion == null || motion == FightManager.Instance.MainChatMotion)
            return;

        CancelInvoke("CancleDefence");

        Debug.Log("Use defence skill");
        InputManager.Instance.SetEmulatePress("l");

        Invoke("CancleDefence", 1);
    }

    private void CancleDefence()
    {
        InputManager.Instance.ReleasePress();
    }

    #endregion

    #region pick items

    public void UpdatePick()
    {
        if (!UIDropNamePanel.Instance)
            return;

        //foreach (var dropItem in UIDropNamePanel.Instance._DropItems)
        //{
        //    dropItem.OnItemClick();
        //}
    }

    public static void DelAllEquip()
    {
        //equip
        foreach (var itemEquip in BackBagPack.Instance.PageEquips)
        {
            if (!itemEquip.IsVolid())
                continue;

            var equipedItem = RoleData.SelectRole.GetEquipItem(itemEquip.EquipItemRecord.Slot);
            bool changeEquip = false;
            if (equipedItem.IsVolid())
            {
                //if (itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.WEAPON)
                {
                    if (itemEquip.EquipLevel > equipedItem.EquipLevel)
                    {
                        changeEquip = true;
                    }
                    else if(itemEquip.EquipQuality > equipedItem.EquipQuality)
                    {
                        changeEquip = true;
                    }
                    
                }
                //else
                //{
                //    if (itemEquip.CombatValue > equipedItem.CombatValue)
                //        changeEquip = true;
                //}
            }
            else
            {
                changeEquip = true;
            }

            if (changeEquip)
            {
                RoleData.SelectRole.PutOnEquip(itemEquip.EquipItemRecord.Slot, itemEquip);
            }
        }

        //destory
        ItemEquip storeWeapon = null;
        foreach (var itemEquip in BackBagPack.Instance.PageEquips)
        {
            if (!itemEquip.IsVolid())
                continue;

            if (itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
            {
                if (!LegendaryData.Instance.IsCollect(itemEquip))
                {
                    LegendaryData.Instance.PutInEquip(itemEquip);
                    continue;
                }
            }

            if (itemEquip.EquipLevel > GameDataValue._DropMatLevel)
            {
                if (itemEquip.EquipQuality != ITEM_QUALITY.WHITE)
                {
                    EquipRefresh.Instance.DestoryMatCnt(itemEquip, false);
                    continue;
                }
            }

            if (itemEquip.EquipItemRecord.Slot == EQUIP_SLOT.WEAPON && itemEquip.RequireLevel > RoleData.SelectRole._RoleLevel)
            {
                if (storeWeapon == null)
                {
                    storeWeapon = itemEquip;
                    continue;
                }
                else if(storeWeapon.BaseAttack < itemEquip.BaseAttack)
                {
                    ShopData.Instance.SellItem(storeWeapon, false);
                    storeWeapon = itemEquip;
                    continue;
                }
            }

            ShopData.Instance.SellItem(itemEquip, false);
        }
    }

    public static void DelLevel()
    {
        while (RoleData.SelectRole.UnDistrubutePoint > 0)
        {
            RoleData.SelectRole.DistributePoint(1, 1);
        }

        //SkillData.Instance.SkillLevelUp
    }

    public static void DelSkill(int type)
    {
        var skillID = GetTestSkill(type);
        var skillData = SkillData.Instance.GetSkillInfo(skillID.ToString());
        if (skillData.SkillRecord.CostStep[0] == (int)MONEYTYPE.GOLD)
        {
            int costValue = GameDataValue.GetSkillLvUpGold(skillData.SkillRecord.CostStep[1], skillData.SkillLevel);
            if (!PlayerDataPack.Instance.DecGold(costValue))
                return;
        }
        else
        {
            int costValue = skillData.SkillRecord.CostStep[1];
            if (!PlayerDataPack.Instance.DecDiamond(costValue))
                return;
        }
        skillData.LevelUp();
    }

    public static int GetTestSkill(int type)
    {
        int skillID = 0;
        if (RoleData.SelectRole.Profession == PROFESSION.BOY_DEFENCE
            || RoleData.SelectRole.Profession == PROFESSION.BOY_DOUGE)
        {
            switch (type)
            {
                case 0:
                    skillID = 10001;
                    break;
                case 1:
                    skillID = 10002;
                    break;
                case 2:
                    skillID = 10003;
                    break;
                case 3:
                    skillID = 10004;
                    break;
            }
        }
        else
        {
            switch (type)
            {
                case 0:
                    skillID = 10005;
                    break;
                case 1:
                    skillID = 10006;
                    break;
                case 2:
                    skillID = 10007;
                    break;
                case 3:
                    skillID = 10008;
                    break;
            }
        }

        return skillID;
    }

    public static void DelRefresh()
    {
        var weaponItem = RoleData.SelectRole.GetEquipItem(EQUIP_SLOT.WEAPON);
        if (weaponItem == null)
            return;

        while (true)
        {
            bool isSucess = EquipRefresh.Instance.EquipRefreshMat(weaponItem, false);
            if (!isSucess)
                break;
        }
    }

    public static void DelGem()
    {
        var gemSetTab = TableReader.GemSet.GetRecord("120000");
        GemSuit.Instance.UseGemSet(gemSetTab);

        foreach (var gemLv in gemSetTab.Gems)
        {
            var gemInfo = GemData.Instance.GetGemInfo(gemLv.Id);
            GemData.Instance.GemLevelUp(gemInfo);
        }
    }

    #endregion

}
