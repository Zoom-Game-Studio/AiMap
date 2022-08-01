using Sirenix.OdinInspector;
using UnityEngine;

namespace C_ScriptsTest
{
    public class ShowPng : MonoBehaviour
    {
        [SerializeField] private string last = @"E:\ZoomGAME\aimap\";

        [Button]
        void ChangeName()
        {
            var n = name.Remove(0, last.Length);
            name = n;
        }

        [Button]
        void Show()
        {
            // var sprite = (Sprite)AssetDatabase.LoadAssetAtPath(name, typeof(Sprite));
            // var spriteRender = new GameObject("png").AddComponent<SpriteRenderer>();
            // spriteRender.sprite = sprite;
            // var t = spriteRender.transform;
            // t.SetParent(transform);
            // t.localPosition = Vector3.zero;
            // t.localEulerAngles = new Vector3(0, 180, 180);
            // t.localScale = Vector3.one;
        }
    }
}