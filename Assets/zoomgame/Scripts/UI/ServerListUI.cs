using System;
using DeveloperKit.Runtime.Pool;
using UniRx;
using UnityDeveloperKit.Runtime.Api;
using UnityEngine;
using UnityEngine.UI;
using Waku.Module;
using zoomgame.Scripts.Architecture.TypeEvent;

namespace QFramework.UI
{
    public interface IServerListItem : IComponent
    {
        void Init(AssetInfoItem evtInfo);
    }

    public class ServerListUI : MonoBehaviour
    {
        public GameObjectPool pool;
        private HttpDownLoad loader;
        private FloatReactiveProperty progress = new FloatReactiveProperty();
        [SerializeField] private Slider progressSlider;

        private void Start()
        {
            MessageBroker.Default.Receive<UpdateServerListEvent>().Subscribe(OnServerListUpdate).AddTo(this);
            MessageBroker.Default.Receive<DownloadEvent>().Subscribe(OnDownload).AddTo(this);
            progress.Subscribe(v =>
            {
                progressSlider.value = progress;
            });
        }

        private void Update()
        {
            if (loader != null)
            {
                progress.Value = loader.progress;
            }
        }
 
        void OnDownload(DownloadEvent evt)
        {
            this.loader = evt.resLoader;
        }

        void OnServerListUpdate(UpdateServerListEvent evt)
        {
            if (evt.infoList == null)
            {
                throw new ArgumentNullException("ServerListUI更新列表参数不可为null");
            }
            
            foreach (var item in pool.Content)
            {
                item.SetActive(false);
                pool.RecycleItem(item);
            }

            for (var i = 0; i < evt.infoList.Count; i++)
            {
                var item = pool.PopItem().GetComponent<IServerListItem>();
                item.Init(evt.infoList[i]);
                item.gameObject.SetActive(true);
            }
        }
    }
}