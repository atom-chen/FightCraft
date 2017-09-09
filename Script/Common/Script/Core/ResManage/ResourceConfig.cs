using UnityEngine;
using System.Collections;

public class ResourceConfig : MonoBehaviour
{

    #region 

    private static ResourceConfig _Instance;

    public static ResourceConfig Instance
    {
        get
        {
            if (_Instance == null)
            {
                var obj = ResourceManager.Instance.GetInstanceGameObject("ResourceConfig");
                _Instance = obj.GetComponent<ResourceConfig>();
            }
            return _Instance;
        }
    }

    #endregion

    #region 

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    #region element

    public Color[] _ElementColor;
    public Gradient[] _ElementGradient;

    #endregion
}
