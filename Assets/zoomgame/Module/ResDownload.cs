namespace Waku.Module
{
    public interface IResDownload<T>
    {
        /// <summary>
        /// 服务器资源列表
        /// </summary>
         T ServerList { get; set; }

        /// <summary>
        /// 本地资源列表
        /// </summary>
         T ClientList { get; set; }

        /// <summary>
        /// 需要下载的资源列表
        /// </summary>
         T DownloadList { get; set; }

        void Init();
    }
}