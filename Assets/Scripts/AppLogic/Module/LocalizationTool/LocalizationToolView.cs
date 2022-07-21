using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class LocalizationToolView : BaseView
    {
        [UISign("LocalizationPanel/")]
        public Button AddTZBtn;
        [UISign("LocalizationPanel/")]
        public Button AddTYBtn;
        [UISign("LocalizationPanel/")]
        public Button AddTXBtn;

        [UISign("LocalizationPanel/")]
        public Button ReduceTZBtn;
        [UISign("LocalizationPanel/")]
        public Button ReduceTYBtn;
        [UISign("LocalizationPanel/")]
        public Button ReduceTXBtn;

        [UISign("LocalizationPanel/")]
        public Button AddRZBtn;
        [UISign("LocalizationPanel/")]
        public Button AddRYBtn;
        [UISign("LocalizationPanel/")]
        public Button AddRXBtn;

        [UISign("LocalizationPanel/")]
        public Button ReduceRZBtn;
        [UISign("LocalizationPanel/")]
        public Button ReduceRYBtn;
        [UISign("LocalizationPanel/")]
        public Button ReduceRXBtn;

        [UISign("LocalizationPanel/")]
        public Button ReduceScaleBtn;
        [UISign("LocalizationPanel/")]
        public Button AddScaleBtn;

        [UISign("LocalizationPanel/")]
        public Button BackBtn;
        [UISign("LocalizationPanel/")]
        public Button SaveBtn;
        [UISign("LocalizationPanel/")]
        public Text SaveStatus;

        public void SetSaveStutusText(string txt)
        {
            if (txt == null) return;
            SaveStatus.text = txt;
        }
    }
}

