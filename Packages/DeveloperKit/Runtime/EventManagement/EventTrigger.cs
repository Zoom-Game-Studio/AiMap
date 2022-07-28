namespace EventManagement
{
    /// <summary>
    /// 事件触发信号的枚举
    /// </summary>
    public enum EventTrigger
    {
        /// <summary>
        /// 连接photon
        /// </summary>
        ConnectPhoton = 0,

        /// <summary>
        /// 加入房间
        /// </summary>
        JoinPhotonRoom = 1,
        JoinPadRoom = 2,
        FailToJoinPadRoom = 3,


        /// <summary>
        /// 设定游戏进度为开始
        /// </summary>
        SetGameStart = 4,

        /// <summary>
        /// 开始炒菜
        /// </summary>
        StartFish = 5,
        
        /// <summary>
        /// 完成炒菜
        /// </summary>
        FinishFish = 6,
        
        /// <summary>
        /// 设定游戏运行数据已经开始
        /// </summary>
        SetCookHasStart = 7,
        HideTagAreaAndShowSlider = 8,
        
        /// <summary>
        /// 重新加入房间
        /// </summary>
        ReJoinRoom = 9,
        
        //debug

        DebugCheckHand = 10,
    }
}