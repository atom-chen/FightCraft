using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.SceneManagement;
using Tables;

public enum LoadSceneStep
{
    StartLoad,
    LoadSceneRes,
    InitScene,
}

public class UILoadingScene : UIBase
{
    #region static funs

    public static void ShowAsyn(string sceneName)
    {
        Hashtable hash = new Hashtable();
        hash.Add("SceneName", sceneName);
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingScene, UILayer.TopUI, hash);
    }

    public static void ShowEnterFightAsyn()
    {
        Hashtable hash = new Hashtable();
        
        GameCore.Instance.UIManager.ShowUI(UIConfig.UILoadingScene, UILayer.TopUI, hash);
    }

    #endregion

    #region 

    public Image _BG;
    public Text _NameText;
    public Text _Tips;
    public Slider _LoadProcess;

    private string _LoadingSceneName;
    private bool _IsEnterFight;
    private float _StartTime;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);
        
        ShowBG();

        _StartTime = Time.time;
        if (hash.ContainsKey("SceneName"))
        {
            _IsEnterFight = false;
            _LoadingSceneName = (string)hash["SceneName"];
            //StartCoroutine(InitializeLevelAsync(_LoadSceneName, true, LoadSceneResFinish, hash));
            StartCoroutine( ResourceManager.Instance.LoadLevelAsync(_LoadingSceneName, false));
        }
        else
        {
            _IsEnterFight = true;
            LogicManager.Instance.InitFightScene();
        }
    }
    
    public void FixedUpdate()
    {
        if (_IsEnterFight)
        {
            _LoadProcess.value = FightManager.Instance.InitProcess;
            if (FightManager.Instance.InitProcess == 1)
            {
                LogicManager.Instance.EnterFightFinish();
                base.Destory();
            }
        }
        else
        {
            _LoadProcess.value = (Time.time - _StartTime) * 0.66f;
            if (_LoadProcess.value >=1 && SceneManager.GetActiveScene().name == _LoadingSceneName)
            {
                LogicManager.Instance.StartLogic();
                base.Destory();
            }
        }
    }
    
    #endregion

    #region 

    public void ShowBG()
    {
        var imageTips = TableReader.LoadingTips.GetRandomImageTips();
        ResourceManager.Instance.SetImage(_BG, imageTips.ImagePath);
        _NameText.text = StrDictionary.GetFormatStr(imageTips.ImageName);
        _Tips.text = StrDictionary.GetFormatStr(imageTips.TipsStr);
    }

    #endregion
}

