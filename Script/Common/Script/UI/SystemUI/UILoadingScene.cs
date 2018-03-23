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
        GameCore.Instance.UIManager.ShowUI("SystemUI/UILoadingScene", UILayer.TopUI, hash);
    }

    public static void ShowAsyn(StageInfoRecord stage)
    {
        Hashtable hash = new Hashtable();
        hash.Add("StageInfo", stage);
        GameCore.Instance.UIManager.ShowUI("SystemUI/UILoadingScene", UILayer.TopUI, hash);
    }

    #endregion

    #region 

    public RawImage _BG;
    public Slider _LoadProcess;

    private List<AsyncOperation> _LoadSceneOperation = new List<AsyncOperation>();
    private string _LoadSceneName;
    private StageInfoRecord _LoadStageInfo;

    private bool _IsFinishLoading;

    private const float MAX_PROCESS_TIME = 5.0f;
    private float _ProcessStartTime;

    private List<string> _LoadedSceneName = new List<string>();

    private LoadSceneStep _LoadSceneStep;
    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _LoadSceneName = "";
        _LoadStageInfo = null;
        _LoadSceneOperation.Clear();

        if (hash.ContainsKey("SceneName"))
        {
            _LoadSceneName = (string)hash["SceneName"];
            var asyncOpt = SceneManager.LoadSceneAsync(_LoadSceneName);
            _LoadSceneOperation.Add(asyncOpt);
        }
        else if (hash.ContainsKey("StageInfo"))
        {
            _LoadStageInfo = (StageInfoRecord)hash["StageInfo"];
            var asyncOpt = SceneManager.LoadSceneAsync(_LoadStageInfo.ScenePath[0]);
            _LoadedSceneName.Add(_LoadStageInfo.ScenePath[0]);
            _LoadSceneOperation.Add(asyncOpt);
        }
        

        ShowBG();
        _IsFinishLoading = false;
        _ProcessStartTime = Time.time;
        _LoadSceneStep = LoadSceneStep.LoadSceneRes;
    }

    public void FixedUpdate()
    {
        if (_IsFinishLoading)
            return;

        transform.SetSiblingIndex(10000);

        if (_LoadSceneStep == LoadSceneStep.LoadSceneRes)
        {
            UpdateLoadSceneRes();
        }
        else if (_LoadSceneStep == LoadSceneStep.InitScene)
        {
            if (_LoadStageInfo != null)
            {
                if (FightManager.Instance != null)
                {
                    _LoadProcess.value = FightManager.Instance.InitProcess;
                    if (FightManager.Instance.InitProcess == 1)
                        base.Destory();
                }
            }
        }
    }

    private void UpdateLoadSceneRes()
    {
        if (_LoadStageInfo == null)
        {
            float processValue = _LoadSceneOperation[0].progress;
            _LoadProcess.value = processValue;
            if (_LoadSceneOperation[0] == null || _LoadSceneOperation[0].isDone)
            {
                _IsFinishLoading = true;
                LogicManager.Instance.StartLogic();
                _LoadSceneStep = LoadSceneStep.InitScene;
                Resources.UnloadUnusedAssets();
                
                base.Destory();
            }
        }
        else
        {
            if (_LoadSceneOperation[0] == null || _LoadSceneOperation[0].isDone)
            {
                if (_LoadSceneOperation.Count == _LoadStageInfo.ValidScenePath.Count)
                {
                    float loadStageProcess = 0;
                    foreach (var asyncOpt in _LoadSceneOperation)
                    {
                        if (asyncOpt == null || asyncOpt.isDone)
                        {
                            loadStageProcess += 1;
                        }
                        else
                        {
                            loadStageProcess += asyncOpt.progress;
                        }
                    }
                    _LoadProcess.value = loadStageProcess / _LoadStageInfo.ValidScenePath.Count;
                    if (loadStageProcess == _LoadStageInfo.ValidScenePath.Count)
                    {
                        LogicManager.Instance.EnterFightFinish();
                        _LoadSceneStep = LoadSceneStep.InitScene;
                    }
                }
                else
                {
                    for (int i = 1; i < _LoadStageInfo.ValidScenePath.Count; ++i)
                    {
                        if (_LoadedSceneName.Contains(_LoadStageInfo.ScenePath[i]))
                        {
                            _LoadSceneOperation.Add(null);
                            continue;
                        }

                        var asyncOpt = SceneManager.LoadSceneAsync(_LoadStageInfo.ScenePath[i], LoadSceneMode.Additive);
                        _LoadSceneOperation.Add(asyncOpt);
                        _LoadedSceneName.Add(_LoadStageInfo.ScenePath[i]);
                    }
                }
            }
            else
            {
                float processValue = _LoadSceneOperation[0].progress;
                _LoadProcess.value = processValue * 0.5f;
            }
        }
    }


    #endregion

    #region 

    public void ShowBG()
    {
        if (_LoadSceneName == GameDefine.GAMELOGIC_SCENE_NAME)
        {
            _BG.texture = ResourceManager.Instance.GetTexture("Loading");
        }
        else
        {
            _BG.texture = ResourceManager.Instance.GetTexture("LoadFight");
        }
    }

    #endregion
}

