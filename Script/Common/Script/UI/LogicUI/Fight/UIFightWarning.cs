using UnityEngine;
using System.Collections;
using System.Collections.Generic;

 
 
using System;



public class UIFightWarning : UIBase
{

    #region static funs

    public static void ShowFightAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowFight", true);
        GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UIFightWarning", UILayer.BaseUI, hash);
    }

    public static void ShowBossAsyn()
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowBoss", true);
        GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UIFightWarning", UILayer.BaseUI, hash);
    }

    public static void ShowDirectAsyn(Transform directFrom, Transform directTo)
    {
        Hashtable hash = new Hashtable();
        hash.Add("ShowDirectFrom", directFrom);
        hash.Add("ShowDirectTo", directTo);
        GameCore.Instance.UIManager.ShowUI("LogicUI/Fight/UIFightWarning", UILayer.BaseUI, hash);
    }

    #endregion

    #region 

    void Update()
    {
        ShowDirectUpdate();
    }

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        if (hash.ContainsKey("ShowFight"))
        {
            ShowFight();
        }
        else if (hash.ContainsKey("ShowDirectFrom"))
        {
            Transform directFrom = (Transform)hash["ShowDirectFrom"];
            Transform directTo = (Transform)hash["ShowDirectTo"];
            ShowDirect(directFrom, directTo);
        }
        else if (hash.ContainsKey("ShowBoss"))
        {
            ShowBoss();
        }
    }



    #endregion

    #region 

    private const float _SHOW_FIGHT_LABEL_TIME = 2.0f;

    public GameObject _FightLabel;

    private void ShowFight()
    {
        StartCoroutine(ShowFightLabel());
    }

    private IEnumerator ShowFightLabel()
    {
        _FightLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(_SHOW_FIGHT_LABEL_TIME);
        _FightLabel.gameObject.SetActive(false);
    }
    #endregion

    #region 

    private const float _SHOW_BOSS_LABEL_TIME = 2.0f;

    public GameObject _BossLabel;

    private void ShowBoss()
    {
        StartCoroutine(ShowBossLabel());
    }

    private IEnumerator ShowBossLabel()
    {
        _BossLabel.gameObject.SetActive(true);
        yield return new WaitForSeconds(_SHOW_BOSS_LABEL_TIME);
        _BossLabel.gameObject.SetActive(false);
    }
    #endregion

    #region 

    public GameObject _GOLabel;
    public GameObject _DirectGO;

    public Transform _DirectFrom;
    public Transform _DirectTo;

    private void ShowDirectUpdate()
    {
        if (_DirectFrom == null || _DirectTo == null)
        {
            _GOLabel.SetActive(false);
            _DirectGO.SetActive(false);
            return;
        }

        if (Vector3.Distance(_DirectFrom.position, _DirectTo.position) < 10)
        {
            _GOLabel.SetActive(false);
            _DirectGO.SetActive(false);
            return;
        }

        var positionFrom = UIManager.Instance.WorldToScreenPoint(_DirectFrom.position);
        var positionTo = UIManager.Instance.WorldToScreenPoint(_DirectTo.position);
        //var positionFrom = _DirectFrom.position;
        //var positionTo = _DirectTo.position;
        _DirectGO.transform.position = positionFrom;
        _DirectGO.transform.LookAt(positionTo);

        //var direct = new Vector2(positionTo.x, positionTo.y) - new Vector2(positionFrom.x, positionFrom.y);
        //float atan = Mathf.Atan(direct.x / direct.y) * Mathf.Rad2Deg;
        //if (positionTo.y >= positionFrom.y)
        //{
        //    atan = -atan;
        //}
        //else
        //{
        //    atan += 180;
        //    atan = -atan;
        //}
        //_DirectGO.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, atan));
    }

    private void ShowDirect(Transform directFrom, Transform directTo)
    {
        _DirectFrom = directFrom;
        _DirectTo = directTo;

        _GOLabel.SetActive(true);
        _DirectGO.SetActive(true);
    }

    #endregion

    #region event

    public void OnBtnExitFight()
    {
        FightManager.Instance.LogicFinish(true);
    }

    #endregion
}

