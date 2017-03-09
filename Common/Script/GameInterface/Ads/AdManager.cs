using UnityEngine;
using System.Collections;
using System;

public class AdManager : MonoBehaviour
{
    #region static

    private static AdManager _Instance;
    public static AdManager Instance
    {
        get
        {
            return _Instance;
        }
    }

    public static void InitAdManager()
    {
        if (_Instance == null)
        {
            GameObject adObj = new GameObject("AdObject");
            _Instance = adObj.AddComponent<AdManager>();
        }
    }
    
    #endregion

    #region 固有函数
    
	void Start () 
	{
        DontDestroyOnLoad(this);
        InitAd();
	}
	
	//void Update () 
	//{
	
	//}

    public string _DebugMsg;
    //void OnGUI()
    //{
    //    GUI.TextArea(new Rect(0, 0, 300, 100), _DebugMsg);
    //}
    #endregion

    #region 

    private string LOCATION_TOP = "isNotTop";
    private string MOGO_ID = "d8df0c1565aa408a91a4ac88ecdf343d";
    //private string MOGO_ID = "93535c6092f543e8a257ee435a69da06";  //test
    private AndroidJavaObject _MainJO;

    private void InitAd()
    {
        //_DebugMsg += "InitAds/n";
        //AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //_MainJO = unityClass.GetStatic<AndroidJavaObject>("currentActivity");

        //try
        //{
        //    //_MainJO.Call("initBanner");
        //    //_MainJO.Call("initInterteristal");
        //    //_MainJO.Call("initInterstitialAd", MOGO_ID);
        //}
        //catch (Exception e)
        //{
        //    _DebugMsg = e.ToString();
        //}
    }

    public void ShowBannerAd()
    {
        //try
        //{
        //    _MainJO.Call("refreshBanner");
        //}
        //catch (Exception e)
        //{
        //    _DebugMsg = e.ToString();
        //}
    }

    public void ShowInterstitialAd()
    {
        //try
        //{
        //    _MainJO.Call("refreshInterteristal");
        //    IsFinishiInterstitialAd = false;
        //    //_DebugMsg = "showInterstitialAd ok";
        //}
        //catch (Exception e)
        //{
        //    _DebugMsg = e.ToString();
        //}
    }

    public void HideInterstitialAd()
    {
        //try
        //{
        //    _MainJO.Call("stopInterteristal");
        //    IsFinishiInterstitialAd = false;
        //}
        //catch (Exception e)
        //{
        //    _DebugMsg = e.ToString();
        //}
    }
    #endregion

        #region 回调

    public bool IsFinishiInterstitialAd { get; set; }

    public void FinishInterstitialAd()
    {
        IsFinishiInterstitialAd = true;
    }

    #endregion
}
