using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GameLogic;
using Tables;

public enum MotionType
{
    MainChar,
    Hero,
    Elite,
    Normal,
    
}

public enum ElementType
{
    None,
    Fire,
    Cold,
    Lighting,
    Wind,
}

public enum BaseAttr
{
    MOVE_SPEED,
    SKILL_SPEED,
    HIT_SPEED,
    HP,
    HP_MAX,
    ATTACK,
    DEFENCE,
}

public class RoleAttrManager : MonoBehaviour
{
    #region baseAttr

    public MotionManager _MotionManager;

    private int _Level = 0;
    public int Level
    {
        get
        {
            return _Level;
        }
    }

    private int _MonsterValue;
    public int MonsterValue
    {
        get
        {
            return _MonsterValue;
        }
    }

    private float _MoveSpeed = 1;
    public float MoveSpeed
    {
        get
        {
            return _MoveSpeed;
        }
    }

    private float _SkillSpeed = 1;
    public float SkillSpeed
    {
        get
        {
            return _SkillSpeed;
        }
    }

    private float _HitSpeed = 1;
    public float HitSpeed
    {
        get
        {
            return _HitSpeed;
        }
    }

    [SerializeField]
    private MotionType _MotionType;
    public MotionType MotionType
    {
        get
        {
            return _MotionType;
        }
    }

    //base
    private int _HP;
    private int _HPMax;
    private int _Attack;
    private int _Defence;

    public float HPPersent
    {
        get
        {
            return (_HP / (float)_HPMax);
        }
    }

    public bool IsLoweringHP
    {
        get
        {
            return HPPersent < 0.3f;
        }
    }

    public Dictionary<FightAttr.FightAttrType, int> _ExAttrs = new Dictionary<FightAttr.FightAttrType, int>();
    #endregion

    #region 

    public void InitMainRoleAttr()
    {
        var roleData = PlayerDataPack.Instance._SelectedRole;
        if (roleData != null)
        {
            _MoveSpeed = roleData.GetBaseMoveSpeed();
            _SkillSpeed = roleData.GetBaseAttackSpeed();
            _HPMax = roleData.GetBaseHP();
            _Attack = roleData.GetBaseAttack();
            _Defence = roleData.GetBaseDefence();
            _Level = roleData._Level;

            foreach (var equip in roleData._EquipList)
            {
                if (!equip.IsVolid())
                    continue;

                _HPMax += equip.BaseHP;
                _Attack += equip.BaseAttack;
                _Defence += equip.BaseDefence;
            }
            _HP = _HPMax;
            _ExAttrs = new Dictionary<FightAttr.FightAttrType, int>(roleData._ExAttrs);
        }
        else
        {
            _MoveSpeed = 1;
            _SkillSpeed = 1;
            _HPMax = 1000;
            _HP = 1000;
            _Attack = 5;
            _Defence = 5;
            _Level = 1;
        }

        InitEvent();
    }

    public void InitEnemyAttr(MonsterBaseRecord monsterBase)
    {
        _MoveSpeed = 1;
        _SkillSpeed = 1;
        _HPMax = 100;
        _HP = 100;
        _Attack = 5;
        _Defence = 1;
        _Level = 1;
        _MonsterValue = 1;

        InitEvent();
    }

    public float GetBaseAttr(BaseAttr attr)
    {
        switch (attr)
        {
            case BaseAttr.MOVE_SPEED:
                return MoveSpeed;
            case BaseAttr.SKILL_SPEED:
                return SkillSpeed;
            case BaseAttr.HP:
                return _HP;
            case BaseAttr.HP_MAX:
                return _HPMax;
            case BaseAttr.ATTACK:
                return _Attack;
            case BaseAttr.DEFENCE:
                return _Defence;
        }

        return 0;
    }

    public void SetBaseAttr(BaseAttr attr, float value)
    {
        switch (attr)
        {
            case BaseAttr.MOVE_SPEED:
                _MoveSpeed = value;
                break;
            case BaseAttr.SKILL_SPEED:
                _SkillSpeed = value;
                break;
            case BaseAttr.HP:
                _HP = (int)value;
                break;
            case BaseAttr.HP_MAX:
                _HPMax = (int)value;
                break;
            case BaseAttr.ATTACK:
                _Attack = (int)value;
                break;
            case BaseAttr.DEFENCE:
                _Defence = (int)value;
                break;
        }
    }

    #endregion

    #region calculate

    private void CalculateDamage(RoleAttrManager sender, Hashtable resultHash)
    {
        //attack
        int attackValue = CalculateAttack(sender, resultHash);

        //defence
        int defenceValue = CalculateDefence(sender, resultHash);

        //damage
        int damage = (int)(attackValue * (1 - defenceValue / (defenceValue + 10000.0f)));

        int finalDamage = CalculateDamage(sender, resultHash, damage);

        
        DamageHP(finalDamage);
    }

    private int CalculateAttack(RoleAttrManager sender, Hashtable resultHash)
    {
        float skillDamageRate = 1;
        if (resultHash.ContainsKey("SkillDamageRate"))
        {
            skillDamageRate = (float)resultHash["SkillDamageRate"];
        }
        int attackValue = (int)(sender._Attack * skillDamageRate);

        return attackValue;
    }

    private int CalculateDefence(RoleAttrManager sender, Hashtable resultHash)
    {
        //base
        int defenceValue = _Defence;

        //to specil enemy
        if (_ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DEFENCE_TO_BOSS) && sender.MotionType == MotionType.Hero)
        {
            defenceValue += _ExAttrs[FightAttr.FightAttrType.INCREASE_DEFENCE_TO_BOSS];
        }
        else if (_ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DEFENCE_TO_ELITE) && sender.MotionType == MotionType.Elite)
        {
            defenceValue += _ExAttrs[FightAttr.FightAttrType.INCREASE_DEFENCE_TO_ELITE];
        }

        //lowering hp
        if (_ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DEFENCE_WHILE_LOWERING_HP) && IsLoweringHP)
        {
            defenceValue += _ExAttrs[FightAttr.FightAttrType.INCREASE_DEFENCE_WHILE_LOWERING_HP];
        }
        if (_ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DEFENCE_WHILE_LOWERING_HP_PERSENT) && IsLoweringHP)
        {
            defenceValue += (int)(_ExAttrs[FightAttr.FightAttrType.INCREASE_DEFENCE_WHILE_LOWERING_HP_PERSENT] * 0.01f * _Defence);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.IGNORE_TARGET_COLD_DEFENCE))
        {
            defenceValue -= (int)(sender._ExAttrs[FightAttr.FightAttrType.IGNORE_TARGET_COLD_DEFENCE] * 0.01f * _Defence);
        }

        return defenceValue;
    }

    private int CalculateDamage(RoleAttrManager sender, Hashtable resultHash, int baseDamage)
    {
        int finalDamage = baseDamage;

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.ENHANCE_DAMAGE))
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.ENHANCE_DAMAGE];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.ENHANCE_DAMAGE_PERSENT))
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.ENHANCE_DAMAGE_PERSENT] * 0.01f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_TO_BOSS) && MotionType == MotionType.Hero)
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_TO_BOSS];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_TO_BOSS_PERSENT) && MotionType == MotionType.Hero)
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_TO_BOSS_PERSENT] * 0.01f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_SINGLE) && FightManager.Instance.SceneEnemyCnt == 1)
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_SINGLE];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_SINGLE_PERSENT) && FightManager.Instance.SceneEnemyCnt == 1)
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_SINGLE_PERSENT] * 0.01f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_MORE_THAN_3) && FightManager.Instance.SceneEnemyCnt > 2)
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_MORE_THAN_3];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_MORE_THAN_3_PERSENT) && FightManager.Instance.SceneEnemyCnt > 2)
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_WHEN_ENEMY_MORE_THAN_3_PERSENT] * 0.01f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP))
        {
            if(FightManager.Instance.Combo < 10)
                finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP];
            else if (FightManager.Instance.Combo < 20)
                finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP] * 1.5f);
            else if (FightManager.Instance.Combo >= 20)
                finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP] * 2.0f);
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP_PERSENT))
        {
            if (FightManager.Instance.Combo < 10)
                finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP_PERSENT] * 0.01f * baseDamage);
            else if (FightManager.Instance.Combo < 20)
                finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP_PERSENT] * 0.015f * baseDamage);
            else if (FightManager.Instance.Combo >= 20)
                finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_AS_COMBOS_GO_UP_PERSENT] * 0.02f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_WHILE_LOWERING_HP) && sender.IsLoweringHP)
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_WHILE_LOWERING_HP];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_WHILE_LOWERING_HP_PERSENT) && sender.IsLoweringHP)
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_WHILE_LOWERING_HP_PERSENT] * 0.01f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_MORE_THEN_60) && HPPersent > 0.6f)
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_MORE_THEN_60];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_MORE_THEN_60_PERSENT) && HPPersent > 0.6f)
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_MORE_THEN_60_PERSENT] * 0.01f * baseDamage);
        }

        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_LESS_THEN_40) && HPPersent < 0.4f)
        {
            finalDamage += sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_LESS_THEN_40];
        }
        if (sender._ExAttrs.ContainsKey(FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_LESS_THEN_40_PERSENT) && HPPersent < 0.4f)
        {
            finalDamage += (int)(sender._ExAttrs[FightAttr.FightAttrType.INCREASE_DAMAGE_TO_TARGET_HP_LESS_THEN_40_PERSENT] * 0.01f * baseDamage);
        }

        if (_ExAttrs.ContainsKey(FightAttr.FightAttrType.REDUSE_DAMAGE))
        {
            finalDamage -= _ExAttrs[FightAttr.FightAttrType.REDUSE_DAMAGE];
        }
        if (_ExAttrs.ContainsKey(FightAttr.FightAttrType.REDUSE_DAMAGE_PERSENT))
        {
            finalDamage -= (int)(_ExAttrs[FightAttr.FightAttrType.REDUSE_DAMAGE_PERSENT] * 0.01f * baseDamage);
        }

        return finalDamage;
    }

    private void DamageHP(int damageValue)
    {
        _HP -= damageValue;
        if (_HP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _MotionManager.MotionDie();
    }

    #endregion

    #region event

    private void InitEvent()
    {
        _MotionManager.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_FIGHT_ATTR_DAMAGE, DamageEvent);
    }

    public void SendDamageEvent(MotionManager targetMotion, float skillDamageRate)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SkillDamageRate", skillDamageRate);

        targetMotion.EventController.PushEvent(GameBase.EVENT_TYPE.EVENT_FIGHT_ATTR_DAMAGE, this, hash);
    }

    public void DamageEvent(object go, Hashtable eventArgs)
    {
        RoleAttrManager sender = go as RoleAttrManager;
        if (sender == null)
            return;

        CalculateDamage(sender, eventArgs);
    }

    #endregion
}
