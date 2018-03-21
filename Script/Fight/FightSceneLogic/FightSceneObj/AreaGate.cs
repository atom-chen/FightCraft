using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaGate : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
        TeleportUpdate();
    }

    #region teleport

    public Transform _DestPos;
    public bool _IsTransScene = true;
    public static float _TeleDistance = 3;
    public static float _TeleProcessTime = 1;

    private bool _Teleporting = false;
    private float _StartingTime = 0;

    private void TeleportUpdate()
    {

        var mainChar = FightManager.Instance.MainChatMotion;
        if (mainChar == null)
            return;

        if (mainChar._ActionState == mainChar._StateIdle &&
            Vector3.Distance(transform.position, mainChar.transform.position) < _TeleDistance)
        {
            if (!_Teleporting)
            {
                _Teleporting = true;
                _StartingTime = Time.time;

            }
            UpdateTeleProcesing();
        }
        else
        {
            if (_Teleporting)
            {
                UpdateTeleProcesing();
                _Teleporting = false;
                _StartingTime = 0;

            }
        }

    }

    private void UpdateTeleProcesing()
    {
        if (_Teleporting)
        {
            var timeDelta = Time.time - _StartingTime;
            FightManager.Instance.MainChatMotion._SkillProcessing = timeDelta / _TeleProcessTime;
            if (FightManager.Instance.MainChatMotion._SkillProcessing >= 1)
            {
                TeleportAct();
                FightManager.Instance.MainChatMotion._SkillProcessing = 0;
                _Teleporting = false;
            }
        }
        else
        {
            FightManager.Instance.MainChatMotion._SkillProcessing = 0;
        }
    }

    private void TeleportAct()
    {
        if (_DestPos == null)
            return;

        FightManager.Instance.TeleportToNextRegion(_DestPos, _IsTransScene);
    }

    #endregion
}
