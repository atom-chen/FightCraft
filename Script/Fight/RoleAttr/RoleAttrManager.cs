using UnityEngine;
using System.Collections;

public enum RoleAttrEnum
{
    None,
    MoveSpeed,
    SkillSpeed,
    HitSpeed,
    MotionType,
    HP,
    HPMax,
    Attack,
    Defence,
    AttackFire,
    AttackCold,
    AttackLighting,
    AttackWind,
    DefenceFire,
    DefenceCold,
    DefenceLighting,
    DefenceWind,
    DamageFixedValue,
    DamageFixedRate,
    DamageReduceFixedValue,
    DamageReduceFixedRate,
}

public enum MotionType
{
    MainChar,
    Hero,
    Elite,
    Normal,
    
}

public class RoleAttrManager : MonoBehaviour
{
    #region baseAttr

    public MotionManager _MotionManager;

    [SerializeField]
    private float _MoveSpeed = 1;
    public float MoveSpeed
    {
        get
        {
            return _MoveSpeed;
        }
    }

    [SerializeField]
    private float _SkillSpeed = 1;
    public float SkillSpeed
    {
        get
        {
            return _SkillSpeed;
        }
    }

    [SerializeField]
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

    private int _HP;
    private int _HPMax;
    private int _Attack;
    private int _Defence;
    private int _AttackFire;
    private int _AttackCold;
    private int _AttackLighting;
    private int _AttackWind;
    private int _DefenceFire;
    private int _DefenceCold;
    private int _DefenceLighting;
    private int _DefenceWind;
    private int _DamageFixedValue;
    private int _DamageFixedRate;
    private int _DamageReduceFixedValue;
    private int _DamageReduceFixedRate;

    #endregion

    public void SetAttr(RoleAttrEnum attr, float value)
    {
        switch (attr)
        {
            case RoleAttrEnum.MoveSpeed:
                _MoveSpeed = value;
                break;
            case RoleAttrEnum.SkillSpeed:
                _SkillSpeed = value;
                break;
            case RoleAttrEnum.HitSpeed:
                _HitSpeed = value;
                break;
        }
    }

    public float GetAttrFloat(RoleAttrEnum attr)
    {
        switch (attr)
        {
            case RoleAttrEnum.MoveSpeed:
                return _MoveSpeed;
            case RoleAttrEnum.SkillSpeed:
                return _SkillSpeed;
            case RoleAttrEnum.HitSpeed:
                return _HitSpeed;
        }

        return -1;
    }

    

    public void InitAttrByLevel(int level)
    {
        _MotionManager.EventController.RegisteEvent(GameBase.EVENT_TYPE.EVENT_FIGHT_ATTR_DAMAGE, DamageEvent);

        _HP = 100;
        _HPMax = 100;
        _Attack = 10;
        _Defence = 1;
    }

    #region calculate

    private void CalculateDamage(RoleAttrManager sender, Hashtable resultHash)
    {
        float skillDamageRate = 1;
        if (resultHash.ContainsKey("SkillDamageRate"))
        {
            skillDamageRate = (float)resultHash["SkillDamageRate"];
        }

        //attack
        int attackValue = (int)(sender._Attack* skillDamageRate);

        //defence
        int defenceValue = _Defence;

        //damage
        int damage = attackValue - defenceValue;

        DamageHP(damage);
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
