using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaGateRandom : AreaGate
{

    protected override void UpdateTeleProcesing()
    {
        if (_Teleporting)
        {
            AddNextScene();
            var timeDelta = Time.time - _StartingTime;
            if (_AsyncLoad != null)
            {
                var process = Mathf.Min(_AsyncLoad.progress, timeDelta / _TeleProcessTime);
                FightManager.Instance.MainChatMotion._SkillProcessing = process;
                if (FightManager.Instance.MainChatMotion._SkillProcessing >= 1)
                {
                    TeleportAct();
                    FightManager.Instance.MainChatMotion._SkillProcessing = 0;
                    _Teleporting = false;
                }
            }
        }
        else
        {
            FightManager.Instance.MainChatMotion._SkillProcessing = 0;
        }
    }

    protected override void TeleportAct()
    {
        Debug.Log("TeleportAct");

        //FightManager.Instance.TeleportToNextRegion(_DestPos, _IsTransScene);
        RandomLogic.TeleportToNext();
    }

    #region act next scene

    private bool _IsAddNextScene = false;
    private AsyncOperation _AsyncLoad;

    public FightSceneLogicRandomArea RandomLogic { get; set; }

    public void AddNextScene()
    {
        if (_IsAddNextScene)
            return;

        if (RandomLogic == null)
            return;

        _IsAddNextScene = true;

        string nextScene = RandomLogic.GetNextScene();
        _AsyncLoad = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }

    #endregion
}
