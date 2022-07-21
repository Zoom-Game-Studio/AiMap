using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using KleinEngine;
namespace AppLogic
{
    public class DownloadManager : MonoBehaviour
    {
        public void ShowPOIIcon(POIInfoInUnity poiInfoInUnity, Image image)
        {
            if (poiInfoInUnity == null) return;
            StartCoroutine(DownSprite(poiInfoInUnity, image));
        }

        IEnumerator DownSprite(POIInfoInUnity poiInfoInUnity, Image image)
        {
            UnityWebRequest wr = new UnityWebRequest(poiInfoInUnity.icon);
            DownloadHandlerTexture texD1 = new DownloadHandlerTexture(true);
            wr.downloadHandler = texD1;
            yield return wr.SendWebRequest();
            int width = 128;
            int high = 128;
            if (!wr.isNetworkError)
            {
                Texture2D tex = new Texture2D(width, high);
                tex = texD1.texture;
                //保存本地          
                Byte[] bytes = tex.EncodeToPNG();
                string folderPath = Application.persistentDataPath + "/Icon";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = folderPath + "/" + poiInfoInUnity.id + ".png";
                if (!File.Exists(filePath))
                    File.WriteAllBytes(filePath, bytes);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                if (image != null)
                    image.sprite = sprite;
            }
        }

    }
}
