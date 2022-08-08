using System;
using UnityEngine;

namespace C_ScriptsTest
{
    public class ShowRailGizmos : MonoBehaviour
    {
        [SerializeField] private Transform[] childList;
        [SerializeField] private Color color = Color.black;

        private void Reset()
        {
            childList = this.transform.GetComponentsInChildren<Transform>();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            for (var i = 0; i < childList.Length -1; i++)
            {
                var a = childList[i];
                var b = childList[i + 1];
                Gizmos.DrawLine(a.position,b.position);
            }
        }
    }
}