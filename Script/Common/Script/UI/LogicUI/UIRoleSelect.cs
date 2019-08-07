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
        GameCore.Instance.UIManager.ShowUI(UIConfig.UIRoleSelect, UILayer.PopUI, hash);
    }

    #endregion

    #region 

    public Text _RoleLevel;
    public Text _AttrLevel;
    public UICameraTexture[] _UICameraTexture;
    public GameObject[] _GrayImgs;
    public AnimationClip[] _Anims;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        for (int i = 0; i < PlayerDataPack._MAX_ROLE_CNT; ++i)
        {
            InitCharModel(i);
        }

        SelectRole(_SelectRoleID);
    }

    private void OnDestroy()
    {
        //foreach (var animGO in _ShowAnims)
        //{
        //    GameObject.Destroy(animGO);
        //}
    }

    #endregion

    #region event

    private int _SelectRoleID = 0;

    public void SelectRole(int roleID)
    {
        _SelectRoleID = roleID;
        Debug.Log("_RoleList.Count:" + PlayerDataPack.Instance._RoleList.Count);
        _RoleLevel.text = PlayerDataPack.Instance._RoleList[_SelectRoleID].RoleLevel.ToString();
        _AttrLevel.text = PlayerDataPack.Instance._RoleList[_SelectRoleID].AttrLevel.ToString();

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

    private int _DefaultShowIdx = -1;

    public void ShowModel(int idx)
    {
        
        for (int i = 0; i < _UICameraTexture.Length; ++i)
        {
            if (i != idx)
            {
                _UICameraTexture[i].gameObject.SetActive(false);
                _GrayImgs[i].SetActive(true);
            }
            else
            {
                _UICameraTexture[i].gameObject.SetActive(true);
                if (_ShowAnims[i] != null)
                {
                    _ShowAnims[i].PlayAnim();
                    _DefaultShowIdx = -1;
                }
                else
                {
                    _DefaultShowIdx = idx;
                }
                _GrayImgs[i].SetActive(false);
            }
        }
    }

    public void InitCharModel(int idx)
    {
        _ShowAnims.Add(null);
        string modelName = PlayerDataPack.Instance._RoleList[idx].ModelName;
        string weaponName = PlayerDataPack.Instance._RoleList[idx].DefaultWeaponModel;

        StartCoroutine(ResourcePool.Instance.LoadCharModel(modelName, weaponName, (resName, resGO, hash)=>
        {
            var modelAnim = resGO.AddComponent<UIModelAnim>();
            List<AnimationClip> anims = new List<AnimationClip>();
            anims.Add(_Anims[idx * 2]);
            anims.Add(_Anims[idx * 2 + 1]);
            modelAnim.InitAnim(anims);

            _ShowAnims[idx] = (modelAnim);
            _UICameraTexture[idx].InitShowGO(resGO);
            if (_DefaultShowIdx == idx)
            {
                ShowModel(_DefaultShowIdx);
            }
        }, null));
        
    }

    #endregion
}

