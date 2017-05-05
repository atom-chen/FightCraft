using UnityEngine;
using System.Collections;

public class FightSceneAreaBase : MonoBehaviour
{
    

    public virtual void StartArea()
    {
        InitFightLogic();
        CloseAllDoor();
    }

    public virtual void FinishArea()
    {
        OpenAllDoor();
        _FightSceneLogic.AreaFinish(this);
    }

    protected virtual void UpdateArea()
    { }

    public virtual void MotionDie(MotionManager motion)
    {
        if (motion.RoleAttrManager.MotionType == MotionType.MainChar)
        {
            FightManager.Instance.LogicFinish(false);
            return;
        }

    }

    #region FightSceneLogic

    public FightSceneLogicPassArea _FightSceneLogic;

    private void InitFightLogic()
    {
        _FightSceneLogic = GetComponentInParent<FightSceneLogicPassArea>();
    }

    #endregion

    #region collider

    public enum TrigType
    {
        TRIG_AUTO,
        TRIG_EVENT,
    }

    public GameObject[] _AreaDoors;
    public TrigType _TrigAreaType;

    private void CloseAllDoor()
    {
        foreach (var door in _AreaDoors)
        {
            door.SetActive(true);
        }
    }

    private void OpenAllDoor()
    {
        foreach (var door in _AreaDoors)
        {
            door.SetActive(false);
        }
    }
    #endregion
}
