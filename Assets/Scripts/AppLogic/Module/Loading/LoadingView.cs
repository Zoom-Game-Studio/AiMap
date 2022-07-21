using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class LoadingView : BaseView
    {
        [UISign]
        public Text ProgressText;

        [UISign]
        public Image ImageMask;

        [UISign]
        public Image TestBtn;

        [UISign]
        public GameObject LoadingPanel;

        public void SetProgressText(string text)
        {
            if (ProgressText != null)
                ProgressText.text = text;
        }

        public void SetImageMaskFillAmount(float fillAmount)
        {
            if (ImageMask != null)
                ImageMask.fillAmount = fillAmount;
        }

    }
}

