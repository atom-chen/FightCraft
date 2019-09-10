using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIHPItem : UIItemBase
{
    public Slider _HPProcess;
    public Slider _MPProcess;

    private RectTransform _RectTransform;
    public MotionManager _ObjMotion;
    private Transform _FollowTransform;
    private Vector3 _HeightDelta;
    private bool _IsShowHP = true;

    public override void Show(Hashtable hash)
    {
        base.Show(hash);

        _ObjMotion = hash["InitObj"] as MotionManager;
        _IsShowHP = (bool)hash["ShowHP"];
        if (_IsShowHP)
        {
            _HPProcess.gameObject.SetActive(true);
        }
        else
        {
            _HPProcess.gameObject.SetActive(false);
        }
        _RectTransform = GetComponent<RectTransform>();
        _FollowTransform = _ObjMotion.AnimationEvent.transform;
        var transform = _FollowTransform.Find("center/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck");
        if (transform == null)
        {
            transform = _FollowTransform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck");
        }

        _HPProcess.value = 1;

        _HeightDelta = transform.position - _FollowTransform.position;
        _HeightDelta.x = 0;
        _HeightDelta.z = 0;
        _HeightDelta.y += 0.2f;

        for (int i = 0; i < _SpBuffNameTexts.Count; ++i)
        {
            _SpBuffNameTexts[i].gameObject.SetActive(false);
        }
    }


    public void Update()
    {
        if (_ObjMotion == null || _ObjMotion.IsMotionDie)
        {
            UIHPPanel.HideHPItem(this);
            gameObject.SetActive(false);
            return;
        }

        //_HPProcess.value = _ObjMotion.RoleAttrManager.HPPersent;
        if (_IsShowHP)
        {
            ActHPProcess();
        }

        ActMPProcess();

        ActSpBuffNames();

        _RectTransform.anchoredPosition = UIManager.Instance.WorldToScreenPoint(_FollowTransform.position + _HeightDelta);
    }

    #region act hp process

    private static float _ShowHpTimeStatic = 3.0f;
    private float _ShowHpTime;

    private void ActHPProcess()
    {
        if (_ObjMotion._ActionState != _ObjMotion._StateIdle
            && _ObjMotion._ActionState != _ObjMotion._StateMove)
        {
            _ShowHpTime = _ShowHpTimeStatic;
        }

        if (_ShowHpTime <= 0)
        {
            return;
        }

        _ShowHpTime -= Time.deltaTime;
        _HPProcess.gameObject.SetActive(true);
        _HPProcess.value = _ObjMotion.RoleAttrManager.HPPersent;
    }

    #endregion

    #region act mp process

    private void ActMPProcess()
    {
        if (_ObjMotion.SkillProcessing > 0 && _ObjMotion.SkillProcessing < 1)
        {
            _MPProcess.gameObject.SetActive(true);
            _MPProcess.value = _ObjMotion.SkillProcessing;
        }
        else
        {
            _MPProcess.gameObject.SetActive(false);
        }
    }

    #endregion

    #region act buff name

    public List<Text> _SpBuffNameTexts;

    private void ActSpBuffNames()
    {
        if (!_ObjMotion._IsBuffNameDirty)
            return;

        for (int i = 0; i < _SpBuffNameTexts.Count; ++i)
        {
            if (_ObjMotion._SpBuffNames.Count > i)
            {
                _SpBuffNameTexts[i].gameObject.SetActive(true);
                _SpBuffNameTexts[i].text = _ObjMotion._SpBuffNames[i];
            }
            else
            {
                _SpBuffNameTexts[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion
}

