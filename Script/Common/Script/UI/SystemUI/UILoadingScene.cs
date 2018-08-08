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

    private int _LoadResCnt = 0;
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
            //StartCoroutine(InitializeLevelAsync(_LoadSceneName, true, LoadSceneResFinish, hash));
            var asyncOpt = SceneManager.LoadSceneAsync(_LoadSceneName);
            _LoadSceneOperation.Add(asyncOpt);
            _LoadSceneStep = LoadSceneStep.LoadSceneRes;
        }
        else if (hash.ContainsKey("StageInfo"))
        {
            _LoadStageInfo = (StageInfoRecord)hash["StageInfo"];
            //LoadStageLevel(_LoadStageInfo, hash);
            //StartCoroutine(InitializeLevelAsync(_LoadStageInfo.ScenePath[0], true, LoadSceneResFinish, hash));
            List<string> validScenes = null;
            if (_LoadStageInfo.FightLogicPath == "FightLogic_Random")
            {
                int sceneType = _LoadStageInfo.ExParam[0];
                validScenes = new List<string>();
                validScenes.Add(FightSceneLogicRandomArea.GetRandomScene(sceneType));
            }
            else
            {
                validScenes = _LoadStageInfo.GetValidScenePath();
            }
            var asyncOpt = SceneManager.LoadSceneAsync(validScenes[0]);
            _LoadedSceneName.Add(validScenes[0]);
            _LoadSceneOperation.Add(asyncOpt);
            _LoadSceneStep = LoadSceneStep.LoadSceneRes;
        }

        ShowBG();
        _IsFinishLoading = false;
        _ProcessStartTime = Time.time;
        
    }

    public void LoadStageLevel(StageInfoRecord stageRecord, Hashtable hash)
    {
        _LoadResCnt = 0;
        for (int i = 0; i < stageRecord.ValidScenePath.Count; ++i)
        {
            StartCoroutine(InitializeLevelAsync(stageRecord.ValidScenePath[i], true, LoadSceneResFinish, hash));
        }
    }

    public void LoadSceneResFinish(string levelName, Hashtable hash)
    {
        ++_LoadResCnt;
        if (_LoadResCnt == _LoadStageInfo.ValidScenePath.Count)
        {
            var asyncOpt = SceneManager.LoadSceneAsync(_LoadStageInfo.ScenePath[0], LoadSceneMode.Single);
            _LoadSceneOperation.Add(asyncOpt);
            _LoadedSceneName.Add(_LoadStageInfo.ScenePath[0]);

            for (int i = 1; i < _LoadStageInfo.ValidScenePath.Count; ++i)
            {
                if (_LoadedSceneName.Contains(_LoadStageInfo.ScenePath[i]))
                {
                    _LoadSceneOperation.Add(null);
                    continue;
                }

                asyncOpt = SceneManager.LoadSceneAsync(_LoadStageInfo.ScenePath[i], LoadSceneMode.Additive);
                _LoadSceneOperation.Add(asyncOpt);
                _LoadedSceneName.Add(_LoadStageInfo.ScenePath[i]);
            }
            _LoadSceneStep = LoadSceneStep.LoadSceneRes;
        }
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
                if (_LoadSceneOperation.Count >= _LoadStageInfo.ValidScenePath.Count)
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
                    if (loadStageProcess >= _LoadStageInfo.ValidScenePath.Count)
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

    public delegate void LoadSceneFinish(string sceneName, Hashtable param);

    public AsyncOperation _LoadingSceneAsyncOperation = null;

    protected IEnumerator InitializeLevelAsync(string levelName, bool isAdditive, LoadSceneFinish delFinish, Hashtable param)
    {
        // This is simply to get the elapsed time for this phase of AssetLoading.
        float startTime = Time.realtimeSinceStartup;
        string sceneAssetBundle = "scene/" + levelName;
        // Load level from assetBundle.
        AssetBundles.AssetBundleLoadOperation request = AssetBundles.AssetBundleManager.LoadLevelAsync(sceneAssetBundle.ToLower() + ".common", levelName, isAdditive);
        if (request == null)
            yield break;

        if (request is AssetBundles.AssetBundleLoadLevelOperation)
        {
            _LoadingSceneAsyncOperation = (request as AssetBundles.AssetBundleLoadLevelOperation).m_Request;
        }
#if UNITY_EDITOR
        else if (request is AssetBundles.AssetBundleLoadLevelSimulationOperation)
        {
            _LoadingSceneAsyncOperation = (request as AssetBundles.AssetBundleLoadLevelSimulationOperation).m_Operation;
        }
#endif

        yield return StartCoroutine(request);

        // Calculate and display the elapsed time.
        float elapsedTime = Time.realtimeSinceStartup - startTime;
        Debug.Log("Finished loading scene " + levelName + " in " + elapsedTime + " seconds");
        _LoadingSceneAsyncOperation = null;
        delFinish(levelName, param);
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

