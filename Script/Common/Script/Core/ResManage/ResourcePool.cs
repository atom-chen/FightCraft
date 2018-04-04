using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePool : InstanceBase<ResourcePool>
{

    void Awake()
    {
        SetInstance(this);
        InitEffect();
        InitAutio();
    }

    void Destory()
    {
        SetInstance(null);
    }

    #region effect

    public List<EffectController> _CommonHitEffect;

    private Dictionary<string, Stack<EffectController>> _IdleEffects = new Dictionary<string, Stack<EffectController>>();

    private void InitEffect()
    {
        _CommonHitEffect = new List<EffectController>();
        var effect1 = ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Dead_A_Hit");
        _CommonHitEffect.Add(effect1.GetComponent<EffectController>());

        var effect2 = ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Dead_B_Hit");
        _CommonHitEffect.Add(effect2.GetComponent<EffectController>());

        var effect3 = ResourceManager.Instance.GetGameObject("Effect/Hit/Effect_Blade_Red");
        _CommonHitEffect.Add(effect3.GetComponent<EffectController>());

        var hitFire = ResourceManager.Instance.GetGameObject("Effect/Hit/Hit_Fire");
        _CommonHitEffect.Add(hitFire.GetComponent<EffectController>());

        var hitIce = ResourceManager.Instance.GetGameObject("Effect/Hit/Hit_Ice");
        _CommonHitEffect.Add(hitIce.GetComponent<EffectController>());

        var hitLight = ResourceManager.Instance.GetGameObject("Effect/Hit/Hit_Light");
        _CommonHitEffect.Add(hitLight.GetComponent<EffectController>());

        var hitWind = ResourceManager.Instance.GetGameObject("Effect/Hit/Hit_Wind");
        _CommonHitEffect.Add(hitWind.GetComponent<EffectController>());
    }

    public EffectController GetIdleEffect(EffectController effct)
    {

        EffectController idleEffect = null;
        if (_IdleEffects.ContainsKey(effct.name))
        {
            if (_IdleEffects[effct.name].Count > 0)
            {
                idleEffect = _IdleEffects[effct.name].Pop();
            }
        }

        if (idleEffect == null)
        {
            idleEffect = GameObject.Instantiate<EffectController>(effct);
        }

        return idleEffect;
    }

    public void RecvIldeEffect(EffectController effct)
    {
        string effectName = effct.name.Replace("(Clone)", "");
        if (!_IdleEffects.ContainsKey(effectName))
        {
            _IdleEffects.Add(effectName, new Stack<EffectController>());
        }
        effct.transform.SetParent(transform);
        _IdleEffects[effectName].Push(effct);
    }

    public bool IsEffectInRecvl(EffectController effct)
    {
        string effectName = effct.name.Replace("(Clone)", "");
        if (!_IdleEffects.ContainsKey(effectName))
        {
            return false;
        }
        return (_IdleEffects[effectName].Contains(effct));
    }


    public void ClearEffects()
    {
        _IdleEffects = new Dictionary<string, Stack<EffectController>>();
    }

    public void PlaySceneEffect(EffectController effct, Vector3 position, Vector3 rotation)
    {
        var effectInstance = GetIdleEffect(effct);
        effectInstance.transform.SetParent(transform);
        effectInstance.transform.position = position;
        effectInstance.transform.rotation = Quaternion.Euler(rotation);
        effectInstance.PlayEffect();
    }
    #endregion

    #region bullet

    private Dictionary<string, Stack<BulletBase>> _IdleBullets = new Dictionary<string, Stack<BulletBase>>();

    public BulletBase GetIdleBullet(BulletBase bullet)
    {
        BulletBase idleBullet = null;
        if (_IdleBullets.ContainsKey(bullet.name))
        {
            if (_IdleBullets[bullet.name].Count > 0)
            {
                idleBullet = _IdleBullets[bullet.name].Pop();
            }
        }

        if (idleBullet == null)
        {
            idleBullet = GameObject.Instantiate<BulletBase>(bullet);
        }

        return idleBullet;
    }

    public void RecvIldeBullet(BulletBase bullet)
    {
        string BulletName = bullet.name.Replace("(Clone)", "");
        if (!_IdleBullets.ContainsKey(BulletName))
        {
            _IdleBullets.Add(BulletName, new Stack<BulletBase>());
        }
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        _IdleBullets[BulletName].Push(bullet);
    }

    public void ClearBullets()
    {
        _IdleBullets = new Dictionary<string, Stack<BulletBase>>();
    }

    #endregion

    #region motion object

    private Dictionary<string, Stack<GameObject>> _IdleModel = new Dictionary<string, Stack<GameObject>>();

    private Dictionary<string, GameObject> _MonsterBasePrefab = new Dictionary<string, GameObject>();

    public void InitMonsterBase(List<string> monIds)
    {
        for (int i = 0; i < monIds.Count; ++i)
        {
            if (_MonsterBasePrefab.ContainsKey(monIds[i]))
                continue;

            var monsterTab = Tables.TableReader.MonsterBase.GetRecord(monIds[i]);
            var instance = ResourceManager.Instance.GetGameObject("ModelBase/" + monsterTab.MotionPath);
            instance.gameObject.SetActive(false);
            _MonsterBasePrefab.Add(monIds[i], instance);

            var monElite = Tables.TableReader.MonsterBase.GetGroupElite(monsterTab);
            if (_MonsterBasePrefab.ContainsKey(monElite.Id))
                continue;
            if (monElite != null)
            {
                var instanceElite = ResourceManager.Instance.GetGameObject("ModelBase/" + monElite.MotionPath);
                instanceElite.gameObject.SetActive(false);
                _MonsterBasePrefab.Add(monElite.Id, instanceElite);
            }
        }
    }

    public MotionManager GetIdleMotion(Tables.MonsterBaseRecord monsterTab)
    {
        if (monsterTab == null)
            return null;

        GameObject motion = null;
        
        if (_MonsterBasePrefab.ContainsKey(monsterTab.Id))
        {
            motion = GameObject.Instantiate(_MonsterBasePrefab[monsterTab.Id]);
        }
        else
        {
            motion = ResourceManager.Instance.GetInstanceGameObject("ModelBase/" + monsterTab.MotionPath);
        }
        var motionScript = motion.GetComponent<MotionManager>();
        var aiScript = motion.GetComponent<AI_Base>();
        aiScript.InitSkillGoes(motionScript);

        GameObject modelObj = null;
        if (_IdleBullets.ContainsKey(monsterTab.ModelPath))
        {
            if (_IdleModel[monsterTab.ModelPath].Count > 0)
            {
                modelObj = _IdleModel[monsterTab.ModelPath].Pop();
            }
        }
        if (modelObj == null)
        {
            modelObj = ResourceManager.Instance.GetInstanceGameObject("Model/" + monsterTab.ModelPath);
            var animation = modelObj.GetComponent<Animation>();
            if (animation == null)
            {
                modelObj.AddComponent<Animation>();
            }

            var animEvent = modelObj.GetComponent<AnimationEventManager>();
            if (animEvent == null)
            {
                modelObj.AddComponent<AnimationEventManager>();
            }

            //var sole = modelObj.transform.FindChild("center/sole");
            //var collider = sole.gameObject.AddComponent<CapsuleCollider>();
            //collider.radius = motionScript._ColliderInfo.x;
            //collider.height = motionScript._ColliderInfo.y;
            //collider.direction = 2;
            //collider.center = new Vector3(0, 0, collider.height * 0.5f);
            //collider.isTrigger = true;
            //var rigidbody = sole.gameObject.AddComponent<Rigidbody>();
            //rigidbody.isKinematic = true;

        }
        //modelObj.gameObject.SetActive(true);
        modelObj.transform.SetParent(motion.transform);
        modelObj.transform.localPosition = Vector3.zero;
        modelObj.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var shadow = ResourceManager.Instance.GetInstanceGameObject("Common/ShadowPlane");
        shadow.transform.SetParent(motion.transform);
        shadow.transform.localPosition = Vector3.zero;
        shadow.transform.localRotation = Quaternion.Euler(Vector3.zero);

        return motion.GetComponent<MotionManager>();
    }

    public void RecvIldeMotion(MotionManager objMotion)
    {
        var model = objMotion.AnimationEvent.gameObject;
        string objName = model.name.Replace("(Clone)", "");
        if (!_IdleModel.ContainsKey(objName))
        {
            _IdleModel.Add(objName, new Stack<GameObject>());
        }
        //objMotion.gameObject.SetActive(false);
        model.transform.SetParent(transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _IdleModel[objName].Push(model);

        GameObject.Destroy(objMotion.gameObject);
    }

    public void ClearObjs()
    {
        _IdleModel = new Dictionary<string, Stack<GameObject>>();
    }

    #endregion

    #region ui

    private Dictionary<string, Stack<GameObject>> _IdleUIItems = new Dictionary<string, Stack<GameObject>>();

    public T GetIdleUIItem<T>(GameObject itemPrefab, Transform parentTrans = null)
    {
        GameObject idleItem = null;
        if (_IdleUIItems.ContainsKey(itemPrefab.name))
        {
            if (_IdleUIItems[itemPrefab.name].Count > 0)
            {
                idleItem = _IdleUIItems[itemPrefab.name].Pop();
            }
        }

        if (idleItem == null)
        {
            idleItem = GameObject.Instantiate<GameObject>(itemPrefab);
        }
        idleItem.gameObject.SetActive(true);
        if (parentTrans != null)
        {
            idleItem.transform.SetParent(parentTrans);
            idleItem.transform.localPosition = Vector3.zero;
            idleItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
            idleItem.transform.localScale = Vector3.one;
        }
        return idleItem.GetComponent<T>();
    }

    public void RecvIldeUIItem(GameObject itemBase)
    {
        string itemName = itemBase.name.Replace("(Clone)", "");
        if (!_IdleUIItems.ContainsKey(itemName))
        {
            _IdleUIItems.Add(itemName, new Stack<GameObject>());
        }
        itemBase.gameObject.SetActive(false);
        itemBase.transform.SetParent(transform);
        if (!_IdleUIItems[itemName].Contains(itemBase))
        {
            _IdleUIItems[itemName].Push(itemBase);
        }
    }

    public void ClearUIItems()
    {
        _IdleUIItems = new Dictionary<string, Stack<GameObject>>();
    }

    #endregion

    #region audio

    public List<AudioClip> _CommonAudio;
    public int _HitSuperArmor = 4;

    private void InitAutio()
    {
        _CommonAudio = new List<AudioClip>();
        var audio1 = ResourceManager.Instance.GetAudioClip("common/hit-1");
        _CommonAudio.Add(audio1);

        var audio2 = ResourceManager.Instance.GetAudioClip("common/hit-2");
        _CommonAudio.Add(audio2);

        var audio3 = ResourceManager.Instance.GetAudioClip("common/hit-3");
        _CommonAudio.Add(audio3);

        var audio4 = ResourceManager.Instance.GetAudioClip("common/hit-4");
        _CommonAudio.Add(audio4);

        var audio5 = ResourceManager.Instance.GetAudioClip("common/hit-5");
        _CommonAudio.Add(audio5);
        
    }


    #endregion

}
