using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Water : MonoBehaviour {

    public bool _EdgeBlend;
    public Camera _BlendCamera;

    void Update ()
    {

        if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
            _EdgeBlend = false;

        if (_EdgeBlend)
        {
            Shader.EnableKeyword("WATER_EDGEBLEND_ON");
            Shader.DisableKeyword("WATER_EDGEBLEND_OFF");
            // just to make sure (some peeps might forget to add a water tile to the patches)
            if (Camera.main)
                Camera.main.depthTextureMode |= DepthTextureMode.Depth;
        }
        else
        {
            Shader.EnableKeyword("WATER_EDGEBLEND_OFF");
            Shader.DisableKeyword("WATER_EDGEBLEND_ON");
        }
        //Debug.Log("Set camera depth:" + edgeBlend);

    }

    public void OnWillRenderObject()
    {
        if (Camera.current && _EdgeBlend)
            Camera.current.depthTextureMode |= DepthTextureMode.Depth;
    }
}
