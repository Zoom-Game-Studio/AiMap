using System.Collections;
using System.Collections.Generic;
using KleinEngine;
using UnityEngine;
using UnityEngine.UI;

namespace AppLogic
{
    public class RecordView : BaseView
    {

        [UISign]
        public TopPanel TopPanel;
        [UISign]
        public Button RecordBtn;
        [UISign]
        public Button PhotoBtn;
        [UISign]
        public Button SwitchGifBtn;
        [UISign]
        public Button SwitchPhotoBtn;
        [UISign]
        public Button SwitchVideoBtn;
        [UISign("SwitchVideoBtn/")]
        public Text SwitchVideoBtnText;
        [UISign("SwitchPhotoBtn/")]
        public Text SwitchPhotoBtnText;
        [UISign("SwitchGifBtn/")]
        public Text SwitchGifBtnText;

        [UISign("RecordBtn/")]
        public Image ImageFillAmount;
        [UISign("RecordBtn/")]
        public Text TextState;

        public bool isRecording = false;



        /// <summary>
        /// 设置录制按钮对应的录制状态
        /// </summary>
        /// <param name="state"></param>
        public void SetTextState(string state)
        {
            if (TextState != null)
            {
                TextState.text = state;
            }
        }

        /// <summary>
        /// 设置录制按钮图片为默认状态
        /// </summary>
        public void SetRecordBtnDefault()
        {
            Sprite sprite = ResourceManager.GetInstance().loadAsset<Sprite>(PATH_TYPE.ICON_RECORD, "default");
            if (sprite != null)
                SetRecordBtnImage(sprite);
        }

        /// <summary>
        /// 设置录制按钮图片为录制中
        /// </summary>
        public void SetRecordBtnRecording()
        {
            Sprite sprite = ResourceManager.GetInstance().loadAsset<Sprite>(PATH_TYPE.ICON_RECORD, "recording");
            if (sprite != null)
                SetRecordBtnImage(sprite);
        }

        /// <summary>
        /// 设置录制按钮的图片
        /// </summary>
        /// <param name="sprite"></param>
        public void SetRecordBtnImage(Sprite sprite)
        {
            if (RecordBtn.GetComponent<Image>() != null)
            {
                RecordBtn.GetComponent<Image>().sprite = sprite;
            }
        }

        /// <summary>
        /// 设置录制时候圆圈的值
        /// </summary>
        /// <param name="value"></param>
        public void SetImageFillAmount(float value)
        {
            if (ImageFillAmount == null)
            {
                Debug.Log("imageFillAmount is null");
                return;
            }
            if (ImageFillAmount != null)
                ImageFillAmount.fillAmount = value;
        }

        /// <summary>
        /// 重置录制时候的圆圈值归零
        /// </summary>
        public void ResetFillAmountImage()
        {
            Debug.Log("ResetFillAmount====");
            SetImageFillAmount(0);
            isRecording = false;
        }


        protected override void onClick(object obj)
        {
            base.onClick(obj);
            if (obj.Equals(RecordBtn))
            {
                Debug.Log("RecordBtn cliked");
            }
            else if (obj.Equals(PhotoBtn))
            {
                Debug.Log("PhotoBtn cliked");

            }
            else if (obj.Equals(SwitchGifBtn))
            {
                SwitchGifBtnText.color = Color.white ;
                SwitchPhotoBtnText.color = Color.gray;
                SwitchVideoBtnText.color = Color.gray;
                RecordBtn.gameObject.SetActive(true);
                PhotoBtn.gameObject.SetActive(false);
                Debug.Log("SwitchGifBtn cliked");

            }
            else if (obj.Equals(SwitchVideoBtn))
            {

                SwitchGifBtnText.color = Color.gray;
                SwitchPhotoBtnText.color = Color.gray;
                SwitchVideoBtnText.color = Color.white;
                RecordBtn.gameObject.SetActive(true);
                PhotoBtn.gameObject.SetActive(false);
                Debug.Log("SwitchVideoBtn cliked");

            }
            else if (obj.Equals(SwitchPhotoBtn))
            {
                SwitchGifBtnText.color = Color.gray;
                SwitchPhotoBtnText.color = Color.white;
                SwitchVideoBtnText.color = Color.gray;
                RecordBtn.gameObject.SetActive(false);
                PhotoBtn.gameObject.SetActive(true);
                Debug.Log("SwitchPhotoBtn cliked");
            }
        }
    }
}

