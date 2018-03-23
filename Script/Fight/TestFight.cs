using System.Collections;
using System.Collections.Generic;
using Tables;
using UnityEngine;

public class TestFight : MonoBehaviour
{	
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
    private int _RandomSkillIdx = 0;
    private ObjMotionSkillAttack _NormalAttack;

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
        if (distance > _CloseRange)
        {
            FightManager.Instance.MainChatMotion.StartMoveState(destPos);
            ReleaseSkill();
        }
        else
        {
            FightManager.Instance.MainChatMotion.StopMoveState();
            
            StartSkill();
        }
    }

    private bool StartSkill()
    {
        if (_EnemyMotion == null)
            return false;

        if (_EnemyMotion.IsMotionDie)
            return false;

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
            if (_NormalAttack.CurStep > 0 && _NormalAttack.CurStep == _RandomSkillIdx)
            {
                //if (_NormalAttack.CanNextInput)
                {
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

        foreach (var dropItem in UIDropNamePanel.Instance._DropItems)
        {
            dropItem.OnItemClick();
        }
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
                if (itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
                {
                    int levelDelta = itemEquip.EquipLevel - equipedItem.EquipLevel;
                    if (levelDelta > 10)
                    {
                        changeEquip = true;
                    }
                }
                else
                {
                    if (itemEquip.EquipValue > equipedItem.EquipValue)
                        changeEquip = true;
                }
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
        foreach (var itemEquip in BackBagPack.Instance.PageEquips)
        {
            if (itemEquip.EquipQuality == ITEM_QUALITY.ORIGIN)
            {
                if (LegendaryData.Instance.IsCollect(itemEquip))
                {
                    LegendaryData.Instance.PutInEquip(itemEquip);
                    continue;
                }
            }

            if (itemEquip.EquipQuality != ITEM_QUALITY.WHITE)
            {
                EquipRefresh.Instance.DestoryMatCnt(itemEquip, false);
                continue;
            }

            ShopData.Instance.SellItem(itemEquip, false);
        }
    }

    #endregion
}
