using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameBase
{
    public class DrawLine : MonoBehaviour
    {
        private const float DOTLINE_INTERVAL = 0.375f;
        private const int DOTLINE_INTERVAL_PIXED = 32;
        #region 固有函数
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        #endregion

        private static GameObject _DotLineParentObj = null;
        private static List<GameObject> _DotLineList = new List<GameObject>();
        private static float _HeightWidthRate;
        private static Vector3 _WorldStartPos;

        public static void InitParentObj()
        {
            if (_DotLineParentObj == null)
            {
                //创建足够长度的线
                _DotLineParentObj = GameObjectManager.Instance.GetEmptyObject("LineObj");
                var dotObjTemp = ResourceManager.Instance.GetGameObject("DotLine/DotLine");
                int dotCount = (int)(Screen.width / DOTLINE_INTERVAL_PIXED) + 1;
                for (int i = 0; i < dotCount; ++i)
                {
                    var dotObj = GameObject.Instantiate(dotObjTemp);
                    dotObj.transform.parent = _DotLineParentObj.transform;
                    dotObj.transform.localPosition = new Vector3(DOTLINE_INTERVAL * i, 0, 0);

                    _DotLineList.Add(dotObj);
                }

                if (Camera.main.pixelHeight > Camera.main.pixelWidth)
                {
                    _HeightWidthRate = (float)Camera.main.pixelHeight / Camera.main.pixelWidth;
                    _WorldStartPos = new Vector3(Camera.main.orthographicSize, _HeightWidthRate * Camera.main.orthographicSize);
                }
                else
                {
                    _HeightWidthRate = (float)Camera.main.pixelWidth / Camera.main.pixelHeight;
                    _WorldStartPos = new Vector3(_HeightWidthRate * Camera.main.orthographicSize, Camera.main.orthographicSize);
                }
            }
        }

        public static void DrawDotLine(Vector3 startpos, Vector3 endPos)
        {
            InitParentObj();
            Debug.Log("startPos:" + startpos + " endPos:" + endPos);
            Debug.Log("startPos:" + Camera.main.ScreenToViewportPoint(startpos) + " endPos:" + Camera.main.ScreenToViewportPoint(endPos));
            float length = Vector3.Distance(startpos, endPos);
            int objCount = (int)(length / DOTLINE_INTERVAL_PIXED) + 1;
            for (int i = 0; i < _DotLineList.Count; ++i)
            {
                if (i < objCount)
                {
                    _DotLineList[i].SetActive(true);
                }
                else
                {
                    _DotLineList[i].SetActive(false);
                }
            }

            _DotLineParentObj.SetActive(true);

            float angle = Vector3.Angle(endPos - startpos, Vector3.right);
            Debug.Log("Angle:" + angle);

            _DotLineParentObj.transform.position = CalWroldPos(startpos);
            if (endPos.y > startpos.y)
            {
                _DotLineParentObj.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
            else
            {
                _DotLineParentObj.transform.rotation = Quaternion.Euler(0, 0, -angle);
            }
        }

        public static void HideDotLine()
        {
            _DotLineParentObj.SetActive(false);
        }

        public static Vector3 CalWroldPos(Vector3 screenPos)
        {
            Vector3 viewPos = Camera.main.ScreenToViewportPoint(screenPos);
            Vector3 worldPos = new Vector3(_WorldStartPos.x * viewPos.x * 2, _WorldStartPos.y * viewPos.y * 2, 0);
            return worldPos - _WorldStartPos;
        } 
    }
}
