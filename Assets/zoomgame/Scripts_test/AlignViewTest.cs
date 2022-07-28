using QFramework;
using Sirenix.OdinInspector;
using UnityEngine;

namespace C_ScriptsTest
{
    public class AlignViewTest : MonoBehaviour
    {
        public Transform view;
        public Transform origin;
        public Transform camera;

        [Button]
        void Align(Transform view)
        {
            var anchor = new GameObject();
            anchor.transform.rotation = view.rotation;
            anchor.transform.position = view.position;
            
            origin.SetParent(anchor.transform);

            anchor.transform.position = camera.transform.position;
            anchor.transform.rotation = camera.transform.rotation;

            origin.transform.SetParent(null);
            anchor.transform.localScale = Vector3.one;
            GameObject.Destroy(anchor);
        }
    }
}