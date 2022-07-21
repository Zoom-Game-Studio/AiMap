using KleinEngine;
//using Protocol;
using System.Collections.Generic;
using UnityEngine;

namespace AppLogic
{
    public class LoadDataCommand : BaseCommand
    {
        int loadCount = 3;

        public override void onExecute(object param)
        {
//#if !UNITY_STANDALONE
//            SDKManager.GetLocation();
//#endif
//            addModuleEvent(ModuleEventType.SCHEME_GET, handleSchemeGet);
//            addModuleEvent(ModuleEventType.VIDEO_GET, handleVideoGet);
//            addModuleEvent(ModuleEventType.PHOTO_GET, handlePhotoGet);

//            addModuleEvent(ModuleEventType.RANK_VIDEO_GET, handleRankVideoGet);
//            addModuleEvent(ModuleEventType.RANK_NEW_SCHEME_GET, handleRankNewSchemeGet);

//            //加载首页信息
//            getProxy<RankProxy>().sendRankGet();

//            //加载3D方案信息
//            getProxy<CarProxy>().sendSchemeGet();

//            //加载视频信息
//            getProxy<VideoProxy>().sendVideoGet();

//            //加载图册信息
//            getProxy<PhotoProxy>().sendPhotoGet();
        }

        void handleSchemeGet(EventObject ev)
        {
            
        }

        void handleVideoGet(EventObject ev)
        {
          
        }

        void handlePhotoGet(EventObject ev)
        {
           
        }

        void handleLoadDataComplete()
        {
            
        }

        void handleRankVideoGet(EventObject ev)
        {
           
        }

        void handleRankNewSchemeGet(EventObject ev)
        {
           
        }
    }
}
