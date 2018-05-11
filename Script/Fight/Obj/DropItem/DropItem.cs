using UnityEngine;
using System.Collections;
 

public class DropItem : MonoBehaviour
{
    private const float _DropTime = 0.3f;
    private const float _DropHeightSpeed = 5.0f;

    public string _DropName;

    private Vector3 _DropSpeed;
    private float _MoveTime;

    private Collider _Collider;
    public Collider Collider
    {
        get
        {
            if (_Collider == null)
            {
                _Collider = gameObject.GetComponent<Collider>();
            }
            return _Collider;
        }
    }

    private DropItemData _DropData;
    public DropItemData DropData
    {
        get
        {
            return _DropData;
        }
    }

    void FixedUpdate()
    {
        if(_MoveTime < _DropTime)
            UpdateDropPos();

        if (DropData._ItemEquip == null)
            UpdateAutoPick();
    }

    public void InitDrop(DropItemData dropData)
    {
        _DropData = dropData;
        if (dropData._DropGold > 0)
        {
            InitGoldModel(dropData._DropGold);
            _DropName = dropData._DropGold.ToString();
            Collider.enabled = false;
        }
        else if (dropData._ItemEquip != null)
        {
            InitItemModel(dropData._ItemEquip);
            _DropName = dropData._ItemEquip.GetEquipNameWithColor();
            Collider.enabled = false;
        }
        else if (dropData._ItemBase != null)
        {
            InitItemModel(dropData._ItemBase);
            _DropName = dropData._ItemBase.CommonItemRecord.Name;
            Collider.enabled = false;
        }
        else
        {
            Debug.Log("Drop Empty");
        }

        transform.position = dropData._MonsterPos;
        _DropSpeed = (dropData._DropPos - dropData._MonsterPos) / _DropTime;
        _MoveTime = 0;
        _StartTime = Time.time;

        UIDropNamePanel.ShowDropItem(this);
    }

    private void InitGoldModel(int gold)
    {
        var obj = ResourceManager.Instance.GetInstanceGameObject("Drop/Drop_055/Drop_055");
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        int randomAngle = Random.Range(0, 360);
        obj.transform.localRotation = Quaternion.Euler(0, randomAngle, 0);
    }

    private void InitItemModel(ItemBase itembase)
    {
        var obj = ResourceManager.Instance.GetInstanceGameObject(itembase.CommonItemRecord.DropItem);
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;

        int randomAngle = Random.Range(0, 360);
        obj.transform.localRotation = Quaternion.Euler(0, randomAngle, 0);
    }

    private void UpdateDropPos()
    {
        _MoveTime += Time.fixedDeltaTime;
        transform.position += _DropSpeed * Time.fixedDeltaTime;
        if (_MoveTime < _DropTime * 0.5f)
        {
            transform.position += new Vector3(0, _DropHeightSpeed * Time.fixedDeltaTime, 0);
        }
        else if(_MoveTime < _DropTime)
        {
            transform.position -= new Vector3(0, _DropHeightSpeed * Time.fixedDeltaTime, 0);
        }
    }

    #region 

    void OnTriggerEnter(Collider other)
    {
        PickDropItem();
    }

    public void PickDropItem()
    {
        MonsterDrop.PickItem(_DropData);
        PlayEffect();
        ResourceManager.Instance.DestoryObj(gameObject);
    }

    #endregion

    #region update auto pick

    private float _AutoPickTime = 2.2f;

    private float _StartTime = 0;
    private void UpdateAutoPick()
    {
        if (Time.time - _StartTime < _AutoPickTime)
            return;

        PickDropItem();
    }

    #endregion

    #region effect

    public EffectController _EffectPrefab;

    private EffectController _EffectInstance;
    public void PlayEffect()
    {
        _EffectInstance = ResourcePool.Instance.GetIdleEffect(_EffectPrefab);
        _EffectInstance.transform.position = transform.position;
        _EffectInstance.gameObject.SetActive(true);
    }

    #endregion
}
