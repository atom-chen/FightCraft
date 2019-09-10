using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Tables;
 

public class FightSceneLogicPassGold : FightSceneLogicPassArea
{
    public float _NextWaveTime = 5.0f;
    public int _LargeCircleWave = 10;

    private int _WaveCnt = 0;
    private float _LastWaveTime = 0;
    private bool _IsCanStartNextWave = false;

    public override void StartLogic()
    {
        if (FightManager.Instance.MainChatMotion != null)
        {
            FightManager.Instance.MainChatMotion.SetPosition(_MainCharBornPos.position);
            FightManager.Instance.MainChatMotion.SetRotate(_MainCharBornPos.rotation.eulerAngles);
        }
        var actGroup = FightManager.Instance._AreaGroups[LogicManager.Instance.EnterStageInfo.ValidScenePath[0]];
        actGroup._LightGO.SetActive(true);

        StartCoroutine(StartLogicDelay());
    }

    private IEnumerator StartLogicDelay()
    {
        yield return new WaitForSeconds(2.0f);

        AreaStart(_FightArea[0]);

        _IsStart = true;
        StartTimmer();
    }

    public override void AreaStart(FightSceneAreaBase startArea)
    {
        ++_WaveCnt;
        _LastWaveTime = Time.time;
        base.AreaStart(startArea);
    }

    public override void AreaFinish(FightSceneAreaBase finishArea)
    {
        _IsCanStartNextWave = true;  
    }

    public override void StartNextArea()
    {
        base.StartNextArea();
    }

    protected override void UpdateLogic()
    {
        if (_IsCanStartNextWave && Time.time - _LastWaveTime > _NextWaveTime)
        {
            int wave = _WaveCnt % _LargeCircleWave;

            if (wave == 0)
            {
                AreaStart(_FightArea[1]);
            }
            else
            {
                AreaStart(_FightArea[0]);
            }
        }
        //UpdateTeleport();
    }
}
