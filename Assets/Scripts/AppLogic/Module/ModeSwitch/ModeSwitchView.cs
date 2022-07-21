using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.UI;
namespace AppLogic
{
    public class ModeSwitchView : BaseView
    {
        [UISign]
        public GameObject ButtonPanel;
        [UISign("ButtonPanel/")]
        public Button SwitchARBtn;
        [UISign("ButtonPanel/")]
        public Button SwitchCityBtn;

        protected override void onClick(object obj)
        {
            base.onClick(obj);
            if(obj.Equals(SwitchARBtn))
            {
                
            }    
        }
    }
}