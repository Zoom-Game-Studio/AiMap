using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AppLogic;
using KleinEngine;




public struct LocModel
{

    public LocModel(double lat, double lng)
    {
        latitude = lat;
        longitude = lng;
    }
    /// <summary>
    /// 经度
    /// </summary>
    public double latitude;
    /// <summary>
    /// 纬度
    /// </summary>
    public double longitude;

    public string ConvertToString()
    {
        string str = longitude.ToString("f6") + "," + latitude.ToString("f6");
        return str;
    }

    public LocModel InverseLatAndLng()
    {
        LocModel loc = new LocModel(longitude, latitude);
        return loc;
    }

    public Vector3 ConvertToVector()
    {
        Vector3 vec = new Vector3((float)longitude, 0, (float)latitude);
        return vec;
    }


}
public class UnityUtilties : MonoBehaviour
{
    #region

    static public double __A = 6378.137; //椭球体长轴,千米
    static public double __B = 6356.752314; //椭球体短轴,千米
    static public double __B0 = 0; //标准纬度,弧度
    static public double __L0 = 0; //原点经度,弧度

    //角度到弧度的转换
    public static double DegreeToRad(double degree)
    {
        return Math.PI * degree / 180;
    }

    //弧度到角度的转换
    public static double RadToDegree(double rad)
    {
        return (180 * rad) / Math.PI;
    }

    //设定__A与__B
    public static void SetAB(double a, double b)
    {
        if (a <= 0 || b <= 0)
        {
            return;
        }
        __A = a;
        __B = b;
    }
    //设定__B0
    public static void SetLB0(double pmtL0, double pmtB0)
    {
        double l0 = DegreeToRad(pmtL0);
        if (l0 < -Math.PI || l0 > Math.PI)
        {
            return;
        }
        __L0 = l0;

        double b0 = DegreeToRad(pmtB0);
        if (b0 < -Math.PI / 2 || b0 > Math.PI / 2)
        {
            return;
        }
        __B0 = b0;
    }

    public class PointXY
    {
        public double x;
        public double y;
    }

    /// <summary>
    /// 经纬度转换xy坐标 返回直角坐标 单位:公里
    /// </summary>
    /// <param name="pmtLB0 参考点经纬度"></param>
    /// <param name="pmtLB1 要转换的经纬度"></param>
    /// <returns></returns>
    static public Vector3 LBToXY(LocModel pmtLB0, LocModel pmtLB1)
    {
        SetLB0(pmtLB0.longitude, pmtLB0.latitude);

        double B = DegreeToRad(pmtLB1.latitude);
        double L = DegreeToRad(pmtLB1.longitude);

        Vector3 xz = new Vector3();
        xz.x = 0; xz.y = 0; xz.z = 0;

        double f/*扁率*/, e/*第一偏心率*/, e_/*第二偏心率*/, NB0/*卯酉圈曲率半径*/, K, dtemp;
        double E = Math.Exp(1);
        if (L < -Math.PI || L > Math.PI || B < -Math.PI / 2 || B > Math.PI / 2)
        {
            return xz;
        }
        if (__A <= 0 || __B <= 0)
        {
            return xz;
        }
        f = (__A - __B) / __A;
        dtemp = 1 - (__B / __A) * (__B / __A);
        if (dtemp < 0)
        {
            return xz;
        }
        e = Math.Sqrt(dtemp);
        dtemp = (__A / __B) * (__A / __B) - 1;
        if (dtemp < 0)
        {
            return xz;
        }
        e_ = Math.Sqrt(dtemp);
        NB0 = ((__A * __A) / __B) / Math.Sqrt(1 + e_ * e_ * Math.Cos(__B0) * Math.Cos(__B0));
        K = NB0 * Math.Cos(__B0);
        xz.x = (float)(K * (L - __L0));
        xz.z = (float)(K * Math.Log(Math.Tan(Math.PI / 4 + (B) / 2) * Math.Pow((1 - e * Math.Sin(B)) / (1 + e * Math.Sin(B)), e / 2)));
        double y0 = K * Math.Log(Math.Tan(Math.PI / 4 + (__B0) / 2) * Math.Pow((1 - e * Math.Sin(__B0)) / (1 + e * Math.Sin(__B0)), e / 2));
        xz.z = (float)(xz.z - y0);

        //xz.z = -xz.z;//正常的Y坐标系（向上）转程序的Y坐标系（向下）

        return xz;
    }

    #endregion

    public static string Vector3ToString(Vector3 vec3)
    {
        return vec3.x + "," + vec3.y + "," + vec3.z;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vec3String, format x,y,z"></param>
    /// <returns></returns>
    public static Vector3 StringToVector3(string vec3String)
    {
        string[] vec3StringAry = vec3String.Split(',');
        Vector3 vector3 = new Vector3(float.Parse(vec3StringAry[0]), float.Parse(vec3StringAry[1]), float.Parse(vec3StringAry[2]));
        return vector3;
    }
    /// <summary>
    /// 读取方法
    /// 文件流方式
    /// </summary>
    public static string ReadTxtByFileName(string path)
    {
        //string path = Application.dataPath + "/model.txt";
        //string path = GlobalObject.DELTA_MODEL_INFO_PATH;
        FileStream files = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[files.Length];
        files.Read(bytes, 0, bytes.Length);
        files.Close();
        string str = UTF8Encoding.UTF8.GetString(bytes);
        return str;
        //print(str);     //第二种方法添加txt文本
        //第二种方法添加txt文本
    }

    /// <summary>
    /// 创建txt 方法一
    /// </summary>
    /// <param name="txtText"></param>
    public static bool AddTxtTextByFileStream(string txtText)
    {
        //string path = Application.dataPath + "/model.txt";
        string path = "";

        if (File.Exists(path))
            File.Delete(path);
        // 文件流创建一个文本文件
        FileStream file = new FileStream(path, FileMode.Create);
        //得到字符串的UTF8 数据流
        byte[] bts = System.Text.Encoding.UTF8.GetBytes(txtText);
        // 文件写入数据流
        file.Write(bts, 0, bts.Length);
        if (file != null)
        {
            //清空缓存
            file.Flush();
            // 关闭流
            file.Close();
            //销毁资源
            file.Dispose();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 创建txt 方法二
    /// </summary>
    /// <param name="txtText"></param>
    public void AddTxtTextByFileInfo(string txtText)
    {
        //string path = Application.dataPath + "/model.txt";
        string path = "";
        StreamWriter sw;
        FileInfo fi = new FileInfo(path);

        if (!File.Exists(path))
        {
            sw = fi.CreateText();
        }
        else
        {
            sw = fi.AppendText();   //在原文件后面追加内容      
        }
        sw.WriteLine(txtText);
        sw.Close();
        sw.Dispose();
    }

    /// <summary>
    /// Base64ToTexture2D
    /// </summary>
    /// <param name="Base64STR"></param>
    /// <returns></returns>
    public static Texture2D Base64ToTexture2D(string Base64STR)
    {
        Texture2D pic = new Texture2D(190, 190, TextureFormat.RGBA32, false);
        byte[] data = System.Convert.FromBase64String(Base64STR);
        pic.LoadImage(data);
        return pic;
    }
    /// <summary>
    /// Texture2DToBase64
    /// </summary>
    /// <param name="t2d"></param>
    /// <returns></returns>
    public static String Texture2DToBase64(Texture2D t2d)
    {
        byte[] bytesArr = t2d.EncodeToJPG();
        string strbaser64 = Convert.ToBase64String(bytesArr);
        return strbaser64;
    }

    /// <summary>
    /// 图片转换成base64编码文本
    /// </summary>
    public static void ImgToBase64String(string path, string recordBase64String)
    {
        try
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[fs.Length];
            fs.Read(buffer, 0, (int)fs.Length);
            string base64String = Convert.ToBase64String(buffer);
            Debug.Log("success base64:" + base64String);
            recordBase64String = base64String;
        }
        catch (Exception e)
        {
            Debug.Log("ImgToBase64String failed:" + e.Message);
        }
    }

    /// <summary>
    /// base64编码文本转换成图片
    /// </summary>
    public static void Base64ToImg(Image imgComponent, string recordBase64String)
    {
        string base64 = recordBase64String;
        byte[] bytes = Convert.FromBase64String(base64);
        Texture2D tex2D = new Texture2D(100, 100);
        tex2D.LoadImage(bytes);
        Sprite s = Sprite.Create(tex2D, new Rect(0, 0, tex2D.width, tex2D.height), new Vector2(0.5f, 0.5f));
        imgComponent.sprite = s;
        Resources.UnloadUnusedAssets();
    }

    /// <summary>
    /// Auths the get file data.
    /// </summary>
    /// <returns>The get file data.</returns>
    /// <param name="fileUrl">File URL.</param>
    public static byte[] AuthGetFileData(string fileUrl)
    {
        FileStream fs = new FileStream(fileUrl, FileMode.Open, FileAccess.Read);
        byte[] buffur = new byte[fs.Length];

        fs.Read(buffur, 0, buffur.Length);
        fs.Close();
        return buffur;
    }

    public static void SetMaterialRenderingQueue(Material material, int queue)
    {
        if (material == null) Debug.Log("SetMaterialRenderingQueue @ material is null");
        material.renderQueue = queue;
    }


    /// <summary>
    /// 存储texture2d到本地
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="filePath"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string SaveCaptureScreen(Texture2D texture, string filePath, string fileName)
    {
        byte[] bytes = texture.EncodeToPNG();
        //        string filename = Application.dataPath + "/ScreenShot.png";
        // string filename = Application.persistentDataPath + "/ScreenShot.png";
        string file = filePath + "/" + fileName;
        Debug.Log("save file to ====" + file);
        //currentPhotoPath = filename;
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }
        if (File.Exists(file))
        {
            File.Delete(file);
        }
        System.IO.File.WriteAllBytes(file, bytes);
        return file;
    }

    /// <summary>
    /// 针对不同的相机进行截图，可以把UI和游戏分开
    /// </summary>
    /// <param name="c"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    public static Texture2D CaptureScreen(Camera c, Rect r)
    {
        RenderTexture rt = new RenderTexture((int)r.width, (int)r.height, 3);

        c.targetTexture = rt;
        c.Render();

        RenderTexture.active = rt;
        Texture2D screenShot = new Texture2D((int)r.width, (int)r.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(r, 0, 0);
        screenShot.Apply();
        c.targetTexture = null;
        RenderTexture.active = null;
        GameObject.Destroy(rt);
        return screenShot;
    }

    /// <summary>
    /// 从路径读取audio 并播放
    /// </summary>
    /// <param name="modelAudio"></param>
    /// <param name="path"></param>
    /// <param name="audioType"></param>
    /// <returns></returns>
    public static IEnumerator GetAudioClip(AudioSource modelAudio, string path, AudioType audioType)
    {
        Debug.Log("GetAudioClip...");
        path = "file://" + path;
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
        {
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.LogError("uwrERROR:" + uwr.error);
            }
            else
            {
                Debug.Log("uwr url:" + uwr.url);
                AudioClip audioClip = DownloadHandlerAudioClip.GetContent(uwr);
                Debug.Log("Set audio clip");
                modelAudio.clip = audioClip;
            }
        }
        modelAudio.Play();
    }

    /// <summary>
    /// 根据子物体名称递归查找
    /// </summary>
    /// <param name="go"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static GameObject FindChild(Transform go, string childName)
    {
        if (null == go) return null;
        if (string.IsNullOrWhiteSpace(childName)) return null;
        Transform child = go.Find(childName);
        if (child != null)
            return child.gameObject;
        int count = go.transform.childCount;
        GameObject obj = null;
        for (int i = 0; i < count; i++)
        {
            child = go.GetChild(i);
            obj = FindChild(child, childName);
            if (obj != null)
                return obj;
        }
        return null;
    }

    /// <summary>
    /// 从URL中截取文件名
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetFileNameFromUrl(string url)
    {
        var splitArray = Regex.Split(url, "/", RegexOptions.IgnoreCase);
        string fileName = splitArray[splitArray.Length - 1];
        return fileName;
    }

    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns>The timestamp.</returns>
    /// <param name="d">D.</param>
    public static string GetTimestamp()
    {
        TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);

        string time = ts.TotalMilliseconds.ToString();
        Debug.Log(time);
        if (time.Contains("."))
            return time.Split(new string[] { "." }, StringSplitOptions.None)[0];
        else
            return time;
        //精确到毫秒
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <returns>The set texture size.</returns>
    /// <param name="tex">Tex.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public static Texture2D ReSetTextureSize(Texture2D tex, int width, int height)
    {
        var rendTex = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
        rendTex.Create();
        Graphics.SetRenderTarget(rendTex);
        GL.PushMatrix();
        GL.Clear(true, true, Color.clear);
        GL.PopMatrix();

        var mat = new Material(Shader.Find("Unlit/Transparent"));
        mat.mainTexture = tex;
        Graphics.SetRenderTarget(rendTex);
        GL.PushMatrix();
        GL.LoadOrtho();
        mat.SetPass(0);
        GL.Begin(GL.QUADS);
        GL.TexCoord2(0, 0);
        GL.Vertex3(0, 0, 0);
        GL.TexCoord2(0, 1);
        GL.Vertex3(0, 1, 0);
        GL.TexCoord2(1, 1);
        GL.Vertex3(1, 1, 0);
        GL.TexCoord2(1, 0);
        GL.Vertex3(1, 0, 0);
        GL.End();
        GL.PopMatrix();

        var finalTex = new Texture2D(rendTex.width, rendTex.height, TextureFormat.ARGB32, false);
        RenderTexture.active = rendTex;
        finalTex.ReadPixels(new Rect(0, 0, finalTex.width, finalTex.height), 0, 0);
        finalTex.Apply();
        return finalTex;
    }

    /// <summary>
    /// 给截图添加水印
    /// </summary>
    /// <returns>The logo.</returns>
    /// <param name="image">Image.</param>
    /// <param name="logo_">Logo.</param>
    public static Texture2D AddLogo(Texture2D image, Texture2D logo_)
    {
        Texture2D logoTexture = new Texture2D(image.width, image.height);

        Color[] colors = image.GetPixels();
        for (int i = 0; i < logo_.width; i++)
        {
            for (int j = 0; j < logo_.height; j++)
            {
                Color c = logo_.GetPixel(i, j);

                if (c.a != 0)
                {
                    colors[logoTexture.width * j + i] = c;
                }
            }
        }
        logoTexture.SetPixels(0, 0, image.width, image.height, colors);
        logoTexture.Apply();
        //Texture2D newTex= ReSetTextureSize(logoTexture, Screen.width, Screen.height);
        return logoTexture;
    }

    /// <summary>
    /// 从png转化为sprite
    /// </summary>
    /// <returns>The sprite form png.</returns>
    /// <param name="filePath">File path.</param>
    /// <param name="width">Width.</param>
    /// <param name="height">Height.</param>
    public static Sprite GetSpriteFormPng(string filePath, int width, int height)
    {
        if (filePath == null || filePath == String.Empty) return null;
        double startTime = (double)Time.time;
        //创建文件读取流
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        //释放文件读取流
        fileStream.Close();
        //释放本机屏幕资源
        fileStream.Dispose();
        fileStream = null;

        //创建Texture
        Texture2D texture = new Texture2D(width, height);
        texture.LoadImage(bytes);

        //创建Sprite
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        if (sprite == null) { Debug.Log("Sprite create failed"); return null; }
        return sprite;
    }

    //通过场景名称加载场景
    private void LoadSceneByName(string sceneName)
    {
        if (sceneName != null)
            StartCoroutine(AsyncLoading(sceneName));
        //SceneManager.LoadScene(sceneName);
    }

    AsyncOperation operation;
    int toProgress;
    int displayProgress;
    private IEnumerator AsyncLoading(string sceneName)
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        while (operation.progress < 0.9f)
        {
            toProgress = (int)operation.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            //SetLoadingPercentage(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        operation.allowSceneActivation = true;
    }
}
