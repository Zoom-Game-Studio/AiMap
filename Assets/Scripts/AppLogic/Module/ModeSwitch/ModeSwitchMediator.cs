using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;

namespace AppLogic
{
    public class ModeSwitchMediator : BaseMediator
    {
        private ModeSwitchView modeSwitchView;
        public ModeSwitchMediator()
        {
            m_mediatorName = MEDIATOR.MODE_SWITCH;
        }
        protected override void onInit()
        {
            //modeSwitchView = viewComponent as ModeSwitchView;
        }
        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if (ev.param.Equals(modeSwitchView.SwitchARBtn))
            {
                Debug.Log("ModeSwitchAR...");
                modeSwitchView.ButtonPanel.SetActive(false);
                modeSwitchView.SwitchARBtn.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                //SDKManager.SendCurrentViewToPhone("This is a photo path...");
                SwitchBtnClick();
            }
            else if (ev.param.Equals(modeSwitchView.SwitchCityBtn))
            {
                Debug.Log("ModeSwitchCity");
                SwitchBtnClick();
            }
        }


        void SwitchBtnClick()
        {
            sendModuleEvent(ModuleEventType.LOADING_SCENE, SCENE_NAME.AIMAPSCENE);
            //sendModuleEvent(ModuleEventType.SET_LOADING_ANI, true);
        }
    }
}