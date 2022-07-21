using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class DataSwitchView : BaseView
    {
        [UISign]
        public GameObject SwitchBtnPanel;
        [UISign("SwitchBtnPanel/")]
        public Button PassengerFlowAnalysisBtn;
        [UISign("SwitchBtnPanel/")]
        public Button CustomerSourceBtn;
        [UISign("SwitchBtnPanel/")]
        public Button CustomerSourceAnalysisBtn;

        protected override void onClick(object obj)
        {
            base.onClick(obj);
        }
    }
}