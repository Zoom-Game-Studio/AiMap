using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using System;

namespace AppLogic
{
    public class PreviewMediator : BaseMediator
    {
        PreviewView previewView;


        public PreviewMediator()
        {
            m_mediatorName = MEDIATOR.PREVIEW;
        }
        protected override void onInit()
        {
            //previewView = viewComponent as PreviewView;
            addModuleEvent(ModuleEventType.START_PREVIEW_VIDEO, HandleVideoPreview);
            addModuleEvent(ModuleEventType.SET_PREVIEW_IMAGE, HandleSetPreviewImage);

            GetMdeiaPlayerCtrl();//获得场景中的播放组件

            sendModuleEvent(ModuleEventType.START_PREVIEW_VIDEO);

        }

        /// <summary>
        /// 设置预览模式下的显示图片
        /// </summary>
        /// <param name="ev"></param>
        private void HandleSetPreviewImage(EventObject ev)
        {
            if (ev.param == null) return;
            Texture texture  = ev.param as Texture;
            if (texture != null)
                previewView.SetPreviewImg(texture);
        }

        private void GetMdeiaPlayerCtrl()
        {
            if (previewView.RawImagePanel != null)
            {
                Transform tempTransform = previewView.RawImagePanel.transform.Find("VideoManager");
            }
        }
        
        private void HandleVideoPreview(EventObject ev)
        {
            if (ev.param == null) return;
            string videoPath = (string)ev.param;
        }

        protected override void onButtonClick(EventObject ev)
        {
            base.onButtonClick(ev);
            if(ev.param.Equals(previewView.VideoPanelCloseBtn))
            {

            }
            else if(ev.param.Equals(previewView.VideoPanelDeleteBtn))
            {

            }
            else if (ev.param.Equals(previewView.VideoPanelSaveBtn))
            {

            }
            else if(ev.param.Equals(previewView.VideoPanelShareBtn))
            {

            }
            else if (ev.param.Equals(previewView.VideoPanelUpLoadBtn))
            {

            }
        }

       
    }
}