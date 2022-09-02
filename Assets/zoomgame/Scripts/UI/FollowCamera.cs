using Sirenix.OdinInspector;
using UnityEngine;

namespace QFramework.UI
{
    public class FollowCamera : MonoBehaviour
    {
        private Transform _cameraTransform;
        private bool _isFollow = false;
        private Transform _temp;
        private void Start()
        {
            _cameraTransform = Camera.main.transform;
            _temp = new GameObject("相机跟随点").transform;
        }

        [Button]
        public void OnSwitchFollow()
        {
            _temp.position = transform.position;
            _temp.rotation = transform.rotation;
            _temp.SetParent(_cameraTransform);
            _isFollow = !_isFollow;
        }

        private void Update()
        {
            if (_isFollow)
            {
                transform.position = _temp.position;
                transform.rotation = _temp.rotation;
            }
        }
    }
}