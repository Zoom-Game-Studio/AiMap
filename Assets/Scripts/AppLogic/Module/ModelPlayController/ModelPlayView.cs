using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class ModelPlayView : BaseView
    {

        [UISign]
        public TopPanel TopPanel;

        protected override void onClick(object obj)
        {
            if (obj.Equals(TopPanel.ReturnBtn))
            {
                Debug.Log("clickl;sjdflk;js;dklj");
            }

        }
    }
    public class TopPanel : BaseView
    {
        [UISign]
        public Button ReturnBtn;


       
    }
}

