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

        public void SetLocation(Location location)
        {
            _location = location;
            if (location.success)
            {
                name = "success";
                transform.localPosition = location.translation;
                transform.rotation = Quaternion.Euler(90,0,0) * _location.rotation;
            }
            else
            {
                name = "fail";
            }
        }

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