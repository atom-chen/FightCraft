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
            idleEffect.name = effct.name;
        }

        return idleEffect;
    }

    public void RecvIldeEffect(EffectController effct)
    {
        string effectName = effct.name;
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
        if (_IdleModel.ContainsKey(monsterTab.ModelPath))
        {
            if (_IdleModel[monsterTab.ModelPath].Count > 0)
            {
                modelObj = _IdleModel[monsterTab.ModelPath].Pop();
            }
        }
        if (modelObj == null)
        {
            modelObj = ResourceManager.Instance.GetInstanceGameObject("Model/" + monsterTab.ModelPath);
            modelObj.name = monsterTab.ModelPath;
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
        string objName = objMotion.MonsterBase.ModelPath;
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

    public Dictionary<int, AudioClip> _CommonAudio;
    public int _HitSuperArmor = 0;

    private void InitAutio()
    {
        _CommonAudio = new Dictionary<int, AudioClip>();
        var audio1 = ResourceManager.Instance.GetAudioClip("common/HitArmor");
        _CommonAudio.Add(0, audio1);

        var audio2 = ResourceManager.Instance.GetAudioClip("common/HitSwordNone");
        _CommonAudio.Add(1,audio2);

        var audio3 = ResourceManager.Instance.GetAudioClip("common/HitSwordBody");
        _CommonAudio.Add(2,audio3);

        var audio4 = ResourceManager.Instance.GetAudioClip("common/HitSwordSlap");
        _CommonAudio.Add(3,audio4);

        var audio5 = ResourceManager.Instance.GetAudioClip("common/HitSwordSlap2");
        _CommonAudio.Add(4,audio5);

        var audio10 = ResourceManager.Instance.GetAudioClip("common/HitHwNone");
        _CommonAudio.Add(10, audio10);

        var audio11 = ResourceManager.Instance.GetAudioClip("common/HitHwBody");
        _CommonAudio.Add(11, audio11);

        var audio12 = ResourceManager.Instance.GetAudioClip("common/HwAtk");
        _CommonAudio.Add(12, audio12);

        var audioEle = ResourceManager.Instance.GetAudioClip("common/AtkFire");
        _CommonAudio.Add(100, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/AtkIce");
        _CommonAudio.Add(101, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/AtkLighting");
        _CommonAudio.Add(102, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/AtkStone");
        _CommonAudio.Add(103, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/AtkWind");
        _CommonAudio.Add(104, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/HitFire");
        _CommonAudio.Add(110, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/HitIce");
        _CommonAudio.Add(111, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/HitLighting");
        _CommonAudio.Add(112, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/HitStone");
        _CommonAudio.Add(113, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/HitWind");
        _CommonAudio.Add(114, audioEle);

        audioEle = ResourceManager.Instance.GetAudioClip("common/AtkFire2");
        _CommonAudio.Add(120, audioEle);

        var audioMon = ResourceManager.Instance.GetAudioClip("common/AtkBow");
        _CommonAudio.Add(200, audioMon);
    }


    #endregion

    #region fight scene obj

    public GameObject CreateFightSceneObj(string path)
    {
        var gameobj = ResourceManager.Instance.GetInstanceGameObject(path);
        if(gameobj != null)
        {
            gameobj.transform.SetParent(transform);
        }
        return gameobj;
    }

    #endregion

}
