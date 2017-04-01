using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

using System;
using UnityEngine.Events;

public class UINumBoardInput : UIBase
{

    #region param
    
    public Text _InputNum;
    public GameObject _NumBoard;
    public Button _BtnAdd;
    public Button _BtnDec;

    private static Material _ImageGrayMaterial;
    private int _Value;
    public int Value
    {
        get
        {
            _Value = int.Parse(_InputNum.text);
            return _Value;
        }
        set
        {
            _Value = value;
            SetNumBtnState();
            _InputNum.text = _Value.ToString();
        }
    }

    private int _MaxValue = -1;
    private int _MinValue = 0;

    [Serializable]
    public class NumModifyEvent : UnityEvent
    {
        public NumModifyEvent()
        {

        }
    }

    [SerializeField]
    private NumModifyEvent _NumModifyEvent;

    #endregion

    #region 

    public void Init(int initValue, int minValue, int maxValue)
    {
        _MaxValue = maxValue;
        _MinValue = minValue;
        Value = initValue;

        if (_ImageGrayMaterial == null)
        {
            _ImageGrayMaterial = Resources.Load<Material>("Material/ImageEffectGray");
        }
    }

    #endregion

    #region add/dec

    public void BtnAdd(int stepValue)
    {
        int resValue = Value + stepValue;
        SetValue(resValue);

        _NumModifyEvent.Invoke();

        
    }

    public void BtnDec(int stepValue)
    {
        int resValue = Value - stepValue;
        SetValue(resValue);

        _NumModifyEvent.Invoke();

        //SetNumBtnState();
    }

    private void SetNumBtnState()
    {
        if (_Value == _MaxValue)
        {
            _BtnAdd.interactable = (false);
            _BtnDec.interactable = (true);

            _BtnAdd.image.material = _ImageGrayMaterial;
            _BtnDec.image.material = null;
        }
        else if (_Value == _MinValue)
        {
            _BtnAdd.interactable = (true);
            _BtnDec.interactable = (false);

            _BtnAdd.image.material = null;
            _BtnDec.image.material = _ImageGrayMaterial;
        }
        else
        {
            _BtnAdd.interactable = (true);
            _BtnDec.interactable = (true);

            _BtnAdd.image.material = null;
            _BtnDec.image.material = null;
        }
    }

    #endregion

    #region num board

    private int _InputBoardNum = 0;

    public void OnBtnNumBoardOpen()
    {
        _NumBoard.SetActive(!_NumBoard.activeSelf);
        _InputBoardNum = 0;
    }

    public void OnBtnNumInput(int num)
    {
        _InputBoardNum = _InputBoardNum * 10 + num;

        SetValue(_InputBoardNum);

        _NumModifyEvent.Invoke();
    }

    public void OnBtnNumDelete()
    {
        _InputBoardNum = (int)(_InputBoardNum * 0.1f);

        SetValue(_InputBoardNum);

        _NumModifyEvent.Invoke();
    }

    public void OnBtnMax()
    {
        _InputBoardNum = _MaxValue;

        SetValue(_InputBoardNum);

        _NumModifyEvent.Invoke();
    }

    #endregion

    private void SetValue(int resValue)
    {
        if (_MaxValue > _MinValue && _MaxValue > 0 && _MinValue >= 0)
        {
            Value = Mathf.Clamp(resValue, _MinValue, _MaxValue);
        }
        else if (_MinValue >= 0)
        {
            Value = Mathf.Min(resValue, _MinValue);
        }
    }

}

