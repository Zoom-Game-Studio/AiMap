using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class SuperDemoCoffeShopMenuCtrl : MonoBehaviour
    {
        private void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.SHOW_COFFE_SHOP_MENU,HandleShowCoffeShopMenu);
        }

        private void HandleShowCoffeShopMenu(EventObject ev)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

    }
}
