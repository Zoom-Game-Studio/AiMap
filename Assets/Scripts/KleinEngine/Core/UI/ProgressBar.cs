using UnityEngine;
using UnityEngine.UI;

namespace KleinEngine
{
    public class ProgressBar : BaseView
    {
        [UISign]
        public RectTransform backBar;
        [UISign]
        public RectTransform frontBar;
        [UISign]
        public Text infoTxt;

        Vector3 scale = Vector3.one;
        public void setProgress(float progress)
        {
            if (progress > 1) progress = 1;
            scale = frontBar.localScale;
            scale.x = progress;
            if (null != frontBar) frontBar.localScale = scale;
        }

        public void setInfo(string str)
        {
            if (null != infoTxt) infoTxt.text = str;
        }
    }
}
