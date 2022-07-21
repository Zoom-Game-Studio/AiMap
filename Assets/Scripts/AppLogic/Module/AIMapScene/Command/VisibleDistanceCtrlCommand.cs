using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class VisibleDistanceCtrlCommand : BaseCommand
    {
        public override void onExecute(object param)
        {
            //return;
            addModuleEvent(ModuleEventType.VISIBLE_MODEL_CHANGE_BY_DISTANCE, HandleVisibleDistanceChanged);
        }

        private void HandleVisibleDistanceChanged(EventObject ev)
        {

            ModelVisibleInfo modelVisible = ev.param as ModelVisibleInfo;
            if (modelVisible == null) return;
            foreach (var model in modelVisible.modelList)
            {
                float deltaDistance = Vector3.Distance(model.transform.position, Camera.main.transform.position);
                bool isActice = deltaDistance > modelVisible.distance ? false : true;
                model.SetActive(isActice);
            }
        }
    }
}
