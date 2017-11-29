using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageTipItem : UIItemBase
{

    #region 

    public Text ShowText;

    public void SetMessage(string tip)
    {
        ShowText.text = tip;
        StartCoroutine(HideItem());
    }

    public IEnumerator HideItem()
    {
        yield return new WaitForSeconds(2.0f);
        ResourcePool.Instance.RecvIldeUIItem(gameObject);
    }

    public void RecvImmediate()
    {
        StopAllCoroutines();
        ResourcePool.Instance.RecvIldeUIItem(gameObject);
    }

    #endregion
}
