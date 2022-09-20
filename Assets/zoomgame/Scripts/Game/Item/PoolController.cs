using System;
using System.Collections;
using System.Collections.Generic;

using BestHTTP;

using QFramework;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



[MonoSingletonPath("[Ztag]/CoroutineController")]
public class PoolController : MonoBehaviour,ISingleton
{
    /// <summary>
    /// 资源缓存池
    /// </summary>
    public static PoolController Instance => MonoSingletonProperty<PoolController>.Instance;

    [Header("缓存资源 藏品backpackInfo.cover(URL)作为key")]
    [Header("图片缓存")] private Dictionary<string, Sprite> imgCashDict = new Dictionary<string, Sprite>();
    [Header("图片TEXTURE缓存")] private Dictionary<string, Texture> textureCashDict = new Dictionary<string, Texture>();




    #region Public

    public void OnSingletonInit()
    {

    }

   

    /// <summary>
    /// 直接设置image sprite
    /// </summary>
    /// <param name="url"></param>
    /// <param name="image"></param>
    public void SetImage(string url, Image image)
    {
       
        FindImg(url, sprite =>
        {
            if (image != null)
                image.sprite = sprite;
        });
    }


    /// <summary>
    /// 取图片藏品的图片
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public void FindImg(string url, Action<Sprite> action,Action<Texture> actTex = null)
    {
        if (string.IsNullOrEmpty(url))
        {
            Debug.LogError("链接为空");
            return;
        } 
        if (imgCashDict.ContainsKey(url))
        {
            Sprite sprite = imgCashDict[url];
            Texture texture = textureCashDict[url];
            action(sprite);
            actTex?.Invoke(texture);
        }
        else
        {
            LoadImgByResloader(url, (spr) =>
            {
                action(spr);
                imgCashDict[url] = spr;
            },(texture)=> {
                actTex?.Invoke(texture);
                textureCashDict[url] = texture;
            });
        }
    }

   

  
    

    #endregion

    #region 资源加载

    void LoadImgByResloader(string url, Action<Sprite> action = null, Action<Texture> actTex = null)
    {
        ResLoader mResLoader = ResLoader.Allocate();
        mResLoader.Add2Load<Texture2D>(
               url.ToNetImageResName(),
                (b, res) =>
                {
                    if (b)
                    {
                        var texture = res.Asset as Texture2D;

                        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                            Vector2.one * 0.5f);
                        actTex?.Invoke(texture);
                        action(sprite);
                        mResLoader.AddObjectForDestroyWhenRecycle2Cache(sprite);
                    }
                });

        mResLoader.LoadAsync();
    }
    /// <summary>
    /// 通过url加载图片
    /// </summary>
    void LoadImgByReskit(string url, Action<Sprite> action = null)
    {
        //创建request
        var loadImgByReskitRequest = new HTTPRequest(new Uri(url), HTTPMethods.Get);
        //不缓存
        loadImgByReskitRequest.DisableCache = false;

        //设置头
        loadImgByReskitRequest.SetHeader("Content-Type", "application/json");

        //回调
        loadImgByReskitRequest.Callback = (req, response) =>
        {
            if (response == null)
            {
                    // HintController.Instance.ShowHint(HintType.ERROR, "网络超时");
                    ("请求图片超时" + url).LogInfo();
                return;
            }

            var texture2D = response.DataAsTexture2D;
            var sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.one * 0.5f);

            action.Invoke(sprite);
        };

        //发送请求
        loadImgByReskitRequest.Send();
    }

    

    #endregion


}
