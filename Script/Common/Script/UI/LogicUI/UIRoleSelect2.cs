using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIRoleSelect2 : UIBase
{

    #region static funs

    public static void ShowAsyn()
    {
        Hashtable hash = new Hashtable();
        GameCore.Instance.UIManager.ShowUI("LogicUI/RoleAttr/UIRoleSelect2", UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public UICameraTexture[] _UICameraTexture;
    public GameObject[] _SelectedGO;
    public GameObject[] _DisableGO;
    public AnimationClip[] _Anims;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        for (int i = 0; i < PlayerDataPack._MAX_ROLE_CNT; ++i)
        {
            GetCharModel(i);
            ShowModel(i);

            if (i == (int)PlayerDataPack.Instance._SelectedRole.Profession)
            {
                _SelectedGO[i].SetActive(true);
            }
            else
            {
                _SelectedGO[i].SetActive(false);
            }
        }

        if (RoleData.SelectRole.TotalLevel >= GameDataValue._ROLE_OPEN_LEVEL)
        {
            for (int i = 0; i < _DisableGO.Length; ++i)
            {
                _DisableGO[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < _DisableGO.Length; ++i)
            {
                _DisableGO[i].SetActive(true);
            }
        }
    }

    #endregion

    #region interaction

    public void SelectRole(int idx)
    {
        for (int i = 0; i < _UICameraTexture.Length; ++i)
        {
            if (i == idx)
            {
                _ShowAnims[i].PlayAnim();
                _SelectedGO[i].SetActive(true);
            }
            else
            {
                _SelectedGO[i].SetActive(false);
            }
        }
    }

    public void ChangeRole(int idx)
    {
        PlayerDataPack.Instance.SelectRole(idx);
        Hide();
    }

    #endregion

    #region gameobj

    private List<UIModelAnim> _ShowAnims = new List<UIModelAnim>();

    public void ShowModel(int idx)
    {
        //for (int i = 0; i < _UICameraTexture.Length; ++i)
        //{
        //    if (i != idx)
        //    {
        //        _UICameraTexture[i].gameObject.SetActive(false);
        //    }
        //    else
        //    {
        //        _UICameraTexture[i].gameObject.SetActive(true);
        //    }
        //}

        _UICameraTexture[idx].gameObject.SetActive(true);
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
        var weaponTrans = model.transform.Find("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/righthand/rightweapon");
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
        modelAnim.InitAnim(anims, false);
        modelAnim.PlayAnim(1);

        _ShowAnims.Add(modelAnim);

        _UICameraTexture[idx].InitShowGO(model);
        return model;
    }

    #endregion
}

