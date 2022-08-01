using DeveloperKit.Runtime.Pool;
using HttpData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace C_ScriptsTest
{
    public class LocationPointNode : MonoBehaviour, IHasUseOfState,IHasPopCallback
    {
        public bool IsInUse { get; set; }
        private Location _location;
        public Quaternion ro;

        public void OnPop()
        {
            gameObject.SetActive(true);
        }

        [Button]
        void ResetRo()
        {
            transform.rotation = _location.rotation;
        }

        [Button]
        void ApplyRo()
        {
        }
    }
}