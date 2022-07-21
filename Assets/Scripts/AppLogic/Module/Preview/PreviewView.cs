using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KleinEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class PreviewView : BaseView
    {
        [UISign]
        public GameObject RawImagePanel;
        [UISign("RawImagePanel/")]
        public RawImage PlayRawImage;


        [UISign("ShareBtnPanel/GridPanel/")]
        public Button VideoPanelDeleteBtn;
        [UISign("ShareBtnPanel/GridPanel/")]
        public Button VideoPanelShareBtn;
        [UISign("ShareBtnPanel/GridPanel/")]
        public Button VideoPanelSaveBtn;
        [UISign("ShareBtnPanel/GridPanel/")]
        public Button VideoPanelUpLoadBtn;
        
        [UISign]
        public Button VideoPanelCloseBtn;
        [UISign]
        public GameObject Mask;


        /// <summary>
        /// 设置预览图片
        /// </summary>
        public void SetPreviewImg(Texture texture)
        {
            PlayRawImage.texture = texture;
        }

        /// <summary>
        /// 设置上传分享界面显示隐藏
        /// </summary>
        /// <param name="isUpload"></param>
        /// <param name="isActive"></param>
        public void SetSharePanel(bool isUpload)
        {
            if (isUpload && VideoPanelUpLoadBtn)
                VideoPanelUpLoadBtn.gameObject.SetActive(isUpload);
            //if (VideoPanelDeleteBtn != null)
            //    VideoPanelDeleteBtn.gameObject.SetActive(isActive);
            //if (VideoPanelShareBtn != null)
            //    VideoPanelShareBtn.gameObject.SetActive(isActive);
            //if (VideoPanelSaveBtn != null)
            //    VideoPanelSaveBtn.gameObject.SetActive(isActive);
        }

        protected override void onClick(object obj)
        {
            base.onClick(obj);
            if(obj.Equals(VideoPanelDeleteBtn))
            {

            }
            else if(obj.Equals(VideoPanelShareBtn))
            {

            }
            else if (obj.Equals(VideoPanelSaveBtn))
            {

            }
            else if (obj.Equals(VideoPanelUpLoadBtn))
            {

            }

        }
    }
}
