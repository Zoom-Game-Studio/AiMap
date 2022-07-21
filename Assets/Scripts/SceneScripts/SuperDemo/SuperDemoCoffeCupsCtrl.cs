using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using KleinEngine;

using System;

namespace AppLogic

{

    public class SuperDemoCoffeCupsCtrl : MonoBehaviour

    {

        private Animator ani;

        private void Awake()

        {

            AppFacade.GetInstance().addEvent(ModuleEventType.SUPERDEMO_COFFE_BUTTON_CTRL, HandleSuperDemoCoffeButtonCtrl);

        }

        private void HandleSuperDemoCoffeButtonCtrl(EventObject ev)

        {

            Debug.Log("HandleSuperDemoCoffeButtonCtrl...");

            ani = transform.GetComponent<Animator>();

            if (ani == null) return;

            ani.SetInteger("Drop", 1);

        }

        private void Update()

        {

            if (ani == null) return;

            AnimatorStateInfo animatorInfo = ani.GetCurrentAnimatorStateInfo(0);

            if (animatorInfo.normalizedTime >= 1.0f && animatorInfo.IsName("Drop"))

            {

                ani.SetInteger("Drop", 0);

            }

        }

    }

}