using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIImgText : MonoBehaviour
{
    public UIImgFont _ImgFont;

    protected string _text = "";
    [SerializeField]
    public string text
    {
        get
        {
            return _text;
        }
        set
        {
            _text = value;
            ShowImage(_text);
        }
    }

    #region

    protected Stack<Image> _IdleImgs = new Stack<Image>();

    protected Image PopIdleImage()
    {
        if (_IdleImgs == null || _IdleImgs.Count == 0)
        {
            var imageGO = new GameObject("charImg");
            var image = imageGO.AddComponent<Image>();
            imageGO.transform.SetParent(transform);
            imageGO.transform.localPosition = Vector2.zero;
            imageGO.transform.localRotation = Quaternion.Euler(Vector3.zero);
            imageGO.transform.localScale = Vector3.one;
            image.gameObject.SetActive(true);
            return image;
        }
        else
        {
            var image = _IdleImgs.Pop();
            image.gameObject.SetActive(true);
            return image;
        }
    }

    protected List<Image> _CharImages = new List<Image>();

    protected void ClearImage()
    {
        for (int i = 0; i < _CharImages.Count; ++i)
        {
            _CharImages[i].gameObject.SetActive(false);
            _IdleImgs.Push(_CharImages[i]);
        }
        _CharImages.Clear();
    }

    protected void ShowImage(string text)
    {
        ClearImage();
        _ImgFont.InitChars();
        for (int i = 0; i < text.Length; ++i)
        {
            var image = PopIdleImage();
            if (!_ImgFont._DictImgChars.ContainsKey(text[i]))
            {
                Debug.LogError("No Img Char:" + text[i]);
                continue;
            }
            var charImg = _ImgFont._DictImgChars[text[i]];
            image.sprite = charImg._Image;
            image.rectTransform.sizeDelta = new Vector2(charImg._CharWidth, charImg._CharHeight);
            image.rectTransform.SetAsLastSibling();

            _CharImages.Add(image);
        }
    }
    #endregion
}
