using UnityEngine;

namespace zoomgame.Scripts.Game
{
    /// <summary>
    /// 截图定位提示
    /// </summary>
    public class LocationTip : MonoBehaviour
    {
        private float _timer;

        private void Update()
        {
            this._timer += Time.deltaTime;
            if (_timer > 2)
            {
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            _timer = 0;
        }
    }
}