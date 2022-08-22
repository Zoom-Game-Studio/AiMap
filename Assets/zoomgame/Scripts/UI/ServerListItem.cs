using System;
using Architecture;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Waku.Module;

namespace QFramework.UI
{
    public class ServerListItem : MonoBehaviour,IServerListItem
    {
        public Text info;
        public Button btn;
        private AssetInfoItem itemInfo;
        public AudioClip clip;

        private void Start()
        {
            btn.onClick.AddListener(OnClickBtn);
        }

        [Button]
        void OnClickBtn()
        {
            AudioSource.PlayClipAtPoint(clip,Camera.main.transform.position);
            AssetDownloader.Instance.AddToDownloadList(itemInfo);
        }

        public void Init(AssetInfoItem evtInfo)
        {
            itemInfo = evtInfo;
            info.text = $"{evtInfo.place}：{evtInfo.name}，{evtInfo.description}";
        }
    }
}