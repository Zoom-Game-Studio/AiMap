using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AppLogic
{
    public class ButtonNextController : MonoBehaviour
    {
       public void  ButtonOnClick()
        {
            Debug.Log("buttononclick...");
            PersonController person = FindObjectOfType<PersonController>();
            if (person == null) { Debug.Log("person is null:");return; }
            person.NextButtonClicked();
        }
    }
}
