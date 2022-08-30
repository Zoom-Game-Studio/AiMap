using System;
using BestHTTP;
using Sirenix.OdinInspector;
using UnityEngine;

namespace C_ScriptsTest
{
    public class CaimiTest : MonoBehaviour
    {
        public string url = "https://aimap.newayz.com/aimap/lantern/v1/ar_riddles/:id?lantern_id=";
        public int id;

        [Button]
        public void Send()
        {
            id++;
            var r = new HTTPRequest(new Uri(url+id),HTTPMethods.Get, (r, p) =>
            {
                Debug.Log(r);
                Debug.Log(p.DataAsText);
                p.Dispose();
                r?.Dispose();

            });
            r.Send();
        }
    }
}