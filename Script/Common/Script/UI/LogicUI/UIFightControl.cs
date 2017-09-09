using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
 


public class UIFightControl : UIBase
{

    #region static funs

    public static List<EVENT_TYPE> GetShowEvent()
    {
        List<EVENT_TYPE> showEvents = new List<EVENT_TYPE>();

        showEvents.Add(EVENT_TYPE.EVENT_FIGHT_START);

        return showEvents;
    }

    #endregion

    #region 



    #endregion

    #region 

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        Debug.Log("Star fight");
    }

    public void OnEnable()
    {

    }

    #endregion

    #region event


    #endregion
}

