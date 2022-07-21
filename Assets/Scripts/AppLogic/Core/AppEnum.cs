namespace AppLogic
{
    public class ModuleEventType
    {
        public const string APP_INIT = "app_init";
        public const string LOADING_INIT = "loading_init";

        //模块控制
        public const string MODULE_ENTER = "module_enter";
        public const string MODULE_EXIT = "module_exit";
        public const string MODULE_REMOVE = "module_remove";
        public const string DOWNLOAD = "download";//下载模块
        public const string LOADING_COMPLETE = "loading_complete";

        //显示Loading
        public const string SET_LOADING_ANI = "set_loading_ani";

        //录制
        public const string START_VIDEO_RECORDING = "start_video_recording";
        public const string STOP_VIDEO_RECORING = "stop_video_recoring";

        //预览
        public const string START_PREVIEW_VIDEO = "start_preview_video";//开始预览视频播放
        public const string SET_PREVIEW_IMAGE = "set_preview_image";//设置预览模式下显示的图片

        public const string MODEL_ENABLE = "model_enable";//设置当前场景中模型的enable
        public const string MODEL_LOAD = "model_load";//模型加载
        public const string MODEL_PLAY = "model_play";//模型播放

        //加载部分
        public const string START_DOWNLOAD = "start_download";//开始下载
        public const string GET_AREA_ASSETS_FILES = "get_area_assets_files";
        public const string LOADING_AREA_ASSETS_DONE = "loading_area_assets_done";//预加载模型进内存完成
        public const string LOAD_AREA_ASSETS_FILES = "load_area_assets_files";//读取已经存在的资源文件
        public const string UNLOAD_UNITY_ASSETS = "unload_unity_assets";//卸载unity加载的资源

        public const string GET_POI_INFO_FROM_PHONE = "get_poi_info_from_phone";//获取POI信息
        public const string UPDATE_POI_INFO = "update_poi_info";//更新POI信息

        public const string GET_POI_POS_IN_MAP = "get_poi_pos_in_map";
        public const string GET_POI_POS_IN_UNITY = "get_poi_pos_in_unity";
        public const string SEND_POI_INFO_FROM_AIMAP_POI = "send_poi_info_from_aimap_poi";

        public const string GET_THREE_D_TEXT = "get_three_d_text";//获取3d字体
        public const string CREATE_A_THREE_D_TEXT = "create_a_three_d_text";//根据用户输入创建一个3d字体

        //文件下载部分
        public const string CHECK_ASSETS_LIST = "check_assets_list";//检测需要下载的文件列表
        public const string LOADING_SCENE = "loading_scene";

        ///UI隐藏
        public const string UI_HIDE = "ui_hide";
        //UI 部分
        public const string UI_SET_LOCALIZE_BACKBTN = "ui_set_localize_backbtn";
        public const string SET_LOCALIZE_STATUS_TEXT = "set_localize_status_text";
        public const string UI_SET_LOCALIZE_BTN_STATUS = "";
        public const string SHOW_POI_ICON = "show_poi_icon";
        public const string SCENE_BACK = "scene_back";

        //场景部分
        public const string SCENE_ENTER = "scene_enter";
        public const string SCENE_AIMAP_POI = "scene_aimap_poi";
        public const string SCENE_BULLET_SCREEN = "scene_bullet_screen";
        public const string SCENE_VISIBLE_DISTANCE = "scene_visible_distance";
        public const string SCENE_PARALLELVERSE = "scene_parallelverse";

        //通用效果
        public const string HIGHLIGHT_OBJ_ON_TRIGGER = "highlight_obj_on_trigger";
        public const string VISIBLE_DISTANCE_CHANGED = "visible_distance_changed";
        public const string VISIBLE_MODEL_CHANGE_BY_DISTANCE = "visible_model_change_by_distance";
        public const string SWITCH_OBJ_ON_TRIGGER = "switch_obj_on_trigger";
        public const string SWITCH_PARALLEL_UNIVERSE = "switch_parallel_universe";
        public const string SWITCH_PARALLEL_UNIVERSE_CALL_BACK = "switch_parallel_universe_call_back";

        //定位部分
        public const string FOUND_MAPS = "found_maps";
        public const string START_POSITION = "start_position";
        public const string SEND_CAMERA_INFO = "send_camera_info";
        public const string CLEAR_ALL_MODEL = "clear_all_model";
        public const string GET_POSITION_IN_UNITY = "get_position_in_unity";
        public const string GET_POSITION_IN_UNITY_CALL_BACK = "get_position_in_unity_call_back";
        public const string ENABLE_CONSISTENCY_CHECK = "enable_consistency_check";

        //场景部分-路径
        public const string GET_WAYPOINTS_LIST = "get_waypoints_list";
        public const string START_SUPERDEMO_CAR = "start_superdemo_car";
        public const string START_PLAY_AIMAPGROUP = "start_play_aimapgroup";
        public const string TALKING_TO_WORKER_IN_SUPERDEMO = "talking_to_worker_in_superdemo";
        public const string SUPERDEMO_GUIDE_TRIGGER = "superdemo_guide_trigger";
        public const string ZHEKEYUAN_ROUTE_TRIGGER = "zhekeyuan_route_trigger";

        //superdemo
        public const string SHOW_COFFE_SHOP_MENU = "show_coffe_shop_menu";
        public const string SUPERDEMO_COFFE_BUTTON_CTRL = "superdemo_coffe_button_ctrl";
        public const string SUPERDEMO_SHOW_BANK_MENU = "superdemo_show_bank_menu";


        //zhinengdao
        public const string ZHINENGDAO_MOVE_TO_NEXT = "zhinengdao_move_to_next";
        //fyxy
        public const string FYXY_MOVE_TO_NEXT = "fyxy_move_to_next";
        public const string FYXY_STOP_AUDIO = "fyxy_stop_audio";

        //9lnewyear
        public const string WAYZ9L_NEW_YEAR_GET_QUESTION = "wayz9l_new_year_get_question";        public const string WAYZ9L_NEW_YEAR_SEND_ANSWER = "wayz9l_new_year_send_answer";
        public const string WAYZ9L_NEW_YEAR_GET_RESULT = "wayz9l_new_year_get_result";

        //9l
        public const string WAYZ9L_PRESENTATION_START = "wayz9l_presentation_start";

        public const string SEND_MODEL_PREFAB_PARA = "send_model_prefab_para";
        public const string GET_MODEL_PREFAB_PARA_AGAIN = "get_model_prefab_para_again";
        public const string RETURN_MODEL_PARA_IN_SCENE = "return_model_para_in_scene";
        //加载模型部分
        public const string LOADING_MODELS = "loading_models";
        public const string LOADING_PREFABS = "loading_prefabs";
        public const string LOADING_ASSET = "loading_Asset";
        public const string HIDE_ALL_MODELS = "hide_all_models";

        //配置工具部分
        public const string CHANGE_MODEL_INFO = "change_model_info";
        public const string SAVE_MODEL_INFO = "save_model_info";
        public const string MODEL_PICKER = "model_picker";
    }

    public struct MEDIATOR
    {
        public const string CONTROL = "Control";
        public const string LOADING = "Loading";
        public const string RECORD = "Record";
        public const string TAKE_PHOTO = "Take_Photo";
        public const string MODEL_PLAY_CONTROLLER = "ModelPlayController";
        public const string MODEL_SCENE = "ModelScene";//模型加载模块
        public const string PREVIEW = "Preview";//预览模块
        public const string DOWNLOAD = "Download";//下载模块
        public const string MODE_SWITCH = "ModeSwitch";//模式选择
        public const string DATA_SWITCH = "DataSwitch";//城市治理数据选择
        public const string LOCALIZE = "Localize";//定位模块
        public const string AI_MAP_SCENE = "AIMapScene";//Map世界场景模块
        public const string LOCALIZATION_TOOL = "LocalizationTool";//本地定位辅助工具
    }

    public struct SCENE_NAME
    {
        public const string MAINSCENE = "MainScene";
        public const string AIMAPSCENE = "AIMapScene";
    }

    public struct PATH_TYPE
    {
        public const string UI = "ui/";
        public const string ICON_RECORD = "icon/record";
        public const string MODEL = "model/";
    }


    public struct MODE_TYPE
    {
        public const int VR = 1;
        public const int AR = 2;
        public const int AR_MATCH = 3;
    }

    public struct INFO_TYPE
    {
        public const int SCHEME = 1;
        public const int VIDEO = 2;
    }

    public enum ShootType
    {
        Video = 0,
        GIF = 1,
        Photo = 2
    }

    public enum MAININFO_WORKTYPE
    {
        /// <summary>
        /// 外观升级
        /// </summary>
        EXTERIOR_UPGRADE,
        /// <summary>
        /// 内饰升级
        /// </summary>
        INTERIOR_UPGRADE,
        /// <summary>
        /// 动力升级
        /// </summary>
        POWER_UPGRADE,
        /// <summary>
        /// 灯光影音升级
        /// </summary>
        LIGHTING_UPGRADE

    }
    public enum MAININFO_SHOPTYPE
    {
        /// <summary>
        /// 改装
        /// </summary>
        MODIFICATION,
        /// <summary>
        /// 维修保养
        /// </summary>
        REPAIR,
        /// <summary>
        /// 4S店
        /// </summary>
        FOUR_S,
        /// <summary>
        /// 商用车
        /// </summary>
        COMMERCIAL
    }

    public struct RECORD_ID
    {
        //public const string SCENE_SELECT = "场景选择";
    }

    public struct BUCKET_TYPE
    {
        public const string VIDEO = "app-video";
        public const string IMAGE = "app-video";
    }

    public enum ERROR_CODE
    {
        ///// <summary>
        ///// 未知错误
        ///// </summary>
        //UNKNOWN_ERROR = 100,

        ///// <summary>
        ///// 不符合规则
        ///// </summary>
        //RULE_NOT_ALLOW = 1000,

        ///// <summary>
        ///// 帐号已存在
        ///// </summary>
        //ACCOUNT_EXIST = 1001,

        ///// <summary>
        ///// 帐号不存在
        ///// </summary>
        //ACCOUNT_EXIST_NONE = 1002,
        ///// <summary>
        ///// 密码错误
        ///// </summary>
        //PASSWORD_ERROR = 1003,

        ///// <summary>
        ///// 已登录
        ///// </summary>
        //LOGINED = 1004, //

        //ROLE_EXIST = 1005,  //角色已创建
        //NAME_SAME = 1006,   //角色名称重复

        ///// <summary>
        ///// token无效
        ///// </summary>
        //TOKEN_INVALID = 1007,   //token无效(需要重新正常方式登录)
        //ONLINE_NONE = 1008, //不在线
    };
}
