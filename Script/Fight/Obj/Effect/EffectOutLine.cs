﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// 边缘高亮
public class EffectOutLine : EffectController
{
    public Material _OutLineMaterial;

    public override void PlayEffect()
    {
        base.PlayEffect();

        AddMaterial();
    }

    public override void PlayEffect(float speed)
    {
        base.PlayEffect(speed);

        AddMaterial();
    }

    public override void HideEffect()
    {
        base.HideEffect();

        RemoveMaterial();
    }

    public void Update()
    {
        UpdateMatAnim();
    }

    #region 

    private SkinnedMeshRenderer _SkinnedMesh;
    private static Dictionary<SkinnedMeshRenderer, int> _RenderMatCnt = new Dictionary<SkinnedMeshRenderer, int>();
    public Material _MatInstance;

    private void AddMaterial()
    {
        if (_SkinnedMesh == null)
        {
            var motion = gameObject.GetComponentInParent<MotionManager>();
            _SkinnedMesh = motion.GetComponentInChildren<SkinnedMeshRenderer>();
        }

        var matCnt = AddMatCnt();
        if (matCnt > 1)
            return;
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {

            _MatInstance = GameObject.Instantiate<Material>(_OutLineMaterial);
            Material[] newMaterialArray = new Material[_SkinnedMesh.materials.Length + 1];
            newMaterialArray[0] = _MatInstance;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                //if (_SkinnedMesh.materials[i].name.Contains("Outline"))
                //{
                //    return;
                //}
                //else
                {
                    newMaterialArray[i + 1] = _SkinnedMesh.materials[i];
                    newMaterialArray[i + 1].SetInt("_ZWrite", 1);
                    newMaterialArray[i + 1].renderQueue = 2450;
                }
            }

            _SkinnedMesh.materials = newMaterialArray;

        }
    }

    private void RemoveMaterial()
    {
        var matHandleCnt = DecMatCnt();
        Debug.Log("DecMatCnt:" + matHandleCnt);
        if (matHandleCnt > 0)
            return;
        //foreach (SkinnedMeshRenderer curMeshRender in meshes)
        {
            int newMaterialArrayCount = 0;
            for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
            {
                if (_SkinnedMesh.materials[i].name.Contains("Outline"))
                {
                    newMaterialArrayCount++;
                }
            }

            if (newMaterialArrayCount > 0)
            {
                Material[] newMaterialArray = new Material[newMaterialArrayCount];
                int curMaterialIndex = 0;
                for (int i = 0; i < _SkinnedMesh.materials.Length; i++)
                {
                    if (curMaterialIndex >= newMaterialArrayCount)
                    {
                        break;
                    }
                    if (!_SkinnedMesh.materials[i].name.Contains("Outline"))
                    {
                        newMaterialArray[curMaterialIndex] = _SkinnedMesh.materials[i];
                        curMaterialIndex++;
                    }
                }

                _SkinnedMesh.materials = newMaterialArray;
            }
        }
    }

    private int AddMatCnt()
    {
        if (!_RenderMatCnt.ContainsKey(_SkinnedMesh))
        {
            _RenderMatCnt.Add(_SkinnedMesh, 0);
        }

        ++_RenderMatCnt[_SkinnedMesh];
        Debug.Log("AddMatCnt:" + _RenderMatCnt[_SkinnedMesh]);
        return _RenderMatCnt[_SkinnedMesh];
    }

    private int DecMatCnt()
    {
        if (!_RenderMatCnt.ContainsKey(_SkinnedMesh))
        {
            return 0;
        }

        --_RenderMatCnt[_SkinnedMesh];
        return _RenderMatCnt[_SkinnedMesh];
    }

    #endregion

    #region mat anim

    public Animator _Animator;
    public Color _MatColor;
    public float _MatWidthRate;
    public float _MatBaseWidth;
    
    private void UpdateMatAnim()
    {
        if (_MatInstance == null)
            return;

        _MatInstance.SetColor("_OutlineColor", _MatColor);
        _MatInstance.SetFloat("_Outline", _MatWidthRate * _MatBaseWidth);
    }

    public void PlayHitted()
    {
        _Animator.Play("EffectOutlineAnimHitted");
    }

    #endregion
}