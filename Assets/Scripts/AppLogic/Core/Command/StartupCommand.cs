using KleinEngine;
using System.IO;

namespace AppLogic
{
    public class StartupCommand : BaseCommand
    {
        public override void onExecute(object param)
        {
            registerCommand(ModuleEventType.APP_INIT, typeof(InitCommand));
            registerCommand(AppFacade.SHUTDOWN, typeof(ShutdownCommand));
            registerCommand(ModuleEventType.SCENE_ENTER, typeof(SceneEnterCommand));
            registerCommand(ModuleEventType.SCENE_AIMAP_POI, typeof(AIMapPOICommand));
            registerCommand(ModuleEventType.SCENE_BULLET_SCREEN, typeof(BulletScreenCommand));
            registerCommand(ModuleEventType.SCENE_VISIBLE_DISTANCE, typeof(VisibleDistanceCtrlCommand));
            registerCommand(ModuleEventType.SCENE_PARALLELVERSE, typeof(ParallelverseCommand));

            //registerCommand(ModuleEventType.LOADING_INIT, typeof(LoadingCommand));

            //registerCommand(ModuleEventType.NET_LOGIN, typeof(LoginCommand));
            //registerCommand(ModuleEventType.NET_LOAD_DATA, typeof(LoadDataCommand));

            //AppUtil.DYNAMIC_VIDEO_PATH = Path.Combine(GlobalObject.ASSET_PATH, "video");
            //if (!Directory.Exists(AppUtil.DYNAMIC_VIDEO_PATH))
            //    Directory.CreateDirectory(AppUtil.DYNAMIC_VIDEO_PATH);

            //if (AppUtil.IS_TEST)
            //    GlobalObject.ASSET_PATH = Path.Combine(GlobalObject.ASSET_PATH,"test");
            //else
            //    GlobalObject.ASSET_PATH = Path.Combine(GlobalObject.ASSET_PATH, "official");
            //if (!Directory.Exists(GlobalObject.ASSET_PATH)) Directory.CreateDirectory(GlobalObject.ASSET_PATH);

            //注册版本更新模块
            //AppFacade.GetInstance().registerMediator(new LoadingMediator());
            sendModuleEvent(ModuleEventType.APP_INIT);
            //sendModuleEvent(ModuleEventType.LOADING_SCENE, SCENE_NAME.AIMAPSCENE);
            //sendModuleEvent(ModuleEventType.SCENE_ENTER, "AIMapScene");


        }
    }
}
