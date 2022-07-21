using UnityEngine.UI;
using UnityEngine;

namespace KleinEngine
{
    public class ImageItemComponent : ItemComponent
    {
        [UISign]
        public Button selectBtn;
        [UISign]
        public Image selectImg;

        public void setImage(Sprite sp)
        {
            selectBtn.image.sprite = sp;
        }

        public override void setSelect(bool flag)
        {
            selectImg.enabled = flag;
        }
    }
}