using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleSelect : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/UIRoleSelect", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public Text _RoleLevel;
    public Text _AttrLevel;
    public UICameraTexture[] _UICameraTexture;
    public AnimationClip[] _Anims;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        for (int i = 0; i < PlayerDataPack._MAX_ROLE_CNT; ++i)
        {
            GetCharModel(i);
        }

        SelectRole(_SelectRoleID);
    }

    #endregion

    #region event

    private int _SelectRoleID = 0;

    public void SelectRole(int roleID)
    {
        _SelectRoleID = roleID;
        Debug.Log("_RoleList.Count:" + PlayerDataPack.Instance._RoleList.Count);
        _RoleLevel.text = PlayerDataPack.Instance._RoleList[_SelectRoleID]._RoleLevel.ToString();
        _AttrLevel.text = PlayerDataPack.Instance._RoleList[_SelectRoleID]._AttrLevel.ToString();

        //var roleData = GetCharModel(roleID);
        
        //_Desc.text = Tables.StrDictionary.GetFormatStr(1010 + roleID);
        ShowModel(roleID);
    }

    public void OnBtnOK()
    {
        LogicManager.Instance.StartLoadRole(_SelectRoleID);
        Hide();
    }
    #endregion

    #region gameobj

    private List<UIModelAnim> _ShowAnims = new List<UIModelAnim>();

    public void ShowModel(int idx)
    {
        for (int i = 0; i < _UICameraTexture.Length; ++i)
        {
            if (i != idx)
            {
                _UICameraTexture[i].gameObject.SetActive(false);
            }
            else
            {
                _UICameraTexture[i].gameObject.SetActive(true);
                _ShowAnims[i].PlayAnim();
            }
        }
    }

    public GameObject GetCharModel(int idx)
    {
        string mainBaseName = PlayerDataPack.Instance._RoleList[idx].MainBaseName;
        string modelName = PlayerDataPack.Instance._RoleList[idx].ModelName;
        string weaponName = PlayerDataPack.Instance._RoleList[idx].DefaultWeaponModel;

        var model = ResourceManager.Instance.GetInstanceGameObject("Model/" + modelName);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(Vector3.zero);

        var weapon = ResourceManager.Instance.GetInstanceGameObject("Model/" + weaponName);
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


        var modelAnim = model.AddComponent<UIModelAnim>();
        List<AnimationClip> anims = new List<AnimationClip>();
        anims.Add(_Anims[idx * 2]);
        anims.Add(_Anims[idx * 2 + 1]);
        modelAnim.InitAnim(anims);

        _ShowAnims.Add(modelAnim);

        _UICameraTexture[idx].InitShowGO(model);
        return model;
    }

    #endregion
}

