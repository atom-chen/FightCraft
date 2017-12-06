using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIBase : MonoBehaviour
{
    public string UIPath;
    public UILayer UILayer;

    #region fiex fun

    public void Awake()
    {
        Init();
    }
    public virtual void Init()
    {

    }

    #endregion

    #region show

    public virtual void PreLoad()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
    }

    public virtual void Show(Hashtable hash)
    {
        Show();
    }

    public virtual void Hide()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void ShowDelay(float time)
    {
        Hide();
        Invoke("Show", time);
    }

    public virtual void ShowLast(float time)
    {
        Show();
        Invoke("Hide", time);
    }

    public virtual void Destory()
    {
        UIManager.Instance.DestoryUI(UIPath);
    }


    #endregion

    #region static

    public static void SetGOActive(Component actGO, bool isActive)
    {
        if (actGO == null)
            return;
 
        actGO.gameObject.SetActive(isActive);
    }

    public static void SetGOActive(GameObject actGO, bool isActive)
    {
        if (actGO == null)
            return;

        actGO.SetActive(isActive);
    }

    #endregion
}

