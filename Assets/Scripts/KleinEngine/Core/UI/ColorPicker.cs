using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KleinEngine
{
    public class ColorPicker : MonoBehaviour
    {
        [SerializeField]
        RectTransform pickPointerTrans;
        [SerializeField]
        Image pickImage;
        [SerializeField]
        Image sliderImage;

        Transform pickImgTrans;
        Texture2D pickTex;
        Color pickColor;
        float colorGrayValue = 1;

        Action<Color> colorChangeCallBack;

        private void Awake()
        {
            pickImgTrans = pickImage.transform;
            pickTex = pickImage.mainTexture as Texture2D;
        }

        public void setColorChange(Action<Color> callBack)
        {
            colorChangeCallBack = callBack;
        }

        void onPickColor(BaseEventData data)
        {
            PointerEventData ev = data as PointerEventData;
            if (null == ev) return;
            Vector2 pos = pickImgTrans.InverseTransformPoint(ev.position);
            pickPointerTrans.anchoredPosition = pos;
            pickColor = pickTex.GetPixel((int)pos.x, (int)pos.y);
            sliderImage.color = pickColor;
            changeColor();
        }

        void onGaryChange(float value)
        {
            colorGrayValue = value;
            changeColor();
        }

        void changeColor()
        {
            Color co = pickColor * colorGrayValue;
            co.a = 1;
            if (null != colorChangeCallBack) colorChangeCallBack(co);
        }
    }
}
