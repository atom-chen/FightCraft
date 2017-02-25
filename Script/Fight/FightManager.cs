using UnityEngine;
using System.Collections;

public class FightManager : MonoBehaviour
{
    #region instance

    private FightManager _Instance;

    public FightManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    #endregion

    // Use this for initialization
    void Start ()
    {
        _Instance = this;
    }
	
	// Update is called once per frame
	void FixedUpdate()
    {
        LogicUpdate();
    }

    #region update

    private int _Click = 0;
    public int Click
    {
        get
        {
            return _Click;
        }
    }

    public void LogicUpdate()
    {
        ++_Click;
    }

    #endregion
}
