using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;

namespace AppLogic
{
    public class DataSwitchMediator : BaseMediator
    {
        private DataSwitchView dataSwitchView;
        public DataSwitchMediator()
        {
            m_mediatorName = MEDIATOR.DATA_SWITCH;
        }
        protected override void onInit()
        {
            //dataSwitchView = viewComponent as DataSwitchView;
            #region  Resources Load Mode Example
            Transform canvas = GameObject.Find("3DCanvas").transform;
            GameObject viewPrefab = Resources.Load<GameObject>("DataSwitchView");
            GameObject viewObj = GameObject.Instantiate<GameObject>(viewPrefab);
            viewObj.transform.SetParent(canvas, false);
            dataSwitchView = new DataSwitchView();
            dataSwitchView.init(viewObj);
            viewComponent = dataSwitchView;
            viewComponent.addEvent(BaseView.BUTTON_CLICK, onButtonClick);
            #endregion

        }
        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if (ev.param.Equals(dataSwitchView.CustomerSourceAnalysisBtn))
            {
                Debug.Log("CustomerSourceAnalysisBtn...");
                SwitchBtnClick();
            }
            else if (ev.param.Equals(dataSwitchView.CustomerSourceBtn))
            {
                Debug.Log("CustomerSourceBtn...");
                SwitchBtnClick();

            }
            else if (ev.param.Equals(dataSwitchView.PassengerFlowAnalysisBtn))
            {
                Debug.Log("PassengerFlowAnalysisBtn...");
                SwitchBtnClick();

            }
        }


        void SwitchBtnClick()
        {
            //sendModuleEvent(ModuleEventType.MODULE_EXIT, MEDIATOR.MODE_SWITCH);
        }

    }
}