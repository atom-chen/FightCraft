using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        _Axis.x = Input.GetAxis("Horizontal");
        _Axis.y = Input.GetAxis("Vertical");
        _InputMotion.MotionInput(this);
    }

    public MotionManager _InputMotion;

    #region input 

    public Vector2 _Axis;

    public bool IsKeyDown(string key)
    {
        return Input.GetKeyDown(key);
    }

    #endregion
}
