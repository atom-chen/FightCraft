using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
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
    Wind
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
    
    [SerializeField]
    private MotionType _MotionType;
    public MotionType MotionType
    {
        get
        {
            return _MotionType;
        }
        set
        {
            _MotionType = value;
        }
    }

    private float _BaseMoveSpeed = 4.5f;
    private float _MoveSpeed = 4.5f;
    public float MoveSpeed
    {
        get
        {
            return _MoveSpeed;
        }
    }
    private float _MoveSpeedRate = 1.0f;
    public float MoveSpeedRate
    {
        get
        {
            return _MoveSpeedRate;
        }
    }
    public void AddMoveSpeedRate(float rate)
    {
        _BaseAttr.AddValue(RoleAttrEnum.MoveSpeed, (int)(rate * 10000));
        RefreshMoveSpeed();
    }
    private void RefreshMoveSpeed()
    {
        _MoveSpeedRate = _BaseAttr.GetValue(RoleAttrEnum.MoveSpeed) * 0.0001f + 1;
        _MoveSpeed = _BaseMoveSpeed * _MoveSpeedRate;
    }

    private float _BaseAttackSpeed = 1.0f;
    private float _AttackSpeed = 1.0f;
    public float AttackSpeed
    {
        get
        {
            return _AttackSpeed;
        }
    }
    public void AddAttackSpeedRate(float rate)
    {
        _BaseAttr.AddValue(RoleAttrEnum.AttackSpeed, (int)(rate * 10000));
        RefreshAttackSpeed();
    }
    private void RefreshAttackSpeed()
    {
        var AttackSpeedRate = _BaseAttr.GetValue(RoleAttrEnum.AttackSpeed) * 0.0001f + 1;
        _AttackSpeed = _BaseAttackSpeed * AttackSpeedRate;
    }

    private int _HP;
    public int HP
    {
        get
        {
            return _HP;
        }
    }

    public float HPPersent
    {
        get
        {
            return (_HP / (float)_BaseAttr.GetValue(RoleAttrEnum.HPMax));
        }
    }

    public void AddHPPersent(float persent)
    {
        int hpMax = _BaseAttr.GetValue(RoleAttrEnum.HPMax);
        _HP += (int)(hpMax * persent);
        _HP = Mathf.Clamp(_HP, 0, hpMax);
    }

    public bool IsLoweringHP
    {
        get
        {
            return HPPersent < 0.3f;
        }
    }

    public RoleData BaseRoleDate
    {
        get
        {
            return PlayerDataPack.Instance._SelectedRole;
        }
    }

    private RoleAttrStruct _BaseAttr;

    public Dictionary<FightAttr.FightAttrType, int> _ExAttrs = new Dictionary<FightAttr.FightAttrType, int>();

    #endregion

    #region skillAttr

    public class SkillAttr
    {
        public float SpeedAdd;
        public float DamageRateAdd;
        public float RangeAdd;
        public float RangeLengthAdd;
        public float BackRangeAdd;
        public float AccumulateTime;
        public float AccumulateDamageRate;
        public int ShadowWarriorCnt;
        public float ShadowWarriorDamageRate;
        public bool ExAttack;
        public List<string> ExBullets = new List<string>();
        public bool CanActAfterDebuff = false;

        public SkillAttr()
        { }

        public SkillAttr(SkillAttr otherSkillAttr)
        {
            SpeedAdd = otherSkillAttr.SpeedAdd;
            DamageRateAdd = otherSkillAttr.DamageRateAdd;
            RangeAdd = otherSkillAttr.RangeAdd;
            RangeLengthAdd = otherSkillAttr.RangeLengthAdd;
            BackRangeAdd = otherSkillAttr.BackRangeAdd;
            AccumulateTime = otherSkillAttr.AccumulateTime;
            AccumulateTime = otherSkillAttr.AccumulateTime;
            AccumulateDamageRate = otherSkillAttr.AccumulateDamageRate;
            ShadowWarriorCnt = otherSkillAttr.ShadowWarriorCnt;
            ShadowWarriorDamageRate = otherSkillAttr.ShadowWarriorDamageRate;
            CanActAfterDebuff = otherSkillAttr.CanActAfterDebuff;
            ExBullets = new List<string>(otherSkillAttr.ExBullets);
        }

    }

    private Dictionary<string, SkillAttr> _SkillAttrs = new Dictionary<string, SkillAttr>();

    private void InitSkillAttr()
    {
        var skillAttr = new SkillAttr();
        skillAttr.SpeedAdd = 0.5f;
        skillAttr.DamageRateAdd = 0.5f;
        //skillAttr.RangeAdd = 0.5f;
        skillAttr.RangeLengthAdd = 0.5f;
        skillAttr.BackRangeAdd = 0.5f;
        _SkillAttrs.Add("j", skillAttr);

        skillAttr = new SkillAttr();
        //skillAttr.SpeedAdd = 0.5f;
        skillAttr.DamageRateAdd = 0.5f;
        //skillAttr.RangeAdd = 0.5f;
        //skillAttr.RangeLengthAdd = 0.5f;
        //skillAttr.BackRangeAdd = 0.5f;
        skillAttr.ShadowWarriorCnt = 2;
        skillAttr.ShadowWarriorDamageRate = 0.3f;
        skillAttr.AccumulateTime = 0.5f;
        //skillAttr.ExBullets.Add("Bullet\\Emitter\\Element\\EleTargetBoomWind");
        skillAttr.CanActAfterDebuff = true;
        _SkillAttrs.Add("1", skillAttr);

        skillAttr = new SkillAttr();
        //skillAttr.SpeedAdd = 0.5f;
        skillAttr.DamageRateAdd = 0.5f;
        //skillAttr.RangeAdd = 0.5f;
        //skillAttr.RangeLengthAdd = 0.5f;
        skillAttr.BackRangeAdd = 0.5f;
        skillAttr.ShadowWarriorCnt = 2;
        skillAttr.ShadowWarriorDamageRate = 0.3f;
        skillAttr.AccumulateTime = 0.5f;
        //skillAttr.ExBullets.Add("Bullet\\Emitter\\Element\\EleTargetBoomWind");
        _SkillAttrs.Add("2", skillAttr);

        skillAttr = new SkillAttr();
        //skillAttr.SpeedAdd = 0.5f;
        skillAttr.DamageRateAdd = 0.5f;
        //skillAttr.RangeAdd = 0.5f;
        //skillAttr.RangeLengthAdd = 0.5f;
        skillAttr.BackRangeAdd = 0.5f;
        skillAttr.ShadowWarriorCnt = 2;
        skillAttr.ShadowWarriorDamageRate = 0.3f;
        skillAttr.AccumulateTime = 0.5f;
        //skillAttr.ExBullets.Add("Bullet\\Emitter\\Element\\EleLineBoomFire");
        //skillAttr.ExBullets.Add("Bullet\\Emitter\\Element\\EleTargetBoomWind");
        _SkillAttrs.Add("3", skillAttr);

        //var skillAttr = new SkillAttr();
        //_SkillAttrs.Add("j", skillAttr);
        //foreach (var skillItem in RoleData.SelectRole.SkillClassItems[Tables.SKILL_CLASS.NORMAL_ATTACK])
        //{
        //    var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillItem._SkillID);
        //    if (skillTab == null)
        //        continue;

        //    if (skillItem.SkillActureLevel <= 0)
        //        continue;

        //    switch (skillTab.SkillAttr)
        //    {
        //        case ATTR_EFFECT.DAMAGE:
        //            skillAttr.DamageRateAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SPEED:
        //            skillAttr.SpeedAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.RANGE:
        //            skillAttr.RangeAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //    }
        //}

        //skillAttr = new SkillAttr();
        //_SkillAttrs.Add("1", skillAttr);
        //foreach (var skillItem in RoleData.SelectRole.SkillClassItems[Tables.SKILL_CLASS.SKILL1])
        //{
        //    var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillItem._SkillID);
        //    if (skillTab == null)
        //        continue;

        //    if (skillItem.SkillActureLevel <= 0)
        //        continue;

        //    switch (skillTab.SkillAttr)
        //    {
        //        case ATTR_EFFECT.DAMAGE:
        //            skillAttr.DamageRateAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SPEED:
        //            skillAttr.SpeedAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.RANGE:
        //            skillAttr.RangeAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_ACCUMULATE:
        //            skillAttr.AccumulateTime = skillTab.EffectValue[1] * 0.0001f;
        //            skillAttr.AccumulateDamageRate = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_SHADOWARRIOR:
        //            skillAttr.ShadowWarriorCnt = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            skillAttr.ShadowWarriorDamageRate = skillTab.EffectValue[1] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_ACTBUFF:
        //            skillAttr.CanActAfterDebuff = true;
        //            break;
        //        case ATTR_EFFECT.SKILL_EXATTACK:
        //            skillAttr.ExAttack = true;
        //            break;
        //    }
        //}

        //skillAttr = new SkillAttr(skillAttr);
        //_SkillAttrs.Add("2", skillAttr);
        //foreach (var skillItem in RoleData.SelectRole.SkillClassItems[Tables.SKILL_CLASS.SKILL2])
        //{
        //    var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillItem._SkillID);
        //    if (skillTab == null)
        //        continue;

        //    if (skillItem.SkillActureLevel <= 0)
        //        continue;

        //    switch (skillTab.SkillAttr)
        //    {
        //        case ATTR_EFFECT.DAMAGE:
        //            skillAttr.DamageRateAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SPEED:
        //            skillAttr.SpeedAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.RANGE:
        //            skillAttr.RangeAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_ACCUMULATE:
        //            skillAttr.AccumulateTime = skillTab.EffectValue[1] * 0.0001f;
        //            skillAttr.AccumulateDamageRate = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_SHADOWARRIOR:
        //            skillAttr.ShadowWarriorCnt = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            skillAttr.ShadowWarriorDamageRate = skillTab.EffectValue[1] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_ACTBUFF:
        //            skillAttr.CanActAfterDebuff = true;
        //            break;
        //        case ATTR_EFFECT.SKILL_EXATTACK:
        //            skillAttr.ExAttack = true;
        //            break;
        //    }
        //}

        //skillAttr = new SkillAttr(skillAttr);
        //_SkillAttrs.Add("3", skillAttr);
        //foreach (var skillItem in RoleData.SelectRole.SkillClassItems[Tables.SKILL_CLASS.SKILL3])
        //{
        //    var skillTab = Tables.TableReader.SkillInfo.GetRecord(skillItem._SkillID);
        //    if (skillTab == null)
        //        continue;

        //    if (skillItem.SkillActureLevel <= 0)
        //        continue;

        //    switch (skillTab.SkillAttr)
        //    {
        //        case ATTR_EFFECT.DAMAGE:
        //            skillAttr.DamageRateAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SPEED:
        //            skillAttr.SpeedAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.RANGE:
        //            skillAttr.RangeAdd = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_ACCUMULATE:
        //            skillAttr.AccumulateTime = skillTab.EffectValue[1] * 0.0001f;
        //            skillAttr.AccumulateDamageRate = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_SHADOWARRIOR:
        //            skillAttr.ShadowWarriorCnt = skillTab.EffectValue[0] * skillItem.SkillActureLevel;
        //            skillAttr.ShadowWarriorDamageRate = skillTab.EffectValue[1] * skillItem.SkillActureLevel;
        //            break;
        //        case ATTR_EFFECT.SKILL_ACTBUFF:
        //            skillAttr.CanActAfterDebuff = true;
        //            break;
        //        case ATTR_EFFECT.SKILL_EXATTACK:
        //            skillAttr.ExAttack = true;
        //            break;
        //    }
        //}

    }

    public SkillAttr GetSkillAttr(string actInput)
    {
        if (_SkillAttrs.ContainsKey(actInput))
            return _SkillAttrs[actInput];

        return null;
    }

    #endregion

    #region 

    public void InitMainRoleAttr()
    {
        var roleData = PlayerDataPack.Instance._SelectedRole;
        if (roleData != null)
        {
            _BaseAttr = new RoleAttrStruct(roleData._BaseAttr);
            //_ExAttrs = new Dictionary<FightAttr.FightAttrType, int>(roleData._ExAttrs);
        }
        else
        {
            _BaseAttr = new RoleAttrStruct();
            _BaseAttr.SetValue(RoleAttrEnum.HPMax, 1000);
            _BaseAttr.SetValue(RoleAttrEnum.Attack, 10);
            _BaseAttr.SetValue(RoleAttrEnum.Defense, 10);
            _BaseMoveSpeed = 4.5f;
        }

        RefreshMoveSpeed();
        RefreshAttackSpeed();
        AddHPPersent(1);
        InitEvent();
        InitSkillAttr();
    }

    public void InitEnemyAttr(MonsterBaseRecord monsterBase)
    {
        _BaseMoveSpeed = 4;
        _Level = 1;
        _MonsterValue = 1;

        _BaseAttr = GetMonsterAttr(monsterBase, _Level, MotionType.Normal);

        RefreshMoveSpeed();
        RefreshAttackSpeed();
        AddHPPersent(1);
        InitEvent();
    }

    public void InitTestAttr()
    {
        _BaseMoveSpeed = 4;

        _BaseAttr = new RoleAttrStruct();
        _BaseAttr.SetValue(RoleAttrEnum.HPMax, 1000);
        _BaseAttr.SetValue(RoleAttrEnum.Attack, 10);
        _BaseAttr.SetValue(RoleAttrEnum.Defense, 10);

        _Level = 1;
        _MonsterValue = 1;

        RefreshMoveSpeed();
        RefreshAttackSpeed();
        AddHPPersent(1);
        InitEvent();
    }

    private RoleAttrStruct GetMonsterAttr(MonsterBaseRecord monsterBase, int level, MotionType monsterType)
    {
        var baseAttr = new RoleAttrStruct();
        int hpStep = 0;
        int attackStep = 0;
        int defenceStep = 0;
        if (level <= 5)
        {
            hpStep = 10;
            attackStep = 1;
            defenceStep = 1;
        }
        else if (level <= 20)
        {
            hpStep = 20;
            attackStep = 2;
            defenceStep = 2;
        }
        else if (level <= 50)
        {
            hpStep = 100;
            attackStep = 5;
            defenceStep = 5;
        }
        else if (level <= 100)
        {
            hpStep = 200;
            attackStep = 10;
            defenceStep = 10;
        }
        else
        {
            hpStep = 180;
            attackStep = 10;
            defenceStep = 8;
        }

        baseAttr.SetValue(RoleAttrEnum.HPMax, 100 + level * hpStep * monsterBase.BaseAttr[0]);
        baseAttr.SetValue(RoleAttrEnum.Attack, 10 + level * attackStep * monsterBase.BaseAttr[1]);
        baseAttr.SetValue(RoleAttrEnum.Defense, 5 + level * defenceStep * monsterBase.BaseAttr[2]);

        return baseAttr;
    }

    public int GetBaseAttr(RoleAttrEnum attr)
    {

        return _BaseAttr.GetValue(attr);
    }

    public void SetBaseAttr(RoleAttrEnum attr, int value)
    {
        _BaseAttr.SetValue(attr, value);

        if (attr == RoleAttrEnum.AttackSpeed)
        {
            RefreshAttackSpeed();
        }
        else if (attr == RoleAttrEnum.MoveSpeed)
        {
            RefreshMoveSpeed();
        }
    }

    #endregion

    #region calculate

    public class DamageClass
    {
        public int TotalDamageValue;
        public int AttachDamageValue;
        public int NormalDamageValue;
        public bool IsCriticle;
        public int FireDamage;
        public int IceDamage;
        public int LightingDamage;
        public int WindDamage;
    }

    private void CalculateDamage(RoleAttrManager sender, Hashtable resultHash)
    {
        //damage
        DamageClass damageClass = new DamageClass();
        CalculateNormalDamage(sender, resultHash, damageClass) ;

        //criticle
        CaculateCriticleHit(sender, resultHash, damageClass);

        //final
        CaculateFinalDamage(sender, resultHash, damageClass);

        //attach
        CaculateAttachDamage(sender, resultHash, damageClass);

        //skill
        var impactBase = (ImpactBase)resultHash["ImpactBase"];
        damageClass.TotalDamageValue = _MotionManager.BuffModifyDamage(damageClass.TotalDamageValue, impactBase);
        damageClass.AttachDamageValue = _MotionManager.BuffModifyDamage(damageClass.AttachDamageValue, impactBase);

        if (MotionType == MotionType.MainChar)
        {
            //GameUI.UIDamagePanel.ShowItem((Vector3)resultHash["DamagePos"], damageClass.TotalDamageValue, damageClass.AttachDamageValue, GameUI.ShowDamageType.Hurt, 1);
            DamagePanel.ShowItem((Vector3)resultHash["DamagePos"], damageClass.TotalDamageValue, damageClass.AttachDamageValue, ShowDamageType.Hurt, 1);
        }
        else if (damageClass.IsCriticle)
        {
            //GameUI.UIDamagePanel.ShowItem((Vector3)resultHash["DamagePos"], damageClass.TotalDamageValue, damageClass.AttachDamageValue, GameUI.ShowDamageType.Critical, 1);
            DamagePanel.ShowItem((Vector3)resultHash["DamagePos"], damageClass.TotalDamageValue, damageClass.AttachDamageValue, ShowDamageType.Critical, 1);
        }
        else
        {
            //GameUI.UIDamagePanel.ShowItem((Vector3)resultHash["DamagePos"], damageClass.TotalDamageValue, damageClass.AttachDamageValue, GameUI.ShowDamageType.Normal, 1);
            DamagePanel.ShowItem((Vector3)resultHash["DamagePos"], damageClass.TotalDamageValue, damageClass.AttachDamageValue, ShowDamageType.Normal, 1);
        }

        DamageHP(damageClass.TotalDamageValue + damageClass.AttachDamageValue);
    }

    private void CalculateNormalDamage(RoleAttrManager sender, Hashtable resultHash, DamageClass damageClass)
    {
        float damageRate = (float)resultHash["SkillDamageRate"];

        int attackValue = sender._BaseAttr.GetValue(RoleAttrEnum.Attack);
        int defenceValue = _BaseAttr.GetValue(RoleAttrEnum.Defense);
        damageClass.NormalDamageValue = Mathf.CeilToInt(attackValue * damageRate * (1 - defenceValue / (defenceValue + (Level + 1) * 200.0f)));

        int ignoreDAttack = sender._BaseAttr.GetValue(RoleAttrEnum.IgnoreDefenceAttack);
        int ignoreDaamge = Mathf.CeilToInt(ignoreDAttack * damageRate);
        damageClass.NormalDamageValue += ignoreDaamge;

        //element attack
        int fireAttack = sender._BaseAttr.GetValue(RoleAttrEnum.FireAttackAdd);
        int fireEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.FireEnhance);
        int fireResistan= _BaseAttr.GetValue(RoleAttrEnum.FireResistan);
        int fireDamage = Mathf.CeilToInt(fireAttack * damageRate * ((fireEnhance - fireResistan) / 250.0f));
        damageClass.FireDamage = fireDamage;

        int coldAttack = sender._BaseAttr.GetValue(RoleAttrEnum.ColdAttackAdd);
        int coldEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.ColdEnhance);
        int coldResistan = _BaseAttr.GetValue(RoleAttrEnum.ColdResistan);
        int coldDamage = Mathf.CeilToInt(coldAttack * damageRate * ((coldEnhance - coldResistan) / 250.0f));
        damageClass.IceDamage = coldDamage;

        int lightingAttack = sender._BaseAttr.GetValue(RoleAttrEnum.LightingAttackAdd);
        int lightingEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.LightingEnhance);
        int lightingResistan = _BaseAttr.GetValue(RoleAttrEnum.LightingResistan);
        int lightingDamage = Mathf.CeilToInt(lightingAttack * damageRate * ((lightingEnhance - lightingResistan) / 250.0f));
        damageClass.LightingDamage = lightingDamage;

        int windAttack = sender._BaseAttr.GetValue(RoleAttrEnum.WindAttackAdd);
        int windEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.WindEnhance);
        int windResistan = _BaseAttr.GetValue(RoleAttrEnum.WindResistan);
        int windDamage = Mathf.CeilToInt(windAttack * damageRate * ((windEnhance - windResistan) / 250.0f));
        damageClass.WindDamage = windDamage;
    }

    private bool CaculateCriticleHit(RoleAttrManager sender, Hashtable resultHash, DamageClass damageClass)
    {
        var criticleRate = _BaseAttr.GetValue(RoleAttrEnum.CriticalHitChance);
        var criticleDamage = _BaseAttr.GetValue(RoleAttrEnum.CriticalHitDamge);
        int randomRate = Random.Range(0, 10001);
        damageClass.IsCriticle = false;
        if (randomRate < criticleRate)
        {
            damageClass.IsCriticle = true;
            damageClass.NormalDamageValue = Mathf.CeilToInt(((criticleDamage * 0.0001f) + 1) * damageClass.NormalDamageValue);
            damageClass.FireDamage = Mathf.CeilToInt(((criticleDamage * 0.0001f) + 1) * damageClass.FireDamage);
            damageClass.IceDamage = Mathf.CeilToInt(((criticleDamage * 0.0001f) + 1) * damageClass.IceDamage);
            damageClass.LightingDamage = Mathf.CeilToInt(((criticleDamage * 0.0001f) + 1) * damageClass.LightingDamage);
            damageClass.WindDamage = Mathf.CeilToInt(((criticleDamage * 0.0001f) + 1) * damageClass.WindDamage);
            return true;
        }
        return false;
    }

    private void CaculateFinalDamage(RoleAttrManager sender, Hashtable resultHash, DamageClass damageClass)
    {
        damageClass.TotalDamageValue = damageClass.FireDamage + damageClass.IceDamage + damageClass.LightingDamage + damageClass.WindDamage + damageClass.NormalDamageValue;
        damageClass.TotalDamageValue -= _BaseAttr.GetValue(RoleAttrEnum.FinalDamageReduse);
        damageClass.TotalDamageValue = Mathf.Max(damageClass.TotalDamageValue, 0);
    }

    private void CaculateAttachDamage(RoleAttrManager sender, Hashtable resultHash, DamageClass damageClass)
    {
        if (!resultHash.ContainsKey("ImpactBase"))
            return;

        var impactBase = (ImpactBase)resultHash["ImpactBase"];
        if (impactBase == null || impactBase.SkillMotion == null)
            return;

        int fireAttack = 0;
        int coldAttack = 0;
        int lightingAttack = 0;
        int windAttack = 0;
        if (impactBase.SkillMotion._ActInput == "k1")
        {
            fireAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill1FireDamagePersent);
            coldAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill1ColdDamagePersent);
            lightingAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill1LightingDamagePersent);
            windAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill1WindDamagePersent);
        }
        else if (impactBase.SkillMotion._ActInput == "k2")
        {
            fireAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill2FireDamagePersent);
            coldAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill2ColdDamagePersent);
            lightingAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill2LightingDamagePersent);
            windAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill2WindDamagePersent);
        }
        else if (impactBase.SkillMotion._ActInput == "k3")
        {
            fireAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill3FireDamagePersent);
            coldAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill3ColdDamagePersent);
            lightingAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill3LightingDamagePersent);
            windAttack = sender._BaseAttr.GetExAttr(RoleAttrEnum.Skill3WindDamagePersent);
        }

        int fireEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.FireEnhance);
        int fireResistan = _BaseAttr.GetValue(RoleAttrEnum.FireResistan);
        int fireDamage = Mathf.CeilToInt(fireAttack * 0.0001f * damageClass.TotalDamageValue * ((fireEnhance - fireResistan) / 250.0f));
        damageClass.FireDamage += fireDamage;
        damageClass.AttachDamageValue += fireDamage;

        int coldEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.ColdEnhance);
        int coldResistan = _BaseAttr.GetValue(RoleAttrEnum.ColdResistan);
        int coldDamage = Mathf.CeilToInt(coldAttack * 0.0001f * damageClass.TotalDamageValue * ((coldEnhance - coldResistan) / 250.0f));
        damageClass.IceDamage += coldDamage;
        damageClass.AttachDamageValue += coldDamage;

        int lightingEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.LightingEnhance);
        int lightingResistan = _BaseAttr.GetValue(RoleAttrEnum.LightingResistan);
        int lightingDamage = Mathf.CeilToInt(lightingAttack * 0.0001f * damageClass.TotalDamageValue * ((lightingEnhance - lightingResistan) / 250.0f));
        damageClass.LightingDamage += lightingDamage;
        damageClass.AttachDamageValue += lightingDamage;

        int windEnhance = sender._BaseAttr.GetValue(RoleAttrEnum.WindEnhance);
        int windResistan = _BaseAttr.GetValue(RoleAttrEnum.WindResistan);
        int windDamage = Mathf.CeilToInt(windAttack * 0.0001f * damageClass.TotalDamageValue * ((windEnhance - windResistan) / 250.0f));
        damageClass.WindDamage += windDamage;
        damageClass.AttachDamageValue += windDamage;

        damageClass.AttachDamageValue = Mathf.Max(damageClass.AttachDamageValue, 0);
    }

    private void DamageHP(int damageValue)
    {
        int orgHP = _HP;
        _HP -= damageValue;

        if (orgHP > 0 && _HP <= 0)
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
        //_MotionManager.EventController.RegisteEvent(EVENT_TYPE.EVENT_FIGHT_ATTR_DAMAGE, DamageEvent);
    }

    public void SendDamageEvent(MotionManager targetMotion, float skillDamageRate, ImpactBase impactBase)
    {
        SendDamageEvent(targetMotion, skillDamageRate, impactBase, targetMotion.transform.position + new Vector3(0, 1.5f, 0));
    }

    public void SendDamageEvent(MotionManager targetMotion, float skillDamageRate, ImpactBase impactBase, Vector3 damagePosition)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SkillDamageRate", skillDamageRate);
        hash.Add("DamagePos", damagePosition);
        hash.Add("ImpactBase", impactBase);

        targetMotion.RoleAttrManager.CalculateDamage(this, hash);
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
