using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class LocalizeView : BaseView
    {
        [UISign]
        public GameObject LocalizePanel;
        [UISign("LocalizePanel/")]
        public Button LocalizeSwitchBtn;
        [UISign("LocalizePanel/")]
        public Button TestBtn;
        [UISign("LocalizePanel/LocalizeSwitchBtn/")]
        public Text LocalizeSwitchBtnText;
        [UISign("LocalizePanel/")]
        public Text Status;
        [UISign("LocalizePanel/")]
        public Text Status1;
        [UISign("LocalizePanel/")]
        public Text Status2;
        [UISign("LocalizePanel/")]
        public Button EnableBtn;


        public void SetStatusText(string text)
        {
            if (Status == null) return;
            Status.text = text;
        }
        public void SetStatus1Text(string text)
        {
            if (Status1 == null) return;
            Status1.text = text;
        }
        public void SetStatus2Text(string text)
        {
            if (Status2 == null) return;
            Status2.text = text;
        }

    }
}