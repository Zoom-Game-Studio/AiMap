using System;
using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace AppLogic
{
    public class Wayz9LLuckSignCtrl : MonoBehaviour
    {
        void Start()
        {
            GetComponent<Button>().onClick.AddListener(LuckySignOnClick);
        }


        private void LuckySignOnClick()
        {
            IDictionary luckySigns = ConfigManager.GetConfigs<Wayz9LLuckySign>();
            Debug.Log("luckySigns:" + luckySigns.Count);
            Random ran = new Random();

            int luckNum = ran.Next(1, luckySigns.Count);
            foreach (Wayz9LLuckySign item in luckySigns.Values)
            {
                if (item == null) continue;
                if((item.id).Equals(luckNum.ToString()))
                {

                    return;
                }


            }
        }


    }
}
