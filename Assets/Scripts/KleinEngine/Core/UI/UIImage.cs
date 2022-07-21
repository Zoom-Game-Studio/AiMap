using UnityEngine;
using UnityEngine.UI;

namespace KleinEngine
{
    public class UIImage : Image
    {
        public void setImage(string abName,string imgname)
        {
            ResourceManager.GetInstance().loadAssetAsync<Sprite>(abName, imgname, onLoadComponent);
        }

        void onLoadComponent(object o)
        {
            Sprite sp = (Sprite)o;
            sprite = sp;
        }
    }
}
