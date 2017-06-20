using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIBase : MonoBehaviour
{
    
    public string UIPath;

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

    public bool IsShowing()
    {
        return gameObject.activeInHierarchy;
    }

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
        GameUI.UIManager.Instance.DestoryUI(this);
    }

    public virtual void Destory(float time)
    {
        StartCoroutine(DestoryDelay(time));
    }

    private IEnumerator DestoryDelay(float time)
    {
        yield return new WaitForSeconds(time);
        Destory();
    }

    #endregion

}

