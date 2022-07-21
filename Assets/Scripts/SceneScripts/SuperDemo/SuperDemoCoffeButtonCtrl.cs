using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using KleinEngine;

using System;

namespace AppLogic

{

    public class SuperDemoCoffeButtonCtrl : MonoBehaviour

    {

        private void Awake()

        {

            AppFacade.GetInstance().addEvent(ModuleEventType.SUPERDEMO_COFFE_BUTTON_CTRL, HandleSuperDemoCoffeButtonClick);

        }

        private void HandleSuperDemoCoffeButtonClick(EventObject obj)

        {

            Debug.Log("HandleSuperDemoCoffeButtonClick...");

            Animation ani = transform.GetComponent<Animation>();

            if (ani == null) return;

            ani.Play();

            //Animator ani = transform.GetComponent<Animator>();

            //if (ani == null) return;

            //ani.enabled = false;

            //ani.enabled = true;


        }

    }

}