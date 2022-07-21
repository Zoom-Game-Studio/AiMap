using UnityEngine;
using KleinEngine;

namespace AppLogic
{
    public class ZheKeYuanTriggerSwitch : MonoBehaviour
    {
        public GameObject[] goes;

        int index = 0;

        void Awake()
        {
            AppFacade.GetInstance().addEvent(ModuleEventType.SWITCH_OBJ_ON_TRIGGER, HandleSwitchObj);
        }

        private void HandleSwitchObj(EventObject ev)
        {
            Debug.Log("HandleSwitchObj...");
            Transform trans = ev.param as Transform;
            if (transform.Equals(trans))
                SwitchObj();
        }

        void HideAll()
        {
            foreach (var go in goes)
            {
                go.SetActive(false);
            }
        }

        void SwitchObj()
        {
            index++;
            if (index >= goes.Length) index = 0;
            HideAll();
            goes[index].SetActive(true);
        }
    }
}
