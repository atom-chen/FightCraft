using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class UICameraTexture : UIBase, IDragHandler
{
    #region 

    public class FakeShowObj
    {
        public Camera _ObjCamera;
        public Transform _ObjTransorm;
        public RenderTexture _ObjTexture;
        public GameObject _ShowingModel;
    }

    private static Stack<FakeShowObj> _IdleFakeList = new Stack<FakeShowObj>();
    private FakeShowObj GetIdleFakeShow()
    {
        if (_IdleFakeList != null && _IdleFakeList.Count > 0)
        {
            var popFakeObj =  _IdleFakeList.Pop();
            if (popFakeObj._ObjCamera != null)
            {
                popFakeObj._ObjCamera.gameObject.SetActive(true);
                return popFakeObj;
            }
        }

        return CreateNewFake();
    }

    private FakeShowObj CreateNewFake()
    {
        FakeShowObj fakeObj = new FakeShowObj();

        GameObject cameraGO = GameObject.Instantiate(_CameraPrefab) as GameObject;
        fakeObj._ObjCamera = cameraGO.GetComponent<Camera>();
        cameraGO.transform.position = new Vector3(1000 + 100 * _CallTimes, -1000, 0);
        fakeObj._ObjTransorm = cameraGO.transform.FindChild("GameObject");
        fakeObj._ObjTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        fakeObj._ObjCamera.orthographicSize = (cameraSize <=0 ?1:cameraSize);
        fakeObj._ObjCamera.targetTexture = fakeObj._ObjTexture;
        ++_CallTimes;

        return fakeObj;
    }

    private void CGFakeObj(FakeShowObj fakeObj)
    {
        fakeObj._ObjCamera.gameObject.SetActive(false);
        GameObject.Destroy(fakeObj._ShowingModel);
        _IdleFakeList.Push(fakeObj);
    }

    private static FakeShowObj _FakeMainPlayer = null;

    #endregion

    public GameObject _CameraPrefab;
    public RawImage _RawImage;

    FakeShowObj _FakeObj;
    public float cameraSize = 1;
    public Vector3 ModelPos;
    private static int _CallTimes = 0;

    public void InitShowGO(GameObject showObj)
    {
        if (_FakeObj == null)
        {
            _RawImage.color = new Color(1, 1, 1, 0);
            _FakeObj = GetIdleFakeShow();
            _RawImage.texture = _FakeObj._ObjTexture;

            _RawImage.color = new Color(1, 1, 1, 1);
        }

        if (_FakeObj._ShowingModel != null && showObj.name == _FakeObj._ShowingModel.name)
            return;

        _FakeObj._ShowingModel = GameObject.Instantiate(showObj) as GameObject;
        _FakeObj._ShowingModel.transform.parent = _FakeObj._ObjTransorm;
        _FakeObj._ShowingModel.transform.localPosition = Vector3.zero + ModelPos;
        _FakeObj._ShowingModel.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _FakeObj._ShowingModel.name = showObj.name;

        Transform[] trans = _FakeObj._ShowingModel.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trans.Length; ++i)
        {
            trans[i].gameObject.layer = 0;
        }

    }

    public void OnDestroy()
    {
        if (_FakeObj != null)
        {
            CGFakeObj(_FakeObj);
        }
    }

    public void ShowRawImage()
    {
        _RawImage.transform.localPosition = Vector3.zero;
    }

    #region set model

    #endregion

    #region opt

    public void OnDrag(PointerEventData eventData)
    {
        if (_FakeObj == null)
            return;

        if (_FakeObj._ShowingModel == null)
            return;

        Vector3 newObjAngle = _FakeObj._ShowingModel.transform.localRotation.eulerAngles;
        newObjAngle.y -= eventData.delta.x;
        _FakeObj._ShowingModel.transform.localRotation = Quaternion.Euler(newObjAngle);
    }

    #endregion

    #region act

    public void BtnAuto()
    { }

    public void BtnSpeak()
    { }

    public void BtnLeave()
    { }

    public void BtnDestoryTeam()
    { }

    public void BtnFollow()
    { }

    public void BtnStopFollow()
    { }

    

    #endregion

}
