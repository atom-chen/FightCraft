using UnityEngine;
using System.Collections;

public enum RoleAttrEnum
{
    None,
    MoveSpeed,
    SkillSpeed,
    HitSpeed
}

public class RoleAttrManager : MonoBehaviour
{
    #region baseAttr

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
}
