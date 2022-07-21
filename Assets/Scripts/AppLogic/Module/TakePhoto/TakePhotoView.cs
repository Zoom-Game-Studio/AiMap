using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class TakePhotoView : BaseView
    {

        [UISign]
        public GameObject TakePhotoPanel;
        [UISign("TakePhotoPanel/")]
        public Button TakePhotoBtn;

        protected override void onClick(object obj)
        {
            base.onClick(obj);
            if (obj.Equals(TakePhotoBtn))
            {
                Debug.Log("TakePhoto cliked");
            }
        }
    }
}

